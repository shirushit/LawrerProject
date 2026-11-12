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
    public partial class frmAllTikimLetipulOrechDin : Form
    {
        private class CaseRow
        {
            public int IDTik { get; set; }
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
            public bool Shulam { get; set; }
            public decimal Balance { get; set; }
            public bool Finished { get; set; }
        }

        private const string AllItems = "(הכל)";
        private List<CaseRow> all = new List<CaseRow>();
        private bool loading;

        public frmAllTikimLetipulOrechDin()
        {
            InitializeComponent();

            checkBox2.ThreeState = true;
            checkBox2.CheckState = CheckState.Indeterminate;

            maskedTextBox1.TextChanged += Filter_Changed;
            textBox7.TextChanged += Filter_Changed;
            comboBox1.SelectedIndexChanged += Filter_Changed;
            comboBox2.SelectedIndexChanged += Filter_Changed;
            comboBox3.SelectedIndexChanged += Filter_Changed;
            comboBox4.SelectedIndexChanged += Filter_Changed;
            comboBox5.SelectedIndexChanged += Filter_Changed;
            checkBox2.CheckStateChanged += Filter_Changed;
            dataGridView1.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OpenSelected(); };
            dataGridView1.DataBindingComplete += (s, e) => Highlight();

            LoadCases();
        }

        private void LoadCases()
        {
            loading = true;
            using (var db = new LawyerDBEntities1())
            {
                Dictionary<string, Customers> people = db.Customers.ToDictionary(c => c.IDcustemer);
                var cases = db.Tikim_Letipul_Orech_Din
                    .Include(t => t.Customers)
                    .Include(t => t.Subjekt_Tik)
                    .Include(t => t.Taalichim_Letikim)
                    .Include(t => t.Bet_Mishpat)
                    .Include(t => t.Tahalich_Tipul_Betik)
                    .Include(t => t.Tashlumim_Letik)
                    .ToList();

                all = cases.Select(t =>
                {
                    Tahalich_Tipul_Betik next = CaseService.OrderedSteps(t).FirstOrDefault(s => s.Buzha != true);
                    return new CaseRow
                    {
                        IDTik = t.IDTik,
                        ClientId = t.IDCostemer,
                        ClientName = t.Customers != null ? t.Customers.ToString() : "",
                        SubjectId = t.IDSubjektTik,
                        Subject = t.Subjekt_Tik != null ? t.Subjekt_Tik.NameSugTik : "",
                        StatusId = t.StatusTik,
                        Status = CaseService.IsFinished(t) ? "הסתיים" : (t.Taalichim_Letikim != null ? t.Taalichim_Letikim.NameTaalich : ""),
                        NextDue = next != null ? next.TaharichMeshoar : null,
                        NextDueText = next != null ? Ui.FormatDate(next.TaharichMeshoar) : "",
                        OpposingClientId = t.KodLakoachNegdi,
                        OpposingClient = NameOf(people, t.KodLakoachNegdi),
                        OpposingLawyerId = t.KodOrechDinNegdi,
                        OpposingLawyer = NameOf(people, t.KodOrechDinNegdi),
                        Court = t.IsCourtCase && t.Bet_Mishpat != null ? t.Bet_Mishpat.NameBetMishpat : "עסקה",
                        Shulam = t.Shulam,
                        Balance = CaseService.OpenBalance(t),
                        Finished = CaseService.IsFinished(t)
                    };
                }).OrderBy(r => r.Finished).ThenBy(r => r.NextDue ?? DateTime.MaxValue).ToList();

                BindFilter(comboBox4, people.Values.Where(p => p.SugPerson == PersonTypes.Client));
                BindFilter(comboBox3, people.Values.Where(p => p.SugPerson == PersonTypes.OpposingClient));
                BindFilter(comboBox1, people.Values.Where(p => p.SugPerson == PersonTypes.OpposingLawyer));
                BindFilter(comboBox5, db.Subjekt_Tik.OrderBy(s => s.NameSugTik).ToList());
                BindFilter(comboBox2, db.Taalichim_Letikim.OrderBy(s => s.NameTaalich).ToList());
            }
            loading = false;
            ApplyFilter();
        }

        private static string NameOf(Dictionary<string, Customers> people, string id)
        {
            Customers c;
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

            string clientId = maskedTextBox1.Text.Trim();
            if (clientId.Length > 0) rows = rows.Where(r => (r.ClientId ?? "").StartsWith(clientId));

            int caseNo;
            if (int.TryParse(textBox7.Text.Trim(), out caseNo)) rows = rows.Where(r => r.IDTik == caseNo);

            Customers client = Selected<Customers>(comboBox4);
            if (client != null) rows = rows.Where(r => r.ClientId == client.IDcustemer);

            Customers opposingClient = Selected<Customers>(comboBox3);
            if (opposingClient != null) rows = rows.Where(r => r.OpposingClientId == opposingClient.IDcustemer);

            Customers opposingLawyer = Selected<Customers>(comboBox1);
            if (opposingLawyer != null) rows = rows.Where(r => r.OpposingLawyerId == opposingLawyer.IDcustemer);

            Subjekt_Tik subject = Selected<Subjekt_Tik>(comboBox5);
            if (subject != null) rows = rows.Where(r => r.SubjectId == subject.IDSugTik);

            Taalichim_Letikim status = Selected<Taalichim_Letikim>(comboBox2);
            if (status != null) rows = rows.Where(r => r.StatusId == status.IDTaalich && !r.Finished);

            if (checkBox2.CheckState != CheckState.Indeterminate)
            {
                bool paid = checkBox2.CheckState == CheckState.Checked;
                rows = rows.Where(r => r.Shulam == paid);
            }

            dataGridView1.DataSource = rows.ToList();
            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "IDTik", "מספר תיק" },
                { "ClientId", "ת.ז. לקוח" },
                { "ClientName", "שם לקוח" },
                { "Subject", "נושא תיק" },
                { "Status", "שלב נוכחי" },
                { "NextDueText", "מועד משוער" },
                { "OpposingClient", "לקוח נגדי" },
                { "OpposingLawyer", "עורך דין נגדי" },
                { "Court", "בית משפט" },
                { "Balance", "יתרה לתשלום" },
                { "Shulam", "שולם" }
            });
        }

        private void Highlight()
        {
            DateTime today = DateTime.Today;
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
            int? id = Ui.SelectedInt(dataGridView1, "IDTik");
            if (id == null)
            {
                Ui.Error("יש לבחור תיק מהרשימה.");
                return;
            }
            using (var form = new frmAddTikLetipulOrechDin(id.Value))
                form.ShowDialog();
            LoadCases();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new frmAddTikLetipulOrechDin())
                form.ShowDialog();
            LoadCases();
        }

        private void btnUpDate_Click(object sender, EventArgs e)
        {
            OpenSelected();
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsDigitKey(e.KeyChar);
        }
    }
}
