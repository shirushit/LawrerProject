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
