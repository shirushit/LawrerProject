using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;
using LawyerProject.GUI;

namespace LawyerProject.GUI
{
    public partial class FrmAddTaalichKavuaBatik : Form
    {
        //הגדרת משתנים גלובלים
        DB.LawyerDBEntities1 d;
        string state;
        Taalichim_Kvuim_Batik T;
        //פעולה בונה
        public FrmAddTaalichKavuaBatik()
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            state = "Add";
            T = new Taalichim_Kvuim_Batik();
            //הצגת קוד התהליך בתיק 
            //lblCodeTaalichKavuaBatik.Text = GetNewKey1(T.IDTaalichKavua.ToList()).ToString();
            //הצגת הקוד תהליך נןשא תיק
           // lblIdSubjektTik.Text = GetNewKey(T.IDSubjektTik.ToList()).ToString();
        }
        //פעולה בונה למצב עדכון
        public FrmAddTaalichKavuaBatik(int code)
        {
            InitializeComponent();
            state = "UpDate";
            d = new DB.LawyerDBEntities1();
            T = d.Taalichim_Kvuim_Batik.FirstOrDefault(x => x.IDTaalichKavua == code);
            FillTextBox();
        }
        //פעולה שממלאה את הנתונים למצב עדכון
        public void FillTextBox()
        {
            lblCodeTaalichKavuaBatik.Text = T.IDTaalichKavua.ToString();
            lblIdSubjektTik.Text = T.IDSubjektTik.ToString();
            numericUpDownNumLevel.Text = T.NumLevel.ToString();
        }
        //פעולה שמחזירה את הקוד האוטומטי  קוד תהליך קבוע בתיק
        public int GetNewKey1(List<DB.Taalichim_Kvuim_Batik> l)
        {
            if (l.Count == 0)
            {
                return 1;
            }
            return Convert.ToInt32(l[l.Count - 1].IDSubjektTik) + 1;
        }
        //פעולה שמחזירה את הקוד האוטומטי  קוד נושא תיק
        public int GetNewKey(List<DB.Taalichim_Kvuim_Batik> l)
        {
            if (l.Count == 0)
            {
                return 1;
            }
            return Convert.ToInt32(l[l.Count - 1].IDSubjektTik) + 1;
        }
        //פעולה ששומרת את כל הנתונים
        public bool check()
        {
            bool ok = true;
            errorProvider1.Clear();
            T.IDSubjektTik = Convert.ToInt32(lblIdSubjektTik.Text);
            T.IDTaalichKavua = Convert.ToInt32(lblCodeTaalichKavuaBatik.Text);
            T.NumLevel = Convert.ToInt32(numericUpDownNumLevel.Text);
            return ok;
        }
        //פעולה שמוסיפה א הנתונים לתהליכים הקבועים בתיק
        private void blnAdd_Click(object sender, EventArgs e)
        {
            //אם הפעולה הצליחה לשמור את הנתונים
            if (check())
            {
                //אם הטופס במצב הוספה ולא עדכון
                if (state == "Add")
                {
                    //מנסה להוסיף את הנתונים לטבלת תהליכים קבועים בתיק
                    try
                    {
                        d.Taalichim_Kvuim_Batik.Add(T);
                        d.SaveChanges();
                        MessageBox.Show(" נשמר בהצלחה");
                        this.Close();
                    }
                    //אם לא, צציג הודעת שגיאה
                    catch (Exception)
                    {
                        MessageBox.Show(" לא נשמר");
                    }
                }
                //אם הטופס במצב הוספה
                if (state == "Update")
                {
                    //תנסה לשמור את הנתונים
                    try
                    {
                        d.SaveChanges();
                        MessageBox.Show(" עודכן בהצלחה");
                        this.Close();
                    }
                    //אם לא, מציג הודעת שגיאה
                    catch (Exception e1)
                    {
                        MessageBox.Show(" לא נשמר");
                    }

                }
            }
        }
    }
}
