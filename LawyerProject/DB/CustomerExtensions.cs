namespace LawyerProject.DB
{
    /// <summary>Display helpers for customers and opposing parties (kept out of the generated entity file).</summary>
    public partial class Customer
    {
        /// <summary>"Last First"; a company keeps its whole name in FirstName and has no last name.</summary>
        public string DisplayName
        {
            get
            {
                string first = (FirstName ?? string.Empty).Trim();
                string last = (LastName ?? string.Empty).Trim();
                return last.Length == 0 ? first : last + " " + first;
            }
        }

        /// <summary>Name with the person type, for lists that mix clients and opposing parties.</summary>
        public string ListText
        {
            get { return string.IsNullOrEmpty(PersonType) ? DisplayName : DisplayName + " (" + PersonType + ")"; }
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
