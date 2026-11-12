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
    public partial class frmAddTaalichLetik : Form
    {
        Taalichim_Letikim t;
        DB.LawyerDBEntities1 d;
        IEnumerable<DB.Taalichim_Letikim> fillter;
        public frmAddTaalichLetik()
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            fillter = d.Taalichim_Letikim.ToList();
            filldatagridview();
        }
        private void filldatagridview()
        {
            d = new LawyerDBEntities1();
            dataGridView1.DataSource = d.Taalichim_Letikim.ToList().Select(y => new { y.IDTaalich, y.NameTaalich, y.MeshechZmanLetaalich, y.SugTaalich }).ToList();
            changeheaders();
        }
        public void changeheaders()
        {
            dataGridView1.Columns["IDTaalich"].HeaderText = "קוד תהליך";
            dataGridView1.Columns["NameTaalich"].HeaderText = "שם תהליך";
            dataGridView1.Columns["MeshechZmanLetaalich"].HeaderText = "משך זמן משוער";
            dataGridView1.Columns["SugTaalich"].HeaderText = "סוג תהליך";
        }
        private void filldgvFILLTER()
        {
            dataGridView1.DataSource = fillter.ToList().Select(y => new { y.IDTaalich, y.NameTaalich, y.MeshechZmanLetaalich, y.SugTaalich }).ToList();
        }

        private void txtNameSubjektTik_TextChanged(object sender, EventArgs e)
        {
            try
            {
                fillter = fillter.Where(x => x.NameTaalich.StartsWith(txtNameTaalich.Text));
                filldgvFILLTER();
            }
            catch
            {

            }
        }
        private void txtNameSubjektTik_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.bdikat_text(e.KeyChar);
        }
        private void btnAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            GUI.FrmAddTaalichimLetik f = new GUI.FrmAddTaalichimLetik();
            f.ShowDialog();
            filldatagridview();
        }
        private void btnUpDate_Click_2(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            GUI.FrmAddTaalichimLetik tl = new FrmAddTaalichimLetik(ID);
            tl.ShowDialog();
            filldatagridview();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {

            GUI.FrmAddTaalichimLetik f = new GUI.FrmAddTaalichimLetik();
            f.ShowDialog();
            filldatagridview();
        }
    }
}
