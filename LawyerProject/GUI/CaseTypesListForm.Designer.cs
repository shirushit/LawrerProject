namespace LawyerProject.GUI
{
    partial class CaseTypesListForm
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
            this.gridCaseTypes = new System.Windows.Forms.DataGridView();
            this.groupSearch = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCaseTypeName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridCaseTypes)).BeginInit();
            this.groupSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnUpdate.Location = new System.Drawing.Point(88, 573);
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
            this.btnAdd.Location = new System.Drawing.Point(88, 512);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 34);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "הוספה";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // gridCaseTypes
            // 
            this.gridCaseTypes.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.gridCaseTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCaseTypes.Location = new System.Drawing.Point(233, 218);
            this.gridCaseTypes.Margin = new System.Windows.Forms.Padding(4);
            this.gridCaseTypes.MultiSelect = false;
            this.gridCaseTypes.Name = "gridCaseTypes";
            this.gridCaseTypes.ReadOnly = true;
            this.gridCaseTypes.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridCaseTypes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCaseTypes.Size = new System.Drawing.Size(435, 327);
            this.gridCaseTypes.TabIndex = 9;
            // 
            // groupSearch
            // 
            this.groupSearch.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupSearch.Controls.Add(this.label1);
            this.groupSearch.Controls.Add(this.txtCaseTypeName);
            this.groupSearch.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.groupSearch.Location = new System.Drawing.Point(331, 32);
            this.groupSearch.Margin = new System.Windows.Forms.Padding(4);
            this.groupSearch.Name = "groupSearch";
            this.groupSearch.Padding = new System.Windows.Forms.Padding(4);
            this.groupSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupSearch.Size = new System.Drawing.Size(337, 146);
            this.groupSearch.TabIndex = 8;
            this.groupSearch.TabStop = false;
            this.groupSearch.Text = "חיפוש נושא תיק";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(202, 99);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "שם נושא תיק";
            // 
            // txtCaseTypeName
            // 
            this.txtCaseTypeName.Location = new System.Drawing.Point(56, 95);
            this.txtCaseTypeName.Margin = new System.Windows.Forms.Padding(4);
            this.txtCaseTypeName.Name = "txtCaseTypeName";
            this.txtCaseTypeName.Size = new System.Drawing.Size(132, 27);
            this.txtCaseTypeName.TabIndex = 0;
            this.txtCaseTypeName.TextChanged += new System.EventHandler(this.txtCaseTypeName_TextChanged);
            this.txtCaseTypeName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCaseTypeName_KeyPress);
            // 
            // CaseTypesListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(820, 658);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.gridCaseTypes);
            this.Controls.Add(this.groupSearch);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CaseTypesListForm";
            this.Text = "כל נושאי תיק";
            ((System.ComponentModel.ISupportInitialize)(this.gridCaseTypes)).EndInit();
            this.groupSearch.ResumeLayout(false);
            this.groupSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView gridCaseTypes;
        private System.Windows.Forms.GroupBox groupSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCaseTypeName;
    }
}