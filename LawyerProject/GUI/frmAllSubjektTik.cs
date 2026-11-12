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
    public partial class frmAllSubjektTik : Form
    {
        Subjekt_Tik S;
        DB.LawyerDBEntities1 d;
        IEnumerable<DB.Subjekt_Tik> fillter;
        public frmAllSubjektTik()
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            fillter = d.Subjekt_Tik.ToList();
            filldatagridview();
        }
        private void filldatagridview()
        {
            d = new LawyerDBEntities1();
            dataGridView1.DataSource = d.Subjekt_Tik.ToList().Select(y => new { y.IDSugTik, y.NameSugTik, y.Agra, y.ScharTircha }).ToList();
            changeheaders();
        }
        public void changeheaders()
        {
            dataGridView1.Columns["IDSugTik"].HeaderText = "קוד סוג תיק";
            dataGridView1.Columns["NameSugTik"].HeaderText = "שם סוג תיק";
            dataGridView1.Columns["ScharTircha"].HeaderText = "שכר טירחה";
            dataGridView1.Columns["Agra"].HeaderText = "אגרה";
        }
        private void filldgvFILLTER()
        {
            dataGridView1.DataSource = fillter.ToList().Select(y => new { y.IDSugTik, y.NameSugTik, y.Agra, y.ScharTircha }).ToList();
        }

        private void txtNameSubjektTik_TextChanged(object sender, EventArgs e)
        {
            try
            {
                fillter = fillter.Where(x => x.NameSugTik.StartsWith(txtNameSubjektTik.Text));
                filldgvFILLTER();
            }
            catch
            {

            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {

            GUI.frmAddSubjektTik f = new GUI.frmAddSubjektTik();
            f.ShowDialog();
            filldatagridview();
        }

        private void btnUpDate_Click_1(object sender, EventArgs e)
        {

            int ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            frmAddSubjektTik bt = new frmAddSubjektTik(ID);
            bt.ShowDialog();
            filldatagridview();
        }

        private void txtNameSubjektTik_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.bdikat_text(e.KeyChar);
        }
    }
}
