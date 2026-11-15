using System;
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
            // usp_TryLogin in create-database.sql uses the same numbers; change both together.
            Assert.AreEqual(5, LoginPolicy.MaxFailures);
            Assert.AreEqual(15, LoginPolicy.LockMinutes);
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
