using System;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Login screen. The user's role (manager / worker) is read from the database.</summary>
    public partial class FrmUsers : Form
    {
        private const int LoginHeight = 360;
        private const int ChangePasswordHeight = 580;

        public FrmUsers()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new LawyerDBEntities1())
                {
                    Users user = new AuthService(db).Login(txtUserName.Text, txtPassword.Text);
                    if (user == null)
                    {
                        Ui.Error("שם המשתמש או הסיסמה שגויים.");
                        txtPassword.Clear();
                        txtPassword.Focus();
                        return;
                    }
                    Session.SignIn(user);
                }
            }
            catch (Exception ex)
            {
                Ui.Error("לא ניתן להתחבר למסד הנתונים. ודאו שהרצתם את database/create-database.sql.", ex);
                return;
            }

            txtPassword.Clear();
            Hide();
            using (var home = new Nivot())
                home.ShowDialog();

            // Back from the home screen = log out.
            Session.SignOut();
            Show();
            txtUserName.Focus();
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            bool hide = !chkShowPassword.Checked;
            txtPassword.UseSystemPasswordChar = hide;
            txtOldPassword.UseSystemPasswordChar = hide;
            txtNewPassword.UseSystemPasswordChar = hide;
            txtConfirmPassword.UseSystemPasswordChar = hide;
        }

        private void lnkChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SetChangePasswordVisible(true);
        }

        private void btnCancelPassword_Click(object sender, EventArgs e)
        {
            SetChangePasswordVisible(false);
        }

        private void btnSavePassword_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new LawyerDBEntities1())
                {
                    string error = new AuthService(db).ChangePassword(
                        txtUserName.Text, txtOldPassword.Text, txtNewPassword.Text, txtConfirmPassword.Text);
                    if (error != null)
                    {
                        Ui.Error(error);
                        return;
                    }
                }
                Ui.Info("הסיסמה עודכנה בהצלחה.");
                SetChangePasswordVisible(false);
            }
            catch (Exception ex)
            {
                Ui.Error("שגיאה בעדכון הסיסמה.", ex);
            }
        }

        private void SetChangePasswordVisible(bool visible)
        {
            groupBoxChangePassword.Visible = visible;
            ClientSize = new System.Drawing.Size(ClientSize.Width, visible ? ChangePasswordHeight : LoginHeight);
            txtOldPassword.Clear();
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();
            if (visible)
            {
                if (txtUserName.Text.Length == 0) Ui.Info("הזינו קודם את שם המשתמש בשדה למעלה.");
                txtOldPassword.Focus();
            }
        }
    }
}
