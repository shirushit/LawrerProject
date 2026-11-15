namespace LawyerProject.GUI
{
    partial class DealsListForm
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
            this.gridDeals = new System.Windows.Forms.DataGridView();
            this.groupSearch = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSubParcel = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPropertyAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtParcel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBlock = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridDeals)).BeginInit();
            this.groupSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnUpdate.Location = new System.Drawing.Point(363, 715);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(135, 50);
            this.btnUpdate.TabIndex = 11;
            this.btnUpdate.Text = "עדכון";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAdd.Location = new System.Drawing.Point(518, 715);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(135, 50);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "הוספה";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gridDeals
            // 
            this.gridDeals.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.gridDeals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDeals.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gridDeals.Location = new System.Drawing.Point(50, 270);
            this.gridDeals.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.gridDeals.MultiSelect = false;
            this.gridDeals.Name = "gridDeals";
            this.gridDeals.ReadOnly = true;
            this.gridDeals.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridDeals.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDeals.Size = new System.Drawing.Size(916, 420);
            this.gridDeals.TabIndex = 9;
            // 
            // groupSearch
            // 
            this.groupSearch.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupSearch.Controls.Add(this.label4);
            this.groupSearch.Controls.Add(this.txtSubParcel);
            this.groupSearch.Controls.Add(this.label3);
            this.groupSearch.Controls.Add(this.txtPropertyAddress);
            this.groupSearch.Controls.Add(this.label2);
            this.groupSearch.Controls.Add(this.txtParcel);
            this.groupSearch.Controls.Add(this.label1);
            this.groupSearch.Controls.Add(this.txtBlock);
            this.groupSearch.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.groupSearch.Location = new System.Drawing.Point(50, 25);
            this.groupSearch.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.groupSearch.Name = "groupSearch";
            this.groupSearch.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.groupSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupSearch.Size = new System.Drawing.Size(916, 225);
            this.groupSearch.TabIndex = 8;
            this.groupSearch.TabStop = false;
            this.groupSearch.Text = "חיפוש עסקה";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(269, 148);
            this.label4.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 19);
            this.label4.TabIndex = 7;
            this.label4.Text = "תת חלקה";
            // 
            // txtSubParcel
            // 
            this.txtSubParcel.Location = new System.Drawing.Point(87, 143);
            this.txtSubParcel.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.txtSubParcel.Name = "txtSubParcel";
            this.txtSubParcel.Size = new System.Drawing.Size(175, 27);
            this.txtSubParcel.TabIndex = 6;
            this.txtSubParcel.TextChanged += new System.EventHandler(this.txtSubParcel_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(537, 79);
            this.label3.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 19);
            this.label3.TabIndex = 5;
            this.label3.Text = "כתובת נכס";
            // 
            // txtPropertyAddress
            // 
            this.txtPropertyAddress.Location = new System.Drawing.Point(351, 69);
            this.txtPropertyAddress.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.txtPropertyAddress.Name = "txtPropertyAddress";
            this.txtPropertyAddress.Size = new System.Drawing.Size(175, 27);
            this.txtPropertyAddress.TabIndex = 4;
            this.txtPropertyAddress.TextChanged += new System.EventHandler(this.txtPropertyAddress_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(533, 153);
            this.label2.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "חלקה";
            // 
            // txtParcel
            // 
            this.txtParcel.Location = new System.Drawing.Point(351, 143);
            this.txtParcel.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.txtParcel.Name = "txtParcel";
            this.txtParcel.Size = new System.Drawing.Size(175, 27);
            this.txtParcel.TabIndex = 2;
            this.txtParcel.TextChanged += new System.EventHandler(this.txtParcel_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(774, 153);
            this.label1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "גוש";
            // 
            // txtBlock
            // 
            this.txtBlock.Location = new System.Drawing.Point(593, 143);
            this.txtBlock.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.txtBlock.Name = "txtBlock";
            this.txtBlock.Size = new System.Drawing.Size(175, 27);
            this.txtBlock.TabIndex = 0;
            this.txtBlock.TextChanged += new System.EventHandler(this.txtBlock_TextChanged);
            // 
            // DealsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1016, 790);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.gridDeals);
            this.Controls.Add(this.groupSearch);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "DealsListForm";
            this.Text = "כל העסקאות";
            ((System.ComponentModel.ISupportInitialize)(this.gridDeals)).EndInit();
            this.groupSearch.ResumeLayout(false);
            this.groupSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView gridDeals;
        private System.Windows.Forms.GroupBox groupSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBlock;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSubParcel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPropertyAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtParcel;
    }
}