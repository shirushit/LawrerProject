using System;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Links a case type to a court ("this court handles civil claims").</summary>
    public partial class CourtCaseTypeForm : Form
    {
        private readonly QuedinDbContext db = new QuedinDbContext();
        private readonly int courtId;

        public CourtCaseTypeForm(int courtId)
        {
            InitializeComponent();
            this.courtId = courtId;

            Court court = db.Courts.Find(courtId);
            lblCourtId.Text = court != null ? court.Name : courtId.ToString();
            lblCourtCaseTypeId.Text = "(חדש)";

            // Only case types that are not linked to this court yet.
            var linked = db.CourtCaseTypes.Where(s => s.CourtId == courtId).Select(s => s.CaseTypeId);
            cmbCaseType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCaseType.DataSource = db.CaseTypes
                .Where(s => !linked.Contains(s.CaseTypeId))
                .OrderBy(s => s.Name)
                .ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var subject = cmbCaseType.SelectedItem as CaseType;
            if (subject == null)
            {
                Ui.Error("יש לבחור נושא תיק.");
                return;
            }
            try
            {
                db.CourtCaseTypes.Add(new CourtCaseType
                {
                    CourtId = courtId,
                    CaseTypeId = subject.CaseTypeId
                });
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
