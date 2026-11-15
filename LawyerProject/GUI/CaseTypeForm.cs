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
        private CaseType caseType;
        private bool isNew;

        public CaseTypeForm()
        {
            InitializeComponent();
            isNew = true;
            caseType = new CaseType();
            lblCode.Text = "(חדש)";
            UpdateState();
        }

        public CaseTypeForm(int id)
        {
            InitializeComponent();
            caseType = db.CaseTypes.FirstOrDefault(s => s.CaseTypeId == id);
            if (caseType == null) throw new ArgumentException("Case type not found: " + id);
            lblCode.Text = caseType.CaseTypeId.ToString();
            txtCaseTypeName.Text = caseType.Name;
            txtBaseFee.Text = caseType.BaseFee.HasValue ? caseType.BaseFee.Value.ToString("0.##", CultureInfo.InvariantCulture) : "";
            txtCourtFee.Text = caseType.CourtFee.HasValue ? caseType.CourtFee.Value.ToString("0.##", CultureInfo.InvariantCulture) : "";
            FillSteps();
            UpdateState();
        }

        private void UpdateState()
        {
            Text = isNew ? "הוספת נושא תיק" : "עדכון נושא תיק";
            btnAddStep.Enabled = btnEditStep.Enabled = btnRemove.Enabled = !isNew;
        }

        private void FillSteps()
        {
            listSteps.Items.Clear();
            if (listSteps.Columns.Count > 0) listSteps.Columns[0].Width = 300;
            foreach (CaseTypeStep step in db.CaseTypeSteps
                         .Where(k => k.CaseTypeId == caseType.CaseTypeId)
                         .OrderBy(k => k.StepOrder)
                         .ToList())
            {
                var item = new ListViewItem(step.StepOrder + ". " + step.Procedure.Name
                                            + " (" + step.Procedure.DurationDays + " ימים)");
                item.Tag = step.CaseTypeStepId;
                listSteps.Items.Add(item);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            bool ok = true;
            string name = txtCaseTypeName.Text.Trim();
            decimal fee, courtFee;

            if (name.Length == 0) { errorProvider1.SetError(txtCaseTypeName, "יש להזין שם נושא"); ok = false; }
            else if (db.CaseTypes.Any(s => s.Name == name && s.CaseTypeId != caseType.CaseTypeId))
            { errorProvider1.SetError(txtCaseTypeName, "נושא בשם זה כבר קיים"); ok = false; }
            // Minimum fee: the case total is never lower than this, whatever the agreed percentage. Empty = no minimum.
            if (txtBaseFee.Text.Trim().Length == 0) fee = 0m;
            else if (!Validation.TryParseMoney(txtBaseFee.Text, out fee)) { errorProvider1.SetError(txtBaseFee, "סכום לא תקין (ריק = ללא מינימום)"); ok = false; }
            if (!Validation.TryParseMoney(txtCourtFee.Text, out courtFee)) { errorProvider1.SetError(txtCourtFee, "יש להזין אגרה (0 אם אין)"); ok = false; }
            if (!ok) return;

            try
            {
                caseType.Name = name;
                caseType.BaseFee = fee;
                caseType.CourtFee = courtFee;
                if (isNew) db.CaseTypes.Add(caseType);
                CatalogService.Save(db, "נושא תיק", (isNew ? "נוסף: " : "עודכן: ") + name);
                Ui.Notify(this, isNew ? "הנושא נשמר. כעת הוסיפו את התהליכים הקבועים שלו." : "הנושא עודכן.");
                isNew = false;
                lblCode.Text = caseType.CaseTypeId.ToString();
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
            if (listSteps.SelectedItems.Count == 0)
            {
                Ui.Error("יש לבחור תהליך מהרשימה.");
                return null;
            }
            return (int)listSteps.SelectedItems[0].Tag;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var form = new CaseTypeStepForm(caseType.CaseTypeId))
                form.ShowDialog();
            FillSteps();
        }

        private void btnEditStep_Click(object sender, EventArgs e)
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
                    CatalogService.Save(db, "נושא תיק", "הוסר תהליך קבוע מנושא " + step.CaseTypeId);
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
