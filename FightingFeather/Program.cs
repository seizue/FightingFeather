using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FightingFeather
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

            // Create an instance of the Main form
            Main mainForm = new Main();

            // Create an instance of the LoginRegisterForm and pass the Main form instance
            LoginRegisterForm loginForm = new LoginRegisterForm(mainForm);

            // Show the LoginRegisterForm
            Application.Run(loginForm);

            // Show the Main form after the login form closes
            mainForm.ShowDialog();
        }

    }
}
