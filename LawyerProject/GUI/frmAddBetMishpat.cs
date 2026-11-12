using System;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Add or edit a court and the case types it handles.</summary>
    public partial class frmAddBetMishpat : Form
    {
        private readonly LawyerDBEntities1 db = new LawyerDBEntities1();
        private Bet_Mishpat court;
        private bool isNew;

        public frmAddBetMishpat()
        {
            InitializeComponent();
            isNew = true;
            court = new Bet_Mishpat();
            lblCode.Text = "(חדש)";
            UpdateState();
        }

        public frmAddBetMishpat(int id)
        {
            InitializeComponent();
            court = db.Bet_Mishpat.FirstOrDefault(b => b.IDBetMishpat == id);
            if (court == null) throw new ArgumentException("Court not found: " + id);
            lblCode.Text = court.IDBetMishpat.ToString();
            txtNameBetMishpat.Text = court.NameBetMishpat;
            FillSubjects();
            UpdateState();
        }

        private void UpdateState()
        {
            Text = isNew ? "הוספת בית משפט" : "עדכון בית משפט";
            // Subjects can be linked only after the court exists.
            btnAddSubjekt.Enabled = BtnDeletSunjekt.Enabled = !isNew;
            BtnUpDateSubjekt.Visible = false;
        }

        private void FillSubjects()
        {
            listView2.Items.Clear();
            foreach (Subjekts_Shebet_Mishpat_Metapel link in db.Subjekts_Shebet_Mishpat_Metapel
                         .Where(s => s.IDBetMishpat == court.IDBetMishpat)
                         .OrderBy(s => s.Subjekt_Tik.NameSugTik)
                         .ToList())
            {
                var item = new ListViewItem(new[] { link.IDTable.ToString(), link.Subjekt_Tik.NameSugTik });
                item.Tag = link.IDTable;
                listView2.Items.Add(item);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            string name = txtNameBetMishpat.Text.Trim();
            if (name.Length == 0)
            {
                errorProvider1.SetError(txtNameBetMishpat, "יש להזין שם בית משפט");
                return;
            }
            if (db.Bet_Mishpat.Any(b => b.NameBetMishpat == name && b.IDBetMishpat != court.IDBetMishpat))
            {
                errorProvider1.SetError(txtNameBetMishpat, "בית משפט בשם זה כבר קיים");
                return;
            }

            try
            {
                court.NameBetMishpat = name;
                if (isNew) db.Bet_Mishpat.Add(court);
                db.SaveChanges();
                Ui.Info(isNew ? "בית המשפט נשמר. כעת אפשר להוסיף את נושאי התיקים שהוא מטפל בהם." : "עודכן בהצלחה.");
                isNew = false;
                lblCode.Text = court.IDBetMishpat.ToString();
                UpdateState();
            }
            catch (Exception ex)
            {
                Ui.Error("שגיאה! לא נשמר.", ex);
            }
        }

        private void txtNameBetMishpat_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNameKey(e.KeyChar);
        }

        private void btnAddSubjekt_Click(object sender, EventArgs e)
        {
            using (var form = new frmAddSubjektShebetMishpatMetapel(court.IDBetMishpat))
                form.ShowDialog();
            FillSubjects();
        }

        private void BtnDeletSunjekt_Click(object sender, EventArgs e)
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
                Subjekts_Shebet_Mishpat_Metapel link = db.Subjekts_Shebet_Mishpat_Metapel.Find(id);
                if (link != null)
                {
                    db.Subjekts_Shebet_Mishpat_Metapel.Remove(link);
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
