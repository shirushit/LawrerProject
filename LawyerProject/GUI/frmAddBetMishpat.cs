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
    public partial class frmAddBetMishpat : Form
    {
      //הגדרת משתנים גלובליים
      DB.LawyerDBEntities1 d;
      string state;
      Bet_Mishpat b;
      List<Subjekts_Shebet_Mishpat_Metapel> subjekt = new List<Subjekts_Shebet_Mishpat_Metapel>();
        //פעולה בונה
        public frmAddBetMishpat()
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            state = "Add";
           b = new Bet_Mishpat();
           lblCode.Text = GetNewKey(d.Bet_Mishpat.ToList()).ToString();
        }
       //פעולה בונה למצב עדכון
        public frmAddBetMishpat(int code)
        {
            InitializeComponent();
            state = "UpDate";
            d = new DB.LawyerDBEntities1();
            b = d.Bet_Mishpat.FirstOrDefault(x => x.IDBetMishpat == code);
            FillTextBox();
            subjekt = d.Subjekts_Shebet_Mishpat_Metapel.ToList();
        }
        //פעולה שאחראית למלא את הנושאים ששיכים לכל תיק
        public void filllistView()
        {
            //לולאה  שעוברת על כל הנושאים ועבוד כל נושא מייצרת שורה בליסט שבתצוגה
            for (int i = 0; i < subjekt.Count; i++)
            {
                //בניית שורה בודדה בליטס
                ListViewItem item = new ListViewItem(new string[] { subjekt[i].Subjekt_Tik.NameSugTik });
                //הוספת השורה לפקד בטופס
                listView2.Items.Add(item);
            }
        }
        //פעולה שמחזירה את הקוד לבית משפט
        public int GetNewKey(List<DB.Bet_Mishpat> l)
        {
            if (l.Count == 0)
            {
                return 1;
            }
            return Convert.ToInt32(l[l.Count - 1].IDBetMishpat) + 1;
        }
        //פעולה שממלאה את הנתונים לטופס במצב עדכון
        public void FillTextBox()
        {
            lblCode. Text = b.IDBetMishpat.ToString();
            txtNameBetMishpat.Text = b.NameBetMishpat;
        }
        //פעולה שבודקת האם הנתונים תקינים
        public bool check()
        {
            bool OK = true;
            errorProvider1.Clear();
            try
            {
                b.IDBetMishpat = Convert.ToInt32(lblCode.Text);
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(lblCode, ex.Message);
                OK = false;
            }
            try
            {  if (txtNameBetMishpat.Text != "")
                     b.NameBetMishpat = txtNameBetMishpat.Text;
                else
                    errorProvider1.SetError(txtNameBetMishpat,"לא הוזן שם בית משפט");   
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(txtNameBetMishpat, ex.Message);
                OK = false;
            }
            return OK;
        }
        //פעולה של הכפתור אישור
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (check())
            {
                if (state == "Add")
                {
                    try
                    {
                        d.Bet_Mishpat.Add(b);
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
                    catch (Exception)
                    {

                        MessageBox.Show("שגיאה! לא עודכן");
                    }
                }
            }
        }
        //פעולה שבודקת שהטקסט שמוקש לפקד הוא רק אותיות
        private void txtNameBetMishpat_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.bdikat_text(e.KeyChar);
        }
        //פעולה לכפתור הוספה
        private void btnAddSubjekt_Click(object sender, EventArgs e)
        {
            GUI.frmAddSubjektShebetMishpatMetapel s = new frmAddSubjektShebetMishpatMetapel(b);
            s.ShowDialog();
            listView2.Items.Clear();
            filllistView();
        }
        //פעולה לכפתור עדכון
        private void BtnUpDateSubjekt_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((listView2.SelectedItems[0].SubItems[0].Text).ToString());
            GUI.frmAddSubjektShebetMishpatMetapel gg = new frmAddSubjektShebetMishpatMetapel(id);
            gg.ShowDialog();
        }
        //פעולת מחיקת נושא תיק
        private void BtnDeletSunjekt_Click(object sender, EventArgs e)
        {
            
            
        }
    }
}
