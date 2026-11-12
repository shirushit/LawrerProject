namespace LawyerProject.GUI
{
    partial class frmAddSubjektTik
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
            this.btnOK = new System.Windows.Forms.Button();
            this.txtNameSugTik = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSharTircha = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAgra = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.listView1 = new System.Windows.Forms.ListView();
            this.TalicimBatik = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.BtnDeletSunjekt = new System.Windows.Forms.Button();
            this.BtnUpDateSubjekt = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnOK.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnOK.Location = new System.Drawing.Point(345, 376);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(101, 34);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "אישור";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // txtNameSugTik
            // 
            this.txtNameSugTik.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.txtNameSugTik.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtNameSugTik.Location = new System.Drawing.Point(294, 139);
            this.txtNameSugTik.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtNameSugTik.Name = "txtNameSugTik";
            this.txtNameSugTik.Size = new System.Drawing.Size(132, 27);
            this.txtNameSugTik.TabIndex = 8;
            this.txtNameSugTik.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNameSugTik_KeyPress);
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCode.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblCode.Location = new System.Drawing.Point(426, 79);
            this.lblCode.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(0, 19);
            this.lblCode.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(455, 149);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 19);
            this.label2.TabIndex = 6;
            this.label2.Text = "שם סוג תיק";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(455, 79);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "קוד נושא תיק";
            // 
            // txtSharTircha
            // 
            this.txtSharTircha.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.txtSharTircha.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtSharTircha.Location = new System.Drawing.Point(294, 209);
            this.txtSharTircha.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtSharTircha.Name = "txtSharTircha";
            this.txtSharTircha.Size = new System.Drawing.Size(132, 27);
            this.txtSharTircha.TabIndex = 11;
            this.txtSharTircha.TextChanged += new System.EventHandler(this.txtSharTircha_TextChanged);
            this.txtSharTircha.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSharTircha_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(455, 219);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 19);
            this.label3.TabIndex = 10;
            this.label3.Text = "שכר טירחה";
            // 
            // txtAgra
            // 
            this.txtAgra.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.txtAgra.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtAgra.Location = new System.Drawing.Point(294, 279);
            this.txtAgra.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtAgra.Name = "txtAgra";
            this.txtAgra.Size = new System.Drawing.Size(132, 27);
            this.txtAgra.TabIndex = 15;
            this.txtAgra.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAgra_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label4.Location = new System.Drawing.Point(455, 289);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 19);
            this.label4.TabIndex = 12;
            this.label4.Text = "אגרה";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TalicimBatik});
            this.listView1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.LabelEdit = true;
            this.listView1.Location = new System.Drawing.Point(22, 18);
            this.listView1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listView1.Size = new System.Drawing.Size(208, 429);
            this.listView1.TabIndex = 14;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // TalicimBatik
            // 
            this.TalicimBatik.Text = "תהליכים בתיק";
            this.TalicimBatik.Width = 98;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.button1.Location = new System.Drawing.Point(190, 481);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 34);
            this.button1.TabIndex = 16;
            this.button1.Text = "הוספת תהליך";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnDeletSunjekt
            // 
            this.BtnDeletSunjekt.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BtnDeletSunjekt.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnDeletSunjekt.Location = new System.Drawing.Point(22, 481);
            this.BtnDeletSunjekt.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BtnDeletSunjekt.Name = "BtnDeletSunjekt";
            this.BtnDeletSunjekt.Size = new System.Drawing.Size(74, 34);
            this.BtnDeletSunjekt.TabIndex = 99;
            this.BtnDeletSunjekt.Text = "מחיקה";
            this.BtnDeletSunjekt.UseVisualStyleBackColor = false;
            this.BtnDeletSunjekt.Click += new System.EventHandler(this.BtnDeletSunjekt_Click);
            // 
            // BtnUpDateSubjekt
            // 
            this.BtnUpDateSubjekt.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BtnUpDateSubjekt.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnUpDateSubjekt.Location = new System.Drawing.Point(106, 481);
            this.BtnUpDateSubjekt.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.BtnUpDateSubjekt.Name = "BtnUpDateSubjekt";
            this.BtnUpDateSubjekt.Size = new System.Drawing.Size(74, 34);
            this.BtnUpDateSubjekt.TabIndex = 98;
            this.BtnUpDateSubjekt.Text = "עידכון";
            this.BtnUpDateSubjekt.UseVisualStyleBackColor = false;
            this.BtnUpDateSubjekt.Click += new System.EventHandler(this.BtnUpDateSubjekt_Click);
            // 
            // frmAddSubjektTik
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(645, 550);
            this.Controls.Add(this.BtnDeletSunjekt);
            this.Controls.Add(this.BtnUpDateSubjekt);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.txtAgra);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSharTircha);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtNameSugTik);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "frmAddSubjektTik";
            this.Text = "הוספת נושאי תיק";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtNameSugTik;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSharTircha;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAgra;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader TalicimBatik;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BtnDeletSunjekt;
        private System.Windows.Forms.Button BtnUpDateSubjekt;
    }
}