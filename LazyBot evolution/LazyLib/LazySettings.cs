
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
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using LazyLib.Helpers;

namespace LazyLib
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class LazySettings
    {
        public enum LazyLanguage
        {
            Unknown = 0,
            English = 1,
            Russian = 2,
            German = 3, 
            French = 4,
            Spanish = 5,
        }

        public const string SettingsName = "\\Settings\\lazy_evo.ini";
        public static string OurDirectory;
        public static int Latency;
        public static bool DebugLog;
        public static bool DebugMode;
        public static bool ShouldMail;
        public static string MailTo;
        public static string Password;
        public static string UserName;
        public static bool BackgroundMode;
        public static bool HookMouse;
        public static bool SetupUseHotkeys;
        public static bool StopAfterBool;
        public static string StopAfter;
        public static string LogOutOnFollowTime;
        public static bool SoundFollow;
        public static bool SoundWhisper;
        public static bool SoundStop;
        public static bool Shutdown;
        public static bool LogoutOnFollow;
        public static bool CombatBoolEat;
        public static bool CombatBoolDrink;
        public static string CombatEatAt;
        public static string CombatDrinkAt;
        public static bool UseCtm;
        public static string KeysGroundMountBar;
        public static string KeysGroundMountKey;
        public static string KeysAttack1Bar;
        public static string KeysAttack1Key;
        public static string KeysEatBar;
        public static string KeysEatKey;
        public static string KeysDrinkBar;
        public static string KeysDrinkKey;
        public static string KeysMoteExtractorBar;
        public static string KeysMoteExtractorKey;
        public static string KeysStafeLeftKeyText;
        public static string KeysStafeRightKeyText;
        public static string KeysInteractKeyText;
        public static string KeysInteractTargetText;
        public static string KeysTargetLastTargetText;
        public static bool ShouldVendor;
        public static bool ShouldRepair;
        public static bool SellCommon;
        public static bool SellUncommon;
        public static bool SellPoor;
        public static string FreeBackspace;
        public static string SelectedEngine;
        public static string SelectedCombat;
        public static bool FirstRun;
        public static bool MacroForMail;
        public static string KeysMailMacroBar;
        public static string KeysMailMacroKey;
        public static LazyLanguage Language;
        public static string TelnetPassword;
        public static int TelnetPort;

        public static void LoadSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            var pIniManager = new IniManager(OurDirectory + SettingsName);
            SelectedEngine = pIniManager.GetString("Engine", "Selected", string.Empty);
            SelectedCombat = pIniManager.GetString("Combat", "Selected", string.Empty);
            FirstRun = pIniManager.GetBoolean("Config", "FirstRun", true);
            DebugMode = pIniManager.GetBoolean("Config", "DebugMode", false);
            Password = pIniManager.GetString("Config", "UserName", String.Empty);
            UserName = pIniManager.GetString("Config", "Password", String.Empty);
            BackgroundMode = pIniManager.GetBoolean("Config", "BackgroundMode", false);
            HookMouse = pIniManager.GetBoolean("Config", "HookMouse", false);
            SetupUseHotkeys = pIniManager.GetBoolean("Config", "UseHotkeys", false);
            StopAfterBool = pIniManager.GetBoolean("Config", "StopAfter", false);
            StopAfter = pIniManager.GetString("Config", "StopAfterTime", "120");
            LogOutOnFollowTime = pIniManager.GetString("Config", "LogoutOnFollowTime", "2");
            SoundFollow = pIniManager.GetBoolean("Config", "FollowSound", true);
            SoundWhisper = pIniManager.GetBoolean("Config", "WhisperSound", true);
            SoundStop = pIniManager.GetBoolean("Config", "SoundStop", true);
            Shutdown = pIniManager.GetBoolean("Config", "ShutdownComputer", false);
            LogoutOnFollow = pIniManager.GetBoolean("Config", "LogoutOnFollow", false);
            UseCtm = pIniManager.GetBoolean("Config", "UseCtm", false);
            DebugLog = pIniManager.GetBoolean("Config", "DebugLog", false);
            Latency = pIniManager.GetInt("Config", "Latency", 0);
            Language = (LazyLanguage)pIniManager.GetInt("Config", "Language", 0);
            //
            CombatBoolEat = pIniManager.GetBoolean("Combat", "CBEat", true);
            CombatBoolDrink = pIniManager.GetBoolean("Combat", "CBDrink", true);
            CombatEatAt = pIniManager.GetString("Combat", "COEat", "0");
            CombatDrinkAt = pIniManager.GetString("Combat", "CODrink", "0");

            KeysGroundMountBar = pIniManager.GetString("Keys", "GroundMountBar", "1");
            KeysGroundMountKey = pIniManager.GetString("Keys", "GroundMountKey", "1");
            KeysAttack1Bar = pIniManager.GetString("Keys", "Attack1Bar", "1");
            KeysAttack1Key = pIniManager.GetString("Keys", "Attack1Key", "1");
            KeysEatBar = pIniManager.GetString("Keys", "EatBar", "1");
            KeysEatKey = pIniManager.GetString("Keys", "EatKey", "1");
            KeysDrinkBar = pIniManager.GetString("Keys", "DrinkBar", "1");
            KeysDrinkKey = pIniManager.GetString("Keys", "DrinkKey", "1");
            KeysMoteExtractorBar = pIniManager.GetString("Keys", "MoteBar", "1");
            KeysMoteExtractorKey = pIniManager.GetString("Keys", "MoteKey", "1");
            KeysStafeLeftKeyText = pIniManager.GetString("Keys", "StafeLeftKeyText", "Q");
            KeysStafeRightKeyText = pIniManager.GetString("Keys", "StafeRightKeyText", "E");
            KeysInteractKeyText = pIniManager.GetString("Keys", "InteractText", "U");
            KeysInteractTargetText = pIniManager.GetString("Keys", "InteractTargetText", "P");
            KeysTargetLastTargetText = pIniManager.GetString("Keys", "KeysTargetLastTargetText", "G");

            //Mail
            ShouldMail = pIniManager.GetBoolean("Mail", "ShouldMail", false);
            MailTo = pIniManager.GetString("Mail", "MailTo", string.Empty);
            MacroForMail = pIniManager.GetBoolean("Mail", "MacroForMail", false);
            KeysMailMacroBar = pIniManager.GetString("Mail", "KeysMailMacroBar", "1");
            KeysMailMacroKey = pIniManager.GetString("Mail", "KeysMailMacroKey", "1");

            //Vendor
            ShouldVendor = pIniManager.GetBoolean("Vendor", "ShouldVendor", false);
            ShouldRepair = pIniManager.GetBoolean("Vendor", "ShouldRepair", false);
            SellCommon = pIniManager.GetBoolean("Vendor", "SellCommon", false);
            SellUncommon = pIniManager.GetBoolean("Vendor", "SellUncommon", false);
            SellPoor = pIniManager.GetBoolean("Vendor", "SellPoor", false);
            FreeBackspace = pIniManager.GetString("Vendor", "FreeBackspace", "2"); 
        }

        public static void SaveSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            var pIniManager = new IniManager(OurDirectory + SettingsName);
            pIniManager.IniWriteValue("Engine", "Selected", SelectedEngine);
            pIniManager.IniWriteValue("Combat", "Selected", SelectedCombat);
            pIniManager.IniWriteValue("Config", "FirstRun", FirstRun);
            pIniManager.IniWriteValue("Config", "UserName", Password);
            pIniManager.IniWriteValue("Config", "Password", UserName);
            pIniManager.IniWriteValue("Config", "BackgroundMode", BackgroundMode);
            pIniManager.IniWriteValue("Config", "HookMouse", HookMouse);
            pIniManager.IniWriteValue("Config", "UseHotkeys", SetupUseHotkeys);
            pIniManager.IniWriteValue("Config", "StopAfter", StopAfterBool);
            pIniManager.IniWriteValue("Config", "StopAfterTime", StopAfter);
            pIniManager.IniWriteValue("Config", "FollowSound", SoundFollow);
            pIniManager.IniWriteValue("Config", "WhisperSound", SoundWhisper);
            pIniManager.IniWriteValue("Config", "SoundStop", SoundStop);
            pIniManager.IniWriteValue("Config", "ShutdownComputer", Shutdown);
            pIniManager.IniWriteValue("Config", "LogoutOnFollow", LogoutOnFollow);
            pIniManager.IniWriteValue("Config", "LogoutOnFollowTime", LogOutOnFollowTime);
            pIniManager.IniWriteValue("Config", "UseCtm", UseCtm);
            pIniManager.IniWriteValue("Config", "DebugLog", DebugLog);
            pIniManager.IniWriteValue("Config", "Latency", Latency);
            pIniManager.IniWriteValue("Config", "Language", Convert.ToInt32(Language));
            //Other
            pIniManager.IniWriteValue("Combat", "CBEat", CombatBoolEat);
            pIniManager.IniWriteValue("Combat", "CBDrink", CombatBoolDrink);
            pIniManager.IniWriteValue("Combat", "COEat", CombatEatAt);
            pIniManager.IniWriteValue("Combat", "CODrink", CombatDrinkAt);

            pIniManager.IniWriteValue("Keys", "GroundMountBar", KeysGroundMountBar);
            pIniManager.IniWriteValue("Keys", "GroundMountKey", KeysGroundMountKey);
            pIniManager.IniWriteValue("Keys", "Attack1Bar", KeysAttack1Bar);
            pIniManager.IniWriteValue("Keys", "Attack1Key", KeysAttack1Key);
            pIniManager.IniWriteValue("Keys", "EatBar", KeysEatBar);
            pIniManager.IniWriteValue("Keys", "EatKey", KeysEatKey);
            pIniManager.IniWriteValue("Keys", "DrinkBar", KeysDrinkBar);
            pIniManager.IniWriteValue("Keys", "DrinkKey", KeysDrinkKey);
            pIniManager.IniWriteValue("Keys", "MoteBar", KeysMoteExtractorBar);
            pIniManager.IniWriteValue("Keys", "MoteKey", KeysMoteExtractorKey);
            pIniManager.IniWriteValue("Keys", "InteractText", KeysInteractKeyText);
            pIniManager.IniWriteValue("Keys", "InteractTargetText", KeysInteractTargetText);
            pIniManager.IniWriteValue("Keys", "StafeLeftKeyText", KeysStafeLeftKeyText);
            pIniManager.IniWriteValue("Keys", "StafeRightKeyText", KeysStafeRightKeyText);
            pIniManager.IniWriteValue("Keys", "KeysTargetLastTargetText", KeysTargetLastTargetText);

            //Mail
            pIniManager.IniWriteValue("Mail", "ShouldMail", ShouldMail);
            pIniManager.IniWriteValue("Mail", "MailTo", MailTo);
            pIniManager.IniWriteValue("Mail", "MacroForMail", MacroForMail);
            pIniManager.IniWriteValue("Mail", "KeysMailMacroBar", KeysMailMacroBar);
            pIniManager.IniWriteValue("Mail", "KeysMailMacroKey", KeysMailMacroKey);

            //Vendor
            pIniManager.IniWriteValue("Vendor", "ShouldVendor", ShouldVendor);
            pIniManager.IniWriteValue("Vendor", "ShouldRepair", ShouldRepair);
            pIniManager.IniWriteValue("Vendor", "SellCommon", SellCommon);
            pIniManager.IniWriteValue("Vendor", "SellUncommon", SellUncommon);
            pIniManager.IniWriteValue("Vendor", "SellPoor", SellPoor);
            pIniManager.IniWriteValue("Vendor", "FreeBackspace", FreeBackspace); 
        }
    }
}