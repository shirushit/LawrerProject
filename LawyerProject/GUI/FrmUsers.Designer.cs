namespace LawyerProject.GUI
{
    partial class FrmUsers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.groupBoxLogin = new System.Windows.Forms.GroupBox();
            this.lnkChangePassword = new System.Windows.Forms.LinkLabel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.chkShowPassword = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.groupBoxChangePassword = new System.Windows.Forms.GroupBox();
            this.btnCancelPassword = new System.Windows.Forms.Button();
            this.btnSavePassword = new System.Windows.Forms.Button();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.txtOldPassword = new System.Windows.Forms.TextBox();
            this.lblOldPassword = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.groupBoxLogin.SuspendLayout();
            this.groupBoxChangePassword.SuspendLayout();
            this.lblKeyboardWarning = new System.Windows.Forms.Label();
            this.lblCpUser = new System.Windows.Forms.Label();
            this.txtCpUser = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            //
            // pictureBoxLogo
            //
            this.pictureBoxLogo.Image = global::LawyerProject.Properties.Resources.לוגו;
            this.pictureBoxLogo.Location = new System.Drawing.Point(20, 12);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(400, 110);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            //
            // groupBoxLogin
            //
            this.groupBoxLogin.Controls.Add(this.lnkChangePassword);
            this.groupBoxLogin.Controls.Add(this.lblKeyboardWarning);
            this.groupBoxLogin.Controls.Add(this.btnLogin);
            this.groupBoxLogin.Controls.Add(this.chkShowPassword);
            this.groupBoxLogin.Controls.Add(this.txtPassword);
            this.groupBoxLogin.Controls.Add(this.lblPassword);
            this.groupBoxLogin.Controls.Add(this.txtUserName);
            this.groupBoxLogin.Controls.Add(this.lblUserName);
            this.groupBoxLogin.Location = new System.Drawing.Point(20, 130);
            this.groupBoxLogin.Name = "groupBoxLogin";
            this.groupBoxLogin.Size = new System.Drawing.Size(400, 240);
            this.groupBoxLogin.TabIndex = 1;
            this.groupBoxLogin.TabStop = false;
            this.groupBoxLogin.Text = "כניסת משתמש";
            //
            // lblUserName
            //
            this.lblUserName.AutoSize = false;
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblUserName.Location = new System.Drawing.Point(276, 38);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(104, 23);
            this.lblUserName.TabIndex = 0;
            this.lblUserName.Text = "שם משתמש";
            //
            // txtUserName
            //
            this.txtUserName.Location = new System.Drawing.Point(30, 35);
            this.txtUserName.MaxLength = 50;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(230, 27);
            this.txtUserName.TabIndex = 1;
            //
            // lblPassword
            //
            this.lblPassword.AutoSize = false;
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPassword.Location = new System.Drawing.Point(306, 78);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(74, 23);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "סיסמה";
            //
            // txtPassword
            //
            this.txtPassword.Location = new System.Drawing.Point(30, 75);
            this.txtPassword.MaxLength = 100;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(230, 27);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            //
            // chkShowPassword
            //
            this.chkShowPassword.AutoSize = false;
            this.chkShowPassword.Location = new System.Drawing.Point(160, 110);
            this.chkShowPassword.Name = "chkShowPassword";
            this.chkShowPassword.Size = new System.Drawing.Size(100, 23);
            this.chkShowPassword.TabIndex = 4;
            this.chkShowPassword.Text = "הצג סיסמה";
            this.chkShowPassword.UseVisualStyleBackColor = true;
            this.chkShowPassword.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);
            //
            // btnLogin
            //
            this.btnLogin.Location = new System.Drawing.Point(150, 145);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(110, 38);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "כניסה";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            //
            // lnkChangePassword
            //
            this.lnkChangePassword.AutoSize = true;
            this.lnkChangePassword.Location = new System.Drawing.Point(30, 155);
            this.lnkChangePassword.Name = "lnkChangePassword";
            this.lnkChangePassword.Size = new System.Drawing.Size(100, 19);
            this.lnkChangePassword.TabIndex = 6;
            this.lnkChangePassword.TabStop = true;
            this.lnkChangePassword.Text = "החלפת סיסמה";
            this.lnkChangePassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkChangePassword_LinkClicked);
            //
                        // 
            // lblKeyboardWarning
            // 
            this.lblKeyboardWarning.ForeColor = System.Drawing.Color.Firebrick;
            this.lblKeyboardWarning.Location = new System.Drawing.Point(20, 195);
            this.lblKeyboardWarning.Name = "lblKeyboardWarning";
            this.lblKeyboardWarning.Size = new System.Drawing.Size(360, 30);
            this.lblKeyboardWarning.TabIndex = 20;
            this.lblKeyboardWarning.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblKeyboardWarning.Text = "";
            // 
            // lblCpUser
            // 
            this.lblCpUser.Location = new System.Drawing.Point(266, 38);
            this.lblCpUser.Name = "lblCpUser";
            this.lblCpUser.Size = new System.Drawing.Size(114, 23);
            this.lblCpUser.TabIndex = 21;
            this.lblCpUser.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCpUser.Text = "שם משתמש";
            // 
            // txtCpUser
            // 
            this.txtCpUser.Location = new System.Drawing.Point(30, 35);
            this.txtCpUser.Name = "txtCpUser";
            this.txtCpUser.Size = new System.Drawing.Size(230, 27);
            this.txtCpUser.TabIndex = 0;
