namespace LawyerProject.GUI
{
    partial class frmAddTashlumimLetik
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
            this.lblTikTitle = new System.Windows.Forms.Label();
            this.lblTik = new System.Windows.Forms.Label();
            this.lblBalanceTitle = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblSchum = new System.Windows.Forms.Label();
            this.lblKind = new System.Windows.Forms.Label();
            this.cmbSugTashlum = new System.Windows.Forms.ComboBox();
            this.lblKindNote = new System.Windows.Forms.Label();
            this.txtSchum = new System.Windows.Forms.TextBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.dateTimePickerTashlum = new System.Windows.Forms.DateTimePicker();
            this.lblMethod = new System.Windows.Forms.Label();
            this.cmbEmzhaeyTashlum = new System.Windows.Forms.ComboBox();
            this.lblTeur = new System.Windows.Forms.Label();
            this.txtTeur = new System.Windows.Forms.TextBox();
            this.lblAfter = new System.Windows.Forms.Label();
            this.lblItraAfter = new System.Windows.Forms.Label();
            this.groupBoxCartisAshray = new System.Windows.Forms.GroupBox();
            this.lblLast4 = new System.Windows.Forms.Label();
            this.txtCardLast4 = new System.Windows.Forms.TextBox();
            this.lblCardNote = new System.Windows.Forms.Label();
            this.groupBoxAavaraBankait = new System.Windows.Forms.GroupBox();
            this.lblAsmachta = new System.Windows.Forms.Label();
            this.txtNumAsmachta = new System.Windows.Forms.TextBox();
            this.groupBoxChek = new System.Windows.Forms.GroupBox();
            this.lblChek = new System.Windows.Forms.Label();
            this.txtnumchek = new System.Windows.Forms.TextBox();
            this.lblBank = new System.Windows.Forms.Label();
            this.txtNumBank = new System.Windows.Forms.TextBox();
            this.lblSnif = new System.Windows.Forms.Label();
            this.txtNumSnif = new System.Windows.Forms.TextBox();
            this.lblAccount = new System.Windows.Forms.Label();
            this.txtnumaccount = new System.Windows.Forms.TextBox();
            this.lblChekDate = new System.Windows.Forms.Label();
            this.dateTimePickerDateChek = new System.Windows.Forms.DateTimePicker();
            this.btnOK = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBoxCartisAshray.SuspendLayout();
            this.groupBoxAavaraBankait.SuspendLayout();
            this.groupBoxChek.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTikTitle
            // 
            this.lblTikTitle.Location = new System.Drawing.Point(376, 20);
            this.lblTikTitle.Name = "lblTikTitle";
            this.lblTikTitle.Size = new System.Drawing.Size(104, 23);
            this.lblTikTitle.TabIndex = 0;
            this.lblTikTitle.AutoSize = false;
            this.lblTikTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblTikTitle.Text = "מספר תיק";
            // 
            // lblTik
            // 
            this.lblTik.Location = new System.Drawing.Point(80, 20);
            this.lblTik.Name = "lblTik";
            this.lblTik.Size = new System.Drawing.Size(250, 23);
            this.lblTik.TabIndex = 1;
            this.lblTik.AutoSize = false;
            this.lblTik.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblTik.Text = "-";
            this.lblTik.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            // 
            // lblBalanceTitle
            // 
            this.lblBalanceTitle.Location = new System.Drawing.Point(346, 52);
            this.lblBalanceTitle.Name = "lblBalanceTitle";
            this.lblBalanceTitle.Size = new System.Drawing.Size(134, 23);
            this.lblBalanceTitle.TabIndex = 2;
            this.lblBalanceTitle.AutoSize = false;
            this.lblBalanceTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblBalanceTitle.Text = "יתרה לתשלום";
            // 
            // lblBalance
            // 
            this.lblBalance.Location = new System.Drawing.Point(80, 52);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(250, 23);
            this.lblBalance.TabIndex = 3;
            this.lblBalance.AutoSize = false;
            this.lblBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblBalance.Text = "-";
            this.lblBalance.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            // 
            // lblKind
            // 
            this.lblKind.Location = new System.Drawing.Point(336, 92);
            this.lblKind.Name = "lblKind";
            this.lblKind.Size = new System.Drawing.Size(144, 23);
            this.lblKind.TabIndex = 90;
            this.lblKind.AutoSize = false;
            this.lblKind.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblKind.Text = "סוג תשלום *";
            // 
            // cmbSugTashlum
            // 
            this.cmbSugTashlum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSugTashlum.Location = new System.Drawing.Point(130, 89);
            this.cmbSugTashlum.Name = "cmbSugTashlum";
            this.cmbSugTashlum.Size = new System.Drawing.Size(200, 27);
            this.cmbSugTashlum.TabIndex = 91;
            this.cmbSugTashlum.SelectedIndexChanged += new System.EventHandler(this.cmbSugTashlum_SelectedIndexChanged);
            // 
            // lblKindNote
            // 
            this.lblKindNote.Location = new System.Drawing.Point(20, 92);
            this.lblKindNote.Name = "lblKindNote";
            this.lblKindNote.Size = new System.Drawing.Size(105, 23);
            this.lblKindNote.TabIndex = 92;
            this.lblKindNote.AutoSize = false;
            this.lblKindNote.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblKindNote.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblKindNote.Text = "";
            // 
            // lblSchum
            // 
            this.lblSchum.Location = new System.Drawing.Point(396, 132);
            this.lblSchum.Name = "lblSchum";
            this.lblSchum.Size = new System.Drawing.Size(84, 23);
            this.lblSchum.TabIndex = 4;
            this.lblSchum.AutoSize = false;
            this.lblSchum.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSchum.Text = "סכום *";
            // 
            // txtSchum
            // 
            this.txtSchum.Location = new System.Drawing.Point(180, 129);
            this.txtSchum.Name = "txtSchum";
            this.txtSchum.Size = new System.Drawing.Size(150, 27);
            this.txtSchum.TabIndex = 5;
            this.txtSchum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Money_KeyPress);
            this.txtSchum.TextChanged += new System.EventHandler(this.txtSchum_TextChanged);
            // 
            // lblDate
            // 
            this.lblDate.Location = new System.Drawing.Point(336, 172);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(144, 23);
            this.lblDate.TabIndex = 6;
            this.lblDate.AutoSize = false;
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblDate.Text = "תאריך תשלום *";
            // 
            // dateTimePickerTashlum
            // 
            this.dateTimePickerTashlum.Location = new System.Drawing.Point(130, 169);
            this.dateTimePickerTashlum.Name = "dateTimePickerTashlum";
            this.dateTimePickerTashlum.Size = new System.Drawing.Size(200, 27);
            this.dateTimePickerTashlum.TabIndex = 7;
            this.dateTimePickerTashlum.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            // 
            // lblMethod
            // 
            this.lblMethod.Location = new System.Drawing.Point(336, 212);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(144, 23);
            this.lblMethod.TabIndex = 8;
            this.lblMethod.AutoSize = false;
            this.lblMethod.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblMethod.Text = "אמצעי תשלום *";
            // 
            // cmbEmzhaeyTashlum
            // 
            this.cmbEmzhaeyTashlum.Location = new System.Drawing.Point(130, 209);
            this.cmbEmzhaeyTashlum.Name = "cmbEmzhaeyTashlum";
            this.cmbEmzhaeyTashlum.Size = new System.Drawing.Size(200, 27);
            this.cmbEmzhaeyTashlum.TabIndex = 9;
            this.cmbEmzhaeyTashlum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEmzhaeyTashlum.SelectedIndexChanged += new System.EventHandler(this.cmbEmzhaeyTashlum_SelectedIndexChanged);
            // 
            // lblTeur
            // 
            this.lblTeur.Location = new System.Drawing.Point(406, 252);
            this.lblTeur.Name = "lblTeur";
            this.lblTeur.Size = new System.Drawing.Size(74, 23);
            this.lblTeur.TabIndex = 10;
            this.lblTeur.AutoSize = false;
            this.lblTeur.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblTeur.Text = "תיאור";
            // 
            // txtTeur
            // 
            this.txtTeur.Location = new System.Drawing.Point(30, 249);
            this.txtTeur.Name = "txtTeur";
            this.txtTeur.Size = new System.Drawing.Size(300, 27);
            this.txtTeur.TabIndex = 11;
            this.txtTeur.MaxLength = 50;
            // 
            // lblAfter
            // 
            this.lblAfter.Location = new System.Drawing.Point(336, 292);
            this.lblAfter.Name = "lblAfter";
            this.lblAfter.Size = new System.Drawing.Size(144, 23);
            this.lblAfter.TabIndex = 12;
            this.lblAfter.AutoSize = false;
            this.lblAfter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblAfter.Text = "יתרה אחרי התשלום";
            // 
            // lblItraAfter
            // 
            this.lblItraAfter.Location = new System.Drawing.Point(80, 292);
            this.lblItraAfter.Name = "lblItraAfter";
            this.lblItraAfter.Size = new System.Drawing.Size(250, 23);
            this.lblItraAfter.TabIndex = 13;
            this.lblItraAfter.AutoSize = false;
            this.lblItraAfter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblItraAfter.Text = "-";
            // 
            // groupBoxCartisAshray
            // 
            this.groupBoxCartisAshray.Controls.Add(this.lblLast4);
            this.groupBoxCartisAshray.Controls.Add(this.txtCardLast4);
            this.groupBoxCartisAshray.Controls.Add(this.lblCardNote);
            this.groupBoxCartisAshray.Location = new System.Drawing.Point(20, 330);
            this.groupBoxCartisAshray.Name = "groupBoxCartisAshray";
            this.groupBoxCartisAshray.Size = new System.Drawing.Size(460, 110);
            this.groupBoxCartisAshray.TabIndex = 14;
            this.groupBoxCartisAshray.Text = "כרטיס אשראי";
            this.groupBoxCartisAshray.TabStop = false;
            // 
            // lblLast4
            // 
            this.lblLast4.Location = new System.Drawing.Point(296, 38);
            this.lblLast4.Name = "lblLast4";
            this.lblLast4.Size = new System.Drawing.Size(149, 23);
            this.lblLast4.TabIndex = 15;
            this.lblLast4.AutoSize = false;
            this.lblLast4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblLast4.Text = "4 ספרות אחרונות *";
            // 
            // txtCardLast4
            // 
            this.txtCardLast4.Location = new System.Drawing.Point(210, 35);
            this.txtCardLast4.Name = "txtCardLast4";
            this.txtCardLast4.Size = new System.Drawing.Size(80, 27);
            this.txtCardLast4.TabIndex = 16;
            this.txtCardLast4.MaxLength = 4;
            this.txtCardLast4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Digits_KeyPress);
            // 
            // lblCardNote
            // 
            this.lblCardNote.Location = new System.Drawing.Point(3, 72);
            this.lblCardNote.Name = "lblCardNote";
            this.lblCardNote.Size = new System.Drawing.Size(442, 23);
            this.lblCardNote.TabIndex = 17;
            this.lblCardNote.AutoSize = false;
            this.lblCardNote.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCardNote.Text = "נשמרות רק 4 הספרות האחרונות (ללא תוקף ו-CVV).";
            this.lblCardNote.ForeColor = System.Drawing.SystemColors.GrayText;
            // 
            // groupBoxAavaraBankait
            // 
            this.groupBoxAavaraBankait.Controls.Add(this.lblAsmachta);
            this.groupBoxAavaraBankait.Controls.Add(this.txtNumAsmachta);
            this.groupBoxAavaraBankait.Location = new System.Drawing.Point(20, 330);
            this.groupBoxAavaraBankait.Name = "groupBoxAavaraBankait";
            this.groupBoxAavaraBankait.Size = new System.Drawing.Size(460, 110);
            this.groupBoxAavaraBankait.TabIndex = 18;
            this.groupBoxAavaraBankait.Text = "העברה בנקאית";
            this.groupBoxAavaraBankait.TabStop = false;
            // 
            // lblAsmachta
            // 
            this.lblAsmachta.Location = new System.Drawing.Point(296, 38);
            this.lblAsmachta.Name = "lblAsmachta";
            this.lblAsmachta.Size = new System.Drawing.Size(149, 23);
            this.lblAsmachta.TabIndex = 19;
            this.lblAsmachta.AutoSize = false;
            this.lblAsmachta.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblAsmachta.Text = "מספר אסמכתא *";
            // 
            // txtNumAsmachta
            // 
            this.txtNumAsmachta.Location = new System.Drawing.Point(90, 35);
            this.txtNumAsmachta.Name = "txtNumAsmachta";
            this.txtNumAsmachta.Size = new System.Drawing.Size(200, 27);
            this.txtNumAsmachta.TabIndex = 20;
            this.txtNumAsmachta.MaxLength = 20;
            this.txtNumAsmachta.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Digits_KeyPress);
            // 
            // groupBoxChek
            // 
            this.groupBoxChek.Controls.Add(this.lblChek);
            this.groupBoxChek.Controls.Add(this.txtnumchek);
            this.groupBoxChek.Controls.Add(this.lblBank);
            this.groupBoxChek.Controls.Add(this.txtNumBank);
            this.groupBoxChek.Controls.Add(this.lblSnif);
            this.groupBoxChek.Controls.Add(this.txtNumSnif);
            this.groupBoxChek.Controls.Add(this.lblAccount);
            this.groupBoxChek.Controls.Add(this.txtnumaccount);
            this.groupBoxChek.Controls.Add(this.lblChekDate);
            this.groupBoxChek.Controls.Add(this.dateTimePickerDateChek);
            this.groupBoxChek.Location = new System.Drawing.Point(20, 330);
            this.groupBoxChek.Name = "groupBoxChek";
            this.groupBoxChek.Size = new System.Drawing.Size(460, 210);
            this.groupBoxChek.TabIndex = 21;
            this.groupBoxChek.Text = "צ'ק";
            this.groupBoxChek.TabStop = false;
            // 
            // lblChek
            // 
            this.lblChek.Location = new System.Drawing.Point(321, 33);
            this.lblChek.Name = "lblChek";
            this.lblChek.Size = new System.Drawing.Size(124, 23);
            this.lblChek.TabIndex = 22;
            this.lblChek.AutoSize = false;
            this.lblChek.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblChek.Text = "מספר צ'ק *";
            // 
            // txtnumchek
            // 
            this.txtnumchek.Location = new System.Drawing.Point(90, 30);
            this.txtnumchek.Name = "txtnumchek";
            this.txtnumchek.Size = new System.Drawing.Size(200, 27);
            this.txtnumchek.TabIndex = 23;
            this.txtnumchek.MaxLength = 10;
            this.txtnumchek.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Digits_KeyPress);
            // 
            // lblBank
            // 
            this.lblBank.Location = new System.Drawing.Point(321, 68);
            this.lblBank.Name = "lblBank";
            this.lblBank.Size = new System.Drawing.Size(124, 23);
            this.lblBank.TabIndex = 24;
            this.lblBank.AutoSize = false;
            this.lblBank.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblBank.Text = "מספר בנק *";
            // 
            // txtNumBank
            // 
            this.txtNumBank.Location = new System.Drawing.Point(210, 65);
            this.txtNumBank.Name = "txtNumBank";
            this.txtNumBank.Size = new System.Drawing.Size(80, 27);
            this.txtNumBank.TabIndex = 25;
            this.txtNumBank.MaxLength = 3;
            this.txtNumBank.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Digits_KeyPress);
            // 
            // lblSnif
            // 
            this.lblSnif.Location = new System.Drawing.Point(311, 103);
            this.lblSnif.Name = "lblSnif";
            this.lblSnif.Size = new System.Drawing.Size(134, 23);
            this.lblSnif.TabIndex = 26;
            this.lblSnif.AutoSize = false;
            this.lblSnif.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSnif.Text = "מספר סניף *";
            // 
            // txtNumSnif
            // 
            this.txtNumSnif.Location = new System.Drawing.Point(210, 100);
            this.txtNumSnif.Name = "txtNumSnif";
            this.txtNumSnif.Size = new System.Drawing.Size(80, 27);
            this.txtNumSnif.TabIndex = 27;
            this.txtNumSnif.MaxLength = 3;
            this.txtNumSnif.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Digits_KeyPress);
            // 
            // lblAccount
            // 
            this.lblAccount.Location = new System.Drawing.Point(301, 138);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(144, 23);
            this.lblAccount.TabIndex = 28;
            this.lblAccount.AutoSize = false;
            this.lblAccount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblAccount.Text = "מספר חשבון *";
            // 
            // txtnumaccount
            // 
            this.txtnumaccount.Location = new System.Drawing.Point(90, 135);
            this.txtnumaccount.Name = "txtnumaccount";
            this.txtnumaccount.Size = new System.Drawing.Size(200, 27);
            this.txtnumaccount.TabIndex = 29;
            this.txtnumaccount.MaxLength = 20;
            this.txtnumaccount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Digits_KeyPress);
            // 
            // lblChekDate
            // 
            this.lblChekDate.Location = new System.Drawing.Point(301, 173);
            this.lblChekDate.Name = "lblChekDate";
            this.lblChekDate.Size = new System.Drawing.Size(144, 23);
            this.lblChekDate.TabIndex = 30;
            this.lblChekDate.AutoSize = false;
            this.lblChekDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblChekDate.Text = "תאריך הצ'ק *";
            // 
            // dateTimePickerDateChek
            // 
            this.dateTimePickerDateChek.Location = new System.Drawing.Point(90, 170);
            this.dateTimePickerDateChek.Name = "dateTimePickerDateChek";
            this.dateTimePickerDateChek.Size = new System.Drawing.Size(200, 27);
            this.dateTimePickerDateChek.TabIndex = 31;
            this.dateTimePickerDateChek.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(330, 555);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(150, 40);
            this.btnOK.TabIndex = 32;
            this.btnOK.Text = "שמירת תשלום";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.RightToLeft = true;
            // 
            // frmAddTashlumimLetik
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 615);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBoxChek);
            this.Controls.Add(this.groupBoxAavaraBankait);
            this.Controls.Add(this.groupBoxCartisAshray);
            this.Controls.Add(this.lblItraAfter);
            this.Controls.Add(this.lblAfter);
            this.Controls.Add(this.txtTeur);
            this.Controls.Add(this.lblTeur);
            this.Controls.Add(this.cmbEmzhaeyTashlum);
            this.Controls.Add(this.lblMethod);
            this.Controls.Add(this.dateTimePickerTashlum);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.txtSchum);
            this.Controls.Add(this.lblSchum);
            this.Controls.Add(this.lblKind);
            this.Controls.Add(this.cmbSugTashlum);
            this.Controls.Add(this.lblKindNote);
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.lblBalanceTitle);
            this.Controls.Add(this.lblTik);
            this.Controls.Add(this.lblTikTitle);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.Name = "frmAddTashlumimLetik";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "הוספת תשלום";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBoxCartisAshray.ResumeLayout(false);
            this.groupBoxCartisAshray.PerformLayout();
            this.groupBoxAavaraBankait.ResumeLayout(false);
            this.groupBoxAavaraBankait.PerformLayout();
            this.groupBoxChek.ResumeLayout(false);
            this.groupBoxChek.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTikTitle;
        private System.Windows.Forms.Label lblTik;
        private System.Windows.Forms.Label lblBalanceTitle;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblSchum;
        private System.Windows.Forms.TextBox txtSchum;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerTashlum;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.ComboBox cmbEmzhaeyTashlum;
        private System.Windows.Forms.Label lblTeur;
        private System.Windows.Forms.TextBox txtTeur;
        private System.Windows.Forms.Label lblAfter;
        private System.Windows.Forms.Label lblItraAfter;
        private System.Windows.Forms.GroupBox groupBoxCartisAshray;
        private System.Windows.Forms.Label lblLast4;
        private System.Windows.Forms.TextBox txtCardLast4;
        private System.Windows.Forms.Label lblCardNote;
        private System.Windows.Forms.GroupBox groupBoxAavaraBankait;
        private System.Windows.Forms.Label lblAsmachta;
        private System.Windows.Forms.TextBox txtNumAsmachta;
        private System.Windows.Forms.GroupBox groupBoxChek;
        private System.Windows.Forms.Label lblChek;
        private System.Windows.Forms.TextBox txtnumchek;
        private System.Windows.Forms.Label lblBank;
        private System.Windows.Forms.TextBox txtNumBank;
        private System.Windows.Forms.Label lblSnif;
        private System.Windows.Forms.TextBox txtNumSnif;
        private System.Windows.Forms.Label lblAccount;
        private System.Windows.Forms.TextBox txtnumaccount;
        private System.Windows.Forms.Label lblChekDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerDateChek;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblKind;
        private System.Windows.Forms.ComboBox cmbSugTashlum;
        private System.Windows.Forms.Label lblKindNote;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
