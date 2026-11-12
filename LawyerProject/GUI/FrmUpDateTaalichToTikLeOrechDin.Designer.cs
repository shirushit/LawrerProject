namespace LawyerProject.GUI
{
    partial class FrmUpDateTaalichToTikLeOrechDin
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
            this.lblNameTaalich = new System.Windows.Forms.Label();
            this.blnAdd = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // lblNameTaalich
            // 
            this.lblNameTaalich.AutoSize = true;
            this.lblNameTaalich.Location = new System.Drawing.Point(128, 84);
            this.lblNameTaalich.Name = "lblNameTaalich";
            this.lblNameTaalich.Size = new System.Drawing.Size(0, 13);
            this.lblNameTaalich.TabIndex = 51;
            // 
            // blnAdd
            // 
            this.blnAdd.Location = new System.Drawing.Point(169, 232);
            this.blnAdd.Name = "blnAdd";
            this.blnAdd.Size = new System.Drawing.Size(75, 23);
            this.blnAdd.TabIndex = 50;
            this.blnAdd.Text = "שמור";
            this.blnAdd.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(194, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 49;
            this.label6.Text = "שם תהליך";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(306, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "תאריך ביצוע";
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(68, 148);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker.TabIndex = 52;
            // 
            // FrmUpDateTaalichToTikLeOrechDin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 336);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.lblNameTaalich);
            this.Controls.Add(this.blnAdd);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Name = "FrmUpDateTaalichToTikLeOrechDin";
            this.Text = "עדכון תהליך לתיק לעורך דין";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNameTaalich;
        private System.Windows.Forms.Button blnAdd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
    }
}