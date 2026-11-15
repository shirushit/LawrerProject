using System;

namespace LawyerProject.BLL
{
    /// <summary>Locks a user for a while after repeated wrong passwords, to stop password guessing.</summary>
    public static class LoginPolicy
    {
        public const int MaxFailures = 5;
        public const int LockMinutes = 15;

        public static bool IsLocked(DateTime? lockedUntil, DateTime now)
        {
            return lockedUntil.HasValue && lockedUntil.Value > now;
        }

        /// <summary>
        /// Counts a failed attempt. Returns the new failure count; after <see cref="MaxFailures"/> failures
        /// the user is locked for <see cref="LockMinutes"/> minutes and the count starts again.
        /// </summary>
        public static int RegisterFailure(int failures, DateTime now, out DateTime? lockedUntil)
        {
            int count = failures + 1;
            if (count >= MaxFailures)
            {
                lockedUntil = now.AddMinutes(LockMinutes);
                return 0;
            }
            lockedUntil = null;
            return count;
        }
    }
}
