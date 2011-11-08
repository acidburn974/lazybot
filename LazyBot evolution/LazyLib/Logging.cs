
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using LazyLib.Helpers;

namespace LazyLib
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public static class Logging
    {
        #region Hook up on class

        /* You can hook yourself onto this class like:
        /// <summary>
        /// Ctor.
        /// </summary>
        public Log()
        {
            InitializeComponent();

            Logging.OnWrite += Logging_OnWrite;
        }

        void Logging_OnWrite(string message, Color col)
        {
            AppendMessage(textLog, message, col);
        }

        /// <summary>
        /// Writes a message to the specified RichTextBox.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="message">The <see cref="string">message</see> to be written.</param>
        /// <param name="col"></param>
        private static void AppendMessage(RichTextBox textBox, string message, Color col)
        {
            try
            {
                if (!textBox.IsDisposed)
                {
                    if (textBox.InvokeRequired)
                    {
                        textBox.Invoke(new Action<RichTextBox, string, Color>(AppendMessage), textBox, message, col);
                        return;
                    }

                    Color oldColor = textBox.SelectionColor;
                    textBox.SelectionColor = col;
                    textBox.AppendText(message);
                    textBox.SelectionColor = oldColor;
                    textBox.AppendText(Environment.NewLine);
                    textBox.ScrollToCaret();
                }
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
            }
        }
         * */

        #endregion

        #region Delegates

        /// <summary>
        /// 
        /// </summary>
        public delegate void DebugDelegate(string message, LogType logType);

        /// <summary>
        /// 
        /// </summary>
        public delegate void WriteDelegate(string message, LogType logType);

        #endregion

        private static readonly Thread QueueThread;
        private static readonly Queue<string> LogQueue = new Queue<string>();

        private static string _logSpam;

        static Logging()
        {
            LogOnWrite = true;
            QueueThread = new Thread(WriteQueue) {IsBackground = true};
            QueueThread.Name = "Logging";
            QueueThread.Start(true);
        }

        public static bool LogOnWrite { get; set; }

        private static string TimeStamp
        {
            get { return string.Format("[{0}] ", DateTime.Now.ToLongTimeString()); }
        }

        public static event WriteDelegate OnWrite;

        public static event DebugDelegate OnDebug;

        private static void InvokeOnWrite(string message, LogType col)
        {
            WriteDelegate @delegate = OnWrite;
            if (@delegate != null)
            {
                @delegate(message, col);
            }
        }

        private static void InvokeOnDebug(string message, LogType col)
        {
            DebugDelegate @delegate = OnDebug;
            if (@delegate != null)
            {
                @delegate(message, col);
            }
        }

        private static void WriteQueue(object blocking)
        {
            if (!Directory.Exists(string.Format("{0}\\Logs", Utilities.ApplicationPath)))
            {
                Directory.CreateDirectory(string.Format("{0}\\Logs", Utilities.ApplicationPath));
            }
            while (true)
            {
                try
                {
                    using (
                        TextWriter tw =
                            new StreamWriter(
                                string.Format("{0}\\Logs\\LogFile.txt", Utilities.ApplicationPath), true))
                    {
                        while (LogQueue.Count != 0)
                        {
                            tw.WriteLine(LogQueue.Dequeue());
                        }
                        if (!((bool) blocking))
                        {
                            break;
                        }
                    }
                    Thread.Sleep(500);
                }
                catch
                {
                    break;
                }
            }
        }

        public static void Write(string format, params object[] args)
        {
            Write(LogType.Normal, format, args);
        }

        public static void Write(LogType color, string format, params object[] args)
        {
            string s = TimeStamp + string.Format(format, args);

            if (s != _logSpam)
            {
                InvokeOnWrite(s, color);
                if (LogOnWrite)
                {
                    LogQueue.Enqueue(s);
                }
            }

            _logSpam = s;
        }

        public static void Debug(string format, params object[] args)
        {
            Debug(LogType.Warning, format, args);
        }

        public static void ExtendedDebug(string format, params object[] args)
        {
            if(LazySettings.DebugMode)
            {
                Debug(format, args);
            }
        }

        public static void Debug(LogType color, string format, params object[] args)
        {
            string s = TimeStamp + string.Format(format, args);
            InvokeOnDebug(s, color);
            if (LogOnWrite)
            {
                Console.WriteLine(s);
                LogQueue.Enqueue(s);
            }
        }
    }
}