using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Who did what and when (managers only): payments, cancellations, step changes, users, backups.</summary>
    public class AuditLogForm : Form
    {
        private class Row
        {
            public string When { get; set; }
            public string User { get; set; }
            public string Action { get; set; }
            public string Entity { get; set; }
            public string Details { get; set; }
            public string Search { get; set; }
        }

        private const int MaxRows = 2000;
        private TextBox txtSearch;
        private Label lblSearch;
        private Label lblCount;
        private DataGridView grid;
        private List<Row> rows = new List<Row>();

        public AuditLogForm()
        {
            InitializeComponent();
            LoadRows();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "יומן פעולות";
            Font = new Font("Calibri", 11F);
            RightToLeft = RightToLeft.Yes;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(1000, 640);

            lblSearch = new Label { Text = "חיפוש", Location = new Point(880, 18), Size = new Size(100, 26), TextAlign = ContentAlignment.MiddleRight };
            txtSearch = new TextBox { Location = new Point(560, 18), Width = 310 };
            txtSearch.TextChanged += (s, e) => Apply();
            lblCount = new Label { Location = new Point(20, 18), Size = new Size(400, 26), ForeColor = SystemColors.GrayText, TextAlign = ContentAlignment.MiddleLeft };
            grid = new DataGridView
            {
                Location = new Point(20, 55),
                Size = new Size(960, 565),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackgroundColor = Color.White,
                RowHeadersVisible = false,
                RightToLeft = RightToLeft.Yes
            };
            Controls.AddRange(new Control[] { lblSearch, txtSearch, lblCount, grid });
            ResumeLayout(false);
        }

        private void LoadRows()
        {
            try
            {
                using (var db = new QuedinDbContext())
                {
                    rows = db.AuditLog.OrderByDescending(y => y.AuditEntryId).Take(MaxRows).ToList().Select(y => new Row
                    {
                        When = y.OccurredAt.ToString("dd/MM/yyyy HH:mm"),
                        User = y.UserName ?? "",
                        Action = y.Action,
                        Entity = (y.Entity ?? "") + (string.IsNullOrEmpty(y.EntityId) ? "" : " " + y.EntityId),
                        Details = y.Details ?? ""
                    }).ToList();
                }
                foreach (Row r in rows)
                    r.Search = (r.When + " " + r.User + " " + r.Action + " " + r.Entity + " " + r.Details).ToLowerInvariant();
                Apply();
            }
            catch (Exception ex)
            {
                Ui.Error("לא ניתן לטעון את היומן.", ex);
            }
        }

        private void Apply()
        {
            IEnumerable<Row> shown = rows;
            foreach (string word in txtSearch.Text.Trim().ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string w = word;
                shown = shown.Where(r => r.Search.Contains(w));
            }
            List<Row> list = shown.ToList();
            grid.DataSource = list;
            Ui.SetHeaders(grid, new Dictionary<string, string>
            {
                { "When", "מועד" }, { "User", "משתמש" }, { "Action", "פעולה" }, { "Entity", "רשומה" }, { "Details", "פרטים" }
            });
            lblCount.Text = list.Count + " פעולות" + (rows.Count == MaxRows ? " (מוצגות " + MaxRows + " האחרונות)" : "");
        }
    }
}
