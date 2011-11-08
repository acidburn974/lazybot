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
    internal class Herb
    {
        private static List<string> _herb;
        private static string _herbXmlPath;

        public static void Load()
        {
            _herb = new List<string>();
            string herbFile;
            switch (LazySettings.Language)
            {
                case LazySettings.LazyLanguage.Russian:
                    herbFile = "ru";
                    break;
                case LazySettings.LazyLanguage.German:
                    herbFile = "de";
                    break;
                case LazySettings.LazyLanguage.French:
                    herbFile = "fr";
                    break;
                case LazySettings.LazyLanguage.Spanish:
                    herbFile = "es";
                    break;
                default:
                    herbFile = "en";
                    break;
            }
            _herbXmlPath = string.Format("{0}\\Collect\\Herb_{1}.xml", FlyingEngine.OurDirectory, herbFile);
            try
            {
                if (File.Exists(_herbXmlPath))
                {
                    var doc = new XmlDocument();
                    doc.Load(_herbXmlPath);
                    try
                    {
                        XmlNodeList herb = doc.GetElementsByTagName("Herb");
                        _herb.AddRange(from XmlNode her in herb select her.ChildNodes[0].Value);
                    }
                    catch (Exception e)
                    {
                        Logging.Write(LogType.Warning, "Error loading list with herbs: " + e);
                    }
                }
                else
                {
                    Logging.Write(LogType.Warning, "Could not find the file {0}", _herbXmlPath);
                }
            }
            catch (Exception e)
            {
                Logging.Write(LogType.Warning, "Error loading herb list: " + e);
            }
        }

        public static void Save()
        {
            string xml = @"<?xml version=""1.0""?>";
            xml += "<HerbList>";
            foreach (string prot in _herb)
            {
                xml += "<Herb>" + prot + "</Herb>";
            }
            xml += "</HerbList>";
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Save(_herbXmlPath);
        }

        public static void AddHerb(string name)
        {
            _herb.Add(name);
        }

        public static void Clear()
        {
            _herb.Clear();
        }

        public static List<string> GetList()
        {
            return _herb;
        }
    }
}