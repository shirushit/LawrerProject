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
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;

namespace LawyerProject.GUI
{
    public partial class frmAddTikLetipulOrechDin : Form
    {
        DB.LawyerDBEntities1 d;
        string state;
        Tikim_Letipul_Orech_Din T;
        List<Tahalich_Tipul_Betik> taalicim = new List<Tahalich_Tipul_Betik>();
        List<Tashlumim_Letik> tashlumim = new List<Tashlumim_Letik>();
        public frmAddTikLetipulOrechDin()
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            state = "Add";
            T = new Tikim_Letipul_Orech_Din();
            lblStatusTik.Text = GetNewKey1(d.Tikim_Letipul_Orech_Din.ToList()).ToString();
            lblCodeTik.Text = GetNewKey(d.Tikim_Letipul_Orech_Din.ToList()).ToString();
            cmbBetMishpat.DataSource = d.Bet_Mishpat.ToList();
            List<Customers> lstCustomers = new List<Customers>();
            lstCustomers = d.Customers.Where(x => x.SugPerson == "לקוח").ToList();
            comboBox3.DataSource = lstCustomers;
            lstCustomers = d.Customers.Where(x => x.SugPerson == "לקוח נגדי").ToList();
            cmbCustemerNegdi.DataSource = lstCustomers;
            lstCustomers = d.Customers.Where(x => x.SugPerson == "עורך דין נגדי").ToList();
            cmbOrechDinNegdi.DataSource = lstCustomers;
            comboBox1.DataSource = d.Subjekt_Tik.ToList();
            checkBoxTikMenoal.Checked = true;
        }
        public frmAddTikLetipulOrechDin(int ID)
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            state = "Update";
            T = new Tikim_Letipul_Orech_Din();
            lblCodeTik.Text = GetNewKey(d.Tikim_Letipul_Orech_Din.ToList()).ToString();
            cmbBetMishpat.DataSource = d.Bet_Mishpat.ToList();
            lblStatusTik.Text = GetNewKey1(d.Tikim_Letipul_Orech_Din.ToList()).ToString();
            List<Customers> lstCustomers = new List<Customers>();
            lstCustomers = d.Customers.Where(x => x.SugPerson == "לקוח").ToList();
            comboBox3.DataSource = lstCustomers;
            lstCustomers = d.Customers.Where(x => x.SugPerson == "לקוח נגדי").ToList();
            cmbCustemerNegdi.DataSource = lstCustomers;
            lstCustomers = d.Customers.Where(x => x.SugPerson == "עורך דין נגדי").ToList();
            cmbOrechDinNegdi.DataSource = lstCustomers;
            comboBox1.DataSource = d.Subjekt_Tik.ToList();
            taalicim = d.Tahalich_Tipul_Betik.ToList();
            tashlumim = d.Tashlumim_Letik.ToList();
            T = d.Tikim_Letipul_Orech_Din.FirstOrDefault(x => x.IDTik == ID);
            cmbStatusTik.DataSource = d.Taalichim_Letikim.ToString();
        }
        //פעולה מחזירה את הקוד של התיק לטיפול העורך דין
        public int GetNewKey(List<DB.Tikim_Letipul_Orech_Din> l)
        {
            if (l.Count == 0)
            {
                return 1;
            }
            return Convert.ToInt32(l[l.Count - 1].IDTik) + 1;
        }
        //פעולה מחזירה את הסטטוס של התיק לטיפול העורך דין
        public int GetNewKey1(List<DB.Tikim_Letipul_Orech_Din> l)
        {
            return (l[l.Count - 1].StatusTik);
        }
        //פעולה שאחראית למלא את התשלומים והתהליכים ששיכים לכל תיק
        public void fillTaalicim()
        {
            //לולאה  שעוברת על כל התהליכים ועבוד כל תהליך מייצרת שורה בליסט שבתצוגה
            for (int i = 0; i < taalicim.Count; i++)
            {
                //בניית שורה בודדה בליטס
                ListViewItem item = new ListViewItem(new string[] { taalicim[i].IDTahalich.ToString(), taalicim[i].Taalichim_Letikim.NameTaalich, taalicim[i].Taalichim_Letikim.NameTaalich, });
                //הוספת השורה לפקד בטופס
                listView1.Items.Add(item);
            }
        }
        public void fillTashlumim()
        {
            //לולאה  שעוברת על כל התשלומים ועבוד כל תשלום מייצרת שורה בליסט שבתצוגה
            for (int i = 0; i < taalicim.Count; i++)
            {
                //בניית שורה בודדה בליטס
                ListViewItem item = new ListViewItem(new string[] { tashlumim[i].Itra.ToString(), tashlumim[i].Schum.ToString(), tashlumim[i].TaarichTashlum.ToString(), tashlumim[i].Teur });
                //הוספת השורה לפקד בטופס
                listView2.Items.Add(item);
         }   }     
        //פעולה שקןראת את כל הנתונים שהמשתמש הזין 
        public void FillTextBox()
        {
            lblCodeTik.Text = T.IDTik.ToString();
            comboBox3.DataSource = d.Customers.ToString();
            comboBox1.DataSource = d.Subjekt_Tik.ToString();
            checkBoxTikMenoal.Checked = Convert.ToBoolean(T.IsTheTikMenualInBETMishpat);
            textBox1.Text = T.IDBetMishpat.ToString();
            checkBoxTikMenoal.Checked = Convert.ToBoolean(T.IsTheTikMenualInBETMishpat);
            checkBoxShulam.Checked = T.Shulam;
            checkBoxShulamMikdama.Checked = Convert.ToBoolean(T.ShulamMikdama);
            txtIPuy.Text = T.IpuiKohach;
            cmbBetMishpat.Text = T.Bet_Mishpat.ToString();
            cmbStatusTik.Text = T.StatusTik.ToString();
            List<Customers> lst = new List<Customers>();
            lst = d.Customers.ToList();
            Customers c=lst.FirstOrDefault(x => x.IDcustemer == T.getCustomerNegdi().IDcustemer.ToString());
            cmbCustemerNegdi.Text = c.ToString();
            cmbOrechDinNegdi.Text = T.KodOrechDinNegdi.ToString();
            txtAdressNeches.Text = T.AdressNeches.ToString();
            txtAchuzRevachim.Text = T.AchuzRevachim.ToString();
            txtChelka.Text = T.Chelka.ToString();
            txtGush.Text = T.Gush.ToString();
            txtSchumKetvia.Text = T.SchumLetvia.ToString();
            txtSchumShenitba.Text = T.SchumShenitba.ToString();
            txtTatChelka.Text = T.TatChelka.ToString();
        }

        //פעולה שבודקת האם הנתונים נכונים
        public bool check()
        {
            //הגדרת משתנה מסוג בוליאני
            bool OK = true;
            //אתחול הפקד errorProvider1
            errorProvider1.Clear();
            //מילוי בפקד comboBox את הרשימה של כל הבתי משפט
            T.IDBetMishpat = ((Bet_Mishpat)(cmbBetMishpat.SelectedItem)).IDBetMishpat;
            //קליטה האם התיק מנוהל בבית משפט
            T.IsTheTikMenualInBETMishpat = checkBoxTikMenoal.Checked;
            //קליטה הייפוי כוח
            T.IpuiKohach = txtIPuy.Text;
            //קליטה האם שולם התשלום לתיק 
            T.Shulam = checkBoxShulam.Checked;
            //קליטה האם שולם מקדמה
            T.ShulamMikdama = checkBoxShulamMikdama.Checked;
            //אם הנתונים של הגוש תקינים אז תקלוט אותו
            try
            {
                T.Gush = txtGush.Text;
            }
            //אם לא הצלחת לקלוט את הנתונים
            catch (Exception ex)
            {
                //תציג הודעה משתמש ליד הפקד של הגוש
                errorProvider1.SetError(txtGush, ex.Message);
                //שינוי המשתנה לשלילי
                OK = false;
            }
            //חלקה
            try
            {
                T.Chelka = txtChelka.Text;
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(txtChelka, ex.Message);
                OK = false;
            }
            //
            try
            {
                T.TatChelka = txtTatChelka.Text;
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(txtTatChelka, ex.Message);
                OK = false;
            }
            try
            {
                T.AdressNeches = txtAdressNeches.Text;
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(txtAdressNeches, ex.Message);
                OK = false;
            }
            try
            {
                T.SchumLetvia = Convert.ToInt32(txtSchumKetvia.Text);
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(txtSchumKetvia, ex.Message);
                OK = false;
            }
            try
            {
               T.SchumShenitba = Convert.ToInt32(txtSchumShenitba.Text);
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(txtSchumShenitba, ex.Message);
                OK = false;
            }
            try
            {
               T.AchuzRevachim = Convert.ToInt32(txtAchuzRevachim.Text);
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(txtAchuzRevachim, ex.Message);
                OK = false;
            }
          
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
                        d.Tikim_Letipul_Orech_Din.Add(T);
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
                    catch
                    {

                        MessageBox.Show("שגיאה! לא עודכן");
                    }
                }
            }
        }
        void AddTahalicim()
        {
            DateTime dt = new DateTime();
            dt=DateTime.Now;
            foreach (Taalichim_Kvuim_Batik taalichim in d.Taalichim_Kvuim_Batik.Where(x=>x.Subjekt_Tik==comboBox1.SelectedItem).OrderBy(x=>x.NumLevel))
            {
                dt = dt.AddDays(Convert.ToInt32(taalichim.Taalichim_Letikim.MeshechZmanLetaalich));
                Tahalich_Tipul_Betik ttb = new Tahalich_Tipul_Betik();
                ttb.IDTahalich = taalichim.IDTaalich;
                ttb.TaharichBizhua = dt;
                ttb.TaharichMeshoar = dt;
                ttb.IDTik = T.IDTik;
                ttb.Buzha = false;
                d.Tahalich_Tipul_Betik.Add(ttb);
                d.SaveChanges();   
            }
        }
       //להוסיף בדיקת תקינות וגוש חלקה ותת חלקה
        private void txtGush_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumberAndProchent(e.KeyChar);
        }

        private void txtSchumKetvia_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.bdikat_mispar(e.KeyChar);
        }

        private void txtSchumShenitba_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.bdikat_mispar(e.KeyChar);
        }

        private void txtAchuzRevachim_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.ChekNumAchuz(e.KeyChar);
            e.Handled = !Validation.bdikat_mispar(e.KeyChar);
        }

        private void checkBoxTikMenoal_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBoxTikMenoal.Checked == true)
            {
                groupBoxBetMishpat.Visible = true;
                groupBoxIska.Visible=false;
            }
            else
            {
                groupBoxBetMishpat.Visible = false;
                groupBoxIska.Visible = true;
            }
        }
        private void txtChelka_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumberAndProchent(e.KeyChar);
        }

        private void txtTatChelka_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumberAndProchent(e.KeyChar);
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }
        //פעולה שפותחת את הטופס הוספת לקוח חדש
        private void button2_Click(object sender, EventArgs e)
        {
            frmAllCostemers allCostemers = new frmAllCostemers();
            allCostemers.ShowDialog();
            comboBox3.DataSource = d.Customers.ToList();
        }

        private void btnAddSubjektTik_Click(object sender, EventArgs e)
        {
            frmAllSubjektTik allSubjektTik = new frmAllSubjektTik();
            allSubjektTik.ShowDialog();
            comboBox1.DataSource = d.Subjekt_Tik.ToList();
        }

        private void btnAddLawyer_Click(object sender, EventArgs e)
        {
            frmAllCostemers allCostemers = new frmAllCostemers();
            allCostemers.ShowDialog();
            cmbOrechDinNegdi.DataSource = d.Customers.ToList();
        }

        private void btnAddCustemerNegdi_Click(object sender, EventArgs e)
        {
            frmAllCostemers allCostemers = new frmAllCostemers();
            allCostemers.ShowDialog();
            cmbCustemerNegdi.DataSource = d.Customers.ToList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmAddBetMishpat AddBetMishpat = new frmAddBetMishpat();
            AddBetMishpat.ShowDialog();
            cmbBetMishpat.DataSource = d.Bet_Mishpat.ToList();
        }

        private void cmbBetMishpat_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        //פעולה שמעדכנת את התהליכים לתיק
        private void button3_Click(object sender, EventArgs e)
        {
            bool f = false;
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            if (listView1.CheckedItems.Count > 0)
            {
                Tahalich_Tipul_Betik ttb = new Tahalich_Tipul_Betik();
                ttb = d.Tahalich_Tipul_Betik.First(x => Convert.ToInt32(listView1.SelectedItems[0].SubItems[7]) == x.IDTahalichTipulLetik);
                foreach(Tahalich_Tipul_Betik t in T.Tahalich_Tipul_Betik.OrderBy(x=>x.TaharichMeshoar))
                {
                    if(t.Buzha==false)
                    {
                        if(t!=ttb)
                        {
                            MessageBox.Show("לא ניתן לדלג על שלבים");
                            break;
                        }
                        else
                        {
                            //התהליך בוצע
                            f = true;
                            ttb.Buzha = true;
                            ttb.TaharichBizhua = dt;
                            d.SaveChanges();
                           
                        } 
                        if(f)
                        {
                            //עדכון תאריכים לתהליכים הבאים
                            dt =  dt.AddDays(Convert.ToInt32(t.Taalichim_Letikim.MeshechZmanLetaalich));
                            t.TaharichMeshoar = dt;
                            t.TaharichBizhua = dt;
                            d.SaveChanges();
                        }
                    
                    }
                }
            }
           
            
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            frmAddTashlumimLetik TashlumimLetik = new frmAddTashlumimLetik(T);
            TashlumimLetik.ShowDialog();
            fillTaalicim();
        }
    }    
}
