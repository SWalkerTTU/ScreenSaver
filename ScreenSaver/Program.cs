using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenSaver
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                string firstArgument = args[0].ToLower().Trim();
                string secondArgument = null;

                //Handle case where arguments are separated by colon.
                if (firstArgument.Length > 2)
                {
                    secondArgument = firstArgument.Substring(3).Trim();
                    firstArgument = firstArgument.Substring(0, 2);
                }
                else if (args.Length > 1)
                {
                    secondArgument = args[1];
                }
                switch(firstArgument)
                {
                    case "/c":
                        Application.Run(new SettingsForm());
                        break;
                    case "/p":
                        if (secondArgument == null)
                        {
                            MessageBox.Show("Sorry, but the expected window handle was not provided.",
                                "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        IntPtr previewWndHandle = new IntPtr(long.Parse(secondArgument));
                        Application.Run(new ScreenSaverForm(previewWndHandle));

                        break;
                    case "/s":
                        ShowScreenSaver();
                        Application.Run();
                        break;
                    default:
                        MessageBox.Show("ScreenSaver", "Sorry, but the command line argument \"" + firstArgument + "\" is not valid.");
                        break;
                }
            }
            else
            {
                Application.Run(new SettingsForm());
            }
        }
        
        static void ShowScreenSaver()
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                ScreenSaverForm screenSaver = new ScreenSaverForm(screen.Bounds);
                screenSaver.Show();
            }
        }
    }
}
