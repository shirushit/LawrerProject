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
        private readonly QuedinDbContext db;

        public CaseService(QuedinDbContext db)
        {
            this.db = db;
        }

        // ------------------------------------------------------------------ timeline

        /// <summary>
        /// Adds the fixed steps of the case type to a new case and sets its status to the first step.
        /// Each step keeps the duration it had when the case was opened. Does not save.
        /// </summary>
        public string CreatePlan(LegalCase legalCase, DateTime openedOn)
        {
            List<StepTemplate> templates = db.CaseTypeSteps
                .Where(k => k.CaseTypeId == legalCase.CaseTypeId)
                .ToList()
                .Select(k => new StepTemplate
                {
                    ProcessId = k.ProcedureId,
                    Level = k.StepOrder,
                    DurationDays = k.Procedure.DurationDays
                })
                .ToList();

            if (templates.Count == 0)
                return "לא הוגדרו תהליכים קבועים לנושא התיק שנבחר. מנהלת המשרד מגדירה אותם במסך \"נושאי תיקים\".";

            List<ScheduledStep> plan = CaseScheduler.BuildPlan(templates, openedOn);
            foreach (ScheduledStep step in plan)
            {
                legalCase.Steps.Add(new CaseStep
                {
                    ProcedureId = step.ProcessId,
                    DueDate = step.EstimatedDate,
                    DoneDate = null,
                    IsDone = false,
                    DurationDays = step.DurationDays
                });
            }
            legalCase.CurrentProcedureId = plan[0].ProcessId;
            return null;
        }

        /// <summary>Marks the next open step as done on the given date (not in the future) and pushes later dates.</summary>
        public string CompleteStep(LegalCase legalCase, int stepId, DateTime doneOn)
        {
            if (Validation.IsFutureDate(doneOn, DateTime.Today)) return "לא ניתן לסמן שלב כבוצע בתאריך עתידי.";
            return ApplyToTimeline(legalCase, stepId, (steps, target) => CaseScheduler.CompleteStep(steps, target, doneOn),
                AuditActions.StepCompleted, "בוצע ב-" + doneOn.ToString("dd/MM/yyyy"));
        }

        /// <summary>Sets an open step to a date decided outside the office (e.g. by the court) and pushes later dates.</summary>
        public string RescheduleStep(LegalCase legalCase, int stepId, DateTime newDate, string note)
        {
            string error = ApplyToTimeline(legalCase, stepId, (steps, target) => CaseScheduler.Reschedule(steps, target, newDate),
                AuditActions.StepRescheduled, "מועד חדש " + newDate.ToString("dd/MM/yyyy"), save: false);
            if (error != null) return error;
            if (!string.IsNullOrWhiteSpace(note))
                SetNoteNoSave(legalCase, stepId, note);
            db.SaveChanges();
            return null;
        }

        /// <summary>Re-opens the most recently completed step (managers only).</summary>
        public string UndoLastStep(LegalCase legalCase)
        {
            if (!Session.IsManager) return "רק מנהלת יכולה לבטל סימון של שלב.";
            List<CaseStep> entities = legalCase.Steps.ToList();
            Dictionary<int, ScheduledStep> map = entities.ToDictionary(e => e.CaseStepId, ToScheduled);
            ScheduledStep undone;
            try
            {
                undone = CaseScheduler.UndoLastCompletion(map.Values.ToList());
            }
            catch (InvalidOperationException ex)
            {
                return ex.Message;
            }
            WriteBack(legalCase, entities, map);
            AuditService.Log(db, AuditActions.StepUndone, "תיק", legalCase.CaseId, "שלב " + undone.Id);
            db.SaveChanges();
            return null;
        }

        /// <summary>Saves a free-text note on a step (for example "postponed at the other side's request").</summary>
        public string SetNote(LegalCase legalCase, int stepId, string note)
        {
            string error = SetNoteNoSave(legalCase, stepId, note);
            if (error != null) return error;
            db.SaveChanges();
            return null;
        }

        private string SetNoteNoSave(LegalCase legalCase, int stepId, string note)
        {
            CaseStep step = legalCase.Steps.FirstOrDefault(s => s.CaseStepId == stepId);
            if (step == null) return "התהליך שנבחר אינו שייך לתיק.";
            string text = (note ?? string.Empty).Trim();
            step.Notes = text.Length == 0 ? null : (text.Length > 200 ? text.Substring(0, 200) : text);
            AuditService.Log(db, AuditActions.StepNote, "תיק", legalCase.CaseId, step.Notes);
            TouchCase(legalCase);
            return null;
        }

        private string ApplyToTimeline(LegalCase legalCase, int stepId,
            Action<IList<ScheduledStep>, ScheduledStep> change, string action, string details, bool save = true)
        {
            List<CaseStep> entities = legalCase.Steps.ToList();
            Dictionary<int, ScheduledStep> map = entities.ToDictionary(e => e.CaseStepId, ToScheduled);

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

            WriteBack(legalCase, entities, map);
            AuditService.Log(db, action, "תיק", legalCase.CaseId, details);
            if (save) db.SaveChanges();
            return null;
        }

        private void WriteBack(LegalCase legalCase, List<CaseStep> entities, Dictionary<int, ScheduledStep> map)
        {
            foreach (CaseStep e in entities)
            {
                ScheduledStep s = map[e.CaseStepId];
                e.IsDone = s.IsDone;
                e.DoneDate = s.DoneDate;
                e.DueDate = s.EstimatedDate;
            }
            ScheduledStep next = CaseScheduler.NextPending(map.Values);
            ScheduledStep last = CaseScheduler.Ordered(map.Values).LastOrDefault();
            legalCase.CurrentProcedureId = next != null ? next.ProcessId : (last != null ? last.ProcessId : legalCase.CurrentProcedureId);
            TouchCase(legalCase);
        }

        /// <summary>Steps of the case in timeline order (the order of the case type's levels).</summary>
        public static List<CaseStep> OrderedSteps(LegalCase legalCase)
        {
            return legalCase.Steps.OrderBy(s => s.CaseStepId).ToList();
        }

        /// <summary>True when every step of the case is done.</summary>
        public static bool IsFinished(LegalCase legalCase)
        {
            return legalCase.Steps.Count > 0 && legalCase.Steps.All(s => s.IsDone == true);
        }

        /// <summary>The next open step of every open case due within <paramref name="days"/> days or late, nearest first.</summary>
        public List<DueItem> DueSteps(DateTime today, int days)
        {
            DateTime limit = today.Date.AddDays(days);
            var cases = db.Cases
                .Where(t => t.Steps.Any(s => s.IsDone != true && s.DueDate <= limit))
                .ToList();

            var items = new List<DueItem>();
            foreach (LegalCase t in cases)
            {
                CaseStep next = OrderedSteps(t).FirstOrDefault(s => s.IsDone != true);
                if (next == null || !next.DueDate.HasValue || next.DueDate.Value > limit) continue;
                items.Add(new DueItem
                {
                    CaseId = t.CaseId,
                    Client = t.Client != null ? t.Client.DisplayName : "",
                    Step = next.Procedure != null ? next.Procedure.Name : "",
                    Due = next.DueDate.Value,
                    DaysLate = CaseScheduler.DaysLate(next.DueDate.Value, today),
                    Note = next.Notes
                });
            }
            return items.OrderBy(i => i.Due).ThenBy(i => i.CaseId).ToList();
        }

        // ------------------------------------------------------------------ money

        /// <summary>Total the office charges: agreed % of the claim (or property value) + the case type's court fee.</summary>
        public static decimal CaseTotal(LegalCase legalCase)
        {
            decimal courtFee = legalCase.CaseType != null ? (legalCase.CaseType.CourtFee ?? 0m) : 0m;
            double percent = Math.Min(100, Math.Max(0, legalCase.FeePercent ?? 0d));
            return FeeCalculator.CaseTotal(Math.Max(0m, legalCase.ClaimAmount ?? 0m), percent, courtFee);
        }

        /// <summary>Sum of all payments (cancellations are negative).</summary>
        public static decimal PaidTotal(LegalCase legalCase)
        {
            return FeeCalculator.Round(legalCase.Payments.Sum(p => p.Amount));
        }

        /// <summary>What the client still owes, always computed from the case total and the payments.</summary>
        public static decimal OpenBalance(LegalCase legalCase)
        {
            return FeeCalculator.Balance(CaseTotal(legalCase), legalCase.Payments.Select(p => p.Amount));
        }

        /// <summary>Keeps the "paid" and "advance paid" flags in line with the payments. Never edited by hand.</summary>
        public static void RefreshPaymentFlags(LegalCase legalCase)
        {
            decimal paid = PaidTotal(legalCase);
            legalCase.IsPaid = CaseTotal(legalCase) > 0 && OpenBalance(legalCase) <= 0;
            legalCase.IsAdvancePaid = legalCase.Advance > 0 ? paid >= legalCase.Advance : (bool?)null;
        }

        /// <summary>
        /// Records a payment. The amount must be between 0.01 and the open balance. The VAT rate in
        /// force today is stored with the payment, so a receipt printed later shows the same numbers.
        /// </summary>
        public string AddPayment(LegalCase legalCase, Payment payment)
        {
            decimal open = OpenBalance(legalCase);
            if (payment.Amount < 0.01m) return "סכום התשלום חייב להיות לפחות 0.01 ₪.";
            if (payment.Amount > open) return "הסכום גבוה מהיתרה לתשלום (" + open.ToString("#,##0.00") + " ₪).";
            if (!PaymentKinds.All.Contains(payment.Kind)) return "יש לבחור סוג תשלום.";

            payment.Amount = FeeCalculator.Round(payment.Amount);
            payment.CaseId = legalCase.CaseId;
            payment.BalanceAfter = FeeCalculator.BalanceAfterPayment(open, payment.Amount);
            payment.VatRate = AppSettings.VatRate;
            payment.IsCancelled = false;
            legalCase.Payments.Add(payment);
            RefreshPaymentFlags(legalCase);
            TouchCase(legalCase);
            AuditService.Log(db, AuditActions.PaymentAdded, "תיק", legalCase.CaseId,
                payment.Kind + " " + payment.Amount.ToString("0.00") + " " + payment.Method);
            db.SaveChanges();
            return null;
        }

        /// <summary>
        /// Cancels a payment by adding an opposite (negative) entry, as accounting requires,
        /// instead of deleting it. The original stays visible and is marked cancelled.
        /// </summary>
        public string CancelPayment(LegalCase legalCase, int paymentId, string reason)
        {
            Payment original = legalCase.Payments.FirstOrDefault(p => p.PaymentId == paymentId);
            if (original == null) return "התשלום אינו שייך לתיק.";
            if (original.IsCancelled) return "התשלום כבר בוטל.";
            if (original.CancelsPaymentId.HasValue) return "לא ניתן לבטל רשומת ביטול.";

            original.IsCancelled = true;
            var reversal = new Payment
            {
                CaseId = legalCase.CaseId,
                Amount = -original.Amount,
                Kind = original.Kind,
                VatRate = original.VatRate,
                Method = original.Method,
                PaidOn = DateTime.Today,
                Description = Truncate("ביטול קבלה " + original.PaymentId + (string.IsNullOrWhiteSpace(reason) ? "" : ": " + reason.Trim()), 50),
                CancelsPaymentId = original.PaymentId,
                IsCancelled = false
            };
            legalCase.Payments.Add(reversal);
            reversal.BalanceAfter = OpenBalance(legalCase);
            RefreshPaymentFlags(legalCase);
            TouchCase(legalCase);
            AuditService.Log(db, AuditActions.PaymentCancelled, "תשלום", original.PaymentId, reason);
            db.SaveChanges();
            return null;
        }

        // ------------------------------------------------------------------ case lifecycle

        /// <summary>A case opened by mistake can be deleted by a manager while nothing was paid or completed.</summary>
        public string DeleteCase(LegalCase legalCase)
        {
            if (!Session.IsManager) return "רק מנהלת יכולה למחוק תיק.";
            if (legalCase.Payments.Any()) return "לא ניתן למחוק תיק שנרשמו בו תשלומים.";
            if (legalCase.Steps.Any(s => s.IsDone == true)) return "לא ניתן למחוק תיק שסומנו בו שלבים כבוצעים.";

            int id = legalCase.CaseId;
            foreach (CaseStep step in legalCase.Steps.ToList())
                db.CaseSteps.Remove(step);
            db.Cases.Remove(legalCase);
            AuditService.Log(db, AuditActions.CaseDeleted, "תיק", id, null);
            db.SaveChanges();
            return null;
        }

        /// <summary>
        /// Forces an UPDATE of the case row so its RowVersion is checked: if another user changed
        /// the case in the meantime, SaveChanges throws instead of silently overwriting.
        /// </summary>
        private void TouchCase(LegalCase legalCase)
        {
            if (legalCase.CaseId == 0) return;
            var entry = db.Entry(legalCase);
            if (entry.State == System.Data.Entity.EntityState.Unchanged)
                entry.Property(t => t.IsPaid).IsModified = true;
        }

        private static string Truncate(string s, int max)
        {
            return s.Length <= max ? s : s.Substring(0, max);
        }

        private ScheduledStep ToScheduled(CaseStep e)
        {
            int days;
            if (e.DurationDays.HasValue)
            {
                days = e.DurationDays.Value; // the duration when the case was opened
            }
            else
            {
                // Steps created before v1.2 have no snapshot; fall back to the catalogue.
                Procedure process = e.Procedure ?? db.Procedures.Find(e.ProcedureId);
                days = process != null ? process.DurationDays : 0;
            }
            return new ScheduledStep
            {
                Id = e.CaseStepId,
                ProcessId = e.ProcedureId,
                DurationDays = days,
                EstimatedDate = e.DueDate ?? DateTime.MaxValue.Date,
                DoneDate = e.DoneDate,
                IsDone = e.IsDone == true
            };
        }
    }
}
