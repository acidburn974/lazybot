
﻿/*
This file is part of LazyBot - Copyright (C) 2011 Arutha

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
    public class SendExceptionClickEventArgs : EventArgs
    {
        public bool RestartApp;
        public bool SendExceptionDetails;
        public Exception UnhandledException;

        public SendExceptionClickEventArgs(bool sendDetailsArg, Exception exceptionArg, bool restartAppArg)
        {
            SendExceptionDetails = sendDetailsArg;
            UnhandledException = exceptionArg; // Used to store captured exception
            RestartApp = restartAppArg; // Contains user's request: should the App to be restarted or not
        }
    }

    /// <summary>
    ///   Class for catching unhandled exception with UI dialog.
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class UnhandledExceptionDlg
    {
        #region Delegates

        public delegate void SendExceptionClickHandler(object sender, SendExceptionClickEventArgs args);

        #endregion

        private bool _dorestart = true;

        /// <summary>
        ///   Default constructor
        /// </summary>
        public UnhandledExceptionDlg()
        {
            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            //Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            // Add the event handler for handling UI thread exceptions to the event:
            Application.ThreadException += ThreadExceptionFunction;

            // Set the unhandled exception mode to force all Windows Forms errors to go through our handler:
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event:
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionFunction;
        }

        /// <summary>
        ///   Set to true if you want to restart your App after falure
        /// </summary>
        public bool RestartApp
        {
            get { return _dorestart; }
            set { _dorestart = value; }
        }

        //public delegate void ShowErrorReportHandler(object sender, System.EventArgs args);

        /// <summary>
        ///   Occurs when user clicks on "Send Error report" button
        /// </summary>
        public event SendExceptionClickHandler OnSendExceptionClick;

        /// <summary>
        ///   Handle the UI exceptions by showing a dialog box
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        private void ThreadExceptionFunction(Object sender, ThreadExceptionEventArgs e)
        {
            // Suppress the Dialog in Debug mode:

            ShowUnhandledExceptionDlg(e.Exception);
        }

        /// <summary>
        ///   Handle the UI exceptions by showing a dialog box
        /// </summary>
        /// <param name = "sender">Sender Object</param>
        /// <param name = "args">Passing arguments: original exception etc.</param>
        private void UnhandledExceptionFunction(Object sender, UnhandledExceptionEventArgs args)
        {
            // Suppress the Dialog in Debug mode:

            ShowUnhandledExceptionDlg((Exception) args.ExceptionObject);
        }

        /// <summary>
        ///   Raise Exception Dialog box for both UI and non-UI Unhandled Exceptions
        /// </summary>
        /// <param name = "e">Catched exception</param>
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

                // Disable the Button if OnSendExceptionClick event is not handled
                exDlgForm.buttonSend.Enabled = (OnSendExceptionClick != null);

                // Attach reflection to checkbox checked status
                exDlgForm.checkBoxRestart.CheckedChanged += delegate { _dorestart = exDlgForm.checkBoxRestart.Checked; };

                exDlgForm.textBox1.AppendText("Message: " + unhandledException.Message + Environment.NewLine);
                exDlgForm.textBox1.AppendText("Inner exception: " + unhandledException.InnerException +
                                              Environment.NewLine);
                exDlgForm.textBox1.AppendText("Source: " + unhandledException.Source + Environment.NewLine);
                exDlgForm.textBox1.AppendText("Stack trace: " + unhandledException.StackTrace + Environment.NewLine);
                exDlgForm.textBox1.AppendText("Target site: " + unhandledException.TargetSite + Environment.NewLine);
                exDlgForm.textBox1.AppendText("Data: " + unhandledException.Data + Environment.NewLine);
                exDlgForm.textBox1.AppendText("Link: " + unhandledException.HelpLink + Environment.NewLine);

                // Show the Dialog box:
                bool sendDetails = (exDlgForm.ShowDialog() == DialogResult.Yes);

                if (OnSendExceptionClick != null)
                {
                    var ar = new SendExceptionClickEventArgs(sendDetails, unhandledException, _dorestart);
                    OnSendExceptionClick(this, ar);
                }
            }
            finally
            {
                exDlgForm.Dispose();
            }
            Environment.Exit(0);
        }


        /// <summary>
        ///   Generates a random string with the given length
        /// </summary>
        /// <param name = "size">Size of the string</param>
        /// <param name = "lowerCase">If true, generate lowercase string</param>
        /// <returns>Random string</returns>
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