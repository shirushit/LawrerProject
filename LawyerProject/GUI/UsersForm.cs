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
            dataGridView1.DataSource = db.Users
                .OrderBy(u => u.UserName)
                .Select(u => new { u.UserId, u.UserName, u.Role })
                .ToList();
            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "UserName", "שם משתמש" },
                { "Role", "סוג משתמש" }
            });
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
            object name = Ui.SelectedValue(dataGridView1, "UserName");
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
                Ui.Notify(this, "הסיסמה של " + name + " אופסה.");
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
