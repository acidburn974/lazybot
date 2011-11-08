
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

namespace LazyLib
{
    public class GeomertrySettings
    {
        public const string SettingsName = "\\Settings\\geometry.ini";
        public static string OurDirectory;
        public static string MainGeometry;
        public static string RotatorStatus;
        public static string RotationForm;
        public static string ProcessSelector;
        public static string RotatorForm;
        public static string RuleEditor;
        public static string ScriptEditor;

        public static void LoadSettings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            var pIniManager = new IniManager(OurDirectory + SettingsName);
            MainGeometry = pIniManager.GetString("Geometry", "MainGeometry", string.Empty);
            RotatorStatus = pIniManager.GetString("Geometry", "RotatorStatus", string.Empty);
            RotationForm = pIniManager.GetString("Geometry", "RotationForm", string.Empty);
            ProcessSelector = pIniManager.GetString("Geometry", "ProcessSelector", string.Empty);
            RotatorForm = pIniManager.GetString("Geometry", "RotatorForm", string.Empty);
            RuleEditor = pIniManager.GetString("Geometry", "RuleEditor", string.Empty);
            ScriptEditor = pIniManager.GetString("Geometry", "ScriptEditor", string.Empty);
        }

        public static void Save()
        {
             var pIniManager = new IniManager(OurDirectory + SettingsName);
             pIniManager.IniWriteValue("Geometry", "MainGeometry", MainGeometry);
             pIniManager.IniWriteValue("Geometry", "RotatorStatus", RotatorStatus);
             pIniManager.IniWriteValue("Geometry", "RotationForm", RotationForm);
             pIniManager.IniWriteValue("Geometry", "ProcessSelector", ProcessSelector);
             pIniManager.IniWriteValue("Geometry", "RotatorForm", RotatorForm);
             pIniManager.IniWriteValue("Geometry", "RuleEditor", RuleEditor);
             pIniManager.IniWriteValue("Geometry", "ScriptEditor", ScriptEditor);
        }
    }
}