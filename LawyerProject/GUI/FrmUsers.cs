using LawyerProject.BLL;
using LawyerProject.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace LawyerProject.GUI
{
    public partial class FrmUsers : Form
    {

        Users us;
        LawyerDBEntities1 d;
        public FrmUsers()
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            AllStatic.MyUsers = new Users();


        }
        bool flagMannager;
        private void btnMannager_Click(object sender, EventArgs e)
        {
            groupBoxChangePassword.Visible = true;
            btnChangePassword.Visible = true;
            btnOK.Visible = true;
            btnAddUser.Visible = true;
            flagMannager = true;
        }

        private void btnRegularUser_Click(object sender, EventArgs e)
        {
            groupBoxEnterUser.Visible = true;
            btnChangePassword.Visible = true;
            btnOK.Visible = true;
            btnAddUser.Visible = false;
            gbAUser.Visible = false;
            flagMannager = false;

        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {

            AllStatic.MyUsers = d.Users.FirstOrDefault(x => x.UserName == txtnameUser.Text && x.PasswordWorker == txtPssword.Text);
            if (AllStatic.MyUsers != null)
            {
                groupBoxChangePassword.Visible = true;
                btnAddUser.Visible = false;
                gbAUser.Visible = false;
            }
            else
                MessageBox.Show(" הסיסמה או שם המשתמש שגויים");

        }
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            gbAUser.Visible = true;
            AllStatic.MyUsers = d.Users.FirstOrDefault(x => x.UserName == txtnameUser.Text
            && x.PasswordWorker == txtPssword.Text);
            if (AllStatic.MyUsers != null && AllStatic.MyUsers.KindWorker == "מנהל")
            {
                gbAUser.Visible = true;
                groupBoxChangePassword.Visible = false;
                btnOkOK.Visible = true;

            }
            else
                MessageBox.Show("הוספת עובד תבוצע רק על ידי מנהל המערכת");
        }

        private void pbOpass_Click(object sender, EventArgs e)
        {
            txtPssword.UseSystemPasswordChar = false;
            pbCpass.Visible = true;
            pbOpass.Visible = false;
        }

        private void pbCpass_Click(object sender, EventArgs e)
        {
            txtPssword.UseSystemPasswordChar = true;
            pbCpass.Visible = false;
            pbOpass.Visible = true;
        }


        private void pbclose_Click(object sender, EventArgs e)
        {
            txtNewPassword.UseSystemPasswordChar = true;
            pbclose.Visible = false;
            pbopen.Visible = true;
        }

        private void pbopen_Click(object sender, EventArgs e)
        {
            txtNewPassword.UseSystemPasswordChar = false;
            pbclose.Visible = true;
            pbopen.Visible = false;
        }

        private void pboOpen1_Click(object sender, EventArgs e)
        {
            txtPssword.UseSystemPasswordChar = false;
            pbClose1.Visible = true;
            pboOpen1.Visible = false;
        }
        private void pbClose1_Click(object sender, EventArgs e)
        {
            txtPssword.UseSystemPasswordChar = true;
            pbClose1.Visible = false;
            pboOpen1.Visible = true;
        }

        private void pbOpen2_Click(object sender, EventArgs e)
        {
            txtTruePassword.UseSystemPasswordChar = false;
            pbClose2.Visible = true;
            pbOpen2.Visible = false;
        }

        private void pbClose2_Click(object sender, EventArgs e)
        {
            txtTruePassword.UseSystemPasswordChar = true;
            pbClose2.Visible = false;
            pbOpen2.Visible = true;

        }

        private void pbOpen3_Click(object sender, EventArgs e)
        {
            txtOldPassword.UseSystemPasswordChar = false;
            pbClose3.Visible = true;
            pbOpen3.Visible = false;
        }

        private void pbClose3_Click(object sender, EventArgs e)
        {
            txtOldPassword.UseSystemPasswordChar = true;
            pbClose3.Visible = false;
            pbOpen3.Visible = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            AllStatic.MyUsers = d.Users.FirstOrDefault(x
               => x.UserName == txtnameUser.Text && x.PasswordWorker ==
               txtPssword.Text);
            if (d.Users.FirstOrDefault(x => x.UserName == txtnameUser.Text &&
              x.PasswordWorker == txtPssword.Text) != null)
            {
                if (flagMannager)
                {
                    if (AllStatic.MyUsers.KindWorker == "מנהל")
                    {
                        Nivot n = new Nivot();
                        n.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("הינך על מצב מנהל,עבור למצב משתמש רגיל");
                    }
                }
                else
                {
                    if (AllStatic.MyUsers.KindWorker != "מנהל")
                    {
                        Nivot n = new Nivot();
                        n.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("הינך על מצב משתמש רגיל, עבור למצב מנהל ");

                    }
                }

            }
            else
                MessageBox.Show("הסיסמה או שם המשתמש שגויים");
        }

        private void btnOkOK_Click(object sender, EventArgs e)
        {
            if (Validation.ChekPassword(txtPssword.Text) == true)
            {

                AllStatic.MyUsers.UserName = txtnameUser.Text;
                AllStatic.MyUsers.PasswordWorker = txtPssword.Text;
                AllStatic.MyUsers.KindWorker = cbAddTypeUser.Text;
                d.Users.Add(AllStatic.MyUsers);
                d.SaveChanges();
                MessageBox.Show("נוסף משתמש בהצלחה");
            }
        }

        private void btnOkCjangepassword_Click(object sender, EventArgs e)
        {
            if (Validation.ChekPassword(txtNewPassword.Text) == true)
                if (txtNewPassword.Text != txtTruePassword.Text)
                    MessageBox.Show("הסיסמאות לא תואמות");
                else
                {
                    AllStatic.MyUsers.PasswordWorker = txtNewPassword.Text;
                    d.Users.AddOrUpdate(AllStatic.MyUsers);
                    d.SaveChanges();
                    txtPssword.Text = txtNewPassword.Text;
                    MessageBox.Show("הסיסמה עודכנה בהצלחה");
                }
        }
    }
}
