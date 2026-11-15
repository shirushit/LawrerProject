using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using LawyerProject.BLL;
using LawyerProject.DB;

namespace LawyerProject.GUI
{
    /// <summary>
    /// Office settings shared by every workstation (managers only): office details for receipts, the VAT
    /// rate, the reminder window, and the non-working days the office adds on top of the built-in holidays.
    /// </summary>
    public class SettingsForm : Form
    {
        private TextBox txtOfficeName, txtOfficeAddress, txtOfficePhone, txtOfficeTaxId, txtVatPercent, txtDueSoonDays;
        private ListView listHolidays;
        private DateTimePicker dtpHoliday;
        private TextBox txtHolidayName;
        private Button btnSave, btnAddHoliday, btnRemoveHoliday;

        public SettingsForm()
        {
            InitializeComponent();
            LoadValues();
            LoadHolidays();
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Text = "הגדרות משרד";
            Font = new Font("Calibri", 11F);
            RightToLeft = RightToLeft.Yes;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            ClientSize = new Size(780, 672);

            var office = new GroupBox { Text = "פרטי המשרד (מודפסים על הקבלות) ותזכורות", Location = new Point(20, 12), Size = new Size(740, 282) };
            int y = 30;
            Func<string, int, TextBox> row = (label, width) =>
            {
                office.Controls.Add(new Label { Text = label, Location = new Point(560, y + 3), Size = new Size(165, 23), TextAlign = ContentAlignment.TopRight });
                var box = new TextBox { Location = new Point(550 - width, y), Width = width };
                office.Controls.Add(box);
                y += 34;
                return box;
            };
            txtOfficeName = row("שם המשרד", 330);
            txtOfficeAddress = row("כתובת", 330);
            txtOfficePhone = row("טלפון", 160);
            txtOfficeTaxId = row("מספר עוסק מורשה", 160);
            txtVatPercent = row("מע\"מ (%)", 80);
            txtDueSoonDays = row("תזכורת - ימים מראש", 80);
            btnSave = new Button { Text = "שמירה", Location = new Point(20, 236), Size = new Size(120, 36), UseVisualStyleBackColor = true };
            btnSave.Click += btnSave_Click;
            office.Controls.Add(btnSave);
            office.Controls.Add(new Label
            {
                Text = "שיעור המע\"מ נשמר עם כל תשלום, כך ששינוי שלו לא משנה קבלות ישנות.",
                Location = new Point(150, 243), Size = new Size(575, 23), ForeColor = SystemColors.GrayText, TextAlign = ContentAlignment.TopRight
            });

            var holidays = new GroupBox { Text = "ימים שבהם לא נקבעים מועדים", Location = new Point(20, 304), Size = new Size(740, 355) };
            holidays.Controls.Add(new Label
            {
                Text = "חגי ישראל שבהם בתי המשפט סגורים מחושבים אוטומטית. כאן מוסיפים ימים נוספים, כמו יום העצמאות או פגרה.",
                Location = new Point(15, 25), Size = new Size(710, 44), ForeColor = SystemColors.GrayText, TextAlign = ContentAlignment.TopRight
            });
            listHolidays = new ListView
            {
                Location = new Point(15, 72), Size = new Size(710, 213), View = View.Details, FullRowSelect = true,
                RightToLeft = RightToLeft.Yes, RightToLeftLayout = true, HideSelection = false
            };
            listHolidays.Columns.Add("תאריך", 110);
            listHolidays.Columns.Add("יום", 90);
            listHolidays.Columns.Add("שם", 300);
            listHolidays.Columns.Add("מקור", 180);
            holidays.Controls.Add(listHolidays);
            dtpHoliday = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Location = new Point(595, 300), Width = 130, RightToLeftLayout = true };
            txtHolidayName = new TextBox { Location = new Point(385, 300), Width = 200, MaxLength = 50 };
            btnAddHoliday = new Button { Text = "הוספה", Location = new Point(265, 297), Size = new Size(110, 34), UseVisualStyleBackColor = true };
            btnRemoveHoliday = new Button { Text = "הסרת היום שנבחר", Location = new Point(15, 297), Size = new Size(170, 34), UseVisualStyleBackColor = true };
            btnAddHoliday.Click += btnAddHoliday_Click;
            btnRemoveHoliday.Click += btnRemoveHoliday_Click;
            holidays.Controls.AddRange(new Control[] { dtpHoliday, txtHolidayName, btnAddHoliday, btnRemoveHoliday });

            Controls.Add(office);
            Controls.Add(holidays);
            ResumeLayout(false);
        }

