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

using System.IO;
using System.Windows.Forms;
using LazyLib.Helpers;

namespace LazyEvo.LGrindEngine.Helpers
{
    internal class GrindingSettings
    {
        public const string SettingsName = "\\Settings\\lazy_grinding.ini";
        public static string OurDirectory;
        public static bool Skin;
        public static bool WaitForLoot;
        public static bool StopLootOnFull;
        public static bool Loot;
        public static bool Mount;
        public static bool Jump;
        public static string Profile;
        public static int ApproachRange;
        public static bool SkipMobsWithAdds;
        public static int SkipAddsDistance;
        public static int SkipAddsCount;
        public static bool ShouldTrain;

        public static void LoadSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            var pIniManager = new IniManager(OurDirectory + SettingsName);
            Skin = pIniManager.GetBoolean("Grinding", "Skin", false);
            WaitForLoot = pIniManager.GetBoolean("Grinding", "WaitForLoot", false);
            StopLootOnFull = pIniManager.GetBoolean("Grinding", "StopLootOnFull", false);
            Loot = pIniManager.GetBoolean("Grinding", "Loot", true);
            Mount = pIniManager.GetBoolean("Grinding", "Mount", true);
            ApproachRange = pIniManager.GetInt("Grinding", "ApproachRange", 40);
            Profile = pIniManager.GetString("Grinding", "Profile", string.Empty);
            Jump = pIniManager.GetBoolean("Grinding", "Jump", false);
            SkipMobsWithAdds = pIniManager.GetBoolean("Grinding", "SkipMobsWithAdds", false);
            //ShouldTrain = pIniManager.GetBoolean("Grinding", "ShouldTrain", false);
            ShouldTrain = false;
            SkipAddsDistance = pIniManager.GetInt("Grinding", "SkipAddsDistance", 20);
            SkipAddsCount = pIniManager.GetInt("Grinding", "SkipAddsCount", 2);
        }

        public static void SaveSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            var pIniManager = new IniManager(OurDirectory + SettingsName);
            pIniManager.IniWriteValue("Grinding", "Profile", Profile);
            pIniManager.IniWriteValue("Grinding", "Skin", Skin.ToString());
            pIniManager.IniWriteValue("Grinding", "WaitForLoot", WaitForLoot.ToString());
            pIniManager.IniWriteValue("Grinding", "StopLootOnFull", StopLootOnFull.ToString());
            pIniManager.IniWriteValue("Grinding", "Loot", Loot.ToString());
            pIniManager.IniWriteValue("Grinding", "Mount", Mount.ToString());
            pIniManager.IniWriteValue("Grinding", "Jump", Jump.ToString());
            pIniManager.IniWriteValue("Grinding", "ApproachRange", ApproachRange.ToString());
            pIniManager.IniWriteValue("Grinding", "SkipMobsWithAdds", SkipMobsWithAdds.ToString());
            pIniManager.IniWriteValue("Grinding", "SkipAddsDistance", SkipAddsDistance.ToString());
            pIniManager.IniWriteValue("Grinding", "SkipAddsCount", SkipAddsCount.ToString());
            pIniManager.IniWriteValue("Grinding", "ShouldTrain", ShouldTrain.ToString());
        }
    }
}