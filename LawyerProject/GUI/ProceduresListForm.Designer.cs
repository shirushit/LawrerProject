namespace LawyerProject.GUI
{
    partial class ProceduresListForm
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
            this.gridProcedures = new System.Windows.Forms.DataGridView();
            this.groupSearch = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProcedureName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridProcedures)).BeginInit();
            this.groupSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnUpdate.Location = new System.Drawing.Point(136, 757);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(100, 34);
            this.btnUpdate.TabIndex = 19;
            this.btnUpdate.Text = "עדכון";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click_2);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAdd.Location = new System.Drawing.Point(136, 715);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 34);
            this.btnAdd.TabIndex = 18;
            this.btnAdd.Text = "הוספה";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gridProcedures
            // 
            this.gridProcedures.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.gridProcedures.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridProcedures.Location = new System.Drawing.Point(136, 307);
            this.gridProcedures.Margin = new System.Windows.Forms.Padding(4);
            this.gridProcedures.MultiSelect = false;
            this.gridProcedures.Name = "gridProcedures";
            this.gridProcedures.ReadOnly = true;
            this.gridProcedures.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridProcedures.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridProcedures.Size = new System.Drawing.Size(995, 349);
            this.gridProcedures.TabIndex = 17;
            // 
            // groupSearch
            // 
            this.groupSearch.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupSearch.Controls.Add(this.label1);
            this.groupSearch.Controls.Add(this.txtProcedureName);
            this.groupSearch.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.groupSearch.Location = new System.Drawing.Point(462, 101);
            this.groupSearch.Margin = new System.Windows.Forms.Padding(4);
            this.groupSearch.Name = "groupSearch";
            this.groupSearch.Padding = new System.Windows.Forms.Padding(4);
            this.groupSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupSearch.Size = new System.Drawing.Size(367, 118);
            this.groupSearch.TabIndex = 16;
            this.groupSearch.TabStop = false;
            this.groupSearch.Text = "חיפוש תהליך לתיק";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(211, 75);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "שם תהליך תיק";
            // 
            // txtProcedureName
            // 
            this.txtProcedureName.Location = new System.Drawing.Point(66, 70);
            this.txtProcedureName.Margin = new System.Windows.Forms.Padding(4);
            this.txtProcedureName.Name = "txtProcedureName";
            this.txtProcedureName.Size = new System.Drawing.Size(132, 27);
            this.txtProcedureName.TabIndex = 0;
            // 
            // ProceduresListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1243, 870);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.gridProcedures);
            this.Controls.Add(this.groupSearch);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ProceduresListForm";
            this.Text = "כל התהליכים לתיקים";
            ((System.ComponentModel.ISupportInitialize)(this.gridProcedures)).EndInit();
            this.groupSearch.ResumeLayout(false);
            this.groupSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView gridProcedures;
        private System.Windows.Forms.GroupBox groupSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProcedureName;
    }
}