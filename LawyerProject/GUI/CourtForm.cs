using System;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Add or edit a court and the case types it handles.</summary>
    public partial class CourtForm : Form
    {
        private readonly QuedinDbContext db = new QuedinDbContext();
        private Court court;
        private bool isNew;

        public CourtForm()
        {
            InitializeComponent();
            isNew = true;
            court = new Court();
            lblCode.Text = "(חדש)";
            UpdateState();
        }

        public CourtForm(int id)
        {
            InitializeComponent();
            court = db.Courts.FirstOrDefault(b => b.CourtId == id);
            if (court == null) throw new ArgumentException("Court not found: " + id);
            lblCode.Text = court.CourtId.ToString();
            txtCourtName.Text = court.Name;
            FillSubjects();
            UpdateState();
        }

        private void UpdateState()
        {
            Text = isNew ? "הוספת בית משפט" : "עדכון בית משפט";
            // Subjects can be linked only after the court exists.
            btnAddCaseType.Enabled = btnRemove.Enabled = !isNew;
            btnSaveDetails.Visible = false;
        }

        private void FillSubjects()
        {
            listView2.Items.Clear();
            foreach (CourtCaseType link in db.CourtCaseTypes
                         .Where(s => s.CourtId == court.CourtId)
                         .OrderBy(s => s.CaseType.Name)
                         .ToList())
            {
                var item = new ListViewItem(new[] { link.CourtCaseTypeId.ToString(), link.CaseType.Name });
                item.Tag = link.CourtCaseTypeId;
                listView2.Items.Add(item);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            string name = txtCourtName.Text.Trim();
            if (name.Length == 0)
            {
                errorProvider1.SetError(txtCourtName, "יש להזין שם בית משפט");
                return;
            }
            if (db.Courts.Any(b => b.Name == name && b.CourtId != court.CourtId))
            {
                errorProvider1.SetError(txtCourtName, "בית משפט בשם זה כבר קיים");
                return;
            }

            try
            {
                court.Name = name;
                if (isNew) db.Courts.Add(court);
                db.SaveChanges();
                Ui.Notify(this, isNew ? "בית המשפט נשמר. כעת אפשר להוסיף את נושאי התיקים שהוא מטפל בהם." : "בית המשפט עודכן.");
                isNew = false;
                lblCode.Text = court.CourtId.ToString();
                UpdateState();
            }
            catch (Exception ex)
            {
                Ui.Error("שגיאה! לא נשמר.", ex);
            }
        }

        private void txtCourtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNameKey(e.KeyChar);
        }

        private void btnAddCaseType_Click(object sender, EventArgs e)
        {
            using (var form = new CourtCaseTypeForm(court.CourtId))
                form.ShowDialog();
            FillSubjects();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count == 0)
            {
                Ui.Error("יש לבחור נושא מהרשימה.");
                return;
            }
            int id = (int)listView2.SelectedItems[0].Tag;
            if (!Ui.Confirm("להסיר את הנושא מרשימת הנושאים של בית המשפט?")) return;

            try
            {
                CourtCaseType link = db.CourtCaseTypes.Find(id);
                if (link != null)
                {
                    db.CourtCaseTypes.Remove(link);
                    db.SaveChanges();
                }
                FillSubjects();
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
