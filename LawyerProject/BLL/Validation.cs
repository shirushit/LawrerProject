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

        public static bool IsPhone(string phone)
        {
            return IsTelephone(phone) || IsCellPhone(phone);
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
            return GetAge(birthDate, DateTime.Today);
        }

        /// <summary>Clients must be adults; anything over 120 is a typo.</summary>
        public static bool IsAdultBirthDate(DateTime birthDate, DateTime today)
        {
            int age = GetAge(birthDate, today);
            return age >= 18 && age <= 120;
        }

        /// <summary>Parses a non-negative money amount (accepts "1,250.50").</summary>
        public static bool TryParseMoney(string text, out decimal amount)
        {
            bool ok = decimal.TryParse((text ?? string.Empty).Trim(), NumberStyles.Number,
                CultureInfo.InvariantCulture, out amount);
            return ok && amount >= 0;
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
