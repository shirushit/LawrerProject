namespace LawyerProject.GUI
{
    partial class UsersForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBoxAdd = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblPasswordRules = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBoxAdd.SuspendLayout();
            this.btnResetPassword = new System.Windows.Forms.Button();
            this.btnToggleActive = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // dataGridView1
            //
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(20, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(420, 200);
            this.dataGridView1.TabIndex = 0;
            //
            // groupBoxAdd
            //
            this.groupBoxAdd.Controls.Add(this.lblPasswordRules);
            this.groupBoxAdd.Controls.Add(this.btnAdd);
            this.groupBoxAdd.Controls.Add(this.cmbRole);
            this.groupBoxAdd.Controls.Add(this.lblRole);
            this.groupBoxAdd.Controls.Add(this.txtPassword);
            this.groupBoxAdd.Controls.Add(this.lblPassword);
            this.groupBoxAdd.Controls.Add(this.txtUserName);
            this.groupBoxAdd.Controls.Add(this.lblUserName);
            this.groupBoxAdd.Location = new System.Drawing.Point(20, 280);
            this.groupBoxAdd.Name = "groupBoxAdd";
            this.groupBoxAdd.Size = new System.Drawing.Size(420, 230);
            this.groupBoxAdd.TabIndex = 1;
            this.groupBoxAdd.TabStop = false;
            this.groupBoxAdd.Text = "הוספת משתמש";
            //
            // lblUserName
            //
            this.lblUserName.AutoSize = false;
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblUserName.Location = new System.Drawing.Point(296, 38);
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
            this.txtUserName.Size = new System.Drawing.Size(250, 27);
            this.txtUserName.TabIndex = 1;
            //
            // lblPassword
            //
            this.lblPassword.AutoSize = false;
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPassword.Location = new System.Drawing.Point(326, 78);
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
            this.txtPassword.Size = new System.Drawing.Size(250, 27);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            //
            // lblRole
            //
            this.lblRole.AutoSize = false;
            this.lblRole.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblRole.Location = new System.Drawing.Point(286, 118);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(114, 23);
            this.lblRole.TabIndex = 4;
            this.lblRole.Text = "סוג משתמש";
            //
            // cmbRole
            //
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.Location = new System.Drawing.Point(30, 115);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(250, 27);
            this.cmbRole.TabIndex = 5;
            //
            // lblPasswordRules
            //
            this.lblPasswordRules.AutoSize = false;
            this.lblPasswordRules.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPasswordRules.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblPasswordRules.Location = new System.Drawing.Point(3, 150);
            this.lblPasswordRules.Name = "lblPasswordRules";
            this.lblPasswordRules.Size = new System.Drawing.Size(397, 23);
            this.lblPasswordRules.TabIndex = 6;
            this.lblPasswordRules.Text = "סיסמה: לפחות 8 תווים, אות אחת וספרה אחת";
            //
            // btnAdd
            //
            this.btnAdd.Location = new System.Drawing.Point(170, 178);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(110, 38);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "הוספה";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnResetPassword
            // 
            this.btnResetPassword.Location = new System.Drawing.Point(270, 230);
            this.btnResetPassword.Name = "btnResetPassword";
            this.btnResetPassword.Size = new System.Drawing.Size(170, 36);
            this.btnResetPassword.TabIndex = 5;
            this.btnResetPassword.Text = "איפוס סיסמה...";
            this.btnResetPassword.UseVisualStyleBackColor = true;
            this.btnResetPassword.Click += new System.EventHandler(this.btnResetPassword_Click);
            // 
            // btnToggleActive
            // 
            this.btnToggleActive.Location = new System.Drawing.Point(90, 230);
            this.btnToggleActive.Name = "btnToggleActive";
            this.btnToggleActive.Size = new System.Drawing.Size(170, 36);
            this.btnToggleActive.TabIndex = 6;
            this.btnToggleActive.Text = "השבתה / הפעלה";
            this.btnToggleActive.UseVisualStyleBackColor = true;
            this.btnToggleActive.Click += new System.EventHandler(this.btnToggleActive_Click);
            //
            // UsersForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 530);
            this.Controls.Add(this.groupBoxAdd);
            this.Controls.Add(this.btnResetPassword);
            this.Controls.Add(this.btnToggleActive);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "UsersForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ניהול משתמשים";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBoxAdd.ResumeLayout(false);
            this.groupBoxAdd.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBoxAdd;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Label lblPasswordRules;
        private System.Windows.Forms.Button btnResetPassword;
        private System.Windows.Forms.Button btnToggleActive;
        private System.Windows.Forms.Button btnAdd;
    }
}
