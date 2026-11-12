using System;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Add or edit a process in the catalogue (name, type, estimated duration in days).</summary>
    public partial class FrmAddTaalichimLetik : Form
    {
        private readonly LawyerDBEntities1 db = new LawyerDBEntities1();
        private readonly Taalichim_Letikim process;
        private readonly bool isNew;

        public FrmAddTaalichimLetik()
        {
            InitializeComponent();
            isNew = true;
            process = new Taalichim_Letikim();
            lblCode.Text = "(חדש)";
            Prepare();
        }

        public FrmAddTaalichimLetik(int id)
        {
            InitializeComponent();
            process = db.Taalichim_Letikim.FirstOrDefault(t => t.IDTaalich == id);
            if (process == null) throw new ArgumentException("Process not found: " + id);
            Prepare();
            lblCode.Text = process.IDTaalich.ToString();
            txtNameTaalich.Text = process.NameTaalich;
            txtSugTaalich.Text = process.SugTaalich;
            numericUpDown1.Value = Math.Max(numericUpDown1.Minimum,
                Math.Min(numericUpDown1.Maximum, CaseService.ParseDays(process.MeshechZmanLetaalich)));
        }

        private void Prepare()
        {
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 3650;
            Text = isNew ? "הוספת תהליך" : "עדכון תהליך";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            string name = txtNameTaalich.Text.Trim();
            if (name.Length == 0)
            {
                errorProvider1.SetError(txtNameTaalich, "יש להזין שם תהליך");
                return;
            }
            if (db.Taalichim_Letikim.Any(t => t.NameTaalich == name && t.IDTaalich != process.IDTaalich))
            {
                errorProvider1.SetError(txtNameTaalich, "תהליך בשם זה כבר קיים");
                return;
            }

            try
            {
                process.NameTaalich = name;
                process.SugTaalich = txtSugTaalich.Text.Trim().Length == 0 ? null : txtSugTaalich.Text.Trim();
                process.MeshechZmanLetaalich = ((int)numericUpDown1.Value).ToString();
                if (isNew) db.Taalichim_Letikim.Add(process);
                db.SaveChanges();
                Ui.Info(isNew ? "התהליך נשמר." : "התהליך עודכן. המשך החדש ישמש לתיקים שייפתחו מעכשיו.");
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
