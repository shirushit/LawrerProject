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
    public partial class frmAllIskaot : Form
    {
        Tikim_Letipul_Orech_Din T;
        DB.LawyerDBEntities1 d;
        IEnumerable<DB.Tikim_Letipul_Orech_Din> fillter;
        public frmAllIskaot()
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            fillter = d.Tikim_Letipul_Orech_Din.ToList();
            filldatagridview();
        }
        public frmAllIskaot(int id)
        {
            InitializeComponent();
            T = d.Tikim_Letipul_Orech_Din.FirstOrDefault(x => x.IDTik == id);
            d = new LawyerDBEntities1();
            fillter = d.Tikim_Letipul_Orech_Din.ToList();
            filldatagridview();
        }
        //פעולה שממלאה את הדאטה גריביו בכל הנתונים של העיסקאות
        private void filldatagridview()
        {
            d = new LawyerDBEntities1();
            dataGridView1.DataSource = d.Tikim_Letipul_Orech_Din.ToList().Select(y => new {  y.AdressNeches, y.Gush, y.Chelka, y.TatChelka,y.IsTheTikMenualInBETMishpat }).Where(x=>x.IsTheTikMenualInBETMishpat==false).ToList();
            changeheaders();
        }
        public void changeheaders()
        {
            dataGridView1.Columns["AdressNeches"].HeaderText = "כתובת נכס";
            dataGridView1.Columns["Gush"].HeaderText = "גוש";
            dataGridView1.Columns["Chelka"].HeaderText = "חלקה";
            dataGridView1.Columns["TatChelka"].HeaderText = "תת חלקה";
            dataGridView1.Columns["IsTheTikMenualInBETMishpat"].Visible = false;
        }
        private void filldgvFILLTER()
        {
            dataGridView1.DataSource = fillter.ToList().Select(y => new { y.AdressNeches, y.Gush, y.Chelka, y.TatChelka, y.IsTheTikMenualInBETMishpat }).Where(x => x.IsTheTikMenualInBETMishpat == false).ToList();
        }
       
        private void btnAdd_Click(object sender, EventArgs e)
        {

            GUI.frmAddTikLetipulOrechDin f = new GUI.frmAddTikLetipulOrechDin();
            f.ShowDialog();
            filldatagridview();
        }
        private void btnUpDate_Click(object sender, EventArgs e)
        {

            int ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            frmAddTikLetipulOrechDin bt = new frmAddTikLetipulOrechDin(ID);
            bt.ShowDialog();
            filldatagridview();
        }

        private void txtAdress_TextChanged(object sender, EventArgs e)
        {
            try
            {
                fillter = fillter.Where(x => x.AdressNeches.StartsWith(txtAdress.Text));
                filldgvFILLTER();
            }
            catch
            {

            }
        }

        private void txtGush_TextChanged(object sender, EventArgs e)
        {
            try
            {
                fillter = fillter.Where(x => x.Gush.StartsWith(txtGush.Text));
                filldgvFILLTER();
            }
            catch
            {

            }
        }

        private void txtChelka_TextChanged(object sender, EventArgs e)
        {
            try
            {
                fillter = fillter.Where(x => x.TatChelka.StartsWith(txtTatChelka.Text));
                filldgvFILLTER();
            }
            catch
            {

            }
        }

        private void txtTatChelka_TextChanged(object sender, EventArgs e)
        {
            try
            {
                fillter = fillter.Where(x => x.Chelka.StartsWith(txtChelka.Text));
                filldgvFILLTER();
            }
            catch
            {

            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            GUI.frmAddTikLetipulOrechDin f = new GUI.frmAddTikLetipulOrechDin();
            f.ShowDialog();
            filldatagridview();
        }

        private void btnUpDate_Click_1(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            frmAddSubjektShebetMishpatMetapel bt = new frmAddSubjektShebetMishpatMetapel(ID);
            bt.ShowDialog();
            filldatagridview();
        }
    }
}
