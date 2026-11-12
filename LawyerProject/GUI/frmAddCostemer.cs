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
using LawyerProject.GUI;
using LawyerProject.BLL;
   

namespace LawyerProject.GUI
{
    public partial class frmAddCostemer : Form
    {
        DB.LawyerDBEntities1 d;
        string state;
        Customers C;
        public frmAddCostemer()
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            state = "Add";
            C = new Customers();
        }
        public frmAddCostemer(string ID)
        {
            //הצגת הפקדים בטופס
            InitializeComponent();
            state = "UpDate";
            d = new DB.LawyerDBEntities1();
            C = d.Customers.FirstOrDefault(x => x.IDcustemer == ID);
        }
        public void FillTextBox()
        {
            txtIDcostemer1.Text = C.IDcustemer.ToString();
            txtName1.Text = C.FirstName.ToString();
            txtLastName1.Text = C.LastName.ToString();
            txtAdress1.Text = C.Adress.ToString();
            maskTWOPhone1.Text = C.Telephone.ToString();
            maskTWOPhone1.Text = C.SmartPhone.ToString();
            txtMail1.Text = C.Mail.ToString();
            mackBirthDay1.Text = C.DateBirthday.ToString();
        }
       public bool check()
        {
            bool OK = true;
            errorProvider1.Clear();
            try
            {
                //אם הכניסו תעודת זהות
                if (txtIDcostemer1.Text != "")
                    if ((d.Customers.ToList().FirstOrDefault(x => x.IDcustemer == txtIDcostemer1.Text) == null))
                        //אם תעודת הזהות תקינה
                        if (Validation.IsLegalId(txtIDcostemer1.Text))
                            //שמירת תעודת הזהות
                            C.IDcustemer = txtIDcostemer1.Text;
                        //אם התעודת זהות לא תקינה, מציג הוגעה שגיאה
                        else
                            errorProvider1.SetError(txtIDcostemer1, "תעודת זהות לא תקינה");
                   //אם תעודת הזהות כבר קיימת
                    else
                        errorProvider1.SetError(txtIDcostemer1, "תעודת זהות קיימת במערכת");
         //אם תעודת זהות לא הוכנסה
                else
                    errorProvider1.SetError(txtIDcostemer1, "לא הוכנס תעודת זהות");

                if (d.Customers.ToList().FirstOrDefault(x => x.IDcustemer == txtIDcostemer1.Text) == null)
                    C.IDcustemer = txtIDcostemer1.Text;
                else
                {
                    errorProvider1.SetError(txtIDcostemer1, "לקוח קיים במערכת");
                    OK = false;
                }
            }
           //בכל שגיאה אחרת
            catch (Exception ex)
            {
                //הצגת הודעת שגיאה למשתמש
                errorProvider1.SetError(txtIDcostemer1, ex.Message);
                //המשתנה אוקיי = שלילי
                OK = false;
            }
            try
            {
                //אם הכניסו שם פרטי
                if (txtName1.Text != "")
                    C.FirstName = txtName1.Text;
                else
                    errorProvider1.SetError(txtName1, "לא הוכנס שם פרטי");

            }
            //במקרה ולא הכניסו
            catch (Exception ex)
            {
                //הצגת הודעה למשתמש
                errorProvider1.SetError(txtName1, ex.Message);
                OK = false;
            }
            try
            {
                //אם הכניסו שם משפחה
                if (txtLastName1.Text != "")
                    C.LastName = txtLastName1.Text;
                else
                    errorProvider1.SetError(txtLastName1, "לא הוכנס שם משפחה");
            }
            //במקרה אחר
            catch (Exception ex)
            {
                errorProvider1.SetError(txtLastName1, ex.Message);
                OK = false;
            }
            try
            {//אם הכניסו כתובת
                if (txtAdress1.Text != "")
                    C.Adress = txtAdress1.Text;
                else
                    errorProvider1.SetError(txtAdress1, "לא הוכנס כתובת");
            }
            //במקרה אחר
            catch (Exception ex)
            {
                errorProvider1.SetError(txtAdress1, ex.Message);
                OK = false;
            }
            try
            {
                //אם הכניסו מספר טלפון
                if (maskONEPhone1.Text != "")
                    //אם מספר הטלפון או נייד או נייח תקין באחד מהם
                    if (Validation.IsTelephone(maskONEPhone1.Text)|| Validation.IsCellPhone(maskONEPhone1.Text))
                        //שמירת מספר הטלפון
                        C.Telephone = maskONEPhone1.Text;
                    //אם מספר הטלפון לא תקין, מציג הודעה שגיאה
                    else
                        errorProvider1.SetError(maskONEPhone1, "טלפון לא תקין");
                // אם המספר לא הוכנס, מציג שגיאה
                else
                    errorProvider1.SetError(maskONEPhone1, "לא הוכנס טלפון" ); 
            }
            //בכל מקרה אחר
            catch (Exception ex)
            {
                errorProvider1.SetError(maskONEPhone1, ex.Message);
                OK = false;
            }
            try
            {
                //אם הכניסו מספר טלפון
               if (maskTWOPhone1.Text != "")
                    //אם מספר הטלפון או נייד או נייח תקין באחד מהם
                    if (Validation.IsTelephone(maskTWOPhone1.Text) || Validation.IsCellPhone(maskTWOPhone1.Text))
                        //שמירת מספר הטלפון
                      C.SmartPhone = maskTWOPhone1.Text;
                    //אם מספר הטלפון לא תקין, מציג הודעה שגיאה
                    else
                        errorProvider1.SetError(maskTWOPhone1, "טלפון לא תקין");
                // אם לא הוכנס טלפון, מציג שגיאה
              else
                    errorProvider1.SetError(maskTWOPhone1, "לא הוכנס טלפון");        
            }
            //בכל מקרה אחר
            catch (Exception ex)
            {
                errorProvider1.SetError(maskTWOPhone1, ex.Message);
                OK = false;
            }
            //בדיקת תקינות למייל
            try
            {
                //אם הכניסו מייל
                if (txtMail1.Text != "")
                //אם המייל תקין 
                if (Validation.CheackMail(txtMail1.Text))
                    //שמירת המייל
                    C.Mail = txtMail1.Text;
                //אם המייל לא תקין, מציג הודעה שגיאה
                else
                    errorProvider1.SetError(txtMail1, "מייל לא תקין");
            // אם לא הוכנס מייל, מציג שגיאה
            else
                errorProvider1.SetError(txtMail1, "לא הוכנס מייל" );
        }
            //בכל מקרה אחר
            catch (Exception ex)
            {
                errorProvider1.SetError(txtMail1, ex.Message);
                OK = false;
            }
            //בדיקת תקינות לתאריך לידה
            try
            {
                //הגדרת מערך מסוג מחרוזת באורך 3
                string[] t = new string[3];
                // חיתוך הלוכסנים מהתאריך והכנסת התאריך לידה שנקלט מהמשתמש לתוך המשתנה במערך 
                t = mackBirthDay1.Text.Split('/');
                //אם התאריך מוגדר בפורמט תקין כלומר הימים הם בין1-31 וגם החודשים בין 1-12 והשנים הם בין השנה הנוכחית לזמננינו
                if (Convert.ToInt32(t[0]) >= 1 && Convert.ToInt32(t[0]) <= 31 && Convert.ToInt32(t[1]) >= 1 && Convert.ToInt32(t[1]) <= 12 && Convert.ToInt32(t[2]) >= 1900 && Convert.ToInt32(t[2]) <= DateTime.Today.Year + 102)
                    //אם גיל הלקוח הוא בין 18 ל 127
                    if (Validation.GetAge(Convert.ToDateTime(mackBirthDay1.Text)) < 18 || Validation.GetAge(Convert.ToDateTime(mackBirthDay1.Text)) > 127)
                        //שמירת שנת הלידה
                        C.DateBirthday = Convert.ToDateTime(mackBirthDay1.Text);
                    //אם התאריך לא בין 18 ל 127, מציג הודעה שגיאה
                    else
                        errorProvider1.SetError(mackBirthDay1, "גיל לקוח לא תקין");
                //אם התאריך לא הגיוני, מציג שגיאה
                else
                    errorProvider1.SetError(mackBirthDay1, "תאריך לא הגיוני" );
            }
            //בכל מקרה אחר
            catch (Exception ex)
            {
                errorProvider1.SetError(mackBirthDay1, ex.Message);
                OK = false;
            }
            //בדיקת תקינות שבודקת האם קיים לקוח דומה במערכת
            try
            {
                if (d.Customers.ToList().FirstOrDefault(x => x.IDcustemer == txtIDcostemer1.Text) == null)
                    C.IDcustemer = txtIDcostemer1.Text;
                else
                {
                    errorProvider1.SetError(txtIDcostemer1, "לקוח קיים במערכת");
                    OK = false;
                }
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(txtIDcostemer1, ex.Message);
            }
            //שמירת קובץ
            return OK;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (check())
            {
                if (state == "Add")
                {
                    try
                    {
                        d.Customers.Add(C);
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
        private void txtLastName1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.bdikat_text(e.KeyChar);
        }
        private void txtName1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.bdikat_text(e.KeyChar);
        }
        //פעולה לפתיחת נתיב להוספת קבצי תעודת זהות
        private void btnAddDock_Click(object sender, EventArgs e)
        {

        }
    }
}
