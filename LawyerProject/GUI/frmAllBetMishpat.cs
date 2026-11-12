using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Courts and the case types each court handles.</summary>
    public partial class frmAllBetMishpat : Form
    {
        private List<Bet_Mishpat> all = new List<Bet_Mishpat>();

        public frmAllBetMishpat()
        {
            InitializeComponent();
            LoadCourts();
        }

        private void LoadCourts()
        {
            using (var db = new LawyerDBEntities1())
                all = db.Bet_Mishpat.OrderBy(b => b.NameBetMishpat).ToList();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string text = txtNameBetMishpat.Text.Trim();
            dataGridView1.DataSource = all
                .Where(b => text.Length == 0 || b.NameBetMishpat.Contains(text))
                .Select(b => new { b.IDBetMishpat, b.NameBetMishpat })
                .ToList();
            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "IDBetMishpat", "קוד" },
                { "NameBetMishpat", "שם בית משפט" }
            });
        }

        private void txtNameBetMishpat_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void txtNameBetMishpat_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNameKey(e.KeyChar);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new frmAddBetMishpat())
                form.ShowDialog();
            LoadCourts();
        }

        private void btnUpDate_Click(object sender, EventArgs e)
        {
            int? id = Ui.SelectedInt(dataGridView1, "IDBetMishpat");
            if (id == null)
            {
                Ui.Error("יש לבחור בית משפט מהרשימה.");
                return;
            }
            using (var form = new frmAddBetMishpat(id.Value))
                form.ShowDialog();
            LoadCourts();
        }
    }
}
