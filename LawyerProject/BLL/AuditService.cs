using System;
using LawyerProject.DB;

namespace LawyerProject.BLL
{
    /// <summary>
    /// Writes who did what and when to the AuditEntry table. The entry is added to the same
    /// context, so it is saved in the same SaveChanges as the change it describes.
    /// </summary>
    public static class AuditService
    {
        public static void Log(QuedinDbContext db, string action, string entity, object entityId, string details)
        {
            db.AuditLog.Add(new AuditEntry
            {
                OccurredAt = Clock.Now,
                UserName = Session.CurrentUser != null ? Session.CurrentUser.UserName : null,
                Action = action,
                Entity = entity,
                EntityId = entityId == null ? null : Convert.ToString(entityId),
                Details = details == null ? null : (details.Length > ColumnLengths.AuditDetails ? details.Substring(0, ColumnLengths.AuditDetails) : details)
            });
        }
    }
}
