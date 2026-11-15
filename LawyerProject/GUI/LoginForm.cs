using System;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Login screen. The user's role (manager / worker) is read from the database.</summary>
    public partial class LoginForm : Form
    {
        private const int LoginHeight = 390;
        private const int ChangePasswordHeight = 650;

        public LoginForm()
        {
            InitializeComponent();
            // Passwords are case-sensitive and usually typed in English: warn about Caps Lock or a Hebrew keyboard.
            foreach (TextBox box in new[] { txtPassword, txtOldPassword, txtNewPassword, txtConfirmPassword })
            {
                box.Enter += (s, e) => UpdateKeyboardWarning();
                box.KeyUp += (s, e) => UpdateKeyboardWarning();
                box.Leave += (s, e) => lblKeyboardWarning.Text = "";
            }
            InputLanguageChanged += (s, e) => UpdateKeyboardWarning();
        }

        private void UpdateKeyboardWarning()
        {
            var warnings = new System.Collections.Generic.List<string>();
            if (Control.IsKeyLocked(Keys.CapsLock)) warnings.Add("Caps Lock פעיל");
            InputLanguage language = InputLanguage.CurrentInputLanguage;
            if (language != null && language.Culture.TwoLetterISOLanguageName == "he") warnings.Add("המקלדת בעברית");
            lblKeyboardWarning.Text = warnings.Count == 0 ? "" : "⚠ " + string.Join(" | ", warnings);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new QuedinDbContext())
                {
                    User user = new AuthService(db).Login(txtUserName.Text, txtPassword.Text);
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
                Ui.Error("לא ניתן להתחבר למערכת.", ex);
                return;
            }

            txtPassword.Clear();
            Hide();
            using (var home = new HomeForm())
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
                using (var db = new QuedinDbContext())
                {
                    string error = new AuthService(db).ChangePassword(
                        txtCpUser.Text, txtOldPassword.Text, txtNewPassword.Text, txtConfirmPassword.Text);
                    if (error != null)
                    {
                        Ui.Error(error);
                        return;
                    }
                }
                txtUserName.Text = txtCpUser.Text.Trim();
                SetChangePasswordVisible(false);
                Ui.Notify(this, "הסיסמה הוחלפה. אפשר להיכנס עם הסיסמה החדשה.");
                txtPassword.Focus();
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
                txtCpUser.Text = txtUserName.Text.Trim();
                if (txtCpUser.Text.Length == 0) txtCpUser.Focus();
                else txtOldPassword.Focus();
            }
        }
    }
}
