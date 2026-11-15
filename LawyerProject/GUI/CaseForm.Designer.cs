namespace LawyerProject.GUI
{
    partial class CaseForm
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
            this.lblCaseIdTitle = new System.Windows.Forms.Label();
            this.lblCaseId = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.cmbClient = new System.Windows.Forms.ComboBox();
            this.btnAddClient = new System.Windows.Forms.Button();
            this.lblCaseType = new System.Windows.Forms.Label();
            this.cmbCaseType = new System.Windows.Forms.ComboBox();
            this.lblCaseTypeLock = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.rbCourt = new System.Windows.Forms.RadioButton();
            this.rbDeal = new System.Windows.Forms.RadioButton();
            this.lblPowerOfAttorney = new System.Windows.Forms.Label();
            this.chkPowerOfAttorney = new System.Windows.Forms.CheckBox();
            this.dtpPowerOfAttorney = new System.Windows.Forms.DateTimePicker();
            this.lblStatusTitle = new System.Windows.Forms.Label();
            this.lblCaseStatus = new System.Windows.Forms.Label();
            this.groupBoxOpposing = new System.Windows.Forms.GroupBox();
            this.lblOpposingParty = new System.Windows.Forms.Label();
            this.cmbOpposingParty = new System.Windows.Forms.ComboBox();
            this.btnAddOpposingParty = new System.Windows.Forms.Button();
            this.lblOpposingLawyer = new System.Windows.Forms.Label();
            this.cmbOpposingLawyer = new System.Windows.Forms.ComboBox();
            this.btnAddLawyer = new System.Windows.Forms.Button();
            this.lblOpposingHint = new System.Windows.Forms.Label();
            this.lblOpposingDetails = new System.Windows.Forms.Label();
            this.groupBoxCourt = new System.Windows.Forms.GroupBox();
            this.lblCourt = new System.Windows.Forms.Label();
            this.cmbCourt = new System.Windows.Forms.ComboBox();
            this.lblClaimAmount = new System.Windows.Forms.Label();
            this.txtClaimAmount = new System.Windows.Forms.TextBox();
            this.lblAwardedAmount = new System.Windows.Forms.Label();
            this.txtAwardedAmount = new System.Windows.Forms.TextBox();
            this.groupBoxDeal = new System.Windows.Forms.GroupBox();
            this.lblPropertyAddress = new System.Windows.Forms.Label();
            this.txtPropertyAddress = new System.Windows.Forms.TextBox();
            this.lblBlock = new System.Windows.Forms.Label();
            this.txtBlock = new System.Windows.Forms.TextBox();
            this.lblParcel = new System.Windows.Forms.Label();
            this.txtParcel = new System.Windows.Forms.TextBox();
            this.lblSubParcel = new System.Windows.Forms.Label();
            this.txtSubParcel = new System.Windows.Forms.TextBox();
            this.lblPropertyValue = new System.Windows.Forms.Label();
            this.txtPropertyValue = new System.Windows.Forms.TextBox();
            this.groupBoxMoney = new System.Windows.Forms.GroupBox();
            this.lblFeePercent = new System.Windows.Forms.Label();
            this.txtFeePercent = new System.Windows.Forms.TextBox();
            this.lblAdvance = new System.Windows.Forms.Label();
            this.txtAdvance = new System.Windows.Forms.TextBox();
            this.lblAdvanceStatus = new System.Windows.Forms.Label();
            this.lblTotalTitle = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblPaidTitle = new System.Windows.Forms.Label();
            this.lblPaid = new System.Windows.Forms.Label();
            this.lblBalanceTitle = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.lblPaidStatus = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDeleteCase = new System.Windows.Forms.Button();
            this.lblSaveHint = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSteps = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.colStepNo = new System.Windows.Forms.ColumnHeader();
            this.colStepName = new System.Windows.Forms.ColumnHeader();
            this.colEstimated = new System.Windows.Forms.ColumnHeader();
            this.colDone = new System.Windows.Forms.ColumnHeader();
            this.colStepStatus = new System.Windows.Forms.ColumnHeader();
            this.colLate = new System.Windows.Forms.ColumnHeader();
            this.colNote = new System.Windows.Forms.ColumnHeader();
            this.btnCompleteStep = new System.Windows.Forms.Button();
            this.btnReschedule = new System.Windows.Forms.Button();
            this.btnStepNote = new System.Windows.Forms.Button();
            this.btnUndoStep = new System.Windows.Forms.Button();
            this.lblStepsHint = new System.Windows.Forms.Label();
            this.tabPayments = new System.Windows.Forms.TabPage();
            this.listView2 = new System.Windows.Forms.ListView();
            this.colPayId = new System.Windows.Forms.ColumnHeader();
            this.colPayDate = new System.Windows.Forms.ColumnHeader();
            this.colPayKind = new System.Windows.Forms.ColumnHeader();
            this.colPayMethod = new System.Windows.Forms.ColumnHeader();
            this.colPayAmount = new System.Windows.Forms.ColumnHeader();
            this.colPayBalance = new System.Windows.Forms.ColumnHeader();
            this.colPayStatus = new System.Windows.Forms.ColumnHeader();
            this.colPayDesc = new System.Windows.Forms.ColumnHeader();
            this.btnAddPayment = new System.Windows.Forms.Button();
            this.btnReceipt = new System.Windows.Forms.Button();
            this.btnCancelPayment = new System.Windows.Forms.Button();
            this.btnRefund = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.groupBoxDetails.SuspendLayout();
            this.groupBoxOpposing.SuspendLayout();
            this.groupBoxCourt.SuspendLayout();
            this.groupBoxDeal.SuspendLayout();
            this.groupBoxMoney.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabSteps.SuspendLayout();
            this.tabPayments.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxDetails
            // 
            this.groupBoxDetails.Controls.Add(this.lblCaseIdTitle);
            this.groupBoxDetails.Controls.Add(this.lblCaseId);
            this.groupBoxDetails.Controls.Add(this.lblCustomer);
            this.groupBoxDetails.Controls.Add(this.cmbClient);
            this.groupBoxDetails.Controls.Add(this.btnAddClient);
            this.groupBoxDetails.Controls.Add(this.lblCaseType);
            this.groupBoxDetails.Controls.Add(this.cmbCaseType);
            this.groupBoxDetails.Controls.Add(this.lblCaseTypeLock);
            this.groupBoxDetails.Controls.Add(this.lblType);
            this.groupBoxDetails.Controls.Add(this.rbCourt);
            this.groupBoxDetails.Controls.Add(this.rbDeal);
            this.groupBoxDetails.Controls.Add(this.lblPowerOfAttorney);
            this.groupBoxDetails.Controls.Add(this.chkPowerOfAttorney);
            this.groupBoxDetails.Controls.Add(this.dtpPowerOfAttorney);
            this.groupBoxDetails.Controls.Add(this.lblStatusTitle);
            this.groupBoxDetails.Controls.Add(this.lblCaseStatus);
            this.groupBoxDetails.Location = new System.Drawing.Point(510, 15);
            this.groupBoxDetails.Name = "groupBoxDetails";
            this.groupBoxDetails.Size = new System.Drawing.Size(470, 265);
            this.groupBoxDetails.TabIndex = 0;
            this.groupBoxDetails.Text = "פרטי תיק";
            this.groupBoxDetails.TabStop = false;
            // 
            // lblCaseIdTitle
            // 
            this.lblCaseIdTitle.Location = new System.Drawing.Point(351, 33);
            this.lblCaseIdTitle.Name = "lblCaseIdTitle";
            this.lblCaseIdTitle.Size = new System.Drawing.Size(104, 23);
            this.lblCaseIdTitle.TabIndex = 1;
            this.lblCaseIdTitle.AutoSize = false;
            this.lblCaseIdTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCaseIdTitle.Text = "מספר תיק";
            // 
            // lblCaseId
            // 
            this.lblCaseId.Location = new System.Drawing.Point(246, 33);
            this.lblCaseId.Name = "lblCaseId";
            this.lblCaseId.Size = new System.Drawing.Size(74, 23);
            this.lblCaseId.TabIndex = 2;
            this.lblCaseId.AutoSize = false;
            this.lblCaseId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCaseId.Text = "(חדש)";
            this.lblCaseId.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            // 
            // lblCustomer
            // 
            this.lblCustomer.Location = new System.Drawing.Point(371, 70);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(84, 23);
            this.lblCustomer.TabIndex = 3;
            this.lblCustomer.AutoSize = false;
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCustomer.Text = "לקוח *";
            // 
            // cmbClient
            // 
            this.cmbClient.Location = new System.Drawing.Point(120, 67);
            this.cmbClient.Name = "cmbClient";
            this.cmbClient.Size = new System.Drawing.Size(200, 27);
            this.cmbClient.TabIndex = 4;
            this.cmbClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            // 
            // btnAddClient
            // 
            this.btnAddClient.Location = new System.Drawing.Point(15, 64);
            this.btnAddClient.Name = "btnAddClient";
            this.btnAddClient.Size = new System.Drawing.Size(95, 34);
            this.btnAddClient.TabIndex = 5;
            this.btnAddClient.Text = "לקוח חדש";
            this.btnAddClient.UseVisualStyleBackColor = true;
            this.btnAddClient.Click += new System.EventHandler(this.btnAddClient_Click);
            // 
            // lblCaseType
            // 
            this.lblCaseType.Location = new System.Drawing.Point(331, 107);
            this.lblCaseType.Name = "lblCaseType";
            this.lblCaseType.Size = new System.Drawing.Size(124, 23);
            this.lblCaseType.TabIndex = 6;
            this.lblCaseType.AutoSize = false;
            this.lblCaseType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCaseType.Text = "נושא תיק *";
            // 
            // cmbCaseType
            // 
            this.cmbCaseType.Location = new System.Drawing.Point(120, 104);
            this.cmbCaseType.Name = "cmbCaseType";
            this.cmbCaseType.Size = new System.Drawing.Size(200, 27);
            this.cmbCaseType.TabIndex = 7;
            this.cmbCaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            // 
            // lblCaseTypeLock
            // 
            this.lblCaseTypeLock.Location = new System.Drawing.Point(3, 107);
            this.lblCaseTypeLock.Name = "lblCaseTypeLock";
            this.lblCaseTypeLock.Size = new System.Drawing.Size(107, 23);
            this.lblCaseTypeLock.TabIndex = 8;
            this.lblCaseTypeLock.AutoSize = false;
            this.lblCaseTypeLock.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCaseTypeLock.Text = "נקבע בפתיחה";
            this.lblCaseTypeLock.ForeColor = System.Drawing.SystemColors.GrayText;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(391, 144);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(64, 23);
            this.lblType.TabIndex = 9;
            this.lblType.AutoSize = false;
            this.lblType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblType.Text = "סוג";
            // 
            // rbCourt
            // 
            this.rbCourt.Location = new System.Drawing.Point(180, 142);
            this.rbCourt.Name = "rbCourt";
            this.rbCourt.Size = new System.Drawing.Size(140, 23);
            this.rbCourt.TabIndex = 10;
            this.rbCourt.AutoSize = true;
            this.rbCourt.Text = "תיק בבית משפט";
            this.rbCourt.UseVisualStyleBackColor = true;
            this.rbCourt.Checked = true;
            this.rbCourt.CheckedChanged += new System.EventHandler(this.CaseType_CheckedChanged);
            // 
            // rbDeal
            // 
            this.rbDeal.Location = new System.Drawing.Point(30, 142);
            this.rbDeal.Name = "rbDeal";
            this.rbDeal.Size = new System.Drawing.Size(140, 23);
            this.rbDeal.TabIndex = 11;
            this.rbDeal.AutoSize = true;
            this.rbDeal.Text = "עסקת נכס";
            this.rbDeal.UseVisualStyleBackColor = true;
            this.rbDeal.CheckedChanged += new System.EventHandler(this.CaseType_CheckedChanged);
            // 
            // lblPowerOfAttorney
            // 
            this.lblPowerOfAttorney.Location = new System.Drawing.Point(341, 181);
            this.lblPowerOfAttorney.Name = "lblPowerOfAttorney";
            this.lblPowerOfAttorney.Size = new System.Drawing.Size(114, 23);
            this.lblPowerOfAttorney.TabIndex = 12;
            this.lblPowerOfAttorney.AutoSize = false;
            this.lblPowerOfAttorney.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPowerOfAttorney.Text = "ייפוי כוח";
            // 
            // chkPowerOfAttorney
            // 
            this.chkPowerOfAttorney.Location = new System.Drawing.Point(202, 180);
            this.chkPowerOfAttorney.Name = "chkPowerOfAttorney";
            this.chkPowerOfAttorney.Size = new System.Drawing.Size(118, 23);
            this.chkPowerOfAttorney.TabIndex = 13;
            this.chkPowerOfAttorney.AutoSize = false;
            this.chkPowerOfAttorney.Text = "נחתם בתאריך";
            this.chkPowerOfAttorney.UseVisualStyleBackColor = true;
            this.chkPowerOfAttorney.CheckedChanged += new System.EventHandler(this.chkPowerOfAttorney_CheckedChanged);
            // 
            // dtpPowerOfAttorney
            // 
            this.dtpPowerOfAttorney.Location = new System.Drawing.Point(65, 178);
            this.dtpPowerOfAttorney.Name = "dtpPowerOfAttorney";
            this.dtpPowerOfAttorney.Size = new System.Drawing.Size(130, 27);
            this.dtpPowerOfAttorney.TabIndex = 14;
            this.dtpPowerOfAttorney.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPowerOfAttorney.CustomFormat = "dd/MM/yyyy";
            this.dtpPowerOfAttorney.Enabled = false;
            // 
            // lblStatusTitle
            // 
            this.lblStatusTitle.Location = new System.Drawing.Point(341, 218);
            this.lblStatusTitle.Name = "lblStatusTitle";
            this.lblStatusTitle.Size = new System.Drawing.Size(114, 23);
            this.lblStatusTitle.TabIndex = 15;
            this.lblStatusTitle.AutoSize = false;
            this.lblStatusTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblStatusTitle.Text = "שלב נוכחי";
            // 
            // lblCaseStatus
            // 
            this.lblCaseStatus.Location = new System.Drawing.Point(256, 218);
            this.lblCaseStatus.Name = "lblCaseStatus";
            this.lblCaseStatus.Size = new System.Drawing.Size(64, 23);
            this.lblCaseStatus.TabIndex = 16;
            this.lblCaseStatus.AutoSize = false;
            this.lblCaseStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCaseStatus.Text = "-";
            // 
            // groupBoxOpposing
            // 
            this.groupBoxOpposing.Controls.Add(this.lblOpposingParty);
            this.groupBoxOpposing.Controls.Add(this.cmbOpposingParty);
            this.groupBoxOpposing.Controls.Add(this.btnAddOpposingParty);
            this.groupBoxOpposing.Controls.Add(this.lblOpposingLawyer);
            this.groupBoxOpposing.Controls.Add(this.cmbOpposingLawyer);
            this.groupBoxOpposing.Controls.Add(this.btnAddLawyer);
            this.groupBoxOpposing.Controls.Add(this.lblOpposingHint);
            this.groupBoxOpposing.Controls.Add(this.lblOpposingDetails);
            this.groupBoxOpposing.Location = new System.Drawing.Point(20, 15);
            this.groupBoxOpposing.Name = "groupBoxOpposing";
            this.groupBoxOpposing.Size = new System.Drawing.Size(470, 265);
            this.groupBoxOpposing.TabIndex = 17;
            this.groupBoxOpposing.Text = "הצד שכנגד";
            this.groupBoxOpposing.TabStop = false;
            // 
            // lblOpposingParty
            // 
            this.lblOpposingParty.Location = new System.Drawing.Point(326, 40);
            this.lblOpposingParty.Name = "lblOpposingParty";
            this.lblOpposingParty.Size = new System.Drawing.Size(129, 23);
            this.lblOpposingParty.TabIndex = 18;
            this.lblOpposingParty.AutoSize = false;
            this.lblOpposingParty.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblOpposingParty.Text = "לקוח נגדי *";
            // 
            // cmbOpposingParty
            // 
            this.cmbOpposingParty.Location = new System.Drawing.Point(120, 37);
            this.cmbOpposingParty.Name = "cmbOpposingParty";
            this.cmbOpposingParty.Size = new System.Drawing.Size(200, 27);
            this.cmbOpposingParty.TabIndex = 19;
            this.cmbOpposingParty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            // 
            // btnAddOpposingParty
            // 
            this.btnAddOpposingParty.Location = new System.Drawing.Point(15, 34);
            this.btnAddOpposingParty.Name = "btnAddOpposingParty";
            this.btnAddOpposingParty.Size = new System.Drawing.Size(95, 34);
            this.btnAddOpposingParty.TabIndex = 20;
            this.btnAddOpposingParty.Text = "חדש";
            this.btnAddOpposingParty.UseVisualStyleBackColor = true;
            this.btnAddOpposingParty.Click += new System.EventHandler(this.btnAddOpposingParty_Click);
            // 
            // lblOpposingLawyer
            // 
            this.lblOpposingLawyer.Location = new System.Drawing.Point(326, 85);
            this.lblOpposingLawyer.Name = "lblOpposingLawyer";
            this.lblOpposingLawyer.Size = new System.Drawing.Size(129, 23);
            this.lblOpposingLawyer.TabIndex = 21;
            this.lblOpposingLawyer.AutoSize = false;
            this.lblOpposingLawyer.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblOpposingLawyer.Text = "עו\"ד נגדי *";
            // 
            // cmbOpposingLawyer
            // 
            this.cmbOpposingLawyer.Location = new System.Drawing.Point(120, 82);
            this.cmbOpposingLawyer.Name = "cmbOpposingLawyer";
            this.cmbOpposingLawyer.Size = new System.Drawing.Size(200, 27);
            this.cmbOpposingLawyer.TabIndex = 22;
            this.cmbOpposingLawyer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            // 
            // btnAddLawyer
            // 
            this.btnAddLawyer.Location = new System.Drawing.Point(15, 79);
            this.btnAddLawyer.Name = "btnAddLawyer";
            this.btnAddLawyer.Size = new System.Drawing.Size(95, 34);
            this.btnAddLawyer.TabIndex = 23;
            this.btnAddLawyer.Text = "חדש";
            this.btnAddLawyer.UseVisualStyleBackColor = true;
            this.btnAddLawyer.Click += new System.EventHandler(this.btnAddLawyer_Click);
            // 
            // lblOpposingHint
            // 
            this.lblOpposingHint.Location = new System.Drawing.Point(3, 130);
            this.lblOpposingHint.Name = "lblOpposingHint";
            this.lblOpposingHint.Size = new System.Drawing.Size(452, 23);
            this.lblOpposingHint.TabIndex = 24;
            this.lblOpposingHint.AutoSize = false;
            this.lblOpposingHint.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblOpposingHint.Text = "ניתן לבחור כל אדם או חברה. פרטי זיהוי אינם חובה.";
            this.lblOpposingHint.ForeColor = System.Drawing.SystemColors.GrayText;
            // 
            // lblOpposingDetails
            // 
            this.lblOpposingDetails.Location = new System.Drawing.Point(391, 170);
            this.lblOpposingDetails.Name = "lblOpposingDetails";
            this.lblOpposingDetails.Size = new System.Drawing.Size(64, 23);
            this.lblOpposingDetails.TabIndex = 25;
            this.lblOpposingDetails.AutoSize = false;
            this.lblOpposingDetails.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblOpposingDetails.Text = "";
            // 
            // groupBoxCourt
            // 
            this.groupBoxCourt.Controls.Add(this.lblCourt);
            this.groupBoxCourt.Controls.Add(this.cmbCourt);
            this.groupBoxCourt.Controls.Add(this.lblClaimAmount);
            this.groupBoxCourt.Controls.Add(this.txtClaimAmount);
            this.groupBoxCourt.Controls.Add(this.lblAwardedAmount);
            this.groupBoxCourt.Controls.Add(this.txtAwardedAmount);
            this.groupBoxCourt.Location = new System.Drawing.Point(510, 290);
            this.groupBoxCourt.Name = "groupBoxCourt";
            this.groupBoxCourt.Size = new System.Drawing.Size(470, 230);
            this.groupBoxCourt.TabIndex = 26;
            this.groupBoxCourt.Text = "פרטי בית משפט";
            this.groupBoxCourt.TabStop = false;
            // 
            // lblCourt
            // 
            this.lblCourt.Location = new System.Drawing.Point(331, 40);
            this.lblCourt.Name = "lblCourt";
            this.lblCourt.Size = new System.Drawing.Size(124, 23);
            this.lblCourt.TabIndex = 27;
            this.lblCourt.AutoSize = false;
            this.lblCourt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCourt.Text = "בית משפט *";
            // 
            // cmbCourt
            // 
            this.cmbCourt.Location = new System.Drawing.Point(120, 37);
            this.cmbCourt.Name = "cmbCourt";
            this.cmbCourt.Size = new System.Drawing.Size(200, 27);
            this.cmbCourt.TabIndex = 28;
            this.cmbCourt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            // 
            // lblClaimAmount
            // 
            this.lblClaimAmount.Location = new System.Drawing.Point(326, 85);
            this.lblClaimAmount.Name = "lblClaimAmount";
            this.lblClaimAmount.Size = new System.Drawing.Size(129, 23);
            this.lblClaimAmount.TabIndex = 29;
            this.lblClaimAmount.AutoSize = false;
            this.lblClaimAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblClaimAmount.Text = "סכום התביעה *";
            // 
            // txtClaimAmount
            // 
            this.txtClaimAmount.Location = new System.Drawing.Point(120, 82);
            this.txtClaimAmount.Name = "txtClaimAmount";
            this.txtClaimAmount.Size = new System.Drawing.Size(200, 27);
            this.txtClaimAmount.TabIndex = 30;
            this.txtClaimAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Money_KeyPress);
            this.txtClaimAmount.TextChanged += new System.EventHandler(this.Money_TextChanged);
            // 
            // lblAwardedAmount
            // 
            this.lblAwardedAmount.Location = new System.Drawing.Point(331, 130);
            this.lblAwardedAmount.Name = "lblAwardedAmount";
            this.lblAwardedAmount.Size = new System.Drawing.Size(124, 23);
            this.lblAwardedAmount.TabIndex = 31;
            this.lblAwardedAmount.AutoSize = false;
            this.lblAwardedAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblAwardedAmount.Text = "סכום שנפסק";
            // 
            // txtAwardedAmount
            // 
            this.txtAwardedAmount.Location = new System.Drawing.Point(120, 127);
            this.txtAwardedAmount.Name = "txtAwardedAmount";
            this.txtAwardedAmount.Size = new System.Drawing.Size(200, 27);
            this.txtAwardedAmount.TabIndex = 32;
            this.txtAwardedAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Money_KeyPress);
            // 
            // groupBoxDeal
            // 
            this.groupBoxDeal.Controls.Add(this.lblPropertyAddress);
            this.groupBoxDeal.Controls.Add(this.txtPropertyAddress);
            this.groupBoxDeal.Controls.Add(this.lblBlock);
            this.groupBoxDeal.Controls.Add(this.txtBlock);
            this.groupBoxDeal.Controls.Add(this.lblParcel);
            this.groupBoxDeal.Controls.Add(this.txtParcel);
            this.groupBoxDeal.Controls.Add(this.lblSubParcel);
            this.groupBoxDeal.Controls.Add(this.txtSubParcel);
            this.groupBoxDeal.Controls.Add(this.lblPropertyValue);
            this.groupBoxDeal.Controls.Add(this.txtPropertyValue);
            this.groupBoxDeal.Location = new System.Drawing.Point(510, 290);
            this.groupBoxDeal.Name = "groupBoxDeal";
            this.groupBoxDeal.Size = new System.Drawing.Size(470, 230);
            this.groupBoxDeal.TabIndex = 33;
            this.groupBoxDeal.Text = "פרטי עסקה";
            this.groupBoxDeal.TabStop = false;
            // 
            // lblPropertyAddress
            // 
            this.lblPropertyAddress.Location = new System.Drawing.Point(326, 35);
            this.lblPropertyAddress.Name = "lblPropertyAddress";
            this.lblPropertyAddress.Size = new System.Drawing.Size(129, 23);
            this.lblPropertyAddress.TabIndex = 34;
            this.lblPropertyAddress.AutoSize = false;
            this.lblPropertyAddress.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPropertyAddress.Text = "כתובת הנכס *";
            // 
            // txtPropertyAddress
            // 
            this.txtPropertyAddress.Location = new System.Drawing.Point(120, 32);
            this.txtPropertyAddress.Name = "txtPropertyAddress";
            this.txtPropertyAddress.Size = new System.Drawing.Size(200, 27);
            this.txtPropertyAddress.TabIndex = 35;
            this.txtPropertyAddress.MaxLength = 50;
            // 
            // lblBlock
            // 
            this.lblBlock.Location = new System.Drawing.Point(381, 75);
            this.lblBlock.Name = "lblBlock";
            this.lblBlock.Size = new System.Drawing.Size(74, 23);
            this.lblBlock.TabIndex = 36;
            this.lblBlock.AutoSize = false;
            this.lblBlock.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblBlock.Text = "גוש *";
            // 
            // txtBlock
            // 
            this.txtBlock.Location = new System.Drawing.Point(200, 72);
            this.txtBlock.Name = "txtBlock";
            this.txtBlock.Size = new System.Drawing.Size(120, 27);
            this.txtBlock.TabIndex = 37;
            this.txtBlock.MaxLength = 50;
            this.txtBlock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Registry_KeyPress);
            // 
            // lblParcel
            // 
            this.lblParcel.Location = new System.Drawing.Point(371, 115);
            this.lblParcel.Name = "lblParcel";
            this.lblParcel.Size = new System.Drawing.Size(84, 23);
            this.lblParcel.TabIndex = 38;
            this.lblParcel.AutoSize = false;
            this.lblParcel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblParcel.Text = "חלקה *";
            // 
            // txtParcel
            // 
            this.txtParcel.Location = new System.Drawing.Point(200, 112);
            this.txtParcel.Name = "txtParcel";
            this.txtParcel.Size = new System.Drawing.Size(120, 27);
            this.txtParcel.TabIndex = 39;
            this.txtParcel.MaxLength = 50;
            this.txtParcel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Registry_KeyPress);
            // 
            // lblSubParcel
            // 
            this.lblSubParcel.Location = new System.Drawing.Point(361, 155);
            this.lblSubParcel.Name = "lblSubParcel";
            this.lblSubParcel.Size = new System.Drawing.Size(94, 23);
            this.lblSubParcel.TabIndex = 40;
            this.lblSubParcel.AutoSize = false;
            this.lblSubParcel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSubParcel.Text = "תת חלקה";
            // 
            // txtSubParcel
            // 
            this.txtSubParcel.Location = new System.Drawing.Point(200, 152);
            this.txtSubParcel.Name = "txtSubParcel";
            this.txtSubParcel.Size = new System.Drawing.Size(120, 27);
            this.txtSubParcel.TabIndex = 41;
            this.txtSubParcel.MaxLength = 50;
            this.txtSubParcel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Registry_KeyPress);
            // 
            // lblPropertyValue
            // 
            this.lblPropertyValue.Location = new System.Drawing.Point(326, 195);
            this.lblPropertyValue.Name = "lblPropertyValue";
            this.lblPropertyValue.Size = new System.Drawing.Size(129, 23);
            this.lblPropertyValue.TabIndex = 42;
            this.lblPropertyValue.AutoSize = false;
            this.lblPropertyValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPropertyValue.Text = "שווי הנכס *";
            // 
            // txtPropertyValue
            // 
            this.txtPropertyValue.Location = new System.Drawing.Point(120, 192);
            this.txtPropertyValue.Name = "txtPropertyValue";
            this.txtPropertyValue.Size = new System.Drawing.Size(200, 27);
            this.txtPropertyValue.TabIndex = 43;
            this.txtPropertyValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Money_KeyPress);
            this.txtPropertyValue.TextChanged += new System.EventHandler(this.Money_TextChanged);
            // 
            // groupBoxMoney
            // 
            this.groupBoxMoney.Controls.Add(this.lblFeePercent);
            this.groupBoxMoney.Controls.Add(this.txtFeePercent);
            this.groupBoxMoney.Controls.Add(this.lblAdvance);
            this.groupBoxMoney.Controls.Add(this.txtAdvance);
            this.groupBoxMoney.Controls.Add(this.lblAdvanceStatus);
            this.groupBoxMoney.Controls.Add(this.lblTotalTitle);
            this.groupBoxMoney.Controls.Add(this.lblTotal);
            this.groupBoxMoney.Controls.Add(this.lblPaidTitle);
            this.groupBoxMoney.Controls.Add(this.lblPaid);
            this.groupBoxMoney.Controls.Add(this.lblBalanceTitle);
            this.groupBoxMoney.Controls.Add(this.lblBalance);
            this.groupBoxMoney.Controls.Add(this.lblPaidStatus);
            this.groupBoxMoney.Location = new System.Drawing.Point(20, 290);
            this.groupBoxMoney.Name = "groupBoxMoney";
            this.groupBoxMoney.Size = new System.Drawing.Size(470, 230);
            this.groupBoxMoney.TabIndex = 44;
            this.groupBoxMoney.Text = "שכר טרחה ותשלומים";
            this.groupBoxMoney.TabStop = false;
            // 
            // lblFeePercent
            // 
            this.lblFeePercent.Location = new System.Drawing.Point(326, 40);
            this.lblFeePercent.Name = "lblFeePercent";
            this.lblFeePercent.Size = new System.Drawing.Size(129, 23);
            this.lblFeePercent.TabIndex = 45;
            this.lblFeePercent.AutoSize = false;
            this.lblFeePercent.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblFeePercent.Text = "אחוז שכר טרחה *";
            // 
            // txtFeePercent
            // 
            this.txtFeePercent.Location = new System.Drawing.Point(240, 37);
            this.txtFeePercent.Name = "txtFeePercent";
            this.txtFeePercent.Size = new System.Drawing.Size(80, 27);
            this.txtFeePercent.TabIndex = 46;
            this.txtFeePercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Money_KeyPress);
            this.txtFeePercent.TextChanged += new System.EventHandler(this.Money_TextChanged);
            // 
            // lblAdvance
            // 
            this.lblAdvance.Location = new System.Drawing.Point(381, 77);
            this.lblAdvance.Name = "lblAdvance";
            this.lblAdvance.Size = new System.Drawing.Size(74, 23);
            this.lblAdvance.TabIndex = 47;
            this.lblAdvance.AutoSize = false;
            this.lblAdvance.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblAdvance.Text = "מקדמה";
            // 
            // txtAdvance
            // 
            this.txtAdvance.Location = new System.Drawing.Point(200, 74);
            this.txtAdvance.Name = "txtAdvance";
            this.txtAdvance.Size = new System.Drawing.Size(120, 27);
            this.txtAdvance.TabIndex = 48;
            this.txtAdvance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Money_KeyPress);
            this.txtAdvance.TextChanged += new System.EventHandler(this.Money_TextChanged);
            // 
            // lblAdvanceStatus
            // 
            this.lblAdvanceStatus.Location = new System.Drawing.Point(126, 77);
            this.lblAdvanceStatus.Name = "lblAdvanceStatus";
            this.lblAdvanceStatus.Size = new System.Drawing.Size(64, 23);
            this.lblAdvanceStatus.TabIndex = 49;
            this.lblAdvanceStatus.AutoSize = false;
            this.lblAdvanceStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblAdvanceStatus.Text = "-";
            // 
            // lblTotalTitle
            // 
            this.lblTotalTitle.Location = new System.Drawing.Point(331, 112);
            this.lblTotalTitle.Name = "lblTotalTitle";
            this.lblTotalTitle.Size = new System.Drawing.Size(124, 23);
            this.lblTotalTitle.TabIndex = 50;
            this.lblTotalTitle.AutoSize = false;
            this.lblTotalTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblTotalTitle.Text = "סה\"כ לתיק";
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(136, 112);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(184, 23);
            this.lblTotal.TabIndex = 51;
            this.lblTotal.AutoSize = false;
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblTotal.Text = "-";
            // 
            // lblPaidTitle
            // 
            this.lblPaidTitle.Location = new System.Drawing.Point(331, 142);
            this.lblPaidTitle.Name = "lblPaidTitle";
            this.lblPaidTitle.Size = new System.Drawing.Size(124, 23);
            this.lblPaidTitle.TabIndex = 52;
            this.lblPaidTitle.AutoSize = false;
            this.lblPaidTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPaidTitle.Text = "שולם עד כה";
            // 
            // lblPaid
            // 
            this.lblPaid.Location = new System.Drawing.Point(136, 142);
            this.lblPaid.Name = "lblPaid";
            this.lblPaid.Size = new System.Drawing.Size(184, 23);
            this.lblPaid.TabIndex = 53;
            this.lblPaid.AutoSize = false;
            this.lblPaid.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPaid.Text = "-";
            // 
            // lblBalanceTitle
            // 
            this.lblBalanceTitle.Location = new System.Drawing.Point(326, 172);
            this.lblBalanceTitle.Name = "lblBalanceTitle";
            this.lblBalanceTitle.Size = new System.Drawing.Size(129, 23);
            this.lblBalanceTitle.TabIndex = 54;
            this.lblBalanceTitle.AutoSize = false;
            this.lblBalanceTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblBalanceTitle.Text = "יתרה לתשלום";
            // 
            // lblBalance
            // 
            this.lblBalance.Location = new System.Drawing.Point(136, 172);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(184, 23);
            this.lblBalance.TabIndex = 55;
            this.lblBalance.AutoSize = false;
            this.lblBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblBalance.Text = "-";
            this.lblBalance.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            // 
            // lblPaidStatus
            // 
            this.lblPaidStatus.Location = new System.Drawing.Point(21, 202);
            this.lblPaidStatus.Name = "lblPaidStatus";
            this.lblPaidStatus.Size = new System.Drawing.Size(434, 23);
            this.lblPaidStatus.TabIndex = 56;
            this.lblPaidStatus.AutoSize = false;
            this.lblPaidStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblPaidStatus.Text = "הסטטוס \"שולם\" מחושב אוטומטית מהתשלומים.";
            this.lblPaidStatus.ForeColor = System.Drawing.SystemColors.GrayText;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(850, 530);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 40);
            this.btnSave.TabIndex = 57;
            this.btnSave.Text = "שמירה";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDeleteCase
            // 
            this.btnDeleteCase.Location = new System.Drawing.Point(20, 530);
            this.btnDeleteCase.Name = "btnDeleteCase";
            this.btnDeleteCase.Size = new System.Drawing.Size(130, 40);
            this.btnDeleteCase.TabIndex = 58;
            this.btnDeleteCase.Text = "מחיקת תיק";
            this.btnDeleteCase.UseVisualStyleBackColor = true;
            this.btnDeleteCase.Click += new System.EventHandler(this.btnDeleteCase_Click);
            // 
            // lblSaveHint
            // 
            this.lblSaveHint.Location = new System.Drawing.Point(421, 541);
            this.lblSaveHint.Name = "lblSaveHint";
            this.lblSaveHint.Size = new System.Drawing.Size(414, 23);
            this.lblSaveHint.TabIndex = 59;
            this.lblSaveHint.AutoSize = false;
            this.lblSaveHint.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSaveHint.Text = "שמרו את התיק כדי לנהל תהליכים ותשלומים.";
            this.lblSaveHint.ForeColor = System.Drawing.SystemColors.GrayText;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSteps);
            this.tabControl1.Controls.Add(this.tabPayments);
            this.tabControl1.Location = new System.Drawing.Point(20, 580);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Size = new System.Drawing.Size(960, 300);
            this.tabControl1.TabIndex = 60;
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabSteps
            // 
            this.tabSteps.Controls.Add(this.listView1);
            this.tabSteps.Controls.Add(this.btnCompleteStep);
            this.tabSteps.Controls.Add(this.btnReschedule);
            this.tabSteps.Controls.Add(this.btnStepNote);
            this.tabSteps.Controls.Add(this.btnUndoStep);
            this.tabSteps.Controls.Add(this.lblStepsHint);
            this.tabSteps.Location = new System.Drawing.Point(4, 28);
            this.tabSteps.Name = "tabSteps";
            this.tabSteps.Size = new System.Drawing.Size(952, 268);
            this.tabSteps.TabIndex = 61;
            this.tabSteps.Text = "תהליכים";
            this.tabSteps.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colStepNo,
            this.colStepName,
            this.colEstimated,
            this.colDone,
            this.colStepStatus,
            this.colLate,
            this.colNote});
            this.listView1.Location = new System.Drawing.Point(6, 6);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(940, 210);
            this.listView1.TabIndex = 62;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.MultiSelect = false;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // colStepNo
            // 
            this.colStepNo.Text = "#";
            this.colStepNo.Width = 40;
            // 
            // colStepName
            // 
            this.colStepName.Text = "שם התהליך";
            this.colStepName.Width = 240;
            // 
            // colEstimated
            // 
            this.colEstimated.Text = "מועד משוער";
            this.colEstimated.Width = 115;
            // 
            // colDone
            // 
            this.colDone.Text = "בוצע בתאריך";
            this.colDone.Width = 115;
            // 
            // colStepStatus
            // 
            this.colStepStatus.Text = "סטטוס";
            this.colStepStatus.Width = 105;
            // 
            // colLate
            // 
            this.colLate.Text = "איחור";
            this.colLate.Width = 105;
            // 
            // colNote
            // 
            this.colNote.Text = "הערות";
            this.colNote.Width = 210;
            // 
            // btnCompleteStep
            // 
            this.btnCompleteStep.Location = new System.Drawing.Point(746, 224);
            this.btnCompleteStep.Name = "btnCompleteStep";
            this.btnCompleteStep.Size = new System.Drawing.Size(200, 36);
            this.btnCompleteStep.TabIndex = 63;
            this.btnCompleteStep.Text = "סימון השלב הבא כבוצע...";
            this.btnCompleteStep.UseVisualStyleBackColor = true;
            this.btnCompleteStep.Click += new System.EventHandler(this.btnCompleteStep_Click);
            // 
            // btnReschedule
            // 
            this.btnReschedule.Location = new System.Drawing.Point(608, 224);
            this.btnReschedule.Name = "btnReschedule";
            this.btnReschedule.Size = new System.Drawing.Size(130, 36);
            this.btnReschedule.TabIndex = 64;
            this.btnReschedule.Text = "שינוי מועד...";
            this.btnReschedule.UseVisualStyleBackColor = true;
            this.btnReschedule.Click += new System.EventHandler(this.btnReschedule_Click);
            // 
            // btnStepNote
            // 
            this.btnStepNote.Location = new System.Drawing.Point(500, 224);
            this.btnStepNote.Name = "btnStepNote";
            this.btnStepNote.Size = new System.Drawing.Size(100, 36);
            this.btnStepNote.TabIndex = 65;
            this.btnStepNote.Text = "הערה...";
            this.btnStepNote.UseVisualStyleBackColor = true;
            this.btnStepNote.Click += new System.EventHandler(this.btnStepNote_Click);
            // 
            // btnUndoStep
            // 
            this.btnUndoStep.Location = new System.Drawing.Point(322, 224);
            this.btnUndoStep.Name = "btnUndoStep";
            this.btnUndoStep.Size = new System.Drawing.Size(170, 36);
            this.btnUndoStep.TabIndex = 66;
            this.btnUndoStep.Text = "ביטול הסימון האחרון";
            this.btnUndoStep.UseVisualStyleBackColor = true;
            this.btnUndoStep.Click += new System.EventHandler(this.btnUndoStep_Click);
            // 
            // lblStepsHint
            // 
            this.lblStepsHint.Location = new System.Drawing.Point(3, 226);
            this.lblStepsHint.Name = "lblStepsHint";
            this.lblStepsHint.Size = new System.Drawing.Size(309, 23);
            this.lblStepsHint.TabIndex = 67;
            this.lblStepsHint.AutoSize = false;
            this.lblStepsHint.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblStepsHint.Text = "כל שינוי מעדכן את מועדי השלבים הבאים.";
            this.lblStepsHint.ForeColor = System.Drawing.SystemColors.GrayText;
            // 
            // tabPayments
            // 
            this.tabPayments.Controls.Add(this.listView2);
            this.tabPayments.Controls.Add(this.btnAddPayment);
            this.tabPayments.Controls.Add(this.btnReceipt);
            this.tabPayments.Controls.Add(this.btnCancelPayment);
            this.tabPayments.Controls.Add(this.btnRefund);
            this.tabPayments.Location = new System.Drawing.Point(4, 28);
            this.tabPayments.Name = "tabPayments";
            this.tabPayments.Size = new System.Drawing.Size(952, 268);
            this.tabPayments.TabIndex = 68;
            this.tabPayments.Text = "תשלומים";
            this.tabPayments.UseVisualStyleBackColor = true;
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPayId,
            this.colPayDate,
            this.colPayKind,
            this.colPayMethod,
            this.colPayAmount,
            this.colPayBalance,
            this.colPayStatus,
            this.colPayDesc});
            this.listView2.Location = new System.Drawing.Point(6, 6);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(940, 210);
            this.listView2.TabIndex = 69;
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.HideSelection = false;
            this.listView2.MultiSelect = false;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // colPayId
            // 
            this.colPayId.Text = "קבלה";
            this.colPayId.Width = 60;
            // 
            // colPayDate
            // 
            this.colPayDate.Text = "תאריך";
            this.colPayDate.Width = 100;
            // 
            // colPayKind
            // 
            this.colPayKind.Text = "סוג";
            this.colPayKind.Width = 115;
            // 
            // colPayMethod
            // 
            this.colPayMethod.Text = "אמצעי תשלום";
            this.colPayMethod.Width = 110;
            // 
            // colPayAmount
            // 
            this.colPayAmount.Text = "סכום";
            this.colPayAmount.Width = 115;
            // 
            // colPayBalance
            // 
            this.colPayBalance.Text = "יתרה";
            this.colPayBalance.Width = 115;
            // 
            // colPayStatus
            // 
            this.colPayStatus.Text = "סטטוס";
            this.colPayStatus.Width = 75;
            // 
            // colPayDesc
            // 
            this.colPayDesc.Text = "תיאור";
            this.colPayDesc.Width = 300;
            // 
            // btnAddPayment
            // 
            this.btnAddPayment.Location = new System.Drawing.Point(796, 224);
            this.btnAddPayment.Name = "btnAddPayment";
            this.btnAddPayment.Size = new System.Drawing.Size(150, 36);
            this.btnAddPayment.TabIndex = 70;
            this.btnAddPayment.Text = "הוספת תשלום";
            this.btnAddPayment.UseVisualStyleBackColor = true;
            this.btnAddPayment.Click += new System.EventHandler(this.btnAddPayment_Click);
            // 
            // btnReceipt
            // 
            this.btnReceipt.Location = new System.Drawing.Point(636, 224);
            this.btnReceipt.Name = "btnReceipt";
            this.btnReceipt.Size = new System.Drawing.Size(150, 36);
            this.btnReceipt.TabIndex = 71;
            this.btnReceipt.Text = "הצגת קבלה";
            this.btnReceipt.UseVisualStyleBackColor = true;
            this.btnReceipt.Click += new System.EventHandler(this.btnReceipt_Click);
            // 
            // btnCancelPayment
            // 
            this.btnCancelPayment.Location = new System.Drawing.Point(476, 224);
            this.btnCancelPayment.Name = "btnCancelPayment";
            this.btnCancelPayment.Size = new System.Drawing.Size(150, 36);
            this.btnCancelPayment.TabIndex = 72;
            this.btnCancelPayment.Text = "ביטול תשלום...";
            this.btnCancelPayment.UseVisualStyleBackColor = true;
            this.btnCancelPayment.Click += new System.EventHandler(this.btnCancelPayment_Click);
            // 
            // btnRefund
            // 
            this.btnRefund.Location = new System.Drawing.Point(316, 224);
            this.btnRefund.Name = "btnRefund";
            this.btnRefund.Size = new System.Drawing.Size(150, 36);
            this.btnRefund.TabIndex = 73;
            this.btnRefund.Text = "החזר עודף...";
            this.btnRefund.UseVisualStyleBackColor = true;
            this.btnRefund.Visible = false;
            this.btnRefund.Click += new System.EventHandler(this.btnRefund_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.RightToLeft = true;
            // 
            // CaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 895);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblSaveHint);
            this.Controls.Add(this.btnDeleteCase);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBoxMoney);
            this.Controls.Add(this.groupBoxDeal);
            this.Controls.Add(this.groupBoxCourt);
            this.Controls.Add(this.groupBoxOpposing);
            this.Controls.Add(this.groupBoxDetails);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.Name = "CaseForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "תיק לטיפול עורך דין";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.groupBoxDetails.ResumeLayout(false);
            this.groupBoxDetails.PerformLayout();
            this.groupBoxOpposing.ResumeLayout(false);
            this.groupBoxOpposing.PerformLayout();
            this.groupBoxCourt.ResumeLayout(false);
            this.groupBoxCourt.PerformLayout();
            this.groupBoxDeal.ResumeLayout(false);
            this.groupBoxDeal.PerformLayout();
            this.groupBoxMoney.ResumeLayout(false);
            this.groupBoxMoney.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabSteps.ResumeLayout(false);
            this.tabSteps.PerformLayout();
            this.tabPayments.ResumeLayout(false);
            this.tabPayments.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDetails;
        private System.Windows.Forms.Label lblCaseIdTitle;
        private System.Windows.Forms.Label lblCaseId;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.ComboBox cmbClient;
        private System.Windows.Forms.Button btnAddClient;
        private System.Windows.Forms.Label lblCaseType;
        private System.Windows.Forms.ComboBox cmbCaseType;
        private System.Windows.Forms.Label lblCaseTypeLock;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.RadioButton rbCourt;
        private System.Windows.Forms.RadioButton rbDeal;
        private System.Windows.Forms.Label lblPowerOfAttorney;
        private System.Windows.Forms.CheckBox chkPowerOfAttorney;
        private System.Windows.Forms.DateTimePicker dtpPowerOfAttorney;
        private System.Windows.Forms.Label lblStatusTitle;
        private System.Windows.Forms.Label lblCaseStatus;
        private System.Windows.Forms.GroupBox groupBoxOpposing;
        private System.Windows.Forms.Label lblOpposingParty;
        private System.Windows.Forms.ComboBox cmbOpposingParty;
        private System.Windows.Forms.Button btnAddOpposingParty;
        private System.Windows.Forms.Label lblOpposingLawyer;
        private System.Windows.Forms.ComboBox cmbOpposingLawyer;
        private System.Windows.Forms.Button btnAddLawyer;
        private System.Windows.Forms.Label lblOpposingHint;
        private System.Windows.Forms.Label lblOpposingDetails;
        private System.Windows.Forms.GroupBox groupBoxCourt;
        private System.Windows.Forms.Label lblCourt;
        private System.Windows.Forms.ComboBox cmbCourt;
        private System.Windows.Forms.Label lblClaimAmount;
        private System.Windows.Forms.TextBox txtClaimAmount;
        private System.Windows.Forms.Label lblAwardedAmount;
        private System.Windows.Forms.TextBox txtAwardedAmount;
        private System.Windows.Forms.GroupBox groupBoxDeal;
        private System.Windows.Forms.Label lblPropertyAddress;
        private System.Windows.Forms.TextBox txtPropertyAddress;
        private System.Windows.Forms.Label lblBlock;
        private System.Windows.Forms.TextBox txtBlock;
        private System.Windows.Forms.Label lblParcel;
        private System.Windows.Forms.TextBox txtParcel;
        private System.Windows.Forms.Label lblSubParcel;
        private System.Windows.Forms.TextBox txtSubParcel;
        private System.Windows.Forms.Label lblPropertyValue;
        private System.Windows.Forms.TextBox txtPropertyValue;
        private System.Windows.Forms.GroupBox groupBoxMoney;
        private System.Windows.Forms.Label lblFeePercent;
        private System.Windows.Forms.TextBox txtFeePercent;
        private System.Windows.Forms.Label lblAdvance;
        private System.Windows.Forms.TextBox txtAdvance;
        private System.Windows.Forms.Label lblAdvanceStatus;
        private System.Windows.Forms.Label lblTotalTitle;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblPaidTitle;
        private System.Windows.Forms.Label lblPaid;
        private System.Windows.Forms.Label lblBalanceTitle;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblPaidStatus;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDeleteCase;
        private System.Windows.Forms.Label lblSaveHint;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSteps;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader colStepNo;
        private System.Windows.Forms.ColumnHeader colStepName;
        private System.Windows.Forms.ColumnHeader colEstimated;
        private System.Windows.Forms.ColumnHeader colDone;
        private System.Windows.Forms.ColumnHeader colStepStatus;
        private System.Windows.Forms.ColumnHeader colLate;
        private System.Windows.Forms.ColumnHeader colNote;
        private System.Windows.Forms.Button btnCompleteStep;
        private System.Windows.Forms.Button btnReschedule;
        private System.Windows.Forms.Button btnStepNote;
        private System.Windows.Forms.Button btnUndoStep;
        private System.Windows.Forms.Label lblStepsHint;
        private System.Windows.Forms.TabPage tabPayments;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader colPayId;
        private System.Windows.Forms.ColumnHeader colPayDate;
        private System.Windows.Forms.ColumnHeader colPayKind;
        private System.Windows.Forms.ColumnHeader colPayMethod;
        private System.Windows.Forms.ColumnHeader colPayAmount;
        private System.Windows.Forms.ColumnHeader colPayBalance;
        private System.Windows.Forms.ColumnHeader colPayStatus;
        private System.Windows.Forms.ColumnHeader colPayDesc;
        private System.Windows.Forms.Button btnAddPayment;
        private System.Windows.Forms.Button btnReceipt;
        private System.Windows.Forms.Button btnCancelPayment;
        private System.Windows.Forms.Button btnRefund;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
