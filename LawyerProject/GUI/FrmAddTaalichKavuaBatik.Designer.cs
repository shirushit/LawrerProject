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
            this.lblCodeTaalichKavuaBatik = new System.Windows.Forms.Label();
            this.lblIdSubjektTik = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownNumLevel = new System.Windows.Forms.NumericUpDown();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.blnAdd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCodeTaalichKavuaBatik
            // 
            this.lblCodeTaalichKavuaBatik.AutoSize = true;
            this.lblCodeTaalichKavuaBatik.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCodeTaalichKavuaBatik.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblCodeTaalichKavuaBatik.Location = new System.Drawing.Point(192, 193);
            this.lblCodeTaalichKavuaBatik.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCodeTaalichKavuaBatik.Name = "lblCodeTaalichKavuaBatik";
            this.lblCodeTaalichKavuaBatik.Size = new System.Drawing.Size(0, 19);
            this.lblCodeTaalichKavuaBatik.TabIndex = 45;
            // 
            // lblIdSubjektTik
            // 
            this.lblIdSubjektTik.AutoSize = true;
            this.lblIdSubjektTik.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblIdSubjektTik.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblIdSubjektTik.Location = new System.Drawing.Point(192, 124);
            this.lblIdSubjektTik.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblIdSubjektTik.Name = "lblIdSubjektTik";
            this.lblIdSubjektTik.Size = new System.Drawing.Size(0, 19);
            this.lblIdSubjektTik.TabIndex = 44;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.Location = new System.Drawing.Point(280, 124);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 19);
            this.label6.TabIndex = 41;
            this.label6.Text = "קוד נושא תיק";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(280, 194);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 19);
            this.label4.TabIndex = 40;
            this.label4.Text = "קוד תהליך קבוע בתיק";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(280, 265);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 19);
            this.label3.TabIndex = 39;
            this.label3.Text = "שלב תהליך";
            // 
            // numericUpDownNumLevel
            // 
            this.numericUpDownNumLevel.Location = new System.Drawing.Point(146, 262);
            this.numericUpDownNumLevel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.numericUpDownNumLevel.Name = "numericUpDownNumLevel";
            this.numericUpDownNumLevel.Size = new System.Drawing.Size(95, 27);
            this.numericUpDownNumLevel.TabIndex = 46;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // blnAdd
            // 
            this.blnAdd.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.blnAdd.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.blnAdd.Location = new System.Drawing.Point(215, 360);
            this.blnAdd.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.blnAdd.Name = "blnAdd";
            this.blnAdd.Size = new System.Drawing.Size(101, 34);
            this.blnAdd.TabIndex = 47;
            this.blnAdd.Text = "אישור";
            this.blnAdd.UseVisualStyleBackColor = false;
            this.blnAdd.Click += new System.EventHandler(this.blnAdd_Click);
            // 
            // FrmAddTaalichKavuaBatik
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(542, 528);
            this.Controls.Add(this.blnAdd);
            this.Controls.Add(this.numericUpDownNumLevel);
            this.Controls.Add(this.lblCodeTaalichKavuaBatik);
            this.Controls.Add(this.lblIdSubjektTik);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "FrmAddTaalichKavuaBatik";
            this.Text = "הוספת תהליך קבוע בתיק";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCodeTaalichKavuaBatik;
        private System.Windows.Forms.Label lblIdSubjektTik;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownNumLevel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button blnAdd;
    }
}