using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>All payments of all cases, with a receipt for each one.</summary>
    public partial class FrmAllPayments : Form
    {
        private class PaymentRow
        {
            public int IDTashlum { get; set; }
            public int IDTik { get; set; }
            public string Client { get; set; }
            public DateTime TaarichTashlum { get; set; }
            public string EmzhaeyTashlum { get; set; }
            public decimal Schum { get; set; }
            public decimal Itra { get; set; }
            public string Teur { get; set; }
        }

        private List<PaymentRow> all = new List<PaymentRow>();

        public FrmAllPayments()
        {
            InitializeComponent();
            LoadPayments();
        }

        private void LoadPayments()
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
                        EmzhaeyTashlum = p.EmzhaeyTashlum,
                        Schum = p.Schum,
                        Itra = p.Itra,
                        Teur = p.Teur
                    })
                    .ToList();
            }
            ApplyFilter();
        }

        // Always filters the full list, so deleting characters widens the results again.
        private void ApplyFilter()
        {
            string text = txtSearch.Text.Trim();
            List<PaymentRow> rows = text.Length == 0
                ? all
                : all.Where(p => p.IDTik.ToString() == text || (p.Client ?? "").Contains(text)).ToList();

            dataGridView1.DataSource = rows;
            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "IDTashlum", "קוד תשלום" },
                { "IDTik", "מספר תיק" },
                { "Client", "לקוח" },
                { "TaarichTashlum", "תאריך" },
                { "EmzhaeyTashlum", "אמצעי תשלום" },
                { "Schum", "סכום" },
                { "Itra", "יתרה" },
                { "Teur", "תיאור" }
            });
            lblTotal.Text = "סה\"כ: " + Ui.FormatMoney(rows.Sum(r => r.Schum));
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            int? id = Ui.SelectedInt(dataGridView1, "IDTashlum");
            if (id == null)
            {
                Ui.Error("יש לבחור תשלום מהרשימה.");
                return;
            }
            using (var receipt = new frmKabala(id.Value))
                receipt.ShowDialog();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnReceipt_Click(sender, e);
        }
    }
}
