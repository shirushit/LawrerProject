using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LawyerProject.DB;
using LawyerProject.BLL;
using LawyerProject.GUI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LawyerProject.GUI
{
    public partial class frmAddSubjektTik : Form
    {
        //הגדרת משתנים גלובליים
        DB.LawyerDBEntities1 d;
        string state;
        Subjekt_Tik s;
        List<Taalichim_Kvuim_Batik> taalichim_kvuim = new List<Taalichim_Kvuim_Batik> ();
        //פפעולה בונה 
        public frmAddSubjektTik()
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            state = "Add";
            s = new Subjekt_Tik();
            lblCode.Text = GetNewKey(d.Subjekt_Tik.ToList()).ToString();
        }
        //פעולה בונה למצב עדכון
        public frmAddSubjektTik(int code)
        {
            InitializeComponent();
            state = "UpDate";
            d = new DB.LawyerDBEntities1();
            s = d.Subjekt_Tik.FirstOrDefault(x => x.IDSugTik == code);
            FillTextBox(); 
            taalichim_kvuim = d.Taalichim_Kvuim_Batik.ToList();
        }
       public void filltaalichimKvuim()
        {
            for(int i = 0; i<taalichim_kvuim.Count; i++)
            {
                ListViewItem iten = new ListViewItem(new string[] { taalichim_kvuim[i].Taalichim_Letikim.NameTaalich});
                listView1.Items.Add(iten);
            }
        }
        public int GetNewKey(List<DB.Subjekt_Tik> l)
        {
            if (l.Count == 0)
            {
                return 1;
            }
            return Convert.ToInt32(l[l.Count - 1].IDSugTik) + 1;
        }
        public void FillTextBox()
        {
            lblCode.Text = s.IDSugTik.ToString();
            txtNameSugTik.Text = s.NameSugTik;
            txtSharTircha.Text = s.ScharTircha.ToString();
            txtAgra.Text = s.Agra.ToString();
        }
        public bool check()
        {
            bool OK = true;
            errorProvider1.Clear();
            s.IDSugTik = Convert.ToInt32(lblCode.Text);  
            //בקידת תקינות להוספת נושא תיק
            try
            {
                //אם הכניסו נושא תיק
                if (txtNameSugTik.Text != "")
                    //אם הנושא תיק לא קיים כבר במערכת
                    if (d.Subjekt_Tik.ToList().FirstOrDefault(x=> x.NameSugTik == txtNameSugTik.Text) == null)
                    //תשמור את הנושא תיק
                    s.NameSugTik = txtNameSugTik.Text;
                     else
                    {
                    errorProvider1.SetError(txtNameSugTik, "סוג תיק כבר קיים במערכת");
                    OK = false;
                     }
                else
                {
                    errorProvider1.SetError(txtNameSugTik, "לא הוכנס סוג תיק");
                    OK = false;
                }
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(txtNameSugTik, ex.Message);
                OK = false;
            }
            try
            {
                //אם הכניסו שכר טרחה
                if (txtSharTircha.Text != "")
                   s.ScharTircha =Convert.ToDecimal( txtSharTircha.Text);
                // אם לא הוכנס שכר טרחה, מציג שגיאה
                else
                {
                   errorProvider1.SetError(txtSharTircha, "לא הוכנס שכר טרחה");
                    OK = false;
                }       
            }
            //בכל מקרה אחר
            catch (Exception ex)
            {
                errorProvider1.SetError(txtSharTircha, ex.Message);
                OK = false;
            }
            //אגרה
            try
            {
                if (txtAgra.Text != "")
                    s.Agra = Convert.ToDecimal( txtAgra.Text);
                else
                {
                    errorProvider1.SetError(txtAgra, "לא הוכנס אגרה");
                    OK = false;
                }      
            }
            //בכל מקרה אחר
            catch (Exception ex)
            {
                errorProvider1.SetError(txtSharTircha, ex.Message);
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
                        d.Subjekt_Tik.Add(s);
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
        private void txtAgra_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.bdikat_mispar(e.KeyChar);
        }

        private void txtSharTircha_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumber(e.KeyChar.ToString());
        }
        private void txtNameSugTik_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.bdikat_text(e.KeyChar);
        }
        //פעולה לכפתור הוספה
        private void button1_Click(object sender, EventArgs e)
        {
            GUI.FrmAddTaalichKavuaBatik t = new GUI.FrmAddTaalichKavuaBatik();
            t.ShowDialog();
            filltaalichimKvuim();
        }
        //פעולה לכפתור עדכון
        private void BtnUpDateSubjekt_Click(object sender, EventArgs e)
        {
            string ID = listView1.SelectedItems[0].SubItems[0].Text;
           // string ID = listView1.SelectedItems[0].Cells[0].Value.ToString();
            GUI.FrmAddTaalichKavuaBatik t = new GUI.FrmAddTaalichKavuaBatik(Convert.ToInt32( ID));
            t.ShowDialog();
            filltaalichimKvuim();
        }

        private void txtSharTircha_TextChanged(object sender, EventArgs e)
        {

        }
//פעולה לכפתור מחיקת תהליך בתיק
        private void BtnDeletSunjekt_Click(object sender, EventArgs e)
        {
        }
    }
}
