using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Clients, opposing parties and opposing lawyers, with free search and a type filter.</summary>
    public partial class CustomersListForm : Form
    {
        private const string AllTypes = "(הכל)";
        private List<Customer> all = new List<Customer>();

        public CustomersListForm()
        {
            InitializeComponent();
            cmbType.Items.Add(AllTypes);
            cmbType.Items.AddRange(PersonTypes.All);
            cmbType.SelectedIndex = 0;
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                using (var db = new QuedinDbContext())
                    all = db.Customers.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();
            }
            catch (Exception ex)
            {
                Ui.Error("לא ניתן לטעון את הרשימה.", ex);
            }
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            IEnumerable<Customer> rows = all;
            string type = cmbType.SelectedItem as string;
            if (type != null && type != AllTypes) rows = rows.Where(c => c.PersonType == type);

            foreach (string word in txtFilter.Text.Trim().ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string w = word;
                string digits = new string(w.Where(char.IsDigit).ToArray());
                rows = rows.Where(c =>
                    (c.CustomerId + " " + c.FirstName + " " + c.LastName + " " + c.City + " " + c.Email).ToLowerInvariant().Contains(w)
                    || (digits.Length >= 3 && ((c.Phone ?? "").Contains(digits) || (c.MobilePhone ?? "").Contains(digits))));
            }

            List<Customer> list = rows.ToList();
            gridCustomers.DataSource = list.Select(c => new
            {
                c.CustomerId,
                IdType = c.IdType == IdTypes.None ? "-" : c.IdType,
                Name = c.DisplayName,
                c.PersonType,
                c.City,
                Address = ((c.Street ?? "") + " " + (c.HouseNumber ?? "")).Trim(),
                c.Phone,
                c.MobilePhone,
                c.Email,
                BirthDate = Ui.FormatDate(c.BirthDate)
            }).ToList();

            Ui.SetHeaders(gridCustomers, new Dictionary<string, string>
            {
                { "CustomerId", "מזהה" },
                { "IdType", "סוג מזהה" },
                { "Name", "שם" },
                { "PersonType", "סוג" },
                { "City", "עיר" },
                { "Address", "כתובת" },
                { "Phone", "טלפון" },
                { "MobilePhone", "נייד" },
                { "Email", "מייל" },
                { "BirthDate", "תאריך לידה" }
            });
            lblCount.Text = list.Count == all.Count ? all.Count + " רשומות" : "מוצגות " + list.Count + " מתוך " + all.Count;
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFilter.Clear();
            cmbType.SelectedIndex = 0;
            txtFilter.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new CustomerForm())
            {
                if (form.ShowDialog(this) != DialogResult.OK) return;
                LoadCustomers();
                Ui.Notify(this, "נשמר: " + form.SavedId);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            object id = Ui.SelectedValue(gridCustomers, "CustomerId");
            if (id == null)
            {
                Ui.Error("יש לבחור רשומה מהרשימה.");
                return;
            }
            using (var form = CustomerForm.ForEdit(id.ToString()))
            {
                if (form.ShowDialog(this) != DialogResult.OK) return;
                LoadCustomers();
                Ui.Notify(this, "הפרטים עודכנו.");
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnUpdate_Click(sender, e);
        }
    }
}
