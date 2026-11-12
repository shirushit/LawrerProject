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
    public partial class FrmUpDateTaalichToTikLeOrechDin : Form
    {
        //הגדרת משתנים גלובלים
        DB.LawyerDBEntities d;
        Tahalich_Tipul_Betik T;
        //פעולה בונה

        //פעולה בונה למצב עדכון תהליך
        public FrmUpDateTaalichToTikLeOrechDin(int code)
        {
            InitializeComponent();
            d = new LawyerDBEntities();
            dateTimePicker.MaxDate = DateTime.Now;
            dateTimePicker.Value = DateTime.Now;
            T = d.Tahalich_Tipul_Betik.FirstOrDefault(x => x.IDTahalichTipulLetik == code);
            fillTaalichimKvuimBatik();
        }
        public void fillTaalichimKvuimBatik()
        {
            //lblCodeTaalichKavuaBatik.Text = T.IDTaalichKavua.ToString();
            //lblIdSubjektTik.Text = T.IDSubjektTik.ToString();
            //numericUpDownNumLevel.Value = T.NumLevel;
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
            //errorProvider1.Clear();
            //T.IDSubjektTik = Convert.ToInt32(lblIdSubjektTik.Text);
            //T.IDTaalichKavua = Convert.ToInt32(lblCodeTaalichKavuaBatik.Text);
            //T.NumLevel = Convert.ToInt32(numericUpDownNumLevel.Text);
            return ok;
        }
        //פעולה שמוסיפה א הנתונים לתהליכים הקבועים בתיק
        private void blnAdd_Click(object sender, EventArgs e)
        {
            //אם הפעולה הצליחה לשמור את הנתונים
            if (check())
            {

                //אם הטופס במצב הוספה

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
