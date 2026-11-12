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
        public partial class frmAllBetMishpat : Form
        {
            Bet_Mishpat b;
            DB.LawyerDBEntities1 d;
            IEnumerable<DB.Bet_Mishpat> fillter;
            public frmAllBetMishpat()
            {
                InitializeComponent();
                d = new LawyerDBEntities1();
                fillter = d.Bet_Mishpat.ToList();
                filldatagridview();
            }
            private void filldatagridview()
            {
                d = new LawyerDBEntities1();
                dataGridView1.DataSource = d.Bet_Mishpat.ToList().Select(y => new { y.IDBetMishpat, y.NameBetMishpat }).ToList();
                changeheaders();
            }
            public void changeheaders()
            {
                dataGridView1.Columns["IDBetMishpat"].HeaderText = "קוד בית משפט";
                dataGridView1.Columns["NameBetMishpat"].HeaderText = "שם בית משפט";
            }
            private void filldgvFILLTER()
            {
                dataGridView1.DataSource = fillter.ToList().Select(y => new { y.IDBetMishpat, y.NameBetMishpat }).ToList();
            }



            private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {

            }

            private void txtNameBetMishpat_TextChanged(object sender, EventArgs e)
            {
                try
                {
                    fillter = fillter.Where(x => x.NameBetMishpat.StartsWith(txtNameBetMishpat.Text));
                    filldgvFILLTER();
                }
                catch
                {

                }
            }
            private void btnAdd_Click(object sender, EventArgs e)
            {

                GUI.frmAddBetMishpat f = new GUI.frmAddBetMishpat();
                f.ShowDialog();
                filldatagridview();
            }
            private void btnUpDate_Click(object sender, EventArgs e)
            {

                int ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                frmAddBetMishpat bt = new frmAddBetMishpat(ID);
                bt.ShowDialog();
                filldatagridview();
            }

        private void txtNameBetMishpat_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
       
    }

