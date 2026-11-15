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
    public partial class FrmAllPayments : Form
    {
        private class PaymentRow
        {
            public int IDTashlum { get; set; }
            public int IDTik { get; set; }
            public string Client { get; set; }
            public DateTime TaarichTashlum { get; set; }
            public string Kind { get; set; }
            public string EmzhaeyTashlum { get; set; }
            public decimal Schum { get; set; }
            public decimal Itra { get; set; }
            public string Status { get; set; }
            public string Teur { get; set; }
            public bool Mevutal { get; set; }
            public int? MevatelTashlum { get; set; }
        }

        private List<PaymentRow> all = new List<PaymentRow>();

        public FrmAllPayments()
        {
            InitializeComponent();
            dataGridView1.DataBindingComplete += (s, e) => GreyCancelled();
            LoadPayments();
        }

        private void LoadPayments()
        {
            try
            {
                using (var db = new LawyerDBEntities1())
                {
                    all = db.Tashlumim_Letik
                        .OrderByDescending(p => p.TaarichTashlum)
                        .ThenByDescending(p => p.IDTashlum)
                        .Select(p => new PaymentRow
                        {
                            IDTashlum = p.IDTashlum,
                            IDTik = p.IDTik,
                            Client = p.Tikim_Letipul_Orech_Din.Customers.LastName + " " + p.Tikim_Letipul_Orech_Din.Customers.FirstName,
                            TaarichTashlum = p.TaarichTashlum,
                            Kind = p.SugTashlum ?? PaymentKinds.Fee,
                            EmzhaeyTashlum = p.EmzhaeyTashlum,
                            Schum = p.Schum,
                            Itra = p.Itra,
                            Teur = p.Teur,
                            Mevutal = p.Mevutal,
                            MevatelTashlum = p.MevatelTashlum
                        })
                        .ToList();
                }
                foreach (PaymentRow r in all)
                {
                    r.Client = (r.Client ?? "").Trim();
                    r.Status = r.Mevutal ? "בוטל" : r.MevatelTashlum.HasValue ? "זיכוי לקבלה " + r.MevatelTashlum : "";
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
                : all.Where(p => p.IDTik.ToString() == text || p.IDTashlum.ToString() == text || (p.Client ?? "").Contains(text)).ToList();

            dataGridView1.DataSource = rows;
            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "IDTashlum", "קבלה" },
                { "IDTik", "תיק" },
                { "Client", "לקוח" },
                { "TaarichTashlum", "תאריך" },
                { "Kind", "סוג" },
                { "EmzhaeyTashlum", "אמצעי תשלום" },
                { "Schum", "סכום" },
                { "Itra", "יתרה" },
                { "Status", "סטטוס" },
                { "Teur", "תיאור" }
            });
            // Cancellations are negative entries, so the sum is what was really received.
            lblTotal.Text = "סה\"כ התקבל (בניכוי ביטולים): " + Ui.FormatMoney(rows.Sum(r => r.Schum));
        }

        private void GreyCancelled()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var r = row.DataBoundItem as PaymentRow;
                if (r != null && (r.Mevutal || r.MevatelTashlum.HasValue)) row.DefaultCellStyle.ForeColor = SystemColors.GrayText;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private PaymentRow Selected()
        {
            int? id = Ui.SelectedInt(dataGridView1, "IDTashlum");
            if (id == null)
            {
                Ui.Error("יש לבחור תשלום מהרשימה.");
                return null;
            }
            return all.First(r => r.IDTashlum == id.Value);
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            PaymentRow row = Selected();
            if (row == null) return;
            using (var receipt = new frmKabala(row.IDTashlum))
                receipt.ShowDialog(this);
        }

        private void btnCancelPayment_Click(object sender, EventArgs e)
        {
            PaymentRow row = Selected();
            if (row == null) return;
            if (row.Mevutal) { Ui.Error("התשלום כבר בוטל."); return; }
            if (row.MevatelTashlum.HasValue) { Ui.Error("זו רשומת זיכוי של ביטול קודם, ואותה לא ניתן לבטל."); return; }

            string reason = Ui.AskText(this, "ביטול תשלום",
                "ביטול קבלה " + row.IDTashlum + " (תיק " + row.IDTik + ") על סך " + Ui.FormatMoney(row.Schum) +
                ".\nהתשלום לא יימחק: תירשם קבלת זיכוי בסכום שלילי והיתרה בתיק תתעדכן.\nסיבת הביטול:", "", 40);
            if (reason == null) return;
            try
            {
                using (var db = new LawyerDBEntities1())
                {
                    Tikim_Letipul_Orech_Din tik = db.Tikim_Letipul_Orech_Din.First(t => t.IDTik == row.IDTik);
                    string error = new CaseService(db).CancelPayment(tik, row.IDTashlum, reason);
                    if (error != null)
                    {
                        Ui.Error(error);
                        return;
                    }
                }
                LoadPayments();
                Ui.Notify(this, "קבלה " + row.IDTashlum + " בוטלה ונרשם זיכוי.");
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
            CaseWindows.OpenCase(Owner ?? (IWin32Window)this, row.IDTik, () => { if (!IsDisposed) LoadPayments(); });
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnReceipt_Click(sender, e);
        }
    }
}
