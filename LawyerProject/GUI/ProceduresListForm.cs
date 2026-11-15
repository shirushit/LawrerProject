using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>The catalogue of processes (steps) and how long each one takes.</summary>
    public partial class ProceduresListForm : Form
    {
        private List<Procedure> all = new List<Procedure>();

        public ProceduresListForm()
        {
            InitializeComponent();
            txtProcedureName.TextChanged += (s, e) => ApplyFilter();
            LoadProcesses();
        }

        private void LoadProcesses()
        {
            using (var db = new QuedinDbContext())
                all = db.Procedures.OrderBy(t => t.Name).ToList();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string text = txtProcedureName.Text.Trim();
            dataGridView1.DataSource = all
                .Where(t => text.Length == 0 || t.Name.Contains(text))
                .Select(t => new { t.ProcedureId, t.Name, t.Category, Days = t.DurationDays })
                .ToList();
            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "ProcedureId", "קוד" },
                { "Name", "שם תהליך" },
                { "Category", "סוג תהליך" },
                { "Days", "משך משוער (ימים)" }
            });
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new ProcedureForm())
                if (form.ShowDialog(this) == DialogResult.OK) Ui.Notify(this, "התהליך נשמר.");
            LoadProcesses();
        }

        private void btnUpdate_Click_2(object sender, EventArgs e)
        {
            int? id = Ui.SelectedInt(dataGridView1, "ProcedureId");
            if (id == null)
            {
                Ui.Error("יש לבחור תהליך מהרשימה.");
                return;
            }
            using (var form = new ProcedureForm(id.Value))
                if (form.ShowDialog(this) == DialogResult.OK)
                    Ui.Notify(this, "התהליך עודכן. המשך החדש ישמש לתיקים שייפתחו מעכשיו (תיקים קיימים שומרים את המשך שלהם).");
            LoadProcesses();
        }
    }
}
