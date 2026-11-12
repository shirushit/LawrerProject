using System.Linq;
using LawyerProject.DB;

namespace LawyerProject.BLL
{
    /// <summary>Login, user creation and password changes. Passwords are stored hashed only.</summary>
    public class AuthService
    {
        private readonly LawyerDBEntities1 db;

        public AuthService(LawyerDBEntities1 db)
        {
            this.db = db;
        }

        /// <summary>Returns the user when the credentials are correct, otherwise null.</summary>
        public Users Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password)) return null;

            string name = userName.Trim();
            Users user = db.Users.FirstOrDefault(u => u.UserName == name);
            if (user == null) return null;

            if (IsHashed(user.PasswordWorker))
                return PasswordHasher.Verify(password, user.PasswordWorker) ? user : null;

            // A database created by the original 2024 version still holds plain-text
            // passwords. Accept the old password once and replace it with a hash.
            if (user.PasswordWorker != password) return null;
            user.PasswordWorker = PasswordHasher.Hash(password);
            db.SaveChanges();
            return user;
        }

        /// <summary>Creates a user. Returns null on success or a Hebrew error message.</summary>
        public string AddUser(string userName, string password, string role)
        {
            if (!Session.IsManager) return "רק מנהל יכול להוסיף משתמשים.";
            if (string.IsNullOrWhiteSpace(userName)) return "יש להזין שם משתמש.";
            if (!Roles.All.Contains(role)) return "יש לבחור סוג משתמש.";

            string name = userName.Trim();
            if (db.Users.Any(u => u.UserName == name)) return "שם המשתמש כבר קיים במערכת.";

            string passwordError = Validation.GetPasswordError(password);
            if (passwordError != null) return passwordError;

            db.Users.Add(new Users
            {
                UserName = name,
                PasswordWorker = PasswordHasher.Hash(password),
                KindWorker = role
            });
            db.SaveChanges();
            return null;
        }

        /// <summary>Changes a password after verifying the old one. Returns null on success or an error message.</summary>
        public string ChangePassword(string userName, string oldPassword, string newPassword, string confirmPassword)
        {
            Users user = Login(userName, oldPassword);
            if (user == null) return "שם המשתמש או הסיסמה הנוכחית שגויים.";
            if (newPassword != confirmPassword) return "הסיסמאות החדשות אינן תואמות.";
            if (newPassword == oldPassword) return "הסיסמה החדשה זהה לסיסמה הנוכחית.";

            string passwordError = Validation.GetPasswordError(newPassword);
            if (passwordError != null) return passwordError;

            user.PasswordWorker = PasswordHasher.Hash(newPassword);
            db.SaveChanges();
            return null;
        }

        private static bool IsHashed(string stored)
        {
            return stored != null && stored.StartsWith("pbkdf2$");
        }
    }
}
