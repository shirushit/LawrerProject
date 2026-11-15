using System;

namespace LawyerProject.BLL
{
    /// <summary>
    /// Locks a user for a while after repeated wrong passwords, to stop password guessing.
    /// The rule is enforced by the database (usp_TryLogin in create-database.sql uses the same numbers),
    /// so it also holds for someone who calls the database directly.
    /// </summary>
    public static class LoginPolicy
    {
        public const int MaxFailures = 5;
        public const int LockMinutes = 15;

        public static bool IsLocked(DateTime? lockedUntil, DateTime now)
        {
            return lockedUntil.HasValue && lockedUntil.Value > now;
        }
    }
}
