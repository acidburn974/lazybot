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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using LazyLib.Helpers;

namespace LazyEvo.Plugins.RotationPlugin
{
    internal class RotationSettings
    {
        public const string SettingsName = "\\Settings\\rotation.ini";
        public static string LoadedRotationManager;
        public static List<KeysData> KeysList = new List<KeysData>();

        public static void LoadSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            string ourDirectory = executableDirectoryName;
            var pIniManager = new IniManager(ourDirectory + SettingsName);
            LoadedRotationManager = pIniManager.GetString("Config", "LoadedRotationManager", String.Empty);
            LoadKeys();
        }

        public static void SaveSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            string ourDirectory = executableDirectoryName;
            var pIniManager = new IniManager(ourDirectory + SettingsName);
            pIniManager.IniWriteValue("Config", "LoadedRotationManager", LoadedRotationManager);
        }

        private static void LoadKeys()
        {
            KeysList.Add(new KeysData("Back", 8));
            KeysList.Add(new KeysData("Tab", 9));
            KeysList.Add(new KeysData("Enter", 13));
            KeysList.Add(new KeysData("ESC", 27));
            KeysList.Add(new KeysData("Space", 32));
            KeysList.Add(new KeysData("PAGE UP", 33));
            KeysList.Add(new KeysData("Page Down", 34));
            KeysList.Add(new KeysData("END", 35));
            KeysList.Add(new KeysData("Home", 36));
            KeysList.Add(new KeysData("Left", 37));
            KeysList.Add(new KeysData("Up", 38));
            KeysList.Add(new KeysData("Right", 39));
            KeysList.Add(new KeysData("Down", 40));
            KeysList.Add(new KeysData("PrintScreen", 44));
            KeysList.Add(new KeysData("Insert", 45));
            KeysList.Add(new KeysData("D 0", 48));
            KeysList.Add(new KeysData("D 1", 49));
            KeysList.Add(new KeysData("D 2", 50));
            KeysList.Add(new KeysData("D 3", 51));
            KeysList.Add(new KeysData("D 4", 52));
            KeysList.Add(new KeysData("D 5", 53));
            KeysList.Add(new KeysData("D 6", 54));
            KeysList.Add(new KeysData("D 7", 55));
            KeysList.Add(new KeysData("D 8", 56));
            KeysList.Add(new KeysData("D 9", 57));
            KeysList.Add(new KeysData("A", 65));
            KeysList.Add(new KeysData("B", 66));
            KeysList.Add(new KeysData("C", 67));
            KeysList.Add(new KeysData("D", 68));
            KeysList.Add(new KeysData("E", 69));
            KeysList.Add(new KeysData("F", 70));
            KeysList.Add(new KeysData("G", 71));
            KeysList.Add(new KeysData("H", 72));
            KeysList.Add(new KeysData("I", 73));
            KeysList.Add(new KeysData("J", 74));
            KeysList.Add(new KeysData("K", 75));
            KeysList.Add(new KeysData("L", 76));
            KeysList.Add(new KeysData("M", 77));
            KeysList.Add(new KeysData("N", 78));
            KeysList.Add(new KeysData("O", 79));
            KeysList.Add(new KeysData("P", 80));
            KeysList.Add(new KeysData("Q", 81));
            KeysList.Add(new KeysData("R", 82));
            KeysList.Add(new KeysData("S", 83));
            KeysList.Add(new KeysData("T", 84));
            KeysList.Add(new KeysData("U", 85));
            KeysList.Add(new KeysData("V", 86));
            KeysList.Add(new KeysData("W", 87));
            KeysList.Add(new KeysData("X", 88));
            KeysList.Add(new KeysData("Y", 89));
            KeysList.Add(new KeysData("Z", 90));


            KeysList.Add(new KeysData("NumPad 0", 96));
            KeysList.Add(new KeysData("NumPad 1", 97));
            KeysList.Add(new KeysData("NumPad 2", 98));
            KeysList.Add(new KeysData("NumPad 3", 99));
            KeysList.Add(new KeysData("NumPad 4", 100));
            KeysList.Add(new KeysData("NumPad 5", 101));
            KeysList.Add(new KeysData("NumPad 6", 102));
            KeysList.Add(new KeysData("NumPad 7", 103));
            KeysList.Add(new KeysData("NumPad 8", 104));
            KeysList.Add(new KeysData("NumPad 9", 105));

            KeysList.Add(new KeysData("*", 106));
            KeysList.Add(new KeysData("+", 107));
            KeysList.Add(new KeysData("-", 109));
            KeysList.Add(new KeysData("Decimal", 110));
            KeysList.Add(new KeysData("/", 111));
            KeysList.Add(new KeysData("F1", 112));
            KeysList.Add(new KeysData("F2", 113));
            KeysList.Add(new KeysData("F3", 114));
            KeysList.Add(new KeysData("F4", 115));
            KeysList.Add(new KeysData("F5", 116));
            KeysList.Add(new KeysData("F6", 117));
            KeysList.Add(new KeysData("F7", 118));
            KeysList.Add(new KeysData("F8", 119));
            KeysList.Add(new KeysData("F9", 120));
            KeysList.Add(new KeysData("F10", 121));
            KeysList.Add(new KeysData("F11", 122));
            KeysList.Add(new KeysData("F12", 123));
        }
    }
}