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
using System.Threading;
using System.Xml;
using LazyLib.Helpers;
using LazyLib.Wow;

#endregion

namespace LazyEvo.PVEBehavior.Behavior
{
    internal class ActionKey : Action
    {
        private Ticker _globalCooldown;
        private KeyWrapper _keyWrapper;
        private string _name;
        private int _times;

        public ActionKey()
        {
        }

        public ActionKey(string name, string bar, string key, string special, int times)
        {
            _times = times;
            Bar = bar;
            Key = key;
            Special = special;
            _name = name;
        }

        public string Bar { get; private set; }
        public string Key { get; private set; }
        public string Special { get; private set; }

        public KeyWrapper GetKey
        {
            get
            {
                if (_keyWrapper == null)
                    _keyWrapper = new KeyWrapper(_name, Special, Bar, Key);
                return _keyWrapper;
            }
        }

        public int Times
        {
            get { return _times; }
        }

        public override bool IsReady
        {
            get { return true; }
        }

        public override bool DoesKeyExist
        {
            get { return true; }
        }

        public override string Name
        {
            get { return _name; }
        }

        public override void Execute(int globalCooldown)
        {
            if (_globalCooldown == null)
            {
                _globalCooldown = new Ticker(globalCooldown);
            }
            if (_times > 0)
            {
                int i = _times;
                while (i > 0)
                {
                    GetKey.SendKey();
                    _globalCooldown.Reset();
                    while (ObjectManager.MyPlayer.IsCasting || !_globalCooldown.IsReady)
                    {
                        Thread.Sleep(10);
                    }
                    i--;
                }
            }
            else
            {
                if (ObjectManager.MyPlayer.IsValid && !ObjectManager.MyPlayer.IsMe)
                    ObjectManager.MyPlayer.Target.Face();
                GetKey.SendKey();
                while (ObjectManager.MyPlayer.IsCasting || !_globalCooldown.IsReady)
                {
                    Thread.Sleep(10);
                }
            }
        }

        public override string GetXml()
        {
            string xml = "<Type>ActionKey</Type>";
            xml += "<Name>" + _name + "</Name>";
            xml += "<Bar>" + Bar + "</Bar>";
            xml += "<Key>" + Key + "</Key>";
            xml += "<Special>" + Special + "</Special>";
            xml += "<Times>" + _times + "</Times>";
            return xml;
        }

        public override void Load(XmlNode node)
        {
            foreach (XmlNode xmlNode in node)
            {
                if (xmlNode.Name.Equals("Name"))
                {
                    _name = xmlNode.InnerText;
                }
                if (xmlNode.Name.Equals("Bar"))
                {
                    Bar = xmlNode.InnerText;
                }
                if (xmlNode.Name.Equals("Key"))
                {
                    Key = xmlNode.InnerText;
                }
                if (xmlNode.Name.Equals("Special"))
                {
                    Special = xmlNode.InnerText;
                }
                if (xmlNode.Name.Equals("Times"))
                {
                    _times = Convert.ToInt32(xmlNode.InnerText);
                }
            }
        }
    }
}