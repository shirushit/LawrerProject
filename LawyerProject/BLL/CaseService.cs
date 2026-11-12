using System;
using System.Collections.Generic;
using System.Linq;
using LawyerProject.DB;

namespace LawyerProject.BLL
{
    /// <summary>A case's next open step, shown on the home screen.</summary>
    public class DueItem
    {
        public int CaseId { get; set; }
        public string Client { get; set; }
        public string Step { get; set; }
        public DateTime Due { get; set; }
        public int DaysLate { get; set; }
        public string Note { get; set; }
    }

    /// <summary>
    /// Connects the pure <see cref="CaseScheduler"/> and <see cref="FeeCalculator"/> to the
    /// Entity Framework model. Methods that change data return null on success or a Hebrew error
    /// message, write an audit entry and save. A save can throw DbUpdateConcurrencyException when
    /// another user changed the same case in the meantime; screens catch it and reload.
    /// </summary>
    public class CaseService
    {
        private readonly LawyerDBEntities1 db;

        public CaseService(LawyerDBEntities1 db)
        {
            this.db = db;
        }

        // ------------------------------------------------------------------ timeline

        /// <summary>
        /// Adds the fixed steps of the case type to a new case and sets its status to the first step.
        /// Each step keeps the duration it had when the case was opened. Does not save.
        /// </summary>
        public string CreatePlan(Tikim_Letipul_Orech_Din tik, DateTime openedOn)
        {
            List<StepTemplate> templates = db.Taalichim_Kvuim_Batik
                .Where(k => k.IDSubjektTik == tik.IDSubjektTik)
                .ToList()
                .Select(k => new StepTemplate
                {
                    ProcessId = k.IDTaalich,
                    Level = k.NumLevel,
                    DurationDays = ParseDays(k.Taalichim_Letikim.MeshechZmanLetaalich)
                })
                .ToList();

            if (templates.Count == 0)
                return "לא הוגדרו תהליכים קבועים לנושא התיק שנבחר. מנהלת המשרד מגדירה אותם במסך \"נושאי תיקים\".";

            List<ScheduledStep> plan = CaseScheduler.BuildPlan(templates, openedOn);
            foreach (ScheduledStep step in plan)
            {
                tik.Tahalich_Tipul_Betik.Add(new Tahalich_Tipul_Betik
                {
                    IDTahalich = step.ProcessId,
                    TaharichMeshoar = step.EstimatedDate,
                    TaharichBizhua = null,
                    Buzha = false,
                    MeshechYamim = step.DurationDays
                });
            }
            tik.StatusTik = plan[0].ProcessId;
            return null;
        }

        /// <summary>Marks the next open step as done on the given date (not in the future) and pushes later dates.</summary>
        public string CompleteStep(Tikim_Letipul_Orech_Din tik, int stepId, DateTime doneOn)
        {
            if (Validation.IsFutureDate(doneOn, DateTime.Today)) return "לא ניתן לסמן שלב כבוצע בתאריך עתידי.";
            return ApplyToTimeline(tik, stepId, (steps, target) => CaseScheduler.CompleteStep(steps, target, doneOn),
                AuditActions.StepCompleted, "בוצע ב-" + doneOn.ToString("dd/MM/yyyy"));
        }

        /// <summary>Sets an open step to a date decided outside the office (e.g. by the court) and pushes later dates.</summary>
        public string RescheduleStep(Tikim_Letipul_Orech_Din tik, int stepId, DateTime newDate, string note)
        {
            string error = ApplyToTimeline(tik, stepId, (steps, target) => CaseScheduler.Reschedule(steps, target, newDate),
                AuditActions.StepRescheduled, "מועד חדש " + newDate.ToString("dd/MM/yyyy"), save: false);
            if (error != null) return error;
            if (!string.IsNullOrWhiteSpace(note))
                SetNoteNoSave(tik, stepId, note);
            db.SaveChanges();
            return null;
        }

