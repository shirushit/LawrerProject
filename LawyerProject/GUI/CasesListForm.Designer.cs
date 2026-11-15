namespace LawyerProject.GUI
{
    partial class CasesListForm
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
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblLegendLate = new System.Windows.Forms.Label();
            this.lblLegendSoon = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtFilterClientId = new System.Windows.Forms.MaskedTextBox();
            this.cmbFilterCaseType = new System.Windows.Forms.ComboBox();
            this.cmbFilterClient = new System.Windows.Forms.ComboBox();
            this.cmbFilterOpposingParty = new System.Windows.Forms.ComboBox();
            this.cmbFilterOpposingLawyer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFilterCaseId = new System.Windows.Forms.TextBox();
            this.cmbFilterStatus = new System.Windows.Forms.ComboBox();
            this.chkFilterPaid = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnUpdate.Location = new System.Drawing.Point(405, 813);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(100, 34);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "עדכון";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAdd.Location = new System.Drawing.Point(579, 813);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 34);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "הוספה";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(32, 243);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1125, 520);
            this.dataGridView1.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox2.Controls.Add(this.txtFilterClientId);
            this.groupBox2.Controls.Add(this.cmbFilterCaseType);
            this.groupBox2.Controls.Add(this.cmbFilterClient);
            this.groupBox2.Controls.Add(this.cmbFilterOpposingParty);
            this.groupBox2.Controls.Add(this.cmbFilterOpposingLawyer);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtFilterCaseId);
            this.groupBox2.Controls.Add(this.cmbFilterStatus);
            this.groupBox2.Controls.Add(this.chkFilterPaid);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.groupBox2.Location = new System.Drawing.Point(32, 18);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox2.Size = new System.Drawing.Size(1125, 180);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "חיפוש תיק לטיפול";
            // 
            // txtFilterClientId
            // 
            this.txtFilterClientId.Location = new System.Drawing.Point(871, 53);
            this.txtFilterClientId.Margin = new System.Windows.Forms.Padding(4);
            this.txtFilterClientId.Mask = "000000000";
            this.txtFilterClientId.Name = "txtFilterClientId";
            this.txtFilterClientId.PromptChar = ' ';
            this.txtFilterClientId.Size = new System.Drawing.Size(160, 37);
            this.txtFilterClientId.TabIndex = 23;
            // 
            // cmbFilterCaseType
            // 
            this.cmbFilterCaseType.FormattingEnabled = true;
            this.cmbFilterCaseType.Location = new System.Drawing.Point(604, 53);
            this.cmbFilterCaseType.Margin = new System.Windows.Forms.Padding(4);
            this.cmbFilterCaseType.Name = "cmbFilterCaseType";
            this.cmbFilterCaseType.Size = new System.Drawing.Size(160, 37);
            this.cmbFilterCaseType.TabIndex = 22;
            // 
            // cmbFilterClient
            // 
            this.cmbFilterClient.FormattingEnabled = true;
            this.cmbFilterClient.Location = new System.Drawing.Point(871, 114);
            this.cmbFilterClient.Margin = new System.Windows.Forms.Padding(4);
            this.cmbFilterClient.Name = "cmbFilterClient";
            this.cmbFilterClient.Size = new System.Drawing.Size(160, 37);
            this.cmbFilterClient.TabIndex = 21;
            // 
            // cmbFilterOpposingParty
            // 
            this.cmbFilterOpposingParty.FormattingEnabled = true;
            this.cmbFilterOpposingParty.Location = new System.Drawing.Point(328, 108);
            this.cmbFilterOpposingParty.Margin = new System.Windows.Forms.Padding(4);
            this.cmbFilterOpposingParty.Name = "cmbFilterOpposingParty";
            this.cmbFilterOpposingParty.Size = new System.Drawing.Size(160, 37);
            this.cmbFilterOpposingParty.TabIndex = 20;
            // 
            // cmbFilterOpposingLawyer
            // 
            this.cmbFilterOpposingLawyer.FormattingEnabled = true;
            this.cmbFilterOpposingLawyer.Location = new System.Drawing.Point(328, 47);
            this.cmbFilterOpposingLawyer.Margin = new System.Windows.Forms.Padding(4);
            this.cmbFilterOpposingLawyer.Name = "cmbFilterOpposingLawyer";
            this.cmbFilterOpposingLawyer.Size = new System.Drawing.Size(160, 37);
            this.cmbFilterOpposingLawyer.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(496, 116);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 29);
            this.label1.TabIndex = 18;
            this.label1.Text = "לקוח נגדי";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(1040, 123);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 29);
            this.label2.TabIndex = 16;
            this.label2.Text = "שם לקוח";
            // 
            // txtFilterCaseId
            // 
            this.txtFilterCaseId.Location = new System.Drawing.Point(604, 110);
            this.txtFilterCaseId.Margin = new System.Windows.Forms.Padding(4);
            this.txtFilterCaseId.Name = "txtFilterCaseId";
            this.txtFilterCaseId.Size = new System.Drawing.Size(160, 37);
            this.txtFilterCaseId.TabIndex = 15;
            this.txtFilterCaseId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilterCaseId_KeyPress);
            // 
            // cmbFilterStatus
            // 
            this.cmbFilterStatus.FormattingEnabled = true;
            this.cmbFilterStatus.Location = new System.Drawing.Point(51, 42);
            this.cmbFilterStatus.Margin = new System.Windows.Forms.Padding(4);
            this.cmbFilterStatus.Name = "cmbFilterStatus";
            this.cmbFilterStatus.Size = new System.Drawing.Size(160, 37);
            this.cmbFilterStatus.TabIndex = 10;
            // 
            // chkFilterPaid
            // 
            this.chkFilterPaid.AutoSize = true;
            this.chkFilterPaid.Location = new System.Drawing.Point(192, 114);
            this.chkFilterPaid.Margin = new System.Windows.Forms.Padding(4);
            this.chkFilterPaid.Name = "chkFilterPaid";
            this.chkFilterPaid.Size = new System.Drawing.Size(22, 21);
            this.chkFilterPaid.TabIndex = 9;
            this.chkFilterPaid.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label7.Location = new System.Drawing.Point(220, 50);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 29);
            this.label7.TabIndex = 8;
            this.label7.Text = "סטטוס תיק";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label10.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label10.Location = new System.Drawing.Point(496, 50);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(121, 29);
            this.label10.TabIndex = 6;
            this.label10.Text = "עורך דין נגדי";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label11.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label11.Location = new System.Drawing.Point(220, 111);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 29);
            this.label11.TabIndex = 5;
            this.label11.Text = "שולם";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label12.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label12.Location = new System.Drawing.Point(788, 123);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(104, 29);
            this.label12.TabIndex = 4;
            this.label12.Text = "מספר תיק";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label13.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label13.Location = new System.Drawing.Point(775, 57);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(117, 29);
            this.label13.TabIndex = 3;
            this.label13.Text = "שם סוג תיק";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label14.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label14.Location = new System.Drawing.Point(1040, 60);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(95, 29);
            this.label14.TabIndex = 1;
            this.label14.Text = "ת.ז. לקוח";
            // 
            // lblSearch
            // 
            this.lblSearch.Location = new System.Drawing.Point(1000, 208);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(157, 26);
            this.lblSearch.TabIndex = 20;
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSearch.Text = "חיפוש חופשי";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(620, 205);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(375, 32);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.Font = new System.Drawing.Font("Calibri", 12F);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(490, 203);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 34);
            this.btnClear.TabIndex = 21;
            this.btnClear.Text = "ניקוי סינון";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblLegendLate
            // 
            this.lblLegendLate.BackColor = System.Drawing.Color.MistyRose;
            this.lblLegendLate.Location = new System.Drawing.Point(32, 208);
            this.lblLegendLate.Name = "lblLegendLate";
            this.lblLegendLate.Size = new System.Drawing.Size(130, 26);
            this.lblLegendLate.TabIndex = 22;
            this.lblLegendLate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLegendLate.Font = new System.Drawing.Font("Calibri", 10F);
            this.lblLegendLate.Text = "שלב באיחור";
            // 
            // lblLegendSoon
            // 
            this.lblLegendSoon.BackColor = System.Drawing.Color.LightYellow;
            this.lblLegendSoon.Location = new System.Drawing.Point(170, 208);
            this.lblLegendSoon.Name = "lblLegendSoon";
            this.lblLegendSoon.Size = new System.Drawing.Size(180, 26);
            this.lblLegendSoon.TabIndex = 23;
            this.lblLegendSoon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLegendSoon.Font = new System.Drawing.Font("Calibri", 10F);
            this.lblLegendSoon.Text = "מועד בשבוע הקרוב";
            // 
            // lblCount
            // 
            this.lblCount.Location = new System.Drawing.Point(800, 818);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(357, 26);
            this.lblCount.TabIndex = 24;
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblCount.Font = new System.Drawing.Font("Calibri", 11F);
            this.lblCount.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblCount.Text = "";
            // 
            // CasesListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1232, 856);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblLegendLate);
            this.Controls.Add(this.lblLegendSoon);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CasesListForm";
            this.Text = ";";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblLegendLate;
        private System.Windows.Forms.Label lblLegendSoon;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFilterCaseId;
        private System.Windows.Forms.ComboBox cmbFilterStatus;
        private System.Windows.Forms.CheckBox chkFilterPaid;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbFilterCaseType;
        private System.Windows.Forms.ComboBox cmbFilterClient;
        private System.Windows.Forms.ComboBox cmbFilterOpposingParty;
        private System.Windows.Forms.ComboBox cmbFilterOpposingLawyer;
        private System.Windows.Forms.MaskedTextBox txtFilterClientId;
    }
}