using System.Data.Entity;
using System.Linq;
using LawyerProject.DB;

namespace LawyerProject.BLL
{
    /// <summary>
    /// Screens keep one DbContext open while the user works. When SaveChanges fails (lost connection,
    /// timeout, a conflict with another user) the context still holds the changes it could not save,
    /// and the next save would write them as well - for example a payment recorded twice. Every
    /// failed save therefore puts the context back to the state of the database.
    /// </summary>
    public static class ChangeDiscarder
    {
        /// <summary>Saves; on any error discards the pending changes and rethrows.</summary>
        public static void SaveOrDiscard(QuedinDbContext db)
        {
            try
            {
                db.SaveChanges();
            }
            catch
            {
                DiscardChanges(db);
                throw;
            }
        }

        /// <summary>
        /// New rows are dropped (and taken out of their case's Steps/Payments lists), edited rows get
        /// their loaded values back and rows marked for deletion are kept.
        /// </summary>
        public static void DiscardChanges(DbContext db)
        {
            foreach (var entry in db.ChangeTracker.Entries().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        var payment = entry.Entity as Payment;
                        if (payment != null && payment.Case != null) payment.Case.Payments.Remove(payment);
                        var step = entry.Entity as CaseStep;
                        if (step != null && step.Case != null) step.Case.Steps.Remove(step);
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
    }
}
