using System;
using System.Windows.Forms;

namespace LawyerProject
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += (s, e) => GUI.Ui.Error("אירעה שגיאה לא צפויה.", e.Exception);
            Application.Run(new GUI.LoginForm());
        }
    }
}
