using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using LawyerProject.BLL;
using LawyerProject.DB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quedin.IntegrationTests
{
    /// <summary>
    /// End-to-end tests of the data layer against the real QuedinDB database
    /// (create it first with database/create-database.sql).
    /// Every test runs inside a transaction that is rolled back, so the demo data never changes.
    /// When the database is not reachable the tests are reported as inconclusive, not failed.
    /// </summary>
    [TestClass]
    public class DataLayerTests
    {
        private TransactionScope scope;
        private QuedinDbContext db;

        [TestInitialize]
        public void Setup()
        {
            scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
            db = new QuedinDbContext();
            try
            {
                if (!db.Users.Any()) Assert.Inconclusive("QuedinDB has no users - run database/create-database.sql.");
            }
            catch (Exception ex) when (!(ex is UnitTestAssertException))
            {
                Assert.Inconclusive("QuedinDB is not reachable: " + ex.GetBaseException().Message);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            Session.SignOut();
            if (db != null) db.Dispose();
            if (scope != null) scope.Dispose(); // no Complete() = rollback
        }

        // ------------------------------------------------------------------ auth

        [TestMethod]
        public void Login_DemoUsers()
        {
            var auth = new AuthService(db);
            User admin = auth.Login("admin", "Quedin2024");

            Assert.IsNotNull(admin);
            Assert.AreEqual(Roles.Manager, admin.Role);
            Assert.IsNotNull(auth.Login("worker", "Worker2024"));
            Assert.IsNull(auth.Login("admin", "wrong-password"));
            Assert.IsNull(auth.Login("nobody", "Quedin2024"));
        }

        [TestMethod]
        public void Login_UpgradesLegacyPlainTextPassword()
        {
            db.Users.Add(new User { UserName = "legacy", PasswordHash = "12345", Role = Roles.Worker, IsActive = true });
            db.SaveChanges();

            User user = new AuthService(db).Login("legacy", "12345");

            Assert.IsNotNull(user);
            string stored = db.Users.AsNoTracking().Single(u => u.UserName == "legacy").PasswordHash;
            Assert.IsTrue(stored.StartsWith("pbkdf2$"), "password should be re-stored as a hash");
            Assert.IsNotNull(new AuthService(db).Login("legacy", "12345"), "login still works after the upgrade");
        }

        [TestMethod]
        public void AddUser_RequiresManager_AndStoresHash()
        {
            var auth = new AuthService(db);
            Assert.IsNotNull(auth.AddUser("new.user", "Secret2024", Roles.Worker), "a non-manager must be refused");

            Session.SignIn(auth.Login("admin", "Quedin2024"));
            Assert.IsNull(auth.AddUser("new.user", "Secret2024", Roles.Worker));
            Assert.IsNotNull(auth.AddUser("new.user", "Secret2024", Roles.Worker), "duplicate user name must be refused");

            User created = db.Users.Single(u => u.UserName == "new.user");
            Assert.AreNotEqual("Secret2024", created.PasswordHash);
            Assert.IsNotNull(auth.Login("new.user", "Secret2024"));
        }

        [TestMethod]
        public void ChangePassword_RequiresCurrentPassword()
        {
            var auth = new AuthService(db);
            Assert.IsNotNull(auth.ChangePassword("worker", "bad", "NewPass2024", "NewPass2024"));
            Assert.IsNull(auth.ChangePassword("worker", "Worker2024", "NewPass2024", "NewPass2024"));
            Assert.IsNotNull(auth.Login("worker", "NewPass2024"));
            Assert.IsNull(auth.Login("worker", "Worker2024"));
        }

        // ------------------------------------------------------------------ cases

        private LegalCase NewCourtCase(int caseTypeId)
        {
            Customer client = db.Customers.First(c => c.PersonType == PersonTypes.Client);
            Customer opposing = db.Customers.First(c => c.PersonType == PersonTypes.OpposingClient);
            Customer lawyer = db.Customers.First(c => c.PersonType == PersonTypes.OpposingLawyer);
            return new LegalCase
            {
                ClientId = client.CustomerId,
                CaseTypeId = caseTypeId,
                IsInCourt = true,
                CourtId = db.Courts.First().CourtId,
                PowerOfAttorney = "TEST",
                ClaimAmount = 10000m,
                FeePercent = 10,
                Advance = 0m,
                OpposingPartyId = opposing.CustomerId,
                OpposingLawyerId = lawyer.CustomerId
            };
        }

        [TestMethod]
        public void OpenCase_CreatesTimelineFromCaseType()
        {
            CaseType type = db.CaseTypes.First(s => s.Steps.Any());
            int expectedSteps = type.Steps.Count;
            var service = new CaseService(db);
            LegalCase legalCase = NewCourtCase(type.CaseTypeId);
            DateTime opened = new DateTime(2025, 1, 1);

            Assert.IsNull(service.CreatePlan(legalCase, opened));
            db.Cases.Add(legalCase);
            db.SaveChanges();

            var saved = db.Cases.Single(t => t.CaseId == legalCase.CaseId);
            var steps = CaseService.OrderedSteps(saved);
            CaseTypeStep first = type.Steps.OrderBy(k => k.StepOrder).First();

            Assert.AreEqual(expectedSteps, steps.Count);
            Assert.AreEqual(first.ProcedureId, saved.CurrentProcedureId);
            Assert.AreEqual(BusinessCalendar.NextWorkday(opened.AddDays(db.Procedures.Find(first.ProcedureId).DurationDays)), steps[0].DueDate);
            Assert.IsTrue(steps.All(s => s.DurationDays.HasValue), "durations are kept with the case");
            Assert.IsTrue(steps.All(s => s.IsDone == false && s.DoneDate == null));
        }

        [TestMethod]
        public void OpenCase_WithoutFixedSteps_IsRefused()
        {
            var empty = new CaseType { Name = "TEST-EMPTY", BaseFee = 0m, CourtFee = 0m };
            db.CaseTypes.Add(empty);
            db.SaveChanges();

            string error = new CaseService(db).CreatePlan(NewCourtCase(empty.CaseTypeId), DateTime.Today);

            Assert.IsNotNull(error);
        }

        [TestMethod]
        public void CompleteStep_PushesFollowingDatesInDatabase()
        {
            CaseType type = db.CaseTypes.First(s => s.Steps.Count >= 3);
            var service = new CaseService(db);
            LegalCase legalCase = NewCourtCase(type.CaseTypeId);
            service.CreatePlan(legalCase, new DateTime(2025, 1, 1));
            db.Cases.Add(legalCase);
            db.SaveChanges();

            var steps = CaseService.OrderedSteps(legalCase);
            DateTime doneOn = new DateTime(2025, 2, 1);
            int secondDuration = db.Procedures.Find(steps[1].ProcedureId).DurationDays;

            Assert.IsNotNull(service.CompleteStep(legalCase, steps[1].CaseStepId, doneOn), "skipping must be refused");
            Assert.IsNull(service.CompleteStep(legalCase, steps[0].CaseStepId, doneOn));

            using (var fresh = new QuedinDbContext())
            {
                var reloaded = CaseService.OrderedSteps(fresh.Cases.Single(t => t.CaseId == legalCase.CaseId));
                Assert.AreEqual(true, reloaded[0].IsDone);
                Assert.AreEqual(doneOn, reloaded[0].DoneDate);
                Assert.AreEqual(BusinessCalendar.NextWorkday(doneOn.AddDays(secondDuration)), reloaded[1].DueDate);
                Assert.AreEqual(reloaded[1].ProcedureId, fresh.Cases.Single(t => t.CaseId == legalCase.CaseId).CurrentProcedureId);
            }
        }

        // ------------------------------------------------------------------ money

        [TestMethod]
        public void OpenBalance_DemoCases()
        {
            // Demo case 1: 85,000 x 20% + 1,200 court fee = 18,200; paid 3,000 + 5,000.
            Assert.AreEqual(10200m, CaseService.OpenBalance(db.Cases.Single(t => t.CaseId == 1)));
            // Demo case 2: 42,000 x 25% + 0 = 10,500; no payments yet.
            Assert.AreEqual(10500m, CaseService.OpenBalance(db.Cases.Single(t => t.CaseId == 2)));
            // Demo case 5 is fully paid.
            Assert.AreEqual(0m, CaseService.OpenBalance(db.Cases.Single(t => t.CaseId == 5)));
        }

        [TestMethod]
        public void DemoData_StatusMatchesNextPendingStep()
        {
            foreach (var legalCase in db.Cases.ToList())
            {
                var next = CaseService.OrderedSteps(legalCase).FirstOrDefault(s => s.IsDone != true);
                if (next != null) Assert.AreEqual(next.ProcedureId, legalCase.CurrentProcedureId, "case " + legalCase.CaseId);
            }
        }

        // ------------------------------------------------------------------ v1.2 rules

        private LegalCase SavedCase()
        {
            CaseType type = db.CaseTypes.First(s => s.Steps.Count >= 2);
            LegalCase legalCase = NewCourtCase(type.CaseTypeId);
            legalCase.ClaimAmount = 10000m;
            legalCase.FeePercent = 10;
            new CaseService(db).CreatePlan(legalCase, new DateTime(2025, 1, 5));
            db.Cases.Add(legalCase);
            db.SaveChanges();
            return legalCase;
        }

        [TestMethod]
        public void Payment_StoresVatAndBalance_AndCancellationAddsReversal()
        {
            LegalCase legalCase = SavedCase();
            var service = new CaseService(db);
            decimal total = CaseService.CaseTotal(legalCase);

            var payment = new Payment { Amount = 500m, Kind = PaymentKinds.Fee, Method = "מזומן", PaidOn = DateTime.Today, Description = "מקדמה" };
            Assert.IsNull(service.AddPayment(legalCase, payment));
            Assert.AreEqual(total - 500m, payment.BalanceAfter);
            Assert.AreEqual(AppSettings.VatRate, payment.VatRate);

            string overpaid = service.AddPayment(legalCase, new Payment { Amount = total, Kind = PaymentKinds.Fee, Method = PaymentMethods.Cash, PaidOn = DateTime.Today });
            StringAssert.Contains(overpaid, "גבוה מהיתרה", "overpayment refused");
            Assert.IsNull(service.CancelPayment(legalCase, payment.PaymentId, "נרשם בטעות"));
            Assert.IsNotNull(service.CancelPayment(legalCase, payment.PaymentId, null), "cannot cancel twice");

            using (var fresh = new QuedinDbContext())
            {
                var t = fresh.Cases.Single(x => x.CaseId == legalCase.CaseId);
                Assert.AreEqual(2, t.Payments.Count);
                Assert.IsTrue(t.Payments.Single(p => p.PaymentId == payment.PaymentId).IsCancelled);
                Assert.AreEqual(-500m, t.Payments.Single(p => p.CancelsPaymentId == payment.PaymentId).Amount);
                Assert.AreEqual(total, CaseService.OpenBalance(t));
                Assert.IsTrue(fresh.AuditLog.Any(y => y.Action == AuditActions.PaymentCancelled && y.EntityId == payment.PaymentId.ToString()));
            }
        }

        [TestMethod]
        public void FullPayment_SetsPaidFlag()
        {
            LegalCase legalCase = SavedCase();
            decimal total = CaseService.CaseTotal(legalCase);
            Assert.IsNull(new CaseService(db).AddPayment(legalCase, new Payment { Amount = total, Kind = PaymentKinds.Fee, Method = PaymentMethods.Cash, PaidOn = DateTime.Today }));
            Assert.IsTrue(legalCase.IsPaid);
            Assert.AreEqual(0m, CaseService.OpenBalance(legalCase));
        }

        [TestMethod]
        public void ConcurrentEdit_OfTheSameCase_IsDetected()
        {
            LegalCase legalCase = SavedCase();
            using (var other = new QuedinDbContext())
            {
                var copy = other.Cases.Single(t => t.CaseId == legalCase.CaseId);
                copy.PropertyAddress = "שונה על ידי משתמש אחר";
                other.SaveChanges();
            }
            legalCase.PropertyAddress = "השינוי שלי";
            Assert.ThrowsException<System.Data.Entity.Infrastructure.DbUpdateConcurrencyException>(() => db.SaveChanges());
        }

        [TestMethod]
        public void RescheduleAndUndo_UpdateTheDatabase()
        {
            LegalCase legalCase = SavedCase();
            var service = new CaseService(db);
            var steps = CaseService.OrderedSteps(legalCase);
            DateTime hearing = new DateTime(2025, 6, 3);

            Assert.IsNull(service.RescheduleStep(legalCase, steps[1].CaseStepId, hearing, "נקבע ע\"י בית המשפט"));
            Assert.AreEqual(hearing, steps[1].DueDate);
            Assert.AreEqual("נקבע ע\"י בית המשפט", steps[1].Notes);

            Assert.IsNull(service.CompleteStep(legalCase, steps[0].CaseStepId, new DateTime(2025, 1, 20)));
            Session.SignOut();
            Assert.IsNotNull(service.UndoLastStep(legalCase), "only a manager may undo");
            Session.SignIn(new User { UserName = "test-manager", Role = Roles.Manager, IsActive = true });
            try
            {
                Assert.IsNull(service.UndoLastStep(legalCase));
                Assert.AreEqual(false, steps[0].IsDone);
            }
            finally
            {
                Session.SignOut();
            }
        }

        [TestMethod]
        public void DeleteCase_OnlyWhenNothingHappenedYet()
        {
            Session.SignIn(new User { UserName = "test-manager", Role = Roles.Manager, IsActive = true });
            try
            {
                LegalCase paid = SavedCase();
                var service = new CaseService(db);
                service.AddPayment(paid, new Payment { Amount = 1m, Kind = PaymentKinds.Fee, Method = PaymentMethods.Cash, PaidOn = DateTime.Today });
                Assert.IsNotNull(service.DeleteCase(paid));

                LegalCase mistake = SavedCase();
                int id = mistake.CaseId;
                Assert.IsNull(service.DeleteCase(mistake));
                Assert.IsFalse(db.Cases.Any(t => t.CaseId == id));
            }
            finally
            {
                Session.SignOut();
            }
        }

        [TestMethod]
        public void DueSteps_ReturnsOneLinePerOpenCase()
        {
            var due = new CaseService(db).DueSteps(new DateTime(2030, 1, 1), 7);
            int openCases = db.Cases.Count(c => c.Steps.Any(s => s.IsDone != true));
            Assert.AreEqual(openCases, due.Count, "in 2030 every open case is late, so each appears once");
            Assert.AreEqual(due.Count, due.Select(d => d.CaseId).Distinct().Count());
        }

        // ------------------------------------------------------------------ edge cases from the report

        [TestMethod]
        public void FixedCourtDate_SurvivesCompletingAnEarlierStep()
        {
            LegalCase legalCase = SavedCase();
            var service = new CaseService(db);
            var steps = CaseService.OrderedSteps(legalCase);
            DateTime hearing = new DateTime(2025, 7, 1);

            Assert.IsNull(service.RescheduleStep(legalCase, steps[1].CaseStepId, hearing, null));
            Assert.IsNull(service.CompleteStep(legalCase, steps[0].CaseStepId, new DateTime(2025, 1, 30)));

            int fixedStepId = steps[1].CaseStepId;
            using (var fresh = new QuedinDbContext())
            {
                CaseStep stored = fresh.CaseSteps.Single(s => s.CaseStepId == fixedStepId);
                Assert.AreEqual(hearing, stored.DueDate);
                Assert.IsTrue(stored.IsFixedDate);
            }
        }

        [TestMethod]
        public void Refund_ReturnsTheOverpayment()
        {
            LegalCase legalCase = SavedCase();
            legalCase.ClaimAmount = 1000000m; // 10% = 100,000: well above any minimum fee, before and after halving
            db.SaveChanges();
            var service = new CaseService(db);
            decimal total = CaseService.CaseTotal(legalCase);
            Assert.IsNull(service.AddPayment(legalCase, new Payment { Amount = total, Kind = PaymentKinds.Fee, Method = PaymentMethods.Cash, PaidOn = DateTime.Today }));

            legalCase.ClaimAmount = legalCase.ClaimAmount / 2; // claim lowered after the client paid
            db.SaveChanges();
            Assert.IsTrue(CaseService.OpenBalance(legalCase) < 0);

            Assert.IsNull(service.RefundOverpayment(legalCase, PaymentMethods.BankTransfer, null));
            Assert.AreEqual(0m, CaseService.OpenBalance(legalCase));
            Assert.IsNotNull(service.RefundOverpayment(legalCase, PaymentMethods.BankTransfer, null), "nothing left to refund");
        }

        [TestMethod]
        public void DeletedCase_IsDetected()
        {
            Session.SignIn(new User { UserName = "test-manager", Role = Roles.Manager, IsActive = true });
            try
            {
                LegalCase legalCase = SavedCase();
                int id = legalCase.CaseId;
                Assert.IsTrue(new CaseService(db).CaseExists(id));
                Assert.IsNotNull(new CaseService(db).CurrentVersion(id));
                Assert.IsNull(new CaseService(db).DeleteCase(legalCase));
                Assert.IsFalse(new CaseService(db).CaseExists(id));
                Assert.IsNull(new CaseService(db).CurrentVersion(id));
            }
            finally
            {
                Session.SignOut();
            }
        }

        [TestMethod]
        public void DuplicateOpenCase_IsFound()
        {
            LegalCase first = SavedCase();
            LegalCase second = SavedCase(); // same client and case type
            LegalCase found = new CaseService(db).FindOpenDuplicate(second.ClientId, second.CaseTypeId, second.CaseId);
            Assert.IsNotNull(found);
            Assert.AreNotEqual(second.CaseId, found.CaseId, "the case itself is not its own duplicate");
        }

        [TestMethod]
        public void ConcurrentEdit_OfTheSameCustomer_IsDetected()
        {
            Customer customer = db.Customers.First(c => c.PersonType == PersonTypes.Client);
            using (var other = new QuedinDbContext())
            {
                other.Customers.Single(c => c.CustomerId == customer.CustomerId).City = "חיפה";
                other.SaveChanges();
            }
            customer.City = "אילת";
            Assert.ThrowsException<System.Data.Entity.Infrastructure.DbUpdateConcurrencyException>(() => db.SaveChanges());
        }

        [TestMethod]
        public void Login_LocksAfterRepeatedFailures_AndResetReleases()
        {
            var auth = new AuthService(db);
            string error;
            for (int i = 0; i < LoginPolicy.MaxFailures; i++) auth.Login("worker", "wrong", out error);

            Assert.IsNull(auth.Login("worker", "Worker2024", out error), "locked even with the right password");
            StringAssert.Contains(error, "נעול");

            Session.SignIn(auth.Login("admin", "Quedin2024"));
            try
            {
                Assert.IsNull(auth.ResetPassword("worker", "Fresh2024x"));
                Assert.IsNotNull(auth.Login("worker", "Fresh2024x"));
            }
            finally
            {
                Session.SignOut();
            }
        }

        [TestMethod]
        public void DeactivatedUser_CannotLogIn()
        {
            var auth = new AuthService(db);
            Session.SignIn(auth.Login("admin", "Quedin2024"));
            try
            {
                Assert.IsNotNull(auth.SetActive("admin", false), "a manager cannot deactivate themselves");
                Assert.IsNull(auth.SetActive("worker", false));
                string error;
                Assert.IsNull(auth.Login("worker", "Worker2024", out error));
                StringAssert.Contains(error, "הושבת");
            }
            finally
            {
                Session.SignOut();
            }
        }

        [TestMethod]
        public void Settings_AreSharedThroughTheDatabase()
        {
            Session.SignIn(new User { UserName = "test-manager", Role = Roles.Manager, IsActive = true });
            try
            {
                Assert.IsNotNull(AppSettings.Save(db, new Dictionary<string, string> { { AppSettings.VatRateKey, "1.5" } }), "invalid rate refused");
                Assert.IsNull(AppSettings.Save(db, new Dictionary<string, string> { { AppSettings.VatRateKey, "0.17" } }));
                using (var fresh = new QuedinDbContext())
                {
                    AppSettings.Load(fresh);
                    Assert.AreEqual(0.17m, AppSettings.VatRate);
                }
            }
            finally
            {
                Session.SignOut();
                AppSettings.Load(db);
            }
        }

        [TestMethod]
        public void AuditLog_IsAppendOnly()
        {
            AuditService.Log(db, "בדיקה", "בדיקה", 1, null);
            db.SaveChanges();
            bool refused = false;
            try
            {
                db.AuditLog.Remove(db.AuditLog.OrderByDescending(a => a.AuditEntryId).First());
                db.SaveChanges();
            }
            catch (Exception)
            {
                refused = true;
            }
            Assert.IsTrue(refused, "deleting from the audit log must fail");
        }

        // ------------------------------------------------------------------ v1.3.1: code review findings

        [TestMethod]
        public void SaveCase_RetryAfterAFailedSave_DoesNotDuplicateTheTimeline()
        {
            CaseType type = db.CaseTypes.First(s => s.Steps.Count >= 2);
            int expectedSteps = type.Steps.Count;
            var service = new CaseService(db);
            LegalCase legalCase = NewCourtCase(type.CaseTypeId);
            string realLawyer = legalCase.OpposingLawyerId;
            legalCase.OpposingLawyerId = "NO-SUCH-ID"; // breaks the foreign key: the save fails in the database

            Assert.ThrowsException<System.Data.Entity.Infrastructure.DbUpdateException>(
                () => service.SaveCase(legalCase, type, true, new DateTime(2025, 1, 1)));
            Assert.AreEqual(0, legalCase.Steps.Count, "the failed attempt left nothing behind");

            legalCase.OpposingLawyerId = realLawyer;
            Assert.IsNull(service.SaveCase(legalCase, type, true, new DateTime(2025, 1, 1)));

            using (var fresh = new QuedinDbContext())
                Assert.AreEqual(expectedSteps, fresh.CaseSteps.Count(s => s.CaseId == legalCase.CaseId));
        }

        [TestMethod]
        public void SaveCase_AuditEntryCarriesTheNewCaseNumber()
        {
            CaseType type = db.CaseTypes.First(s => s.Steps.Any());
            LegalCase legalCase = NewCourtCase(type.CaseTypeId);
            Assert.IsNull(new CaseService(db).SaveCase(legalCase, type, true, DateTime.Today));
            string id = legalCase.CaseId.ToString();
            Assert.IsTrue(db.AuditLog.Any(a => a.Action == AuditActions.CaseOpened && a.EntityId == id));
        }

        [TestMethod]
        public void SaveCase_RulesAreCheckedInTheServiceToo()
        {
            CaseType type = db.CaseTypes.First(s => s.Steps.Any());
            LegalCase legalCase = NewCourtCase(type.CaseTypeId);
            legalCase.OpposingPartyId = legalCase.ClientId;
            Assert.IsNotNull(new CaseService(db).SaveCase(legalCase, type, true, DateTime.Today), "client cannot be their own opponent");

            legalCase = NewCourtCase(type.CaseTypeId);
            legalCase.ClaimAmount = Validation.MaxMoney + 1m;
            Assert.IsNotNull(new CaseService(db).SaveCase(legalCase, type, true, DateTime.Today), "amount above the limit");
        }

        [TestMethod]
        public void AddPayment_FailedSave_IsNotWrittenByTheNextSave()
        {
            LegalCase legalCase = SavedCase();
            var service = new CaseService(db);
            var tooLong = new Payment { Amount = 100m, Kind = PaymentKinds.Fee, Method = PaymentMethods.Cash, PaidOn = DateTime.Today,
                                        Description = new string('x', 60) }; // longer than the column: the save fails

            Assert.ThrowsException<System.Data.Entity.Validation.DbEntityValidationException>(() => service.AddPayment(legalCase, tooLong));
            Assert.AreEqual(0, legalCase.Payments.Count, "the failed payment was discarded");

            Assert.IsNull(service.AddPayment(legalCase, new Payment { Amount = 100m, Kind = PaymentKinds.Fee, Method = PaymentMethods.Cash, PaidOn = DateTime.Today }));
            using (var fresh = new QuedinDbContext())
                Assert.AreEqual(1, fresh.Payments.Count(p => p.CaseId == legalCase.CaseId), "exactly one payment, not two");
        }

        [TestMethod]
        public void Catalogue_ChangesNeedAManager()
        {
            db.Courts.Add(new Court { Name = "TEST-COURT" });
            Assert.ThrowsException<UnauthorizedAccessException>(() => CatalogService.Save(db, "בית משפט", "TEST"));

            Session.SignIn(new User { UserName = "test-manager", Role = Roles.Manager, IsActive = true });
            CatalogService.Save(db, "בית משפט", "TEST");
            Assert.IsTrue(db.Courts.Any(c => c.Name == "TEST-COURT"));
            Assert.IsTrue(db.AuditLog.Any(a => a.Action == AuditActions.CatalogChanged && a.Details == "TEST"));
        }

        [TestMethod]
        public void Payments_CannotBeEditedInTheDatabase()
        {
            var ex = Assert.ThrowsException<System.Data.SqlClient.SqlException>(
                () => db.Database.ExecuteSqlCommand("UPDATE dbo.Payments SET Amount = Amount + 1 WHERE PaymentId = 1"));
            StringAssert.Contains(ex.Message, "cannot be edited");
        }

        [TestMethod]
        public void LoginSalt_DoesNotRevealWhichUsersExist()
        {
            string known = db.Database.SqlQuery<string>("EXEC dbo.usp_GetLoginSalt @p0", "admin").Single();
            string unknown = db.Database.SqlQuery<string>("EXEC dbo.usp_GetLoginSalt @p0", "nobody-here").Single();
            StringAssert.StartsWith(known, "pbkdf2$");
            StringAssert.StartsWith(unknown, "pbkdf2$");
            Assert.AreEqual(unknown, db.Database.SqlQuery<string>("EXEC dbo.usp_GetLoginSalt @p0", "nobody-here").Single(), "stable");
            Assert.IsFalse(db.Database.SqlQuery<string>("EXEC dbo.usp_GetLoginSalt @p0", "admin").Single().Contains(
                db.Users.Single(u => u.UserName == "admin").PasswordHash.Split('$')[3]), "the hash itself is never returned");
        }

        // ------------------------------------------------------------------ v1.3.1: second review

        [TestMethod]
        public void Login_ReturnsTheStoredUserName_WhateverCaseWasTyped()
        {
            User user = new AuthService(db).Login("ADMIN", "Quedin2024");
            Assert.IsNotNull(user);
            Assert.AreEqual("admin", user.UserName);

            // A manager logged in as "ADMIN" still cannot deactivate their own account.
            Session.SignIn(user);
            Assert.IsNotNull(new AuthService(db).SetActive("admin", false));
        }

        [TestMethod]
        public void ChangePassword_WrongCurrentPassword_IsCountedAndLocks()
        {
            var auth = new AuthService(db);
            for (int i = 0; i < LoginPolicy.MaxFailures; i++)
                Assert.IsNotNull(auth.ChangePassword("admin", "guess-" + i, "NewPass2024", "NewPass2024"));

            StringAssert.Contains(auth.ChangePassword("admin", "Quedin2024", "NewPass2024", "NewPass2024"), "נעול",
                "after the guesses even the right password is refused until the lock ends");
            Assert.IsNull(auth.Login("admin", "NewPass2024"), "the password was not changed");
        }

        [TestMethod]
        public void ChangePassword_IsAuditedByTheDatabase()
        {
            Assert.IsNull(new AuthService(db).ChangePassword("worker", "Worker2024", "NewPass2024", "NewPass2024"));
            Assert.IsTrue(db.AuditLog.Any(a => a.Action == AuditActions.PasswordChanged && a.EntityId == "worker"));
        }

        [TestMethod]
        public void TryLogin_UnknownUserAndWrongPassword_GetTheSameAnswer()
        {
            int unknown = TryLoginResult("nobody-here");
            int wrong = TryLoginResult("worker");
            Assert.AreEqual(wrong, unknown);
        }

        private int TryLoginResult(string userName)
        {
            return db.Database.SqlQuery<LoginRow>("EXEC dbo.usp_TryLogin @p0, @p1", userName, "not-a-hash").Single().Result;
        }

        private class LoginRow
        {
            public int Result { get; set; }
        }

        [TestMethod]
        public void Payment_CannotBeMarkedCancelledWithoutACreditNote()
        {
            var ex = Assert.ThrowsException<System.Data.SqlClient.SqlException>(
                () => db.Database.ExecuteSqlCommand("UPDATE dbo.Payments SET IsCancelled = 1 WHERE PaymentId = 1"));
            StringAssert.Contains(ex.Message, "credit note");
        }

        [TestMethod]
        public void CaseKeepsItsFees_WhenTheCaseTypeChangesLater()
        {
            CaseType type = db.CaseTypes.First(s => s.Steps.Any());
            Assert.IsNull(new CaseService(db).SaveCase(NewCourtCase(type.CaseTypeId), type, true, DateTime.Today));
            LegalCase legalCase = db.Cases.OrderByDescending(c => c.CaseId).First();
            decimal before = CaseService.CaseTotal(legalCase);

            type.BaseFee = (type.BaseFee ?? 0m) + 50000m; // the manager raises the minimum fee afterwards
            type.CourtFee = (type.CourtFee ?? 0m) + 1000m;
            db.SaveChanges();

            Assert.AreEqual(before, CaseService.CaseTotal(legalCase), "an existing case keeps the fees it was opened with");
        }

        [TestMethod]
        public void AuditEntries_RecordTheDatabaseLogin()
        {
            AuditService.Log(db, AuditActions.CaseUpdated, "תיק", 1, "TEST-DBLOGIN");
            db.SaveChanges();
            string login = db.Database.SqlQuery<string>("SELECT DbLogin FROM dbo.AuditLog WHERE Details = N'TEST-DBLOGIN'").Single();
            Assert.IsFalse(string.IsNullOrEmpty(login));
        }

        [TestMethod]
        public void SaveCase_WithoutAnOuterTransaction_RollsBackAFailedSave()
        {
            // The app has no TransactionScope: SaveCase opens its own transaction. Run that path for real
            // (outside the test's scope) and clean up afterwards.
            using (new TransactionScope(TransactionScopeOption.Suppress))
            using (var own = new QuedinDbContext())
            {
                CaseType type = own.CaseTypes.First(s => s.Steps.Count >= 2);
                var service = new CaseService(own);
                LegalCase legalCase = NewCourtCase(type.CaseTypeId);
                string realLawyer = legalCase.OpposingLawyerId;
                legalCase.OpposingLawyerId = "NO-SUCH-ID";
                int casesBefore = own.Cases.Count();

                Assert.ThrowsException<System.Data.Entity.Infrastructure.DbUpdateException>(
                    () => service.SaveCase(legalCase, type, true, new DateTime(2025, 1, 1)));
                Assert.AreEqual(casesBefore, own.Cases.Count(), "nothing was saved");

                legalCase.OpposingLawyerId = realLawyer;
                Assert.IsNull(service.SaveCase(legalCase, type, true, new DateTime(2025, 1, 1)));
                try
                {
                    Assert.AreEqual(type.Steps.Count, own.CaseSteps.Count(s => s.CaseId == legalCase.CaseId));
                }
                finally
                {
                    own.Database.ExecuteSqlCommand("DELETE FROM dbo.CaseSteps WHERE CaseId = @p0; DELETE FROM dbo.Cases WHERE CaseId = @p0", legalCase.CaseId);
                }
            }
        }
    }
}
