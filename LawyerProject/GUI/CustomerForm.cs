using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>
    /// Add or edit a person or company: client, opposing party or opposing lawyer.
    /// Clients need full details; for the other side only a name is required, because the office
    /// often does not know the opposing party's ID number or address when the case is opened.
    /// </summary>
    public partial class CustomerForm : Form
    {
        private readonly QuedinDbContext db = new QuedinDbContext();
        private readonly bool isNew;
        private readonly Customer customer;
        private bool loading;

        /// <summary>The ID of the saved person, so a calling screen can select it.</summary>
        public string SavedId { get; private set; }

        public CustomerForm() : this((string)null, PersonTypes.Client)
        {
        }

        /// <summary>New person of a given type (used by the case screen's "add opposing lawyer" etc.).</summary>
        public CustomerForm(string personType) : this(null, personType)
        {
        }

        private CustomerForm(string id, string personType)
        {
            InitializeComponent();
            loading = true;
            cmbPersonType.Items.AddRange(PersonTypes.All);
            cmbIdType.Items.AddRange(IdTypes.All);
            listCases.DoubleClick += listCases_DoubleClick;

            if (id == null)
            {
                isNew = true;
                customer = new Customer();
                cmbPersonType.SelectedItem = personType ?? PersonTypes.Client;
                cmbIdType.SelectedItem = personType == PersonTypes.Client || personType == null ? IdTypes.IsraeliId : IdTypes.None;
                Text = "הוספת " + cmbPersonType.SelectedItem;
                groupBoxCases.Visible = false;
                ClientSize = new System.Drawing.Size(ClientSize.Width, groupBoxCases.Top);
            }
            else
            {
                customer = db.Customers.FirstOrDefault(c => c.CustomerId == id);
                if (customer == null) throw new ArgumentException("Customer not found: " + id);
                FillControls();
                txtIdNumber.ReadOnly = true; // the identifier is the primary key
                cmbIdType.Enabled = false;
                Text = "עדכון פרטי " + customer.PersonType + " - " + customer.DisplayName;
                FillCases();
            }
            loading = false;
            ApplyRules();
        }

        /// <summary>Edit an existing person.</summary>
        public static CustomerForm ForEdit(string id)
        {
            return new CustomerForm(id, null);
        }

        private string PersonType { get { return cmbPersonType.SelectedItem as string; } }

        private string IdType { get { return cmbIdType.SelectedItem as string; } }

        private bool IsClient { get { return PersonType == PersonTypes.Client; } }

        private bool IsCompany { get { return IdType == IdTypes.CompanyNumber; } }

        private void FillControls()
        {
            txtIdNumber.Text = customer.CustomerId;
            cmbPersonType.SelectedItem = customer.PersonType;
            cmbIdType.SelectedItem = customer.IdType ?? IdTypes.IsraeliId;
            txtFirstName.Text = customer.FirstName;
            txtLastName.Text = customer.LastName;
            txtCity.Text = customer.City;
            txtStreet.Text = customer.Street;
            txtHouseNumber.Text = customer.HouseNumber;
            txtPostalCode.Text = customer.PostalCode;
            txtPhone.Text = customer.Phone;
            txtMobilePhone.Text = customer.MobilePhone;
            txtEmail.Text = customer.Email;
            mtbBirthDate.Text = customer.BirthDate.HasValue ? customer.BirthDate.Value.ToString("dd/MM/yyyy") : "";
        }

        /// <summary>Cases in which this person takes part, in any role.</summary>
        private void FillCases()
        {
            string id = customer.CustomerId;
            var cases = db.Cases
                .Include(t => t.CaseType).Include(t => t.CurrentProcedure)
                .Include(t => t.Steps).Include(t => t.Payments)
                .Where(t => t.ClientId == id || t.OpposingPartyId == id || t.OpposingLawyerId == id)
                .OrderByDescending(t => t.CaseId)
                .ToList();

            listCases.Items.Clear();
            foreach (LegalCase t in cases)
            {
                string role = t.ClientId == id ? "לקוח" : t.OpposingPartyId == id ? "צד שכנגד" : "עו\"ד הצד שכנגד";
                var item = new ListViewItem(new[]
                {
                    t.CaseId.ToString(),
                    role,
                    t.CaseType != null ? t.CaseType.Name : "",
                    CaseService.IsFinished(t) ? "הסתיים" : (t.CurrentProcedure != null ? t.CurrentProcedure.Name : ""),
                    t.ClientId == id ? Ui.FormatMoney(CaseService.OpenBalance(t)) : ""
                });
                item.Tag = t.CaseId;
                listCases.Items.Add(item);
            }
            groupBoxCases.Text = "תיקים (" + cases.Count + ") - לחיצה כפולה פותחת את התיק";
        }

        private void listCases_DoubleClick(object sender, EventArgs e)
        {
            if (listCases.SelectedItems.Count == 0) return;
            CaseWindows.OpenCase(Owner ?? (IWin32Window)this, (int)listCases.SelectedItems[0].Tag, null);
        }

        // ------------------------------------------------------------------ rules per type

        private void Rules_Changed(object sender, EventArgs e)
        {
            if (!loading) ApplyRules();
        }

        /// <summary>Labels show which fields are required for the chosen person type and identifier type.</summary>
        private void ApplyRules()
        {
            bool client = IsClient;
            bool none = IdType == IdTypes.None;

            lblId.Text = none ? "מספר מזהה" : (IdType ?? "מספר מזהה") + " *";
            txtIdNumber.Enabled = !none || !isNew;
            if (none && isNew) txtIdNumber.Text = "";
            lblFirstName.Text = IsCompany ? "שם החברה *" : "שם פרטי *";
            lblLastName.Text = IsCompany ? "שם משפחה" : (client ? "שם משפחה *" : "שם משפחה");
            txtLastName.Enabled = !IsCompany;
            if (IsCompany) txtLastName.Text = "";
            mtbBirthDate.Enabled = !IsCompany;

            string star = client ? " *" : "";
            lblCity.Text = "עיר" + star;
            lblStreet.Text = "רחוב" + star;
            lblHouseNumber.Text = "מספר בית" + star;
            lblPhone1.Text = "טלפון" + star;
            lblEmail.Text = "מייל" + star;

            lblIdHint.Text = none
                ? "יישמר קוד פנימי במקום מספר מזהה."
                : IdType == IdTypes.Passport ? "5-20 אותיות לועזיות וספרות." : IsCompany ? "9 ספרות." : "";
            lblRules.Text = client
                ? "ללקוח נדרשים מזהה, כתובת, טלפון ומייל (טלפון בחו\"ל: +44...)."
                : "לצד שכנגד נדרש רק שם. מזהה ופרטי קשר - אם ידועים.";
        }

        // ------------------------------------------------------------------ saving

        private static string Digits(string text)
        {
            return new string((text ?? "").Where(char.IsDigit).ToArray());
        }

        /// <summary>The identifier as it is stored: digits for ID/company numbers, upper-case for passports.</summary>
        private string ReadIdentifier()
        {
            string raw = (txtIdNumber.Text ?? "").Trim();
            if (IdType == IdTypes.Passport) return raw.ToUpperInvariant();
            return Digits(raw);
        }

        /// <summary>A unique internal code for a party whose ID number is unknown ("N" + 8 digits).</summary>
        private string NewInternalCode()
        {
            var random = new Random();
            while (true)
            {
                string code = "N" + random.Next(10000000, 99999999);
                if (!db.Customers.Any(c => c.CustomerId == code)) return code;
            }
        }

        /// <summary>Validates every field, shows all errors at once and copies valid values into the entity.</summary>
        private bool ValidateAndCopy()
        {
            errorProvider1.Clear();
            bool ok = true;
            Action<Control, string> fail = (control, message) => { errorProvider1.SetError(control, message); ok = false; };
            bool client = IsClient;

            if (PersonType == null) fail(cmbPersonType, "יש לבחור סוג");
            if (IdType == null) fail(cmbIdType, "יש לבחור סוג מזהה");
            else if (client && IdType == IdTypes.None) fail(cmbIdType, "ללקוח של המשרד נדרש מספר מזהה (לקבלות)");

            string id = null;
            if (isNew && IdType != null && IdType != IdTypes.None)
            {
                id = ReadIdentifier();
                string idError = Validation.GetIdentifierError(IdType, id);
                if (idError != null) fail(txtIdNumber, idError);
                else if (db.Customers.Any(c => c.CustomerId == id))
                {
                    Customer existing = db.Customers.First(c => c.CustomerId == id);
                    fail(txtIdNumber, "מזהה זה כבר קיים במערכת: " + existing.DisplayName + " (" + existing.PersonType + ")");
                }
            }

            if (IsCompany)
            {
                if (!Validation.IsPartyName(txtFirstName.Text)) fail(txtFirstName, "יש להזין את שם החברה (עד 50 תווים)");
            }
            else
            {
                if (!(client ? Validation.IsName(txtFirstName.Text) : Validation.IsPartyName(txtFirstName.Text))) fail(txtFirstName, "יש להזין שם");
                if (txtLastName.Text.Trim().Length > 0 || client)
                    if (!Validation.IsName(txtLastName.Text)) fail(txtLastName, "יש להזין שם משפחה");
            }

            Func<TextBox, bool> filled = t => t.Text.Trim().Length > 0;
            if (client || filled(txtCity)) { if (!Validation.IsName(txtCity.Text)) fail(txtCity, "יש להזין עיר"); }
            if (client && !filled(txtStreet)) fail(txtStreet, "יש להזין רחוב");
            if (client && !filled(txtHouseNumber)) fail(txtHouseNumber, "יש להזין מספר בית");

            string phone = txtPhone.Text.Trim();
            if ((client || phone.Length > 0) && !Validation.IsPhone(phone))
                fail(txtPhone, "מספר טלפון לא תקין (לדוגמה 02-6543210, 050-1234567 או +44 20 7123 4567)");

            string mobile = txtMobilePhone.Text.Trim();
            if (mobile.Length > 0 && !Validation.IsPhone(mobile)) fail(txtMobilePhone, "מספר נייד לא תקין");

            if ((client || filled(txtEmail)) && !Validation.IsEmail(txtEmail.Text)) fail(txtEmail, "כתובת מייל לא תקינה");

            DateTime? birthDate = null;
            if (!IsCompany && Digits(mtbBirthDate.Text).Length > 0)
            {
                DateTime parsed;
                if (!Validation.TryParseDate(mtbBirthDate.Text, out parsed)) fail(mtbBirthDate, "תאריך לא תקין");
                else if (Validation.IsFutureDate(parsed, DateTime.Today)) fail(mtbBirthDate, "תאריך לידה עתידי");
                else if (client && !Validation.IsAdultBirthDate(parsed, DateTime.Today)) fail(mtbBirthDate, "לקוח חייב להיות בן 18 ומעלה");
                else birthDate = parsed;
            }

            if (!ok) return false;

            if (isNew)
            {
                customer.CustomerId = IdType == IdTypes.None ? NewInternalCode() : id;
                customer.IdType = IdType;
            }
            customer.PersonType = PersonType;
            customer.FirstName = txtFirstName.Text.Trim();
            customer.LastName = IsCompany ? "" : txtLastName.Text.Trim();
            customer.City = NullIfEmpty(txtCity.Text);
            customer.Street = NullIfEmpty(txtStreet.Text);
            customer.HouseNumber = NullIfEmpty(txtHouseNumber.Text);
            customer.PostalCode = NullIfEmpty(txtPostalCode.Text);
            customer.Phone = Validation.NormalizePhone(phone);
            customer.MobilePhone = Validation.NormalizePhone(mobile);
            customer.Email = NullIfEmpty(txtEmail.Text);
            customer.BirthDate = birthDate;
            return true;
        }

        private static string NullIfEmpty(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? null : text.Trim();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValidateAndCopy()) return;
            try
            {
                if (isNew) db.Customers.Add(customer);
                db.SaveChanges();
                SavedId = customer.CustomerId;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Ui.Error("הנתונים לא נשמרו.", ex);
            }
        }

        private void txtFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Company and opposing-party names may contain digits and quotes (בע"מ); person names may not.
            if (sender == txtFirstName && (IsCompany || !IsClient)) return;
            e.Handled = !Validation.IsNameKey(e.KeyChar);
        }

        private void Digits_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !Validation.IsDigitKey(e.KeyChar);
        }

        private void Id_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) ||
                          (IdType == IdTypes.Passport && e.KeyChar < 128 && char.IsLetter(e.KeyChar)));
        }

        private void Phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) || "+- ()".IndexOf(e.KeyChar) >= 0);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            db.Dispose();
            base.OnFormClosed(e);
        }
    }
}
