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
    public partial class frmKabala : Form
    {
        private Bitmap printImage;

        public frmKabala(int paymentId)
        {
            InitializeComponent();
            using (var db = new LawyerDBEntities1())
            {
                Tashlumim_Letik p = db.Tashlumim_Letik.FirstOrDefault(x => x.IDTashlum == paymentId);
                if (p == null) throw new ArgumentException("Payment not found: " + paymentId);
                Customers c = p.Tikim_Letipul_Orech_Din.Customers;

                // The rate stored with the payment, so a receipt reprinted after a VAT change shows the same numbers.
                decimal rate = p.VatRate ?? AppSettings.VatRate;
                string kind = p.SugTashlum ?? PaymentKinds.Fee;
                VatBreakdown vat = FeeCalculator.ReceiptVat(p.Schum, kind, rate);
                bool credit = p.MevatelTashlum.HasValue;

                string title = (credit ? "קבלת זיכוי מס' " : "קבלה מס' ") + p.IDTashlum;
                if (p.Mevutal) title += " - מבוטלת";
                lblTitle.Text = title;
                lblTitle.ForeColor = p.Mevutal || credit ? Color.DarkRed : lblTitle.ForeColor;
                lblOffice.Text = OfficeHeader();
                lblDate.Text = Ui.FormatDate(p.TaarichTashlum);
                lblCustomer.Text = c != null ? c.DisplayName + " (" + (c.SugMezahe ?? IdTypes.IsraeliId) + " " + c.IDcustemer + ")" : "";
                lblPhone.Text = c != null ? (c.SmartPhone ?? c.Telephone ?? "") : "";
                lblTik.Text = p.IDTik.ToString();
                lblMethod.Text = p.EmzhaeyTashlum + Details(p);
                lblDesc.Text = kind + (string.IsNullOrEmpty(p.Teur) ? "" : " - " + p.Teur);
                if (p.Mevutal)
                {
                    Tashlumim_Letik reversal = db.Tashlumim_Letik.FirstOrDefault(x => x.MevatelTashlum == p.IDTashlum);
                    if (reversal != null) lblDesc.Text += " (בוטלה בקבלת זיכוי " + reversal.IDTashlum + ")";
                }
                lblVatTitle.Text = kind == PaymentKinds.Expenses
                    ? "מע\"מ (אגרה - פטור):"
                    : "מע\"מ (" + (rate * 100).ToString("0.#") + "%):";
                lblTotalTitle.Text = credit ? "סה\"כ זיכוי:" : "סה\"כ שולם:";
                lblNet.Text = Ui.FormatMoney(vat.Net);
                lblVat.Text = Ui.FormatMoney(vat.Vat);
                lblTotal.Text = Ui.FormatMoney(vat.Total);
                lblBalance.Text = Ui.FormatMoney(p.Itra);
                Text = "קבלה מס' " + p.IDTashlum;
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

        private static string Details(Tashlumim_Letik p)
        {
            if (!string.IsNullOrEmpty(p.NumCartis)) return " " + p.NumCartis;
            if (!string.IsNullOrEmpty(p.NumAsmachta)) return " (אסמכתא " + p.NumAsmachta + ")";
            if (!string.IsNullOrEmpty(p.NumChek)) return " (צ'ק " + p.NumChek + ", לתאריך " + Ui.FormatDate(p.TaarichLachek) + ")";
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
