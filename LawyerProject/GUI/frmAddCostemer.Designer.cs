namespace LawyerProject.GUI
{
    partial class frmAddCostemer
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
            this.components = new System.ComponentModel.Container();
            this.maskTWOPhone1 = new System.Windows.Forms.MaskedTextBox();
            this.maskONEPhone1 = new System.Windows.Forms.MaskedTextBox();
            this.mackBirthDay1 = new System.Windows.Forms.MaskedTextBox();
            this.txtMail1 = new System.Windows.Forms.TextBox();
            this.txtName1 = new System.Windows.Forms.TextBox();
            this.txtLastName1 = new System.Windows.Forms.TextBox();
            this.txtAdress1 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtDock = new System.Windows.Forms.TextBox();
            this.txtIDcostemer1 = new System.Windows.Forms.MaskedTextBox();
            this.btnAddDock = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // maskTWOPhone1
            // 
            this.maskTWOPhone1.Location = new System.Drawing.Point(256, 443);
            this.maskTWOPhone1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.maskTWOPhone1.Mask = "000-0000000";
            this.maskTWOPhone1.Name = "maskTWOPhone1";
            this.maskTWOPhone1.Size = new System.Drawing.Size(132, 27);
            this.maskTWOPhone1.TabIndex = 76;
            // 
            // maskONEPhone1
            // 
            this.maskONEPhone1.Location = new System.Drawing.Point(256, 352);
            this.maskONEPhone1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.maskONEPhone1.Mask = "000-0000000";
            this.maskONEPhone1.Name = "maskONEPhone1";
            this.maskONEPhone1.Size = new System.Drawing.Size(132, 27);
            this.maskONEPhone1.TabIndex = 75;
            // 
            // mackBirthDay1
            // 
            this.mackBirthDay1.Location = new System.Drawing.Point(256, 585);
            this.mackBirthDay1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.mackBirthDay1.Mask = "00/00/0000";
            this.mackBirthDay1.Name = "mackBirthDay1";
            this.mackBirthDay1.Size = new System.Drawing.Size(132, 27);
            this.mackBirthDay1.TabIndex = 73;
            this.mackBirthDay1.ValidatingType = typeof(System.DateTime);
            // 
            // txtMail1
            // 
            this.txtMail1.Location = new System.Drawing.Point(256, 516);
            this.txtMail1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtMail1.Name = "txtMail1";
            this.txtMail1.Size = new System.Drawing.Size(132, 27);
            this.txtMail1.TabIndex = 67;
            // 
            // txtName1
            // 
            this.txtName1.Location = new System.Drawing.Point(256, 121);
            this.txtName1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtName1.Name = "txtName1";
            this.txtName1.Size = new System.Drawing.Size(132, 27);
            this.txtName1.TabIndex = 66;
            this.txtName1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName1_KeyPress);
            // 
            // txtLastName1
            // 
            this.txtLastName1.Location = new System.Drawing.Point(256, 197);
            this.txtLastName1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtLastName1.Name = "txtLastName1";
            this.txtLastName1.Size = new System.Drawing.Size(132, 27);
            this.txtLastName1.TabIndex = 65;
            this.txtLastName1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLastName1_KeyPress);
            // 
            // txtAdress1
            // 
            this.txtAdress1.Location = new System.Drawing.Point(256, 275);
            this.txtAdress1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtAdress1.Name = "txtAdress1";
            this.txtAdress1.Size = new System.Drawing.Size(132, 27);
            this.txtAdress1.TabIndex = 64;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label10.ForeColor = System.Drawing.SystemColors.Info;
            this.label10.Location = new System.Drawing.Point(410, 132);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 19);
            this.label10.TabIndex = 54;
            this.label10.Text = "שם פרטי";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label9.ForeColor = System.Drawing.SystemColors.Info;
            this.label9.Location = new System.Drawing.Point(410, 208);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 19);
            this.label9.TabIndex = 53;
            this.label9.Text = "שם משפחה";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.ForeColor = System.Drawing.SystemColors.Info;
            this.label8.Location = new System.Drawing.Point(410, 285);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 19);
            this.label8.TabIndex = 52;
            this.label8.Text = "כתובת ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.ForeColor = System.Drawing.SystemColors.Info;
            this.label7.Location = new System.Drawing.Point(410, 357);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 19);
            this.label7.TabIndex = 51;
            this.label7.Text = "טלפון 1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.ForeColor = System.Drawing.SystemColors.Info;
            this.label6.Location = new System.Drawing.Point(410, 447);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 19);
            this.label6.TabIndex = 50;
            this.label6.Text = "טלפון 2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.ForeColor = System.Drawing.SystemColors.Info;
            this.label5.Location = new System.Drawing.Point(422, 526);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 19);
            this.label5.TabIndex = 49;
            this.label5.Text = "מייל";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.ForeColor = System.Drawing.SystemColors.Info;
            this.label4.Location = new System.Drawing.Point(410, 595);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 19);
            this.label4.TabIndex = 48;
            this.label4.Text = "תאריך לידה";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.ForeColor = System.Drawing.SystemColors.Info;
            this.label3.Location = new System.Drawing.Point(416, 658);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 19);
            this.label3.TabIndex = 47;
            this.label3.Text = "צילום ת.ז";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.ForeColor = System.Drawing.Color.FloralWhite;
            this.label1.Location = new System.Drawing.Point(410, 56);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 19);
            this.label1.TabIndex = 45;
            this.label1.Text = "ת.ז לקוח";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // txtDock
            // 
            this.txtDock.Location = new System.Drawing.Point(256, 658);
            this.txtDock.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtDock.Name = "txtDock";
            this.txtDock.ReadOnly = true;
            this.txtDock.Size = new System.Drawing.Size(132, 27);
            this.txtDock.TabIndex = 80;
            // 
            // txtIDcostemer1
            // 
            this.txtIDcostemer1.Location = new System.Drawing.Point(256, 56);
            this.txtIDcostemer1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtIDcostemer1.Mask = "000000000";
            this.txtIDcostemer1.Name = "txtIDcostemer1";
            this.txtIDcostemer1.PromptChar = ' ';
            this.txtIDcostemer1.Size = new System.Drawing.Size(132, 27);
            this.txtIDcostemer1.TabIndex = 82;
            // 
            // btnAddDock
            // 
            this.btnAddDock.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddDock.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAddDock.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.btnAddDock.ForeColor = System.Drawing.Color.FloralWhite;
            this.btnAddDock.Location = new System.Drawing.Point(93, 651);
            this.btnAddDock.Name = "btnAddDock";
            this.btnAddDock.Size = new System.Drawing.Size(135, 39);
            this.btnAddDock.TabIndex = 83;
            this.btnAddDock.Text = "הוספת קובץ";
            this.btnAddDock.UseVisualStyleBackColor = false;
            this.btnAddDock.Visible = false;
            this.btnAddDock.Click += new System.EventHandler(this.btnAddDock_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAdd.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.FloralWhite;
            this.btnAdd.Location = new System.Drawing.Point(256, 714);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(109, 39);
            this.btnAdd.TabIndex = 84;
            this.btnAdd.Text = "שמירה";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmAddCostemer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(622, 786);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnAddDock);
            this.Controls.Add(this.txtIDcostemer1);
            this.Controls.Add(this.txtDock);
            this.Controls.Add(this.maskTWOPhone1);
            this.Controls.Add(this.maskONEPhone1);
            this.Controls.Add(this.mackBirthDay1);
            this.Controls.Add(this.txtMail1);
            this.Controls.Add(this.txtName1);
            this.Controls.Add(this.txtLastName1);
            this.Controls.Add(this.txtAdress1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "frmAddCostemer";
            this.Text = "הוספת לקוח";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MaskedTextBox maskTWOPhone1;
        private System.Windows.Forms.MaskedTextBox maskONEPhone1;
        private System.Windows.Forms.MaskedTextBox mackBirthDay1;
        private System.Windows.Forms.TextBox txtMail1;
        private System.Windows.Forms.TextBox txtName1;
        private System.Windows.Forms.TextBox txtLastName1;
        private System.Windows.Forms.TextBox txtAdress1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox txtDock;
        private System.Windows.Forms.MaskedTextBox txtIDcostemer1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnAddDock;
    }
}