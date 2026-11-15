using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Courts and the case types each court handles.</summary>
    public partial class CourtsListForm : Form
    {
        private List<Court> all = new List<Court>();

        public CourtsListForm()
        {
            InitializeComponent();
            LoadCourts();
        }

        private void LoadCourts()
        {
            using (var db = new QuedinDbContext())
                all = db.Courts.OrderBy(b => b.Name).ToList();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string text = txtCourtName.Text.Trim();
            dataGridView1.DataSource = all
                .Where(b => text.Length == 0 || b.Name.Contains(text))
                .Select(b => new { b.CourtId, b.Name })
                .ToList();
            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "CourtId", "קוד" },
                { "Name", "שם בית משפט" }
            });
        }

        private void txtCourtName_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void txtCourtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNameKey(e.KeyChar);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new CourtForm())
                form.ShowDialog();
            LoadCourts();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int? id = Ui.SelectedInt(dataGridView1, "CourtId");
            if (id == null)
            {
                Ui.Error("יש לבחור בית משפט מהרשימה.");
                return;
            }
            using (var form = new CourtForm(id.Value))
                form.ShowDialog();
            LoadCourts();
        }
    }
}
