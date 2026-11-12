using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>The catalogue of processes (steps) and how long each one takes.</summary>
    public partial class FrmAllTaalichimLetik : Form
    {
        private List<Taalichim_Letikim> all = new List<Taalichim_Letikim>();

        public FrmAllTaalichimLetik()
        {
            InitializeComponent();
            txtNameTaalich.TextChanged += (s, e) => ApplyFilter();
            LoadProcesses();
        }

        private void LoadProcesses()
        {
            using (var db = new LawyerDBEntities1())
                all = db.Taalichim_Letikim.OrderBy(t => t.NameTaalich).ToList();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string text = txtNameTaalich.Text.Trim();
            dataGridView1.DataSource = all
                .Where(t => text.Length == 0 || t.NameTaalich.Contains(text))
                .Select(t => new { t.IDTaalich, t.NameTaalich, t.SugTaalich, Days = t.MeshechZmanLetaalich })
                .ToList();
            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "IDTaalich", "קוד" },
                { "NameTaalich", "שם תהליך" },
                { "SugTaalich", "סוג תהליך" },
                { "Days", "משך משוער (ימים)" }
            });
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new FrmAddTaalichimLetik())
                form.ShowDialog();
            LoadProcesses();
        }

        private void btnUpDate_Click_2(object sender, EventArgs e)
        {
            int? id = Ui.SelectedInt(dataGridView1, "IDTaalich");
            if (id == null)
            {
                Ui.Error("יש לבחור תהליך מהרשימה.");
                return;
            }
            using (var form = new FrmAddTaalichimLetik(id.Value))
                form.ShowDialog();
            LoadProcesses();
        }
    }
}
