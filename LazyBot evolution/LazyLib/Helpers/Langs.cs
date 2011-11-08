
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
#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

#endregion

namespace LazyLib.Helpers
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class Langs
    {
        private static List<string> _bagsFull;
        private static List<string> _skillToLow;
        private static List<string> _druidFlying;
        private static List<string> _mountCantMount;
        private static List<string> _cantDoThatWhileMoving;
        private static XmlDocument _xmlDoc;
        private static List<string> _trainingDummy;
        private static List<string> _youHaveBeen;
        public static void Load()
        {
            _bagsFull = new List<string>();
            _skillToLow = new List<string>();
            _druidFlying = new List<string>();
            _mountCantMount = new List<string>();
            _cantDoThatWhileMoving = new List<string>();
            _trainingDummy = new List<string>();
            _youHaveBeen = new List<string>();
            try
            {
                _xmlDoc = new XmlDocument();
                _xmlDoc.Load(LazySettings.OurDirectory + "\\Langs.xml");
            }
            catch (Exception)
            {
                Logging.Write("Could not load Langs.xml, check if the file is corrupted");
                return;
            }
            try
            {
                LoadLang("en");
                LoadLang("de");
                LoadLang("fr");
                LoadLang("ru");
                LoadLang("pt");
            }
            catch (Exception)
            {
                Logging.Write("Could not load Langs.xml (Load langs), check if the file is corrupted");
                return;
            }
        }

        private static void LoadLang(string lang)
        {
            XmlNode em = _xmlDoc.GetElementsByTagName(lang)[0];
            foreach (XmlNode node in em.ChildNodes)
            {
                if (node.Name.Equals("BagsFull"))
                    _bagsFull.Add(node.InnerText.ToUpper());
                if (node.Name.Equals("SkillToLow"))
                    _skillToLow.Add(node.InnerText.ToUpper());
                if (node.Name.Equals("DruidFlying"))
                    _druidFlying.Add(node.InnerText.ToUpper());
                if (node.Name.Equals("ShapeShift"))
                    _druidFlying.Add(node.InnerText.ToUpper());
                if (node.Name.Equals("CantMount"))
                    _mountCantMount.Add(node.InnerText.ToUpper());
                if (node.Name.Equals("WhileMoving"))
                    _cantDoThatWhileMoving.Add(node.InnerText.ToUpper());
                if (node.Name.Equals("TrainingDummy"))
                    _trainingDummy.Add(node.InnerText.ToUpper());
                if (node.Name.Equals("YouHaveBeen"))
                    _youHaveBeen.Add(node.InnerText.ToUpper());
            }
        }

        public static bool TrainingDummy(string text)
        {
            return _trainingDummy.Any(text.ToUpper().Contains);
        }

        public static bool CantDoThatWhileMoving(string text)
        {
            return _cantDoThatWhileMoving.Any(text.ToUpper().Contains);
        }

        public static bool MountCantMount(string text)
        {
            return _mountCantMount.Any(text.ToUpper().Contains);
        }

        public static bool BagsFull(string text)
        {
            return _bagsFull.Any(text.ToUpper().Contains);
        }

        public static bool SkillToLow(string text)
        {
            return _skillToLow.Any(text.ToUpper().Contains);
        }

        public static bool DruidFlying(string text)
        {
            return _druidFlying.Any(text.ToUpper().Contains);
        }

        public static bool YouhaveBeen(string text)
        {
            return _youHaveBeen.Any(text.ToUpper().Contains);
        }
    }
}