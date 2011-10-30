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
#region

using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#endregion

namespace LazyLib.Dialogs.UnhandledExceptionDlg
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class UnhandledExceptionDlg
    {
        private bool _dorestart = true;
        public UnhandledExceptionDlg()
        {
            Application.ThreadException += ThreadExceptionFunction;

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionFunction;
        }

        public bool RestartApp
        {
            get { return _dorestart; }
            set { _dorestart = value; }
        }


        private void ThreadExceptionFunction(Object sender, ThreadExceptionEventArgs e)
        {

            ShowUnhandledExceptionDlg(e.Exception);
        }

        private void UnhandledExceptionFunction(Object sender, UnhandledExceptionEventArgs args)
        {

            ShowUnhandledExceptionDlg((Exception) args.ExceptionObject);
        }

        private void ShowUnhandledExceptionDlg(Exception e)
        {
            Exception unhandledException = e;

            if (unhandledException == null)
                unhandledException = new Exception("Unknown unhandled Exception was occurred!");

            var exDlgForm = new UnhandledExDlgForm();
            try
            {
                string appName = Process.GetCurrentProcess().ProcessName;
                exDlgForm.Text = RandomString(8, true);
                exDlgForm.labelTitle.Text = String.Format(exDlgForm.labelTitle.Text, appName);
                exDlgForm.checkBoxRestart.Text = String.Format(exDlgForm.checkBoxRestart.Text, appName);
                exDlgForm.checkBoxRestart.Checked = RestartApp;

                exDlgForm.checkBoxRestart.CheckedChanged += delegate { _dorestart = exDlgForm.checkBoxRestart.Checked; };

                exDlgForm.textBox1.AppendText("Message: " + unhandledException.Message + Environment.NewLine);
                exDlgForm.textBox1.AppendText("Inner exception: " + unhandledException.InnerException +
                                              Environment.NewLine);
                exDlgForm.textBox1.AppendText("Source: " + unhandledException.Source + Environment.NewLine);
                exDlgForm.textBox1.AppendText("Stack trace: " + unhandledException.StackTrace + Environment.NewLine);
                exDlgForm.textBox1.AppendText("Target site: " + unhandledException.TargetSite + Environment.NewLine);
                exDlgForm.textBox1.AppendText("Data: " + unhandledException.Data + Environment.NewLine);
                exDlgForm.textBox1.AppendText("Link: " + unhandledException.HelpLink + Environment.NewLine);

                bool sendDetails = (exDlgForm.ShowDialog() == DialogResult.Yes);
            }
            finally
            {
                exDlgForm.Dispose();
            }
            Environment.Exit(0);
        }

        private static string RandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder();
            var random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26*random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}