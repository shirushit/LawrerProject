using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LawyerProject.BLL
{
    /// <summary>
    /// Input validation rules. Pure functions only: no UI, no database,
    /// so every rule can be unit-tested (see tests/Quedin.Tests).
    /// </summary>
    public static class Validation
    {
        public const int MinPasswordLength = 8;

        private static readonly Regex LandlineRegex = new Regex(@"^0(2|3|4|8|9|7[2-9])-?\d{7}$", RegexOptions.Compiled);
        private static readonly Regex CellPhoneRegex = new Regex(@"^05\d-?\d{7}$", RegexOptions.Compiled);
        private static readonly Regex EmailRegex = new Regex(@"^[A-Za-z0-9._%+\-]+@[A-Za-z0-9.\-]+\.[A-Za-z]{2,}$", RegexOptions.Compiled);
        private static readonly Regex HebrewOrEnglishNameRegex = new Regex(@"^[א-תA-Za-z][א-תA-Za-z '\-]*$", RegexOptions.Compiled);
        // Company / organization names: also digits, quotes (בע"מ, בע״מ), geresh, dot, ampersand, parentheses.
        private static readonly Regex PartyNameRegex = new Regex(@"^[א-תA-Za-z0-9][א-תA-Za-z0-9 '\-""״׳.,&()/]*$", RegexOptions.Compiled);
        private static readonly Regex InternationalPhoneRegex = new Regex(@"^\+[1-9]\d{7,14}$", RegexOptions.Compiled);
        private static readonly Regex PassportRegex = new Regex(@"^[A-Za-z0-9]{5,20}$", RegexOptions.Compiled);
        // Money: plain digits or correctly grouped thousands, at most 2 decimals. "12,5" is rejected, not read as 125.
        private static readonly Regex MoneyRegex = new Regex(@"^(\d+|\d{1,3}(,\d{3})+)(\.\d{1,2})?$", RegexOptions.Compiled);

        // ---------- Whole-value checks ----------

        /// <summary>Israeli ID number (Teudat Zehut) with check digit.</summary>
        public static bool IsLegalId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;
            id = id.Trim();
            if (id.Length < 5 || id.Length > 9) return false;
            foreach (char ch in id)
                if (ch < '0' || ch > '9') return false;

            id = id.PadLeft(9, '0');
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int k = ((i % 2) + 1) * (id[i] - '0');
                if (k > 9) k -= 9;
                sum += k;
            }
            return sum % 10 == 0;
        }

        /// <summary>Israeli landline: 02/03/04/08/09/07x, with or without a dash.</summary>
        public static bool IsTelephone(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) && LandlineRegex.IsMatch(phone.Trim());
        }

        /// <summary>Israeli mobile number: 05x-xxxxxxx, with or without a dash.</summary>
        public static bool IsCellPhone(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) && CellPhoneRegex.IsMatch(phone.Trim());
        }

        /// <summary>Israeli landline or mobile, or an international number such as +44 20 7123 4567.</summary>
        public static bool IsPhone(string phone)
        {
            if (IsTelephone(phone) || IsCellPhone(phone)) return true;
            string normalized = NormalizePhone(phone);
            return normalized != null && InternationalPhoneRegex.IsMatch(normalized);
        }

        /// <summary>Removes spaces, dashes and brackets so phones are stored the same way (max 20 characters).</summary>
        public static string NormalizePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return null;
            var sb = new System.Text.StringBuilder();
            foreach (char ch in phone.Trim())
                if (char.IsDigit(ch) || (ch == '+' && sb.Length == 0)) sb.Append(ch);
            return sb.Length == 0 ? null : sb.ToString();
        }

        /// <summary>A person or a company name (allows digits, quotes and punctuation used in company names).</summary>
        public static bool IsPartyName(string text)
        {
            return !string.IsNullOrWhiteSpace(text) && text.Trim().Length <= 50 && PartyNameRegex.IsMatch(text.Trim());
        }

        /// <summary>
        /// Checks an identifier by its type: Israeli ID / company number (check digit), passport (5-20 letters
        /// and digits). Returns null when valid, otherwise a Hebrew error message.
        /// </summary>
        public static string GetIdentifierError(string idType, string value)
        {
            string v = (value ?? string.Empty).Trim();
            if (idType == IdTypes.IsraeliId)
                return IsLegalId(v) ? null : "מספר תעודת זהות לא תקין";
            if (idType == IdTypes.CompanyNumber)
                return v.Length == 9 && IsLegalId(v) ? null : "מספר חברה (ח.פ.) לא תקין - 9 ספרות";
            if (idType == IdTypes.Passport)
                return PassportRegex.IsMatch(v) ? null : "מספר דרכון: 5-20 אותיות לועזיות וספרות";
            if (idType == IdTypes.None)
                return null;
            return "יש לבחור סוג מזהה";
        }

        public static bool IsEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email.Trim());
        }

        /// <summary>A person or place name in Hebrew or English letters.</summary>
        public static bool IsName(string text)
        {
            return !string.IsNullOrWhiteSpace(text) && HebrewOrEnglishNameRegex.IsMatch(text.Trim());
        }

        /// <summary>Percent between 0 and 100 inclusive.</summary>
        public static bool IsPercent(double value)
        {
            return value >= 0 && value <= 100;
        }

        /// <summary>
        /// Returns null when the password is valid, otherwise a Hebrew message
        /// describing every rule that failed.
        /// </summary>
        public static string GetPasswordError(string password)
        {
            if (password == null) password = string.Empty;
            bool hasDigit = false, hasLetter = false;
            foreach (char ch in password)
            {
                if (char.IsDigit(ch)) hasDigit = true;
                if (char.IsLetter(ch)) hasLetter = true;
            }

            var problems = new System.Collections.Generic.List<string>();
            if (password.Length < MinPasswordLength) problems.Add("לפחות " + MinPasswordLength + " תווים");
            if (!hasLetter) problems.Add("לפחות אות אחת");
            if (!hasDigit) problems.Add("לפחות ספרה אחת");

            return problems.Count == 0 ? null : "הסיסמה חייבת להכיל: " + string.Join(", ", problems);
        }

        public static bool IsValidPassword(string password)
        {
            return GetPasswordError(password) == null;
        }

        /// <summary>Parses dd/MM/yyyy (the format of the masked date boxes).</summary>
        public static bool TryParseDate(string text, out DateTime date)
        {
            return DateTime.TryParseExact((text ?? string.Empty).Trim(), "dd/MM/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }

        public static int GetAge(DateTime birthDate, DateTime today)
        {
            int age = today.Year - birthDate.Year;
            if (today < birthDate.AddYears(age)) age--;
            return age;
        }

        public static int GetAge(DateTime birthDate)
        {
            return GetAge(birthDate, Clock.Today);
        }

        /// <summary>Clients must be adults; anything over 120 is a typo.</summary>
        public static bool IsAdultBirthDate(DateTime birthDate, DateTime today)
        {
            int age = GetAge(birthDate, today);
            return age >= 18 && age <= 120;
        }

        /// <summary>
        /// Parses a non-negative money amount: "1250", "1,250.50" or "1250.5".
        /// Rejects ambiguous input such as "12,5" (which a lenient parser reads as 125) and more than 2 decimals.
        /// </summary>
        public static bool TryParseMoney(string text, out decimal amount)
        {
            amount = 0m;
            string t = (text ?? string.Empty).Trim();
            if (!MoneyRegex.IsMatch(t)) return false;
            return decimal.TryParse(t.Replace(",", string.Empty), NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out amount);
        }

        /// <summary>A payment amount: at least 0.01.</summary>
        public static bool TryParsePayment(string text, out decimal amount)
        {
            return TryParseMoney(text, out amount) && amount >= 0.01m;
        }

        public static bool IsFutureDate(DateTime date, DateTime today)
        {
            return date.Date > today.Date;
        }

        // ---------- Key-press filters (used by TextBox.KeyPress) ----------

        private static bool IsControlKey(char ch)
        {
            return ch == '\b' || char.IsControl(ch);
        }

        public static bool IsDigitKey(char ch)
        {
            return IsControlKey(ch) || (ch >= '0' && ch <= '9');
        }

        public static bool IsMoneyKey(char ch)
        {
            return IsDigitKey(ch) || ch == '.' || ch == ',';
        }

        /// <summary>Digits plus the separators used in land registry numbers (gush/chelka).</summary>
        public static bool IsRegistryKey(char ch)
        {
            return IsDigitKey(ch) || ch == '/' || ch == '-';
        }

        public static bool IsNameKey(char ch)
        {
            return IsControlKey(ch) || ch == ' ' || ch == '-' || ch == '\''
                || (ch >= 'א' && ch <= 'ת')
                || (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z');
        }
    }
}
