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
            this.groupBoxDetails = new System.Windows.Forms.GroupBox();
            this.lblSugPerson = new System.Windows.Forms.Label();
            this.cmbSugPerson = new System.Windows.Forms.ComboBox();
            this.lblSugMezahe = new System.Windows.Forms.Label();
            this.cmbSugMezahe = new System.Windows.Forms.ComboBox();
            this.lblId = new System.Windows.Forms.Label();
            this.txtIDcostemer1 = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.txtName1 = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtLastName1 = new System.Windows.Forms.TextBox();
            this.lblBirthDay = new System.Windows.Forms.Label();
            this.mackBirthDay1 = new System.Windows.Forms.MaskedTextBox();
            this.lblIdHint = new System.Windows.Forms.Label();
            this.groupBoxContact = new System.Windows.Forms.GroupBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.lblStreet = new System.Windows.Forms.Label();
            this.txtAdress1 = new System.Windows.Forms.TextBox();
            this.lblHouse = new System.Windows.Forms.Label();
            this.txtHouse = new System.Windows.Forms.TextBox();
            this.lblMikud = new System.Windows.Forms.Label();
            this.txtMikud = new System.Windows.Forms.TextBox();
            this.lblPhone1 = new System.Windows.Forms.Label();
            this.maskONEPhone1 = new System.Windows.Forms.TextBox();
            this.lblPhone2 = new System.Windows.Forms.Label();
            this.maskTWOPhone1 = new System.Windows.Forms.TextBox();
            this.lblMail = new System.Windows.Forms.Label();
            this.txtMail1 = new System.Windows.Forms.TextBox();
            this.lblRules = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupBoxCases = new System.Windows.Forms.GroupBox();
            this.listCases = new System.Windows.Forms.ListView();
            this.colCaseId = new System.Windows.Forms.ColumnHeader();
            this.colCaseRole = new System.Windows.Forms.ColumnHeader();
            this.colCaseSubject = new System.Windows.Forms.ColumnHeader();
            this.colCaseStatus = new System.Windows.Forms.ColumnHeader();
            this.colCaseBalance = new System.Windows.Forms.ColumnHeader();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBoxDetails.SuspendLayout();
            this.groupBoxContact.SuspendLayout();
            this.groupBoxCases.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxDetails
            // 
            this.groupBoxDetails.Controls.Add(this.lblSugPerson);
            this.groupBoxDetails.Controls.Add(this.cmbSugPerson);
            this.groupBoxDetails.Controls.Add(this.lblSugMezahe);
            this.groupBoxDetails.Controls.Add(this.cmbSugMezahe);
            this.groupBoxDetails.Controls.Add(this.lblId);
            this.groupBoxDetails.Controls.Add(this.txtIDcostemer1);
            this.groupBoxDetails.Controls.Add(this.lblFirstName);
            this.groupBoxDetails.Controls.Add(this.txtName1);
            this.groupBoxDetails.Controls.Add(this.lblLastName);
            this.groupBoxDetails.Controls.Add(this.txtLastName1);
            this.groupBoxDetails.Controls.Add(this.lblBirthDay);
            this.groupBoxDetails.Controls.Add(this.mackBirthDay1);
            this.groupBoxDetails.Controls.Add(this.lblIdHint);
            this.groupBoxDetails.Location = new System.Drawing.Point(420, 15);
            this.groupBoxDetails.Name = "groupBoxDetails";
            this.groupBoxDetails.Size = new System.Drawing.Size(380, 330);
            this.groupBoxDetails.TabIndex = 0;
            this.groupBoxDetails.Text = "זיהוי";
            this.groupBoxDetails.TabStop = false;
            // 
            // lblSugPerson
            // 
            this.lblSugPerson.Location = new System.Drawing.Point(291, 33);
            this.lblSugPerson.Name = "lblSugPerson";
            this.lblSugPerson.Size = new System.Drawing.Size(74, 23);
            this.lblSugPerson.TabIndex = 1;
            this.lblSugPerson.AutoSize = false;
            this.lblSugPerson.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSugPerson.Text = "סוג *";
            // 
            // cmbSugPerson
            // 
            this.cmbSugPerson.Location = new System.Drawing.Point(20, 30);
            this.cmbSugPerson.Name = "cmbSugPerson";
            this.cmbSugPerson.Size = new System.Drawing.Size(210, 27);
            this.cmbSugPerson.TabIndex = 2;
            this.cmbSugPerson.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSugPerson.SelectedIndexChanged += new System.EventHandler(this.Rules_Changed);
            // 
            // lblSugMezahe
            // 
            this.lblSugMezahe.Location = new System.Drawing.Point(241, 73);
            this.lblSugMezahe.Name = "lblSugMezahe";
            this.lblSugMezahe.Size = new System.Drawing.Size(124, 23);
            this.lblSugMezahe.TabIndex = 3;
            this.lblSugMezahe.AutoSize = false;
            this.lblSugMezahe.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSugMezahe.Text = "סוג מזהה *";
            // 
            // cmbSugMezahe
            // 
            this.cmbSugMezahe.Location = new System.Drawing.Point(20, 70);
            this.cmbSugMezahe.Name = "cmbSugMezahe";
            this.cmbSugMezahe.Size = new System.Drawing.Size(210, 27);
            this.cmbSugMezahe.TabIndex = 4;
            this.cmbSugMezahe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSugMezahe.SelectedIndexChanged += new System.EventHandler(this.Rules_Changed);
            // 
            // lblId
            // 
            this.lblId.Location = new System.Drawing.Point(236, 113);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(129, 23);
            this.lblId.TabIndex = 5;
            this.lblId.AutoSize = false;
            this.lblId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblId.Text = "מספר מזהה *";
            // 
            // txtIDcostemer1
            // 
            this.txtIDcostemer1.Location = new System.Drawing.Point(20, 110);
            this.txtIDcostemer1.Name = "txtIDcostemer1";
            this.txtIDcostemer1.Size = new System.Drawing.Size(210, 27);
            this.txtIDcostemer1.TabIndex = 6;
            this.txtIDcostemer1.MaxLength = 20;
            this.txtIDcostemer1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Id_KeyPress);
            // 
            // lblFirstName
            // 
            this.lblFirstName.Location = new System.Drawing.Point(251, 153);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(114, 23);
            this.lblFirstName.TabIndex = 7;
            this.lblFirstName.AutoSize = false;
            this.lblFirstName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblFirstName.Text = "שם פרטי *";
            // 
            // txtName1
            // 
            this.txtName1.Location = new System.Drawing.Point(20, 150);
            this.txtName1.Name = "txtName1";
            this.txtName1.Size = new System.Drawing.Size(210, 27);
            this.txtName1.TabIndex = 8;
            this.txtName1.MaxLength = 50;
            this.txtName1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName1_KeyPress);
            // 
            // lblLastName
            // 
            this.lblLastName.Location = new System.Drawing.Point(241, 193);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(124, 23);
            this.lblLastName.TabIndex = 9;
            this.lblLastName.AutoSize = false;
            this.lblLastName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblLastName.Text = "שם משפחה *";
            // 
            // txtLastName1
            // 
            this.txtLastName1.Location = new System.Drawing.Point(20, 190);
            this.txtLastName1.Name = "txtLastName1";
            this.txtLastName1.Size = new System.Drawing.Size(210, 27);
            this.txtLastName1.TabIndex = 10;
            this.txtLastName1.MaxLength = 25;
            this.txtLastName1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName1_KeyPress);
            // 
            // lblBirthDay
            // 
            this.lblBirthDay.Location = new System.Drawing.Point(241, 233);
            this.lblBirthDay.Name = "lblBirthDay";
            this.lblBirthDay.Size = new System.Drawing.Size(124, 23);
            this.lblBirthDay.TabIndex = 11;
            this.lblBirthDay.AutoSize = false;
            this.lblBirthDay.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblBirthDay.Text = "תאריך לידה";
            // 
            // mackBirthDay1
            // 
            this.mackBirthDay1.Location = new System.Drawing.Point(100, 230);
            this.mackBirthDay1.Name = "mackBirthDay1";
            this.mackBirthDay1.Size = new System.Drawing.Size(130, 27);
            this.mackBirthDay1.TabIndex = 12;
            this.mackBirthDay1.Mask = "00/00/0000";
            this.mackBirthDay1.ValidatingType = typeof(System.DateTime);
            // 
            // lblIdHint
            // 
            this.lblIdHint.Location = new System.Drawing.Point(301, 275);
            this.lblIdHint.Name = "lblIdHint";
            this.lblIdHint.Size = new System.Drawing.Size(64, 23);
            this.lblIdHint.TabIndex = 13;
            this.lblIdHint.AutoSize = false;
            this.lblIdHint.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblIdHint.Text = "";
            this.lblIdHint.ForeColor = System.Drawing.SystemColors.GrayText;
            // 
            // groupBoxContact
            // 
            this.groupBoxContact.Controls.Add(this.lblCity);
            this.groupBoxContact.Controls.Add(this.txtCity);
            this.groupBoxContact.Controls.Add(this.lblStreet);
            this.groupBoxContact.Controls.Add(this.txtAdress1);
            this.groupBoxContact.Controls.Add(this.lblHouse);
            this.groupBoxContact.Controls.Add(this.txtHouse);
            this.groupBoxContact.Controls.Add(this.lblMikud);
            this.groupBoxContact.Controls.Add(this.txtMikud);
            this.groupBoxContact.Controls.Add(this.lblPhone1);
            this.groupBoxContact.Controls.Add(this.maskONEPhone1);
            this.groupBoxContact.Controls.Add(this.lblPhone2);
            this.groupBoxContact.Controls.Add(this.maskTWOPhone1);
            this.groupBoxContact.Controls.Add(this.lblMail);
            this.groupBoxContact.Controls.Add(this.txtMail1);
            this.groupBoxContact.Location = new System.Drawing.Point(20, 15);
            this.groupBoxContact.Name = "groupBoxContact";
            this.groupBoxContact.Size = new System.Drawing.Size(380, 330);
            this.groupBoxContact.TabIndex = 14;
            this.groupBoxContact.Text = "כתובת ופרטי קשר";
            this.groupBoxContact.TabStop = false;
            // 
            // lblCity
            // 
            this.lblCity.Location = new System.Drawing.Point(301, 33);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(64, 23);
            this.lblCity.TabIndex = 15;
            this.lblCity.AutoSize = false;
            this.lblCity.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCity.Text = "עיר";
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(20, 30);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(210, 27);
            this.txtCity.TabIndex = 16;
            this.txtCity.MaxLength = 50;
            this.txtCity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName1_KeyPress);
            // 
            // lblStreet
            // 
            this.lblStreet.Location = new System.Drawing.Point(301, 73);
            this.lblStreet.Name = "lblStreet";
            this.lblStreet.Size = new System.Drawing.Size(64, 23);
            this.lblStreet.TabIndex = 17;
            this.lblStreet.AutoSize = false;
            this.lblStreet.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblStreet.Text = "רחוב";
            // 
            // txtAdress1
            // 
            this.txtAdress1.Location = new System.Drawing.Point(20, 70);
            this.txtAdress1.Name = "txtAdress1";
            this.txtAdress1.Size = new System.Drawing.Size(210, 27);
            this.txtAdress1.TabIndex = 18;
            this.txtAdress1.MaxLength = 50;
            // 
            // lblHouse
            // 
            this.lblHouse.Location = new System.Drawing.Point(261, 113);
            this.lblHouse.Name = "lblHouse";
            this.lblHouse.Size = new System.Drawing.Size(104, 23);
            this.lblHouse.TabIndex = 19;
            this.lblHouse.AutoSize = false;
            this.lblHouse.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblHouse.Text = "מספר בית";
            // 
            // txtHouse
            // 
            this.txtHouse.Location = new System.Drawing.Point(150, 110);
            this.txtHouse.Name = "txtHouse";
            this.txtHouse.Size = new System.Drawing.Size(80, 27);
            this.txtHouse.TabIndex = 20;
            this.txtHouse.MaxLength = 5;
            // 
            // lblMikud
            // 
            this.lblMikud.Location = new System.Drawing.Point(291, 153);
            this.lblMikud.Name = "lblMikud";
            this.lblMikud.Size = new System.Drawing.Size(74, 23);
            this.lblMikud.TabIndex = 21;
            this.lblMikud.AutoSize = false;
            this.lblMikud.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblMikud.Text = "מיקוד";
            // 
            // txtMikud
            // 
            this.txtMikud.Location = new System.Drawing.Point(110, 150);
            this.txtMikud.Name = "txtMikud";
            this.txtMikud.Size = new System.Drawing.Size(120, 27);
            this.txtMikud.TabIndex = 22;
            this.txtMikud.MaxLength = 7;
            this.txtMikud.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Digits_KeyPress);
            // 
            // lblPhone1
            // 
            this.lblPhone1.Location = new System.Drawing.Point(291, 193);
            this.lblPhone1.Name = "lblPhone1";
            this.lblPhone1.Size = new System.Drawing.Size(74, 23);
            this.lblPhone1.TabIndex = 23;
            this.lblPhone1.AutoSize = false;
            this.lblPhone1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPhone1.Text = "טלפון";
            // 
            // maskONEPhone1
            // 
            this.maskONEPhone1.Location = new System.Drawing.Point(20, 190);
            this.maskONEPhone1.Name = "maskONEPhone1";
            this.maskONEPhone1.Size = new System.Drawing.Size(210, 27);
            this.maskONEPhone1.TabIndex = 24;
            this.maskONEPhone1.MaxLength = 20;
            this.maskONEPhone1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Phone_KeyPress);
            // 
            // lblPhone2
            // 
            this.lblPhone2.Location = new System.Drawing.Point(301, 233);
            this.lblPhone2.Name = "lblPhone2";
            this.lblPhone2.Size = new System.Drawing.Size(64, 23);
            this.lblPhone2.TabIndex = 25;
            this.lblPhone2.AutoSize = false;
            this.lblPhone2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPhone2.Text = "נייד";
            // 
            // maskTWOPhone1
            // 
            this.maskTWOPhone1.Location = new System.Drawing.Point(20, 230);
            this.maskTWOPhone1.Name = "maskTWOPhone1";
            this.maskTWOPhone1.Size = new System.Drawing.Size(210, 27);
            this.maskTWOPhone1.TabIndex = 26;
            this.maskTWOPhone1.MaxLength = 20;
            this.maskTWOPhone1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Phone_KeyPress);
            // 
            // lblMail
            // 
            this.lblMail.Location = new System.Drawing.Point(301, 273);
            this.lblMail.Name = "lblMail";
            this.lblMail.Size = new System.Drawing.Size(64, 23);
            this.lblMail.TabIndex = 27;
            this.lblMail.AutoSize = false;
            this.lblMail.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblMail.Text = "מייל";
            // 
            // txtMail1
            // 
            this.txtMail1.Location = new System.Drawing.Point(20, 270);
            this.txtMail1.Name = "txtMail1";
            this.txtMail1.Size = new System.Drawing.Size(210, 27);
            this.txtMail1.TabIndex = 28;
            this.txtMail1.MaxLength = 50;
            // 
            // lblRules
            // 
            this.lblRules.Location = new System.Drawing.Point(586, 360);
            this.lblRules.Name = "lblRules";
            this.lblRules.Size = new System.Drawing.Size(64, 23);
            this.lblRules.TabIndex = 29;
            this.lblRules.AutoSize = false;
            this.lblRules.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblRules.Text = "";
            this.lblRules.ForeColor = System.Drawing.SystemColors.GrayText;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(670, 352);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(130, 40);
            this.btnAdd.TabIndex = 30;
            this.btnAdd.Text = "שמירה";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBoxCases
            // 
            this.groupBoxCases.Controls.Add(this.listCases);
            this.groupBoxCases.Location = new System.Drawing.Point(20, 405);
            this.groupBoxCases.Name = "groupBoxCases";
            this.groupBoxCases.Size = new System.Drawing.Size(780, 230);
            this.groupBoxCases.TabIndex = 31;
            this.groupBoxCases.Text = "תיקים";
            this.groupBoxCases.TabStop = false;
            // 
            // listCases
            // 
            this.listCases.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCaseId,
            this.colCaseRole,
            this.colCaseSubject,
            this.colCaseStatus,
            this.colCaseBalance});
            this.listCases.Location = new System.Drawing.Point(10, 25);
            this.listCases.Name = "listCases";
            this.listCases.Size = new System.Drawing.Size(760, 195);
            this.listCases.TabIndex = 32;
            this.listCases.FullRowSelect = true;
            this.listCases.GridLines = true;
            this.listCases.HideSelection = false;
            this.listCases.MultiSelect = false;
            this.listCases.UseCompatibleStateImageBehavior = false;
            this.listCases.View = System.Windows.Forms.View.Details;
            // 
            // colCaseId
            // 
            this.colCaseId.Text = "תיק";
            this.colCaseId.Width = 60;
            // 
            // colCaseRole
            // 
            this.colCaseRole.Text = "תפקיד בתיק";
            this.colCaseRole.Width = 130;
            // 
            // colCaseSubject
            // 
            this.colCaseSubject.Text = "נושא";
            this.colCaseSubject.Width = 180;
            // 
            // colCaseStatus
            // 
            this.colCaseStatus.Text = "שלב נוכחי";
            this.colCaseStatus.Width = 200;
            // 
            // colCaseBalance
            // 
            this.colCaseBalance.Text = "יתרה";
            this.colCaseBalance.Width = 170;
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
            this.ClientSize = new System.Drawing.Size(820, 650);
            this.Controls.Add(this.groupBoxCases);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblRules);
            this.Controls.Add(this.groupBoxContact);
            this.Controls.Add(this.groupBoxDetails);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.Name = "frmAddCostemer";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "פרטי לקוח";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBoxDetails.ResumeLayout(false);
            this.groupBoxDetails.PerformLayout();
            this.groupBoxContact.ResumeLayout(false);
            this.groupBoxContact.PerformLayout();
            this.groupBoxCases.ResumeLayout(false);
            this.groupBoxCases.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDetails;
        private System.Windows.Forms.Label lblSugPerson;
        private System.Windows.Forms.ComboBox cmbSugPerson;
        private System.Windows.Forms.Label lblSugMezahe;
        private System.Windows.Forms.ComboBox cmbSugMezahe;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.TextBox txtIDcostemer1;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.TextBox txtName1;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox txtLastName1;
        private System.Windows.Forms.Label lblBirthDay;
        private System.Windows.Forms.MaskedTextBox mackBirthDay1;
        private System.Windows.Forms.Label lblIdHint;
        private System.Windows.Forms.GroupBox groupBoxContact;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label lblStreet;
        private System.Windows.Forms.TextBox txtAdress1;
        private System.Windows.Forms.Label lblHouse;
        private System.Windows.Forms.TextBox txtHouse;
        private System.Windows.Forms.Label lblMikud;
        private System.Windows.Forms.TextBox txtMikud;
        private System.Windows.Forms.Label lblPhone1;
        private System.Windows.Forms.TextBox maskONEPhone1;
        private System.Windows.Forms.Label lblPhone2;
        private System.Windows.Forms.TextBox maskTWOPhone1;
        private System.Windows.Forms.Label lblMail;
        private System.Windows.Forms.TextBox txtMail1;
        private System.Windows.Forms.Label lblRules;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox groupBoxCases;
        private System.Windows.Forms.ListView listCases;
        private System.Windows.Forms.ColumnHeader colCaseId;
        private System.Windows.Forms.ColumnHeader colCaseRole;
        private System.Windows.Forms.ColumnHeader colCaseSubject;
        private System.Windows.Forms.ColumnHeader colCaseStatus;
        private System.Windows.Forms.ColumnHeader colCaseBalance;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
