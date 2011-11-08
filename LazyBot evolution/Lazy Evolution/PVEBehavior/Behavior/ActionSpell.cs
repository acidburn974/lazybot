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

using System.Xml;
using LazyLib.ActionBar;
using LazyLib.Helpers;

#endregion

namespace LazyEvo.PVEBehavior.Behavior
{
    internal class ActionSpell : Action
    {
        private bool Exist;
        private string _name;
        private BarSpell _spell;
        private bool chek;

        public ActionSpell()
        {
        }

        public ActionSpell(string name)
        {
            _name = name;
        }

        public override bool DoesKeyExist
        {
            get
            {
                if (!chek)
                {
                    chek = true;
                    Exist = BarMapper.HasSpellByName(_name);
                }
                return Exist;
            }
        }

        public override string Name
        {
            get { return _name; }
        }

        public override bool IsReady
        {
            get { return BarMapper.IsSpellReadyByName(_name); }
        }

        public override void Execute(int globalcooldown)
        {
            if (DoesKeyExist)
            {
                if (!KeyHelper.HasKey(_name))
                    _spell = null;
                //Load the spell and set the global cooldown
                if (_spell == null)
                {
                    _spell = BarMapper.GetSpellByName(_name);
                    _spell.SetCooldown(globalcooldown);
                    KeyHelper.AddKey(_name, "", _spell.Bar.ToString(), _spell.Key.ToString());
                }

                _spell.CastSpell();
            }
        }

        public override string GetXml()
        {
            string xml = "<Type>ActionSpell</Type>";
            xml += "<Name>" + _name + "</Name>";
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
            }
        }
    }
}