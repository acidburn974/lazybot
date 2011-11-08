/*
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
using System.IO;
using System.Windows.Forms;
using LazyLib;

#endregion

namespace LazyEvo.Forms.Helpers
{
    internal class CheckUpdate
    {
        private static string _ourDirectory;
        private static readonly string ExecutableName = Application.ExecutablePath;
        private static string _executableDirectoryName;
        private static FileInfo _executableFileInfo;

        public static void CheckForUpdate()
        {
#if !DEBUG
            _executableFileInfo = new FileInfo(ExecutableName);
            _executableDirectoryName = _executableFileInfo.DirectoryName;
            _ourDirectory = _executableDirectoryName;
            try
            {
                var proc = new Process
                               {
                                   StartInfo = new ProcessStartInfo(_ourDirectory + "\\wyUpdate.exe", "-quickcheck -justcheck -noerr")
                               };
                proc.Start();
                proc.WaitForExit();
                int exitCode = proc.ExitCode;
                proc.Close();
                if (exitCode == 2)
                {
                    MessageBox.Show(@"New update ready. Closing to update");
                    Process.Start(_ourDirectory + "\\wyUpdate.exe");
                    Environment.Exit(0);
                }
            }
            catch (Exception e)
            {
                Logging.Write("Could not start the updating program, cannot auto update: " + e);
            }
#endif
        }
    }
}