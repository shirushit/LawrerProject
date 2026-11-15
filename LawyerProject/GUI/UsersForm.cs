using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Manager-only screen: list users and add new ones (passwords are hashed).</summary>
    public partial class UsersForm : Form
    {
        private readonly QuedinDbContext db = new QuedinDbContext();

        public UsersForm()
        {
            InitializeComponent();
            cmbRole.Items.AddRange(Roles.All);
            cmbRole.SelectedItem = Roles.Worker;
            LoadUsers();
        }

        private void LoadUsers()
        {
            // The password hash is never loaded into the grid.
            DateTime now = Clock.Now;
            gridUsers.DataSource = db.Users
                .OrderBy(u => u.UserName)
                .Select(u => new { u.UserId, u.UserName, u.Role, u.IsActive, u.LockedUntil })
                .ToList()
                .Select(u => new
                {
                    u.UserId,
                    u.UserName,
                    u.Role,
                    Status = !u.IsActive ? "מושבת" : LoginPolicy.IsLocked(u.LockedUntil, now) ? "נעול עד " + u.LockedUntil.Value.ToString("HH:mm") : "פעיל"
                })
                .ToList();
            Ui.SetHeaders(gridUsers, new Dictionary<string, string>
            {
                { "UserName", "שם משתמש" },
                { "Role", "סוג משתמש" },
                { "Status", "מצב" }
            });
        }

        /// <summary>Turns a user's access off (for example an employee who left) or back on.</summary>
        private void btnToggleActive_Click(object sender, EventArgs e)
        {
            object name = Ui.SelectedValue(gridUsers, "UserName");
            if (name == null)
            {
                Ui.Error("יש לבחור משתמש מהרשימה.");
                return;
            }
            User user = db.Users.FirstOrDefault(u => u.UserName == (string)name);
            if (user == null) return;
            bool activate = !user.IsActive;
            if (!Ui.Confirm(activate ? "להפעיל מחדש את המשתמש " + name + "?" : "להשבית את המשתמש " + name + "? הוא לא יוכל להיכנס למערכת עד שיופעל מחדש.")) return;
            try
            {
                string error = new AuthService(db).SetActive((string)name, activate);
                if (error != null)
                {
                    Ui.Error(error);
                    return;
                }
                LoadUsers();
                Ui.Notify(this, activate ? "המשתמש הופעל." : "המשתמש הושבת.");
            }
            catch (Exception ex)
            {
                Ui.Error("השינוי לא נשמר.", ex);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string error = new AuthService(db).AddUser(txtUserName.Text, txtPassword.Text, cmbRole.SelectedItem as string);
                if (error != null)
                {
                    Ui.Error(error);
                    return;
                }
                Ui.Notify(this, "המשתמש " + txtUserName.Text.Trim() + " נוסף.");
                txtUserName.Clear();
                txtPassword.Clear();
                LoadUsers();
            }
            catch (Exception ex)
            {
                Ui.Error("שגיאה בהוספת המשתמש.", ex);
            }
        }

        /// <summary>For a user who forgot the password: the manager sets a temporary one and gives it to the user.</summary>
        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            object name = Ui.SelectedValue(gridUsers, "UserName");
            if (name == null)
            {
                Ui.Error("יש לבחור משתמש מהרשימה.");
                return;
            }
            string password = Ui.AskText(this, "איפוס סיסמה",
                "סיסמה זמנית חדשה למשתמש " + name + " (לפחות 8 תווים, אות וספרה).\nמסרו אותה למשתמש ובקשו ממנו להחליף אותה במסך הכניסה.", "", 50);
            if (password == null) return;
            try
            {
                string error = new AuthService(db).ResetPassword(name.ToString(), password);
                if (error != null)
                {
                    Ui.Error(error);
                    return;
                }
                LoadUsers();
                Ui.Notify(this, "הסיסמה של " + name + " אופסה (ונעילה, אם הייתה, שוחררה).");
            }
            catch (Exception ex)
            {
                Ui.Error("הסיסמה לא אופסה.", ex);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db.Dispose();
            base.OnFormClosed(e);
        }
    }
}
