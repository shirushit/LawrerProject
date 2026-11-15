using System;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Add or edit a procedure in the catalogue (name, type, estimated duration in days).</summary>
    public partial class ProcedureForm : Form
    {
        private readonly QuedinDbContext db = new QuedinDbContext();
        private readonly Procedure procedure;
        private readonly bool isNew;

        public ProcedureForm()
        {
            InitializeComponent();
            isNew = true;
            procedure = new Procedure();
            lblCode.Text = "(חדש)";
            Prepare();
        }

        public ProcedureForm(int id)
        {
            InitializeComponent();
            procedure = db.Procedures.FirstOrDefault(t => t.ProcedureId == id);
            if (procedure == null) throw new ArgumentException("Process not found: " + id);
            Prepare();
            lblCode.Text = procedure.ProcedureId.ToString();
            txtProcedureName.Text = procedure.Name;
            txtCategory.Text = procedure.Category;
            nudDurationDays.Value = Math.Max(nudDurationDays.Minimum,
                Math.Min(nudDurationDays.Maximum, procedure.DurationDays));
        }

        private void Prepare()
        {
            nudDurationDays.Minimum = 1;
            nudDurationDays.Maximum = 3650;
            Text = isNew ? "הוספת תהליך" : "עדכון תהליך";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            string name = txtProcedureName.Text.Trim();
            if (name.Length == 0)
            {
                errorProvider1.SetError(txtProcedureName, "יש להזין שם תהליך");
                return;
            }
            if (txtCategory.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtCategory, "יש להזין סוג תהליך (לדוגמה: הגשה, דיון, חתימה)");
                return;
            }
            if (db.Procedures.Any(t => t.Name == name && t.ProcedureId != procedure.ProcedureId))
            {
                errorProvider1.SetError(txtProcedureName, "תהליך בשם זה כבר קיים");
                return;
            }

            try
            {
                procedure.Name = name;
                procedure.Category = txtCategory.Text.Trim();
                procedure.DurationDays = (int)nudDurationDays.Value;
                if (isNew) db.Procedures.Add(procedure);
                CatalogService.Save(db, "תהליך", (isNew ? "נוסף: " : "עודכן: ") + name);
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
