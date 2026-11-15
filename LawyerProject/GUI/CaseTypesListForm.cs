using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Case types (e.g. "civil claim"), their fees and fixed steps.</summary>
    public partial class CaseTypesListForm : Form
    {
        private List<CaseType> all = new List<CaseType>();

        public CaseTypesListForm()
        {
            InitializeComponent();
            LoadTypes();
        }

        private void LoadTypes()
        {
            using (var db = new QuedinDbContext())
                all = db.CaseTypes.OrderBy(s => s.Name).ToList();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string text = txtCaseTypeName.Text.Trim();
            dataGridView1.DataSource = all
                .Where(s => text.Length == 0 || s.Name.Contains(text))
                .Select(s => new { s.CaseTypeId, s.Name, s.BaseFee, s.CourtFee })
                .ToList();
            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "CaseTypeId", "קוד" },
                { "Name", "נושא תיק" },
                { "BaseFee", "שכר טרחה" },
                { "CourtFee", "אגרה" }
            });
        }

        private void txtCaseTypeName_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void txtCaseTypeName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNameKey(e.KeyChar);
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            using (var form = new CaseTypeForm())
                form.ShowDialog();
            LoadTypes();
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            int? id = Ui.SelectedInt(dataGridView1, "CaseTypeId");
            if (id == null)
            {
                Ui.Error("יש לבחור נושא תיק מהרשימה.");
                return;
            }
            using (var form = new CaseTypeForm(id.Value))
                form.ShowDialog();
            LoadTypes();
        }
    }
}
