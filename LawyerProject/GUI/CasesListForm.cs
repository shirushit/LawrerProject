using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>
    /// All cases with filters. Cases whose next step is due within
    /// <see cref="AppSettings.DueSoonDays"/> days are highlighted; late ones in red.
    /// </summary>
    public partial class CasesListForm : Form
    {
        private class CaseRow
        {
            public int CaseId { get; set; }
            public string ClientId { get; set; }
            public string ClientName { get; set; }
            public int SubjectId { get; set; }
            public string Subject { get; set; }
            public int StatusId { get; set; }
            public string Status { get; set; }
            public DateTime? NextDue { get; set; }
            public string NextDueText { get; set; }
            public string OpposingClientId { get; set; }
            public string OpposingClient { get; set; }
            public string OpposingLawyerId { get; set; }
            public string OpposingLawyer { get; set; }
            public string Court { get; set; }
            public bool IsPaid { get; set; }
            public decimal Balance { get; set; }
            public bool Finished { get; set; }
            public string LateText { get; set; }
            public string SearchText { get; set; }
        }

        private const string AllItems = "(הכל)";
        private List<CaseRow> all = new List<CaseRow>();
        private bool loading;

        public CasesListForm()
        {
            InitializeComponent();

            chkFilterPaid.ThreeState = true;
            chkFilterPaid.CheckState = CheckState.Indeterminate;

            txtSearch.TextChanged += Filter_Changed;
            lblLegendSoon.Text = "מועד ב-" + AppSettings.DueSoonDays + " הימים הקרובים";
            txtFilterClientId.TextChanged += Filter_Changed;
            txtFilterCaseId.TextChanged += Filter_Changed;
            cmbFilterOpposingLawyer.SelectedIndexChanged += Filter_Changed;
            cmbFilterStatus.SelectedIndexChanged += Filter_Changed;
            cmbFilterOpposingParty.SelectedIndexChanged += Filter_Changed;
            cmbFilterClient.SelectedIndexChanged += Filter_Changed;
            cmbFilterCaseType.SelectedIndexChanged += Filter_Changed;
            chkFilterPaid.CheckStateChanged += Filter_Changed;
            dataGridView1.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OpenSelected(); };
            dataGridView1.DataBindingComplete += (s, e) => Highlight();

            LoadCases();
        }

        private void LoadCases()
        {
            loading = true;
            using (var db = new QuedinDbContext())
            {
                Dictionary<string, Customer> people = db.Customers.ToDictionary(c => c.CustomerId);
                var cases = db.Cases
                    .Include(t => t.Client)
                    .Include(t => t.CaseType)
                    .Include(t => t.CurrentProcedure)
                    .Include(t => t.Court)
                    .Include(t => t.Steps)
                    .Include(t => t.Payments)
                    .ToList();

                all = cases.Select(t =>
                {
                    CaseStep next = CaseService.OrderedSteps(t).FirstOrDefault(s => s.IsDone != true);
                    int late = next != null && next.DueDate.HasValue ? CaseScheduler.DaysLate(next.DueDate.Value, Clock.Today) : 0;
                    var row = new CaseRow
                    {
                        CaseId = t.CaseId,
                        ClientId = t.ClientId,
                        ClientName = t.Client != null ? t.Client.ToString() : "",
                        SubjectId = t.CaseTypeId,
                        Subject = t.CaseType != null ? t.CaseType.Name : "",
                        StatusId = t.CurrentProcedureId,
                        Status = CaseService.IsFinished(t) ? "הסתיים" : (t.CurrentProcedure != null ? t.CurrentProcedure.Name : ""),
                        NextDue = next != null ? next.DueDate : null,
                        NextDueText = next != null ? Ui.FormatDate(next.DueDate) : "",
                        OpposingClientId = t.OpposingPartyId,
                        OpposingClient = NameOf(people, t.OpposingPartyId),
                        OpposingLawyerId = t.OpposingLawyerId,
                        OpposingLawyer = NameOf(people, t.OpposingLawyerId),
                        Court = t.IsCourtCase && t.Court != null ? t.Court.Name : "עסקה",
                        IsPaid = t.IsPaid,
                        Balance = CaseService.OpenBalance(t),
                        Finished = CaseService.IsFinished(t),
                        LateText = late > 0 ? late + " ימים" : ""
                    };
                    row.SearchText = string.Join(" ", row.CaseId, row.ClientId, row.ClientName, row.Subject, row.Status,
                        row.OpposingClient, row.OpposingLawyer, row.Court, t.Block, t.Parcel, t.PropertyAddress).ToLowerInvariant();
                    return row;
                }).OrderBy(r => r.Finished).ThenBy(r => r.NextDue ?? DateTime.MaxValue).ToList();

                BindFilter(cmbFilterClient, people.Values.Where(p => p.PersonType == PersonTypes.Client));
                var opposingIds = new HashSet<string>(cases.Select(c => c.OpposingPartyId));
                var lawyerIds = new HashSet<string>(cases.Select(c => c.OpposingLawyerId));
                BindFilter(cmbFilterOpposingParty, people.Values.Where(p => opposingIds.Contains(p.CustomerId)));
                BindFilter(cmbFilterOpposingLawyer, people.Values.Where(p => lawyerIds.Contains(p.CustomerId)));
                BindFilter(cmbFilterCaseType, db.CaseTypes.OrderBy(s => s.Name).ToList());
                BindFilter(cmbFilterStatus, db.Procedures.OrderBy(s => s.Name).ToList());
            }
            loading = false;
            ApplyFilter();
        }

        private static string NameOf(Dictionary<string, Customer> people, string id)
        {
            Customer c;
            return id != null && people.TryGetValue(id, out c) ? c.ToString() : "";
        }

        private static void BindFilter(ComboBox combo, IEnumerable<object> items)
        {
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.DataSource = new object[] { AllItems }.Concat(items.OrderBy(i => i.ToString())).ToList();
        }

        private static T Selected<T>(ComboBox combo) where T : class
        {
            return combo.SelectedItem as T;
        }

        private void Filter_Changed(object sender, EventArgs e)
        {
            if (!loading) ApplyFilter();
        }

        private void ApplyFilter()
        {
            IEnumerable<CaseRow> rows = all;

            // Free search: every word must appear somewhere in the row (name, ID, case no., court, block/parcel...).
            foreach (string word in txtSearch.Text.Trim().ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string w = word;
                rows = rows.Where(r => r.SearchText.Contains(w));
            }

            string clientId = txtFilterClientId.Text.Trim();
            if (clientId.Length > 0) rows = rows.Where(r => (r.ClientId ?? "").StartsWith(clientId));

            int caseNo;
            if (int.TryParse(txtFilterCaseId.Text.Trim(), out caseNo)) rows = rows.Where(r => r.CaseId == caseNo);

            Customer client = Selected<Customer>(cmbFilterClient);
            if (client != null) rows = rows.Where(r => r.ClientId == client.CustomerId);

            Customer opposingClient = Selected<Customer>(cmbFilterOpposingParty);
            if (opposingClient != null) rows = rows.Where(r => r.OpposingClientId == opposingClient.CustomerId);

            Customer opposingLawyer = Selected<Customer>(cmbFilterOpposingLawyer);
            if (opposingLawyer != null) rows = rows.Where(r => r.OpposingLawyerId == opposingLawyer.CustomerId);

            CaseType subject = Selected<CaseType>(cmbFilterCaseType);
            if (subject != null) rows = rows.Where(r => r.SubjectId == subject.CaseTypeId);

            Procedure status = Selected<Procedure>(cmbFilterStatus);
            if (status != null) rows = rows.Where(r => r.StatusId == status.ProcedureId && !r.Finished);

            if (chkFilterPaid.CheckState != CheckState.Indeterminate)
            {
                bool paid = chkFilterPaid.CheckState == CheckState.Checked;
                rows = rows.Where(r => r.IsPaid == paid);
            }

            List<CaseRow> list = rows.ToList();
            dataGridView1.DataSource = list;
            lblCount.Text = list.Count == all.Count ? all.Count + " תיקים" : "מוצגים " + list.Count + " מתוך " + all.Count + " תיקים";
            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "CaseId", "מספר תיק" },
                { "ClientId", "ת.ז. לקוח" },
                { "ClientName", "שם לקוח" },
                { "Subject", "נושא תיק" },
                { "Status", "שלב נוכחי" },
                { "NextDueText", "מועד משוער" },
                { "LateText", "באיחור" },
                { "OpposingClient", "לקוח נגדי" },
                { "OpposingLawyer", "עורך דין נגדי" },
                { "Court", "בית משפט" },
                { "Balance", "יתרה לתשלום" },
                { "IsPaid", "שולם" }
            });
        }

        private void Highlight()
        {
            DateTime today = Clock.Today;
            int days = AppSettings.DueSoonDays;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var data = row.DataBoundItem as CaseRow;
                if (data == null || data.Finished || data.NextDue == null) continue;
                if (data.NextDue.Value.Date < today)
                    row.DefaultCellStyle.BackColor = Color.MistyRose;
                else if (data.NextDue.Value.Date <= today.AddDays(days))
                    row.DefaultCellStyle.BackColor = Color.LightYellow;
            }
        }

        private void OpenSelected()
        {
            int? id = Ui.SelectedInt(dataGridView1, "CaseId");
            if (id == null)
            {
                Ui.Error("יש לבחור תיק מהרשימה.");
                return;
            }
            CaseWindows.OpenCase(this, id.Value, () => { if (!IsDisposed) LoadCases(); });
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CaseWindows.NewCase(this, false, () => { if (!IsDisposed) LoadCases(); });
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            loading = true;
            txtSearch.Clear();
            txtFilterClientId.Clear();
            txtFilterCaseId.Clear();
            foreach (ComboBox c in new[] { cmbFilterOpposingLawyer, cmbFilterStatus, cmbFilterOpposingParty, cmbFilterClient, cmbFilterCaseType })
                if (c.Items.Count > 0) c.SelectedIndex = 0;
            chkFilterPaid.CheckState = CheckState.Indeterminate;
            loading = false;
            ApplyFilter();
            txtSearch.Focus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            OpenSelected();
        }

        private void txtFilterCaseId_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsDigitKey(e.KeyChar);
        }
    }
}
