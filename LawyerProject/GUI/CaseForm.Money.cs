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
    /// <summary>CaseForm: the fees panel and the payments tab.</summary>
    public partial class CaseForm
    {
        // ------------------------------------------------------------------ UI state

        private void UpdateMode()
        {
            bool court = rbCourt.Checked;
            groupBoxCourt.Visible = court;
            groupBoxDeal.Visible = !court;

            // The fixed steps come from the case type, so it is chosen once, when the case is opened.
            cmbCaseType.Enabled = isNew;
            lblCaseTypeLock.Visible = !isNew;
            tabsCase.Enabled = !isNew;
            lblSaveHint.Visible = isNew;
            btnDeleteCase.Visible = !isNew && Session.IsManager;
            if (isNew) Text = court ? "פתיחת תיק חדש" : "עסקת נכס חדשה";
            UpdateMoney();
        }

        private decimal? ReadMoney(TextBox box)
        {
            decimal value;
            return Validation.TryParseMoney(box.Text, out value) ? value : (decimal?)null;
        }

        private double? ReadPercent()
        {
            double value;
            return double.TryParse(txtFeePercent.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out value)
                ? value : (double?)null;
        }

        /// <summary>Recalculates the case total and the open balance shown on screen.</summary>
        private void UpdateMoney()
        {
            if (loading) return;
            decimal? amount = ReadMoney(rbCourt.Checked ? txtClaimAmount : txtPropertyValue);
            double? percent = ReadPercent();
            var caseType = cmbCaseType.SelectedItem as CaseType;

            if (amount == null || percent == null || !Validation.IsPercent(percent.Value))
            {
                lblTotal.Text = "-";
                lblBalance.Text = "-";
                return;
            }

            decimal minimumFee, courtFee;
            CaseService.FeesOf(legalCase, caseType, out minimumFee, out courtFee);
            decimal total = FeeCalculator.CaseTotal(amount.Value, percent.Value, courtFee, minimumFee);
            decimal paid = isNew ? 0m : CaseService.PaidTotal(legalCase);
            decimal balance = FeeCalculator.Balance(total, new[] { paid });
            lblTotal.Text = Ui.FormatMoney(total);
            lblPaid.Text = Ui.FormatMoney(paid);
            lblBalance.Text = balance < 0 ? Ui.FormatMoney(balance) + "  (שולם יותר מהסכום)" : Ui.FormatMoney(balance);
            lblBalance.ForeColor = balance <= 0 && total > 0 ? Color.DarkGreen : SystemColors.ControlText;

            decimal? advance = txtAdvance.Text.Trim().Length == 0 ? 0m : ReadMoney(txtAdvance);
            lblAdvanceStatus.Text = advance == null || advance == 0 ? "" : (paid >= advance ? "שולמה" : "טרם שולמה");
        }

        private bool reverting;

        private void CaseType_CheckedChanged(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;
            if (!rb.Checked || loading || reverting) { if (rb.Checked) UpdateMode(); return; }

            // Switching hides the other group; warn when it already holds data that will not be saved.
            bool toCourt = rb == rbCourt;
            bool hiddenHasData = toCourt
                ? new[] { txtPropertyAddress, txtBlock, txtParcel, txtSubParcel, txtPropertyValue }.Any(t => t.Text.Trim().Length > 0)
                : cmbCourt.SelectedItem != null || new[] { txtClaimAmount, txtAwardedAmount }.Any(t => t.Text.Trim().Length > 0);
            if (hiddenHasData && !Ui.Confirm(toCourt
                    ? "פרטי העסקה שהוזנו (כתובת, גוש, חלקה ושווי) לא יישמרו בתיק בית משפט. להמשיך?"
                    : "פרטי בית המשפט שהוזנו (בית משפט וסכומים) לא יישמרו בעסקת נכס. להמשיך?"))
            {
                reverting = true;
                (toCourt ? rbDeal : rbCourt).Checked = true;
                reverting = false;
                return;
            }
            UpdateMode();
        }

        private void cmbCaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            LoadCourts();
            UpdateMoney();
        }

        private void Money_TextChanged(object sender, EventArgs e)
        {
            UpdateMoney();
        }

        private void cmbOpposingParty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading) ShowOpposingDetails();
        }

        private void Money_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsMoneyKey(e.KeyChar);
        }

        private void Registry_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsRegistryKey(e.KeyChar);
        }


        private void FillPayments()
        {
            listPayments.Items.Clear();
            foreach (Payment p in legalCase.Payments.OrderBy(p => p.PaymentId))
            {
                string status = p.IsCancelled ? "בוטל" : (p.CancelsPaymentId.HasValue ? "זיכוי" : "");
                var item = new ListViewItem(new[]
                {
                    p.PaymentId.ToString(),
                    Ui.FormatDate(p.PaidOn),
                    p.Kind ?? PaymentKinds.Fee,
                    p.Method,
                    Ui.FormatMoney(p.Amount),
                    Ui.FormatMoney(p.BalanceAfter),
                    status,
                    p.Description
                });
                item.Tag = p.PaymentId;
                if (p.IsCancelled || p.CancelsPaymentId.HasValue) item.ForeColor = SystemColors.GrayText;
                listPayments.Items.Add(item);
            }
            Ui.FitColumns(listPayments);
            btnReceipt.Enabled = listPayments.Items.Count > 0;
            btnCancelPayment.Enabled = listPayments.Items.Count > 0;
            btnRefund.Visible = !isNew && CaseService.OpenBalance(legalCase) < 0;
        }


        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            using (var form = new PaymentForm(legalCase.CaseId))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Changed = true;
                    Ui.Notify(this, "התשלום נרשם. קבלה מספר " + form.SavedPaymentId + ".");
                }
            }
            LoadCase(legalCase.CaseId);
            tabsCase.SelectedTab = tabPayments;
        }

        private void btnCancelPayment_Click(object sender, EventArgs e)
        {
            if (listPayments.SelectedItems.Count == 0)
            {
                Ui.Error("יש לבחור תשלום מהרשימה.");
                return;
            }
            int id = (int)listPayments.SelectedItems[0].Tag;
            Payment p = legalCase.Payments.First(x => x.PaymentId == id);
            if (p.IsCancelled) { Ui.Error("התשלום כבר בוטל."); return; }
            if (p.CancelsPaymentId.HasValue) { Ui.Error("זו רשומת זיכוי של ביטול קודם, ואותה לא ניתן לבטל."); return; }

            string reason = Ui.AskText(this, "ביטול תשלום",
                "ביטול קבלה " + p.PaymentId + " על סך " + Ui.FormatMoney(p.Amount) + ".\nהתשלום לא יימחק: תירשם קבלת זיכוי בסכום שלילי והיתרה תתעדכן.\nסיבת הביטול:",
                "", 40);
            if (reason == null) return;
            RunOperation(svc => svc.CancelPayment(legalCase, id, reason), "התשלום בוטל ונרשם זיכוי.", tabPayments);
            Payment cancelled = legalCase.Payments.FirstOrDefault(x => x.PaymentId == id);
            if (cancelled != null && cancelled.IsCancelled && Ui.Confirm("לרשום עכשיו את התשלום הנכון במקום זה שבוטל?"))
                btnAddPayment_Click(sender, e);
        }

        private void btnRefund_Click(object sender, EventArgs e)
        {
            decimal overpaid = -CaseService.OpenBalance(legalCase);
            if (overpaid <= 0) return;
            string method = Ui.AskChoice(this, "החזר עודף ללקוח",
                "הלקוח שילם " + Ui.FormatMoney(overpaid) + " מעבר לסכום התיק.\nתירשם קבלת זיכוי על הסכום. באיזה אמצעי מוחזר הכסף?",
                PaymentMethods.All, PaymentMethods.BankTransfer);
            if (method == null) return;
            RunOperation(svc => svc.RefundOverpayment(legalCase, method, null), "נרשם החזר של " + Ui.FormatMoney(overpaid) + " ללקוח.", tabPayments);
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            if (listPayments.SelectedItems.Count == 0)
            {
                Ui.Error("יש לבחור תשלום מהרשימה.");
                return;
            }
            using (var form = new ReceiptForm((int)listPayments.SelectedItems[0].Tag))
                form.ShowDialog(this);
        }
    }
}
