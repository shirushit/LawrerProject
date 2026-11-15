using LawyerProject.DB;

namespace LawyerProject.BLL
{
    /// <summary>The user who is logged in to this running instance of the app.</summary>
    public static class Session
    {
        public static User CurrentUser { get; private set; }

        public static bool IsLoggedIn
        {
            get { return CurrentUser != null; }
        }

        public static bool IsManager
        {
            get { return CurrentUser != null && CurrentUser.Role == Roles.Manager; }
        }

        public static void SignIn(User user)
        {
            CurrentUser = user;
        }

        public static void SignOut()
        {
            CurrentUser = null;
        }
    }
}
