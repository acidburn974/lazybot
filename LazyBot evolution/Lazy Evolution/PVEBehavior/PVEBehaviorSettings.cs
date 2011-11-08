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
using LazyLib.Helpers;

namespace LazyEvo.PVEBehavior
{
    internal class PveBehaviorSettings
    {
        public const string SettingsName = "\\Settings\\pve_behavior.ini";
        public static bool AllowScripts;
        public static string LoadedBeharvior;
        public static bool AvoidAddsCombat; //TODO
        public static int SkipAddsDis;

        public static void LoadSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            string ourDirectory = executableDirectoryName;
            var pIniManager = new IniManager(ourDirectory + SettingsName);
            LoadedBeharvior = pIniManager.GetString("Config", "LoadedBeharvior", String.Empty);
            AvoidAddsCombat = pIniManager.GetBoolean("Config", "AvoidAddsCombat", false);
            SkipAddsDis = pIniManager.GetInt("Config", "SkipAddsDis", 0);
            AllowScripts = pIniManager.GetBoolean("Config", "AllowScripts", false);
        }

        public static void SaveSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            string ourDirectory = executableDirectoryName;
            var pIniManager = new IniManager(ourDirectory + SettingsName);
            pIniManager.IniWriteValue("Config", "LoadedBeharvior", LoadedBeharvior);
            pIniManager.IniWriteValue("Config", "AvoidAddsCombat", AvoidAddsCombat.ToString());
            pIniManager.IniWriteValue("Config", "SkipAddsDis", SkipAddsDis.ToString());
            pIniManager.IniWriteValue("Config", "AllowScripts", AllowScripts);
        }
    }
}