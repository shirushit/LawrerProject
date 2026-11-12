namespace LawyerProject.DB
{
    /// <summary>Display helpers for customers and opposing parties (kept out of the generated entity file).</summary>
    public partial class Customers
    {
        /// <summary>"First Last", or only the name for a company (companies keep their name in FirstName).</summary>
        public string DisplayName
        {
            get
            {
                string first = (FirstName ?? string.Empty).Trim();
                string last = (LastName ?? string.Empty).Trim();
                return last.Length == 0 ? first : first + " " + last;
            }
        }
    }
}
