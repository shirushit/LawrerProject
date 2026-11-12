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

                VatBreakdown vat = FeeCalculator.SplitVat(p.Schum, AppSettings.VatRate);

                lblTitle.Text = "קבלה מס' " + p.IDTashlum;
                lblDate.Text = Ui.FormatDate(p.TaarichTashlum);
                lblCustomer.Text = c != null ? c.FirstName + " " + c.LastName + " (ת.ז. " + c.IDcustemer + ")" : "";
                lblPhone.Text = c != null ? c.Telephone : "";
                lblTik.Text = p.IDTik.ToString();
                lblMethod.Text = p.EmzhaeyTashlum + Details(p);
                lblDesc.Text = string.IsNullOrEmpty(p.Teur) ? "-" : p.Teur;
                lblVatTitle.Text = "מע\"מ (" + (AppSettings.VatRate * 100).ToString("0.#") + "%):";
                lblNet.Text = Ui.FormatMoney(vat.Net);
                lblVat.Text = Ui.FormatMoney(vat.Vat);
                lblTotal.Text = Ui.FormatMoney(vat.Total);
                lblBalance.Text = Ui.FormatMoney(p.Itra);
                Text = "קבלה מס' " + p.IDTashlum;
            }
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
