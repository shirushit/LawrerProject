using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Manager-only screen: list users and add new ones (passwords are hashed).</summary>
    public partial class FrmManageUsers : Form
    {
        private readonly LawyerDBEntities1 db = new LawyerDBEntities1();

        public FrmManageUsers()
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
                .Select(u => new { u.Code, u.UserName, u.KindWorker })
                .ToList();
            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "UserName", "שם משתמש" },
                { "KindWorker", "סוג משתמש" }
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
                Ui.Info("המשתמש נוסף בהצלחה.");
                txtUserName.Clear();
                txtPassword.Clear();
                LoadUsers();
            }
            catch (Exception ex)
            {
                Ui.Error("שגיאה בהוספת המשתמש.", ex);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db.Dispose();
            base.OnFormClosed(e);
        }
    }
}
