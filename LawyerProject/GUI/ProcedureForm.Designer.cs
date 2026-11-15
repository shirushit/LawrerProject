namespace LawyerProject.GUI
{
    partial class ProcedureForm
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
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtProcedureName = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblProcedureId = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.nudDurationDays = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDurationDays)).BeginInit();
            this.SuspendLayout();
            // 
            // txtCategory
            // 
            this.txtCategory.Location = new System.Drawing.Point(40, 311);
            this.txtCategory.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.MaxLength = 50;
            this.txtCategory.Size = new System.Drawing.Size(219, 27);
            this.txtCategory.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(288, 322);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 19);
            this.label4.TabIndex = 34;
            this.label4.Text = "סוג תהליך";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(288, 251);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 19);
            this.label3.TabIndex = 32;
            this.label3.Text = "משך זמן משוער בימים";
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnOK.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnOK.Location = new System.Drawing.Point(222, 403);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(101, 34);
            this.btnOK.TabIndex = 31;
            this.btnOK.Text = "אישור";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtProcedureName
            // 
            this.txtProcedureName.Location = new System.Drawing.Point(40, 171);
            this.txtProcedureName.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtProcedureName.Name = "txtProcedureName";
            this.txtProcedureName.MaxLength = 50;
            this.txtProcedureName.Size = new System.Drawing.Size(219, 27);
            this.txtProcedureName.TabIndex = 30;
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblCode.Location = new System.Drawing.Point(259, 111);
            this.lblCode.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(0, 19);
            this.lblCode.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(288, 181);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 19);
            this.label2.TabIndex = 28;
            this.label2.Text = "שם תהליך";
            // 
            // lblProcedureId
            // 
            this.lblProcedureId.AutoSize = true;
            this.lblProcedureId.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblProcedureId.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblProcedureId.Location = new System.Drawing.Point(288, 111);
            this.lblProcedureId.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblProcedureId.Name = "lblProcedureId";
            this.lblProcedureId.Size = new System.Drawing.Size(70, 19);
            this.lblProcedureId.TabIndex = 27;
            this.lblProcedureId.Text = "קוד תהליך";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // nudDurationDays
            // 
            this.nudDurationDays.Location = new System.Drawing.Point(127, 248);
            this.nudDurationDays.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.nudDurationDays.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudDurationDays.Name = "nudDurationDays";
            this.nudDurationDays.Size = new System.Drawing.Size(134, 27);
            this.nudDurationDays.TabIndex = 36;
          //  this.nudDurationDays.ValueChanged += new System.EventHandler(this.nudDurationDays_ValueChanged);
            // 
            // ProcedureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(569, 507);
            this.Controls.Add(this.nudDurationDays);
            this.Controls.Add(this.txtCategory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtProcedureName);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblProcedureId);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "ProcedureForm";
            this.Text = "הוספת תהליך ";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDurationDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtProcedureName;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblProcedureId;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.NumericUpDown nudDurationDays;
    }
}