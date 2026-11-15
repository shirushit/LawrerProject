namespace LawyerProject.GUI
{
    partial class CustomersListForm
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
            this.groupSearch = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.gridCustomers = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridCustomers)).BeginInit();
            this.groupSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupSearch
            // 
            this.groupSearch.Controls.Add(this.label1);
            this.groupSearch.Controls.Add(this.txtFilter);
            this.groupSearch.Controls.Add(this.lblType);
            this.groupSearch.Controls.Add(this.cmbType);
            this.groupSearch.Controls.Add(this.btnClear);
            this.groupSearch.Controls.Add(this.lblCount);
            this.groupSearch.Location = new System.Drawing.Point(20, 15);
            this.groupSearch.Name = "groupSearch";
            this.groupSearch.Size = new System.Drawing.Size(1260, 80);
            this.groupSearch.TabIndex = 0;
            this.groupSearch.Text = "חיפוש";
            this.groupSearch.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(1066, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 23);
            this.label1.TabIndex = 1;
            this.label1.AutoSize = false;
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label1.Text = "שם, מזהה, טלפון או עיר";
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(760, 34);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(300, 27);
            this.txtFilter.TabIndex = 2;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(686, 37);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(44, 23);
            this.lblType.TabIndex = 3;
            this.lblType.AutoSize = false;
            this.lblType.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblType.Text = "סוג";
            // 
            // cmbType
            // 
            this.cmbType.Location = new System.Drawing.Point(480, 34);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(200, 27);
            this.cmbType.TabIndex = 4;
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(320, 30);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(130, 34);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "ניקוי סינון";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblCount
            // 
            this.lblCount.Location = new System.Drawing.Point(236, 37);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(64, 23);
            this.lblCount.TabIndex = 6;
            this.lblCount.AutoSize = false;
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCount.Text = "";
            this.lblCount.ForeColor = System.Drawing.SystemColors.GrayText;
            // 
            // gridCustomers
            // 
            this.gridCustomers.Location = new System.Drawing.Point(20, 110);
            this.gridCustomers.Name = "gridCustomers";
            this.gridCustomers.Size = new System.Drawing.Size(1260, 640);
            this.gridCustomers.TabIndex = 7;
            this.gridCustomers.BackgroundColor = System.Drawing.Color.White;
            this.gridCustomers.RowHeadersVisible = false;
            this.gridCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCustomers.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(1150, 765);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(130, 40);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "הוספה";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(990, 765);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(150, 40);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "עדכון ותיקים";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // CustomersListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 820);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.gridCustomers);
            this.Controls.Add(this.groupSearch);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.Name = "CustomersListForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "לקוחות וצדדים";
            ((System.ComponentModel.ISupportInitialize)(this.gridCustomers)).EndInit();
            this.groupSearch.ResumeLayout(false);
            this.groupSearch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.DataGridView gridCustomers;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
    }
}
