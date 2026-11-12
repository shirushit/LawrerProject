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
    public partial class frmAllTikimLetipulOrechDin : Form
    {
        Tikim_Letipul_Orech_Din T;
        DB.LawyerDBEntities1 d;
        IEnumerable<DB.Tikim_Letipul_Orech_Din> fillter;
        public frmAllTikimLetipulOrechDin()
        {
            InitializeComponent();
            d = new LawyerDBEntities1();
            fillter = d.Tikim_Letipul_Orech_Din.ToList();
            filldatagridview();
            List<Customers> lstCustomers=new List<Customers>();
            lstCustomers = d.Customers.Where(x => x.SugPerson == "לקוח").ToList();
            comboBox4.DataSource = lstCustomers;
            lstCustomers = d.Customers.Where(x => x.SugPerson == "לקוח נגדי").ToList();
            comboBox1.DataSource = lstCustomers;
            lstCustomers = d.Customers.Where(x => x.SugPerson == "עורך דין נגדי").ToList();
            comboBox3.DataSource = lstCustomers;
            comboBox5.DataSource = d.Subjekt_Tik.ToList();
            comboBox2.DataSource = d.Tikim_Letipul_Orech_Din.ToList();
        }
        private void filldatagridview()
        {
            
            dataGridView1.DataSource = d.Tikim_Letipul_Orech_Din.ToList().Select(y => new {
                y.Customers.IDcustemer,
                name_customer = y.Customers.LastName + " " + y.Customers.FirstName, 
                y.Subjekt_Tik.NameSugTik,
                y.IDTik,
                customerNegdi=y.getCustomerNegdi().LastName+" "+y.getCustomerNegdi().FirstName,
                orechDinNegdi = y.getOrechDinNegdi().LastName + " " + y.getCustomerNegdi().FirstName,
                y.StatusTik, y.Bet_Mishpat.NameBetMishpat, y.IsTheTikMenualInBETMishpat, y.Shulam }).ToList();
            changeheaders();
        }
        public void changeheaders()
        {
            dataGridView1.Columns["name_customer"].HeaderText = "שם לקוח";
            dataGridView1.Columns["IDcustemer"].HeaderText = "ת.ז לקוח";
            dataGridView1.Columns["NameSugTik"].HeaderText = "שם סוג תיק";
            dataGridView1.Columns["IDTik"].HeaderText = "מספר תיק";
            dataGridView1.Columns["NameBetMishpat"].HeaderText = "שם בית משפט";
            dataGridView1.Columns["orechDinNegdi"].HeaderText = "עורך דין נגדי";
            dataGridView1.Columns["customerNegdi"].HeaderText = "לקוח נגדי";
            dataGridView1.Columns["StatusTik"].HeaderText = "סטטוס תיק";
            dataGridView1.Columns["Shulam"].HeaderText = "שולם";
           
        }


        private void filldgvFILLTER()
        {
            dataGridView1.DataSource = fillter.ToList().Select(y => new {
                y.Customers.IDcustemer,
                name_customer = y.Customers.LastName + " " + y.Customers.FirstName,
                y.Subjekt_Tik.NameSugTik,
                y.IDTik,
                customerNegdi = y.getCustomerNegdi().LastName + " " + y.getCustomerNegdi().FirstName,
                orechDinNegdi = y.getOrechDinNegdi().LastName + " " + y.getCustomerNegdi().FirstName,
                y.StatusTik,
                y.Bet_Mishpat.NameBetMishpat,
                y.IsTheTikMenualInBETMishpat,
                y.Shulam
            }).ToList();
        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            GUI.frmAddTikLetipulOrechDin f = new GUI.frmAddTikLetipulOrechDin();
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

        private void txtIDCostemer_TextChanged_1(object sender, EventArgs e)
        {

            try
            {
                //fillter = fillter.Where(x => x.IDcustemer.StartsWith(txtIDCostemer.Text));
              //  filldgvFILLTER();
            }
            catch
            {

            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNumber(e.KeyChar.ToString());
        }
    }
}