// groupBoxChangePassword
            //
            this.groupBoxChangePassword.Controls.Add(this.btnCancelPassword);
            this.groupBoxChangePassword.Controls.Add(this.txtCpUser);
            this.groupBoxChangePassword.Controls.Add(this.lblCpUser);
            this.groupBoxChangePassword.Controls.Add(this.btnSavePassword);
            this.groupBoxChangePassword.Controls.Add(this.txtConfirmPassword);
            this.groupBoxChangePassword.Controls.Add(this.lblConfirmPassword);
            this.groupBoxChangePassword.Controls.Add(this.txtNewPassword);
            this.groupBoxChangePassword.Controls.Add(this.lblNewPassword);
            this.groupBoxChangePassword.Controls.Add(this.txtOldPassword);
            this.groupBoxChangePassword.Controls.Add(this.lblOldPassword);
            this.groupBoxChangePassword.Location = new System.Drawing.Point(20, 380);
            this.groupBoxChangePassword.Name = "groupBoxChangePassword";
            this.groupBoxChangePassword.Size = new System.Drawing.Size(400, 250);
            this.groupBoxChangePassword.TabIndex = 2;
            this.groupBoxChangePassword.TabStop = false;
            this.groupBoxChangePassword.Text = "החלפת סיסמה";
            this.groupBoxChangePassword.Visible = false;
            //
            // lblOldPassword
            //
            this.lblOldPassword.AutoSize = false;
            this.lblOldPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblOldPassword.Location = new System.Drawing.Point(266, 78);
            this.lblOldPassword.Name = "lblOldPassword";
            this.lblOldPassword.Size = new System.Drawing.Size(114, 23);
            this.lblOldPassword.TabIndex = 0;
            this.lblOldPassword.Text = "סיסמה נוכחית";
            //
            // txtOldPassword
            //
            this.txtOldPassword.Location = new System.Drawing.Point(30, 75);
            this.txtOldPassword.MaxLength = 100;
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.Size = new System.Drawing.Size(230, 27);
            this.txtOldPassword.TabIndex = 1;
            this.txtOldPassword.UseSystemPasswordChar = true;
            //
            // lblNewPassword
            //
            this.lblNewPassword.AutoSize = false;
            this.lblNewPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblNewPassword.Location = new System.Drawing.Point(266, 118);
            this.lblNewPassword.Name = "lblNewPassword";
            this.lblNewPassword.Size = new System.Drawing.Size(114, 23);
            this.lblNewPassword.TabIndex = 2;
            this.lblNewPassword.Text = "סיסמה חדשה";
            //
            // txtNewPassword
            //
            this.txtNewPassword.Location = new System.Drawing.Point(30, 115);
            this.txtNewPassword.MaxLength = 100;
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Size = new System.Drawing.Size(230, 27);
            this.txtNewPassword.TabIndex = 3;
            this.txtNewPassword.UseSystemPasswordChar = true;
            //
            // lblConfirmPassword
            //
            this.lblConfirmPassword.AutoSize = false;
            this.lblConfirmPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblConfirmPassword.Location = new System.Drawing.Point(266, 158);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(114, 23);
            this.lblConfirmPassword.TabIndex = 4;
            this.lblConfirmPassword.Text = "אימות סיסמה";
            //
            // txtConfirmPassword
            //
            this.txtConfirmPassword.Location = new System.Drawing.Point(30, 155);
            this.txtConfirmPassword.MaxLength = 100;
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.Size = new System.Drawing.Size(230, 27);
            this.txtConfirmPassword.TabIndex = 5;
            this.txtConfirmPassword.UseSystemPasswordChar = true;
            //
            // btnSavePassword
            //
            this.btnSavePassword.Location = new System.Drawing.Point(150, 195);
            this.btnSavePassword.Name = "btnSavePassword";
            this.btnSavePassword.Size = new System.Drawing.Size(110, 38);
            this.btnSavePassword.TabIndex = 6;
            this.btnSavePassword.Text = "שמירה";
            this.btnSavePassword.UseVisualStyleBackColor = true;
            this.btnSavePassword.Click += new System.EventHandler(this.btnSavePassword_Click);
            //
            // btnCancelPassword
            //
            this.btnCancelPassword.Location = new System.Drawing.Point(30, 195);
            this.btnCancelPassword.Name = "btnCancelPassword";
            this.btnCancelPassword.Size = new System.Drawing.Size(110, 38);
            this.btnCancelPassword.TabIndex = 7;
            this.btnCancelPassword.Text = "ביטול";
            this.btnCancelPassword.UseVisualStyleBackColor = true;
            this.btnCancelPassword.Click += new System.EventHandler(this.btnCancelPassword_Click);
            //
            // FrmUsers
            //
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 390);
            this.Controls.Add(this.groupBoxChangePassword);
            this.Controls.Add(this.groupBoxLogin);
            this.Controls.Add(this.pictureBoxLogo);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmUsers";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QUEDIN - כניסה למערכת";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.groupBoxLogin.ResumeLayout(false);
            this.groupBoxLogin.PerformLayout();
            this.groupBoxChangePassword.ResumeLayout(false);
            this.groupBoxChangePassword.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.GroupBox groupBoxLogin;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkShowPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.LinkLabel lnkChangePassword;
        private System.Windows.Forms.GroupBox groupBoxChangePassword;
        private System.Windows.Forms.Label lblOldPassword;
        private System.Windows.Forms.TextBox txtOldPassword;
        private System.Windows.Forms.Label lblNewPassword;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Button btnSavePassword;
        private System.Windows.Forms.Label lblKeyboardWarning;
        private System.Windows.Forms.Label lblCpUser;
        private System.Windows.Forms.TextBox txtCpUser;
        private System.Windows.Forms.Button btnCancelPassword;
    }
}
