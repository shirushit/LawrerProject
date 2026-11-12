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
    public partial class frmAllCostemers : Form
    {

        Customers C;
        DB.LawyerDBEntities1 d;
        IEnumerable<DB.Customers> fillter;
        public frmAllCostemers()
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            fillter = d.Customers.ToList();
            filldatagridview();
        }
        private void filldatagridview()
        {
            d = new LawyerDBEntities1();
            dataGridView1.DataSource = d.Customers.ToList().Select(y => new { y.IDcustemer, y.FirstName,y.LastName, y.City, y.Adress, y.House, y.Mikud, y.Telephone, y.SmartPhone, y.Mail, y.DateBirthday, y.CopyID }).ToList();
            changeheaders();
        }
        public void changeheaders()
        {
            dataGridView1.Columns["IDcustemer"].HeaderText = "ת.ז. לקוח";
            dataGridView1.Columns["FirstName"].HeaderText = "שם פרטי";
            dataGridView1.Columns["LastName"].HeaderText = "שם משפחה";
            dataGridView1.Columns["City"].HeaderText = "עיר";
            dataGridView1.Columns["Adress"].HeaderText = "כתובת";
            dataGridView1.Columns["House"].HeaderText = "מספר בית";
            dataGridView1.Columns["Mikud"].HeaderText = "מיקוד";
            dataGridView1.Columns["Telephone"].HeaderText = "טלפון";
            dataGridView1.Columns["SmartPhone"].HeaderText = "סמארטפון נייד";
            dataGridView1.Columns["Mail"].HeaderText = "מייל";
            dataGridView1.Columns["DateBirthday"].HeaderText = "תאריך לידה";
            dataGridView1.Columns["CopyID"].HeaderText = "צילום ת.ז";
        }

        private void filldgvFILLTER()
        {
            dataGridView1.DataSource = fillter.ToList().Select(y => new { y.IDcustemer, y.FirstName, y.City, y.Adress ,y.House, y.Mikud, y.Telephone, y.SmartPhone, y.Mail, y.DateBirthday, y.CopyID }).ToList();
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            GUI.frmAddCostemer f = new GUI.frmAddCostemer();
            f.ShowDialog();
            filldatagridview();
        }

        private void btnUpDate_Click(object sender, EventArgs e)
        {
            string ID = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            frmAddCostemer c = new frmAddCostemer(ID);
            c.ShowDialog();
            filldatagridview();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtIDCostemer_TextChanged(object sender, EventArgs e)
        {
            try
            {
                fillter = fillter.Where(x => x.IDcustemer.StartsWith(txtIDCostemer.Text));
                filldgvFILLTER();
            }
            catch
            {

            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtIDCostemer_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumber(e.KeyChar.ToString());
        }
    }
}
