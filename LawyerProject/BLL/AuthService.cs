using System;
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
            string error;
            return Login(userName, password, out error);
        }

        /// <summary>
        /// Checks the credentials. Returns the user, or null with a Hebrew message in <paramref name="error"/>.
        /// After <see cref="LoginPolicy.MaxFailures"/> wrong passwords the user is locked for a while;
        /// a deactivated user cannot log in at all.
        /// </summary>
        public User Login(string userName, string password, out string error)
        {
            error = "שם המשתמש או הסיסמה שגויים.";
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password)) return null;

            string name = userName.Trim();
            User user = db.Users.FirstOrDefault(u => u.UserName == name);
            if (user == null) return null;

            DateTime now = Clock.Now;
            if (!user.IsActive)
            {
                error = "המשתמש הושבת. פנו למנהל המשרד.";
                return null;
            }
            if (LoginPolicy.IsLocked(user.LockedUntil, now))
            {
                error = "המשתמש נעול עד " + user.LockedUntil.Value.ToString("HH:mm") + " בגלל ניסיונות כניסה שגויים. מנהל יכול לשחרר אותו על ידי איפוס סיסמה.";
                return null;
            }

            bool ok;
            if (IsHashed(user.PasswordHash))
            {
                ok = PasswordHasher.Verify(password, user.PasswordHash);
            }
            else
            {
                // A database created by the original 2024 version still holds plain-text
                // passwords. Accept the old password once and replace it with a hash.
                ok = user.PasswordHash == password;
                if (ok) user.PasswordHash = PasswordHasher.Hash(password);
            }

            if (!ok)
            {
                DateTime? lockedUntil;
                user.FailedLogins = LoginPolicy.RegisterFailure(user.FailedLogins, now, out lockedUntil);
                if (lockedUntil.HasValue)
                {
                    user.LockedUntil = lockedUntil;
                    AuditService.Log(db, AuditActions.UserLocked, "משתמש", user.UserName, "עד " + lockedUntil.Value.ToString("HH:mm"));
                    error = "יותר מדי ניסיונות שגויים. המשתמש נעול ל-" + LoginPolicy.LockMinutes + " דקות.";
                }
                db.SaveChanges();
                return null;
            }

            if (user.FailedLogins != 0 || user.LockedUntil.HasValue)
            {
                user.FailedLogins = 0;
                user.LockedUntil = null;
            }
            db.SaveChanges();
            error = null;
            return user;
        }

        /// <summary>Turns a user's access on or off (managers only; a manager cannot deactivate themselves).</summary>
        public string SetActive(string userName, bool active)
        {
            if (!Session.IsManager) return "רק מנהל יכול להשבית משתמשים.";
            string name = (userName ?? string.Empty).Trim();
            User user = db.Users.FirstOrDefault(u => u.UserName == name);
            if (user == null) return "המשתמש לא נמצא.";
            if (!active && Session.CurrentUser != null && Session.CurrentUser.UserName == user.UserName)
                return "לא ניתן להשבית את המשתמש שאיתו את/ה מחובר/ת.";
            if (user.IsActive == active) return null;

            user.IsActive = active;
            AuditService.Log(db, active ? AuditActions.UserActivated : AuditActions.UserDeactivated, "משתמש", user.UserName, null);
            db.SaveChanges();
            return null;
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
                Role = role,
                IsActive = true
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
            user.FailedLogins = 0;
            user.LockedUntil = null; // a reset also releases a locked user
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
