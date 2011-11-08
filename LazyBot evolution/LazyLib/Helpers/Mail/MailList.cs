
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

namespace LazyLib.Helpers.Mail
{
    public static class MailList
    {
        private static List<string> _mailList = new List<string>();

        public static List<string> GetList
        {
            get { return _mailList; }
        }

        public static void Clear()
        {
            _mailList.Clear();
        }

        public static void AddMail(string name)
        {
            _mailList.Add(name);
        }

        public static void Load()
        {
            _mailList = new List<string>();
            try
            {
                if (File.Exists(LazySettings.OurDirectory + "\\Settings\\MailList.xml"))
                {
                    var doc = new XmlDocument();
                    doc.Load(LazySettings.OurDirectory + "\\Settings\\MailList.xml");
                    try
                    {
                        XmlNodeList mailList = doc.GetElementsByTagName("Mail");
                        _mailList.AddRange(from XmlNode mail in mailList select mail.ChildNodes[0].Value);
                    }
                    catch (Exception e)
                    {
                        Logging.Write("Error loading MailList: " + e);
                    }
                }
                else
                {
                    Logging.Write("Could not find the file MailList.xml will not mail anything");
                }
            }
            catch (Exception e)
            {
                Logging.Write("Error loading mail list: " + e);
            }
        }

        public static void Save()
        {
            string xml = @"<?xml version=""1.0""?>";
            xml += "<MailList>";
            foreach (string mail in _mailList)
            {
                xml += "<Mail>" + mail + "</Mail>";
            }
            xml += "</MailList>";
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            if (Directory.Exists(LazySettings.OurDirectory + "\\Settings\\"))
                Directory.CreateDirectory(LazySettings.OurDirectory + "\\Settings\\");
            doc.Save(LazySettings.OurDirectory + "\\Settings\\MailList.xml");
        }

        public static bool ShouldMail(string name)
        {
            if (_mailList.Contains(name))
                return true;
            return false;
        }
    }
}