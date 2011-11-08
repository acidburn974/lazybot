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

using System;
using System.IO;
using System.Windows.Forms;
using LazyLib;
using LazyLib.Helpers;

namespace LazyEvo.Plugins
{
    internal class ReloggerSettings
    {
        public const string SettingsName = "\\Settings\\lazy_relog.ini";
        public static string OurDirectory;
        public static string CharacterName;
        public static string AccountName { get; set; }
        public static string AccountPw { get; set; }
        //public static string _CharacterName { get; set; }
        public static bool ReloggingEnabled { get; set; }
        public static bool PeriodicReloggingEnabled { get; set; }
        public static int PeriodicLogOut { get; set; }
        public static int PeriodicLogIn { get; set; }
        public static int AccountAccount { get; set; }


        public static void SaveSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            var pIniManager = new IniManager(OurDirectory + SettingsName);

            pIniManager.IniWriteValue("Relog", "AccoutnName", Encryptor.Encrypt(AccountName));
            pIniManager.IniWriteValue("Relog", "AccoutnPW", Encryptor.Encrypt(AccountPw));
            pIniManager.IniWriteValue("Relog", "EnableRelogging", ReloggingEnabled.ToString());
            pIniManager.IniWriteValue("Relog", "EnablePeriodicRelogging", PeriodicReloggingEnabled.ToString());
            pIniManager.IniWriteValue("Relog", "PeriodicLogOut", PeriodicLogOut.ToString());
            pIniManager.IniWriteValue("Relog", "PeriodicLogIn", PeriodicLogIn.ToString());
            pIniManager.IniWriteValue("Relog", "AccountAccount", AccountAccount.ToString());
            pIniManager.IniWriteValue("Relog", "CharacterName", CharacterName);
        }

        public static void LoadSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            var pIniManager = new IniManager(OurDirectory + SettingsName);
            try
            {
                if (!string.IsNullOrEmpty(pIniManager.GetString("Relog", "AccoutnName", string.Empty)))
                {
                    AccountName = Encryptor.Decrypt(pIniManager.GetString("Relog", "AccoutnName", string.Empty));
                }
                else
                {
                    AccountName = string.Empty;
                }
                if (!string.IsNullOrEmpty(pIniManager.GetString("Relog", "AccoutnPW", string.Empty)))
                {
                    AccountPw = Encryptor.Decrypt(pIniManager.GetString("Relog", "AccoutnPW", string.Empty));
                }
                else
                {
                    AccountPw = string.Empty;
                }
                ReloggingEnabled = pIniManager.GetBoolean("Relog", "EnableRelogging", false);
                PeriodicReloggingEnabled = pIniManager.GetBoolean("Relog", "EnablePeriodicRelogging", false);
                PeriodicLogOut = pIniManager.GetInt("Relog", "PeriodicLogOut", 60);
                PeriodicLogIn = pIniManager.GetInt("Relog", "PeriodicLogIn", 30);
                AccountAccount = pIniManager.GetInt("Relog", "AccountAccount", 1);
                CharacterName = pIniManager.GetString("Relog", "CharacterName", string.Empty);
            }
            catch (Exception)
            {
                Logging.Debug("Could not load relogger settings. All relogger values have been reset.");
                AccountName = "";
                AccountPw = "";
                ReloggingEnabled = false;
                PeriodicReloggingEnabled = false;
                PeriodicLogIn = 30;
                PeriodicLogOut = 60;
                AccountAccount = 1;
            }
        }
    }
}