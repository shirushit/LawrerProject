using System.Linq;

namespace LawyerProject.DB
{
    // Hand-written additions to the generated entity classes (kept out of the generated files).

    public partial class Court
    {
        public override string ToString()
        {
            return Name;
        }
    }

    public partial class CaseType
    {
        public override string ToString()
        {
            return Name;
        }
    }

    public partial class Procedure
    {
        public override string ToString()
        {
            return Name;
        }
    }

    public partial class LegalCase
    {
        // OpposingPartyId / OpposingLawyerId hold the ID of a row in Customers.
        // The original version called itself recursively here (StackOverflowException);
        // these helpers look the person up by that ID instead.

        /// <summary>The opposing party of this case, or null.</summary>
        public Customer GetOpposingParty(QuedinDbContext db)
        {
            return FindCustomer(db, OpposingPartyId);
        }

        /// <summary>The opposing lawyer of this case, or null.</summary>
        public Customer GetOpposingLawyer(QuedinDbContext db)
        {
            return FindCustomer(db, OpposingLawyerId);
        }

        /// <summary>True for a court case, false for a property transaction.</summary>
        public bool IsCourtCase
        {
            get { return IsInCourt ?? true; }
        }

        private static Customer FindCustomer(QuedinDbContext db, string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;
            return db.Customers.FirstOrDefault(c => c.CustomerId == id);
        }
    }
}
