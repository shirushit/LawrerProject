namespace LawyerProject.BLL
{
    /// <summary>Values stored in Customer.PersonType. One table holds all three kinds of people.</summary>
    public static class PersonTypes
    {
        public const string Client = "לקוח";
        public const string OpposingClient = "לקוח נגדי";
        public const string OpposingLawyer = "עורך דין נגדי";

        public static readonly string[] All = { Client, OpposingClient, OpposingLawyer };
    }

    /// <summary>Values stored in User.Role.</summary>
    public static class Roles
    {
        public const string Manager = "מנהל";
        public const string Worker = "משתמש רגיל";

        public static readonly string[] All = { Manager, Worker };
    }

    /// <summary>Values stored in Payment.Method.</summary>
    public static class PaymentMethods
    {
        public const string Cash = "מזומן";
        public const string CreditCard = "אשראי";
        public const string BankTransfer = "העברה בנקאית";
        public const string Check = "צ'ק";

        public static readonly string[] All = { Cash, CreditCard, BankTransfer, Check };
    }

    /// <summary>Values stored in Customer.IdType (what CustomerId contains).</summary>
    public static class IdTypes
    {
        public const string IsraeliId = "ת.ז.";
        public const string CompanyNumber = "ח.פ.";
        public const string Passport = "דרכון";
        public const string None = "ללא מזהה";

        public static readonly string[] All = { IsraeliId, CompanyNumber, Passport, None };
    }

    /// <summary>Values stored in Payment.Kind. Only fees carry VAT.</summary>
    public static class PaymentKinds
    {
        public const string Fee = "שכר טרחה";
        public const string Expenses = "אגרה והוצאות";

        /// <summary>Money returned to a client who paid more than the case total (recorded by the system, not picked by users).</summary>
        public const string Refund = "החזר ללקוח";

        /// <summary>The kinds a user can choose when recording a payment.</summary>
        public static readonly string[] All = { Fee, Expenses };
    }

    /// <summary>Column lengths from create-database.sql that the code shortens text to.</summary>
    public static class ColumnLengths
    {
        public const int PaymentDescription = 50;
        public const int StepNotes = 200;
        public const int AuditDetails = 400;
    }

    /// <summary>Actions written to the audit log (AuditEntry.Action).</summary>
    public static class AuditActions
    {
        public const string CaseOpened = "פתיחת תיק";
        public const string CaseUpdated = "עדכון תיק";
        public const string CaseDeleted = "מחיקת תיק";
        public const string StepCompleted = "סימון שלב כבוצע";
        public const string StepUndone = "ביטול סימון שלב";
        public const string StepRescheduled = "שינוי מועד שלב";
        public const string StepNote = "הערה לשלב";
        public const string PaymentAdded = "רישום תשלום";
        public const string PaymentCancelled = "ביטול תשלום";
        public const string UserAdded = "הוספת משתמש";
        public const string PasswordChanged = "החלפת סיסמה";
        public const string PasswordReset = "איפוס סיסמה";
        public const string Backup = "גיבוי";
        public const string Refund = "החזר ללקוח";
        public const string CustomerUpdated = "עדכון פרטי לקוח";
        public const string UserActivated = "הפעלת משתמש";
        public const string UserDeactivated = "השבתת משתמש";
        public const string UserLocked = "נעילת משתמש";
        public const string SettingsChanged = "שינוי הגדרות";
        public const string HolidayAdded = "הוספת יום חופש";
        public const string HolidayRemoved = "הסרת יום חופש";
        public const string CatalogChanged = "שינוי בהגדרות המשרד";
    }
}
