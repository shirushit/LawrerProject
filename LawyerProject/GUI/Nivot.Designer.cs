namespace LawyerProject.GUI
{
    partial class Nivot
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
            this.btnCustomers = new System.Windows.Forms.Button();
            this.btnKabalot = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnIskaot = new System.Windows.Forms.Button();
            this.btnTikim = new System.Windows.Forms.Button();
            this.btnCourts = new System.Windows.Forms.Button();
            this.btnCaseTypes = new System.Windows.Forms.Button();
            this.btnProcesses = new System.Windows.Forms.Button();
            this.btnUsers = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelToday = new System.Windows.Forms.Panel();
            this.lblTodayTitle = new System.Windows.Forms.Label();
            this.lblTodaySub = new System.Windows.Forms.Label();
            this.gridDue = new System.Windows.Forms.DataGridView();
            this.lblSetup = new System.Windows.Forms.Label();
            this.btnSetupGuide = new System.Windows.Forms.Button();
            this.btnAudit = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridDue)).BeginInit();
            this.panelToday.SuspendLayout();
            this.SuspendLayout();
            //
            // btnAbout
            //
            this.btnAbout.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.btnAbout.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbout.ForeColor = System.Drawing.Color.FromArgb(235, 200, 110);
            this.btnAbout.Location = new System.Drawing.Point(1075, 44);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(202, 56);
            this.btnAbout.TabIndex = 0;
            this.btnAbout.Text = "אודות";
            this.btnAbout.UseVisualStyleBackColor = false;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            //
            // btnCustomers
            //
            this.btnCustomers.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.btnCustomers.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomers.ForeColor = System.Drawing.Color.FromArgb(235, 200, 110);
            this.btnCustomers.Location = new System.Drawing.Point(815, 44);
            this.btnCustomers.Name = "btnCustomers";
            this.btnCustomers.Size = new System.Drawing.Size(202, 56);
            this.btnCustomers.TabIndex = 1;
            this.btnCustomers.Text = "לקוחות";
            this.btnCustomers.UseVisualStyleBackColor = false;
            this.btnCustomers.Click += new System.EventHandler(this.btnCustomers_Click);
            //
            // btnTikim
            //
            this.btnTikim.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.btnTikim.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTikim.ForeColor = System.Drawing.Color.FromArgb(235, 200, 110);
            this.btnTikim.Location = new System.Drawing.Point(554, 44);
            this.btnTikim.Name = "btnTikim";
            this.btnTikim.Size = new System.Drawing.Size(202, 56);
            this.btnTikim.TabIndex = 2;
            this.btnTikim.Text = "תיקים";
            this.btnTikim.UseVisualStyleBackColor = false;
            this.btnTikim.Click += new System.EventHandler(this.btnTikim_Click);
            //
            // btnIskaot
            //
            this.btnIskaot.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.btnIskaot.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIskaot.ForeColor = System.Drawing.Color.FromArgb(235, 200, 110);
            this.btnIskaot.Location = new System.Drawing.Point(295, 44);
            this.btnIskaot.Name = "btnIskaot";
            this.btnIskaot.Size = new System.Drawing.Size(202, 56);
            this.btnIskaot.TabIndex = 3;
            this.btnIskaot.Text = "עסקאות";
            this.btnIskaot.UseVisualStyleBackColor = false;
            this.btnIskaot.Click += new System.EventHandler(this.btnIskaot_Click);
            //
            // btnKabalot
            //
            this.btnKabalot.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.btnKabalot.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKabalot.ForeColor = System.Drawing.Color.FromArgb(235, 200, 110);
            this.btnKabalot.Location = new System.Drawing.Point(35, 44);
            this.btnKabalot.Name = "btnKabalot";
            this.btnKabalot.Size = new System.Drawing.Size(202, 56);
            this.btnKabalot.TabIndex = 4;
            this.btnKabalot.Text = "תשלומים וקבלות";
            this.btnKabalot.UseVisualStyleBackColor = false;
            this.btnKabalot.Click += new System.EventHandler(this.btnKabalot_Click);
            //
            // btnCourts
            //
            this.btnCourts.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.btnCourts.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCourts.ForeColor = System.Drawing.Color.FromArgb(235, 200, 110);
            this.btnCourts.Location = new System.Drawing.Point(1075, 120);
            this.btnCourts.Name = "btnCourts";
            this.btnCourts.Size = new System.Drawing.Size(202, 46);
            this.btnCourts.TabIndex = 5;
            this.btnCourts.Text = "בתי משפט";
            this.btnCourts.UseVisualStyleBackColor = false;
            this.btnCourts.Click += new System.EventHandler(this.btnCourts_Click);
            //
            // btnCaseTypes
            //
            this.btnCaseTypes.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.btnCaseTypes.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCaseTypes.ForeColor = System.Drawing.Color.FromArgb(235, 200, 110);
            this.btnCaseTypes.Location = new System.Drawing.Point(815, 120);
            this.btnCaseTypes.Name = "btnCaseTypes";
            this.btnCaseTypes.Size = new System.Drawing.Size(202, 46);
            this.btnCaseTypes.TabIndex = 6;
            this.btnCaseTypes.Text = "נושאי תיקים";
            this.btnCaseTypes.UseVisualStyleBackColor = false;
            this.btnCaseTypes.Click += new System.EventHandler(this.btnCaseTypes_Click);
            //
            // btnProcesses
            //
            this.btnProcesses.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.btnProcesses.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcesses.ForeColor = System.Drawing.Color.FromArgb(235, 200, 110);
            this.btnProcesses.Location = new System.Drawing.Point(554, 120);
            this.btnProcesses.Name = "btnProcesses";
            this.btnProcesses.Size = new System.Drawing.Size(202, 46);
            this.btnProcesses.TabIndex = 7;
            this.btnProcesses.Text = "תהליכים";
            this.btnProcesses.UseVisualStyleBackColor = false;
            this.btnProcesses.Click += new System.EventHandler(this.btnProcesses_Click);
            //
            // btnUsers
            //
            this.btnUsers.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.btnUsers.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUsers.ForeColor = System.Drawing.Color.FromArgb(235, 200, 110);
            this.btnUsers.Location = new System.Drawing.Point(295, 120);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(202, 46);
            this.btnUsers.TabIndex = 8;
            this.btnUsers.Text = "ניהול משתמשים";
            this.btnUsers.UseVisualStyleBackColor = false;
            this.btnUsers.Click += new System.EventHandler(this.btnUsers_Click);
            //
            // btnLogout
            //
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.btnLogout.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(235, 200, 110);
            this.btnLogout.Location = new System.Drawing.Point(35, 120);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(202, 46);
            this.btnLogout.TabIndex = 9;
            this.btnLogout.Text = "התנתקות";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            //
            // lblWelcome
            //
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.lblWelcome.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.ForeColor = System.Drawing.Color.FromArgb(235, 200, 110);
            this.lblWelcome.Location = new System.Drawing.Point(180, 700);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(0, 23);
            this.lblWelcome.TabIndex = 10;
            // 
            // panelToday
            // 
            this.panelToday.BackColor = System.Drawing.Color.White;
            this.panelToday.Controls.Add(this.lblTodayTitle);
            this.panelToday.Controls.Add(this.lblTodaySub);
            this.panelToday.Controls.Add(this.gridDue);
            this.panelToday.Controls.Add(this.lblSetup);
            this.panelToday.Location = new System.Drawing.Point(35, 185);
            this.panelToday.Name = "panelToday";
            this.panelToday.Size = new System.Drawing.Size(1242, 495);
            this.panelToday.TabIndex = 20;
            // 
            // lblTodayTitle
            // 
            this.lblTodayTitle.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold);
            this.lblTodayTitle.Location = new System.Drawing.Point(732, 10);
            this.lblTodayTitle.Name = "lblTodayTitle";
            this.lblTodayTitle.Size = new System.Drawing.Size(500, 32);
            this.lblTodayTitle.TabIndex = 0;
            this.lblTodayTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTodayTitle.Text = "מה דחוף היום";
            // 
            // lblTodaySub
            // 
            this.lblTodaySub.Font = new System.Drawing.Font("Calibri", 11F);
            this.lblTodaySub.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblTodaySub.Location = new System.Drawing.Point(232, 42);
            this.lblTodaySub.Name = "lblTodaySub";
            this.lblTodaySub.Size = new System.Drawing.Size(1000, 24);
            this.lblTodaySub.TabIndex = 1;
            this.lblTodaySub.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTodaySub.Text = "השלב הבא בכל תיק פתוח שמועדו עבר או מתקרב. לחיצה כפולה פותחת את התיק.";
            // 
            // gridDue
            // 
            this.gridDue.BackgroundColor = System.Drawing.Color.White;
            this.gridDue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDue.Font = new System.Drawing.Font("Calibri", 11F);
            this.gridDue.Location = new System.Drawing.Point(10, 72);
            this.gridDue.Name = "gridDue";
            this.gridDue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridDue.RowHeadersVisible = false;
            this.gridDue.Size = new System.Drawing.Size(1222, 360);
            this.gridDue.TabIndex = 2;
            this.gridDue.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridDue_CellDoubleClick);
            // 
            // lblSetup
            // 
            this.lblSetup.BackColor = System.Drawing.Color.LightYellow;
            this.lblSetup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblSetup.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.lblSetup.Location = new System.Drawing.Point(10, 442);
            this.lblSetup.Name = "lblSetup";
            this.lblSetup.Size = new System.Drawing.Size(1222, 42);
            this.lblSetup.TabIndex = 3;
            this.lblSetup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSetup.Text = "המערכת עדיין לא הוגדרה: אין נושאי תיקים עם תהליכים. לחצו כאן למדריך ההקמה.";
            this.lblSetup.Visible = false;
            this.lblSetup.Click += new System.EventHandler(this.btnSetupGuide_Click);
            // 
            // btnSetupGuide
            // 
            this.btnSetupGuide.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.btnSetupGuide.Location = new System.Drawing.Point(1075, 692);
            this.btnSetupGuide.Name = "btnSetupGuide";
            this.btnSetupGuide.Size = new System.Drawing.Size(202, 40);
            this.btnSetupGuide.TabIndex = 30;
            this.btnSetupGuide.Text = "מדריך הקמה";
            this.btnSetupGuide.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.btnSetupGuide.ForeColor = System.Drawing.Color.FromArgb(235, 200, 110);
            this.btnSetupGuide.UseVisualStyleBackColor = false;
            this.btnSetupGuide.Click += new System.EventHandler(this.btnSetupGuide_Click);
            // 
            // btnAudit
            // 
            this.btnAudit.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.btnAudit.Location = new System.Drawing.Point(815, 692);
            this.btnAudit.Name = "btnAudit";
            this.btnAudit.Size = new System.Drawing.Size(202, 40);
            this.btnAudit.TabIndex = 31;
            this.btnAudit.Text = "יומן פעולות";
            this.btnAudit.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.btnAudit.ForeColor = System.Drawing.Color.FromArgb(235, 200, 110);
            this.btnAudit.UseVisualStyleBackColor = false;
            this.btnAudit.Click += new System.EventHandler(this.btnAudit_Click);
            // 
            // btnBackup
            // 
            this.btnBackup.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.btnBackup.Location = new System.Drawing.Point(554, 692);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(202, 40);
            this.btnBackup.TabIndex = 32;
            this.btnBackup.Text = "גיבוי נתונים";
            this.btnBackup.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.btnBackup.ForeColor = System.Drawing.Color.FromArgb(235, 200, 110);
            this.btnBackup.UseVisualStyleBackColor = false;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            //
            // pictureBox1
            //
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.pictureBox1.Image = global::LawyerProject.Properties.Resources.לוגו2;
            this.pictureBox1.Location = new System.Drawing.Point(35, 686);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(130, 52);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            //
            // Nivot
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
            this.ClientSize = new System.Drawing.Size(1352, 745);
            this.Controls.Add(this.panelToday);
            this.Controls.Add(this.btnSetupGuide);
            this.Controls.Add(this.btnAudit);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.lblWelcome);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnUsers);
            this.Controls.Add(this.btnProcesses);
            this.Controls.Add(this.btnCaseTypes);
            this.Controls.Add(this.btnCourts);
            this.Controls.Add(this.btnTikim);
            this.Controls.Add(this.btnIskaot);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnKabalot);
            this.Controls.Add(this.btnCustomers);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Nivot";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QUEDIN - דף הבית";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDue)).EndInit();
            this.panelToday.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCustomers;
        private System.Windows.Forms.Panel panelToday;
        private System.Windows.Forms.Label lblTodayTitle;
        private System.Windows.Forms.Label lblTodaySub;
        private System.Windows.Forms.DataGridView gridDue;
        private System.Windows.Forms.Label lblSetup;
        private System.Windows.Forms.Button btnSetupGuide;
        private System.Windows.Forms.Button btnAudit;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Button btnKabalot;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnIskaot;
        private System.Windows.Forms.Button btnTikim;
        private System.Windows.Forms.Button btnCourts;
        private System.Windows.Forms.Button btnCaseTypes;
        private System.Windows.Forms.Button btnProcesses;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
