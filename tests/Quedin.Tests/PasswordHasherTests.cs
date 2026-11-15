using LawyerProject.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quedin.Tests
{
    [TestClass]
    public class PasswordHasherTests
    {
        [TestMethod]
        public void Hash_ThenVerify_Succeeds()
        {
            string stored = PasswordHasher.Hash("Quedin2024", 1000);

            Assert.IsTrue(stored.StartsWith("pbkdf2$1000$"));
            Assert.IsTrue(PasswordHasher.Verify("Quedin2024", stored));
        }

        [TestMethod]
        public void Verify_WrongPassword_Fails()
        {
            string stored = PasswordHasher.Hash("Quedin2024", 1000);

            Assert.IsFalse(PasswordHasher.Verify("quedin2024", stored));
            Assert.IsFalse(PasswordHasher.Verify("", stored));
        }

        [TestMethod]
        public void Hash_UsesRandomSalt()
        {
            Assert.AreNotEqual(PasswordHasher.Hash("same", 1000), PasswordHasher.Hash("same", 1000));
        }

        [TestMethod]
        public void Hash_FitsTheDatabaseColumn()
        {
            Assert.IsTrue(PasswordHasher.Hash("Quedin2024").Length <= 200);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("12345")]                       // legacy plain-text password
        [DataRow("pbkdf2$abc$salt$hash")]
        [DataRow("pbkdf2$1000$***$***")]
        public void Verify_MalformedStoredValue_ReturnsFalse(string stored)
        {
            Assert.IsFalse(PasswordHasher.Verify("12345", stored));
        }

        [TestMethod]
        public void DemoUsersInSqlScript_VerifyWithTheirDocumentedPasswords()
        {
            // Hashes copied from database/create-database.sql (generated with Python's hashlib).
            Assert.IsTrue(PasswordHasher.Verify("Quedin2024",
                "pbkdf2$100000$cXVlZGluLWRlbW8tc2FsdDE=$Kmr3WtJMlhRlHwEsl64u3hXYgOI4X3NMPBL5JyRKBa4="));
            Assert.IsTrue(PasswordHasher.Verify("Worker2024",
                "pbkdf2$100000$cXVlZGluLWRlbW8tc2FsdDI=$D4bLJ4IlgXxnzMguYNwv+DaETNIhWIS73LeJBgkOPsA="));
        }

        [TestMethod]
        public void HashWithSalt_RecreatesTheStoredHashOnlyForTheRightPassword()
        {
            string stored = PasswordHasher.Hash("Quedin2024", 1000);
            string saltPrefix = stored.Substring(0, stored.LastIndexOf('$') + 1);

            Assert.AreEqual(stored, PasswordHasher.HashWithSalt("Quedin2024", saltPrefix));
            Assert.AreNotEqual(stored, PasswordHasher.HashWithSalt("quedin2024", saltPrefix));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("legacy$")]
        [DataRow("pbkdf2$abc$c2FsdA==$")]
        [DataRow("pbkdf2$1000$not base64!$")]
        [DataRow("pbkdf2$1000$c2FsdA==$extra")]
        public void HashWithSalt_RejectsABadPrefix(string saltPrefix)
        {
            Assert.IsNull(PasswordHasher.HashWithSalt("Quedin2024", saltPrefix));
        }
    }
}
