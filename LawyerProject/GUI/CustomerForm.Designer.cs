namespace LawyerProject.GUI
{
    partial class CustomerForm
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
            this.lblPersonType = new System.Windows.Forms.Label();
            this.cmbPersonType = new System.Windows.Forms.ComboBox();
            this.lblIdType = new System.Windows.Forms.Label();
            this.cmbIdType = new System.Windows.Forms.ComboBox();
            this.lblId = new System.Windows.Forms.Label();
            this.txtIdNumber = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblBirthDay = new System.Windows.Forms.Label();
            this.mtbBirthDate = new System.Windows.Forms.MaskedTextBox();
            this.lblIdHint = new System.Windows.Forms.Label();
            this.groupBoxContact = new System.Windows.Forms.GroupBox();
            this.lblCity = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.lblStreet = new System.Windows.Forms.Label();
            this.txtStreet = new System.Windows.Forms.TextBox();
            this.lblHouseNumber = new System.Windows.Forms.Label();
            this.txtHouseNumber = new System.Windows.Forms.TextBox();
            this.lblPostalCode = new System.Windows.Forms.Label();
            this.txtPostalCode = new System.Windows.Forms.TextBox();
            this.lblPhone1 = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblPhone2 = new System.Windows.Forms.Label();
            this.txtMobilePhone = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
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
            this.groupBoxDetails.Controls.Add(this.lblPersonType);
            this.groupBoxDetails.Controls.Add(this.cmbPersonType);
            this.groupBoxDetails.Controls.Add(this.lblIdType);
            this.groupBoxDetails.Controls.Add(this.cmbIdType);
            this.groupBoxDetails.Controls.Add(this.lblId);
            this.groupBoxDetails.Controls.Add(this.txtIdNumber);
            this.groupBoxDetails.Controls.Add(this.lblFirstName);
            this.groupBoxDetails.Controls.Add(this.txtFirstName);
            this.groupBoxDetails.Controls.Add(this.lblLastName);
            this.groupBoxDetails.Controls.Add(this.txtLastName);
            this.groupBoxDetails.Controls.Add(this.lblBirthDay);
            this.groupBoxDetails.Controls.Add(this.mtbBirthDate);
            this.groupBoxDetails.Controls.Add(this.lblIdHint);
            this.groupBoxDetails.Location = new System.Drawing.Point(420, 15);
            this.groupBoxDetails.Name = "groupBoxDetails";
            this.groupBoxDetails.Size = new System.Drawing.Size(380, 330);
            this.groupBoxDetails.TabIndex = 0;
            this.groupBoxDetails.Text = "זיהוי";
            this.groupBoxDetails.TabStop = false;
            // 
            // lblPersonType
            // 
            this.lblPersonType.Location = new System.Drawing.Point(291, 33);
            this.lblPersonType.Name = "lblPersonType";
            this.lblPersonType.Size = new System.Drawing.Size(74, 23);
            this.lblPersonType.TabIndex = 1;
            this.lblPersonType.AutoSize = false;
            this.lblPersonType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPersonType.Text = "סוג *";
            // 
            // cmbPersonType
            // 
            this.cmbPersonType.Location = new System.Drawing.Point(20, 30);
            this.cmbPersonType.Name = "cmbPersonType";
            this.cmbPersonType.Size = new System.Drawing.Size(210, 27);
            this.cmbPersonType.TabIndex = 2;
            this.cmbPersonType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPersonType.SelectedIndexChanged += new System.EventHandler(this.Rules_Changed);
            // 
            // lblIdType
            // 
            this.lblIdType.Location = new System.Drawing.Point(241, 73);
            this.lblIdType.Name = "lblIdType";
            this.lblIdType.Size = new System.Drawing.Size(124, 23);
            this.lblIdType.TabIndex = 3;
            this.lblIdType.AutoSize = false;
            this.lblIdType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblIdType.Text = "סוג מזהה *";
            // 
            // cmbIdType
            // 
            this.cmbIdType.Location = new System.Drawing.Point(20, 70);
            this.cmbIdType.Name = "cmbIdType";
            this.cmbIdType.Size = new System.Drawing.Size(210, 27);
            this.cmbIdType.TabIndex = 4;
            this.cmbIdType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIdType.SelectedIndexChanged += new System.EventHandler(this.Rules_Changed);
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
            // txtIdNumber
            // 
            this.txtIdNumber.Location = new System.Drawing.Point(20, 110);
            this.txtIdNumber.Name = "txtIdNumber";
            this.txtIdNumber.Size = new System.Drawing.Size(210, 27);
            this.txtIdNumber.TabIndex = 6;
            this.txtIdNumber.MaxLength = 20;
            this.txtIdNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Id_KeyPress);
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
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(20, 150);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(210, 27);
            this.txtFirstName.TabIndex = 8;
            this.txtFirstName.MaxLength = 50;
            this.txtFirstName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFirstName_KeyPress);
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
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(20, 190);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(210, 27);
            this.txtLastName.TabIndex = 10;
            this.txtLastName.MaxLength = 25;
            this.txtLastName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFirstName_KeyPress);
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
            // mtbBirthDate
            // 
            this.mtbBirthDate.Location = new System.Drawing.Point(100, 230);
            this.mtbBirthDate.Name = "mtbBirthDate";
            this.mtbBirthDate.Size = new System.Drawing.Size(130, 27);
            this.mtbBirthDate.TabIndex = 12;
            this.mtbBirthDate.Mask = "00/00/0000";
            this.mtbBirthDate.ValidatingType = typeof(System.DateTime);
            // 
            // lblIdHint
            // 
            this.lblIdHint.Location = new System.Drawing.Point(301, 275);
            this.lblIdHint.Name = "lblIdHint";
            this.lblIdHint.Size = new System.Drawing.Size(200, 23);
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
            this.groupBoxContact.Controls.Add(this.txtStreet);
            this.groupBoxContact.Controls.Add(this.lblHouseNumber);
            this.groupBoxContact.Controls.Add(this.txtHouseNumber);
            this.groupBoxContact.Controls.Add(this.lblPostalCode);
            this.groupBoxContact.Controls.Add(this.txtPostalCode);
            this.groupBoxContact.Controls.Add(this.lblPhone1);
            this.groupBoxContact.Controls.Add(this.txtPhone);
            this.groupBoxContact.Controls.Add(this.lblPhone2);
            this.groupBoxContact.Controls.Add(this.txtMobilePhone);
            this.groupBoxContact.Controls.Add(this.lblEmail);
            this.groupBoxContact.Controls.Add(this.txtEmail);
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
            this.txtCity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFirstName_KeyPress);
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
            // txtStreet
            // 
            this.txtStreet.Location = new System.Drawing.Point(20, 70);
            this.txtStreet.Name = "txtStreet";
            this.txtStreet.Size = new System.Drawing.Size(210, 27);
            this.txtStreet.TabIndex = 18;
            this.txtStreet.MaxLength = 50;
            // 
            // lblHouseNumber
            // 
            this.lblHouseNumber.Location = new System.Drawing.Point(261, 113);
            this.lblHouseNumber.Name = "lblHouseNumber";
            this.lblHouseNumber.Size = new System.Drawing.Size(104, 23);
            this.lblHouseNumber.TabIndex = 19;
            this.lblHouseNumber.AutoSize = false;
            this.lblHouseNumber.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblHouseNumber.Text = "מספר בית";
            // 
            // txtHouseNumber
            // 
            this.txtHouseNumber.Location = new System.Drawing.Point(150, 110);
            this.txtHouseNumber.Name = "txtHouseNumber";
            this.txtHouseNumber.Size = new System.Drawing.Size(80, 27);
            this.txtHouseNumber.TabIndex = 20;
            this.txtHouseNumber.MaxLength = 5;
            // 
            // lblPostalCode
            // 
            this.lblPostalCode.Location = new System.Drawing.Point(291, 153);
            this.lblPostalCode.Name = "lblPostalCode";
            this.lblPostalCode.Size = new System.Drawing.Size(74, 23);
            this.lblPostalCode.TabIndex = 21;
            this.lblPostalCode.AutoSize = false;
            this.lblPostalCode.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPostalCode.Text = "מיקוד";
            // 
            // txtPostalCode
            // 
            this.txtPostalCode.Location = new System.Drawing.Point(110, 150);
            this.txtPostalCode.Name = "txtPostalCode";
            this.txtPostalCode.Size = new System.Drawing.Size(120, 27);
            this.txtPostalCode.TabIndex = 22;
            this.txtPostalCode.MaxLength = 7;
            this.txtPostalCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Digits_KeyPress);
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
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(20, 190);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(210, 27);
            this.txtPhone.TabIndex = 24;
            this.txtPhone.MaxLength = 20;
            this.txtPhone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Phone_KeyPress);
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
            // txtMobilePhone
            // 
            this.txtMobilePhone.Location = new System.Drawing.Point(20, 230);
            this.txtMobilePhone.Name = "txtMobilePhone";
            this.txtMobilePhone.Size = new System.Drawing.Size(210, 27);
            this.txtMobilePhone.TabIndex = 26;
            this.txtMobilePhone.MaxLength = 20;
            this.txtMobilePhone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Phone_KeyPress);
            // 
            // lblEmail
            // 
            this.lblEmail.Location = new System.Drawing.Point(301, 273);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(64, 23);
            this.lblEmail.TabIndex = 27;
            this.lblEmail.AutoSize = false;
            this.lblEmail.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblEmail.Text = "מייל";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(20, 270);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(210, 27);
            this.txtEmail.TabIndex = 28;
            this.txtEmail.MaxLength = 50;
            // 
            // lblRules
            // 
            this.lblRules.Location = new System.Drawing.Point(20, 360);
            this.lblRules.Name = "lblRules";
            this.lblRules.Size = new System.Drawing.Size(630, 23);
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
            // CustomerForm
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
            this.Name = "CustomerForm";
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
        private System.Windows.Forms.Label lblPersonType;
        private System.Windows.Forms.ComboBox cmbPersonType;
        private System.Windows.Forms.Label lblIdType;
        private System.Windows.Forms.ComboBox cmbIdType;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.TextBox txtIdNumber;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Label lblBirthDay;
        private System.Windows.Forms.MaskedTextBox mtbBirthDate;
        private System.Windows.Forms.Label lblIdHint;
        private System.Windows.Forms.GroupBox groupBoxContact;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label lblStreet;
        private System.Windows.Forms.TextBox txtStreet;
        private System.Windows.Forms.Label lblHouseNumber;
        private System.Windows.Forms.TextBox txtHouseNumber;
        private System.Windows.Forms.Label lblPostalCode;
        private System.Windows.Forms.TextBox txtPostalCode;
        private System.Windows.Forms.Label lblPhone1;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblPhone2;
        private System.Windows.Forms.TextBox txtMobilePhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
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
