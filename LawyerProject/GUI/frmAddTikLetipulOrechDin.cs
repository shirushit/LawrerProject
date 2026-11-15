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
    public partial class frmAddTikLetipulOrechDin : Form
    {
        private LawyerDBEntities1 db;
        private Tikim_Letipul_Orech_Din tik;
        private bool isNew;
        private bool loading;

        /// <summary>New court case.</summary>
        public frmAddTikLetipulOrechDin() : this(false)
        {
        }

        /// <summary>New case; <paramref name="isDeal"/> = property transaction instead of a court case.</summary>
        public frmAddTikLetipulOrechDin(bool isDeal)
        {
            InitializeComponent();
            isNew = true;
            db = new LawyerDBEntities1();
            tik = new Tikim_Letipul_Orech_Din();
            LoadLists();
            rbDeal.Checked = isDeal;
            rbCourt.Checked = !isDeal;
            UpdateMode();
        }

        /// <summary>Existing case.</summary>
        public frmAddTikLetipulOrechDin(int id)
        {
            InitializeComponent();
            LoadCase(id);
        }

        // ------------------------------------------------------------------ loading

        private void LoadCase(int id)
        {
            if (db != null) db.Dispose();
            db = new LawyerDBEntities1();
            tik = db.Tikim_Letipul_Orech_Din.FirstOrDefault(t => t.IDTik == id);
            if (tik == null) throw new ArgumentException("Case not found: " + id);
            isNew = false;
            LoadLists();
            FillControls();
            FillSteps();
            FillPayments();
            UpdateMode();
        }

        private void LoadLists()
        {
            loading = true;
            List<Customers> people = db.Customers.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();
            // Clients first; any person can be an opposing party or lawyer (a client in one case
            // may be the other side in another), so those lists show everyone, their own type first.
            cmbCustomer.DisplayMember = cmbCustemerNegdi.DisplayMember = cmbOrechDinNegdi.DisplayMember = "ListText";
            cmbCustomer.DataSource = people.OrderBy(c => c.SugPerson == PersonTypes.Client ? 0 : 1).ToList();
            cmbCustemerNegdi.DataSource = people.OrderBy(c => c.SugPerson == PersonTypes.OpposingClient ? 0 : 1).ToList();
            cmbOrechDinNegdi.DataSource = people.OrderBy(c => c.SugPerson == PersonTypes.OpposingLawyer ? 0 : 1).ToList();
            cmbSubjektTik.DataSource = db.Subjekt_Tik.OrderBy(s => s.NameSugTik).ToList();
            cmbCustomer.SelectedIndex = cmbCustemerNegdi.SelectedIndex = cmbOrechDinNegdi.SelectedIndex = -1;
            if (isNew) cmbSubjektTik.SelectedIndex = -1;
            loading = false;
            cmbSubjektTik.SelectedIndexChanged -= cmbSubjektTik_SelectedIndexChanged;
            cmbSubjektTik.SelectedIndexChanged += cmbSubjektTik_SelectedIndexChanged;
            cmbCustemerNegdi.SelectedIndexChanged -= cmbCustemerNegdi_SelectedIndexChanged;
            cmbCustemerNegdi.SelectedIndexChanged += cmbCustemerNegdi_SelectedIndexChanged;
            LoadCourts();
        }

        /// <summary>Courts that handle the selected case type; all courts if none is linked yet.</summary>
        private void LoadCourts()
        {
            var subject = cmbSubjektTik.SelectedItem as Subjekt_Tik;
            List<Bet_Mishpat> courts = null;
            if (subject != null)
                courts = db.Subjekts_Shebet_Mishpat_Metapel
                    .Where(s => s.IDSubjekt == subject.IDSugTik)
                    .Select(s => s.Bet_Mishpat)
                    .Distinct()
                    .OrderBy(b => b.NameBetMishpat)
                    .ToList();
            if (courts == null || courts.Count == 0)
                courts = db.Bet_Mishpat.OrderBy(b => b.NameBetMishpat).ToList();

            object previous = cmbBetMishpat.SelectedItem;
            cmbBetMishpat.DataSource = courts;
            cmbBetMishpat.SelectedItem = previous;
            if (cmbBetMishpat.SelectedItem == null) cmbBetMishpat.SelectedIndex = -1;
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
            Text = "תיק מספר " + tik.IDTik;
            lblCodeTik.Text = tik.IDTik.ToString();
            Select(cmbCustomer, o => ((Customers)o).IDcustemer == tik.IDCostemer);
            Select(cmbSubjektTik, o => ((Subjekt_Tik)o).IDSugTik == tik.IDSubjektTik);
            Select(cmbCustemerNegdi, o => ((Customers)o).IDcustemer == tik.KodLakoachNegdi);
            Select(cmbOrechDinNegdi, o => ((Customers)o).IDcustemer == tik.KodOrechDinNegdi);
            loading = false;
            LoadCourts();
            Select(cmbBetMishpat, o => ((Bet_Mishpat)o).IDBetMishpat == tik.IDBetMishpat);

            rbCourt.Checked = tik.IsCourtCase;
            rbDeal.Checked = !tik.IsCourtCase;
            DateTime signed;
            chkIpuy.Checked = TryReadSigned(tik.IpuiKohach, out signed);
            dtpIpuy.Value = chkIpuy.Checked ? signed : DateTime.Today;
            dtpIpuy.Enabled = chkIpuy.Checked;

            string money = tik.SchumLetvia.HasValue ? tik.SchumLetvia.Value.ToString("0.##", CultureInfo.InvariantCulture) : "";
            txtSchumKetvia.Text = tik.IsCourtCase ? money : "";
            txtPropertyValue.Text = tik.IsCourtCase ? "" : money;
            txtSchumShenitba.Text = tik.SchumShenitba.HasValue ? tik.SchumShenitba.Value.ToString("0.##", CultureInfo.InvariantCulture) : "";
            txtAdressNeches.Text = tik.AdressNeches;
            txtGush.Text = tik.Gush;
            txtChelka.Text = tik.Chelka;
            txtTatChelka.Text = tik.TatChelka;

            txtAchuzRevachim.Text = tik.AchuzRevachim.HasValue ? tik.AchuzRevachim.Value.ToString(CultureInfo.InvariantCulture) : "";
            txtMikdama.Text = tik.Mikdama > 0 ? tik.Mikdama.ToString("0.##", CultureInfo.InvariantCulture) : "";

            lblStatusTik.Text = CaseService.IsFinished(tik)
                ? "הסתיים"
                : (tik.Taalichim_Letikim != null ? tik.Taalichim_Letikim.NameTaalich : "-");
            ShowOpposingDetails();
            UpdateMoney();
        }

        private const string SignedPrefix = "נחתם ";

        /// <summary>Power of attorney is stored as "נחתם dd/MM/yyyy", or "" when not signed yet.</summary>
        private static bool TryReadSigned(string value, out DateTime date)
        {
            date = DateTime.Today;
            if (string.IsNullOrWhiteSpace(value)) return false;
            string v = value.Trim();
            if (v.StartsWith(SignedPrefix)) v = v.Substring(SignedPrefix.Length);
            if (DateTime.TryParseExact(v, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)) return true;
            date = DateTime.Today;
            return true; // older free text such as "כן" counts as signed
        }

        private void chkIpuy_CheckedChanged(object sender, EventArgs e)
        {
            dtpIpuy.Enabled = chkIpuy.Checked;
        }

        private void ShowOpposingDetails()
        {
            var opposing = cmbCustemerNegdi.SelectedItem as Customers;
            if (opposing == null) { lblOpposingDetails.Text = ""; return; }
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(opposing.SugMezahe) && opposing.SugMezahe != IdTypes.None)
                parts.Add(opposing.SugMezahe + " " + opposing.IDcustemer);
            if (!string.IsNullOrEmpty(opposing.SmartPhone)) parts.Add(opposing.SmartPhone);
            else if (!string.IsNullOrEmpty(opposing.Telephone)) parts.Add(opposing.Telephone);
            lblOpposingDetails.Text = parts.Count == 0 ? "אין פרטי זיהוי או קשר" : string.Join("  |  ", parts);
        }

        private void FillSteps()
        {
            listView1.Items.Clear();
            DateTime today = DateTime.Today;
            Tahalich_Tipul_Betik next = CaseService.OrderedSteps(tik).FirstOrDefault(s => s.Buzha != true);
            int number = 1;
            foreach (Tahalich_Tipul_Betik step in CaseService.OrderedSteps(tik))
            {
                bool done = step.Buzha == true;
                bool late = !done && step.TaharichMeshoar.HasValue && step.TaharichMeshoar.Value.Date < today;
                string status = done ? "בוצע" : late ? "באיחור" : (step == next ? "השלב הבא" : "ממתין");
                string lateText = "";
                if (late) lateText = CaseScheduler.DaysLate(step.TaharichMeshoar.Value, today) + " ימים";
                else if (done && step.TaharichBizhua.HasValue && step.TaharichMeshoar.HasValue && step.TaharichBizhua.Value > step.TaharichMeshoar.Value)
                    lateText = "בוצע באיחור";

                var item = new ListViewItem(new[]
                {
                    number.ToString(),
                    step.Taalichim_Letikim != null ? step.Taalichim_Letikim.NameTaalich : step.IDTahalich.ToString(),
                    Ui.FormatDate(step.TaharichMeshoar),
                    done ? Ui.FormatDate(step.TaharichBizhua) : "",
                    status,
                    lateText,
                    step.Hearot ?? ""
                });
                item.Tag = step.IDTahalichTipulLetik;
                if (done) item.ForeColor = SystemColors.GrayText;
                else if (late) item.BackColor = Color.MistyRose;
                if (step == next) item.Font = new Font(listView1.Font, FontStyle.Bold);
                listView1.Items.Add(item);
                number++;
            }
            btnCompleteStep.Enabled = next != null;
            btnReschedule.Enabled = next != null;
            btnStepNote.Enabled = listView1.Items.Count > 0;
            btnUndoStep.Visible = Session.IsManager;
            btnUndoStep.Enabled = tik.Tahalich_Tipul_Betik.Any(st => st.Buzha == true);
        }

        private void FillPayments()
        {
            listView2.Items.Clear();
            foreach (Tashlumim_Letik p in tik.Tashlumim_Letik.OrderBy(p => p.IDTashlum))
            {
                string status = p.Mevutal ? "בוטל" : (p.MevatelTashlum.HasValue ? "זיכוי" : "");
                var item = new ListViewItem(new[]
                {
                    p.IDTashlum.ToString(),
                    Ui.FormatDate(p.TaarichTashlum),
                    p.SugTashlum ?? PaymentKinds.Fee,
                    p.EmzhaeyTashlum,
                    Ui.FormatMoney(p.Schum),
                    Ui.FormatMoney(p.Itra),
                    status,
                    p.Teur
                });
                item.Tag = p.IDTashlum;
                if (p.Mevutal || p.MevatelTashlum.HasValue) item.ForeColor = SystemColors.GrayText;
                listView2.Items.Add(item);
            }
            btnReceipt.Enabled = listView2.Items.Count > 0;
            btnCancelPayment.Enabled = listView2.Items.Count > 0;
        }

        // ------------------------------------------------------------------ UI state

        private void UpdateMode()
        {
            bool court = rbCourt.Checked;
            groupBoxBetMishpat.Visible = court;
            groupBoxIska.Visible = !court;

            // The fixed steps come from the case type, so it is chosen once, when the case is opened.
            cmbSubjektTik.Enabled = isNew;
            lblSubjektLock.Visible = !isNew;
            tabControl1.Enabled = !isNew;
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
            return double.TryParse(txtAchuzRevachim.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out value)
                ? value : (double?)null;
        }

        /// <summary>Recalculates the case total and the open balance shown on screen.</summary>
        private void UpdateMoney()
        {
            if (loading) return;
            decimal? amount = ReadMoney(rbCourt.Checked ? txtSchumKetvia : txtPropertyValue);
            double? percent = ReadPercent();
            var subject = cmbSubjektTik.SelectedItem as Subjekt_Tik;

            if (amount == null || percent == null || !Validation.IsPercent(percent.Value))
            {
                lblTotal.Text = "-";
                lblBalance.Text = "-";
                return;
            }

            decimal total = FeeCalculator.CaseTotal(amount.Value, percent.Value, subject != null ? (subject.Agra ?? 0m) : 0m);
            decimal paid = isNew ? 0m : CaseService.PaidTotal(tik);
            decimal balance = FeeCalculator.Balance(total, new[] { paid });
            lblTotal.Text = Ui.FormatMoney(total);
            lblPaid.Text = Ui.FormatMoney(paid);
            lblBalance.Text = balance < 0 ? Ui.FormatMoney(balance) + "  (שולם יותר מהסכום)" : Ui.FormatMoney(balance);
            lblBalance.ForeColor = balance <= 0 && total > 0 ? Color.DarkGreen : SystemColors.ControlText;

            decimal? mikdama = txtMikdama.Text.Trim().Length == 0 ? 0m : ReadMoney(txtMikdama);
            lblMikdamaStatus.Text = mikdama == null || mikdama == 0 ? "" : (paid >= mikdama ? "שולמה" : "טרם שולמה");
        }

        private bool reverting;

        private void CaseType_CheckedChanged(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;
            if (!rb.Checked || loading || reverting) { if (rb.Checked) UpdateMode(); return; }

            // Switching hides the other group; warn when it already holds data that will not be saved.
            bool toCourt = rb == rbCourt;
            bool hiddenHasData = toCourt
                ? new[] { txtAdressNeches, txtGush, txtChelka, txtTatChelka, txtPropertyValue }.Any(t => t.Text.Trim().Length > 0)
                : cmbBetMishpat.SelectedItem != null || new[] { txtSchumKetvia, txtSchumShenitba }.Any(t => t.Text.Trim().Length > 0);
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

        private void cmbSubjektTik_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loading) return;
            LoadCourts();
            UpdateMoney();
        }

        private void Money_TextChanged(object sender, EventArgs e)
        {
            UpdateMoney();
        }

        private void cmbCustemerNegdi_SelectedIndexChanged(object sender, EventArgs e)
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

            var client = cmbCustomer.SelectedItem as Customers;
            var subject = cmbSubjektTik.SelectedItem as Subjekt_Tik;
            var opposingClient = cmbCustemerNegdi.SelectedItem as Customers;
            var opposingLawyer = cmbOrechDinNegdi.SelectedItem as Customers;
            var court = cmbBetMishpat.SelectedItem as Bet_Mishpat;
            bool isCourt = rbCourt.Checked;

            if (client == null) fail(cmbCustomer, "יש לבחור לקוח");
            if (subject == null) fail(cmbSubjektTik, "יש לבחור נושא תיק");
            if (opposingClient == null) fail(cmbCustemerNegdi, "יש לבחור את הצד שכנגד");
            if (opposingLawyer == null) fail(cmbOrechDinNegdi, "יש לבחור עורך דין נגדי");
            if (client != null && opposingClient != null && client.IDcustemer == opposingClient.IDcustemer)
                fail(cmbCustemerNegdi, "הלקוח והצד שכנגד הם אותו אדם");
            if (chkIpuy.Checked && Validation.IsFutureDate(dtpIpuy.Value, DateTime.Today))
                fail(dtpIpuy, "תאריך חתימה עתידי");

            decimal? claim = null, awarded = null, propertyValue = null;
            if (isCourt)
            {
                if (court == null) fail(cmbBetMishpat, "יש לבחור בית משפט");
                claim = ReadMoney(txtSchumKetvia);
                if (claim == null || claim <= 0) fail(txtSchumKetvia, "יש להזין סכום תביעה");
                if (txtSchumShenitba.Text.Trim().Length > 0)
                {
                    awarded = ReadMoney(txtSchumShenitba);
                    if (awarded == null) fail(txtSchumShenitba, "סכום לא תקין");
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtAdressNeches.Text)) fail(txtAdressNeches, "יש להזין כתובת נכס");
                if (string.IsNullOrWhiteSpace(txtGush.Text)) fail(txtGush, "יש להזין גוש");
                if (string.IsNullOrWhiteSpace(txtChelka.Text)) fail(txtChelka, "יש להזין חלקה");
                propertyValue = ReadMoney(txtPropertyValue);
                if (propertyValue == null || propertyValue <= 0) fail(txtPropertyValue, "יש להזין שווי נכס");
            }

            double? percent = ReadPercent();
            if (percent == null || !Validation.IsPercent(percent.Value)) fail(txtAchuzRevachim, "אחוז בין 0 ל-100");

            decimal mikdama = 0m;
            if (txtMikdama.Text.Trim().Length > 0)
            {
                decimal? m = ReadMoney(txtMikdama);
                if (m == null) fail(txtMikdama, "סכום לא תקין");
                else mikdama = m.Value;
            }

            if (!ok) return false;

            tik.IDCostemer = client.IDcustemer;
            tik.IDSubjektTik = subject.IDSugTik;
            tik.KodLakoachNegdi = opposingClient.IDcustemer;
            tik.KodOrechDinNegdi = opposingLawyer.IDcustemer;
            tik.IpuiKohach = chkIpuy.Checked ? SignedPrefix + dtpIpuy.Value.ToString("dd/MM/yyyy") : "";
            tik.IsTheTikMenualInBETMishpat = isCourt;
            tik.IDBetMishpat = isCourt ? court.IDBetMishpat : (int?)null;
            tik.SchumLetvia = isCourt ? claim : propertyValue;
            tik.SchumShenitba = isCourt ? awarded : null;
            tik.AdressNeches = isCourt ? null : txtAdressNeches.Text.Trim();
            tik.Gush = isCourt ? null : txtGush.Text.Trim();
            tik.Chelka = isCourt ? null : txtChelka.Text.Trim();
            tik.TatChelka = isCourt || txtTatChelka.Text.Trim().Length == 0 ? null : txtTatChelka.Text.Trim();
            tik.AchuzRevachim = percent;
            tik.Mikdama = mikdama;
            CaseService.RefreshPaymentFlags(tik); // never typed by hand
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateAndCopy()) return;
            try
            {
                if (isNew)
                {
                    string error = new CaseService(db).CreatePlan(tik, DateTime.Today);
                    if (error != null)
                    {
                        Ui.Error(error);
                        return;
                    }
                    db.Tikim_Letipul_Orech_Din.Add(tik);
                }
                bool wasNew = isNew;
                AuditService.Log(db, wasNew ? AuditActions.CaseOpened : AuditActions.CaseUpdated, "תיק", tik.IDTik == 0 ? null : (object)tik.IDTik, null);
                db.SaveChanges();
                LoadCase(tik.IDTik);
                Ui.Notify(this, wasNew ? "התיק נפתח. מספר תיק: " + tik.IDTik : "התיק עודכן.");
                Changed = true;
            }
            catch (Exception ex)
            {
                HandleError("התיק לא נשמר.", ex);
            }
        }

        /// <summary>True after anything was saved, so the list that opened this window can refresh.</summary>
        public bool Changed { get; private set; }

        /// <summary>
        /// Shows the error; on a concurrency conflict (another user changed this case) reloads the case
        /// so the user sees the current data instead of keeping a stale copy.
        /// </summary>
        private void HandleError(string message, Exception ex)
        {
            Ui.Error(message, ex);
            if (!isNew && (ex is System.Data.Entity.Infrastructure.DbUpdateConcurrencyException || ex is System.Data.Entity.Infrastructure.DbUpdateException))
                LoadCase(tik.IDTik);
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
                    LoadCase(tik.IDTik);
                }
                else
                {
                    Changed = true;
                    LoadCase(tik.IDTik);
                    Ui.Notify(this, done);
                }
                tabControl1.SelectedTab = tab;
            }
            catch (Exception ex)
            {
                HandleError("הפעולה לא נשמרה.", ex);
                tabControl1.SelectedTab = tab;
            }
        }

        private void btnDeleteCase_Click(object sender, EventArgs e)
        {
            if (!Ui.Confirm("למחוק את תיק מספר " + tik.IDTik + "?\nמחיקה אפשרית רק לתיק שנפתח בטעות - לפני תשלומים ולפני שסומן בו שלב. לא ניתן לשחזר.")) return;
            try
            {
                string error = new CaseService(db).DeleteCase(tik);
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

        private Tahalich_Tipul_Betik NextStep()
        {
            return CaseService.OrderedSteps(tik).FirstOrDefault(s => s.Buzha != true);
        }

        private static string StepName(Tahalich_Tipul_Betik step)
        {
            return step.Taalichim_Letikim != null ? step.Taalichim_Letikim.NameTaalich : "";
        }

        private Tahalich_Tipul_Betik SelectedStep()
        {
            if (listView1.SelectedItems.Count == 0) return null;
            int id = (int)listView1.SelectedItems[0].Tag;
            return tik.Tahalich_Tipul_Betik.FirstOrDefault(s => s.IDTahalichTipulLetik == id);
        }

        private void btnCompleteStep_Click(object sender, EventArgs e)
        {
            Tahalich_Tipul_Betik next = NextStep();
            if (next == null) return;

            Tahalich_Tipul_Betik previous = CaseService.OrderedSteps(tik).LastOrDefault(s => s.Buzha == true);
            DateTime? min = previous != null ? previous.TaharichBizhua : null;
            DateTime doneOn;
            string unused;
            if (!Ui.AskDate(this, "סימון שלב כבוצע",
                    "באיזה תאריך בוצע השלב \"" + StepName(next) + "\"?\nהמועדים של כל השלבים הבאים יחושבו מתאריך זה.",
                    DateTime.Today, min, DateTime.Today, null, null, out doneOn, out unused)) return;

            RunOperation(svc => svc.CompleteStep(tik, next.IDTahalichTipulLetik, doneOn),
                "השלב \"" + StepName(next) + "\" סומן כבוצע.", tabSteps);
        }

        private void btnReschedule_Click(object sender, EventArgs e)
        {
            Tahalich_Tipul_Betik step = SelectedStep();
            if (step == null || step.Buzha == true) step = NextStep();
            if (step == null) return;

            DateTime newDate;
            string note;
            if (!Ui.AskDate(this, "שינוי מועד",
                    "מועד חדש לשלב \"" + StepName(step) + "\" (למשל מועד דיון שנקבע ע\"י בית המשפט).\nהשלבים הבאים יחושבו מהמועד החדש.",
                    step.TaharichMeshoar ?? DateTime.Today, null, null, "סיבת השינוי (לא חובה)", step.Hearot, out newDate, out note)) return;

            if (!BusinessCalendar.IsWorkday(newDate) && !Ui.Confirm("המועד שנבחר הוא " + Ui.DayName(newDate) + ". לשמור בכל זאת?")) return;

            RunOperation(svc => svc.RescheduleStep(tik, step.IDTahalichTipulLetik, newDate, note),
                "המועד עודכן ל-" + newDate.ToString("dd/MM/yyyy") + ".", tabSteps);
        }

        private void btnStepNote_Click(object sender, EventArgs e)
        {
            Tahalich_Tipul_Betik step = SelectedStep() ?? NextStep();
            if (step == null) return;
            string note = Ui.AskText(this, "הערה לשלב", "הערה לשלב \"" + StepName(step) + "\" (עד 200 תווים):", step.Hearot, 200);
            if (note == null) return;
            RunOperation(svc => svc.SetNote(tik, step.IDTahalichTipulLetik, note), "ההערה נשמרה.", tabSteps);
        }

        private void btnUndoStep_Click(object sender, EventArgs e)
        {
            Tahalich_Tipul_Betik last = CaseService.OrderedSteps(tik).LastOrDefault(s => s.Buzha == true);
            if (last == null) return;
            if (!Ui.Confirm("לבטל את סימון השלב \"" + StepName(last) + "\" כבוצע?\nהשלב יחזור להיות השלב הבא, ומועדי השלבים שאחריו יחושבו מחדש.")) return;
            RunOperation(svc => svc.UndoLastStep(tik), "סימון השלב בוטל.", tabSteps);
        }

        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            using (var form = new frmAddTashlumimLetik(tik.IDTik))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Changed = true;
                    Ui.Notify(this, "התשלום נרשם. קבלה מספר " + form.SavedPaymentId + ".");
                }
            }
            LoadCase(tik.IDTik);
            tabControl1.SelectedTab = tabPayments;
        }

        private void btnCancelPayment_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
            {
                Ui.Error("יש לבחור תשלום מהרשימה.");
                return;
            }
            int id = (int)listView2.SelectedItems[0].Tag;
            Tashlumim_Letik p = tik.Tashlumim_Letik.First(x => x.IDTashlum == id);
            if (p.Mevutal) { Ui.Error("התשלום כבר בוטל."); return; }
            if (p.MevatelTashlum.HasValue) { Ui.Error("זו רשומת זיכוי של ביטול קודם, ואותה לא ניתן לבטל."); return; }

            string reason = Ui.AskText(this, "ביטול תשלום",
                "ביטול קבלה " + p.IDTashlum + " על סך " + Ui.FormatMoney(p.Schum) + ".\nהתשלום לא יימחק: תירשם קבלת זיכוי בסכום שלילי והיתרה תתעדכן.\nסיבת הביטול:",
                "", 40);
            if (reason == null) return;
            RunOperation(svc => svc.CancelPayment(tik, id, reason), "התשלום בוטל ונרשם זיכוי.", tabPayments);
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
            {
                Ui.Error("יש לבחור תשלום מהרשימה.");
                return;
            }
            using (var form = new frmKabala((int)listView2.SelectedItems[0].Tag))
                form.ShowDialog(this);
        }

        // ------------------------------------------------------------------ quick-add people

        private void AddPerson(string personType, ComboBox target)
        {
            using (var form = new frmAddCostemer(personType))
            {
                if (form.ShowDialog() != DialogResult.OK) return;
                string id = form.SavedId;
                object[] keep = { cmbCustomer.SelectedItem, cmbCustemerNegdi.SelectedItem, cmbOrechDinNegdi.SelectedItem };
                var subject = cmbSubjektTik.SelectedItem as Subjekt_Tik;
                var court = cmbBetMishpat.SelectedItem as Bet_Mishpat;
                LoadLists();
                Changed = true;
                loading = true;
                Select(cmbSubjektTik, o => subject != null && ((Subjekt_Tik)o).IDSugTik == subject.IDSugTik);
                loading = false;
                LoadCourts();
                Select(cmbBetMishpat, o => court != null && ((Bet_Mishpat)o).IDBetMishpat == court.IDBetMishpat);
                Select(cmbCustomer, o => keep[0] != null && ((Customers)o).IDcustemer == ((Customers)keep[0]).IDcustemer);
                Select(cmbCustemerNegdi, o => keep[1] != null && ((Customers)o).IDcustemer == ((Customers)keep[1]).IDcustemer);
                Select(cmbOrechDinNegdi, o => keep[2] != null && ((Customers)o).IDcustemer == ((Customers)keep[2]).IDcustemer);
                Select(target, o => ((Customers)o).IDcustemer == id);
            }
        }

        private void btnAddCustemer_Click(object sender, EventArgs e) { AddPerson(PersonTypes.Client, cmbCustomer); }

        private void btnAddCustemerNegdi_Click(object sender, EventArgs e) { AddPerson(PersonTypes.OpposingClient, cmbCustemerNegdi); }

        private void btnAddLawyer_Click(object sender, EventArgs e) { AddPerson(PersonTypes.OpposingLawyer, cmbOrechDinNegdi); }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (db != null) db.Dispose();
            base.OnFormClosed(e);
        }
    }
}
