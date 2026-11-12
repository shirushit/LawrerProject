using System;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Links a case type to a court ("this court handles civil claims").</summary>
    public partial class frmAddSubjektShebetMishpatMetapel : Form
    {
        private readonly LawyerDBEntities1 db = new LawyerDBEntities1();
        private readonly int courtId;

        public frmAddSubjektShebetMishpatMetapel(int courtId)
        {
            InitializeComponent();
            this.courtId = courtId;

            Bet_Mishpat court = db.Bet_Mishpat.Find(courtId);
            lblIdBetMishpat.Text = court != null ? court.NameBetMishpat : courtId.ToString();
            lblIdSubjektLebetMishpat.Text = "(חדש)";

            // Only case types that are not linked to this court yet.
            var linked = db.Subjekts_Shebet_Mishpat_Metapel.Where(s => s.IDBetMishpat == courtId).Select(s => s.IDSubjekt);
            cmbNameSubjectBetMishpat.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNameSubjectBetMishpat.DataSource = db.Subjekt_Tik
                .Where(s => !linked.Contains(s.IDSugTik))
                .OrderBy(s => s.NameSugTik)
                .ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var subject = cmbNameSubjectBetMishpat.SelectedItem as Subjekt_Tik;
            if (subject == null)
            {
                Ui.Error("יש לבחור נושא תיק.");
                return;
            }
            try
            {
                db.Subjekts_Shebet_Mishpat_Metapel.Add(new Subjekts_Shebet_Mishpat_Metapel
                {
                    IDBetMishpat = courtId,
                    IDSubjekt = subject.IDSugTik
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
