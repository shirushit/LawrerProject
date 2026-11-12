using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Case types (e.g. "civil claim"), their fees and fixed steps.</summary>
    public partial class frmAllSubjektTik : Form
    {
        private List<Subjekt_Tik> all = new List<Subjekt_Tik>();

        public frmAllSubjektTik()
        {
            InitializeComponent();
            LoadTypes();
        }

        private void LoadTypes()
        {
            using (var db = new LawyerDBEntities1())
                all = db.Subjekt_Tik.OrderBy(s => s.NameSugTik).ToList();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string text = txtNameSubjektTik.Text.Trim();
            dataGridView1.DataSource = all
                .Where(s => text.Length == 0 || s.NameSugTik.Contains(text))
                .Select(s => new { s.IDSugTik, s.NameSugTik, s.ScharTircha, s.Agra })
                .ToList();
            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "IDSugTik", "קוד" },
                { "NameSugTik", "נושא תיק" },
                { "ScharTircha", "שכר טרחה" },
                { "Agra", "אגרה" }
            });
        }

        private void txtNameSubjektTik_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void txtNameSubjektTik_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNameKey(e.KeyChar);
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            using (var form = new frmAddSubjektTik())
                form.ShowDialog();
            LoadTypes();
        }

        private void btnUpDate_Click_1(object sender, EventArgs e)
        {
            int? id = Ui.SelectedInt(dataGridView1, "IDSugTik");
            if (id == null)
            {
                Ui.Error("יש לבחור נושא תיק מהרשימה.");
                return;
            }
            using (var form = new frmAddSubjektTik(id.Value))
                form.ShowDialog();
            LoadTypes();
        }
    }
}