        private void LoadValues()
        {
            txtOfficeName.Text = AppSettings.OfficeName;
            txtOfficeAddress.Text = AppSettings.OfficeAddress;
            txtOfficePhone.Text = AppSettings.OfficePhone;
            txtOfficeTaxId.Text = AppSettings.OfficeTaxId;
            txtVatPercent.Text = (AppSettings.VatRate * 100).ToString("0.##", CultureInfo.InvariantCulture);
            txtDueSoonDays.Text = AppSettings.DueSoonDays.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            decimal percent;
            if (!decimal.TryParse(txtVatPercent.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out percent) || percent < 0 || percent >= 100)
            {
                Ui.Error("מע\"מ חייב להיות מספר בין 0 ל-100 (לדוגמה 18).");
                return;
            }
            var changes = new Dictionary<string, string>
            {
                { AppSettings.OfficeNameKey, txtOfficeName.Text },
                { AppSettings.OfficeAddressKey, txtOfficeAddress.Text },
                { AppSettings.OfficePhoneKey, txtOfficePhone.Text },
                { AppSettings.OfficeTaxIdKey, txtOfficeTaxId.Text },
                { AppSettings.VatRateKey, (percent / 100m).ToString("0.####", CultureInfo.InvariantCulture) },
                { AppSettings.DueSoonDaysKey, txtDueSoonDays.Text.Trim() }
            };
            try
            {
                using (var db = new QuedinDbContext())
                {
                    string error = AppSettings.Save(db, changes);
                    if (error != null)
                    {
                        Ui.Error(error);
                        return;
                    }
                }
                Ui.Notify(this, "ההגדרות נשמרו לכל עמדות המשרד.");
            }
            catch (Exception ex)
            {
                Ui.Error("ההגדרות לא נשמרו.", ex);
            }
        }

        // ------------------------------------------------------------------ holidays

        private void LoadHolidays()
        {
            listHolidays.Items.Clear();
            int year = Clock.Today.Year;
            var rows = new List<Tuple<DateTime, string, bool>>();
            try
            {
                using (var db = new QuedinDbContext())
                    rows.AddRange(db.Holidays.ToList().Select(h => Tuple.Create(h.HolidayDate, h.Name, true)));
            }
            catch (Exception ex)
            {
                Ui.Error("לא ניתן לטעון את רשימת הימים.", ex);
            }
            for (int y = year; y <= year + 1; y++)
                rows.AddRange(BusinessCalendar.JewishHolidays(y).Select(h => Tuple.Create(h.Key, h.Value, false)));

            foreach (var r in rows.Where(r => r.Item1 >= Clock.Today.AddDays(-30)).OrderBy(r => r.Item1))
            {
                var item = new ListViewItem(new[] { r.Item1.ToString("dd/MM/yyyy"), Ui.DayName(r.Item1), r.Item2, r.Item3 ? "נוסף על ידי המשרד" : "חג (אוטומטי)" });
                item.Tag = r.Item3 ? (object)r.Item1 : null;
                if (!r.Item3) item.ForeColor = SystemColors.GrayText;
                listHolidays.Items.Add(item);
            }
        }

        private void btnAddHoliday_Click(object sender, EventArgs e)
        {
            DateTime date = dtpHoliday.Value.Date;
            string name = txtHolidayName.Text.Trim();
            if (name.Length == 0)
            {
                Ui.Error("יש לכתוב שם ליום (לדוגמה: יום העצמאות).");
                return;
            }
            try
            {
                using (var db = new QuedinDbContext())
                {
                    if (db.Holidays.Find(date) != null)
                    {
                        Ui.Error("היום הזה כבר ברשימה.");
                        return;
                    }
                    CatalogService.RequireManager();
                    db.Holidays.Add(new Holiday { HolidayDate = date, Name = name });
                    AuditService.Log(db, AuditActions.HolidayAdded, "יום חופש", date.ToString("dd/MM/yyyy"), name);
                    db.SaveChanges();
                    AppSettings.Load(db);
                }
                txtHolidayName.Clear();
                LoadHolidays();
                Ui.Notify(this, "נוסף: " + name + ". מועדים חדשים לא ייפלו ביום הזה.");
            }
            catch (Exception ex)
            {
                Ui.Error("היום לא נוסף.", ex);
            }
        }

        private void btnRemoveHoliday_Click(object sender, EventArgs e)
        {
            if (listHolidays.SelectedItems.Count == 0 || listHolidays.SelectedItems[0].Tag == null)
            {
                Ui.Error("יש לבחור יום שהמשרד הוסיף (חגים אוטומטיים לא ניתן להסיר).");
                return;
            }
            var date = (DateTime)listHolidays.SelectedItems[0].Tag;
            try
            {
                using (var db = new QuedinDbContext())
                {
                    Holiday row = db.Holidays.Find(date);
                    if (row != null)
                    {
                        CatalogService.RequireManager();
                        db.Holidays.Remove(row);
                        AuditService.Log(db, AuditActions.HolidayRemoved, "יום חופש", date.ToString("dd/MM/yyyy"), row.Name);
                        db.SaveChanges();
                    }
                    AppSettings.Load(db);
                }
                LoadHolidays();
            }
            catch (Exception ex)
            {
                Ui.Error("היום לא הוסר.", ex);
            }
        }
    }
}
