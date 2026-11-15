namespace LawyerProject.GUI
{
    partial class CourtCaseTypeForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCourtCaseTypeId = new System.Windows.Forms.Label();
            this.lblCourtId = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.cmbCaseType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.button1.Location = new System.Drawing.Point(193, 311);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 34);
            this.button1.TabIndex = 14;
            this.button1.Text = "הוספה";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label6.Location = new System.Drawing.Point(282, 91);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 19);
            this.label6.TabIndex = 13;
            this.label6.Text = "קוד נושא לבת משפט";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label4.Location = new System.Drawing.Point(282, 161);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 19);
            this.label4.TabIndex = 11;
            this.label4.Text = "קוד בית משפט";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label3.Location = new System.Drawing.Point(282, 231);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 19);
            this.label3.TabIndex = 10;
            this.label3.Text = "שם נושא";
            // 
            // lblCourtCaseTypeId
            // 
            this.lblCourtCaseTypeId.AutoSize = true;
            this.lblCourtCaseTypeId.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblCourtCaseTypeId.Location = new System.Drawing.Point(190, 91);
            this.lblCourtCaseTypeId.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCourtCaseTypeId.Name = "lblCourtCaseTypeId";
            this.lblCourtCaseTypeId.Size = new System.Drawing.Size(0, 19);
            this.lblCourtCaseTypeId.TabIndex = 37;
            // 
            // lblCourtId
            // 
            this.lblCourtId.AutoSize = true;
            this.lblCourtId.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblCourtId.Location = new System.Drawing.Point(190, 159);
            this.lblCourtId.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCourtId.Name = "lblCourtId";
            this.lblCourtId.Size = new System.Drawing.Size(0, 19);
            this.lblCourtId.TabIndex = 38;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // cmbCaseType
            // 
            this.cmbCaseType.FormattingEnabled = true;
            this.cmbCaseType.Location = new System.Drawing.Point(101, 219);
            this.cmbCaseType.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.cmbCaseType.Name = "cmbCaseType";
            this.cmbCaseType.Size = new System.Drawing.Size(159, 27);
            this.cmbCaseType.TabIndex = 39;
            // 
            // CourtCaseTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(526, 462);
            this.Controls.Add(this.cmbCaseType);
            this.Controls.Add(this.lblCourtId);
            this.Controls.Add(this.lblCourtCaseTypeId);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "CourtCaseTypeForm";
            this.Text = "נושאים בבית משפט";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCourtCaseTypeId;
        private System.Windows.Forms.Label lblCourtId;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ComboBox cmbCaseType;
    }
}