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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using LazyLib;

#endregion

namespace LazyEvo.LFlyingEngine.Helpers
{
    internal static class Mine
    {
        private static List<string> _mine;
        private static string _mineXmlPath;

        public static List<string> Load()
        {
            _mine = new List<string>();
            string mineFile;
            switch (LazySettings.Language)
            {
                case LazySettings.LazyLanguage.Russian:
                    mineFile = "ru";
                    break;
                case LazySettings.LazyLanguage.German:
                    mineFile = "de";
                    break;
                case LazySettings.LazyLanguage.French:
                    mineFile = "fr";
                    break;
                case LazySettings.LazyLanguage.Spanish:
                    mineFile = "es";
                    break;
                default:
                    mineFile = "en";
                    break;
            }
            _mineXmlPath = string.Format("{0}\\Collect\\Mine_{1}.xml", FlyingEngine.OurDirectory, mineFile);
            try
            {
                if (File.Exists(_mineXmlPath))
                {
                    var doc = new XmlDocument();
                    doc.Load(_mineXmlPath);
                    try
                    {
                        XmlNodeList herb = doc.GetElementsByTagName("Mine");
                        _mine.AddRange(from XmlNode her in herb select her.ChildNodes[0].Value);
                    }
                    catch (Exception e)
                    {
                        Logging.Write(LogType.Warning, "Error loading list with mines: " + e);
                    }
                }
                else
                {
                    Logging.Write(LogType.Warning, "Could not find the file {0}", _mineXmlPath);
                }
            }
            catch (Exception e)
            {
                Logging.Write(LogType.Warning, "Error loading mines list: " + e);
            }
            return _mine;
        }

        public static void Save()
        {
            string xml = @"<?xml version=""1.0""?>";
            xml += "<MineList>";
            foreach (string prot in _mine)
            {
                xml += "<Mine>" + prot + "</Mine>";
            }
            xml += "</MineList>";
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Save(_mineXmlPath);
        }

        public static void AddMine(string name)
        {
            _mine.Add(name);
        }

        public static void Clear()
        {
            _mine.Clear();
        }

        public static List<string> GetList()
        {
            return _mine;
        }
    }
}