        /// <summary>Re-opens the most recently completed step (managers only).</summary>
        public string UndoLastStep(Tikim_Letipul_Orech_Din tik)
        {
            if (!Session.IsManager) return "רק מנהלת יכולה לבטל סימון של שלב.";
            List<Tahalich_Tipul_Betik> entities = tik.Tahalich_Tipul_Betik.ToList();
            Dictionary<int, ScheduledStep> map = entities.ToDictionary(e => e.IDTahalichTipulLetik, ToScheduled);
            ScheduledStep undone;
            try
            {
                undone = CaseScheduler.UndoLastCompletion(map.Values.ToList());
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
            WriteBack(tik, entities, map);
            AuditService.Log(db, AuditActions.StepUndone, "תיק", tik.IDTik, "שלב " + undone.Id);
            db.SaveChanges();
            return null;
        }

        /// <summary>Saves a free-text note on a step (for example "postponed at the other side's request").</summary>
        public string SetNote(Tikim_Letipul_Orech_Din tik, int stepId, string note)
        {
            string error = SetNoteNoSave(tik, stepId, note);
            if (error != null) return error;
            db.SaveChanges();
            return null;
        }

        private string SetNoteNoSave(Tikim_Letipul_Orech_Din tik, int stepId, string note)
        {
            Tahalich_Tipul_Betik step = tik.Tahalich_Tipul_Betik.FirstOrDefault(s => s.IDTahalichTipulLetik == stepId);
            if (step == null) return "התהליך שנבחר אינו שייך לתיק.";
            string text = (note ?? string.Empty).Trim();
            step.Hearot = text.Length == 0 ? null : (text.Length > 200 ? text.Substring(0, 200) : text);
            AuditService.Log(db, AuditActions.StepNote, "תיק", tik.IDTik, step.Hearot);
            TouchCase(tik);
            return null;
        }

        private string ApplyToTimeline(Tikim_Letipul_Orech_Din tik, int stepId,
            Action<IList<ScheduledStep>, ScheduledStep> change, string action, string details, bool save = true)
        {
            List<Tahalich_Tipul_Betik> entities = tik.Tahalich_Tipul_Betik.ToList();
            Dictionary<int, ScheduledStep> map = entities.ToDictionary(e => e.IDTahalichTipulLetik, ToScheduled);

            ScheduledStep target;
            if (!map.TryGetValue(stepId, out target)) return "התהליך שנבחר אינו שייך לתיק.";

            try
            {
                change(map.Values.ToList(), target);
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }

            WriteBack(tik, entities, map);
            AuditService.Log(db, action, "תיק", tik.IDTik, details);
            if (save) db.SaveChanges();
            return null;
        }

        private void WriteBack(Tikim_Letipul_Orech_Din tik, List<Tahalich_Tipul_Betik> entities, Dictionary<int, ScheduledStep> map)
        {
            foreach (Tahalich_Tipul_Betik e in entities)
            {
                ScheduledStep s = map[e.IDTahalichTipulLetik];
                e.Buzha = s.IsDone;
                e.TaharichBizhua = s.DoneDate;
                e.TaharichMeshoar = s.EstimatedDate;
            }
            ScheduledStep next = CaseScheduler.NextPending(map.Values);
            ScheduledStep last = CaseScheduler.Ordered(map.Values).LastOrDefault();
            tik.StatusTik = next != null ? next.ProcessId : (last != null ? last.ProcessId : tik.StatusTik);
            TouchCase(tik);
        }

        /// <summary>Steps of the case in timeline order (the order of the case type's levels).</summary>
        public static List<Tahalich_Tipul_Betik> OrderedSteps(Tikim_Letipul_Orech_Din tik)
        {
            return tik.Tahalich_Tipul_Betik.OrderBy(s => s.IDTahalichTipulLetik).ToList();
        }

        /// <summary>True when every step of the case is done.</summary>
        public static bool IsFinished(Tikim_Letipul_Orech_Din tik)
        {
            return tik.Tahalich_Tipul_Betik.Count > 0 && tik.Tahalich_Tipul_Betik.All(s => s.Buzha == true);
        }

        /// <summary>The next open step of every open case due within <paramref name="days"/> days or late, nearest first.</summary>
        public List<DueItem> DueSteps(DateTime today, int days)
        {
            DateTime limit = today.Date.AddDays(days);
            var cases = db.Tikim_Letipul_Orech_Din
                .Where(t => t.Tahalich_Tipul_Betik.Any(s => s.Buzha != true && s.TaharichMeshoar <= limit))
                .ToList();

            var items = new List<DueItem>();
            foreach (Tikim_Letipul_Orech_Din t in cases)
            {
                Tahalich_Tipul_Betik next = OrderedSteps(t).FirstOrDefault(s => s.Buzha != true);
                if (next == null || !next.TaharichMeshoar.HasValue || next.TaharichMeshoar.Value > limit) continue;
                items.Add(new DueItem
                {
                    CaseId = t.IDTik,
                    Client = t.Customers != null ? t.Customers.DisplayName : "",
                    Step = next.Taalichim_Letikim != null ? next.Taalichim_Letikim.NameTaalich : "",
                    Due = next.TaharichMeshoar.Value,
                    DaysLate = CaseScheduler.DaysLate(next.TaharichMeshoar.Value, today),
                    Note = next.Hearot
                });
            }
            return items.OrderBy(i => i.Due).ThenBy(i => i.CaseId).ToList();
        }

        // ------------------------------------------------------------------ money

        /// <summary>Total the office charges: agreed % of the claim (or property value) + the case type's court fee.</summary>
        public static decimal CaseTotal(Tikim_Letipul_Orech_Din tik)
        {
            decimal courtFee = tik.Subjekt_Tik != null ? (tik.Subjekt_Tik.Agra ?? 0m) : 0m;
            double percent = Math.Min(100, Math.Max(0, tik.AchuzRevachim ?? 0d));
            return FeeCalculator.CaseTotal(Math.Max(0m, tik.SchumLetvia ?? 0m), percent, courtFee);
        }

        /// <summary>Sum of all payments (cancellations are negative).</summary>
        public static decimal PaidTotal(Tikim_Letipul_Orech_Din tik)
        {
            return FeeCalculator.Round(tik.Tashlumim_Letik.Sum(p => p.Schum));
        }

        /// <summary>What the client still owes, always computed from the case total and the payments.</summary>
        public static decimal OpenBalance(Tikim_Letipul_Orech_Din tik)
        {
            return FeeCalculator.Balance(CaseTotal(tik), tik.Tashlumim_Letik.Select(p => p.Schum));
        }

        /// <summary>Keeps the "paid" and "advance paid" flags in line with the payments. Never edited by hand.</summary>
        public static void RefreshPaymentFlags(Tikim_Letipul_Orech_Din tik)
        {
            decimal paid = PaidTotal(tik);
            tik.Shulam = CaseTotal(tik) > 0 && OpenBalance(tik) <= 0;
            tik.ShulamMikdama = tik.Mikdama > 0 ? paid >= tik.Mikdama : (bool?)null;
        }

        /// <summary>
        /// Records a payment. The amount must be between 0.01 and the open balance. The VAT rate in
        /// force today is stored with the payment, so a receipt printed later shows the same numbers.
        /// </summary>
        public string AddPayment(Tikim_Letipul_Orech_Din tik, Tashlumim_Letik payment)
        {
            decimal open = OpenBalance(tik);
            if (payment.Schum < 0.01m) return "סכום התשלום חייב להיות לפחות 0.01 ₪.";
            if (payment.Schum > open) return "הסכום גבוה מהיתרה לתשלום (" + open.ToString("#,##0.00") + " ₪).";
            if (!PaymentKinds.All.Contains(payment.SugTashlum)) return "יש לבחור סוג תשלום.";

            payment.Schum = FeeCalculator.Round(payment.Schum);
            payment.IDTik = tik.IDTik;
            payment.Itra = FeeCalculator.BalanceAfterPayment(open, payment.Schum);
            payment.VatRate = AppSettings.VatRate;
            payment.Mevutal = false;
            tik.Tashlumim_Letik.Add(payment);
            RefreshPaymentFlags(tik);
            TouchCase(tik);
            AuditService.Log(db, AuditActions.PaymentAdded, "תיק", tik.IDTik,
                payment.SugTashlum + " " + payment.Schum.ToString("0.00") + " " + payment.EmzhaeyTashlum);
            db.SaveChanges();
            return null;
        }

        /// <summary>
        /// Cancels a payment by adding an opposite (negative) entry, as accounting requires,
        /// instead of deleting it. The original stays visible and is marked cancelled.
        /// </summary>
        public string CancelPayment(Tikim_Letipul_Orech_Din tik, int paymentId, string reason)
        {
            Tashlumim_Letik original = tik.Tashlumim_Letik.FirstOrDefault(p => p.IDTashlum == paymentId);
            if (original == null) return "התשלום אינו שייך לתיק.";
            if (original.Mevutal) return "התשלום כבר בוטל.";
            if (original.MevatelTashlum.HasValue) return "לא ניתן לבטל רשומת ביטול.";

            original.Mevutal = true;
            var reversal = new Tashlumim_Letik
            {
                IDTik = tik.IDTik,
                Schum = -original.Schum,
                SugTashlum = original.SugTashlum,
                VatRate = original.VatRate,
                EmzhaeyTashlum = original.EmzhaeyTashlum,
                TaarichTashlum = DateTime.Today,
                Teur = Truncate("ביטול קבלה " + original.IDTashlum + (string.IsNullOrWhiteSpace(reason) ? "" : ": " + reason.Trim()), 50),
                MevatelTashlum = original.IDTashlum,
                Mevutal = false
            };
            tik.Tashlumim_Letik.Add(reversal);
            reversal.Itra = OpenBalance(tik);
            RefreshPaymentFlags(tik);
            TouchCase(tik);
            AuditService.Log(db, AuditActions.PaymentCancelled, "תשלום", original.IDTashlum, reason);
            db.SaveChanges();
            return null;
        }

        // ------------------------------------------------------------------ case lifecycle

        /// <summary>A case opened by mistake can be deleted by a manager while nothing was paid or completed.</summary>
        public string DeleteCase(Tikim_Letipul_Orech_Din tik)
        {
            if (!Session.IsManager) return "רק מנהלת יכולה למחוק תיק.";
            if (tik.Tashlumim_Letik.Any()) return "לא ניתן למחוק תיק שנרשמו בו תשלומים.";
            if (tik.Tahalich_Tipul_Betik.Any(s => s.Buzha == true)) return "לא ניתן למחוק תיק שסומנו בו שלבים כבוצעים.";

            int id = tik.IDTik;
            foreach (Tahalich_Tipul_Betik step in tik.Tahalich_Tipul_Betik.ToList())
                db.Tahalich_Tipul_Betik.Remove(step);
            db.Tikim_Letipul_Orech_Din.Remove(tik);
            AuditService.Log(db, AuditActions.CaseDeleted, "תיק", id, null);
            db.SaveChanges();
            return null;
        }

        /// <summary>
        /// Forces an UPDATE of the case row so its RowVersion is checked: if another user changed
        /// the case in the meantime, SaveChanges throws instead of silently overwriting.
        /// </summary>
        private void TouchCase(Tikim_Letipul_Orech_Din tik)
        {
            if (tik.IDTik == 0) return;
            var entry = db.Entry(tik);
            if (entry.State == System.Data.Entity.EntityState.Unchanged)
                entry.Property(t => t.Shulam).IsModified = true;
        }

        public static int ParseDays(string value)
        {
            int days;
            return int.TryParse(value, out days) && days > 0 ? days : 0;
        }

        private static string Truncate(string s, int max)
        {
            return s.Length <= max ? s : s.Substring(0, max);
        }

        private ScheduledStep ToScheduled(Tahalich_Tipul_Betik e)
        {
            int days;
            if (e.MeshechYamim.HasValue)
            {
                days = e.MeshechYamim.Value; // the duration when the case was opened
            }
            else
            {
                // Steps created before v1.2 have no snapshot; fall back to the catalogue.
                Taalichim_Letikim process = e.Taalichim_Letikim ?? db.Taalichim_Letikim.Find(e.IDTahalich);
                days = process != null ? ParseDays(process.MeshechZmanLetaalich) : 0;
            }
            return new ScheduledStep
            {
                Id = e.IDTahalichTipulLetik,
                ProcessId = e.IDTahalich,
                DurationDays = days,
                EstimatedDate = e.TaharichMeshoar ?? DateTime.MaxValue.Date,
                DoneDate = e.TaharichBizhua,
                IsDone = e.Buzha == true
            };
        }
    }
}
