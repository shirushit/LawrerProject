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
    public partial class Nivot : Form
    {
        public Nivot()
        {
            InitializeComponent();
        } 
        //פעולה לפתיחת טופס אודות
private void btnAbout_Click(object sender, EventArgs e)
        {
            FrmAbout A = new FrmAbout(); 
            A.Show();
        }
        //פעולה לפתיחת טופס לקוחות
        private void btnCustomers_Click(object sender, EventArgs e)
        {
            frmAllCostemers C = new frmAllCostemers();
            C.Show();
        }
        //פעולה לפתיחת טופס תיקים בטיפול העו"ד
        private void btnTikim_Click(object sender, EventArgs e)
        {
            frmAllTikimLetipulOrechDin  T = new frmAllTikimLetipulOrechDin();
            T.Show();
        }
        //פעולה לפתיחת טופס עיסקאות
        private void btnIskaot_Click(object sender, EventArgs e)
        {
            frmAllIskaot I = new frmAllIskaot();
            I.Show();
        }
        //פעולה לפתיחת טופס קבלות
        private void btnKabalot_Click(object sender, EventArgs e)
        {
            //frmKabala C = new frmKabala();
            //C.Show();
        }
    }
}
