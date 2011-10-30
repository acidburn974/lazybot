/*
This file is part of LazyBot.

    LazyBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    LazyBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with LazyBot.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.IO;
using System.Windows.Forms;
using LazyEvo.Forms;
using LazyEvo.Forms.Helpers;
using LazyEvo.Plugins;
using LazyLib;
using LazyLib.Dialogs.UnhandledExceptionDlg;

namespace LazyEvo
{
    internal static class Program
    {
        public static int AttachTo;

        /// <summary>
        ///   The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            new UnhandledExceptionDlg {RestartApp = false};
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CheckDotNet();
            CheckUpdate.CheckForUpdate();
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            string ourDirectory = executableDirectoryName;
            if (!Directory.Exists(ourDirectory + "\\Logs"))
                Directory.CreateDirectory(ourDirectory + "\\Logs");
            if (File.Exists(ourDirectory + "\\Logs\\OldLogFile.txt"))
            {
                File.Delete(ourDirectory + "\\Logs\\OldLogFile.txt");
            }
            if (File.Exists(ourDirectory + "\\Logs\\LogFile.txt"))
            {
                File.Move(ourDirectory + "\\Logs\\LogFile.txt", ourDirectory + "\\Logs\\OldLogFile.txt");
            }
            LazyForms.Load();
            LazySettings.LoadSettings();
            ReloggerSettings.LoadSettings();
            if (LazySettings.UserName == String.Empty || LazySettings.Password == String.Empty)
            {
                LazyForms.LoginForm.ShowDialog();
            }
            License.CheckLicense(LazySettings.UserName, LazySettings.Password);
            if (LazySettings.FirstRun)
            {
                new Wizard().ShowDialog();
            }
            var selector = new Selector();
            selector.ShowDialog();
            if (AttachTo == 0)
                Environment.Exit(0);

            Application.Run(LazyForms.MainForm);
        }

        private static void CheckDotNet()
        {
            try
            {
                DoCheckDotNet();
            }
            catch
            {
                MessageBox.Show("You do not have .Net 3.5 installed, LazyBot cannot work without it. Please install .Net 3.5 from microsoft");
                Environment.Exit(0);
            }
        }

        private static void DoCheckDotNet()
        {
            AppDomain.CurrentDomain.Load("System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
        }
    }
}