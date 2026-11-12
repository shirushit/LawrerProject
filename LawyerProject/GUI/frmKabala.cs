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
using static System.Windows.Forms.AxHost;

namespace LawyerProject.GUI
{
    public partial class frmKabala : Form
    {
         DB.LawyerDBEntities1 d;
        string state;
        Tashlumim_Letik T;
        Tikim_Letipul_Orech_Din tlod;
        Customers c;
        //פעולה בונה למצב הוספה 
        public frmKabala()
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            state = "Add";
            T = new Tashlumim_Letik();
            lblDate.Text = DateTime.Today.ToString();
        //    lblnameC = c.FirstName.FirstOrDefault(x => x.IDBetMishpat == b);
           

        }
        //פעולה בונה למצב עדכון
        public frmKabala(int ID)
        {
            //הצגת הפקדים בטופס
            InitializeComponent();
            state = "UpDate";
            d = new DB.LawyerDBEntities1();
            T = d.Tashlumim_Letik.FirstOrDefault(x => x.IDTashlum == ID);
        }
    }
}
