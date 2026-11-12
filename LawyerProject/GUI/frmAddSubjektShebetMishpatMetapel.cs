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
    public partial class frmAddSubjektShebetMishpatMetapel : Form
    {
        DB.LawyerDBEntities1 d;
        string state;
        Subjekts_Shebet_Mishpat_Metapel S;
        public frmAddSubjektShebetMishpatMetapel(Bet_Mishpat b)
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            state = "Add";
            S = new Subjekts_Shebet_Mishpat_Metapel();
            lblIdBetMishpat.Text = b.IDBetMishpat.ToString();
            lblIdSubjektLebetMishpat.Text = GetNewKey(d.Subjekts_Shebet_Mishpat_Metapel.ToList()).ToString();
        }
        //פעולה בונה למצב עדכון
        public frmAddSubjektShebetMishpatMetapel(int b)
        {
            InitializeComponent();
            state = "UpDate";
            d = new DB.LawyerDBEntities1();
            S = d.Subjekts_Shebet_Mishpat_Metapel.FirstOrDefault(x => x.IDBetMishpat == b);
            Filltextbox();
        }
        //פעולה שממלאה את הקומבובוקס
        public void Filltextbox()
        {
            lblIdSubjektLebetMishpat.Text = S.IDTable.ToString();
            lblIdBetMishpat.Text = S.IDBetMishpat.ToString();
            cmbNameSubjectBetMishpat.Text = S.Bet_Mishpat.ToString();
        }
        public int GetNewKey(List<DB.Subjekts_Shebet_Mishpat_Metapel> l)
        {
            if (l.Count == 0)
            {
                return 1;
            }
            return Convert.ToInt32(l[l.Count - 1].IDTable) + 1;
        }
        public bool check()
        {
            bool ok = true;
            errorProvider1.Clear();

            S.IDTable = Convert.ToInt32(lblIdSubjektLebetMishpat.Text);
            S.IDBetMishpat = Convert.ToInt32(lblIdBetMishpat.Text);
            S.IDSubjekt = ((Subjekt_Tik)cmbNameSubjectBetMishpat.SelectedItem).IDSugTik;
            return ok;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (check())
            {
                if (state == "Add")
                {
                    try
                    {
                        d.Subjekts_Shebet_Mishpat_Metapel.Add(S);
                        d.SaveChanges();
                        MessageBox.Show(" נשמר בהצלחה");
                        this.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(" לא נשמר");
                    }
                }
                if (state == "Update")
                {
                    try
                    {
                        d.SaveChanges();
                        MessageBox.Show(" עודכן בהצלחה");
                        this.Close();
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(" לא נשמר");
                    }
                }
            }
        }
    }
}
