using System;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Adds a fixed step (process + order) to a case type.</summary>
    public partial class FrmAddTaalichKavuaBatik : Form
    {
        private LawyerDBEntities1 db = new LawyerDBEntities1();
        private readonly Taalichim_Kvuim_Batik step;
        private readonly bool isNew;

        /// <summary>New fixed step for the given case type.</summary>
        public FrmAddTaalichKavuaBatik(int subjectId)
        {
            InitializeComponent();
            isNew = true;
            step = new Taalichim_Kvuim_Batik { IDSubjektTik = subjectId };
            int last = db.Taalichim_Kvuim_Batik.Where(k => k.IDSubjektTik == subjectId).Select(k => (int?)k.NumLevel).Max() ?? 0;
            numericUpDownNumLevel.Value = Math.Min(numericUpDownNumLevel.Maximum, last + 1);
            Init();
        }

        private FrmAddTaalichKavuaBatik(Taalichim_Kvuim_Batik existing, LawyerDBEntities1 context)
        {
            InitializeComponent();
            db.Dispose();
            db = context;
            step = existing;
            Init();
            cmbTaalich.SelectedItem = cmbTaalich.Items.Cast<Taalichim_Letikim>().FirstOrDefault(t => t.IDTaalich == step.IDTaalich);
            numericUpDownNumLevel.Value = Math.Max(numericUpDownNumLevel.Minimum, Math.Min(numericUpDownNumLevel.Maximum, step.NumLevel));
        }

        /// <summary>Edit an existing fixed step.</summary>
        public static FrmAddTaalichKavuaBatik ForEdit(int id)
        {
            var context = new LawyerDBEntities1();
            Taalichim_Kvuim_Batik existing = context.Taalichim_Kvuim_Batik.Find(id);
            if (existing == null)
            {
                context.Dispose();
                throw new ArgumentException("Fixed step not found: " + id);
            }
            return new FrmAddTaalichKavuaBatik(existing, context);
        }

        private void Init()
        {
            Subjekt_Tik subject = db.Subjekt_Tik.Find(step.IDSubjektTik);
            lblIdSubjektTik.Text = subject != null ? subject.NameSugTik : step.IDSubjektTik.ToString();
            cmbTaalich.DataSource = db.Taalichim_Letikim.OrderBy(t => t.NameTaalich).ToList();
            if (isNew) cmbTaalich.SelectedIndex = -1;
            Text = isNew ? "הוספת תהליך קבוע" : "עדכון תהליך קבוע";
        }

        private void blnAdd_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            var process = cmbTaalich.SelectedItem as Taalichim_Letikim;
            if (process == null)
            {
                errorProvider1.SetError(cmbTaalich, "יש לבחור תהליך");
                return;
            }
            int level = (int)numericUpDownNumLevel.Value;
            if (db.Taalichim_Kvuim_Batik.Any(k => k.IDSubjektTik == step.IDSubjektTik && k.NumLevel == level && k.IDTaalichKavua != step.IDTaalichKavua))
            {
                errorProvider1.SetError(numericUpDownNumLevel, "כבר קיים תהליך בשלב זה");
                return;
            }

            try
            {
                step.IDTaalich = process.IDTaalich;
                step.NumLevel = level;
                if (isNew) db.Taalichim_Kvuim_Batik.Add(step);
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
