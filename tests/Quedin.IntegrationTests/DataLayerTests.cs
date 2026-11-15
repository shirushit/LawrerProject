using System;
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
            db.Users.Add(new User { UserName = "legacy", PasswordHash = "12345", Role = Roles.Worker });
            db.SaveChanges();

            User user = new AuthService(db).Login("legacy", "12345");

            Assert.IsNotNull(user);
            Assert.IsTrue(user.PasswordHash.StartsWith("pbkdf2$"), "password should be re-stored as a hash");
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

        private LegalCase NewCourtCase(int subjectId)
        {
            Customer client = db.Customers.First(c => c.PersonType == PersonTypes.Client);
            Customer opposing = db.Customers.First(c => c.PersonType == PersonTypes.OpposingClient);
            Customer lawyer = db.Customers.First(c => c.PersonType == PersonTypes.OpposingLawyer);
            return new LegalCase
            {
                ClientId = client.CustomerId,
                CaseTypeId = subjectId,
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

            Assert.IsNotNull(service.AddPayment(legalCase, new Payment { Amount = total, Kind = PaymentKinds.Fee }), "overpayment refused");
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
            Assert.IsNull(new CaseService(db).AddPayment(legalCase, new Payment { Amount = total, Kind = PaymentKinds.Fee, PaidOn = DateTime.Today }));
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
            Session.SignIn(new User { UserName = "test-manager", Role = Roles.Manager });
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
            Session.SignIn(new User { UserName = "test-manager", Role = Roles.Manager });
            try
            {
                LegalCase paid = SavedCase();
                var service = new CaseService(db);
                service.AddPayment(paid, new Payment { Amount = 1m, Kind = PaymentKinds.Fee, PaidOn = DateTime.Today });
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
            Assert.AreEqual(due.Count, due.Select(d => d.CaseId).Distinct().Count());
        }
    }
}
