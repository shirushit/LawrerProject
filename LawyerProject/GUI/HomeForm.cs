using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>
    /// Home screen: "what is urgent today" (the next step of every open case that is late or due soon),
    /// a setup banner while the system is still empty, and the menus. Management screens are shown to managers only.
    /// Screens open side by side (not modal), one window per screen.
    /// </summary>
    public partial class HomeForm : Form
    {
        private readonly Dictionary<Type, Form> screens = new Dictionary<Type, Form>();
        private bool confirmedExit;

        public HomeForm()
        {
            InitializeComponent();

            bool manager = Session.IsManager;
            btnCourts.Visible = manager;
            btnCaseTypes.Visible = manager;
            btnProcesses.Visible = manager;
            btnUsers.Visible = manager;
            btnSetupGuide.Visible = manager;
            btnAudit.Visible = manager;
            btnBackup.Visible = manager;

            if (Session.CurrentUser != null)
                lblWelcome.Text = "שלום " + Session.CurrentUser.UserName + " (" + Session.CurrentUser.Role + ")";

            Activated += (s, e) => RefreshToday();
        }

        // ------------------------------------------------------------------ "what is urgent today"

        private class DueRow
        {
            public int CaseId { get; set; }
            public string Client { get; set; }
            public string Step { get; set; }
            public string Due { get; set; }
            public string When { get; set; }
            public string Note { get; set; }
            public int DaysLate { get; set; }
        }

        private DateTime lastRefresh = DateTime.MinValue;

        /// <summary>Reloads the list when the home screen gets focus (at most every few seconds).</summary>
        private void RefreshToday()
        {
            if ((DateTime.Now - lastRefresh).TotalSeconds < 5) return;
            lastRefresh = DateTime.Now;
            try
            {
                using (var db = new QuedinDbContext())
                {
                    bool configured = db.CaseTypes.Any(t => t.Steps.Any());
                    lblSetup.Visible = !configured;
                    if (!configured && !Session.IsManager)
                        lblSetup.Text = "המערכת עדיין לא הוגדרה. פנו למנהלת המשרד כדי להגדיר נושאי תיקים ותהליכים.";

                    List<DueItem> items = new CaseService(db).DueSteps(DateTime.Today, AppSettings.DueSoonDays);
                    gridDue.DataSource = items.Select(i => new DueRow
                    {
                        CaseId = i.CaseId,
                        Client = i.Client,
                        Step = i.Step,
                        Due = Ui.FormatDate(i.Due) + " (" + Ui.DayName(i.Due) + ")",
                        When = i.DaysLate > 0 ? "באיחור " + i.DaysLate + " ימים" : i.DaysLate == 0 ? "היום" : "בעוד " + (-i.DaysLate) + " ימים",
                        Note = i.Note,
                        DaysLate = i.DaysLate
                    }).ToList();
                    Ui.SetHeaders(gridDue, new Dictionary<string, string>
                    {
                        { "CaseId", "תיק" }, { "Client", "לקוח" }, { "Step", "השלב הבא" },
                        { "Due", "מועד" }, { "When", "מצב" }, { "Note", "הערה" }
                    });
                    foreach (DataGridViewRow row in gridDue.Rows)
                    {
                        var r = (DueRow)row.DataBoundItem;
                        row.DefaultCellStyle.BackColor = r.DaysLate > 0 ? Color.MistyRose : r.DaysLate == 0 ? Color.Moccasin : Color.LightYellow;
                    }
                    lblTodayTitle.Text = items.Count == 0
                        ? "מה דחוף היום - אין שלבים באיחור או בימים הקרובים"
                        : "מה דחוף היום (" + items.Count + ")";
                }
            }
            catch (Exception ex)
            {
                lblTodayTitle.Text = "מה דחוף היום";
                lblTodaySub.Text = "לא ניתן לטעון את הרשימה: " + Ui.FriendlyReason(ex);
            }
        }

        private void gridDue_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int? id = Ui.SelectedInt(gridDue, "CaseId");
            if (id != null) CaseWindows.OpenCase(this, id.Value, () => { lastRefresh = DateTime.MinValue; RefreshToday(); });
        }

        // ------------------------------------------------------------------ menus

        /// <summary>Shows a screen next to the home screen; a second click brings the open one to the front.</summary>
        private void Open<T>(Func<T> create) where T : Form
        {
            Form existing;
            if (screens.TryGetValue(typeof(T), out existing) && !existing.IsDisposed)
            {
                if (existing.WindowState == FormWindowState.Minimized) existing.WindowState = FormWindowState.Normal;
                existing.Activate();
                return;
            }
            Form form;
            try
            {
                form = create();
            }
            catch (Exception ex)
            {
                Ui.Error("לא ניתן לפתוח את המסך.", ex);
                return;
            }
            screens[typeof(T)] = form;
            form.FormClosed += (s, e) => { screens.Remove(typeof(T)); lastRefresh = DateTime.MinValue; };
            form.Show(this);
        }

        private void btnAbout_Click(object sender, EventArgs e) { Open(() => new AboutForm()); }

        private void btnCustomers_Click(object sender, EventArgs e) { Open(() => new CustomersListForm()); }

        private void btnCases_Click(object sender, EventArgs e) { Open(() => new CasesListForm()); }

        private void btnDeals_Click(object sender, EventArgs e) { Open(() => new DealsListForm()); }

        private void btnPayments_Click(object sender, EventArgs e) { Open(() => new PaymentsListForm()); }

        private void btnCourts_Click(object sender, EventArgs e) { Open(() => new CourtsListForm()); }

        private void btnCaseTypes_Click(object sender, EventArgs e) { Open(() => new CaseTypesListForm()); }

        private void btnProcesses_Click(object sender, EventArgs e) { Open(() => new ProceduresListForm()); }

        private void btnUsers_Click(object sender, EventArgs e) { Open(() => new UsersForm()); }

        private void btnSetupGuide_Click(object sender, EventArgs e)
        {
            if (!Session.IsManager) return;
            Open(() => new SetupGuideForm());
        }

        private void btnAudit_Click(object sender, EventArgs e) { Open(() => new AuditLogForm()); }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog { Description = "בחרו תיקייה לקובץ הגיבוי (במחשב שבו פועל מסד הנתונים)" })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                Cursor = Cursors.WaitCursor;
                try
                {
                    using (var db = new QuedinDbContext())
                    {
                        string path = BackupService.Backup(db, dialog.SelectedPath);
                        Ui.Notify(this, "הגיבוי נשמר: " + path);
                    }
                }
                catch (Exception ex)
                {
                    Ui.Error("הגיבוי נכשל.", ex);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        // ------------------------------------------------------------------ logout

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (!ConfirmExit()) return;
            confirmedExit = true;
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!confirmedExit && e.CloseReason == CloseReason.UserClosing && !ConfirmExit())
            {
                e.Cancel = true;
                return;
            }
            CaseWindows.CloseAll();
            base.OnFormClosing(e);
        }

        private static bool ConfirmExit()
        {
            string message = "להתנתק מהמערכת?";
            if (CaseWindows.OpenCount > 0)
                message += "\nפתוחים " + CaseWindows.OpenCount + " חלונות של תיקים - שינויים שלא נשמרו בהם יאבדו.";
            return Ui.Confirm(message);
        }
    }
}
