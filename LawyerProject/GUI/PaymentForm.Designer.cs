namespace LawyerProject.GUI
{
    partial class PaymentForm
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
            this.lblCaseTitle = new System.Windows.Forms.Label();
            this.lblCase = new System.Windows.Forms.Label();
            this.lblBalanceTitle = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblKind = new System.Windows.Forms.Label();
            this.cmbKind = new System.Windows.Forms.ComboBox();
            this.lblKindNote = new System.Windows.Forms.Label();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpPaidOn = new System.Windows.Forms.DateTimePicker();
            this.lblMethod = new System.Windows.Forms.Label();
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblAfter = new System.Windows.Forms.Label();
            this.lblBalanceAfter = new System.Windows.Forms.Label();
            this.groupBoxCard = new System.Windows.Forms.GroupBox();
            this.lblLast4 = new System.Windows.Forms.Label();
            this.txtCardLast4 = new System.Windows.Forms.TextBox();
            this.lblCardNote = new System.Windows.Forms.Label();
            this.groupBoxBankTransfer = new System.Windows.Forms.GroupBox();
            this.lblReference = new System.Windows.Forms.Label();
            this.txtReference = new System.Windows.Forms.TextBox();
            this.groupBoxCheque = new System.Windows.Forms.GroupBox();
            this.lblCheque = new System.Windows.Forms.Label();
            this.txtChequeNumber = new System.Windows.Forms.TextBox();
            this.lblBank = new System.Windows.Forms.Label();
            this.txtBankNumber = new System.Windows.Forms.TextBox();
            this.lblBranch = new System.Windows.Forms.Label();
            this.txtBranchNumber = new System.Windows.Forms.TextBox();
            this.lblAccount = new System.Windows.Forms.Label();
            this.txtAccountNumber = new System.Windows.Forms.TextBox();
            this.lblChequeDate = new System.Windows.Forms.Label();
            this.dtpChequeDate = new System.Windows.Forms.DateTimePicker();
            this.btnOK = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBoxCard.SuspendLayout();
            this.groupBoxBankTransfer.SuspendLayout();
            this.groupBoxCheque.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCaseTitle
            // 
            this.lblCaseTitle.Location = new System.Drawing.Point(376, 20);
            this.lblCaseTitle.Name = "lblCaseTitle";
            this.lblCaseTitle.Size = new System.Drawing.Size(104, 23);
            this.lblCaseTitle.TabIndex = 0;
            this.lblCaseTitle.AutoSize = false;
            this.lblCaseTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCaseTitle.Text = "מספר תיק";
            // 
            // lblCase
            // 
            this.lblCase.Location = new System.Drawing.Point(80, 20);
            this.lblCase.Name = "lblCase";
            this.lblCase.Size = new System.Drawing.Size(250, 23);
            this.lblCase.TabIndex = 1;
            this.lblCase.AutoSize = false;
            this.lblCase.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCase.Text = "-";
            this.lblCase.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
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
            // cmbKind
            // 
            this.cmbKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKind.Location = new System.Drawing.Point(130, 89);
            this.cmbKind.Name = "cmbKind";
            this.cmbKind.Size = new System.Drawing.Size(200, 27);
            this.cmbKind.TabIndex = 91;
            this.cmbKind.SelectedIndexChanged += new System.EventHandler(this.cmbKind_SelectedIndexChanged);
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
            // lblAmount
            // 
            this.lblAmount.Location = new System.Drawing.Point(396, 132);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(84, 23);
            this.lblAmount.TabIndex = 4;
            this.lblAmount.AutoSize = false;
            this.lblAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblAmount.Text = "סכום *";
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(180, 129);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(150, 27);
            this.txtAmount.TabIndex = 5;
            this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Money_KeyPress);
            this.txtAmount.TextChanged += new System.EventHandler(this.txtAmount_TextChanged);
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
            // dtpPaidOn
            // 
            this.dtpPaidOn.Location = new System.Drawing.Point(130, 169);
            this.dtpPaidOn.Name = "dtpPaidOn";
            this.dtpPaidOn.Size = new System.Drawing.Size(200, 27);
            this.dtpPaidOn.TabIndex = 7;
            this.dtpPaidOn.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPaidOn.CustomFormat = "dd/MM/yyyy";
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
            // cmbMethod
            // 
            this.cmbMethod.Location = new System.Drawing.Point(130, 209);
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.Size = new System.Drawing.Size(200, 27);
            this.cmbMethod.TabIndex = 9;
            this.cmbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMethod.SelectedIndexChanged += new System.EventHandler(this.cmbMethod_SelectedIndexChanged);
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(406, 252);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(74, 23);
            this.lblDescription.TabIndex = 10;
            this.lblDescription.AutoSize = false;
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblDescription.Text = "תיאור";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(30, 249);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(300, 27);
            this.txtDescription.TabIndex = 11;
            this.txtDescription.MaxLength = 50;
            // 
            // lblAfter
            // 
            this.lblAfter.Location = new System.Drawing.Point(336, 292);
            this.lblAfter.Name = "lblAfter";
            this.lblAfter.Size = new System.Drawing.Size(144, 23);
            this.lblAfter.TabIndex = 12;
            this.lblAfter.AutoSize = false;
            this.lblAfter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblAfter.Text = "יתרה חדשה";
            // 
            // lblBalanceAfter
            // 
            this.lblBalanceAfter.Location = new System.Drawing.Point(80, 292);
            this.lblBalanceAfter.Name = "lblBalanceAfter";
            this.lblBalanceAfter.Size = new System.Drawing.Size(250, 23);
            this.lblBalanceAfter.TabIndex = 13;
            this.lblBalanceAfter.AutoSize = false;
            this.lblBalanceAfter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblBalanceAfter.Text = "-";
            // 
            // groupBoxCard
            // 
            this.groupBoxCard.Controls.Add(this.lblLast4);
            this.groupBoxCard.Controls.Add(this.txtCardLast4);
            this.groupBoxCard.Controls.Add(this.lblCardNote);
            this.groupBoxCard.Location = new System.Drawing.Point(20, 330);
            this.groupBoxCard.Name = "groupBoxCard";
            this.groupBoxCard.Size = new System.Drawing.Size(460, 110);
            this.groupBoxCard.TabIndex = 14;
            this.groupBoxCard.Text = "כרטיס אשראי";
            this.groupBoxCard.TabStop = false;
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
            // groupBoxBankTransfer
            // 
            this.groupBoxBankTransfer.Controls.Add(this.lblReference);
            this.groupBoxBankTransfer.Controls.Add(this.txtReference);
            this.groupBoxBankTransfer.Location = new System.Drawing.Point(20, 330);
            this.groupBoxBankTransfer.Name = "groupBoxBankTransfer";
            this.groupBoxBankTransfer.Size = new System.Drawing.Size(460, 110);
            this.groupBoxBankTransfer.TabIndex = 18;
            this.groupBoxBankTransfer.Text = "העברה בנקאית";
            this.groupBoxBankTransfer.TabStop = false;
            // 
            // lblReference
            // 
            this.lblReference.Location = new System.Drawing.Point(296, 38);
            this.lblReference.Name = "lblReference";
            this.lblReference.Size = new System.Drawing.Size(149, 23);
            this.lblReference.TabIndex = 19;
            this.lblReference.AutoSize = false;
            this.lblReference.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblReference.Text = "מספר אסמכתא *";
            // 
            // txtReference
            // 
            this.txtReference.Location = new System.Drawing.Point(90, 35);
            this.txtReference.Name = "txtReference";
            this.txtReference.Size = new System.Drawing.Size(200, 27);
            this.txtReference.TabIndex = 20;
            this.txtReference.MaxLength = 20;
            this.txtReference.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Digits_KeyPress);
            // 
            // groupBoxCheque
            // 
            this.groupBoxCheque.Controls.Add(this.lblCheque);
            this.groupBoxCheque.Controls.Add(this.txtChequeNumber);
            this.groupBoxCheque.Controls.Add(this.lblBank);
            this.groupBoxCheque.Controls.Add(this.txtBankNumber);
            this.groupBoxCheque.Controls.Add(this.lblBranch);
            this.groupBoxCheque.Controls.Add(this.txtBranchNumber);
            this.groupBoxCheque.Controls.Add(this.lblAccount);
            this.groupBoxCheque.Controls.Add(this.txtAccountNumber);
            this.groupBoxCheque.Controls.Add(this.lblChequeDate);
            this.groupBoxCheque.Controls.Add(this.dtpChequeDate);
            this.groupBoxCheque.Location = new System.Drawing.Point(20, 330);
            this.groupBoxCheque.Name = "groupBoxCheque";
            this.groupBoxCheque.Size = new System.Drawing.Size(460, 210);
            this.groupBoxCheque.TabIndex = 21;
            this.groupBoxCheque.Text = "צ'ק";
            this.groupBoxCheque.TabStop = false;
            // 
            // lblCheque
            // 
            this.lblCheque.Location = new System.Drawing.Point(321, 33);
            this.lblCheque.Name = "lblCheque";
            this.lblCheque.Size = new System.Drawing.Size(124, 23);
            this.lblCheque.TabIndex = 22;
            this.lblCheque.AutoSize = false;
            this.lblCheque.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCheque.Text = "מספר צ'ק *";
            // 
            // txtChequeNumber
            // 
            this.txtChequeNumber.Location = new System.Drawing.Point(90, 30);
            this.txtChequeNumber.Name = "txtChequeNumber";
            this.txtChequeNumber.Size = new System.Drawing.Size(200, 27);
            this.txtChequeNumber.TabIndex = 23;
            this.txtChequeNumber.MaxLength = 10;
            this.txtChequeNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Digits_KeyPress);
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
            // txtBankNumber
            // 
            this.txtBankNumber.Location = new System.Drawing.Point(210, 65);
            this.txtBankNumber.Name = "txtBankNumber";
            this.txtBankNumber.Size = new System.Drawing.Size(80, 27);
            this.txtBankNumber.TabIndex = 25;
            this.txtBankNumber.MaxLength = 3;
            this.txtBankNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Digits_KeyPress);
            // 
            // lblBranch
            // 
            this.lblBranch.Location = new System.Drawing.Point(311, 103);
            this.lblBranch.Name = "lblBranch";
            this.lblBranch.Size = new System.Drawing.Size(134, 23);
            this.lblBranch.TabIndex = 26;
            this.lblBranch.AutoSize = false;
            this.lblBranch.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblBranch.Text = "מספר סניף *";
            // 
            // txtBranchNumber
            // 
            this.txtBranchNumber.Location = new System.Drawing.Point(210, 100);
            this.txtBranchNumber.Name = "txtBranchNumber";
            this.txtBranchNumber.Size = new System.Drawing.Size(80, 27);
            this.txtBranchNumber.TabIndex = 27;
            this.txtBranchNumber.MaxLength = 3;
            this.txtBranchNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Digits_KeyPress);
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
            // txtAccountNumber
            // 
            this.txtAccountNumber.Location = new System.Drawing.Point(90, 135);
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Size = new System.Drawing.Size(200, 27);
            this.txtAccountNumber.TabIndex = 29;
            this.txtAccountNumber.MaxLength = 20;
            this.txtAccountNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Digits_KeyPress);
            // 
            // lblChequeDate
            // 
            this.lblChequeDate.Location = new System.Drawing.Point(301, 173);
            this.lblChequeDate.Name = "lblChequeDate";
            this.lblChequeDate.Size = new System.Drawing.Size(144, 23);
            this.lblChequeDate.TabIndex = 30;
            this.lblChequeDate.AutoSize = false;
            this.lblChequeDate.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblChequeDate.Text = "תאריך הצ'ק *";
            // 
            // dtpChequeDate
            // 
            this.dtpChequeDate.Location = new System.Drawing.Point(90, 170);
            this.dtpChequeDate.Name = "dtpChequeDate";
            this.dtpChequeDate.Size = new System.Drawing.Size(200, 27);
            this.dtpChequeDate.TabIndex = 31;
            this.dtpChequeDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpChequeDate.CustomFormat = "dd/MM/yyyy";
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
            // PaymentForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 615);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBoxCheque);
            this.Controls.Add(this.groupBoxBankTransfer);
            this.Controls.Add(this.groupBoxCard);
            this.Controls.Add(this.lblBalanceAfter);
            this.Controls.Add(this.lblAfter);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.cmbMethod);
            this.Controls.Add(this.lblMethod);
            this.Controls.Add(this.dtpPaidOn);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.lblKind);
            this.Controls.Add(this.cmbKind);
            this.Controls.Add(this.lblKindNote);
            this.Controls.Add(this.lblBalance);
            this.Controls.Add(this.lblBalanceTitle);
            this.Controls.Add(this.lblCase);
            this.Controls.Add(this.lblCaseTitle);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.Name = "PaymentForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "הוספת תשלום";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBoxCard.ResumeLayout(false);
            this.groupBoxCard.PerformLayout();
            this.groupBoxBankTransfer.ResumeLayout(false);
            this.groupBoxBankTransfer.PerformLayout();
            this.groupBoxCheque.ResumeLayout(false);
            this.groupBoxCheque.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCaseTitle;
        private System.Windows.Forms.Label lblCase;
        private System.Windows.Forms.Label lblBalanceTitle;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpPaidOn;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.ComboBox cmbMethod;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblAfter;
        private System.Windows.Forms.Label lblBalanceAfter;
        private System.Windows.Forms.GroupBox groupBoxCard;
        private System.Windows.Forms.Label lblLast4;
        private System.Windows.Forms.TextBox txtCardLast4;
        private System.Windows.Forms.Label lblCardNote;
        private System.Windows.Forms.GroupBox groupBoxBankTransfer;
        private System.Windows.Forms.Label lblReference;
        private System.Windows.Forms.TextBox txtReference;
        private System.Windows.Forms.GroupBox groupBoxCheque;
        private System.Windows.Forms.Label lblCheque;
        private System.Windows.Forms.TextBox txtChequeNumber;
        private System.Windows.Forms.Label lblBank;
        private System.Windows.Forms.TextBox txtBankNumber;
        private System.Windows.Forms.Label lblBranch;
        private System.Windows.Forms.TextBox txtBranchNumber;
        private System.Windows.Forms.Label lblAccount;
        private System.Windows.Forms.TextBox txtAccountNumber;
        private System.Windows.Forms.Label lblChequeDate;
        private System.Windows.Forms.DateTimePicker dtpChequeDate;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblKind;
        private System.Windows.Forms.ComboBox cmbKind;
        private System.Windows.Forms.Label lblKindNote;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
