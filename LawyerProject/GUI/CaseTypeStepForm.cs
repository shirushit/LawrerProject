using System;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Adds a fixed step (process + order) to a case type.</summary>
    public partial class CaseTypeStepForm : Form
    {
        private QuedinDbContext db = new QuedinDbContext();
        private readonly CaseTypeStep step;
        private readonly bool isNew;

        /// <summary>New fixed step for the given case type.</summary>
        public CaseTypeStepForm(int subjectId)
        {
            InitializeComponent();
            isNew = true;
            step = new CaseTypeStep { CaseTypeId = subjectId };
            int last = db.CaseTypeSteps.Where(k => k.CaseTypeId == subjectId).Select(k => (int?)k.StepOrder).Max() ?? 0;
            nudStepOrder.Value = Math.Min(nudStepOrder.Maximum, last + 1);
            Init();
        }

        private CaseTypeStepForm(CaseTypeStep existing, QuedinDbContext context)
        {
            InitializeComponent();
            db.Dispose();
            db = context;
            step = existing;
            Init();
            cmbProcedure.SelectedItem = cmbProcedure.Items.Cast<Procedure>().FirstOrDefault(t => t.ProcedureId == step.ProcedureId);
            nudStepOrder.Value = Math.Max(nudStepOrder.Minimum, Math.Min(nudStepOrder.Maximum, step.StepOrder));
        }

        /// <summary>Edit an existing fixed step.</summary>
        public static CaseTypeStepForm ForEdit(int id)
        {
            var context = new QuedinDbContext();
            CaseTypeStep existing = context.CaseTypeSteps.Find(id);
            if (existing == null)
            {
                context.Dispose();
                throw new ArgumentException("Fixed step not found: " + id);
            }
            return new CaseTypeStepForm(existing, context);
        }

        private void Init()
        {
            CaseType subject = db.CaseTypes.Find(step.CaseTypeId);
            lblCaseTypeId.Text = subject != null ? subject.Name : step.CaseTypeId.ToString();
            cmbProcedure.DataSource = db.Procedures.OrderBy(t => t.Name).ToList();
            if (isNew) cmbProcedure.SelectedIndex = -1;
            Text = isNew ? "הוספת תהליך קבוע" : "עדכון תהליך קבוע";
        }

        private void btnAddStep_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            var process = cmbProcedure.SelectedItem as Procedure;
            if (process == null)
            {
                errorProvider1.SetError(cmbProcedure, "יש לבחור תהליך");
                return;
            }
            int level = (int)nudStepOrder.Value;
            if (db.CaseTypeSteps.Any(k => k.CaseTypeId == step.CaseTypeId && k.StepOrder == level && k.CaseTypeStepId != step.CaseTypeStepId))
            {
                errorProvider1.SetError(nudStepOrder, "כבר קיים תהליך בשלב זה");
                return;
            }

            try
            {
                step.ProcedureId = process.ProcedureId;
                step.StepOrder = level;
                if (isNew) db.CaseTypeSteps.Add(step);
                db.SaveChanges();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Ui.Error("שגיאה! לא נשמר.", ex);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db.Dispose();
            base.OnFormClosed(e);
        }
    }
}
