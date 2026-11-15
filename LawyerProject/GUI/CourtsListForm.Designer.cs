namespace LawyerProject.GUI
{
    partial class CourtsListForm
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
            this.gridCourts = new System.Windows.Forms.DataGridView();
            this.groupSearch = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCourtName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridCourts)).BeginInit();
            this.groupSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnUpdate.Location = new System.Drawing.Point(314, 678);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(101, 34);
            this.btnUpdate.TabIndex = 7;
            this.btnUpdate.Text = "עדכון";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAdd.Location = new System.Drawing.Point(314, 617);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(101, 34);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "הוספה";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gridCourts
            // 
            this.gridCourts.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.gridCourts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCourts.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.gridCourts.Location = new System.Drawing.Point(144, 246);
            this.gridCourts.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.gridCourts.MultiSelect = false;
            this.gridCourts.Name = "gridCourts";
            this.gridCourts.ReadOnly = true;
            this.gridCourts.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridCourts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCourts.Size = new System.Drawing.Size(475, 327);
            this.gridCourts.TabIndex = 5;
            // 
            // groupSearch
            // 
            this.groupSearch.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupSearch.Controls.Add(this.label1);
            this.groupSearch.Controls.Add(this.txtCourtName);
            this.groupSearch.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.groupSearch.Location = new System.Drawing.Point(353, 57);
            this.groupSearch.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupSearch.Name = "groupSearch";
            this.groupSearch.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupSearch.Size = new System.Drawing.Size(266, 146);
            this.groupSearch.TabIndex = 4;
            this.groupSearch.TabStop = false;
            this.groupSearch.Text = "חיפוש בית משפט";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(167, 99);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "שם בית משפט";
            // 
            // txtCourtName
            // 
            this.txtCourtName.Location = new System.Drawing.Point(25, 89);
            this.txtCourtName.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtCourtName.Name = "txtCourtName";
            this.txtCourtName.Size = new System.Drawing.Size(132, 27);
            this.txtCourtName.TabIndex = 0;
            this.txtCourtName.TextChanged += new System.EventHandler(this.txtCourtName_TextChanged);
            this.txtCourtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCourtName_KeyPress);
            // 
            // CourtsListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(766, 735);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.gridCourts);
            this.Controls.Add(this.groupSearch);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "CourtsListForm";
            this.Text = "כל בתי המשפט";
            ((System.ComponentModel.ISupportInitialize)(this.gridCourts)).EndInit();
            this.groupSearch.ResumeLayout(false);
            this.groupSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView gridCourts;
        private System.Windows.Forms.GroupBox groupSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCourtName;
    }
}