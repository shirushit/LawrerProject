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

        private void FillSteps()
        {
            listSteps.Items.Clear();
            DateTime today = Clock.Today;
            CaseStep next = CaseService.OrderedSteps(legalCase).FirstOrDefault(s => s.IsDone != true);
            int number = 1;
            foreach (CaseStep step in CaseService.OrderedSteps(legalCase))
            {
                bool done = step.IsDone == true;
                bool late = CaseScheduler.UrgencyOf(step.DueDate, done, today, 0) == Urgency.Late;
                string status = done ? "בוצע" : late ? "באיחור" : (step == next ? "השלב הבא" : "ממתין");
                if (!done && step.IsFixedDate) status += " · מועד קבוע";
                string lateText = "";
                if (late) lateText = CaseScheduler.DaysLate(step.DueDate.Value, today) + " ימים";
                else if (done && step.DoneDate.HasValue && step.DueDate.HasValue && step.DoneDate.Value > step.DueDate.Value)
                    lateText = "בוצע באיחור";

                var item = new ListViewItem(new[]
                {
                    number.ToString(),
                    step.Procedure != null ? step.Procedure.Name : step.ProcedureId.ToString(),
                    Ui.FormatDate(step.DueDate),
                    done ? Ui.FormatDate(step.DoneDate) : "",
                    status,
                    lateText,
                    step.Notes ?? ""
                });
                item.Tag = step.CaseStepId;
                if (done) item.ForeColor = SystemColors.GrayText;
                else if (late) item.BackColor = Color.MistyRose;
                if (step == next) item.Font = new Font(listSteps.Font, FontStyle.Bold);
                listSteps.Items.Add(item);
                number++;
            }
            btnCompleteStep.Enabled = next != null;
            btnReschedule.Enabled = next != null;
            btnStepNote.Enabled = listSteps.Items.Count > 0;
            btnUndoStep.Visible = Session.IsManager;
            btnUndoStep.Enabled = legalCase.Steps.Any(st => st.IsDone == true);
        }

        private void FillPayments()
        {
            listPayments.Items.Clear();
            foreach (Payment p in legalCase.Payments.OrderBy(p => p.PaymentId))
            {
                string status = p.IsCancelled ? "בוטל" : (p.CancelsPaymentId.HasValue ? "זיכוי" : "");
                var item = new ListViewItem(new[]
                {
                    p.PaymentId.ToString(),
                    Ui.FormatDate(p.PaidOn),
                    p.Kind ?? PaymentKinds.Fee,
                    p.Method,
                    Ui.FormatMoney(p.Amount),
                    Ui.FormatMoney(p.BalanceAfter),
                    status,
                    p.Description
                });
                item.Tag = p.PaymentId;
                if (p.IsCancelled || p.CancelsPaymentId.HasValue) item.ForeColor = SystemColors.GrayText;
                listPayments.Items.Add(item);
            }
            btnReceipt.Enabled = listPayments.Items.Count > 0;
            btnCancelPayment.Enabled = listPayments.Items.Count > 0;
            btnRefund.Visible = !isNew && CaseService.OpenBalance(legalCase) < 0;
        }

        // ------------------------------------------------------------------ UI state

        private void UpdateMode()
        {
            bool court = rbCourt.Checked;
            groupBoxCourt.Visible = court;
            groupBoxDeal.Visible = !court;

            // The fixed steps come from the case type, so it is chosen once, when the case is opened.
            cmbCaseType.Enabled = isNew;
            lblCaseTypeLock.Visible = !isNew;
            tabsCase.Enabled = !isNew;
            lblSaveHint.Visible = isNew;
            btnDeleteCase.Visible = !isNew && Session.IsManager;
            if (isNew) Text = court ? "פתיחת תיק חדש" : "עסקת נכס חדשה";
            UpdateMoney();
        }

        private decimal? ReadMoney(TextBox box)
        {
            decimal value;
            return Validation.TryParseMoney(box.Text, out value) ? value : (decimal?)null;
        }

        private double? ReadPercent()
        {
            double value;
            return double.TryParse(txtFeePercent.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out value)
                ? value : (double?)null;
        }

        /// <summary>Recalculates the case total and the open balance shown on screen.</summary>
        private void UpdateMoney()
        {
            if (loading) return;
            decimal? amount = ReadMoney(rbCourt.Checked ? txtClaimAmount : txtPropertyValue);
            double? percent = ReadPercent();
            var caseType = cmbCaseType.SelectedItem as CaseType;

            if (amount == null || percent == null || !Validation.IsPercent(percent.Value))
            {
                lblTotal.Text = "-";
                lblBalance.Text = "-";
                return;
            }

            decimal total = FeeCalculator.CaseTotal(amount.Value, percent.Value, caseType != null ? (caseType.CourtFee ?? 0m) : 0m, caseType != null ? (caseType.BaseFee ?? 0m) : 0m);
            decimal paid = isNew ? 0m : CaseService.PaidTotal(legalCase);
            decimal balance = FeeCalculator.Balance(total, new[] { paid });
            lblTotal.Text = Ui.FormatMoney(total);
            lblPaid.Text = Ui.FormatMoney(paid);
            lblBalance.Text = balance < 0 ? Ui.FormatMoney(balance) + "  (שולם יותר מהסכום)" : Ui.FormatMoney(balance);
            lblBalance.ForeColor = balance <= 0 && total > 0 ? Color.DarkGreen : SystemColors.ControlText;

            decimal? advance = txtAdvance.Text.Trim().Length == 0 ? 0m : ReadMoney(txtAdvance);
            lblAdvanceStatus.Text = advance == null || advance == 0 ? "" : (paid >= advance ? "שולמה" : "טרם שולמה");
        }

        private bool reverting;

        private void CaseType_CheckedChanged(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;
            if (!rb.Checked || loading || reverting) { if (rb.Checked) UpdateMode(); return; }

            // Switching hides the other group; warn when it already holds data that will not be saved.
            bool toCourt = rb == rbCourt;
            bool hiddenHasData = toCourt
                ? new[] { txtPropertyAddress, txtBlock, txtParcel, txtSubParcel, txtPropertyValue }.Any(t => t.Text.Trim().Length > 0)
                : cmbCourt.SelectedItem != null || new[] { txtClaimAmount, txtAwardedAmount }.Any(t => t.Text.Trim().Length > 0);
            if (hiddenHasData && !Ui.Confirm(toCourt
                    ? "פרטי העסקה שהוזנו (כתובת, גוש, חלקה ושווי) לא יישמרו בתיק בית משפט. להמשיך?"
                    : "פרטי בית המשפט שהוזנו (בית משפט וסכומים) לא יישמרו בעסקת נכס. להמשיך?"))
            {
                reverting = true;
                (toCourt ? rbDeal : rbCourt).Checked = true;
                reverting = false;
                return;
            }
            UpdateMode();
        }

        private void cmbCaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            LoadCourts();
            UpdateMoney();
        }

        private void Money_TextChanged(object sender, EventArgs e)
        {
            UpdateMoney();
        }

        private void cmbOpposingParty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loading) ShowOpposingDetails();
        }

        private void Money_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsMoneyKey(e.KeyChar);
        }

        private void Registry_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsRegistryKey(e.KeyChar);
        }

        // ------------------------------------------------------------------ saving

        private bool ValidateAndCopy()
        {
            errorProvider1.Clear();
            bool ok = true;
            Action<Control, string> fail = (control, message) => { errorProvider1.SetError(control, message); ok = false; };

            var client = cmbClient.SelectedItem as Customer;
            var caseType = cmbCaseType.SelectedItem as CaseType;
            var opposingClient = cmbOpposingParty.SelectedItem as Customer;
            var opposingLawyer = cmbOpposingLawyer.SelectedItem as Customer;
            var court = cmbCourt.SelectedItem as Court;
            bool isCourt = rbCourt.Checked;

            if (client == null) fail(cmbClient, "יש לבחור לקוח");
            if (caseType == null) fail(cmbCaseType, "יש לבחור נושא תיק");
            if (opposingClient == null) fail(cmbOpposingParty, "יש לבחור את הצד שכנגד");
            if (opposingLawyer == null) fail(cmbOpposingLawyer, "יש לבחור עורך דין נגדי");
            if (client != null && opposingClient != null && client.CustomerId == opposingClient.CustomerId)
                fail(cmbOpposingParty, "הלקוח והצד שכנגד הם אותו אדם");
            if (chkPowerOfAttorney.Checked && Validation.IsFutureDate(dtpPowerOfAttorney.Value, Clock.Today))
                fail(dtpPowerOfAttorney, "תאריך חתימה עתידי");

            decimal? claim = null, awarded = null, propertyValue = null;
            if (isCourt)
            {
                if (court == null) fail(cmbCourt, "יש לבחור בית משפט");
                claim = ReadMoney(txtClaimAmount);
                if (claim == null || claim <= 0) fail(txtClaimAmount, "יש להזין סכום תביעה");
                if (txtAwardedAmount.Text.Trim().Length > 0)
                {
                    awarded = ReadMoney(txtAwardedAmount);
                    if (awarded == null) fail(txtAwardedAmount, "סכום לא תקין");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtPropertyAddress.Text)) fail(txtPropertyAddress, "יש להזין כתובת נכס");
                if (string.IsNullOrWhiteSpace(txtBlock.Text)) fail(txtBlock, "יש להזין גוש");
                if (string.IsNullOrWhiteSpace(txtParcel.Text)) fail(txtParcel, "יש להזין חלקה");
                propertyValue = ReadMoney(txtPropertyValue);
                if (propertyValue == null || propertyValue <= 0) fail(txtPropertyValue, "יש להזין שווי נכס");
            }

            double? percent = ReadPercent();
            if (percent == null || !Validation.IsPercent(percent.Value)) fail(txtFeePercent, "אחוז בין 0 ל-100");

            decimal advance = 0m;
            if (txtAdvance.Text.Trim().Length > 0)
            {
                decimal? m = ReadMoney(txtAdvance);
                if (m == null) fail(txtAdvance, "סכום לא תקין");
                else advance = m.Value;
            }

            if (ok && caseType != null)
            {
                decimal total = FeeCalculator.CaseTotal(isCourt ? (claim ?? 0m) : (propertyValue ?? 0m), percent.Value, caseType.CourtFee ?? 0m, caseType.BaseFee ?? 0m);
                if (advance > total && total > 0) fail(txtAdvance, "המקדמה גדולה מסכום התיק (" + Ui.FormatMoney(total) + ")");
                decimal paid = isNew ? 0m : CaseService.PaidTotal(legalCase);
                if (ok && paid > total && !Ui.Confirm("סכום התיק החדש (" + Ui.FormatMoney(total) + ") נמוך ממה שהלקוח כבר שילם (" +
                        Ui.FormatMoney(paid) + ").\nאחרי השמירה אפשר לרשום החזר של העודף בלשונית התשלומים. להמשיך?"))
                    return false;
            }

            if (!ok) return false;

            if (client != null && caseType != null)
            {
                LegalCase duplicate = new CaseService(db).FindOpenDuplicate(client.CustomerId, caseType.CaseTypeId, isNew ? 0 : legalCase.CaseId);
                if (duplicate != null && (isNew || legalCase.ClientId != client.CustomerId || legalCase.CaseTypeId != caseType.CaseTypeId) &&
                    !Ui.Confirm("ללקוח זה כבר יש תיק פתוח מאותו נושא (תיק מספר " + duplicate.CaseId + "). לפתוח תיק נוסף?"))
                    return false;
            }

            legalCase.ClientId = client.CustomerId;
            legalCase.CaseTypeId = caseType.CaseTypeId;
            legalCase.OpposingPartyId = opposingClient.CustomerId;
            legalCase.OpposingLawyerId = opposingLawyer.CustomerId;
            legalCase.PowerOfAttorney = chkPowerOfAttorney.Checked ? SignedPrefix + dtpPowerOfAttorney.Value.ToString("dd/MM/yyyy") : "";
            legalCase.IsInCourt = isCourt;
            legalCase.CourtId = isCourt ? court.CourtId : (int?)null;
            legalCase.ClaimAmount = isCourt ? claim : propertyValue;
            legalCase.AwardedAmount = isCourt ? awarded : null;
            legalCase.PropertyAddress = isCourt ? null : txtPropertyAddress.Text.Trim();
            legalCase.Block = isCourt ? null : txtBlock.Text.Trim();
            legalCase.Parcel = isCourt ? null : txtParcel.Text.Trim();
            legalCase.SubParcel = isCourt || txtSubParcel.Text.Trim().Length == 0 ? null : txtSubParcel.Text.Trim();
            legalCase.FeePercent = percent;
            legalCase.Advance = advance;
            CaseService.RefreshPaymentFlags(legalCase); // never typed by hand
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateAndCopy()) return;
            try
            {
                bool wasNew = isNew;
                // Business rules, the timeline of a new case and the audit entry live in the service.
                string error = new CaseService(db).SaveCase(legalCase, cmbCaseType.SelectedItem as CaseType, isNew, Clock.Today);
                if (error != null)
                {
                    Ui.Error(error);
                    return;
                }
                dirty = false;
                LoadCase(legalCase.CaseId);
                Ui.Notify(this, wasNew ? "התיק נפתח. מספר תיק: " + legalCase.CaseId : "התיק עודכן.");
                Changed = true;
                WarnAboutConflicts();
            }
            catch (Exception ex)
            {
                HandleError("התיק לא נשמר.", ex);
            }
        }

        /// <summary>The number of the case shown (0 while a new case is not saved yet).</summary>
        public int CaseId
        {
            get { return isNew || legalCase == null ? 0 : legalCase.CaseId; }
        }

        /// <summary>True after anything was saved, so the list that opened this window can refresh.</summary>
        public bool Changed { get; private set; }

        /// <summary>
        /// Shows the error; on a concurrency conflict (another user changed this case) reloads the case
        /// so the user sees the current data instead of keeping a stale copy.
        /// </summary>
        private void HandleError(string message, Exception ex)
        {
            bool conflict = ex is System.Data.Entity.Infrastructure.DbUpdateConcurrencyException || ex is System.Data.Entity.Infrastructure.DbUpdateException;
            if (!isNew && conflict)
            {
                bool exists;
                try
                {
                    using (var check = new QuedinDbContext())
                        exists = new CaseService(check).CaseExists(legalCase.CaseId);
                }
                catch (Exception)
                {
                    exists = true; // cannot tell; show the original error
                }
                if (!exists)
                {
                    CaseDeleted();
                    return;
                }
            }
            Ui.Error(message, ex);
            if (!isNew && conflict)
            {
                dirty = false;
                LoadCase(legalCase.CaseId);
            }
        }

        /// <summary>Warns when a calculated step now falls after a court-set date (the office must decide what to do).</summary>
        private void WarnAboutConflicts()
        {
            if (isNew) return;
            List<CaseStep> conflicts = new CaseService(db).ScheduleConflicts(legalCase);
            if (conflicts.Count == 0) return;
            Ui.Error("שימו לב: לפי החישוב, השלבים הבאים יגיעו אחרי מועד שקבע בית המשפט:\n" +
                     string.Join("\n", conflicts.Select(c => "• " + StepName(c) + " - " + Ui.FormatDate(c.DueDate))) +
                     "\nכדאי לקבוע להם מועד ידני או לבקש דחייה.");
        }

        /// <summary>Runs a case operation, shows its error or a short confirmation and reloads the case.</summary>
        private void RunOperation(Func<CaseService, string> operation, string done, TabPage tab)
        {
            try
            {
                string error = operation(new CaseService(db));
                if (error != null)
                {
                    Ui.Error(error);
                    LoadCase(legalCase.CaseId);
                }
                else
                {
                    Changed = true;
                    LoadCase(legalCase.CaseId);
                    Ui.Notify(this, done);
                    WarnAboutConflicts();
                }
                tabsCase.SelectedTab = tab;
            }
            catch (Exception ex)
            {
                HandleError("הפעולה לא נשמרה.", ex);
                tabsCase.SelectedTab = tab;
            }
        }

        private void btnDeleteCase_Click(object sender, EventArgs e)
        {
            if (!Ui.Confirm("למחוק את תיק מספר " + legalCase.CaseId + "?\nמחיקה אפשרית רק לתיק שנפתח בטעות - לפני תשלומים ולפני שסומן בו שלב. לא ניתן לשחזר.")) return;
            try
            {
                string error = new CaseService(db).DeleteCase(legalCase);
                if (error != null) { Ui.Error(error); return; }
                Changed = true;
                Close();
            }
            catch (Exception ex)
            {
                HandleError("התיק לא נמחק.", ex);
            }
        }

        // ------------------------------------------------------------------ steps & payments

        private CaseStep NextStep()
        {
            return CaseService.OrderedSteps(legalCase).FirstOrDefault(s => s.IsDone != true);
        }

        private static string StepName(CaseStep step)
        {
            return step.Procedure != null ? step.Procedure.Name : "";
        }

        private CaseStep SelectedStep()
        {
            if (listSteps.SelectedItems.Count == 0) return null;
            int id = (int)listSteps.SelectedItems[0].Tag;
            return legalCase.Steps.FirstOrDefault(s => s.CaseStepId == id);
        }

        private void btnCompleteStep_Click(object sender, EventArgs e)
        {
            CaseStep next = NextStep();
            if (next == null) return;

            CaseStep previous = CaseService.OrderedSteps(legalCase).LastOrDefault(s => s.IsDone == true);
            DateTime? min = previous != null ? previous.DoneDate : null;
            DateTime doneOn;
            string unused;
            if (!Ui.AskDate(this, "סימון שלב כבוצע",
                    "באיזה תאריך בוצע השלב \"" + StepName(next) + "\"?\nהמועדים של כל השלבים הבאים יחושבו מתאריך זה.",
                    Clock.Today, min, Clock.Today, null, null, out doneOn, out unused)) return;

            RunOperation(svc => svc.CompleteStep(legalCase, next.CaseStepId, doneOn),
                "השלב \"" + StepName(next) + "\" סומן כבוצע.", tabSteps);
        }

        private void btnReschedule_Click(object sender, EventArgs e)
        {
            CaseStep step = SelectedStep();
            if (step == null || step.IsDone == true) step = NextStep();
            if (step == null) return;

            DateTime newDate;
            string note;
            if (!Ui.AskDate(this, "שינוי מועד",
                    "מועד חדש לשלב \"" + StepName(step) + "\" (למשל מועד דיון שנקבע ע\"י בית המשפט).\nהמועד יסומן כקבוע: החישוב האוטומטי לא יזיז אותו, והשלבים הבאים יחושבו ממנו.",
                    step.DueDate ?? Clock.Today, Clock.Today.AddYears(-CaseService.MaxYearsAway), Clock.Today.AddYears(CaseService.MaxYearsAway), "סיבת השינוי (לא חובה)", step.Notes, out newDate, out note)) return;

            if (!BusinessCalendar.IsWorkday(newDate))
            {
                string holiday = BusinessCalendar.HolidayName(newDate);
                if (!Ui.Confirm("המועד שנבחר הוא " + (holiday ?? Ui.DayName(newDate)) + ". לשמור בכל זאת?")) return;
            }

            RunOperation(svc => svc.RescheduleStep(legalCase, step.CaseStepId, newDate, note),
                "המועד עודכן ל-" + newDate.ToString("dd/MM/yyyy") + ".", tabSteps);
        }

        private void btnStepNote_Click(object sender, EventArgs e)
        {
            CaseStep step = SelectedStep() ?? NextStep();
            if (step == null) return;
            string note = Ui.AskText(this, "הערה לשלב", "הערה לשלב \"" + StepName(step) + "\" (עד 200 תווים):", step.Notes, 200);
            if (note == null) return;
            RunOperation(svc => svc.SetNote(legalCase, step.CaseStepId, note), "ההערה נשמרה.", tabSteps);
        }

        private void btnUndoStep_Click(object sender, EventArgs e)
        {
            CaseStep last = CaseService.OrderedSteps(legalCase).LastOrDefault(s => s.IsDone == true);
            if (last == null) return;
            if (!Ui.Confirm("לבטל את סימון השלב \"" + StepName(last) + "\" כבוצע?\nהשלב יחזור להיות השלב הבא, ומועדי השלבים שאחריו יחושבו מחדש.")) return;
            RunOperation(svc => svc.UndoLastStep(legalCase), "סימון השלב בוטל.", tabSteps);
        }

        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            using (var form = new PaymentForm(legalCase.CaseId))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Changed = true;
                    Ui.Notify(this, "התשלום נרשם. קבלה מספר " + form.SavedPaymentId + ".");
                }
            }
            LoadCase(legalCase.CaseId);
            tabsCase.SelectedTab = tabPayments;
        }

        private void btnCancelPayment_Click(object sender, EventArgs e)
        {
            if (listPayments.SelectedItems.Count == 0)
            {
                Ui.Error("יש לבחור תשלום מהרשימה.");
                return;
            }
            int id = (int)listPayments.SelectedItems[0].Tag;
            Payment p = legalCase.Payments.First(x => x.PaymentId == id);
            if (p.IsCancelled) { Ui.Error("התשלום כבר בוטל."); return; }
            if (p.CancelsPaymentId.HasValue) { Ui.Error("זו רשומת זיכוי של ביטול קודם, ואותה לא ניתן לבטל."); return; }

            string reason = Ui.AskText(this, "ביטול תשלום",
                "ביטול קבלה " + p.PaymentId + " על סך " + Ui.FormatMoney(p.Amount) + ".\nהתשלום לא יימחק: תירשם קבלת זיכוי בסכום שלילי והיתרה תתעדכן.\nסיבת הביטול:",
                "", 40);
            if (reason == null) return;
            RunOperation(svc => svc.CancelPayment(legalCase, id, reason), "התשלום בוטל ונרשם זיכוי.", tabPayments);
            Payment cancelled = legalCase.Payments.FirstOrDefault(x => x.PaymentId == id);
            if (cancelled != null && cancelled.IsCancelled && Ui.Confirm("לרשום עכשיו את התשלום הנכון במקום זה שבוטל?"))
                btnAddPayment_Click(sender, e);
        }

        private void btnRefund_Click(object sender, EventArgs e)
        {
            decimal overpaid = -CaseService.OpenBalance(legalCase);
            if (overpaid <= 0) return;
            string method = Ui.AskChoice(this, "החזר עודף ללקוח",
                "הלקוח שילם " + Ui.FormatMoney(overpaid) + " מעבר לסכום התיק.\nתירשם קבלת זיכוי על הסכום. באיזה אמצעי מוחזר הכסף?",
                PaymentMethods.All, PaymentMethods.BankTransfer);
            if (method == null) return;
            RunOperation(svc => svc.RefundOverpayment(legalCase, method, null), "נרשם החזר של " + Ui.FormatMoney(overpaid) + " ללקוח.", tabPayments);
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            if (listPayments.SelectedItems.Count == 0)
            {
                Ui.Error("יש לבחור תשלום מהרשימה.");
                return;
            }
            using (var form = new ReceiptForm((int)listPayments.SelectedItems[0].Tag))
                form.ShowDialog(this);
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
