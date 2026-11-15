using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>A printable receipt for one payment, with the VAT breakdown.</summary>
    public partial class ReceiptForm : Form
    {
        private Bitmap printImage;

        public ReceiptForm(int paymentId)
        {
            InitializeComponent();
            using (var db = new QuedinDbContext())
            {
                Payment p = db.Payments.FirstOrDefault(x => x.PaymentId == paymentId);
                if (p == null) throw new ArgumentException("Payment not found: " + paymentId);
                Customer c = p.Case.Client;

                // The rate stored with the payment, so a receipt reprinted after a VAT change shows the same numbers.
                decimal rate = p.VatRate ?? AppSettings.VatRate;
                string kind = p.Kind ?? PaymentKinds.Fee;
                VatBreakdown vat = FeeCalculator.ReceiptVat(p.Amount, kind, rate);
                bool credit = p.CancelsPaymentId.HasValue;

                string title = (credit ? "קבלת זיכוי מס' " : "קבלה מס' ") + p.PaymentId;
                if (p.IsCancelled) title += " - מבוטלת";
                lblTitle.Text = title;
                lblTitle.ForeColor = p.IsCancelled || credit ? Color.DarkRed : lblTitle.ForeColor;
                lblOffice.Text = OfficeHeader();
                lblDate.Text = Ui.FormatDate(p.PaidOn);
                lblCustomer.Text = c != null ? c.DisplayName + " (" + (c.IdType ?? IdTypes.IsraeliId) + " " + c.CustomerId + ")" : "";
                lblPhone.Text = c != null ? (c.MobilePhone ?? c.Phone ?? "") : "";
                lblCase.Text = p.CaseId.ToString();
                lblMethod.Text = p.Method + Details(p);
                lblDesc.Text = kind + (string.IsNullOrEmpty(p.Description) ? "" : " - " + p.Description);
                if (p.IsCancelled)
                {
                    Payment reversal = db.Payments.FirstOrDefault(x => x.CancelsPaymentId == p.PaymentId);
                    if (reversal != null) lblDesc.Text += " (בוטלה בקבלת זיכוי " + reversal.PaymentId + ")";
                }
                lblVatTitle.Text = kind == PaymentKinds.Expenses
                    ? "מע\"מ (אגרה - פטור):"
                    : "מע\"מ (" + (rate * 100).ToString("0.#") + "%):";
                lblTotalTitle.Text = credit ? "סה\"כ זיכוי:" : "סה\"כ שולם:";
                lblNet.Text = Ui.FormatMoney(vat.Net);
                lblVat.Text = Ui.FormatMoney(vat.Vat);
                lblTotal.Text = Ui.FormatMoney(vat.Total);
                lblBalance.Text = Ui.FormatMoney(p.BalanceAfter);
                Text = "קבלה מס' " + p.PaymentId;
            }
        }

        private static string OfficeHeader()
        {
            var line = new System.Collections.Generic.List<string>();
            if (AppSettings.OfficeAddress.Length > 0) line.Add(AppSettings.OfficeAddress);
            if (AppSettings.OfficePhone.Length > 0) line.Add("טל' " + AppSettings.OfficePhone);
            if (AppSettings.OfficeTaxId.Length > 0) line.Add("עוסק מורשה " + AppSettings.OfficeTaxId);
            return AppSettings.OfficeName + (line.Count > 0 ? Environment.NewLine + string.Join(" | ", line) : "");
        }

        private static string Details(Payment p)
        {
            if (!string.IsNullOrEmpty(p.CardLast4)) return " " + p.CardLast4;
            if (!string.IsNullOrEmpty(p.Reference)) return " (אסמכתא " + p.Reference + ")";
            if (!string.IsNullOrEmpty(p.ChequeNumber)) return " (צ'ק " + p.ChequeNumber + ", לתאריך " + Ui.FormatDate(p.ChequeDate) + ")";
            return "";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printImage = new Bitmap(panelReceipt.Width, panelReceipt.Height);
            panelReceipt.DrawToBitmap(printImage, new Rectangle(0, 0, panelReceipt.Width, panelReceipt.Height));

            using (var document = new PrintDocument())
            using (var preview = new PrintPreviewDialog())
            {
                document.DocumentName = Text;
                document.PrintPage += (s, args) =>
                {
                    Rectangle area = args.MarginBounds;
                    float scale = Math.Min((float)area.Width / printImage.Width, (float)area.Height / printImage.Height);
                    scale = Math.Min(scale, 1.5f);
                    args.Graphics.DrawImage(printImage, area.Left, area.Top, printImage.Width * scale, printImage.Height * scale);
                };
                preview.Document = document;
                preview.ShowDialog(this);
            }
            printImage.Dispose();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
