using System;
using System.IO;
using LawyerProject.DB;

namespace LawyerProject.BLL
{
    /// <summary>Full database backup to a .bak file, run by a manager from the home screen.</summary>
    public static class BackupService
    {
        public static string DefaultFileName(DateTime now)
        {
            return "QuedinDB_" + now.ToString("yyyyMMdd_HHmm") + ".bak";
        }

        /// <summary>Backs up the database into <paramref name="folder"/>. Returns the file path, or throws with the SQL error.</summary>
        public static string Backup(QuedinDbContext db, string folder)
        {
            if (!Session.IsManager) throw new InvalidOperationException("רק מנהלת יכולה לגבות את המערכת.");
            string path = Path.Combine(folder, DefaultFileName(DateTime.Now));
            string dbName = db.Database.Connection.Database;
            // BACKUP cannot run inside a transaction; the database name cannot be a parameter, so quote it.
            db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction,
                "BACKUP DATABASE [" + dbName.Replace("]", "]]") + "] TO DISK = @p0 WITH INIT", path);
            AuditService.Log(db, AuditActions.Backup, "מערכת", null, path);
            db.SaveChanges();
            return path;
        }
    }
}
