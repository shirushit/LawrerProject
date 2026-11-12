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
        private LawyerDBEntities1 db;

        [TestInitialize]
        public void Setup()
        {
            scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
            db = new LawyerDBEntities1();
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
            Users admin = auth.Login("admin", "Quedin2024");

            Assert.IsNotNull(admin);
            Assert.AreEqual(Roles.Manager, admin.KindWorker);
            Assert.IsNotNull(auth.Login("worker", "Worker2024"));
            Assert.IsNull(auth.Login("admin", "wrong-password"));
            Assert.IsNull(auth.Login("nobody", "Quedin2024"));
        }

        [TestMethod]
        public void Login_UpgradesLegacyPlainTextPassword()
        {
            db.Users.Add(new Users { UserName = "legacy", PasswordWorker = "12345", KindWorker = Roles.Worker });
            db.SaveChanges();

            Users user = new AuthService(db).Login("legacy", "12345");

            Assert.IsNotNull(user);
            Assert.IsTrue(user.PasswordWorker.StartsWith("pbkdf2$"), "password should be re-stored as a hash");
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

            Users created = db.Users.Single(u => u.UserName == "new.user");
            Assert.AreNotEqual("Secret2024", created.PasswordWorker);
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

        private Tikim_Letipul_Orech_Din NewCourtCase(int subjectId)
        {
            Customers client = db.Customers.First(c => c.SugPerson == PersonTypes.Client);
            Customers opposing = db.Customers.First(c => c.SugPerson == PersonTypes.OpposingClient);
            Customers lawyer = db.Customers.First(c => c.SugPerson == PersonTypes.OpposingLawyer);
            return new Tikim_Letipul_Orech_Din
            {
                IDCostemer = client.IDcustemer,
                IDSubjektTik = subjectId,
                IsTheTikMenualInBETMishpat = true,
                IDBetMishpat = db.Bet_Mishpat.First().IDBetMishpat,
                IpuiKohach = "TEST",
                SchumLetvia = 10000m,
                AchuzRevachim = 10,
                Mikdama = 0m,
                KodLakoachNegdi = opposing.IDcustemer,
                KodOrechDinNegdi = lawyer.IDcustemer
            };
        }

        [TestMethod]
        public void OpenCase_CreatesTimelineFromCaseType()
        {
            Subjekt_Tik type = db.Subjekt_Tik.First(s => s.Taalichim_Kvuim_Batik.Any());
            int expectedSteps = type.Taalichim_Kvuim_Batik.Count;
            var service = new CaseService(db);
            Tikim_Letipul_Orech_Din tik = NewCourtCase(type.IDSugTik);
            DateTime opened = new DateTime(2025, 1, 1);

            Assert.IsNull(service.CreatePlan(tik, opened));
            db.Tikim_Letipul_Orech_Din.Add(tik);
            db.SaveChanges();

            var saved = db.Tikim_Letipul_Orech_Din.Single(t => t.IDTik == tik.IDTik);
            var steps = CaseService.OrderedSteps(saved);
            Taalichim_Kvuim_Batik first = type.Taalichim_Kvuim_Batik.OrderBy(k => k.NumLevel).First();

            Assert.AreEqual(expectedSteps, steps.Count);
            Assert.AreEqual(first.IDTaalich, saved.StatusTik);
            Assert.AreEqual(BusinessCalendar.NextWorkday(opened.AddDays(CaseService.ParseDays(db.Taalichim_Letikim.Find(first.IDTaalich).MeshechZmanLetaalich))), steps[0].TaharichMeshoar);
            Assert.IsTrue(steps.All(s => s.MeshechYamim.HasValue), "durations are kept with the case");
            Assert.IsTrue(steps.All(s => s.Buzha == false && s.TaharichBizhua == null));
        }

        [TestMethod]
        public void OpenCase_WithoutFixedSteps_IsRefused()
        {
            var empty = new Subjekt_Tik { NameSugTik = "TEST-EMPTY", ScharTircha = 0m, Agra = 0m };
            db.Subjekt_Tik.Add(empty);
            db.SaveChanges();

            string error = new CaseService(db).CreatePlan(NewCourtCase(empty.IDSugTik), DateTime.Today);

            Assert.IsNotNull(error);
        }

        [TestMethod]
        public void CompleteStep_PushesFollowingDatesInDatabase()
        {
            Subjekt_Tik type = db.Subjekt_Tik.First(s => s.Taalichim_Kvuim_Batik.Count >= 3);
            var service = new CaseService(db);
            Tikim_Letipul_Orech_Din tik = NewCourtCase(type.IDSugTik);
            service.CreatePlan(tik, new DateTime(2025, 1, 1));
            db.Tikim_Letipul_Orech_Din.Add(tik);
            db.SaveChanges();

            var steps = CaseService.OrderedSteps(tik);
            DateTime doneOn = new DateTime(2025, 2, 1);
            int secondDuration = CaseService.ParseDays(db.Taalichim_Letikim.Find(steps[1].IDTahalich).MeshechZmanLetaalich);

            Assert.IsNotNull(service.CompleteStep(tik, steps[1].IDTahalichTipulLetik, doneOn), "skipping must be refused");
            Assert.IsNull(service.CompleteStep(tik, steps[0].IDTahalichTipulLetik, doneOn));

            using (var fresh = new LawyerDBEntities1())
            {
                var reloaded = CaseService.OrderedSteps(fresh.Tikim_Letipul_Orech_Din.Single(t => t.IDTik == tik.IDTik));
                Assert.AreEqual(true, reloaded[0].Buzha);
                Assert.AreEqual(doneOn, reloaded[0].TaharichBizhua);
                Assert.AreEqual(BusinessCalendar.NextWorkday(doneOn.AddDays(secondDuration)), reloaded[1].TaharichMeshoar);
                Assert.AreEqual(reloaded[1].IDTahalich, fresh.Tikim_Letipul_Orech_Din.Single(t => t.IDTik == tik.IDTik).StatusTik);
            }
        }

        // ------------------------------------------------------------------ money

        [TestMethod]
        public void OpenBalance_DemoCases()
        {
            // Demo case 1: 85,000 x 20% + 1,200 court fee = 18,200; paid 3,000 + 5,000.
            Assert.AreEqual(10200m, CaseService.OpenBalance(db.Tikim_Letipul_Orech_Din.Single(t => t.IDTik == 1)));
            // Demo case 2: 42,000 x 25% + 0 = 10,500; no payments yet.
            Assert.AreEqual(10500m, CaseService.OpenBalance(db.Tikim_Letipul_Orech_Din.Single(t => t.IDTik == 2)));
            // Demo case 5 is fully paid.
            Assert.AreEqual(0m, CaseService.OpenBalance(db.Tikim_Letipul_Orech_Din.Single(t => t.IDTik == 5)));
        }

        [TestMethod]
        public void DemoData_StatusMatchesNextPendingStep()
        {
            foreach (var tik in db.Tikim_Letipul_Orech_Din.ToList())
            {
                var next = CaseService.OrderedSteps(tik).FirstOrDefault(s => s.Buzha != true);
                if (next != null) Assert.AreEqual(next.IDTahalich, tik.StatusTik, "case " + tik.IDTik);
            }
        }

        // ------------------------------------------------------------------ v1.2 rules

        private Tikim_Letipul_Orech_Din SavedCase()
        {
            Subjekt_Tik type = db.Subjekt_Tik.First(s => s.Taalichim_Kvuim_Batik.Count >= 2);
            Tikim_Letipul_Orech_Din tik = NewCourtCase(type.IDSugTik);
            tik.SchumLetvia = 10000m;
            tik.AchuzRevachim = 10;
            new CaseService(db).CreatePlan(tik, new DateTime(2025, 1, 5));
            db.Tikim_Letipul_Orech_Din.Add(tik);
            db.SaveChanges();
            return tik;
        }

        [TestMethod]
        public void Payment_StoresVatAndBalance_AndCancellationAddsReversal()
        {
            Tikim_Letipul_Orech_Din tik = SavedCase();
            var service = new CaseService(db);
            decimal total = CaseService.CaseTotal(tik);

            var payment = new Tashlumim_Letik { Schum = 500m, SugTashlum = PaymentKinds.Fee, EmzhaeyTashlum = "מזומן", TaarichTashlum = DateTime.Today, Teur = "מקדמה" };
            Assert.IsNull(service.AddPayment(tik, payment));
            Assert.AreEqual(total - 500m, payment.Itra);
            Assert.AreEqual(AppSettings.VatRate, payment.VatRate);

            Assert.IsNotNull(service.AddPayment(tik, new Tashlumim_Letik { Schum = total, SugTashlum = PaymentKinds.Fee }), "overpayment refused");
            Assert.IsNull(service.CancelPayment(tik, payment.IDTashlum, "נרשם בטעות"));
            Assert.IsNotNull(service.CancelPayment(tik, payment.IDTashlum, null), "cannot cancel twice");

            using (var fresh = new LawyerDBEntities1())
            {
                var t = fresh.Tikim_Letipul_Orech_Din.Single(x => x.IDTik == tik.IDTik);
                Assert.AreEqual(2, t.Tashlumim_Letik.Count);
                Assert.IsTrue(t.Tashlumim_Letik.Single(p => p.IDTashlum == payment.IDTashlum).Mevutal);
                Assert.AreEqual(-500m, t.Tashlumim_Letik.Single(p => p.MevatelTashlum == payment.IDTashlum).Schum);
                Assert.AreEqual(total, CaseService.OpenBalance(t));
                Assert.IsTrue(fresh.Yoman.Any(y => y.Peula == AuditActions.PaymentCancelled && y.KodYeshut == payment.IDTashlum.ToString()));
            }
        }

        [TestMethod]
        public void FullPayment_SetsPaidFlag()
        {
            Tikim_Letipul_Orech_Din tik = SavedCase();
            decimal total = CaseService.CaseTotal(tik);
            Assert.IsNull(new CaseService(db).AddPayment(tik, new Tashlumim_Letik { Schum = total, SugTashlum = PaymentKinds.Fee, TaarichTashlum = DateTime.Today }));
            Assert.IsTrue(tik.Shulam);
            Assert.AreEqual(0m, CaseService.OpenBalance(tik));
        }

        [TestMethod]
        public void ConcurrentEdit_OfTheSameCase_IsDetected()
        {
            Tikim_Letipul_Orech_Din tik = SavedCase();
            using (var other = new LawyerDBEntities1())
            {
                var copy = other.Tikim_Letipul_Orech_Din.Single(t => t.IDTik == tik.IDTik);
                copy.AdressNeches = "שונה על ידי משתמש אחר";
                other.SaveChanges();
            }
            tik.AdressNeches = "השינוי שלי";
            Assert.ThrowsException<System.Data.Entity.Infrastructure.DbUpdateConcurrencyException>(() => db.SaveChanges());
        }

        [TestMethod]
        public void RescheduleAndUndo_UpdateTheDatabase()
        {
            Tikim_Letipul_Orech_Din tik = SavedCase();
            var service = new CaseService(db);
            var steps = CaseService.OrderedSteps(tik);
            DateTime hearing = new DateTime(2025, 6, 3);

            Assert.IsNull(service.RescheduleStep(tik, steps[1].IDTahalichTipulLetik, hearing, "נקבע ע\"י בית המשפט"));
            Assert.AreEqual(hearing, steps[1].TaharichMeshoar);
            Assert.AreEqual("נקבע ע\"י בית המשפט", steps[1].Hearot);

            Assert.IsNull(service.CompleteStep(tik, steps[0].IDTahalichTipulLetik, new DateTime(2025, 1, 20)));
            Session.SignOut();
            Assert.IsNotNull(service.UndoLastStep(tik), "only a manager may undo");
            Session.SignIn(new Users { UserName = "test-manager", KindWorker = Roles.Manager });
            try
            {
                Assert.IsNull(service.UndoLastStep(tik));
                Assert.AreEqual(false, steps[0].Buzha);
            }
            finally
            {
                Session.SignOut();
            }
        }

        [TestMethod]
        public void DeleteCase_OnlyWhenNothingHappenedYet()
        {
            Session.SignIn(new Users { UserName = "test-manager", KindWorker = Roles.Manager });
            try
            {
                Tikim_Letipul_Orech_Din paid = SavedCase();
                var service = new CaseService(db);
                service.AddPayment(paid, new Tashlumim_Letik { Schum = 1m, SugTashlum = PaymentKinds.Fee, TaarichTashlum = DateTime.Today });
                Assert.IsNotNull(service.DeleteCase(paid));

                Tikim_Letipul_Orech_Din mistake = SavedCase();
                int id = mistake.IDTik;
                Assert.IsNull(service.DeleteCase(mistake));
                Assert.IsFalse(db.Tikim_Letipul_Orech_Din.Any(t => t.IDTik == id));
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
