namespace LawyerProject.GUI
{
    partial class PaymentsListForm
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
            this.groupBoxSearch = new System.Windows.Forms.GroupBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.gridPayments = new System.Windows.Forms.DataGridView();
            this.btnReceipt = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.groupBoxSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPayments)).BeginInit();
            this.btnCancelPayment = new System.Windows.Forms.Button();
            this.btnOpenCase = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // groupBoxSearch
            //
            this.groupBoxSearch.Controls.Add(this.txtSearch);
            this.groupBoxSearch.Controls.Add(this.lblSearch);
            this.groupBoxSearch.Location = new System.Drawing.Point(20, 15);
            this.groupBoxSearch.Name = "groupBoxSearch";
            this.groupBoxSearch.Size = new System.Drawing.Size(1180, 70);
            this.groupBoxSearch.TabIndex = 0;
            this.groupBoxSearch.TabStop = false;
            this.groupBoxSearch.Text = "חיפוש";
            //
            // lblSearch
            //
            this.lblSearch.AutoSize = false;
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblSearch.Location = new System.Drawing.Point(976, 30);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(184, 23);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "תיק / לקוח / קבלה";
            //
            // txtSearch
            //
            this.txtSearch.Location = new System.Drawing.Point(720, 27);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(250, 27);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            //
            // gridPayments
            //
            this.gridPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPayments.Location = new System.Drawing.Point(20, 95);
            this.gridPayments.Name = "gridPayments";
            this.gridPayments.Size = new System.Drawing.Size(1180, 380);
            this.gridPayments.TabIndex = 1;
            this.gridPayments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            //
            // btnReceipt
            //
            this.btnReceipt.Location = new System.Drawing.Point(1060, 490);
            this.btnReceipt.Name = "btnReceipt";
            this.btnReceipt.Size = new System.Drawing.Size(140, 40);
            this.btnReceipt.TabIndex = 2;
            this.btnReceipt.Text = "הצגת קבלה";
            this.btnReceipt.UseVisualStyleBackColor = true;
            this.btnReceipt.Click += new System.EventHandler(this.btnReceipt_Click);
            //
            // lblTotal
            //
            this.lblTotal.AutoSize = false;
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblTotal.Location = new System.Drawing.Point(20, 500);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(400, 23);
            this.lblTotal.TabIndex = 3;
            // 
            // btnCancelPayment
            // 
            this.btnCancelPayment.Location = new System.Drawing.Point(910, 490);
            this.btnCancelPayment.Name = "btnCancelPayment";
            this.btnCancelPayment.Size = new System.Drawing.Size(140, 40);
            this.btnCancelPayment.TabIndex = 10;
            this.btnCancelPayment.Text = "ביטול תשלום...";
            this.btnCancelPayment.UseVisualStyleBackColor = true;
            this.btnCancelPayment.Click += new System.EventHandler(this.btnCancelPayment_Click);
            // 
            // btnOpenCase
            // 
            this.btnOpenCase.Location = new System.Drawing.Point(760, 490);
            this.btnOpenCase.Name = "btnOpenCase";
            this.btnOpenCase.Size = new System.Drawing.Size(140, 40);
            this.btnOpenCase.TabIndex = 11;
            this.btnOpenCase.Text = "פתיחת התיק";
            this.btnOpenCase.UseVisualStyleBackColor = true;
            this.btnOpenCase.Click += new System.EventHandler(this.btnOpenCase_Click);
            //
            // PaymentsListForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 550);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnCancelPayment);
            this.Controls.Add(this.btnOpenCase);
            this.Controls.Add(this.btnReceipt);
            this.Controls.Add(this.gridPayments);
            this.Controls.Add(this.groupBoxSearch);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.Name = "PaymentsListForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "תשלומים וקבלות";
            this.groupBoxSearch.ResumeLayout(false);
            this.groupBoxSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridPayments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView gridPayments;
        private System.Windows.Forms.Button btnReceipt;
        private System.Windows.Forms.Button btnCancelPayment;
        private System.Windows.Forms.Button btnOpenCase;
        private System.Windows.Forms.Label lblTotal;
    }
}
