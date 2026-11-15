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
        /// The stored hash never comes to the app: the app gets the salt, hashes the typed password and the
        /// database compares, counts failures and locks the user after <see cref="LoginPolicy.MaxFailures"/>
        /// wrong passwords (see usp_TryLogin in create-database.sql). A deactivated user cannot log in.
        /// </summary>
        public User Login(string userName, string password, out string error)
        {
            error = "שם המשתמש או הסיסמה שגויים.";
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrEmpty(password)) return null;

            string name = userName.Trim();
            string candidate, upgrade;
            HashForLogin(name, password, out candidate, out upgrade);

            LoginAttempt result = db.Database.SqlQuery<LoginAttempt>(
                "EXEC dbo.usp_TryLogin @UserName, @Candidate, @UpgradeHash",
                Param("@UserName", name), Param("@Candidate", candidate), Param("@UpgradeHash", upgrade)).Single();

            switch (result.Result)
            {
                case LoginAttempt.Success:
                    error = null;
                    return new User { UserId = result.UserId ?? 0, UserName = name, Role = result.Role, IsActive = true };
                case LoginAttempt.Locked:
                    error = "המשתמש נעול עד " + result.LockedUntil.Value.ToString("HH:mm") + " בגלל ניסיונות כניסה שגויים. מנהל/ת יכול/ה לשחרר אותו על ידי איפוס סיסמה.";
                    return null;
                case LoginAttempt.NowLocked:
                    AuditService.Log(db, AuditActions.UserLocked, "משתמש", name, "עד " + result.LockedUntil.Value.ToString("HH:mm"));
                    ChangeDiscarder.SaveOrDiscard(db);
                    error = "יותר מדי ניסיונות שגויים. המשתמש נעול ל-" + LoginPolicy.LockMinutes + " דקות.";
                    return null;
                case LoginAttempt.Deactivated:
                    error = "המשתמש הושבת. פנו למנהל/ת המשרד.";
                    return null;
                default: // wrong password or unknown user: the same answer
                    return null;
            }
        }

        /// <summary>
        /// Hashes the typed password with the user's salt from the database. An unknown user gets a
        /// made-up salt, so both cases take the same time. A password from the 2024 version (stored as
        /// plain text) is compared as is, and <paramref name="upgrade"/> holds the hash that replaces it.
        /// </summary>
        private void HashForLogin(string name, string password, out string candidate, out string upgrade)
        {
            string salt = db.Database.SqlQuery<string>("EXEC dbo.usp_GetLoginSalt @UserName", Param("@UserName", name)).FirstOrDefault();
            if (salt == LegacySalt)
            {
                candidate = password;
                upgrade = PasswordHasher.Hash(password);
            }
            else
            {
                candidate = PasswordHasher.HashWithSalt(password, salt);
                upgrade = null;
            }
        }

        private const string LegacySalt = "legacy$";

        private static System.Data.SqlClient.SqlParameter Param(string name, string value)
        {
            return new System.Data.SqlClient.SqlParameter(name, System.Data.SqlDbType.NVarChar, 200) { Value = (object)value ?? DBNull.Value };
        }

        /// <summary>The row returned by usp_TryLogin.</summary>
        private class LoginAttempt
        {
            public const int Success = 0, WrongPassword = 1, Locked = 2, Deactivated = 3, UnknownUser = 4, NowLocked = 5;

            public int Result { get; set; }
            public int? UserId { get; set; }
            public string Role { get; set; }
            public DateTime? LockedUntil { get; set; }
        }

        /// <summary>Turns a user's access on or off (managers only; a manager cannot deactivate themselves).</summary>
        public string SetActive(string userName, bool active)
        {
            if (!Session.IsManager) return "רק מנהל/ת יכול/ה להשבית משתמשים.";
            string name = (userName ?? string.Empty).Trim();
            User user = db.Users.FirstOrDefault(u => u.UserName == name);
            if (user == null) return "המשתמש לא נמצא.";
            if (!active && Session.CurrentUser != null && Session.CurrentUser.UserName == user.UserName)
                return "לא ניתן להשבית את המשתמש שאיתו את/ה מחובר/ת.";
            if (user.IsActive == active) return null;

            user.IsActive = active;
            AuditService.Log(db, active ? AuditActions.UserActivated : AuditActions.UserDeactivated, "משתמש", user.UserName, null);
            ChangeDiscarder.SaveOrDiscard(db);
            return null;
        }

        /// <summary>Creates a user. Returns null on success or a Hebrew error message.</summary>
        public string AddUser(string userName, string password, string role)
        {
            if (!Session.IsManager) return "רק מנהל/ת יכול/ה להוסיף משתמשים.";
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
            ChangeDiscarder.SaveOrDiscard(db);
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

            // The database checks the current password again before it stores the new hash.
            string oldCandidate, ignored;
            HashForLogin(user.UserName, oldPassword, out oldCandidate, out ignored);
            int changed = db.Database.SqlQuery<int>("EXEC dbo.usp_ChangeOwnPassword @UserName, @OldCandidate, @NewHash",
                Param("@UserName", user.UserName), Param("@OldCandidate", oldCandidate),
                Param("@NewHash", PasswordHasher.Hash(newPassword))).Single();
            if (changed != 1) return "הסיסמה לא הוחלפה. נסו שוב.";

            AuditService.Log(db, AuditActions.PasswordChanged, "משתמש", user.UserName, null);
            ChangeDiscarder.SaveOrDiscard(db);
            return null;
        }

        /// <summary>A manager sets a new password for a user who forgot theirs.</summary>
        public string ResetPassword(string userName, string newPassword)
        {
            if (!Session.IsManager) return "רק מנהל/ת יכול/ה לאפס סיסמה.";
            string name = (userName ?? string.Empty).Trim();
            User user = db.Users.FirstOrDefault(u => u.UserName == name);
            if (user == null) return "המשתמש לא נמצא.";
            string passwordError = Validation.GetPasswordError(newPassword);
            if (passwordError != null) return passwordError;

            user.PasswordHash = PasswordHasher.Hash(newPassword);
            user.FailedLogins = 0;
            user.LockedUntil = null; // a reset also releases a locked user
            AuditService.Log(db, AuditActions.PasswordReset, "משתמש", user.UserName, null);
            ChangeDiscarder.SaveOrDiscard(db);
            return null;
        }

    }
}
