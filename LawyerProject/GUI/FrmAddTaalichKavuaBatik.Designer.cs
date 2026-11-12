namespace LawyerProject.GUI
{
    partial class FrmAddTaalichKavuaBatik
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
            this.label6 = new System.Windows.Forms.Label();
            this.lblIdSubjektTik = new System.Windows.Forms.Label();
            this.lblTaalich = new System.Windows.Forms.Label();
            this.cmbTaalich = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownNumLevel = new System.Windows.Forms.NumericUpDown();
            this.lblLevelHint = new System.Windows.Forms.Label();
            this.blnAdd = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(374, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 23);
            this.label6.TabIndex = 0;
            this.label6.AutoSize = false;
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label6.Text = "נושא תיק";
            // 
            // lblIdSubjektTik
            // 
            this.lblIdSubjektTik.Location = new System.Drawing.Point(60, 25);
            this.lblIdSubjektTik.Name = "lblIdSubjektTik";
            this.lblIdSubjektTik.Size = new System.Drawing.Size(260, 23);
            this.lblIdSubjektTik.TabIndex = 1;
            this.lblIdSubjektTik.AutoSize = false;
            this.lblIdSubjektTik.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblIdSubjektTik.Text = "-";
            this.lblIdSubjektTik.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            // 
            // lblTaalich
            // 
            this.lblTaalich.Location = new System.Drawing.Point(382, 70);
            this.lblTaalich.Name = "lblTaalich";
            this.lblTaalich.Size = new System.Drawing.Size(68, 23);
            this.lblTaalich.TabIndex = 2;
            this.lblTaalich.AutoSize = false;
            this.lblTaalich.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblTaalich.Text = "תהליך *";
            // 
            // cmbTaalich
            // 
            this.cmbTaalich.Location = new System.Drawing.Point(90, 67);
            this.cmbTaalich.Name = "cmbTaalich";
            this.cmbTaalich.Size = new System.Drawing.Size(230, 27);
            this.cmbTaalich.TabIndex = 3;
            this.cmbTaalich.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(359, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 23);
            this.label3.TabIndex = 4;
            this.label3.AutoSize = false;
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label3.Text = "מספר שלב *";
            // 
            // numericUpDownNumLevel
            // 
            this.numericUpDownNumLevel.Location = new System.Drawing.Point(240, 112);
            this.numericUpDownNumLevel.Name = "numericUpDownNumLevel";
            this.numericUpDownNumLevel.Size = new System.Drawing.Size(80, 27);
            this.numericUpDownNumLevel.TabIndex = 5;
            this.numericUpDownNumLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numericUpDownNumLevel.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.numericUpDownNumLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblLevelHint
            // 
            this.lblLevelHint.Location = new System.Drawing.Point(42, 115);
            this.lblLevelHint.Name = "lblLevelHint";
            this.lblLevelHint.Size = new System.Drawing.Size(188, 23);
            this.lblLevelHint.TabIndex = 6;
            this.lblLevelHint.AutoSize = false;
            this.lblLevelHint.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblLevelHint.Text = "השלבים מתבצעים לפי הסדר";
            this.lblLevelHint.ForeColor = System.Drawing.SystemColors.GrayText;
            // 
            // blnAdd
            // 
            this.blnAdd.Location = new System.Drawing.Point(200, 160);
            this.blnAdd.Name = "blnAdd";
            this.blnAdd.Size = new System.Drawing.Size(120, 38);
            this.blnAdd.TabIndex = 7;
            this.blnAdd.Text = "שמירה";
            this.blnAdd.UseVisualStyleBackColor = true;
            this.blnAdd.Click += new System.EventHandler(this.blnAdd_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.RightToLeft = true;
            // 
            // FrmAddTaalichKavuaBatik
            // 
            this.AcceptButton = this.blnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 215);
            this.Controls.Add(this.blnAdd);
            this.Controls.Add(this.lblLevelHint);
            this.Controls.Add(this.numericUpDownNumLevel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbTaalich);
            this.Controls.Add(this.lblTaalich);
            this.Controls.Add(this.lblIdSubjektTik);
            this.Controls.Add(this.label6);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.Name = "FrmAddTaalichKavuaBatik";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "תהליך קבוע בנושא תיק";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblIdSubjektTik;
        private System.Windows.Forms.Label lblTaalich;
        private System.Windows.Forms.ComboBox cmbTaalich;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownNumLevel;
        private System.Windows.Forms.Label lblLevelHint;
        private System.Windows.Forms.Button blnAdd;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
