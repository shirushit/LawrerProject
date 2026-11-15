using System.Linq;
using LawyerProject.DB;

namespace LawyerProject.BLL
{
    /// <summary>Login, user creation and password changes. Passwords are stored hashed only.</summary>
    public class AuthService
    {
        private readonly QuedinDbContext db;

        public AuthService(QuedinDbContext db)
        {
            this.db = db;
        }

        /// <summary>Returns the user when the credentials are correct, otherwise null.</summary>
        public User Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password)) return null;

            string name = userName.Trim();
            User user = db.Users.FirstOrDefault(u => u.UserName == name);
            if (user == null) return null;

            if (IsHashed(user.PasswordHash))
                return PasswordHasher.Verify(password, user.PasswordHash) ? user : null;

            // A database created by the original 2024 version still holds plain-text
            // passwords. Accept the old password once and replace it with a hash.
            if (user.PasswordHash != password) return null;
            user.PasswordHash = PasswordHasher.Hash(password);
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

            db.Users.Add(new User
            {
                UserName = name,
                PasswordHash = PasswordHasher.Hash(password),
                Role = role
            });
            AuditService.Log(db, AuditActions.UserAdded, "משתמש", name, role);
            db.SaveChanges();
            return null;
        }

        /// <summary>Changes a password after verifying the old one. Returns null on success or an error message.</summary>
        public string ChangePassword(string userName, string oldPassword, string newPassword, string confirmPassword)
        {
            User user = Login(userName, oldPassword);
            if (user == null) return "שם המשתמש או הסיסמה הנוכחית שגויים.";
            if (newPassword != confirmPassword) return "הסיסמאות החדשות אינן תואמות.";
            if (newPassword == oldPassword) return "הסיסמה החדשה זהה לסיסמה הנוכחית.";

            string passwordError = Validation.GetPasswordError(newPassword);
            if (passwordError != null) return passwordError;

            user.PasswordHash = PasswordHasher.Hash(newPassword);
            AuditService.Log(db, AuditActions.PasswordChanged, "משתמש", user.UserName, null);
            db.SaveChanges();
            return null;
        }

        /// <summary>A manager sets a new password for a user who forgot theirs.</summary>
        public string ResetPassword(string userName, string newPassword)
        {
            if (!Session.IsManager) return "רק מנהל יכול לאפס סיסמה.";
            string name = (userName ?? string.Empty).Trim();
            User user = db.Users.FirstOrDefault(u => u.UserName == name);
            if (user == null) return "המשתמש לא נמצא.";
            string passwordError = Validation.GetPasswordError(newPassword);
            if (passwordError != null) return passwordError;

            user.PasswordHash = PasswordHasher.Hash(newPassword);
            AuditService.Log(db, AuditActions.PasswordReset, "משתמש", user.UserName, null);
            db.SaveChanges();
            return null;
        }

        private static bool IsHashed(string stored)
        {
            return stored != null && stored.StartsWith("pbkdf2$");
        }
    }
}
