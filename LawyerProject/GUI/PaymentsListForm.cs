using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>All payments of all cases: receipts, cancellations (credit notes) and a jump to the case.</summary>
    public partial class PaymentsListForm : Form
    {
        private class PaymentRow
        {
            public int PaymentId { get; set; }
            public int CaseId { get; set; }
            public string Client { get; set; }
            public DateTime PaidOn { get; set; }
            public string Kind { get; set; }
            public string Method { get; set; }
            public decimal Amount { get; set; }
            public decimal BalanceAfter { get; set; }
            public string Status { get; set; }
            public string Description { get; set; }
            public bool IsCancelled { get; set; }
            public int? CancelsPaymentId { get; set; }
        }

        private List<PaymentRow> all = new List<PaymentRow>();

        public PaymentsListForm()
        {
            InitializeComponent();
            gridPayments.DataBindingComplete += (s, e) => GreyCancelled();
            LoadPayments();
        }

        private void LoadPayments()
        {
            try
            {
                using (var db = new QuedinDbContext())
                {
                    all = db.Payments
                        .OrderByDescending(p => p.PaidOn)
                        .ThenByDescending(p => p.PaymentId)
                        .Select(p => new PaymentRow
                        {
                            PaymentId = p.PaymentId,
                            CaseId = p.CaseId,
                            Client = p.Case.Client.LastName + " " + p.Case.Client.FirstName,
                            PaidOn = p.PaidOn,
                            Kind = p.Kind ?? PaymentKinds.Fee,
                            Method = p.Method,
                            Amount = p.Amount,
                            BalanceAfter = p.BalanceAfter,
                            Description = p.Description,
                            IsCancelled = p.IsCancelled,
                            CancelsPaymentId = p.CancelsPaymentId
                        })
                        .ToList();
                }
                foreach (PaymentRow r in all)
                {
                    r.Client = (r.Client ?? "").Trim();
                    r.Status = r.IsCancelled ? "בוטל" : r.CancelsPaymentId.HasValue ? "זיכוי לקבלה " + r.CancelsPaymentId : "";
                }
            }
            catch (Exception ex)
            {
                Ui.Error("לא ניתן לטעון את התשלומים.", ex);
            }
            ApplyFilter();
        }

        // Always filters the full list, so deleting characters widens the results again.
        private void ApplyFilter()
        {
            string text = txtSearch.Text.Trim();
            List<PaymentRow> rows = text.Length == 0
                ? all
                : all.Where(p => p.CaseId.ToString() == text || p.PaymentId.ToString() == text || (p.Client ?? "").Contains(text)).ToList();

            gridPayments.DataSource = rows;
            Ui.SetHeaders(gridPayments, new Dictionary<string, string>
            {
                { "PaymentId", "קבלה" },
                { "CaseId", "תיק" },
                { "Client", "לקוח" },
                { "PaidOn", "תאריך" },
                { "Kind", "סוג" },
                { "Method", "אמצעי תשלום" },
                { "Amount", "סכום" },
                { "BalanceAfter", "יתרה" },
                { "Status", "סטטוס" },
                { "Description", "תיאור" }
            });
            // Cancellations are negative entries, so the sum is what was really received.
            lblTotal.Text = "סה\"כ התקבל (בניכוי ביטולים): " + Ui.FormatMoney(rows.Sum(r => r.Amount));
        }

        private void GreyCancelled()
        {
            foreach (DataGridViewRow row in gridPayments.Rows)
            {
                var r = row.DataBoundItem as PaymentRow;
                if (r != null && (r.IsCancelled || r.CancelsPaymentId.HasValue)) row.DefaultCellStyle.ForeColor = SystemColors.GrayText;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private PaymentRow Selected()
        {
            int? id = Ui.SelectedInt(gridPayments, "PaymentId");
            if (id == null)
            {
                Ui.Error("יש לבחור תשלום מהרשימה.");
                return null;
            }
            return all.First(r => r.PaymentId == id.Value);
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            PaymentRow row = Selected();
            if (row == null) return;
            using (var receipt = new ReceiptForm(row.PaymentId))
                receipt.ShowDialog(this);
        }

        private void btnCancelPayment_Click(object sender, EventArgs e)
        {
            PaymentRow row = Selected();
            if (row == null) return;
            if (row.IsCancelled) { Ui.Error("התשלום כבר בוטל."); return; }
            if (row.CancelsPaymentId.HasValue) { Ui.Error("זו רשומת זיכוי של ביטול קודם, ואותה לא ניתן לבטל."); return; }

            string reason = Ui.AskText(this, "ביטול תשלום",
                "ביטול קבלה " + row.PaymentId + " (תיק " + row.CaseId + ") על סך " + Ui.FormatMoney(row.Amount) +
                ".\nהתשלום לא יימחק: תירשם קבלת זיכוי בסכום שלילי והיתרה בתיק תתעדכן.\nסיבת הביטול:", "", 40);
            if (reason == null) return;
            try
            {
                using (var db = new QuedinDbContext())
                {
                    LegalCase legalCase = db.Cases.First(t => t.CaseId == row.CaseId);
                    string error = new CaseService(db).CancelPayment(legalCase, row.PaymentId, reason);
                    if (error != null)
                    {
                        Ui.Error(error);
                        return;
                    }
                }
                LoadPayments();
                Ui.Notify(this, "קבלה " + row.PaymentId + " בוטלה ונרשם זיכוי.");
            }
            catch (Exception ex)
            {
                Ui.Error("התשלום לא בוטל.", ex);
                LoadPayments();
            }
        }

        private void btnOpenCase_Click(object sender, EventArgs e)
        {
            PaymentRow row = Selected();
            if (row == null) return;
            CaseWindows.OpenCase(Owner ?? (IWin32Window)this, row.CaseId, () => { if (!IsDisposed) LoadPayments(); });
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnReceipt_Click(sender, e);
        }
    }
}
