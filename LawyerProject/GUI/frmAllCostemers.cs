using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Clients, opposing parties and opposing lawyers, with free search and a type filter.</summary>
    public partial class frmAllCostemers : Form
    {
        private const string AllTypes = "(הכל)";
        private List<Customers> all = new List<Customers>();

        public frmAllCostemers()
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
                using (var db = new LawyerDBEntities1())
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
            IEnumerable<Customers> rows = all;
            string type = cmbType.SelectedItem as string;
            if (type != null && type != AllTypes) rows = rows.Where(c => c.SugPerson == type);

            foreach (string word in txtIDCostemer.Text.Trim().ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string w = word;
                string digits = new string(w.Where(char.IsDigit).ToArray());
                rows = rows.Where(c =>
                    (c.IDcustemer + " " + c.FirstName + " " + c.LastName + " " + c.City + " " + c.Mail).ToLowerInvariant().Contains(w)
                    || (digits.Length >= 3 && ((c.Telephone ?? "").Contains(digits) || (c.SmartPhone ?? "").Contains(digits))));
            }

            List<Customers> list = rows.ToList();
            dataGridView1.DataSource = list.Select(c => new
            {
                c.IDcustemer,
                IdType = c.SugMezahe == IdTypes.None ? "-" : c.SugMezahe,
                Name = c.DisplayName,
                c.SugPerson,
                c.City,
                Address = ((c.Adress ?? "") + " " + (c.House ?? "")).Trim(),
                c.Telephone,
                c.SmartPhone,
                c.Mail,
                BirthDate = Ui.FormatDate(c.DateBirthday)
            }).ToList();

            Ui.SetHeaders(dataGridView1, new Dictionary<string, string>
            {
                { "IDcustemer", "מזהה" },
                { "IdType", "סוג מזהה" },
                { "Name", "שם" },
                { "SugPerson", "סוג" },
                { "City", "עיר" },
                { "Address", "כתובת" },
                { "Telephone", "טלפון" },
                { "SmartPhone", "נייד" },
                { "Mail", "מייל" },
                { "BirthDate", "תאריך לידה" }
            });
            lblCount.Text = list.Count == all.Count ? all.Count + " רשומות" : "מוצגות " + list.Count + " מתוך " + all.Count;
        }

        private void txtIDCostemer_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtIDCostemer.Clear();
            cmbType.SelectedIndex = 0;
            txtIDCostemer.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var form = new frmAddCostemer())
            {
                if (form.ShowDialog(this) != DialogResult.OK) return;
                LoadCustomers();
                Ui.Notify(this, "נשמר: " + form.SavedId);
            }
        }

        private void btnUpDate_Click(object sender, EventArgs e)
        {
            object id = Ui.SelectedValue(dataGridView1, "IDcustemer");
            if (id == null)
            {
                Ui.Error("יש לבחור רשומה מהרשימה.");
                return;
            }
            using (var form = frmAddCostemer.ForEdit(id.ToString()))
            {
                if (form.ShowDialog(this) != DialogResult.OK) return;
                LoadCustomers();
                Ui.Notify(this, "הפרטים עודכנו.");
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnUpDate_Click(sender, e);
        }
    }
}
