using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Clients, opposing parties and opposing lawyers.</summary>
    public partial class frmAllCostemers : Form
    {
        private List<Customers> all = new List<Customers>();

        public frmAllCostemers()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            using (var db = new LawyerDBEntities1())
                all = db.Customers.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string text = txtIDCostemer.Text.Trim();
            IEnumerable<Customers> rows = all;
            if (text.Length > 0)
                rows = rows.Where(c => c.IDcustemer.StartsWith(text)
                                    || (c.FirstName + " " + c.LastName).Contains(text)
                                    || (c.LastName + " " + c.FirstName).Contains(text));

            dataGridView1.DataSource = rows.Select(c => new
            {
                c.IDcustemer,
                c.FirstName,
                c.LastName,
                c.SugPerson,
                c.City,
                Address = c.Adress + " " + c.House,
                c.Telephone,
                c.SmartPhone,
                c.Mail,
                BirthDate = Ui.FormatDate(c.DateBirthday)
            }).ToList();

            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "IDcustemer", "ת.ז." },
                { "FirstName", "שם פרטי" },
                { "LastName", "שם משפחה" },
                { "SugPerson", "סוג" },
                { "City", "עיר" },
                { "Address", "כתובת" },
                { "Telephone", "טלפון" },
                { "SmartPhone", "נייד" },
                { "Mail", "מייל" },
                { "BirthDate", "תאריך לידה" }
            });
        }

        private void txtIDCostemer_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new frmAddCostemer())
                form.ShowDialog();
            LoadCustomers();
        }

        private void btnUpDate_Click(object sender, EventArgs e)
        {
            object id = Ui.SelectedValue(dataGridView1, "IDcustemer");
            if (id == null)
            {
                Ui.Error("יש לבחור לקוח מהרשימה.");
                return;
            }
            using (var form = frmAddCostemer.ForEdit(id.ToString()))
                form.ShowDialog();
            LoadCustomers();
        }
    }
}
