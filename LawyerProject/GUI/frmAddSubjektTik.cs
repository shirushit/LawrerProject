using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>
    /// Add or edit a case type: its fees and the ordered list of fixed steps
    /// that every new case of this type receives.
    /// </summary>
    public partial class frmAddSubjektTik : Form
    {
        private readonly LawyerDBEntities1 db = new LawyerDBEntities1();
        private Subjekt_Tik subject;
        private bool isNew;

        public frmAddSubjektTik()
        {
            InitializeComponent();
            isNew = true;
            subject = new Subjekt_Tik();
            lblCode.Text = "(חדש)";
            UpdateState();
        }

        public frmAddSubjektTik(int id)
        {
            InitializeComponent();
            subject = db.Subjekt_Tik.FirstOrDefault(s => s.IDSugTik == id);
            if (subject == null) throw new ArgumentException("Case type not found: " + id);
            lblCode.Text = subject.IDSugTik.ToString();
            txtNameSugTik.Text = subject.NameSugTik;
            txtSharTircha.Text = subject.ScharTircha.HasValue ? subject.ScharTircha.Value.ToString("0.##", CultureInfo.InvariantCulture) : "";
            txtAgra.Text = subject.Agra.HasValue ? subject.Agra.Value.ToString("0.##", CultureInfo.InvariantCulture) : "";
            FillSteps();
            UpdateState();
        }

        private void UpdateState()
        {
            Text = isNew ? "הוספת נושא תיק" : "עדכון נושא תיק";
            button1.Enabled = BtnUpDateSubjekt.Enabled = BtnDeletSunjekt.Enabled = !isNew;
        }

        private void FillSteps()
        {
            listView1.Items.Clear();
            if (listView1.Columns.Count > 0) listView1.Columns[0].Width = 300;
            foreach (Taalichim_Kvuim_Batik step in db.Taalichim_Kvuim_Batik
                         .Where(k => k.IDSubjektTik == subject.IDSugTik)
                         .OrderBy(k => k.NumLevel)
                         .ToList())
            {
                var item = new ListViewItem(step.NumLevel + ". " + step.Taalichim_Letikim.NameTaalich
                                            + " (" + step.Taalichim_Letikim.MeshechZmanLetaalich + " ימים)");
                item.Tag = step.IDTaalichKavua;
                listView1.Items.Add(item);
            }
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool ok = true;
            string name = txtNameSugTik.Text.Trim();
            decimal fee, courtFee;

            if (name.Length == 0) { errorProvider1.SetError(txtNameSugTik, "יש להזין שם נושא"); ok = false; }
            else if (db.Subjekt_Tik.Any(s => s.NameSugTik == name && s.IDSugTik != subject.IDSugTik))
            { errorProvider1.SetError(txtNameSugTik, "נושא בשם זה כבר קיים"); ok = false; }
            if (!Validation.TryParseMoney(txtSharTircha.Text, out fee)) { errorProvider1.SetError(txtSharTircha, "יש להזין שכר טרחה"); ok = false; }
            if (!Validation.TryParseMoney(txtAgra.Text, out courtFee)) { errorProvider1.SetError(txtAgra, "יש להזין אגרה (0 אם אין)"); ok = false; }
            if (!ok) return;

            try
            {
                subject.NameSugTik = name;
                subject.ScharTircha = fee;
                subject.Agra = courtFee;
                if (isNew) db.Subjekt_Tik.Add(subject);
                db.SaveChanges();
                Ui.Notify(this, isNew ? "הנושא נשמר. כעת הוסיפו את התהליכים הקבועים שלו." : "הנושא עודכן.");
                isNew = false;
                lblCode.Text = subject.IDSugTik.ToString();
                UpdateState();
            }
            catch (Exception ex)
            {
                Ui.Error("שגיאה! לא נשמר.", ex);
            }
        }

        private void Money_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsMoneyKey(e.KeyChar);
        }

        private void txtAgra_KeyPress(object sender, KeyPressEventArgs e) { Money_KeyPress(sender, e); }

        private void txtSharTircha_KeyPress(object sender, KeyPressEventArgs e) { Money_KeyPress(sender, e); }

        private void txtNameSugTik_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNameKey(e.KeyChar);
        }

        private int? SelectedStepId()
        {
            if (listView1.SelectedItems.Count == 0)
            {
                Ui.Error("יש לבחור תהליך מהרשימה.");
                return null;
            }
            return (int)listView1.SelectedItems[0].Tag;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var form = new FrmAddTaalichKavuaBatik(subject.IDSugTik))
                form.ShowDialog();
            FillSteps();
        }

        private void BtnUpDateSubjekt_Click(object sender, EventArgs e)
        {
            int? id = SelectedStepId();
            if (id == null) return;
            using (var form = FrmAddTaalichKavuaBatik.ForEdit(id.Value))
                form.ShowDialog();
            FillSteps();
        }

        private void BtnDeletSunjekt_Click(object sender, EventArgs e)
        {
            int? id = SelectedStepId();
            if (id == null) return;
            if (!Ui.Confirm("להסיר את התהליך מהתהליכים הקבועים של הנושא?\nתיקים קיימים לא ישתנו.")) return;
            try
            {
                Taalichim_Kvuim_Batik step = db.Taalichim_Kvuim_Batik.Find(id.Value);
                if (step != null)
                {
                    db.Taalichim_Kvuim_Batik.Remove(step);
                    db.SaveChanges();
                }
                FillSteps();
            }
            catch (Exception ex)
            {
                Ui.Error("שגיאה במחיקה.", ex);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db.Dispose();
            base.OnFormClosed(e);
        }
    }
}
