using System.Linq;

namespace LawyerProject.DB
{
    public partial class Tikim_Letipul_Orech_Din
    {
        // KodLakoachNegdi / KodOrechDinNegdi hold the ID number of a row in Customers.
        // The previous version called itself recursively here (StackOverflowException);
        // these helpers look the person up by that ID instead.

        /// <summary>The opposing party of this case, or null.</summary>
        public Customers GetOpposingClient(LawyerDBEntities1 db)
        {
            return FindCustomer(db, KodLakoachNegdi);
        }

        /// <summary>The opposing lawyer of this case, or null.</summary>
        public Customers GetOpposingLawyer(LawyerDBEntities1 db)
        {
            return FindCustomer(db, KodOrechDinNegdi);
        }

        /// <summary>True for a court case, false for a property transaction (iska).</summary>
        public bool IsCourtCase
        {
            get { return IsTheTikMenualInBETMishpat ?? true; }
        }

        private static Customers FindCustomer(LawyerDBEntities1 db, string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            return db.Customers.FirstOrDefault(c => c.IDcustemer == id);
        }
    }
}
