using System;
using System.IO;
using LawyerProject.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Quedin.Tests
{
    [TestClass]
    public class LoginPolicyAndClockTests
    {
        [TestMethod]
        public void LoginPolicy_IsLockedUntilTheTimePasses()
        {
            DateTime now = new DateTime(2026, 11, 15, 10, 0, 0);
            DateTime lockedUntil = now.AddMinutes(LoginPolicy.LockMinutes);
            Assert.IsTrue(LoginPolicy.IsLocked(lockedUntil, now.AddMinutes(LoginPolicy.LockMinutes - 1)));
            Assert.IsFalse(LoginPolicy.IsLocked(lockedUntil, now.AddMinutes(LoginPolicy.LockMinutes + 1)));
            Assert.IsFalse(LoginPolicy.IsLocked(null, now), "never locked");
        }

        [TestMethod]
        public void LoginPolicy_MatchesTheDatabaseProcedure()
        {
            // usp_TryLogin enforces the lock in the database; the app shows these numbers in its messages.
            string sql = File.ReadAllText(FindRepoFile(Path.Combine("database", "create-database.sql")));
            int start = sql.IndexOf("CREATE PROCEDURE [dbo].[usp_TryLogin]", StringComparison.Ordinal);
            Assert.IsTrue(start >= 0, "usp_TryLogin not found");
            string procedure = sql.Substring(start, sql.IndexOf("\nGO", start, StringComparison.Ordinal) - start);

            StringAssert.Contains(procedure, "[FailedLogins] + 1 >= " + LoginPolicy.MaxFailures + " ");
            StringAssert.Contains(procedure, "DATEADD(minute, " + LoginPolicy.LockMinutes + ", @now)");
        }

        /// <summary>Finds a file of the repository by walking up from the test's folder.</summary>
        private static string FindRepoFile(string relativePath)
        {
            foreach (string start in new[] { AppDomain.CurrentDomain.BaseDirectory, Environment.CurrentDirectory })
            {
                for (var dir = new DirectoryInfo(start); dir != null; dir = dir.Parent)
                {
                    string candidate = Path.Combine(dir.FullName, relativePath);
                    if (File.Exists(candidate)) return candidate;
                }
            }
            Assert.Inconclusive(relativePath + " was not found above " + AppDomain.CurrentDomain.BaseDirectory);
            return null;
        }

        [TestMethod]
        public void Clock_FollowsTheServerTime()
        {
            try
            {
                Clock.SetServerTime(DateTime.Now.AddDays(1));
                // Compare Clock with the PC clock at the same moment (no race around midnight).
                TimeSpan ahead = Clock.Now - DateTime.Now;
                Assert.IsTrue(Math.Abs((ahead - TimeSpan.FromDays(1)).TotalSeconds) < 5, "the app follows the server, one day ahead");
                Assert.AreEqual(Clock.Now.Date, Clock.Today);

                Clock.SetServerTime(DateTime.Now.AddMilliseconds(500));
                Assert.AreEqual(TimeSpan.Zero, Clock.Offset, "network delay is ignored");
            }
            finally
            {
                Clock.Reset();
            }
        }
    }
}
