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
        [DataRow("18")] // too short
        public void IsLegalId_ValidCheckDigit(string id)
        {
            Assert.AreEqual(id.Length >= 5, Validation.IsLegalId(id));
        }

        [DataTestMethod]
        [DataRow("123456789")]
        [DataRow("12345678a")]
        [DataRow("")]
        [DataRow(null)]
        [DataRow("1234567890")]
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
    }
}
