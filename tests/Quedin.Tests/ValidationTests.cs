using System;
using LawyerProject.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quedin.Tests
{
    [TestClass]
    public class ValidationTests
    {
        [DataTestMethod]
        [DataRow("000000018")]
        [DataRow("123456782")]
        [DataRow("12345674")] // 8 digits: read as 012345674
        public void IsLegalId_ValidCheckDigit(string id)
        {
            Assert.IsTrue(Validation.IsLegalId(id));
        }

        [DataTestMethod]
        [DataRow("123456789")]
        [DataRow("12345678a")]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("1234567890")]
        [DataRow("18")] // the check digit is right, but an ID is at least 5 digits
        public void IsLegalId_Rejects(string id)
        {
            Assert.IsFalse(Validation.IsLegalId(id));
        }

        [DataTestMethod]
        [DataRow("02-6543210", true)]
        [DataRow("026543210", true)]
        [DataRow("077-1234567", true)]
        [DataRow("050-1234567", false)]
        [DataRow("01-1234567", false)]
        public void IsTelephone(string phone, bool expected)
        {
            Assert.AreEqual(expected, Validation.IsTelephone(phone));
        }

        [DataTestMethod]
        [DataRow("050-1234567", true)]
        [DataRow("0521234567", true)]
        [DataRow("05-1234567", false)]
        [DataRow("04-1234567", false)]
        public void IsCellPhone(string phone, bool expected)
        {
            Assert.AreEqual(expected, Validation.IsCellPhone(phone));
        }

        [DataTestMethod]
        [DataRow("dana.levi@example.com", true)]
        [DataRow("Office2024@law-firm.co.il", true)]
        [DataRow("no-at-sign.com", false)]
        [DataRow("a@b", false)]
        [DataRow("", false)]
        public void IsEmail(string email, bool expected)
        {
            Assert.AreEqual(expected, Validation.IsEmail(email));
        }

        [DataTestMethod]
        [DataRow("דנה", true)]
        [DataRow("בן-חיים", true)]
        [DataRow("O'Brien", true)]
        [DataRow("Dana2", false)]
        [DataRow("   ", false)]
        public void IsName(string name, bool expected)
        {
            Assert.AreEqual(expected, Validation.IsName(name));
        }

        [TestMethod]
        public void PasswordRules()
        {
            Assert.IsTrue(Validation.IsValidPassword("Quedin2024"));
            Assert.IsFalse(Validation.IsValidPassword("short1"));
            Assert.IsFalse(Validation.IsValidPassword("noDigitsHere"));
            Assert.IsFalse(Validation.IsValidPassword("1234567890"));
            Assert.IsNull(Validation.GetPasswordError("Quedin2024"));
            StringAssert.Contains(Validation.GetPasswordError("abc"), "8");
        }

        [TestMethod]
        public void Age_CountsBirthdayCorrectly()
        {
            var birth = new DateTime(2006, 6, 15);
            Assert.AreEqual(17, Validation.GetAge(birth, new DateTime(2024, 6, 14)));
            Assert.AreEqual(18, Validation.GetAge(birth, new DateTime(2024, 6, 15)));
            Assert.IsFalse(Validation.IsAdultBirthDate(birth, new DateTime(2024, 6, 14)));
            Assert.IsTrue(Validation.IsAdultBirthDate(birth, new DateTime(2024, 6, 15)));
        }

        [TestMethod]
        public void TryParseDate_UsesDayMonthYear()
        {
            DateTime d;
            Assert.IsTrue(Validation.TryParseDate("03/04/2024", out d));
            Assert.AreEqual(new DateTime(2024, 4, 3), d);
            Assert.IsFalse(Validation.TryParseDate("31/02/2024", out d));
            Assert.IsFalse(Validation.TryParseDate("  /  /", out d));
        }

        [TestMethod]
        public void KeyFilters()
        {
            Assert.IsTrue(Validation.IsDigitKey('7'));
            Assert.IsTrue(Validation.IsDigitKey('\b'));
            Assert.IsFalse(Validation.IsDigitKey('a'));
            Assert.IsTrue(Validation.IsMoneyKey('.'));
            Assert.IsTrue(Validation.IsNameKey('ש'));
            Assert.IsFalse(Validation.IsNameKey('5'));
            Assert.IsTrue(Validation.IsRegistryKey('/'));
        }

        // ---------------------------------------------------------- money parsing edge cases

        [DataTestMethod]
        [DataRow("1250", 1250.0)]
        [DataRow("1,250.50", 1250.5)]
        [DataRow("1250.5", 1250.5)]
        [DataRow("0.01", 0.01)]
        [DataRow(" 18,200 ", 18200.0)]
        public void TryParseMoney_AcceptsClearAmounts(string text, double expected)
        {
            decimal amount;
            Assert.IsTrue(Validation.TryParseMoney(text, out amount), text);
            Assert.AreEqual((decimal)expected, amount);
        }

        [DataTestMethod]
        [DataRow("12,5")]      // used to become 125
        [DataRow("1,25,000")]
        [DataRow("10.555")]    // more than 2 decimals
        [DataRow("-5")]
        [DataRow("1.2.3")]
        [DataRow("")]
        [DataRow("abc")]
        public void TryParseMoney_RejectsAmbiguousInput(string text)
        {
            decimal amount;
            Assert.IsFalse(Validation.TryParseMoney(text, out amount), text);
        }

        [TestMethod]
        public void TryParsePayment_RequiresAtLeastOneAgora()
        {
            decimal amount;
            Assert.IsFalse(Validation.TryParsePayment("0", out amount));
            Assert.IsFalse(Validation.TryParsePayment("0.004", out amount)); // used to be saved as 0
            Assert.IsTrue(Validation.TryParsePayment("0.01", out amount));
        }

        // ---------------------------------------------------------- identifiers

        [TestMethod]
        public void GetIdentifierError_ChecksByType()
        {
            Assert.IsNull(Validation.GetIdentifierError(IdTypes.IsraeliId, "000000018"));
            Assert.IsNotNull(Validation.GetIdentifierError(IdTypes.IsraeliId, "123456789"));
            Assert.IsNull(Validation.GetIdentifierError(IdTypes.CompanyNumber, "510000003"));
            Assert.IsNotNull(Validation.GetIdentifierError(IdTypes.CompanyNumber, "18"), "a company number has 9 digits");
            Assert.IsNull(Validation.GetIdentifierError(IdTypes.Passport, "AB123456"));
            Assert.IsNotNull(Validation.GetIdentifierError(IdTypes.Passport, "AB1"));
            Assert.IsNull(Validation.GetIdentifierError(IdTypes.None, ""), "opposing party without a known ID");
            Assert.IsNotNull(Validation.GetIdentifierError(null, "000000018"));
        }

        // ---------------------------------------------------------- phones and names

        [DataTestMethod]
        [DataRow("+44 20 7123 4567")]
        [DataRow("+972-50-1234567")]
        [DataRow("050-1234567")]
        [DataRow("03-6123456")]
        public void IsPhone_AcceptsIsraeliAndInternational(string phone)
        {
            Assert.IsTrue(Validation.IsPhone(phone), phone);
        }

        [DataTestMethod]
        [DataRow("+0123456789")]
        [DataRow("+12")]
        [DataRow("12345")]
        public void IsPhone_RejectsInvalid(string phone)
        {
            Assert.IsFalse(Validation.IsPhone(phone), phone);
        }

        [TestMethod]
        public void NormalizePhone_KeepsDigitsAndLeadingPlus()
        {
            Assert.AreEqual("+442071234567", Validation.NormalizePhone(" +44 (20) 7123-4567 "));
            Assert.AreEqual("0501234567", Validation.NormalizePhone("050-123-4567"));
            Assert.IsNull(Validation.NormalizePhone("  "));
        }

        [DataTestMethod]
        [DataRow("אלפא נכסים בע\"מ")]
        [DataRow("אלפא נכסים בע״מ")]
        [DataRow("Smith & Sons (2020) Ltd.")]
        [DataRow("כהן")]
        public void IsPartyName_AcceptsCompanyNames(string name)
        {
            Assert.IsTrue(Validation.IsPartyName(name), name);
        }

        [TestMethod]
        public void IsPartyName_RejectsEmptyOrTooLong()
        {
            Assert.IsFalse(Validation.IsPartyName(" "));
            Assert.IsFalse(Validation.IsPartyName(new string('א', 51)));
            Assert.IsFalse(Validation.IsPartyName("@company"));
        }

        [TestMethod]
        public void IsFutureDate_ComparesDaysOnly()
        {
            DateTime today = new DateTime(2026, 11, 12);
            Assert.IsFalse(Validation.IsFutureDate(today.AddHours(23), today));
            Assert.IsTrue(Validation.IsFutureDate(today.AddDays(1), today));
        }

        // ---------- identifiers and money limits (v1.3.1)

        [DataTestMethod]
        [DataRow("12345674", "012345674")]
        [DataRow("012345674", "012345674")]
        [DataRow(" 123456782 ", "123456782")]
        [DataRow("AB12345", "AB12345")] // a passport is only trimmed
        [DataRow("", "")]
        public void NormalizeIdNumber_PadsDigitsToNine(string input, string expected)
        {
            Assert.AreEqual(expected, Validation.NormalizeIdNumber(input));
        }

        [TestMethod]
        public void NormalizeIdNumber_SameIdWithOrWithoutLeadingZero_IsTheSameKey()
        {
            Assert.AreEqual(Validation.NormalizeIdNumber("12345674"), Validation.NormalizeIdNumber("012345674"));
        }

        [TestMethod]
        public void TryParseMoney_RejectsAmountsAboveTheLimit()
        {
            decimal amount;
            Assert.IsTrue(Validation.TryParseMoney("1,000,000,000", out amount));
            Assert.AreEqual(Validation.MaxMoney, amount);
            Assert.IsFalse(Validation.TryParseMoney("1000000000.01", out amount));
            Assert.IsFalse(Validation.TryParseMoney("1000000000000000", out amount), "a typo must not reach the database");
        }

        [TestMethod]
        public void IsMoneyInRange_Bounds()
        {
            Assert.IsTrue(Validation.IsMoneyInRange(0m));
            Assert.IsTrue(Validation.IsMoneyInRange(Validation.MaxMoney));
            Assert.IsFalse(Validation.IsMoneyInRange(-0.01m));
            Assert.IsFalse(Validation.IsMoneyInRange(Validation.MaxMoney + 0.01m));
        }
    }
}
