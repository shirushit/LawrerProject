using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>
    /// Open a new case or manage an existing one: details, opposing side, fees,
    /// the step timeline (with automatic date updates) and payments.
    /// </summary>
    public partial class CaseForm : Form
    {
        private QuedinDbContext db;
        private LegalCase legalCase;
        private bool isNew;
        private bool loading;
        private bool dirty;
        private byte[] loadedVersion;

        /// <summary>True when the user changed a field that was not saved yet.</summary>
        public bool HasUnsavedChanges
        {
            get { return dirty; }
        }

        /// <summary>Set before closing on logout, so the window closes without asking about unsaved changes.</summary>
        public bool DiscardChangesOnClose { get; set; }

        /// <summary>New court case.</summary>
        public CaseForm() : this(false)
        {
        }

        /// <summary>New case; <paramref name="isDeal"/> = property transaction instead of a court case.</summary>
        public CaseForm(bool isDeal)
        {
            InitializeComponent();
            isNew = true;
            db = new QuedinDbContext();
            legalCase = new LegalCase();
            LoadLists();
            rbDeal.Checked = isDeal;
            rbCourt.Checked = !isDeal;
            UpdateMode();
            TrackChanges(this);
            dirty = false;
        }

        /// <summary>Existing case.</summary>
        public CaseForm(int id)
        {
            InitializeComponent();
            LoadCase(id);
            TrackChanges(this);
            Activated += (s, e) => CheckForOutsideChanges();
        }

        /// <summary>Marks the form dirty when the user edits any input of the case details.</summary>
        private void TrackChanges(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c == tabsCase) continue; // steps and payments are saved immediately
                if (c is TextBox || c is MaskedTextBox) c.TextChanged += MarkDirty;
                else if (c is ComboBox) ((ComboBox)c).SelectedIndexChanged += MarkDirty;
                else if (c is CheckBox) ((CheckBox)c).CheckedChanged += MarkDirty;
                else if (c is RadioButton) ((RadioButton)c).CheckedChanged += MarkDirty;
                else if (c is DateTimePicker) ((DateTimePicker)c).ValueChanged += MarkDirty;
                if (c.HasChildren) TrackChanges(c);
            }
        }

        private void MarkDirty(object sender, EventArgs e)
        {
            if (!loading) dirty = true;
        }

        /// <summary>
        /// When the window gets focus again, checks whether another user changed or deleted the case
        /// in the meantime. Without local edits the case is reloaded; with local edits the user is told.
        /// </summary>
        private void CheckForOutsideChanges()
        {
            if (isNew || loadedVersion == null) return;
            byte[] current;
            try
            {
                current = new CaseService(db).CurrentVersion(legalCase.CaseId);
            }
            catch (Exception)
            {
                return; // no connection right now; the next save will report it
            }
            if (current == null)
            {
                CaseDeleted();
                return;
            }
            if (current.SequenceEqual(loadedVersion)) return;
            if (dirty)
            {
                Ui.Notify(this, "שימו לב: משתמש אחר עדכן את התיק. שמירה של השינויים שלך תיחסם - כדאי לרשום אותם בצד.");
                return;
            }
            LoadCase(legalCase.CaseId);
            Ui.Notify(this, "התיק עודכן על ידי משתמש אחר ונטען מחדש.");
        }

        /// <summary>The case no longer exists (a manager deleted it while this window was open).</summary>
        private void CaseDeleted()
        {
            Ui.Error("תיק מספר " + legalCase.CaseId + " נמחק על ידי משתמש אחר. החלון ייסגר.");
            dirty = false;
            Changed = true;
            DiscardChangesOnClose = true;
            BeginInvoke((Action)Close);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (dirty && !DiscardChangesOnClose && e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult answer = MessageBox.Show(this, "יש שינויים בתיק שלא נשמרו. לשמור אותם לפני הסגירה?", "שינויים שלא נשמרו",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                if (answer == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                if (answer == DialogResult.Yes)
                {
                    btnSave_Click(this, EventArgs.Empty);
                    if (dirty)
                    {
                        e.Cancel = true; // the save failed or was refused; stay open
                        return;
                    }
                }
            }
            base.OnFormClosing(e);
        }

        // ------------------------------------------------------------------ loading

        private void LoadCase(int id)
        {
            if (db != null) db.Dispose();
            db = new QuedinDbContext();
            legalCase = db.Cases.FirstOrDefault(t => t.CaseId == id);
            if (legalCase == null) throw new ArgumentException("Case not found: " + id);
            isNew = false;
            loadedVersion = legalCase.RowVersion;
            loading = true;
            LoadLists();
            FillControls();
            FillSteps();
            FillPayments();
            UpdateMode();
            loading = false;
            dirty = false;
        }

        private void LoadLists()
        {
            loading = true;
            List<Customer> people = db.Customers.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();
            // Clients first; any person can be an opposing party or lawyer (a client in one case
            // may be the other side in another), so those lists show everyone, their own type first.
            cmbClient.DisplayMember = cmbOpposingParty.DisplayMember = cmbOpposingLawyer.DisplayMember = "ListText";
            cmbClient.DataSource = people.OrderBy(c => c.PersonType == PersonTypes.Client ? 0 : 1).ToList();
            cmbOpposingParty.DataSource = people.OrderBy(c => c.PersonType == PersonTypes.OpposingClient ? 0 : 1).ToList();
            cmbOpposingLawyer.DataSource = people.OrderBy(c => c.PersonType == PersonTypes.OpposingLawyer ? 0 : 1).ToList();
            cmbCaseType.DataSource = db.CaseTypes.OrderBy(s => s.Name).ToList();
            cmbClient.SelectedIndex = cmbOpposingParty.SelectedIndex = cmbOpposingLawyer.SelectedIndex = -1;
            if (isNew) cmbCaseType.SelectedIndex = -1;
            loading = false;
            cmbCaseType.SelectedIndexChanged -= cmbCaseType_SelectedIndexChanged;
            cmbCaseType.SelectedIndexChanged += cmbCaseType_SelectedIndexChanged;
            cmbOpposingParty.SelectedIndexChanged -= cmbOpposingParty_SelectedIndexChanged;
            cmbOpposingParty.SelectedIndexChanged += cmbOpposingParty_SelectedIndexChanged;
            LoadCourts();
        }

        /// <summary>Courts that handle the selected case type; all courts if none is linked yet.</summary>
        private void LoadCourts()
        {
            var caseType = cmbCaseType.SelectedItem as CaseType;
            List<Court> courts = null;
            if (caseType != null)
                courts = db.CourtCaseTypes
                    .Where(s => s.CaseTypeId == caseType.CaseTypeId)
                    .Select(s => s.Court)
                    .Distinct()
                    .OrderBy(b => b.Name)
                    .ToList();
            if (courts == null || courts.Count == 0)
                courts = db.Courts.OrderBy(b => b.Name).ToList();

            object previous = cmbCourt.SelectedItem;
            cmbCourt.DataSource = courts;
            cmbCourt.SelectedItem = previous;
            if (cmbCourt.SelectedItem == null) cmbCourt.SelectedIndex = -1;
        }

        private static void Select(ComboBox combo, Func<object, bool> match)
        {
            foreach (object item in combo.Items)
                if (match(item)) { combo.SelectedItem = item; return; }
            combo.SelectedIndex = -1;
        }

        private void FillControls()
        {
            loading = true;
            Text = "תיק מספר " + legalCase.CaseId;
            lblCaseId.Text = legalCase.CaseId.ToString();
            Select(cmbClient, o => ((Customer)o).CustomerId == legalCase.ClientId);
            Select(cmbCaseType, o => ((CaseType)o).CaseTypeId == legalCase.CaseTypeId);
            Select(cmbOpposingParty, o => ((Customer)o).CustomerId == legalCase.OpposingPartyId);
            Select(cmbOpposingLawyer, o => ((Customer)o).CustomerId == legalCase.OpposingLawyerId);
            loading = false;
            LoadCourts();
            Select(cmbCourt, o => ((Court)o).CourtId == legalCase.CourtId);

            rbCourt.Checked = legalCase.IsCourtCase;
            rbDeal.Checked = !legalCase.IsCourtCase;
            DateTime signed;
            chkPowerOfAttorney.Checked = TryReadSigned(legalCase.PowerOfAttorney, out signed);
            dtpPowerOfAttorney.Value = chkPowerOfAttorney.Checked ? signed : Clock.Today;
            dtpPowerOfAttorney.Enabled = chkPowerOfAttorney.Checked;

            string money = legalCase.ClaimAmount.HasValue ? legalCase.ClaimAmount.Value.ToString("0.##", CultureInfo.InvariantCulture) : "";
            txtClaimAmount.Text = legalCase.IsCourtCase ? money : "";
            txtPropertyValue.Text = legalCase.IsCourtCase ? "" : money;
            txtAwardedAmount.Text = legalCase.AwardedAmount.HasValue ? legalCase.AwardedAmount.Value.ToString("0.##", CultureInfo.InvariantCulture) : "";
            txtPropertyAddress.Text = legalCase.PropertyAddress;
            txtBlock.Text = legalCase.Block;
            txtParcel.Text = legalCase.Parcel;
            txtSubParcel.Text = legalCase.SubParcel;

            txtFeePercent.Text = legalCase.FeePercent.HasValue ? legalCase.FeePercent.Value.ToString(CultureInfo.InvariantCulture) : "";
            txtAdvance.Text = legalCase.Advance > 0 ? legalCase.Advance.ToString("0.##", CultureInfo.InvariantCulture) : "";

            lblCaseStatus.Text = CaseService.IsFinished(legalCase)
                ? "הסתיים"
                : (legalCase.CurrentProcedure != null ? legalCase.CurrentProcedure.Name : "-");
            ShowOpposingDetails();
            UpdateMoney();
        }

        private const string SignedPrefix = "נחתם ";

        /// <summary>Power of attorney is stored as "נחתם dd/MM/yyyy", or "" when not signed yet.</summary>
        private static bool TryReadSigned(string value, out DateTime date)
        {
            date = Clock.Today;
            if (string.IsNullOrWhiteSpace(value)) return false;
            string v = value.Trim();
            if (v.StartsWith(SignedPrefix)) v = v.Substring(SignedPrefix.Length);
            if (DateTime.TryParseExact(v, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)) return true;
            date = Clock.Today;
            return true; // older free text such as "כן" counts as signed
        }

        private void chkPowerOfAttorney_CheckedChanged(object sender, EventArgs e)
        {
            dtpPowerOfAttorney.Enabled = chkPowerOfAttorney.Checked;
        }

        private void ShowOpposingDetails()
        {
            var opposing = cmbOpposingParty.SelectedItem as Customer;
            if (opposing == null) { lblOpposingDetails.Text = ""; return; }
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(opposing.IdType) && opposing.IdType != IdTypes.None)
                parts.Add(opposing.IdType + " " + opposing.CustomerId);
            if (!string.IsNullOrEmpty(opposing.MobilePhone)) parts.Add(opposing.MobilePhone);
            else if (!string.IsNullOrEmpty(opposing.Phone)) parts.Add(opposing.Phone);
            lblOpposingDetails.Text = parts.Count == 0 ? "אין פרטי זיהוי או קשר" : string.Join("  |  ", parts);
        }

        // ------------------------------------------------------------------ quick-add people

        private void AddPerson(string personType, ComboBox target)
        {
            using (var form = new CustomerForm(personType))
            {
                if (form.ShowDialog() != DialogResult.OK) return;
                string id = form.SavedId;
                object[] keep = { cmbClient.SelectedItem, cmbOpposingParty.SelectedItem, cmbOpposingLawyer.SelectedItem };
                var caseType = cmbCaseType.SelectedItem as CaseType;
                var court = cmbCourt.SelectedItem as Court;
                LoadLists();
                Changed = true;
                loading = true;
                Select(cmbCaseType, o => caseType != null && ((CaseType)o).CaseTypeId == caseType.CaseTypeId);
                loading = false;
                LoadCourts();
                Select(cmbCourt, o => court != null && ((Court)o).CourtId == court.CourtId);
                Select(cmbClient, o => keep[0] != null && ((Customer)o).CustomerId == ((Customer)keep[0]).CustomerId);
                Select(cmbOpposingParty, o => keep[1] != null && ((Customer)o).CustomerId == ((Customer)keep[1]).CustomerId);
                Select(cmbOpposingLawyer, o => keep[2] != null && ((Customer)o).CustomerId == ((Customer)keep[2]).CustomerId);
                Select(target, o => ((Customer)o).CustomerId == id);
            }
        }

        private void btnAddClient_Click(object sender, EventArgs e) { AddPerson(PersonTypes.Client, cmbClient); }

        private void btnAddOpposingParty_Click(object sender, EventArgs e) { AddPerson(PersonTypes.OpposingClient, cmbOpposingParty); }

        private void btnAddLawyer_Click(object sender, EventArgs e) { AddPerson(PersonTypes.OpposingLawyer, cmbOpposingLawyer); }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (db != null) db.Dispose();
            base.OnFormClosed(e);
        }
    }
}
