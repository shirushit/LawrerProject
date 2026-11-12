using System.Configuration;
using System.Globalization;

namespace LawyerProject.BLL
{
    /// <summary>Values read from App.config (appSettings).</summary>
    public static class AppSettings
    {
        public static decimal VatRate
        {
            get { return ReadDecimal("VatRate", 0.18m); }
        }

        public static int DueSoonDays
        {
            get { return (int)ReadDecimal("DueSoonDays", 7m); }
        }

        /// <summary>Office details printed on receipts.</summary>
        public static string OfficeName { get { return ReadString("OfficeName", "משרד עורכי דין"); } }
        public static string OfficeAddress { get { return ReadString("OfficeAddress", ""); } }
        public static string OfficeTaxId { get { return ReadString("OfficeTaxId", ""); } }
        public static string OfficePhone { get { return ReadString("OfficePhone", ""); } }

        private static string ReadString(string key, string fallback)
        {
            string raw = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(raw) ? fallback : raw.Trim();
        }

        private static decimal ReadDecimal(string key, decimal fallback)
        {
            decimal value;
            string raw = ConfigurationManager.AppSettings[key];
            return decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out value)
                ? value
                : fallback;
        }
    }
}
