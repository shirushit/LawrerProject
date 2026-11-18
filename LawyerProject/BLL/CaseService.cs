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

        /// <summary>A step date further than this from today is surely a typo (and the Hebrew calendar ends in 2239).</summary>
        public const int MaxYearsAway = 20;

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
                    ProcedureId = k.ProcedureId,
                    Level = k.StepOrder,
                    DurationDays = k.Procedure.DurationDays
                })
                .ToList();

            if (templates.Count == 0)
                return "לא הוגדרו תהליכים קבועים לנושא התיק שנבחר. מנהל/ת המשרד מגדיר/ה אותם במסך \"נושאי תיקים\".";

            List<ScheduledStep> plan = CaseScheduler.BuildPlan(templates, openedOn);
            foreach (ScheduledStep step in plan)
            {
                legalCase.Steps.Add(new CaseStep
                {
                    ProcedureId = step.ProcedureId,
                    DueDate = step.EstimatedDate,
                    DoneDate = null,
                    IsDone = false,
                    DurationDays = step.DurationDays
                });
            }
            legalCase.CurrentProcedureId = plan[0].ProcedureId;
            return null;
        }

        /// <summary>Marks the next open step as done on the given date (not in the future) and pushes later dates.</summary>
        public string CompleteStep(LegalCase legalCase, int stepId, DateTime doneOn)
        {
            if (Validation.IsFutureDate(doneOn, Clock.Today)) return "לא ניתן לסמן שלב כבוצע בתאריך עתידי.";
            return ApplyToTimeline(legalCase, stepId, (steps, target) => CaseScheduler.CompleteStep(steps, target, doneOn),
                AuditActions.StepCompleted, "בוצע ב-" + doneOn.ToString("dd/MM/yyyy"));
        }

        /// <summary>Sets an open step to a date decided outside the office (e.g. by the court) and pushes later dates.</summary>
        public string RescheduleStep(LegalCase legalCase, int stepId, DateTime newDate, string note)
        {
            if (Math.Abs((newDate.Date - Clock.Today).TotalDays) > MaxYearsAway * 366)
                return "המועד רחוק מדי (יותר מ-" + MaxYearsAway + " שנים). ייתכן שנפלה טעות בהקלדה.";
            string error = ApplyToTimeline(legalCase, stepId, (steps, target) => CaseScheduler.Reschedule(steps, target, newDate),
                AuditActions.StepRescheduled, "מועד חדש " + newDate.ToString("dd/MM/yyyy"), save: false);
            if (error != null) return error;
            if (!string.IsNullOrWhiteSpace(note))
                SetNoteNoSave(legalCase, stepId, note);
            ChangeDiscarder.SaveOrDiscard(db);
            return null;
        }

        /// <summary>Re-opens the most recently completed step (managers only).</summary>
        public string UndoLastStep(LegalCase legalCase)
        {
            if (!Session.IsManager) return "רק מנהל/ת יכול/ה לבטל סימון של שלב.";
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
            ChangeDiscarder.SaveOrDiscard(db);
            return null;
        }

        /// <summary>Saves a free-text note on a step (for example "postponed at the other side's request").</summary>
        public string SetNote(LegalCase legalCase, int stepId, string note)
        {
            string error = SetNoteNoSave(legalCase, stepId, note);
            if (error != null) return error;
            ChangeDiscarder.SaveOrDiscard(db);
            return null;
        }

        private string SetNoteNoSave(LegalCase legalCase, int stepId, string note)
        {
            CaseStep step = legalCase.Steps.FirstOrDefault(s => s.CaseStepId == stepId);
            if (step == null) return "התהליך שנבחר אינו שייך לתיק.";
            string text = (note ?? string.Empty).Trim();
            step.Notes = text.Length == 0 ? null : (text.Length > ColumnLengths.StepNotes ? text.Substring(0, ColumnLengths.StepNotes) : text);
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
            if (save) ChangeDiscarder.SaveOrDiscard(db);
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
                e.IsFixedDate = s.IsFixed;
            }
            ScheduledStep next = CaseScheduler.NextPending(map.Values);
            ScheduledStep last = CaseScheduler.Ordered(map.Values).LastOrDefault();
            legalCase.CurrentProcedureId = next != null ? next.ProcedureId : (last != null ? last.ProcedureId : legalCase.CurrentProcedureId);
            TouchCase(legalCase);
        }

        /// <summary>
        /// Open steps whose calculated date is after a later court-set date (for example evidence due after
        /// the hearing). Shown to the user as a warning; nothing is changed automatically.
        /// </summary>
        public List<CaseStep> ScheduleConflicts(LegalCase legalCase)
        {
            List<CaseStep> entities = legalCase.Steps.ToList();
            Dictionary<int, ScheduledStep> map = entities.ToDictionary(e => e.CaseStepId, ToScheduled);
            HashSet<int> ids = new HashSet<int>(CaseScheduler.Conflicts(map.Values).Select(s => s.Id));
            return entities.Where(e => ids.Contains(e.CaseStepId)).OrderBy(e => e.CaseStepId).ToList();
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
                .Include("Client").Include("Steps.Procedure") // one query instead of one per case
                .Where(t => t.Steps.Any(s => s.IsDone != true && s.DueDate <= limit))
                .ToList();

            var items = new List<DueItem>();
            foreach (LegalCase t in cases)
            {
                CaseStep next = OrderedSteps(t).FirstOrDefault(s => s.IsDone != true);
                if (next == null || CaseScheduler.UrgencyOf(next.DueDate, false, today, days) == Urgency.None) continue;
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

        /// <summary>
        /// Total the office charges: agreed % of the claim (or property value), at least the case type's
        /// minimum fee, plus the case type's court fee.
        /// </summary>
        public static decimal CaseTotal(LegalCase legalCase)
        {
            decimal minimumFee, courtFee;
            FeesOf(legalCase, legalCase.CaseType, out minimumFee, out courtFee);
            double percent = Math.Min(100, Math.Max(0, legalCase.FeePercent ?? 0d));
            return FeeCalculator.CaseTotal(Math.Max(0m, legalCase.ClaimAmount ?? 0m), percent, courtFee, minimumFee);
        }

        /// <summary>
        /// The minimum fee and court fee that apply to a case: the ones stored when it was opened (or when
        /// its case type was changed), so editing the catalogue later does not change old totals. A case
        /// being opened, or moved to another case type, takes the fees of <paramref name="caseType"/>.
        /// </summary>
        public static void FeesOf(LegalCase legalCase, CaseType caseType, out decimal minimumFee, out decimal courtFee)
        {
            bool sameType = caseType == null || legalCase.CaseTypeId == caseType.CaseTypeId;
            if (sameType && legalCase.CourtFee.HasValue)
            {
                minimumFee = Math.Max(0m, legalCase.MinimumFee ?? 0m);
                courtFee = Math.Max(0m, legalCase.CourtFee.Value);
                return;
            }
            minimumFee = caseType != null ? Math.Max(0m, caseType.BaseFee ?? 0m) : 0m;
            courtFee = caseType != null ? Math.Max(0m, caseType.CourtFee ?? 0m) : 0m;
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
            if (!PaymentMethods.All.Contains(payment.Method)) return "יש לבחור אמצעי תשלום.";

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
            ChangeDiscarder.SaveOrDiscard(db);
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

            var reversal = new Payment
            {
                CaseId = legalCase.CaseId,
                Amount = -original.Amount,
                Kind = original.Kind,
                VatRate = original.VatRate,
                Method = original.Method,
                PaidOn = Clock.Today,
                Description = Truncate("ביטול קבלה " + original.PaymentId + (string.IsNullOrWhiteSpace(reason) ? "" : ": " + reason.Trim()), ColumnLengths.PaymentDescription),
                CancelsPaymentId = original.PaymentId,
                IsCancelled = false
            };
            legalCase.Payments.Add(reversal);
            reversal.BalanceAfter = OpenBalance(legalCase);
            RefreshPaymentFlags(legalCase);
            TouchCase(legalCase);
            AuditService.Log(db, AuditActions.PaymentCancelled, "תשלום", original.PaymentId, reason);

            // Two saves in one transaction: the credit note first, then the "cancelled" mark. The database
            // refuses the mark when no credit note exists, so nobody can cancel a receipt without one.
            InTransaction(() =>
            {
                db.SaveChanges();
                original.IsCancelled = true;
                db.SaveChanges();
            }, () =>
            {
                original.IsCancelled = false;
                if (db.Entry(reversal).State != System.Data.Entity.EntityState.Detached)
                    db.Entry(reversal).State = System.Data.Entity.EntityState.Detached;
                legalCase.Payments.Remove(reversal);
            });
            return null;
        }

        /// <summary>
        /// Records money returned to a client who paid more than the case total (for example after the
        /// claim amount was lowered). The refund is a negative entry, like a cancellation.
        /// </summary>
        public string RefundOverpayment(LegalCase legalCase, string method, string reason)
        {
            decimal overpaid = FeeCalculator.Overpaid(CaseTotal(legalCase), legalCase.Payments.Select(p => p.Amount));
            if (overpaid <= 0) return "אין עודף להחזיר בתיק זה.";
            if (!PaymentMethods.All.Contains(method)) return "יש לבחור אמצעי תשלום.";

            var refund = new Payment
            {
                CaseId = legalCase.CaseId,
                Amount = -overpaid,
                Kind = PaymentKinds.Refund,
                VatRate = AppSettings.VatRate,
                Method = method,
                PaidOn = Clock.Today,
                Description = Truncate("החזר עודף ללקוח" + (string.IsNullOrWhiteSpace(reason) ? "" : ": " + reason.Trim()), ColumnLengths.PaymentDescription),
                IsCancelled = false,
                BalanceAfter = 0m
            };
            legalCase.Payments.Add(refund);
            RefreshPaymentFlags(legalCase);
            TouchCase(legalCase);
            AuditService.Log(db, AuditActions.Refund, "תיק", legalCase.CaseId, overpaid.ToString("0.00") + " " + method);
            ChangeDiscarder.SaveOrDiscard(db);
            return null;
        }

        // ------------------------------------------------------------------ case lifecycle

        /// <summary>
        /// The rules every case must keep, whatever screen saves it. Returns null when the case is valid
        /// or a Hebrew message. <paramref name="caseType"/> is the selected case type (for its court fee).
        /// </summary>
        public static string CheckCase(LegalCase c, CaseType caseType)
        {
            if (string.IsNullOrWhiteSpace(c.ClientId)) return "יש לבחור לקוח.";
            if (caseType == null || c.CaseTypeId != caseType.CaseTypeId) return "יש לבחור נושא תיק.";
            if (string.IsNullOrWhiteSpace(c.OpposingPartyId)) return "יש לבחור את הצד שכנגד.";
            if (string.IsNullOrWhiteSpace(c.OpposingLawyerId)) return "יש לבחור עורך דין נגדי.";
            if (c.ClientId == c.OpposingPartyId) return "הלקוח והצד שכנגד הם אותו אדם.";

            decimal amount = c.ClaimAmount ?? 0m;
            if (amount <= 0m || !Validation.IsMoneyInRange(amount))
                return c.IsInCourt == true ? "סכום התביעה חייב להיות בין 0.01 ל-1,000,000,000 ₪." : "שווי הנכס חייב להיות בין 0.01 ל-1,000,000,000 ₪.";
            if (c.IsInCourt == true)
            {
                if (!c.CourtId.HasValue) return "יש לבחור בית משפט.";
                if (c.AwardedAmount.HasValue && !Validation.IsMoneyInRange(c.AwardedAmount.Value)) return "הסכום שנפסק אינו תקין.";
            }
            else if (string.IsNullOrWhiteSpace(c.PropertyAddress) || string.IsNullOrWhiteSpace(c.Block) || string.IsNullOrWhiteSpace(c.Parcel))
            {
                return "בעסקת נכס יש להזין כתובת, גוש וחלקה.";
            }

            if (!c.FeePercent.HasValue || !Validation.IsPercent(c.FeePercent.Value)) return "אחוז שכר הטרחה חייב להיות בין 0 ל-100.";
            decimal advance = c.Advance;
            if (!Validation.IsMoneyInRange(advance)) return "המקדמה אינה תקינה.";
            decimal minimumFee, courtFee;
            FeesOf(c, caseType, out minimumFee, out courtFee);
            decimal total = FeeCalculator.CaseTotal(amount, c.FeePercent.Value, courtFee, minimumFee);
            if (total > 0m && advance > total) return "המקדמה גדולה מסכום התיק (" + total.ToString("#,##0.00") + " ₪).";
            return null;
        }

        /// <summary>
        /// Checks and saves a case. A new case gets its timeline from the case type. The audit entry is
        /// written after the insert (so it carries the new case number), in the same transaction.
        /// If anything fails nothing is saved and the case can be saved again without duplicate steps.
        /// </summary>
        public string SaveCase(LegalCase legalCase, CaseType caseType, bool isNew, DateTime today)
        {
            string error = CheckCase(legalCase, caseType);
            if (error != null)
            {
                if (!isNew) ChangeDiscarder.DiscardChanges(db); // do not leave invalid values for the next save
                return error;
            }

            // Keep the case type's fees with the case (new case, or a case moved to another type).
            if (isNew || !legalCase.CourtFee.HasValue || db.Entry(legalCase).Property(c => c.CaseTypeId).IsModified)
            {
                legalCase.MinimumFee = caseType.BaseFee ?? 0m;
                legalCase.CourtFee = caseType.CourtFee ?? 0m;
            }

            if (isNew)
            {
                error = CreatePlan(legalCase, today);
                if (error != null)
                {
                    legalCase.Steps.Clear();
                    return error;
                }
                db.Cases.Add(legalCase);
            }

            // The case and its audit entry (which needs the new case number) are saved together.
            InTransaction(() =>
            {
                db.SaveChanges();
                AuditService.Log(db, isNew ? AuditActions.CaseOpened : AuditActions.CaseUpdated, "תיק", legalCase.CaseId, null);
                db.SaveChanges();
            }, () =>
            {
                if (isNew) ForgetNewCase(legalCase);
            });
            return null;
        }

        /// <summary>
        /// Runs several saves as one transaction. On any error the transaction is rolled back, the context
        /// is put back to the database state and <paramref name="forget"/> undoes what had already been
        /// saved inside the failed transaction. Under an outer TransactionScope (the integration tests)
        /// that scope is the transaction.
        /// </summary>
        private void InTransaction(Action work, Action forget)
        {
            System.Data.Entity.DbContextTransaction transaction =
                System.Transactions.Transaction.Current == null ? db.Database.BeginTransaction() : null;
            try
            {
                work();
                if (transaction != null) transaction.Commit();
            }
            catch
            {
                if (transaction != null) transaction.Rollback();
                ChangeDiscarder.DiscardChanges(db);
                forget();
                throw;
            }
            finally
            {
                if (transaction != null) transaction.Dispose();
            }
        }

        /// <summary>After a failed insert: takes the case and its steps out of the context, as if never added.</summary>
        private void ForgetNewCase(LegalCase legalCase)
        {
            foreach (CaseStep step in legalCase.Steps.ToList())
                db.Entry(step).State = System.Data.Entity.EntityState.Detached;
            db.Entry(legalCase).State = System.Data.Entity.EntityState.Detached;
            legalCase.Steps.Clear();
            legalCase.CaseId = 0;
        }

        /// <summary>False when the case was deleted (for example by another user while it was open).</summary>
        public bool CaseExists(int caseId)
        {
            return db.Cases.Any(c => c.CaseId == caseId);
        }

        /// <summary>The case's current row version in the database, or null when it no longer exists.</summary>
        public byte[] CurrentVersion(int caseId)
        {
            return db.Cases.Where(c => c.CaseId == caseId).Select(c => c.RowVersion).FirstOrDefault();
        }

        /// <summary>An open case (not all steps done) of the same client and case type, other than <paramref name="exceptCaseId"/>.</summary>
        public LegalCase FindOpenDuplicate(string clientId, int caseTypeId, int exceptCaseId)
        {
            return db.Cases
                .Where(c => c.ClientId == clientId && c.CaseTypeId == caseTypeId && c.CaseId != exceptCaseId)
                .Where(c => c.Steps.Any(s => s.IsDone != true))
                .OrderBy(c => c.CaseId)
                .FirstOrDefault();
        }

        /// <summary>A case opened by mistake can be deleted by a manager while nothing was paid or completed.</summary>
        public string DeleteCase(LegalCase legalCase)
        {
            if (!Session.IsManager) return "רק מנהל/ת יכול/ה למחוק תיק.";
            if (legalCase.Payments.Any()) return "לא ניתן למחוק תיק שנרשמו בו תשלומים.";
            if (legalCase.Steps.Any(s => s.IsDone == true)) return "לא ניתן למחוק תיק שסומנו בו שלבים כבוצעים.";

            int id = legalCase.CaseId;
            foreach (CaseStep step in legalCase.Steps.ToList())
                db.CaseSteps.Remove(step);
            db.Cases.Remove(legalCase);
            AuditService.Log(db, AuditActions.CaseDeleted, "תיק", id, null);
            ChangeDiscarder.SaveOrDiscard(db);
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
                Procedure procedure = e.Procedure ?? db.Procedures.Find(e.ProcedureId);
                days = procedure != null ? procedure.DurationDays : 0;
                e.DurationDays = days; // keep it from now on, so a later catalogue change does not move this case
            }
            return new ScheduledStep
            {
                Id = e.CaseStepId,
                ProcedureId = e.ProcedureId,
                DurationDays = days,
                EstimatedDate = e.DueDate ?? Clock.Today, // never empty in practice; MaxValue would overflow when dates are pushed
                DoneDate = e.DoneDate,
                IsDone = e.IsDone == true,
                IsFixed = e.IsFixedDate
            };
        }
    }
}
