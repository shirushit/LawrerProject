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
    public partial class frmAddCostemer : Form
    {
        private readonly LawyerDBEntities1 db = new LawyerDBEntities1();
        private readonly bool isNew;
        private readonly Customers customer;
        private bool loading;

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
            loading = true;
            cmbSugPerson.Items.AddRange(PersonTypes.All);
            cmbSugMezahe.Items.AddRange(IdTypes.All);
            listCases.DoubleClick += listCases_DoubleClick;

            if (id == null)
            {
                isNew = true;
                customer = new Customers();
                cmbSugPerson.SelectedItem = personType ?? PersonTypes.Client;
                cmbSugMezahe.SelectedItem = personType == PersonTypes.Client || personType == null ? IdTypes.IsraeliId : IdTypes.None;
                Text = "הוספת " + cmbSugPerson.SelectedItem;
                groupBoxCases.Visible = false;
                ClientSize = new System.Drawing.Size(ClientSize.Width, groupBoxCases.Top);
            }
            else
            {
                customer = db.Customers.FirstOrDefault(c => c.IDcustemer == id);
                if (customer == null) throw new ArgumentException("Customer not found: " + id);
                FillControls();
                txtIDcostemer1.ReadOnly = true; // the identifier is the primary key
                cmbSugMezahe.Enabled = false;
                Text = "עדכון פרטי " + customer.SugPerson + " - " + customer.DisplayName;
                FillCases();
            }
            loading = false;
            ApplyRules();
        }

        /// <summary>Edit an existing person.</summary>
        public static frmAddCostemer ForEdit(string id)
        {
            return new frmAddCostemer(id, null);
        }

        private string PersonType { get { return cmbSugPerson.SelectedItem as string; } }

        private string IdType { get { return cmbSugMezahe.SelectedItem as string; } }

        private bool IsClient { get { return PersonType == PersonTypes.Client; } }

        private bool IsCompany { get { return IdType == IdTypes.CompanyNumber; } }

        private void FillControls()
        {
            txtIDcostemer1.Text = customer.IDcustemer;
            cmbSugPerson.SelectedItem = customer.SugPerson;
            cmbSugMezahe.SelectedItem = customer.SugMezahe ?? IdTypes.IsraeliId;
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

        /// <summary>Cases in which this person takes part, in any role.</summary>
        private void FillCases()
        {
            string id = customer.IDcustemer;
            var cases = db.Tikim_Letipul_Orech_Din
                .Include(t => t.Subjekt_Tik).Include(t => t.Taalichim_Letikim)
                .Include(t => t.Tahalich_Tipul_Betik).Include(t => t.Tashlumim_Letik)
                .Where(t => t.IDCostemer == id || t.KodLakoachNegdi == id || t.KodOrechDinNegdi == id)
                .OrderByDescending(t => t.IDTik)
                .ToList();

            listCases.Items.Clear();
            foreach (Tikim_Letipul_Orech_Din t in cases)
            {
                string role = t.IDCostemer == id ? "לקוח" : t.KodLakoachNegdi == id ? "צד שכנגד" : "עו\"ד הצד שכנגד";
                var item = new ListViewItem(new[]
                {
                    t.IDTik.ToString(),
                    role,
                    t.Subjekt_Tik != null ? t.Subjekt_Tik.NameSugTik : "",
                    CaseService.IsFinished(t) ? "הסתיים" : (t.Taalichim_Letikim != null ? t.Taalichim_Letikim.NameTaalich : ""),
                    t.IDCostemer == id ? Ui.FormatMoney(CaseService.OpenBalance(t)) : ""
                });
                item.Tag = t.IDTik;
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
            txtIDcostemer1.Enabled = !none || !isNew;
            if (none && isNew) txtIDcostemer1.Text = "";
            lblFirstName.Text = IsCompany ? "שם החברה *" : "שם פרטי *";
            lblLastName.Text = IsCompany ? "שם משפחה" : (client ? "שם משפחה *" : "שם משפחה");
            txtLastName1.Enabled = !IsCompany;
            if (IsCompany) txtLastName1.Text = "";
            mackBirthDay1.Enabled = !IsCompany;

            string star = client ? " *" : "";
            lblCity.Text = "עיר" + star;
            lblStreet.Text = "רחוב" + star;
            lblHouse.Text = "מספר בית" + star;
            lblPhone1.Text = "טלפון" + star;
            lblMail.Text = "מייל" + star;

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
            string raw = (txtIDcostemer1.Text ?? "").Trim();
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
                if (!db.Customers.Any(c => c.IDcustemer == code)) return code;
            }
        }

        /// <summary>Validates every field, shows all errors at once and copies valid values into the entity.</summary>
        private bool ValidateAndCopy()
        {
            errorProvider1.Clear();
            bool ok = true;
            Action<Control, string> fail = (control, message) => { errorProvider1.SetError(control, message); ok = false; };
            bool client = IsClient;

            if (PersonType == null) fail(cmbSugPerson, "יש לבחור סוג");
            if (IdType == null) fail(cmbSugMezahe, "יש לבחור סוג מזהה");
            else if (client && IdType == IdTypes.None) fail(cmbSugMezahe, "ללקוח של המשרד נדרש מספר מזהה (לקבלות)");

            string id = null;
            if (isNew && IdType != null && IdType != IdTypes.None)
            {
                id = ReadIdentifier();
                string idError = Validation.GetIdentifierError(IdType, id);
                if (idError != null) fail(txtIDcostemer1, idError);
                else if (db.Customers.Any(c => c.IDcustemer == id))
                {
                    Customers existing = db.Customers.First(c => c.IDcustemer == id);
                    fail(txtIDcostemer1, "מזהה זה כבר קיים במערכת: " + existing.DisplayName + " (" + existing.SugPerson + ")");
                }
            }

            if (IsCompany)
            {
                if (!Validation.IsPartyName(txtName1.Text)) fail(txtName1, "יש להזין את שם החברה (עד 50 תווים)");
            }
            else
            {
                if (!(client ? Validation.IsName(txtName1.Text) : Validation.IsPartyName(txtName1.Text))) fail(txtName1, "יש להזין שם");
                if (txtLastName1.Text.Trim().Length > 0 || client)
                    if (!Validation.IsName(txtLastName1.Text)) fail(txtLastName1, "יש להזין שם משפחה");
            }

            Func<TextBox, bool> filled = t => t.Text.Trim().Length > 0;
            if (client || filled(txtCity)) { if (!Validation.IsName(txtCity.Text)) fail(txtCity, "יש להזין עיר"); }
            if (client && !filled(txtAdress1)) fail(txtAdress1, "יש להזין רחוב");
            if (client && !filled(txtHouse)) fail(txtHouse, "יש להזין מספר בית");

            string phone = maskONEPhone1.Text.Trim();
            if ((client || phone.Length > 0) && !Validation.IsPhone(phone))
                fail(maskONEPhone1, "מספר טלפון לא תקין (לדוגמה 02-6543210, 050-1234567 או +44 20 7123 4567)");

            string mobile = maskTWOPhone1.Text.Trim();
            if (mobile.Length > 0 && !Validation.IsPhone(mobile)) fail(maskTWOPhone1, "מספר נייד לא תקין");

            if ((client || filled(txtMail1)) && !Validation.IsEmail(txtMail1.Text)) fail(txtMail1, "כתובת מייל לא תקינה");

            DateTime? birthDate = null;
            if (!IsCompany && Digits(mackBirthDay1.Text).Length > 0)
            {
                DateTime parsed;
                if (!Validation.TryParseDate(mackBirthDay1.Text, out parsed)) fail(mackBirthDay1, "תאריך לא תקין");
                else if (Validation.IsFutureDate(parsed, DateTime.Today)) fail(mackBirthDay1, "תאריך לידה עתידי");
                else if (client && !Validation.IsAdultBirthDate(parsed, DateTime.Today)) fail(mackBirthDay1, "לקוח חייב להיות בן 18 ומעלה");
                else birthDate = parsed;
            }

            if (!ok) return false;

            if (isNew)
            {
                customer.IDcustemer = IdType == IdTypes.None ? NewInternalCode() : id;
                customer.SugMezahe = IdType;
            }
            customer.SugPerson = PersonType;
            customer.FirstName = txtName1.Text.Trim();
            customer.LastName = IsCompany ? "" : txtLastName1.Text.Trim();
            customer.City = NullIfEmpty(txtCity.Text);
            customer.Adress = NullIfEmpty(txtAdress1.Text);
            customer.House = NullIfEmpty(txtHouse.Text);
            customer.Mikud = NullIfEmpty(txtMikud.Text);
            customer.Telephone = Validation.NormalizePhone(phone);
            customer.SmartPhone = Validation.NormalizePhone(mobile);
            customer.Mail = NullIfEmpty(txtMail1.Text);
            customer.DateBirthday = birthDate;
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
                SavedId = customer.IDcustemer;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Ui.Error("הנתונים לא נשמרו.", ex);
            }
        }

        private void txtName1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Company and opposing-party names may contain digits and quotes (בע"מ); person names may not.
            if (sender == txtName1 && (IsCompany || !IsClient)) return;
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
