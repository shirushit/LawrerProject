
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using LawyerProject.BLL;
using LawyerProject.DB;
using LawyerProject.GUI;

namespace LawyerProject.GUI
{
    public partial class FrmAddTaalichimLetik : Form
    {
        DB.LawyerDBEntities1 d;
        string state;
        Taalichim_Letikim t;
        List<Taalichim_Kvuim_Batik> taalichim_kvuim = new List<Taalichim_Kvuim_Batik>();
        public FrmAddTaalichimLetik()
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            state = "Add";
            t = new Taalichim_Letikim();
            lblCode.Text = GetNewKey(d.Taalichim_Letikim.ToList()).ToString();
        }
        public FrmAddTaalichimLetik(int code)
        {
            InitializeComponent();
            state = "UpDate";
            d = new DB.LawyerDBEntities1();
            t = d.Taalichim_Letikim.FirstOrDefault(x => x.IDTaalich == code);
            FillTextBox();
       
        }
       //חיפוש קוד תיק 
        public int GetNewKey(List<DB.Taalichim_Letikim> l)
        {
            if (l.Count == 0)
            {
                return 1;
            }
            return Convert.ToInt32(l[l.Count - 1].IDTaalich) + 1;
        }
        public void FillTextBox()
        {
            lblCode.Text = t.IDTaalich.ToString();
            txtNameTaalich.Text = t.NameTaalich.ToString();
            txtSugTaalich.Text = t.SugTaalich.ToString();
            numericUpDown1.Value = Convert.ToInt32(t.MeshechZmanLetaalich);
        }
        public bool check()
        {
            bool OK = true;
            errorProvider1.Clear();
            t.IDTaalich = Convert.ToInt32(lblCode.Text);
            //בקידת תקינות להוספת תהליך לתיק
            try
            {
                //אם הכניסו שם תהליך
                if (txtNameTaalich.Text != "")
                    //אם השם תהליך לא קיים כבר במערכת
                    if (d.Taalichim_Letikim.ToList().FirstOrDefault(x => x.NameTaalich == txtNameTaalich.Text) == null)
                        //תשמור את התהליך
                        t.NameTaalich = txtNameTaalich.Text;
                    else
                    {
                        errorProvider1.SetError(txtNameTaalich, "תהליך לתיק כבר קיים במערכת");
                        OK = false;
                    }
                else
                {
                    errorProvider1.SetError(txtNameTaalich, "לא הוכנס תהליך לתיק");
                    OK = false;
                }
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(txtNameTaalich, ex.Message);
                OK = false;
            }

            try
            {
                //שמירת משך זמן משוער לתהליך
                    t.MeshechZmanLetaalich = numericUpDown1.Value.ToString(); 
            }
            //בכל מקרה אחר
            catch (Exception ex)
            {
                errorProvider1.SetError(numericUpDown1, ex.Message);
                OK = false;
            }
            //סוג תהליך
            //עוד חודש לדבר עם המורה ולשאול אותה מה לעשות עם הסוג תההליך ואיזה פקד מתאים לשים לה
            try
            {
                if (txtSugTaalich.Text != "")
                    t.SugTaalich = txtSugTaalich.Text.ToString();
                else
                {
                    errorProvider1.SetError(txtSugTaalich, "לא הוכנס סוג תהליך");
                    OK = false;
                }

            }
            //בכל מקרה אחר
            catch (Exception ex)
            {
                errorProvider1.SetError(txtSugTaalich, ex.Message);
                OK = false;
            }
            return OK;
        }
        private void btnOK_Click_1(object sender, EventArgs e)
        {
            if (check())
            {
                if (state == "Add")
                {
                    try
                    {
                        d.Taalichim_Letikim.Add(t);
                        d.SaveChanges();
                        MessageBox.Show("נשמר בהצלחה");
                        this.Close();
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("שגיאה! לא נשמר");
                    }

                }
                if (state == "UpDate")
                {
                    try
                    {
                        d.SaveChanges();
                        MessageBox.Show(" עודכן בהצלחה");
                        this.Close();
                    }
                    catch (Exception /*el*/)
                    {
                        MessageBox.Show("שגיאה! לא עודכן");
                    }
                }
            }
            else
            {
                MessageBox.Show("שגיאה! לא עודכן");
            }
        } 
        private void btnOK_Click(object sender, EventArgs e)
        {
            frmAddTaalichLetik frmAddTaalichLetik = new frmAddTaalichLetik();
            frmAddTaalichLetik.ShowDialog();
        }

    }
}
