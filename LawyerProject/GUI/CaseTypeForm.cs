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
    public partial class CaseTypeForm : Form
    {
        private readonly QuedinDbContext db = new QuedinDbContext();
        private CaseType subject;
        private bool isNew;

        public CaseTypeForm()
        {
            InitializeComponent();
            isNew = true;
            subject = new CaseType();
            lblCode.Text = "(חדש)";
            UpdateState();
        }

        public CaseTypeForm(int id)
        {
            InitializeComponent();
            subject = db.CaseTypes.FirstOrDefault(s => s.CaseTypeId == id);
            if (subject == null) throw new ArgumentException("Case type not found: " + id);
            lblCode.Text = subject.CaseTypeId.ToString();
            txtCaseTypeName.Text = subject.Name;
            txtBaseFee.Text = subject.BaseFee.HasValue ? subject.BaseFee.Value.ToString("0.##", CultureInfo.InvariantCulture) : "";
            txtCourtFee.Text = subject.CourtFee.HasValue ? subject.CourtFee.Value.ToString("0.##", CultureInfo.InvariantCulture) : "";
            FillSteps();
            UpdateState();
        }

        private void UpdateState()
        {
            Text = isNew ? "הוספת נושא תיק" : "עדכון נושא תיק";
            button1.Enabled = btnSaveDetails.Enabled = btnRemove.Enabled = !isNew;
        }

        private void FillSteps()
        {
            listView1.Items.Clear();
            if (listView1.Columns.Count > 0) listView1.Columns[0].Width = 300;
            foreach (CaseTypeStep step in db.CaseTypeSteps
                         .Where(k => k.CaseTypeId == subject.CaseTypeId)
                         .OrderBy(k => k.StepOrder)
                         .ToList())
            {
                var item = new ListViewItem(step.StepOrder + ". " + step.Procedure.Name
                                            + " (" + step.Procedure.DurationDays + " ימים)");
                item.Tag = step.CaseTypeStepId;
                listView1.Items.Add(item);
            }
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool ok = true;
            string name = txtCaseTypeName.Text.Trim();
            decimal fee, courtFee;

            if (name.Length == 0) { errorProvider1.SetError(txtCaseTypeName, "יש להזין שם נושא"); ok = false; }
            else if (db.CaseTypes.Any(s => s.Name == name && s.CaseTypeId != subject.CaseTypeId))
            { errorProvider1.SetError(txtCaseTypeName, "נושא בשם זה כבר קיים"); ok = false; }
            if (!Validation.TryParseMoney(txtBaseFee.Text, out fee)) { errorProvider1.SetError(txtBaseFee, "יש להזין שכר טרחה"); ok = false; }
            if (!Validation.TryParseMoney(txtCourtFee.Text, out courtFee)) { errorProvider1.SetError(txtCourtFee, "יש להזין אגרה (0 אם אין)"); ok = false; }
            if (!ok) return;

            try
            {
                subject.Name = name;
                subject.BaseFee = fee;
                subject.CourtFee = courtFee;
                if (isNew) db.CaseTypes.Add(subject);
                db.SaveChanges();
                Ui.Notify(this, isNew ? "הנושא נשמר. כעת הוסיפו את התהליכים הקבועים שלו." : "הנושא עודכן.");
                isNew = false;
                lblCode.Text = subject.CaseTypeId.ToString();
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

        private void txtCourtFee_KeyPress(object sender, KeyPressEventArgs e) { Money_KeyPress(sender, e); }

        private void txtBaseFee_KeyPress(object sender, KeyPressEventArgs e) { Money_KeyPress(sender, e); }

        private void txtCaseTypeName_KeyPress(object sender, KeyPressEventArgs e)
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
            using (var form = new CaseTypeStepForm(subject.CaseTypeId))
                form.ShowDialog();
            FillSteps();
        }

        private void btnSaveDetails_Click(object sender, EventArgs e)
        {
            int? id = SelectedStepId();
            if (id == null) return;
            using (var form = CaseTypeStepForm.ForEdit(id.Value))
                form.ShowDialog();
            FillSteps();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int? id = SelectedStepId();
            if (id == null) return;
            if (!Ui.Confirm("להסיר את התהליך מהתהליכים הקבועים של הנושא?\nתיקים קיימים לא ישתנו.")) return;
            try
            {
                CaseTypeStep step = db.CaseTypeSteps.Find(id.Value);
                if (step != null)
                {
                    db.CaseTypeSteps.Remove(step);
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
