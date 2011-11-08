
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
using System.IO;
using System.Windows.Forms;
using LazyLib.Helpers;

namespace LFishingEngine
{
    internal class FishingSettings
    {
        public const string SettingsName = "\\Settings\\lazy_fishing.ini";
        public static string OurDirectory;
        internal static string LureBar { get; set; }
        internal static string LureKey { get; set; }
        internal static bool UseLure { get; set; }
        public static void LoadSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            var pIniManager = new IniManager(OurDirectory + SettingsName);
            LureBar = pIniManager.GetString("Fishing", "LureBar", "1");
            LureKey = pIniManager.GetString("Fishing", "LureKey", "1");
            UseLure = pIniManager.GetBoolean("Fishing", "UseLure", false);
        }

        public static void SaveSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            var pIniManager = new IniManager(OurDirectory + SettingsName);
            pIniManager.IniWriteValue("Fishing", "LureBar", LureBar);
            pIniManager.IniWriteValue("Fishing", "LureKey", LureKey);
            pIniManager.IniWriteValue("Fishing", "UseLure", UseLure.ToString());
        }
    }
}