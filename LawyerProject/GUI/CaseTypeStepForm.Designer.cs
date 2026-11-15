namespace LawyerProject.GUI
{
    partial class CaseTypeStepForm
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
            this.lblCaseTypeId = new System.Windows.Forms.Label();
            this.lblProcedure = new System.Windows.Forms.Label();
            this.cmbProcedure = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudStepOrder = new System.Windows.Forms.NumericUpDown();
            this.lblLevelHint = new System.Windows.Forms.Label();
            this.btnAddStep = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nudStepOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(346, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 23);
            this.label6.TabIndex = 0;
            this.label6.AutoSize = false;
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label6.Text = "נושא תיק";
            // 
            // lblCaseTypeId
            // 
            this.lblCaseTypeId.Location = new System.Drawing.Point(90, 25);
            this.lblCaseTypeId.Name = "lblCaseTypeId";
            this.lblCaseTypeId.Size = new System.Drawing.Size(230, 23);
            this.lblCaseTypeId.TabIndex = 1;
            this.lblCaseTypeId.AutoSize = false;
            this.lblCaseTypeId.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblCaseTypeId.Text = "-";
            this.lblCaseTypeId.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            // 
            // lblProcedure
            // 
            this.lblProcedure.Location = new System.Drawing.Point(356, 70);
            this.lblProcedure.Name = "lblProcedure";
            this.lblProcedure.Size = new System.Drawing.Size(94, 23);
            this.lblProcedure.TabIndex = 2;
            this.lblProcedure.AutoSize = false;
            this.lblProcedure.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblProcedure.Text = "תהליך *";
            // 
            // cmbProcedure
            // 
            this.cmbProcedure.Location = new System.Drawing.Point(90, 67);
            this.cmbProcedure.Name = "cmbProcedure";
            this.cmbProcedure.Size = new System.Drawing.Size(230, 27);
            this.cmbProcedure.TabIndex = 3;
            this.cmbProcedure.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(326, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 23);
            this.label3.TabIndex = 4;
            this.label3.AutoSize = false;
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label3.Text = "מספר שלב *";
            // 
            // nudStepOrder
            // 
            this.nudStepOrder.Location = new System.Drawing.Point(240, 112);
            this.nudStepOrder.Name = "nudStepOrder";
            this.nudStepOrder.Size = new System.Drawing.Size(80, 27);
            this.nudStepOrder.TabIndex = 5;
            this.nudStepOrder.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudStepOrder.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudStepOrder.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblLevelHint
            // 
            this.lblLevelHint.Location = new System.Drawing.Point(66, 115);
            this.lblLevelHint.Name = "lblLevelHint";
            this.lblLevelHint.Size = new System.Drawing.Size(164, 23);
            this.lblLevelHint.TabIndex = 6;
            this.lblLevelHint.AutoSize = false;
            this.lblLevelHint.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblLevelHint.Text = "לפי סדר הביצוע";
            this.lblLevelHint.ForeColor = System.Drawing.SystemColors.GrayText;
            // 
            // btnAddStep
            // 
            this.btnAddStep.Location = new System.Drawing.Point(200, 160);
            this.btnAddStep.Name = "btnAddStep";
            this.btnAddStep.Size = new System.Drawing.Size(120, 38);
            this.btnAddStep.TabIndex = 7;
            this.btnAddStep.Text = "שמירה";
            this.btnAddStep.UseVisualStyleBackColor = true;
            this.btnAddStep.Click += new System.EventHandler(this.btnAddStep_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.RightToLeft = true;
            // 
            // CaseTypeStepForm
            // 
            this.AcceptButton = this.btnAddStep;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 215);
            this.Controls.Add(this.btnAddStep);
            this.Controls.Add(this.lblLevelHint);
            this.Controls.Add(this.nudStepOrder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbProcedure);
            this.Controls.Add(this.lblProcedure);
            this.Controls.Add(this.lblCaseTypeId);
            this.Controls.Add(this.label6);
            this.Font = new System.Drawing.Font("Calibri", 12F);
            this.Name = "CaseTypeStepForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "תהליך קבוע בנושא תיק";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            ((System.ComponentModel.ISupportInitialize)(this.nudStepOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblCaseTypeId;
        private System.Windows.Forms.Label lblProcedure;
        private System.Windows.Forms.ComboBox cmbProcedure;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudStepOrder;
        private System.Windows.Forms.Label lblLevelHint;
        private System.Windows.Forms.Button btnAddStep;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
