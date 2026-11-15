using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using LawyerProject.DB;

namespace LawyerProject.BLL
{
    /// <summary>
    /// Office-wide settings (VAT rate, office details, reminder window). They live in the Settings table
    /// so every workstation uses the same values; App.config only supplies defaults for a new database.
    /// </summary>
    public static class AppSettings
    {
        public const string VatRateKey = "VatRate";
        public const string DueSoonDaysKey = "DueSoonDays";
        public const string OfficeNameKey = "OfficeName";
        public const string OfficeAddressKey = "OfficeAddress";
        public const string OfficePhoneKey = "OfficePhone";
        public const string OfficeTaxIdKey = "OfficeTaxId";

        private static Dictionary<string, string> values = new Dictionary<string, string>();

        public static decimal VatRate
        {
            get { return ReadDecimal(VatRateKey, 0.18m); }
        }

        public static int DueSoonDays
        {
            get { return (int)ReadDecimal(DueSoonDaysKey, 7m); }
        }

        public static string OfficeName { get { return ReadString(OfficeNameKey, "משרד עורכי דין"); } }
        public static string OfficeAddress { get { return ReadString(OfficeAddressKey, ""); } }
        public static string OfficeTaxId { get { return ReadString(OfficeTaxIdKey, ""); } }
        public static string OfficePhone { get { return ReadString(OfficePhoneKey, ""); } }

        /// <summary>Reads all settings and the office's extra holidays from the database (at login and after a change).</summary>
        public static void Load(QuedinDbContext db)
        {
            values = db.Settings.ToList().ToDictionary(s => s.SettingKey, s => s.Value ?? "");
            BusinessCalendar.SetExtraHolidays(db.Holidays.ToList().ToDictionary(h => h.HolidayDate, h => h.Name));
        }

        /// <summary>Saves the given settings (managers only). Returns null or a Hebrew error message.</summary>
        public static string Save(QuedinDbContext db, IDictionary<string, string> changes)
        {
            if (!Session.IsManager) return "רק מנהל יכול לשנות הגדרות.";
            decimal vat;
            string rawVat;
            if (changes.TryGetValue(VatRateKey, out rawVat) &&
                (!decimal.TryParse(rawVat, NumberStyles.Number, CultureInfo.InvariantCulture, out vat) || vat < 0 || vat >= 1))
                return "שיעור מע\"מ לא תקין (לדוגמה 0.18).";
            int days;
            string rawDays;
            if (changes.TryGetValue(DueSoonDaysKey, out rawDays) && (!int.TryParse(rawDays, out days) || days < 1 || days > 60))
                return "מספר הימים לתזכורת חייב להיות בין 1 ל-60.";

            foreach (KeyValuePair<string, string> change in changes)
            {
                string value = (change.Value ?? "").Trim();
                Setting row = db.Settings.Find(change.Key);
                if (row == null) db.Settings.Add(new Setting { SettingKey = change.Key, Value = value });
                else if (row.Value != value) row.Value = value;
                else continue;
                AuditService.Log(db, AuditActions.SettingsChanged, "הגדרות", change.Key, value);
            }
            db.SaveChanges();
            Load(db);
            return null;
        }

        private static string ReadString(string key, string fallback)
        {
            string raw;
            if (!values.TryGetValue(key, out raw) || string.IsNullOrWhiteSpace(raw))
                raw = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(raw) ? fallback : raw.Trim();
        }

        private static decimal ReadDecimal(string key, decimal fallback)
        {
            decimal value;
            return decimal.TryParse(ReadString(key, ""), NumberStyles.Number, CultureInfo.InvariantCulture, out value)
                ? value
                : fallback;
        }
    }
}
