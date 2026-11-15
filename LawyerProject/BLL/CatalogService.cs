using System;
using LawyerProject.DB;

namespace LawyerProject.BLL
{
    /// <summary>
    /// Saves changes to the office catalogue (courts, case types, procedures and their links).
    /// The catalogue decides the timeline of every new case, so only a manager may change it, and
    /// every change is written to the audit log. The check is here, not only in hidden buttons.
    /// </summary>
    public static class CatalogService
    {
        public const string ManagersOnly = "רק מנהל/ת המשרד יכול/ה לשנות את הגדרות המשרד.";

        /// <summary>Throws <see cref="UnauthorizedAccessException"/> unless the user is a manager.</summary>
        public static void RequireManager()
        {
            if (!Session.IsManager) throw new UnauthorizedAccessException(ManagersOnly);
        }

        /// <summary>Checks the role, writes an audit entry ("what" and a short description) and saves.</summary>
        public static void Save(QuedinDbContext db, string what, string details)
        {
            RequireManager();
            AuditService.Log(db, AuditActions.CatalogChanged, what, null, details);
            ChangeDiscarder.SaveOrDiscard(db);
        }
    }
}
