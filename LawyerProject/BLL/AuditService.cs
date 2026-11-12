using System;
using LawyerProject.DB;

namespace LawyerProject.BLL
{
    /// <summary>
    /// Writes who did what and when to the Yoman table. The entry is added to the same
    /// context, so it is saved in the same SaveChanges as the change it describes.
    /// </summary>
    public static class AuditService
    {
        public static void Log(LawyerDBEntities1 db, string action, string entity, object entityId, string details)
        {
            db.Yoman.Add(new Yoman
            {
                Taarich = DateTime.Now,
                UserName = Session.CurrentUser != null ? Session.CurrentUser.UserName : null,
                Peula = action,
                Yeshut = entity,
                KodYeshut = entityId == null ? null : Convert.ToString(entityId),
                Pratim = details == null ? null : (details.Length > 400 ? details.Substring(0, 400) : details)
            });
        }
    }
}
