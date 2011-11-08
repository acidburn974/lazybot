
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace LazyLib.Helpers.Vendor
{
    public static class ProtectedList
    {
        private static List<string> _protectedList = new List<string>();

        public static List<string> GetList
        {
            get { return _protectedList; }
        }

        public static void Clear()
        {
            _protectedList.Clear();
        }

        public static void AddProtected(string name)
        {
            _protectedList.Add(name);
        }

        public static void Load()
        {
            _protectedList = new List<string>();
            try
            {
                if (File.Exists(LazySettings.OurDirectory + "\\Settings\\ProtectedList.xml"))
                {
                    var doc = new XmlDocument();
                    doc.Load(LazySettings.OurDirectory + "\\Settings\\ProtectedList.xml");
                    try
                    {
                        XmlNodeList mailList = doc.GetElementsByTagName("Protected");
                        _protectedList.AddRange(from XmlNode mail in mailList select mail.ChildNodes[0].Value);
                    }
                    catch (Exception e)
                    {
                        Logging.Write("Error loading ProtectedList: " + e);
                    }
                }
                else
                {
                    Logging.Write("Could not find the file ProtectedList.xml will not mail anything");
                }
            }
            catch (Exception e)
            {
                Logging.Write("Error loading ProtectedList list: " + e);
            }
        }

        public static void Save()
        {
            string xml = @"<?xml version=""1.0""?>";
            xml += "<ProtectedList>";
            foreach (string prot in _protectedList)
            {
                xml += "<Protected>" + prot + "</Protected>";
            }
            xml += "</ProtectedList>";
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            if (Directory.Exists(LazySettings.OurDirectory + "\\Settings\\"))
                Directory.CreateDirectory(LazySettings.OurDirectory + "\\Settings\\");
            doc.Save(LazySettings.OurDirectory + "\\Settings\\ProtectedList.xml");
        }

        public static bool ShouldVendor(string name)
        {
            if (_protectedList.Contains(name))
                return false;
            return true;
        }
    }
}