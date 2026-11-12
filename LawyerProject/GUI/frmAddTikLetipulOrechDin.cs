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
            cmbCustomer.DataSource = people.Where(c => c.SugPerson == PersonTypes.Client).ToList();
            cmbCustemerNegdi.DataSource = people.Where(c => c.SugPerson == PersonTypes.OpposingClient).ToList();
            cmbOrechDinNegdi.DataSource = people.Where(c => c.SugPerson == PersonTypes.OpposingLawyer).ToList();
            cmbSubjektTik.DataSource = db.Subjekt_Tik.OrderBy(s => s.NameSugTik).ToList();
            cmbCustomer.SelectedIndex = cmbCustemerNegdi.SelectedIndex = cmbOrechDinNegdi.SelectedIndex = -1;
            if (isNew) cmbSubjektTik.SelectedIndex = -1;
            loading = false;
            cmbSubjektTik.SelectedIndexChanged -= cmbSubjektTik_SelectedIndexChanged;
            cmbSubjektTik.SelectedIndexChanged += cmbSubjektTik_SelectedIndexChanged;
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
            txtIPuy.Text = tik.IpuiKohach;

            string money = tik.SchumLetvia.HasValue ? tik.SchumLetvia.Value.ToString("0.##", CultureInfo.InvariantCulture) : "";
            txtSchumKetvia.Text = tik.IsCourtCase ? money : "";
            txtPropertyValue.Text = tik.IsCourtCase ? "" : money;
            txtSchumShenitba.Text = tik.SchumShenitba.HasValue ? tik.SchumShenitba.Value.ToString("0.##", CultureInfo.InvariantCulture) : "";
            txtAdressNeches.Text = tik.AdressNeches;
            txtGush.Text = tik.Gush;
            txtChelka.Text = tik.Chelka;
            txtTatChelka.Text = tik.TatChelka;

            txtAchuzRevachim.Text = tik.AchuzRevachim.HasValue ? tik.AchuzRevachim.Value.ToString(CultureInfo.InvariantCulture) : "";
            txtMikdama.Text = tik.Mikdama.ToString("0.##", CultureInfo.InvariantCulture);
            checkBoxShulamMikdama.Checked = tik.ShulamMikdama == true;
            checkBoxShulam.Checked = tik.Shulam;

            lblStatusTik.Text = CaseService.IsFinished(tik)
                ? "הסתיים"
                : (tik.Taalichim_Letikim != null ? tik.Taalichim_Letikim.NameTaalich : "-");
            UpdateMoney();
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

                var item = new ListViewItem(new[]
                {
                    number.ToString(),
                    step.Taalichim_Letikim != null ? step.Taalichim_Letikim.NameTaalich : step.IDTahalich.ToString(),
                    Ui.FormatDate(step.TaharichMeshoar),
                    done ? Ui.FormatDate(step.TaharichBizhua) : "",
                    status
                });
                item.Tag = step.IDTahalichTipulLetik;
                if (done) item.ForeColor = SystemColors.GrayText;
                else if (late) item.BackColor = Color.MistyRose;
                if (step == next) item.Font = new Font(listView1.Font, FontStyle.Bold);
                listView1.Items.Add(item);
                number++;
            }
            btnCompleteStep.Enabled = next != null;
        }

        private void FillPayments()
        {
            listView2.Items.Clear();
            foreach (Tashlumim_Letik p in tik.Tashlumim_Letik.OrderBy(p => p.IDTashlum))
            {
                var item = new ListViewItem(new[]
                {
                    p.IDTashlum.ToString(),
                    Ui.FormatDate(p.TaarichTashlum),
                    p.EmzhaeyTashlum,
                    Ui.FormatMoney(p.Schum),
                    Ui.FormatMoney(p.Itra),
                    p.Teur
                });
                item.Tag = p.IDTashlum;
                listView2.Items.Add(item);
            }
            btnReceipt.Enabled = listView2.Items.Count > 0;
        }

        // ------------------------------------------------------------------ UI state

        private void UpdateMode()
        {
            bool court = rbCourt.Checked;
            groupBoxBetMishpat.Visible = court;
            groupBoxIska.Visible = !court;

            // The fixed steps come from the case type, so it is chosen once, when the case is opened.
            cmbSubjektTik.Enabled = isNew;
            tabControl1.Enabled = !isNew;
            lblSaveHint.Visible = isNew;
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
            lblTotal.Text = Ui.FormatMoney(total);
            decimal balance = !isNew && tik.Tashlumim_Letik.Count > 0 ? CaseService.OpenBalance(tik) : total;
            lblBalance.Text = Ui.FormatMoney(balance);
        }

        private void CaseType_CheckedChanged(object sender, EventArgs e)
        {
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
            if (opposingClient == null) fail(cmbCustemerNegdi, "יש לבחור לקוח נגדי");
            if (opposingLawyer == null) fail(cmbOrechDinNegdi, "יש לבחור עורך דין נגדי");
            if (string.IsNullOrWhiteSpace(txtIPuy.Text)) fail(txtIPuy, "יש להזין ייפוי כוח");

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
            tik.IpuiKohach = txtIPuy.Text.Trim();
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
            tik.ShulamMikdama = checkBoxShulamMikdama.Checked;
            tik.Shulam = checkBoxShulam.Checked;
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
                db.SaveChanges();
                Ui.Info(isNew ? "התיק נפתח. מספר תיק: " + tik.IDTik : "התיק עודכן בהצלחה.");
                LoadCase(tik.IDTik);
            }
            catch (Exception ex)
            {
                Ui.Error("שגיאה! התיק לא נשמר.", ex);
            }
        }

        // ------------------------------------------------------------------ steps & payments

        private void btnCompleteStep_Click(object sender, EventArgs e)
        {
            Tahalich_Tipul_Betik next = CaseService.OrderedSteps(tik).FirstOrDefault(s => s.Buzha != true);
            if (next == null) return;

            string name = next.Taalichim_Letikim != null ? next.Taalichim_Letikim.NameTaalich : "";
            if (!Ui.Confirm("לסמן את השלב \"" + name + "\" כבוצע היום?\nהמועדים של כל השלבים הבאים יעודכנו בהתאם.")) return;

            try
            {
                string error = new CaseService(db).CompleteStep(tik, next.IDTahalichTipulLetik, DateTime.Today);
                if (error != null)
                {
                    Ui.Error(error);
                    return;
                }
                LoadCase(tik.IDTik);
                tabControl1.SelectedTab = tabSteps;
            }
            catch (Exception ex)
            {
                Ui.Error("שגיאה בעדכון השלב.", ex);
            }
        }

        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            using (var form = new frmAddTashlumimLetik(tik.IDTik))
                form.ShowDialog();
            LoadCase(tik.IDTik);
            tabControl1.SelectedTab = tabPayments;
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
            {
                Ui.Error("יש לבחור תשלום מהרשימה.");
                return;
            }
            using (var form = new frmKabala((int)listView2.SelectedItems[0].Tag))
                form.ShowDialog();
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
