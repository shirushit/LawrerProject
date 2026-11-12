using System;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>Add or edit a person: client, opposing party or opposing lawyer.</summary>
    public partial class frmAddCostemer : Form
    {
        private readonly LawyerDBEntities1 db = new LawyerDBEntities1();
        private readonly bool isNew;
        private readonly Customers customer;

        /// <summary>The ID of the saved person, so a calling screen can select it.</summary>
        public string SavedId { get; private set; }

        public frmAddCostemer() : this((string)null, PersonTypes.Client)
        {
        }

        /// <summary>New person of a given type (used by the case screen's "add opposing lawyer" etc.).</summary>
        public frmAddCostemer(string personType) : this(null, personType)
        {
        }

        private frmAddCostemer(string id, string personType)
        {
            InitializeComponent();
            cmbSugPerson.Items.AddRange(PersonTypes.All);

            if (id == null)
            {
                isNew = true;
                customer = new Customers();
                cmbSugPerson.SelectedItem = personType ?? PersonTypes.Client;
                Text = "הוספת " + cmbSugPerson.SelectedItem;
            }
            else
            {
                customer = db.Customers.FirstOrDefault(c => c.IDcustemer == id);
                if (customer == null) throw new ArgumentException("Customer not found: " + id);
                FillControls();
                txtIDcostemer1.ReadOnly = true; // the ID number is the primary key
                Text = "עדכון פרטי " + customer.SugPerson;
            }
        }

        /// <summary>Edit an existing person.</summary>
        public static frmAddCostemer ForEdit(string id)
        {
            return new frmAddCostemer(id, null);
        }

        private void FillControls()
        {
            txtIDcostemer1.Text = customer.IDcustemer;
            cmbSugPerson.SelectedItem = customer.SugPerson;
            txtName1.Text = customer.FirstName;
            txtLastName1.Text = customer.LastName;
            txtCity.Text = customer.City;
            txtAdress1.Text = customer.Adress;
            txtHouse.Text = customer.House;
            txtMikud.Text = customer.Mikud;
            maskONEPhone1.Text = customer.Telephone;
            maskTWOPhone1.Text = customer.SmartPhone;
            txtMail1.Text = customer.Mail;
            mackBirthDay1.Text = customer.DateBirthday.HasValue ? customer.DateBirthday.Value.ToString("dd/MM/yyyy") : "";
        }

        private static string Digits(string text)
        {
            return new string((text ?? "").Where(char.IsDigit).ToArray());
        }

        /// <summary>Validates every field, shows all errors at once and copies valid values into the entity.</summary>
        private bool ValidateAndCopy()
        {
            errorProvider1.Clear();
            bool ok = true;
            Action<Control, string> fail = (control, message) => { errorProvider1.SetError(control, message); ok = false; };

            string id = Digits(txtIDcostemer1.Text);
            if (isNew)
            {
                if (!Validation.IsLegalId(id)) fail(txtIDcostemer1, "מספר תעודת זהות לא תקין");
                else if (db.Customers.Any(c => c.IDcustemer == id)) fail(txtIDcostemer1, "תעודת זהות זו כבר קיימת במערכת");
            }

            string personType = cmbSugPerson.SelectedItem as string;
            if (personType == null) fail(cmbSugPerson, "יש לבחור סוג");

            if (!Validation.IsName(txtName1.Text)) fail(txtName1, "יש להזין שם פרטי");
            if (!Validation.IsName(txtLastName1.Text)) fail(txtLastName1, "יש להזין שם משפחה");
            if (!Validation.IsName(txtCity.Text)) fail(txtCity, "יש להזין עיר");
            if (string.IsNullOrWhiteSpace(txtAdress1.Text)) fail(txtAdress1, "יש להזין רחוב");
            if (string.IsNullOrWhiteSpace(txtHouse.Text)) fail(txtHouse, "יש להזין מספר בית");

            string phone = maskONEPhone1.Text.Trim();
            if (!Validation.IsPhone(phone)) fail(maskONEPhone1, "מספר טלפון לא תקין (לדוגמה 02-6543210 או 050-1234567)");

            string mobile = maskTWOPhone1.Text.Trim();
            if (mobile.Length > 0 && !Validation.IsCellPhone(mobile)) fail(maskTWOPhone1, "מספר נייד לא תקין");

            if (!Validation.IsEmail(txtMail1.Text)) fail(txtMail1, "כתובת מייל לא תקינה");

            DateTime? birthDate = null;
            if (Digits(mackBirthDay1.Text).Length > 0)
            {
                DateTime parsed;
                if (!Validation.TryParseDate(mackBirthDay1.Text, out parsed)) fail(mackBirthDay1, "תאריך לא תקין");
                else if (personType == PersonTypes.Client && !Validation.IsAdultBirthDate(parsed, DateTime.Today)) fail(mackBirthDay1, "לקוח חייב להיות בן 18 ומעלה");
                else birthDate = parsed;
            }

            if (!ok) return false;

            if (isNew) customer.IDcustemer = id;
            customer.SugPerson = personType;
            customer.FirstName = txtName1.Text.Trim();
            customer.LastName = txtLastName1.Text.Trim();
            customer.City = txtCity.Text.Trim();
            customer.Adress = txtAdress1.Text.Trim();
            customer.House = txtHouse.Text.Trim();
            customer.Mikud = string.IsNullOrWhiteSpace(txtMikud.Text) ? null : txtMikud.Text.Trim();
            customer.Telephone = phone.Replace("-", "");
            customer.SmartPhone = mobile.Length == 0 ? null : mobile.Replace("-", "");
            customer.Mail = txtMail1.Text.Trim();
            customer.DateBirthday = birthDate;
            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValidateAndCopy()) return;
            try
            {
                if (isNew) db.Customers.Add(customer);
                db.SaveChanges();
                SavedId = customer.IDcustemer;
                Ui.Info(isNew ? "נשמר בהצלחה." : "עודכן בהצלחה.");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Ui.Error("שגיאה! הנתונים לא נשמרו.", ex);
            }
        }

        private void txtName1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsNameKey(e.KeyChar);
        }

        private void Digits_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsDigitKey(e.KeyChar);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db.Dispose();
            base.OnFormClosed(e);
        }
    }
}
