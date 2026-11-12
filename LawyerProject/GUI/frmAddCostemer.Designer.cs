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
            this.lblId = new System.Windows.Forms.Label();
            this.txtIDcostemer1 = new System.Windows.Forms.MaskedTextBox();
            this.lblSugPerson = new System.Windows.Forms.Label();
            this.cmbSugPerson = new System.Windows.Forms.ComboBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.txtName1 = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtLastName1 = new System.Windows.Forms.TextBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.lblStreet = new System.Windows.Forms.Label();
            this.txtAdress1 = new System.Windows.Forms.TextBox();
            this.lblHouse = new System.Windows.Forms.Label();
            this.txtHouse = new System.Windows.Forms.TextBox();
            this.lblMikud = new System.Windows.Forms.Label();
            this.txtMikud = new System.Windows.Forms.TextBox();
            this.lblPhone1 = new System.Windows.Forms.Label();
            this.maskONEPhone1 = new System.Windows.Forms.MaskedTextBox();
            this.lblPhone2 = new System.Windows.Forms.Label();
            this.maskTWOPhone1 = new System.Windows.Forms.MaskedTextBox();
            this.lblMail = new System.Windows.Forms.Label();
            this.txtMail1 = new System.Windows.Forms.TextBox();
            this.lblBirthDay = new System.Windows.Forms.Label();
            this.mackBirthDay1 = new System.Windows.Forms.MaskedTextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            //
            // lblId
            //
            this.lblId.AutoSize = false;
            this.lblId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblId.Location = new System.Drawing.Point(329, 23);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(61, 23);
            this.lblId.TabIndex = 0;
            this.lblId.Text = "ת.ז. *";
            //
            // txtIDcostemer1
            //
            this.txtIDcostemer1.Location = new System.Drawing.Point(50, 20);
            this.txtIDcostemer1.Mask = "000000000";
            this.txtIDcostemer1.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.txtIDcostemer1.Name = "txtIDcostemer1";
            this.txtIDcostemer1.Size = new System.Drawing.Size(200, 27);
            this.txtIDcostemer1.TabIndex = 1;
            //
            // lblSugPerson
            //
            this.lblSugPerson.AutoSize = false;
            this.lblSugPerson.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSugPerson.Location = new System.Drawing.Point(337, 68);
            this.lblSugPerson.Name = "lblSugPerson";
            this.lblSugPerson.Size = new System.Drawing.Size(53, 23);
            this.lblSugPerson.TabIndex = 2;
            this.lblSugPerson.Text = "סוג *";
            //
            // cmbSugPerson
            //
            this.cmbSugPerson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSugPerson.Location = new System.Drawing.Point(50, 65);
            this.cmbSugPerson.Name = "cmbSugPerson";
            this.cmbSugPerson.Size = new System.Drawing.Size(200, 27);
            this.cmbSugPerson.TabIndex = 3;
            //
            // lblFirstName
            //
            this.lblFirstName.AutoSize = false;
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblFirstName.Location = new System.Drawing.Point(307, 113);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(83, 23);
            this.lblFirstName.TabIndex = 4;
            this.lblFirstName.Text = "שם פרטי *";
            //
            // txtName1
            //
            this.txtName1.Location = new System.Drawing.Point(50, 110);
            this.txtName1.MaxLength = 25;
            this.txtName1.Name = "txtName1";
            this.txtName1.Size = new System.Drawing.Size(200, 27);
            this.txtName1.TabIndex = 5;
            this.txtName1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName1_KeyPress);
            //
            // lblLastName
            //
            this.lblLastName.AutoSize = false;
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblLastName.Location = new System.Drawing.Point(299, 158);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(91, 23);
            this.lblLastName.TabIndex = 6;
            this.lblLastName.Text = "שם משפחה *";
            //
            // txtLastName1
            //
            this.txtLastName1.Location = new System.Drawing.Point(50, 155);
            this.txtLastName1.MaxLength = 25;
            this.txtLastName1.Name = "txtLastName1";
            this.txtLastName1.Size = new System.Drawing.Size(200, 27);
            this.txtLastName1.TabIndex = 7;
            this.txtLastName1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName1_KeyPress);
            //
            // lblCity
            //
            this.lblCity.AutoSize = false;
            this.lblCity.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCity.Location = new System.Drawing.Point(337, 203);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(53, 23);
            this.lblCity.TabIndex = 8;
            this.lblCity.Text = "עיר *";
            //
            // txtCity
            //
            this.txtCity.Location = new System.Drawing.Point(50, 200);
            this.txtCity.MaxLength = 50;
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(200, 27);
            this.txtCity.TabIndex = 9;
            this.txtCity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName1_KeyPress);
            //
            // lblStreet
            //
            this.lblStreet.AutoSize = false;
            this.lblStreet.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblStreet.Location = new System.Drawing.Point(329, 248);
            this.lblStreet.Name = "lblStreet";
            this.lblStreet.Size = new System.Drawing.Size(61, 23);
            this.lblStreet.TabIndex = 10;
            this.lblStreet.Text = "רחוב *";
            //
            // txtAdress1
            //
            this.txtAdress1.Location = new System.Drawing.Point(50, 245);
            this.txtAdress1.MaxLength = 50;
            this.txtAdress1.Name = "txtAdress1";
            this.txtAdress1.Size = new System.Drawing.Size(200, 27);
            this.txtAdress1.TabIndex = 11;
            //
            // lblHouse
            //
            this.lblHouse.AutoSize = false;
            this.lblHouse.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblHouse.Location = new System.Drawing.Point(299, 293);
            this.lblHouse.Name = "lblHouse";
            this.lblHouse.Size = new System.Drawing.Size(91, 23);
            this.lblHouse.TabIndex = 12;
            this.lblHouse.Text = "מספר בית *";
            //
            // txtHouse
            //
            this.txtHouse.Location = new System.Drawing.Point(170, 290);
            this.txtHouse.MaxLength = 5;
            this.txtHouse.Name = "txtHouse";
            this.txtHouse.Size = new System.Drawing.Size(80, 27);
            this.txtHouse.TabIndex = 13;
            //
            // lblMikud
            //
            this.lblMikud.AutoSize = false;
            this.lblMikud.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblMikud.Location = new System.Drawing.Point(337, 338);
            this.lblMikud.Name = "lblMikud";
            this.lblMikud.Size = new System.Drawing.Size(53, 23);
            this.lblMikud.TabIndex = 14;
            this.lblMikud.Text = "מיקוד";
            //
            // txtMikud
            //
            this.txtMikud.Location = new System.Drawing.Point(130, 335);
            this.txtMikud.MaxLength = 7;
            this.txtMikud.Name = "txtMikud";
            this.txtMikud.Size = new System.Drawing.Size(120, 27);
            this.txtMikud.TabIndex = 15;
            this.txtMikud.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Digits_KeyPress);
            //
            // lblPhone1
            //
            this.lblPhone1.AutoSize = false;
            this.lblPhone1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPhone1.Location = new System.Drawing.Point(322, 383);
            this.lblPhone1.Name = "lblPhone1";
            this.lblPhone1.Size = new System.Drawing.Size(68, 23);
            this.lblPhone1.TabIndex = 16;
            this.lblPhone1.Text = "טלפון *";
            //
            // maskONEPhone1
            //
            this.maskONEPhone1.Location = new System.Drawing.Point(50, 380);
            this.maskONEPhone1.MaxLength = 11;
            this.maskONEPhone1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.maskONEPhone1.Name = "maskONEPhone1";
            this.maskONEPhone1.Size = new System.Drawing.Size(200, 27);
            this.maskONEPhone1.TabIndex = 17;
            //
            // lblPhone2
            //
            this.lblPhone2.AutoSize = false;
            this.lblPhone2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPhone2.Location = new System.Drawing.Point(344, 428);
            this.lblPhone2.Name = "lblPhone2";
            this.lblPhone2.Size = new System.Drawing.Size(46, 23);
            this.lblPhone2.TabIndex = 18;
            this.lblPhone2.Text = "נייד";
            //
            // maskTWOPhone1
            //
            this.maskTWOPhone1.Location = new System.Drawing.Point(50, 425);
            this.maskTWOPhone1.MaxLength = 11;
            this.maskTWOPhone1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.maskTWOPhone1.Name = "maskTWOPhone1";
            this.maskTWOPhone1.Size = new System.Drawing.Size(200, 27);
            this.maskTWOPhone1.TabIndex = 19;
            //
            // lblMail
            //
            this.lblMail.AutoSize = false;
            this.lblMail.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblMail.Location = new System.Drawing.Point(329, 473);
            this.lblMail.Name = "lblMail";
            this.lblMail.Size = new System.Drawing.Size(61, 23);
            this.lblMail.TabIndex = 20;
            this.lblMail.Text = "מייל *";
            //
            // txtMail1
            //
            this.txtMail1.Location = new System.Drawing.Point(50, 470);
            this.txtMail1.MaxLength = 50;
            this.txtMail1.Name = "txtMail1";
            this.txtMail1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtMail1.Size = new System.Drawing.Size(200, 27);
            this.txtMail1.TabIndex = 21;
            //
            // lblBirthDay
            //
            this.lblBirthDay.AutoSize = false;
            this.lblBirthDay.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblBirthDay.Location = new System.Drawing.Point(299, 518);
            this.lblBirthDay.Name = "lblBirthDay";
            this.lblBirthDay.Size = new System.Drawing.Size(91, 23);
            this.lblBirthDay.TabIndex = 22;
            this.lblBirthDay.Text = "תאריך לידה";
            //
            // mackBirthDay1
            //
            this.mackBirthDay1.Location = new System.Drawing.Point(50, 515);
            this.mackBirthDay1.Mask = "00/00/0000";
            this.mackBirthDay1.Name = "mackBirthDay1";
            this.mackBirthDay1.Size = new System.Drawing.Size(200, 27);
            this.mackBirthDay1.TabIndex = 23;
            //
            // btnAdd
            //
            this.btnAdd.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.btnAdd.Location = new System.Drawing.Point(130, 565);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 40);
            this.btnAdd.TabIndex = 24;
            this.btnAdd.Text = "שמירה";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnOK_Click);
            //
            // errorProvider1
            //
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.RightToLeft = true;
            //
            // frmAddCostemer
            //
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 625);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.mackBirthDay1);
            this.Controls.Add(this.lblBirthDay);
            this.Controls.Add(this.txtMail1);
            this.Controls.Add(this.lblMail);
            this.Controls.Add(this.maskTWOPhone1);
            this.Controls.Add(this.lblPhone2);
            this.Controls.Add(this.maskONEPhone1);
            this.Controls.Add(this.lblPhone1);
            this.Controls.Add(this.txtMikud);
            this.Controls.Add(this.lblMikud);
            this.Controls.Add(this.txtHouse);
            this.Controls.Add(this.lblHouse);
            this.Controls.Add(this.txtAdress1);
            this.Controls.Add(this.lblStreet);
            this.Controls.Add(this.txtCity);
            this.Controls.Add(this.lblCity);
            this.Controls.Add(this.txtLastName1);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.txtName1);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.cmbSugPerson);
            this.Controls.Add(this.lblSugPerson);
            this.Controls.Add(this.txtIDcostemer1);
            this.Controls.Add(this.lblId);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmAddCostemer";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "פרטי לקוח";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.MaskedTextBox txtIDcostemer1;
        private System.Windows.Forms.Label lblSugPerson;
        private System.Windows.Forms.ComboBox cmbSugPerson;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.TextBox txtName1;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox txtLastName1;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label lblStreet;
        private System.Windows.Forms.TextBox txtAdress1;
        private System.Windows.Forms.Label lblHouse;
        private System.Windows.Forms.TextBox txtHouse;
        private System.Windows.Forms.Label lblMikud;
        private System.Windows.Forms.TextBox txtMikud;
        private System.Windows.Forms.Label lblPhone1;
        private System.Windows.Forms.MaskedTextBox maskONEPhone1;
        private System.Windows.Forms.Label lblPhone2;
        private System.Windows.Forms.MaskedTextBox maskTWOPhone1;
        private System.Windows.Forms.Label lblMail;
        private System.Windows.Forms.TextBox txtMail1;
        private System.Windows.Forms.Label lblBirthDay;
        private System.Windows.Forms.MaskedTextBox mackBirthDay1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
