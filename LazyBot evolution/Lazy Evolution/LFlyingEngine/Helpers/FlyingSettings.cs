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

namespace LazyEvo.LFlyingEngine.Helpers
{
    internal class FlyingSettings
    {
        internal const string SettingsName = "\\Settings\\lazy_flying.ini";
        internal static string OurDirectory;
        internal static bool Herb;
        internal static bool Mine;
        internal static float ApproachModifier;
        internal static string MaxUnits;
        internal static bool StopOnDeath;
        internal static bool StopHarvestWithPlayerAround;
        internal static bool AvoidPlayers;
        internal static bool AutoBlacklist;
        internal static bool AvoidElites;
        internal static bool StopOnFullBags;
        internal static bool WaitForLoot;
        internal static bool WaitForRessSick;
        internal static string FlyingMountBar;
        internal static string FlyingMountKey;
        internal static string Profile;
        internal static bool FindCorpse;
        internal static bool Fish;
        internal static bool Lure;
        internal static double MaxTimeAtSchool;
        internal static double FishApproach;
        internal static string LureBar;
        internal static string LureKey;
        internal static string WaterwalkBar;
        internal static string WaterwalkKey;
        internal static string ExtraBar;
        internal static string ExtraKey;
        public static bool DruidAvoidCombat;
        public static bool SendKeyOnStartCombat;

        public static void LoadSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            var pIniManager = new IniManager(OurDirectory + SettingsName);
            Herb = pIniManager.GetBoolean("Flying", "Herb", false);
            Mine = pIniManager.GetBoolean("Flying", "Mine", false);
            ApproachModifier = (float) Convert.ToDouble(pIniManager.GetString("Flying", "ApproachModifier", "0"));
            MaxUnits = pIniManager.GetString("Flying", "MaxUnits", "3");
            StopOnDeath = pIniManager.GetBoolean("Flying", "StopOnDeath", false);
            StopHarvestWithPlayerAround = pIniManager.GetBoolean("Flying", "StopHarvest", true);
            AvoidPlayers = pIniManager.GetBoolean("Flying", "AvoidPlayers", true);
            AutoBlacklist = pIniManager.GetBoolean("Flying", "AutoBlacklist", false);
            StopOnFullBags = pIniManager.GetBoolean("Flying", "StopOnFullBags", false);
            AvoidElites = pIniManager.GetBoolean("Flying", "AvoidElites", true);
            FindCorpse = pIniManager.GetBoolean("Flying", "FindCorpse", true);
            WaitForLoot = pIniManager.GetBoolean("Flying", "WaitForLoot", true);
            WaitForRessSick = pIniManager.GetBoolean("Flying", "WaitForRessSick", false);
            FlyingMountBar = pIniManager.GetString("Flying", "FlyingMountBar", "0");
            FlyingMountKey = pIniManager.GetString("Flying", "FlyingMountKey", "0");
            Profile = pIniManager.GetString("Flying", "Profile", string.Empty);
            DruidAvoidCombat = pIniManager.GetBoolean("Flying", "DruidAvoidCombat", false);
            Fish = pIniManager.GetBoolean("Flying", "Fish", false);
            Lure = pIniManager.GetBoolean("Flying", "Lure", false);
            SendKeyOnStartCombat = pIniManager.GetBoolean("Flying", "SendKeyOnStartCombat", false);
            MaxTimeAtSchool = Convert.ToDouble(pIniManager.GetString("Flying", "MaxTimeAtSchool", "4"));
            FishApproach = Convert.ToDouble(pIniManager.GetString("Flying", "FishApproach", "30"));
            LureBar = pIniManager.GetString("Flying", "LureBar", "1");
            LureKey = pIniManager.GetString("Flying", "LureKey", "1");
            WaterwalkBar = pIniManager.GetString("Flying", "WaterwalkBar", "1");
            WaterwalkKey = pIniManager.GetString("Flying", "WaterwalkKey", "1");
            ExtraBar = pIniManager.GetString("Flying", "ExtraBar", "1");
            ExtraKey = pIniManager.GetString("Flying", "ExtraKey", "1");
        }

        public static void SaveSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            var pIniManager = new IniManager(OurDirectory + SettingsName);
            pIniManager.IniWriteValue("Flying", "Herb", Herb.ToString());
            pIniManager.IniWriteValue("Flying", "Mine", Mine.ToString());
            pIniManager.IniWriteValue("Flying", "ApproachModifier", ApproachModifier.ToString());
            pIniManager.IniWriteValue("Flying", "MaxUnits", MaxUnits);
            pIniManager.IniWriteValue("Flying", "StopOnDeath", StopOnDeath.ToString());
            pIniManager.IniWriteValue("Flying", "StopHarvest", StopHarvestWithPlayerAround.ToString());
            pIniManager.IniWriteValue("Flying", "StopOnFullBags", StopOnFullBags.ToString());
            pIniManager.IniWriteValue("Flying", "AvoidPlayers", AvoidPlayers.ToString());
            pIniManager.IniWriteValue("Flying", "AutoBlacklist", AutoBlacklist.ToString());
            pIniManager.IniWriteValue("Flying", "AvoidElites", AvoidElites.ToString());
            pIniManager.IniWriteValue("Flying", "FindCorpse", FindCorpse.ToString());
            pIniManager.IniWriteValue("Flying", "WaitForLoot", WaitForLoot.ToString());
            pIniManager.IniWriteValue("Flying", "WaitForRessSick", WaitForRessSick.ToString());
            pIniManager.IniWriteValue("Flying", "FlyingMountBar", FlyingMountBar);
            pIniManager.IniWriteValue("Flying", "FlyingMountKey", FlyingMountKey);
            pIniManager.IniWriteValue("Flying", "Profile", Profile);
            pIniManager.IniWriteValue("Flying", "DruidAvoidCombat", DruidAvoidCombat);
            pIniManager.IniWriteValue("Flying", "Fish", Fish);
            pIniManager.IniWriteValue("Flying", "Lure", Lure);
            pIniManager.IniWriteValue("Flying", "MaxTimeAtSchool", MaxTimeAtSchool);
            pIniManager.IniWriteValue("Flying", "FishApproach", FishApproach);
            pIniManager.IniWriteValue("Flying", "LureBar", LureBar);
            pIniManager.IniWriteValue("Flying", "LureKey", LureKey);
            pIniManager.IniWriteValue("Flying", "WaterwalkBar", WaterwalkBar);
            pIniManager.IniWriteValue("Flying", "WaterwalkKey", WaterwalkKey);
            pIniManager.IniWriteValue("Flying", "ExtraBar", ExtraBar);
            pIniManager.IniWriteValue("Flying", "ExtraKey", ExtraKey);
            pIniManager.IniWriteValue("Flying", "SendKeyOnStartCombat", SendKeyOnStartCombat);
        }
    }
}