namespace LawyerProject.BLL
{
    /// <summary>Values stored in Customers.SugPerson. One table holds all three kinds of people.</summary>
    public static class PersonTypes
    {
        public const string Client = "לקוח";
        public const string OpposingClient = "לקוח נגדי";
        public const string OpposingLawyer = "עורך דין נגדי";

        public static readonly string[] All = { Client, OpposingClient, OpposingLawyer };
    }

    /// <summary>Values stored in Users.KindWorker.</summary>
    public static class Roles
    {
        public const string Manager = "מנהל";
        public const string Worker = "משתמש רגיל";

        public static readonly string[] All = { Manager, Worker };
    }

    /// <summary>Values stored in Tashlumim_Letik.EmzhaeyTashlum.</summary>
    public static class PaymentMethods
    {
        public const string Cash = "מזומן";
        public const string CreditCard = "אשראי";
        public const string BankTransfer = "העברה בנקאית";
        public const string Check = "צ'ק";
    }
}
