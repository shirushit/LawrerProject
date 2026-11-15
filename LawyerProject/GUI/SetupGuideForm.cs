using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>
    /// Step-by-step setup for a new office (managers only). Each step shows whether it is done
    /// and opens the screen where it is done, in the order the data depends on each other.
    /// </summary>
    public class SetupGuideForm : Form
    {
        private class Step
        {
            public string Title;
            public string Help;
            public Func<QuedinDbContext, bool> IsDone;
            public Func<Form> Open;
            public Label Mark;
            public Label Text;
            public Button Button;
        }

        private Step[] steps;
        private Label lblIntro;
        private Button btnClose;

        public SetupGuideForm()
        {
            InitializeComponent();
            RefreshStatus();
            Activated += (s, e) => RefreshStatus();
        }

        private void InitializeComponent()
        {
            steps = new[]
            {
                new Step { Title = "1. בתי משפט", Help = "הוספת בתי המשפט שהמשרד עובד מולם.",
                    IsDone = db => db.Courts.Any(), Open = () => new CourtsListForm() },
                new Step { Title = "2. תהליכים", Help = "רשימת כל השלבים האפשריים (הגשת תביעה, דיון מקדמי...) ומשך כל אחד בימים.",
                    IsDone = db => db.Procedures.Any(), Open = () => new ProceduresListForm() },
                new Step { Title = "3. נושאי תיקים", Help = "סוגי התיקים (תביעה כספית, מקרקעין...), שכר טרחה ואגרה.",
                    IsDone = db => db.CaseTypes.Any(), Open = () => new CaseTypesListForm() },
                new Step { Title = "4. תהליכים קבועים לכל נושא", Help = "לכל נושא - סדר השלבים שנוצרים אוטומטית בפתיחת תיק. בלי זה לא ניתן לפתוח תיק.",
                    IsDone = db => db.CaseTypes.Any() && db.CaseTypes.All(t => t.Steps.Any()), Open = () => new CaseTypesListForm() },
                new Step { Title = "5. קישור נושאים לבתי משפט", Help = "איזה בית משפט דן באיזה נושא, כדי שבפתיחת תיק יוצגו רק בתי המשפט המתאימים.",
                    IsDone = db => db.CourtCaseTypes.Any(), Open = () => new CourtsListForm() },
                new Step { Title = "6. משתמשים", Help = "משתמש לכל עובד/ת. הסיסמה הראשונית נמסרת לעובד, והוא מחליף אותה במסך הכניסה.",
                    IsDone = db => db.Users.Count() > 1, Open = () => new UsersForm() },
                new Step { Title = "7. פרטי המשרד ומע\"מ", Help = "שם, כתובת, טלפון ומספר עוסק מורשה לקבלות, ושיעור המע\"מ. משותף לכל עמדות המשרד.",
                    IsDone = db => db.Settings.Any(x => x.SettingKey == AppSettings.OfficeTaxIdKey && x.Value != null && x.Value != ""),
                    Open = () => new SettingsForm() },
                new Step { Title = "8. גיבוי ראשון", Help = "כפתור \"גיבוי נתונים\" בדף הבית. מומלץ לגבות בסוף כל יום עבודה.",
                    IsDone = db => db.AuditLog.Any(entry => entry.Action == AuditActions.Backup), Open = null },
            };

            SuspendLayout();
            Text = "מדריך הקמת המערכת";
            Font = new Font("Calibri", 11F);
            RightToLeft = RightToLeft.Yes;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            int width = 920;

            lblIntro = new Label
            {
                Text = "כדי להתחיל לעבוד יש להגדיר את הנתונים הבסיסיים לפי הסדר. שלב שהושלם מסומן ב-✓.",
                Location = new Point(20, 15), Size = new Size(width - 40, 40), TextAlign = ContentAlignment.MiddleRight
            };
            Controls.Add(lblIntro);

            int y = 65;
            foreach (Step step in steps)
            {
                step.Mark = new Label { Location = new Point(width - 55, y + 4), Size = new Size(35, 35), Font = new Font("Calibri", 18F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };
                step.Text = new Label { Location = new Point(150, y), Size = new Size(width - 215, 50), TextAlign = ContentAlignment.MiddleRight };
                step.Mark.Text = "○";
                step.Text.Text = step.Title + Environment.NewLine + step.Help;
                Controls.Add(step.Mark);
                Controls.Add(step.Text);
                if (step.Open != null)
                {
                    Step s = step;
                    step.Button = new Button { Text = "פתיחה", Location = new Point(20, y + 8), Size = new Size(110, 34), UseVisualStyleBackColor = true };
                    step.Button.Click += (o, e) => OpenStep(s);
                    Controls.Add(step.Button);
                }
                y += 58;
            }

            btnClose = new Button { Text = "סגירה", Location = new Point(20, y + 10), Size = new Size(110, 36), DialogResult = DialogResult.Cancel, UseVisualStyleBackColor = true };
            btnClose.Click += (o, e) => Close();
            Controls.Add(btnClose);
            CancelButton = btnClose;
            ClientSize = new Size(width, y + 60);
            ResumeLayout(false);
        }

        private void OpenStep(Step step)
        {
            try
            {
                using (Form form = step.Open())
                    form.ShowDialog(this);
            }
            catch (Exception ex)
            {
                Ui.Error("לא ניתן לפתוח את המסך.", ex);
            }
            RefreshStatus();
        }

        private void RefreshStatus()
        {
            try
            {
                using (var db = new QuedinDbContext())
                {
                    foreach (Step step in steps)
                    {
                        bool done = step.IsDone(db);
                        step.Mark.Text = done ? "✓" : "○";
                        step.Mark.ForeColor = done ? Color.SeaGreen : SystemColors.GrayText;
                        step.Text.Text = step.Title + (done ? "" : "  - טרם בוצע") + Environment.NewLine + step.Help;
                        step.Text.Font = new Font(Font, done ? FontStyle.Regular : FontStyle.Bold);
                    }
                }
            }
            catch (Exception ex)
            {
                lblIntro.Text = "לא ניתן לבדוק את מצב ההגדרות: " + Ui.FriendlyReason(ex);
            }
        }
    }
}
