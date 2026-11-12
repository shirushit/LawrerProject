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
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LawyerProject.GUI
{
    public partial class frmAddTashlumimLetik : Form
    {
        DB.LawyerDBEntities1 d;
        string state;
        Tashlumim_Letik T;
        Tikim_Letipul_Orech_Din tlod;
        //פעולה בונה למצב הוספה 
        public frmAddTashlumimLetik(Tikim_Letipul_Orech_Din t)
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            state = "Add";
            T = new Tashlumim_Letik();
            lblCodTik.Text = t.IDTik.ToString();
            tlod = new Tikim_Letipul_Orech_Din();
            lblIDTashlum.Text = GetNewKey(d.Tashlumim_Letik.ToList()).ToString();
            lblCodTik.Text = GetNewKey1(d.Tikim_Letipul_Orech_Din.ToList()).ToString();
        }
        //פעולה בונה למצב עדכון
        public frmAddTashlumimLetik(int ID)
        {
            //הצגת הפקדים בטופס
            InitializeComponent();
            state = "UpDate";
            d = new DB.LawyerDBEntities1();
            T = d.Tashlumim_Letik.FirstOrDefault(x => x.IDTashlum == ID);
        }
        private double ChishuvItra()
        {
            double Itra =Convert.ToDouble(tlod.SchumLetvia) * Convert.ToDouble(tlod.AchuzRevachim) / 100+ Convert.ToDouble(tlod.Subjekt_Tik.Agra);
            if (d.Tashlumim_Letik.Any(x=>x.Tikim_Letipul_Orech_Din==tlod) )
            {
                Itra =Convert.ToDouble( d.Tashlumim_Letik.LastOrDefault(x => x.Tikim_Letipul_Orech_Din == tlod).Itra);
            }
            return Itra;
        }
        //פעולה מחזירה את הקוד של התלום לתיק 
        public int GetNewKey(List<DB.Tashlumim_Letik> l)
        {
            if (l.Count == 0)
            {
                return 1;
            }
            return Convert.ToInt32(l[l.Count - 1].IDTashlum) + 1;
        }
        //פעולה מחזירה את הקוד תיק 
        public int GetNewKey1(List<DB.Tikim_Letipul_Orech_Din> l)
        {
            if (l.Count == 0)
            {
                return 1;
            }
            return Convert.ToInt32(l[l.Count - 1].IDTik) + 1;
        }
        public void FillTextBox()
        {
            //שמירת אמצעי תשלום 
            comboBoxEmzhaeiTashlum.SelectedText = T.EmzhaeyTashlum.ToString();
            txtSum.Text = T.Schum.ToString();
            richTextBoxTeur.Text = T.Teur.ToString();
            dateTimePickerDateTashlum.Text = DateTime.Now.ToString();
            txtitra.Text = T.Itra.ToString();
            //פרטי כרטיס אשראי
            if (comboBoxEmzhaeiTashlum.SelectedIndex == 1)
            {
                maskedTextBoxNumCartis.Text = T.NumCartis.ToString();
                maskedTextBoxTokef.Text = T.Tokef.ToString();
                maskedTextBoxID.Text = T.NumTeudatZeut.ToString();
                maskedTextBoxtreedigits.Text = T.ThreeNums.ToString();
            }
            //פרטי העברה בנקאית
            if (comboBoxEmzhaeiTashlum.SelectedIndex == 2)
                txtNumAsmachta.Text = T.NumAsmachta.ToString();
            //פרטי צ'ק
            if (comboBoxEmzhaeiTashlum.SelectedIndex == 3)
            {
                txtnumaccount.Text = T.NumCheshbon.ToString();
                txtNumBank.Text = T.NumBank.ToString();
                txtnumchek.Text = T.NumChek.ToString();
                txtNumSnif.Text = T.NumSnif.ToString();
                dateTimePickerDateChek.Text = DateTime.Now.ToString();
            }
        }
        public bool check()
        {
            bool OK = true;
            errorProvider1.Clear();
            txtSum.Text = T.Schum.ToString();
            //תשלום מזומן
            if (comboBoxEmzhaeiTashlum.SelectedIndex == 0)
                comboBoxEmzhaeiTashlum.SelectedText = T.EmzhaeyTashlum.ToString();
            //פרטי כרטיס אשראי
            if (comboBoxEmzhaeiTashlum.SelectedIndex == 1)
            {
                //בדיקת מספר כרטיס אשראי תקין
                try
                {
                    //אם הכניסו מספר כרטיס 
                    if (maskedTextBoxID.Text != "")
                        //אם מספר כרטיס  תקין
                        if (Validation.IsLegalId(maskedTextBoxID.Text))
                            //שמירת מספר אשראי
                            T.NumTeudatZeut = maskedTextBoxID.Text;
                        //אם מספר אשראי לא תקין, מציג הודעת שגיאה
                        else
                            errorProvider1.SetError(maskedTextBoxID, "תעודת זהות לא תקינה");
                    // אם מספר אשראי לא הוזן, מציג שגיאה
                    else
                        errorProvider1.SetError(maskedTextBoxID, "לא הוכנס תעודת זהות");
                }
                //בכל שגיאה אחרת
                catch (Exception ex)
                {
                    //הצגת הודעת שגיאה למשתמש
                    errorProvider1.SetError(maskedTextBoxID, ex.Message);
                    //המשתנה אוקיי = שלילי
                    OK = false;
                }
                maskedTextBoxNumCartis.Text = T.NumCartis.ToString();
                maskedTextBoxTokef.Text = T.Tokef.ToString();
                //בדיקת תעודת זהות לבעל הכרטיס
                try
                {
                    //אם הכניסו תעודת זהות
                    if (maskedTextBoxID.Text != "")
                        //אם תעודת הזהות תקינה
                        if (Validation.IsLegalId(maskedTextBoxID.Text))
                                //שמירת תעודת הזהות
                                T.NumTeudatZeut = maskedTextBoxID.Text;
                        //אם התעודת זהות לא תקינה, מציג הוגעה שגיאה
                        else
                            errorProvider1.SetError(maskedTextBoxID, "תעודת זהות לא תקינה");
                    // אם התעודה לא הוזנה, מציג שגיאה
                    else
                        errorProvider1.SetError(maskedTextBoxID, "לא הוכנס תעודת זהות");
                }
                //בכל שגיאה אחרת
                catch (Exception ex)
                {
                    //הצגת הודעת שגיאה למשתמש
                    errorProvider1.SetError(maskedTextBoxID, ex.Message);
                    //המשתנה אוקיי = שלילי
                    OK = false;
                }
                maskedTextBoxtreedigits.Text = T.ThreeNums.ToString();
            }
            //פרטי העברה בנקאית
            if (comboBoxEmzhaeiTashlum.SelectedIndex == 2)
                txtNumAsmachta.Text = T.NumAsmachta.ToString();
            //פרטי צ'ק
            if (comboBoxEmzhaeiTashlum.SelectedIndex == 3)
            {
                //בדיקת מספר חשבון בנק
                try
                {
                   // אם הכניסו מספר חשבון
                    if (txtNumBank.Text != "")
                            T.NumBank = txtNumBank.Text;
                    // אם החשבון לא הוזן, מציג שגיאה
                    else
                        errorProvider1.SetError(txtNumBank, "לא הוכנס מספר בנק");
                }
                //בכל שגיאה אחרת
                catch (Exception ex)
                {
                    //הצגת הודעת שגיאה למשתמש
                    errorProvider1.SetError(txtNumBank, ex.Message);
                    OK = false;
                }
                //בדיקת מספר חשבון 
                try
                {
                    // אם הכניסו מספר חשבון
                    if (txtnumaccount.Text != "")
                        T.NumCheshbon = txtnumaccount.Text;
                    // אם החשבון לא הוזן, מציג שגיאה
                    else
                        errorProvider1.SetError(txtNumBank, "לא הוכנס מספר חשבון");
                }
                //בכל שגיאה אחרת
                catch (Exception ex)
                {
                    //הצגת הודעת שגיאה למשתמש
                    errorProvider1.SetError(txtnumaccount, ex.Message);
                    OK = false;
                }
                //בדיקת מספר סניף 
                try
                {
                    // אם הכניסו מספר סניף
                    if (txtNumSnif.Text != "")
                        T.NumCheshbon = txtNumSnif.Text;
                    // אם החשבון לא הוזן, מציג שגיאה
                    else
                        errorProvider1.SetError(txtNumSnif, "לא הוכנס מספר סניף");
                }
                //בכל שגיאה אחרת
                catch (Exception ex)
                {
                    //הצגת הודעת שגיאה למשתמש
                    errorProvider1.SetError(txtNumSnif, ex.Message);
                    OK = false;
                }
                //שמירת התאריך לצ'ק  
                dateTimePickerDateChek.Text = DateTime.Now.ToString();
            }
            //בדיקה לסכום לתשלום
            try
            {
                // אם הכניסו סכום
                if (Convert.ToDecimal( txtSum.Text) != 0)
                    T.Schum =Convert.ToDecimal( txtSum.Text);
                // אם סכום לא הוזן, מציג שגיאה
                else
                    errorProvider1.SetError(txtSum, "לא הוכנס סכום לתשלום");
            }
            //בכל שגיאה אחרת
            catch (Exception ex)
            {
                //הצגת הודעת שגיאה למשתמש
                errorProvider1.SetError(txtSum, ex.Message);
                OK = false;
            }
            //בדיקה לתיאור תשלום
            try
            {
                // אם הכניסו תאור
                if (richTextBoxTeur.Text != " ")
                    T.Teur = richTextBoxTeur.Text;
                // אם תאור לא הוזן, מציג שגיאה
                else
                    errorProvider1.SetError(richTextBoxTeur, "לא הוכנס תאור לתשלום");
            }
            //בכל שגיאה אחרת
            catch (Exception ex)
            {
                //הצגת הודעת שגיאה למשתמש
                errorProvider1.SetError(richTextBoxTeur, ex.Message);
                OK = false;
            }
            T.IDTashlum = tlod.IDTik;
            T.IDTik = Convert.ToInt32( lblCodTik.Text);    
           //שמירת היתרה לתשלום
            try
            {
             T.Itra=Convert.ToDecimal( Convert.ToDouble( txtitra.Text) - Convert.ToDouble( txtSum.Text) ) ;
            }
            catch (Exception ex)
            {
                //הצגת הודעת שגיאה למשתמש
                errorProvider1.SetError(txtitra, ex.Message);
                //המשתנה אוקיי = שלילי
                OK = false;
            }
            //שמירת קובץ
            return OK;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (check())
            {
                if (state == "Add")
                {
                    try
                    {
                        d.Tashlumim_Letik.Add(T);
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
        //פעולה שמציגה אמצעי תשלום מתאימים לפי בחירת המשתמש
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //אתחול האמצאי תשלום
            groupBoxCartisAshray.Visible = false;
            groupBoxChek.Visible = false;
            groupBoxAavaraBankait.Visible = false;
            if (comboBoxEmzhaeiTashlum.Text == "כרטיס אשראי")
            {
                groupBoxCartisAshray.Visible = true;
                groupBoxChek.Visible = false;
                groupBoxAavaraBankait.Visible = false;
            }
            if (comboBoxEmzhaeiTashlum.Text == "מזומן")
            {
                groupBoxChek.Visible = true;
                groupBoxCartisAshray.Visible = false;
                groupBoxAavaraBankait.Visible = false;
            }
            if (comboBoxEmzhaeiTashlum.Text == "העברה בנקאית")
            {
                groupBoxAavaraBankait.Visible = true;
                groupBoxChek.Visible = false;
                groupBoxCartisAshray.Visible = false;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //מחשבת את היתרה שנשארה
            txtitra.Text = (Convert.ToDouble(txtitra.Text) - Convert.ToDouble(txtSum.Text)).ToString();
        }
        //בדיקת תקינות לפקדים מסוג טקסט. מאפשר רק מספרים
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumber(e.KeyChar.ToString());
        }
        //בדיקת תקינות לפקדים מסוג טקסט. מאפשר רק מספרים
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumber(e.KeyChar.ToString());
        }
        //בדיקת תקינות לפקדים מסוג טקסט. מאפשר רק מספרים
        private void txtnumchek_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumber(e.KeyChar.ToString());
        }
        //בדיקת תקינות לפקדים מסוג טקסט. מאפשר רק מספרים
        private void txtnumaccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumber(e.KeyChar.ToString());
        }
        //פתיחת טופס הדפסת חשבונית
        private void btnAddKabala_Click(object sender, EventArgs e)
        {
            
        }

        private void txtnumchek_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumber(e.KeyChar.ToString());
        }

        private void txtNumBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumber(e.KeyChar.ToString());
        }

        private void txtNumSnif_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumber(e.KeyChar.ToString());
        }

        private void txtnumaccount_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumber(e.KeyChar.ToString());
        }

        private void txtNumAsmachta_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumber(e.KeyChar.ToString());
        }

        private void txtSum_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumber(e.KeyChar.ToString());
        }

        private void frmAddTashlumimLetik_Load(object sender, EventArgs e)
        {

        }
    }
}