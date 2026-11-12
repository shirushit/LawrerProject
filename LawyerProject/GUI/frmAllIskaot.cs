using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Property transactions: cases that are not managed in court.</summary>
    public partial class frmAllIskaot : Form
    {
        private class DealRow
        {
            public int IDTik { get; set; }
            public string Client { get; set; }
            public string AdressNeches { get; set; }
            public string Gush { get; set; }
            public string Chelka { get; set; }
            public string TatChelka { get; set; }
            public decimal? PropertyValue { get; set; }
            public decimal Balance { get; set; }
        }

        private List<DealRow> all = new List<DealRow>();

        public frmAllIskaot()
        {
            InitializeComponent();
            LoadDeals();
        }

        private void LoadDeals()
        {
            using (var db = new LawyerDBEntities1())
            {
                all = db.Tikim_Letipul_Orech_Din
                    .Include(t => t.Customers)
                    .Include(t => t.Subjekt_Tik)
                    .Include(t => t.Tashlumim_Letik)
                    .Where(t => t.IsTheTikMenualInBETMishpat == false)
                    .ToList()
                    .Select(t => new DealRow
                    {
                        IDTik = t.IDTik,
                        Client = t.Customers != null ? t.Customers.ToString() : "",
                        AdressNeches = t.AdressNeches,
                        Gush = t.Gush,
                        Chelka = t.Chelka,
                        TatChelka = t.TatChelka,
                        PropertyValue = t.SchumLetvia,
                        Balance = CaseService.OpenBalance(t)
                    })
                    .OrderByDescending(r => r.IDTik)
                    .ToList();
            }
            ApplyFilter();
        }

        private static bool Matches(string value, TextBox box)
        {
            string text = box.Text.Trim();
            return text.Length == 0 || (value ?? "").StartsWith(text);
        }

        private void ApplyFilter()
        {
            dataGridView1.DataSource = all
                .Where(r => (txtAdress.Text.Trim().Length == 0 || (r.AdressNeches ?? "").Contains(txtAdress.Text.Trim()))
                         && Matches(r.Gush, txtGush)
                         && Matches(r.Chelka, txtChelka)
                         && Matches(r.TatChelka, txtTatChelka))
                .ToList();
            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "IDTik", "מספר תיק" },
                { "Client", "לקוח" },
                { "AdressNeches", "כתובת נכס" },
                { "Gush", "גוש" },
                { "Chelka", "חלקה" },
                { "TatChelka", "תת חלקה" },
                { "PropertyValue", "שווי נכס" },
                { "Balance", "יתרה לתשלום" }
            });
        }

        private void txtAdress_TextChanged(object sender, EventArgs e) { ApplyFilter(); }

        private void txtGush_TextChanged(object sender, EventArgs e) { ApplyFilter(); }

        private void txtChelka_TextChanged(object sender, EventArgs e) { ApplyFilter(); }

        private void txtTatChelka_TextChanged(object sender, EventArgs e) { ApplyFilter(); }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            using (var form = new frmAddTikLetipulOrechDin(isDeal: true))
                form.ShowDialog();
            LoadDeals();
        }

        private void btnUpDate_Click_1(object sender, EventArgs e)
        {
            int? id = Ui.SelectedInt(dataGridView1, "IDTik");
            if (id == null)
            {
                Ui.Error("יש לבחור עסקה מהרשימה.");
                return;
            }
            using (var form = new frmAddTikLetipulOrechDin(id.Value))
                form.ShowDialog();
            LoadDeals();
        }
    }
}
