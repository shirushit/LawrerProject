using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>CaseForm: saving and deleting a case, and showing save errors.</summary>
    public partial class CaseForm
    {
        // ------------------------------------------------------------------ saving

        private bool ValidateAndCopy()
        {
            errorProvider1.Clear();
            bool ok = true;
            Action<Control, string> fail = (control, message) => { errorProvider1.SetError(control, message); ok = false; };

            var client = cmbClient.SelectedItem as Customer;
            var caseType = cmbCaseType.SelectedItem as CaseType;
            var opposingClient = cmbOpposingParty.SelectedItem as Customer;
            var opposingLawyer = cmbOpposingLawyer.SelectedItem as Customer;
            var court = cmbCourt.SelectedItem as Court;
            bool isCourt = rbCourt.Checked;

            if (client == null) fail(cmbClient, "יש לבחור לקוח");
            if (caseType == null) fail(cmbCaseType, "יש לבחור נושא תיק");
            if (opposingClient == null) fail(cmbOpposingParty, "יש לבחור את הצד שכנגד");
            if (opposingLawyer == null) fail(cmbOpposingLawyer, "יש לבחור עורך דין נגדי");
            if (client != null && opposingClient != null && client.CustomerId == opposingClient.CustomerId)
                fail(cmbOpposingParty, "הלקוח והצד שכנגד הם אותו אדם");
            if (chkPowerOfAttorney.Checked && Validation.IsFutureDate(dtpPowerOfAttorney.Value, Clock.Today))
                fail(dtpPowerOfAttorney, "תאריך חתימה עתידי");

            decimal? claim = null, awarded = null, propertyValue = null;
            if (isCourt)
            {
                if (court == null) fail(cmbCourt, "יש לבחור בית משפט");
                claim = ReadMoney(txtClaimAmount);
                if (claim == null || claim <= 0) fail(txtClaimAmount, "יש להזין סכום תביעה");
                if (txtAwardedAmount.Text.Trim().Length > 0)
                {
                    awarded = ReadMoney(txtAwardedAmount);
                    if (awarded == null) fail(txtAwardedAmount, "סכום לא תקין");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtPropertyAddress.Text)) fail(txtPropertyAddress, "יש להזין כתובת נכס");
                if (string.IsNullOrWhiteSpace(txtBlock.Text)) fail(txtBlock, "יש להזין גוש");
                if (string.IsNullOrWhiteSpace(txtParcel.Text)) fail(txtParcel, "יש להזין חלקה");
                propertyValue = ReadMoney(txtPropertyValue);
                if (propertyValue == null || propertyValue <= 0) fail(txtPropertyValue, "יש להזין שווי נכס");
            }

            double? percent = ReadPercent();
            if (percent == null || !Validation.IsPercent(percent.Value)) fail(txtFeePercent, "אחוז בין 0 ל-100");

            decimal advance = 0m;
            if (txtAdvance.Text.Trim().Length > 0)
            {
                decimal? m = ReadMoney(txtAdvance);
                if (m == null) fail(txtAdvance, "סכום לא תקין");
                else advance = m.Value;
            }

            if (ok && caseType != null)
            {
                decimal minimumFee, courtFee;
                CaseService.FeesOf(legalCase, caseType, out minimumFee, out courtFee);
                decimal total = FeeCalculator.CaseTotal(isCourt ? (claim ?? 0m) : (propertyValue ?? 0m), percent.Value, courtFee, minimumFee);
                if (advance > total && total > 0) fail(txtAdvance, "המקדמה גדולה מסכום התיק (" + Ui.FormatMoney(total) + ")");
                decimal paid = isNew ? 0m : CaseService.PaidTotal(legalCase);
                if (ok && paid > total && !Ui.Confirm("סכום התיק החדש (" + Ui.FormatMoney(total) + ") נמוך ממה שהלקוח כבר שילם (" +
                        Ui.FormatMoney(paid) + ").\nאחרי השמירה אפשר לרשום החזר של העודף בלשונית התשלומים. להמשיך?"))
                    return false;
            }

            if (!ok) return false;

            if (client != null && caseType != null)
            {
                LegalCase duplicate = new CaseService(db).FindOpenDuplicate(client.CustomerId, caseType.CaseTypeId, isNew ? 0 : legalCase.CaseId);
                if (duplicate != null && (isNew || legalCase.ClientId != client.CustomerId || legalCase.CaseTypeId != caseType.CaseTypeId) &&
                    !Ui.Confirm("ללקוח זה כבר יש תיק פתוח מאותו נושא (תיק מספר " + duplicate.CaseId + "). לפתוח תיק נוסף?"))
                    return false;
            }

            legalCase.ClientId = client.CustomerId;
            legalCase.CaseTypeId = caseType.CaseTypeId;
            legalCase.OpposingPartyId = opposingClient.CustomerId;
            legalCase.OpposingLawyerId = opposingLawyer.CustomerId;
            legalCase.PowerOfAttorney = chkPowerOfAttorney.Checked ? SignedPrefix + dtpPowerOfAttorney.Value.ToString("dd/MM/yyyy") : "";
            legalCase.IsInCourt = isCourt;
            legalCase.CourtId = isCourt ? court.CourtId : (int?)null;
            legalCase.ClaimAmount = isCourt ? claim : propertyValue;
            legalCase.AwardedAmount = isCourt ? awarded : null;
            legalCase.PropertyAddress = isCourt ? null : txtPropertyAddress.Text.Trim();
            legalCase.Block = isCourt ? null : txtBlock.Text.Trim();
            legalCase.Parcel = isCourt ? null : txtParcel.Text.Trim();
            legalCase.SubParcel = isCourt || txtSubParcel.Text.Trim().Length == 0 ? null : txtSubParcel.Text.Trim();
            legalCase.FeePercent = percent;
            legalCase.Advance = advance;
            CaseService.RefreshPaymentFlags(legalCase); // never typed by hand
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateAndCopy()) return;
            try
            {
                bool wasNew = isNew;
                // Business rules, the timeline of a new case and the audit entry live in the service.
                string error = new CaseService(db).SaveCase(legalCase, cmbCaseType.SelectedItem as CaseType, isNew, Clock.Today);
                if (error != null)
                {
                    Ui.Error(error);
                    return;
                }
                dirty = false;
                LoadCase(legalCase.CaseId);
                Ui.Notify(this, wasNew ? "התיק נפתח. מספר תיק: " + legalCase.CaseId : "התיק עודכן.");
                Changed = true;
                WarnAboutConflicts();
            }
            catch (Exception ex)
            {
                HandleError("התיק לא נשמר.", ex);
            }
        }

        /// <summary>The number of the case shown (0 while a new case is not saved yet).</summary>
        public int CaseId
        {
            get { return isNew || legalCase == null ? 0 : legalCase.CaseId; }
        }

        /// <summary>True after anything was saved, so the list that opened this window can refresh.</summary>
        public bool Changed { get; private set; }

        /// <summary>
        /// Shows the error; on a concurrency conflict (another user changed this case) reloads the case
        /// so the user sees the current data instead of keeping a stale copy.
        /// </summary>
        private void HandleError(string message, Exception ex)
        {
            bool conflict = ex is System.Data.Entity.Infrastructure.DbUpdateConcurrencyException || ex is System.Data.Entity.Infrastructure.DbUpdateException;
            if (!isNew && conflict)
            {
                bool exists;
                try
                {
                    using (var check = new QuedinDbContext())
                        exists = new CaseService(check).CaseExists(legalCase.CaseId);
                }
                catch (Exception)
                {
                    exists = true; // cannot tell; show the original error
                }
                if (!exists)
                {
                    CaseDeleted();
                    return;
                }
            }
            Ui.Error(message, ex);
            if (!isNew && conflict)
            {
                dirty = false;
                LoadCase(legalCase.CaseId);
            }
        }

        /// <summary>Warns when a calculated step now falls after a court-set date (the office must decide what to do).</summary>
        private void WarnAboutConflicts()
        {
            if (isNew) return;
            List<CaseStep> conflicts = new CaseService(db).ScheduleConflicts(legalCase);
            if (conflicts.Count == 0) return;
            Ui.Error("שימו לב: לפי החישוב, השלבים הבאים יגיעו אחרי מועד שקבע בית המשפט:\n" +
                     string.Join("\n", conflicts.Select(c => "• " + StepName(c) + " - " + Ui.FormatDate(c.DueDate))) +
                     "\nכדאי לקבוע להם מועד ידני או לבקש דחייה.");
        }

        /// <summary>Runs a case operation, shows its error or a short confirmation and reloads the case.</summary>
        private void RunOperation(Func<CaseService, string> operation, string done, TabPage tab)
        {
            try
            {
                string error = operation(new CaseService(db));
                if (error != null)
                {
                    Ui.Error(error);
                    LoadCase(legalCase.CaseId);
                }
                else
                {
                    Changed = true;
                    LoadCase(legalCase.CaseId);
                    Ui.Notify(this, done);
                    WarnAboutConflicts();
                }
                tabsCase.SelectedTab = tab;
            }
            catch (Exception ex)
            {
                HandleError("הפעולה לא נשמרה.", ex);
                tabsCase.SelectedTab = tab;
            }
        }

        private void btnDeleteCase_Click(object sender, EventArgs e)
        {
            if (!Ui.Confirm("למחוק את תיק מספר " + legalCase.CaseId + "?\nמחיקה אפשרית רק לתיק שנפתח בטעות - לפני תשלומים ולפני שסומן בו שלב. לא ניתן לשחזר.")) return;
            try
            {
                string error = new CaseService(db).DeleteCase(legalCase);
                if (error != null) { Ui.Error(error); return; }
                Changed = true;
                Close();
            }
            catch (Exception ex)
            {
                HandleError("התיק לא נמחק.", ex);
            }
        }
    }
}
