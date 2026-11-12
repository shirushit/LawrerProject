namespace LawyerProject.GUI
{
    partial class frmAddBetMishpat
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
            this.lblCode = new System.Windows.Forms.Label();
            this.txtNameBetMishpat = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.listView2 = new System.Windows.Forms.ListView();
            this.IDTable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NameSubjekt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BtnDeletSunjekt = new System.Windows.Forms.Button();
            this.BtnUpDateSubjekt = new System.Windows.Forms.Button();
            this.btnAddSubjekt = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lblCode.Location = new System.Drawing.Point(446, 221);
            this.lblCode.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(0, 19);
            this.lblCode.TabIndex = 2;
            // 
            // txtNameBetMishpat
            // 
            this.txtNameBetMishpat.Location = new System.Drawing.Point(378, 282);
            this.txtNameBetMishpat.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtNameBetMishpat.Name = "txtNameBetMishpat";
            this.txtNameBetMishpat.MaxLength = 50;
            this.txtNameBetMishpat.Size = new System.Drawing.Size(132, 27);
            this.txtNameBetMishpat.TabIndex = 3;
            this.txtNameBetMishpat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNameBetMishpat_KeyPress);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // listView2
            // 
            this.listView2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IDTable,
            this.NameSubjekt});
            this.listView2.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.listView2.FullRowSelect = true;
            this.listView2.GridLines = true;
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(32, 37);
            this.listView2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.listView2.MultiSelect = false;
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(276, 505);
            this.listView2.TabIndex = 5;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // IDTable
            // 
            this.IDTable.Text = "קוד טבלה";
            this.IDTable.Width = 0;
            // 
            // NameSubjekt
            // 
            this.NameSubjekt.Text = "שם נושא";
            this.NameSubjekt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NameSubjekt.Width = 120;
            // 
            // BtnDeletSunjekt
            // 
            this.BtnDeletSunjekt.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BtnDeletSunjekt.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.BtnDeletSunjekt.Location = new System.Drawing.Point(52, 496);
            this.BtnDeletSunjekt.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BtnDeletSunjekt.Name = "BtnDeletSunjekt";
            this.BtnDeletSunjekt.Size = new System.Drawing.Size(74, 34);
            this.BtnDeletSunjekt.TabIndex = 96;
            this.BtnDeletSunjekt.Text = "מחיקה";
            this.BtnDeletSunjekt.UseVisualStyleBackColor = false;
            this.BtnDeletSunjekt.Click += new System.EventHandler(this.BtnDeletSunjekt_Click);
            // 
            // BtnUpDateSubjekt
            // 
            this.BtnUpDateSubjekt.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BtnUpDateSubjekt.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.BtnUpDateSubjekt.Location = new System.Drawing.Point(136, 496);
            this.BtnUpDateSubjekt.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BtnUpDateSubjekt.Name = "BtnUpDateSubjekt";
            this.BtnUpDateSubjekt.Size = new System.Drawing.Size(74, 34);
            this.BtnUpDateSubjekt.TabIndex = 95;
            this.BtnUpDateSubjekt.Text = "עידכון";
            this.BtnUpDateSubjekt.UseVisualStyleBackColor = false;
            // 
            // btnAddSubjekt
            // 
            this.btnAddSubjekt.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddSubjekt.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.btnAddSubjekt.Location = new System.Drawing.Point(218, 496);
            this.btnAddSubjekt.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnAddSubjekt.Name = "btnAddSubjekt";
            this.btnAddSubjekt.Size = new System.Drawing.Size(78, 34);
            this.btnAddSubjekt.TabIndex = 94;
            this.btnAddSubjekt.Text = "הוספה";
            this.btnAddSubjekt.UseVisualStyleBackColor = false;
            this.btnAddSubjekt.Click += new System.EventHandler(this.btnAddSubjekt_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(539, 232);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 19);
            this.label3.TabIndex = 97;
            this.label3.Text = "קוד בית משפט";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(538, 290);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 19);
            this.label4.TabIndex = 98;
            this.label4.Text = "שם בית משפט";
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnOK.ForeColor = System.Drawing.SystemColors.InactiveBorder;
            this.btnOK.Location = new System.Drawing.Point(468, 415);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(78, 34);
            this.btnOK.TabIndex = 100;
            this.btnOK.Text = "אישור";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmAddBetMishpat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(750, 593);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BtnDeletSunjekt);
            this.Controls.Add(this.BtnUpDateSubjekt);
            this.Controls.Add(this.btnAddSubjekt);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.txtNameBetMishpat);
            this.Controls.Add(this.lblCode);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "frmAddBetMishpat";
            this.Text = "הוספת בית משפט";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TextBox txtNameBetMishpat;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ColumnHeader IDTable;
        private System.Windows.Forms.ColumnHeader NameSubjekt;
        private System.Windows.Forms.Button BtnDeletSunjekt;
        private System.Windows.Forms.Button BtnUpDateSubjekt;
        private System.Windows.Forms.Button btnAddSubjekt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOK;
    }
}