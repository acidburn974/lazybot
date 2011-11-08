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
using System.Globalization;
using System.Xml;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.NpcClasses
{
    internal class Trainer : Npc
    {
        public Trainer()
        {
        }

        public Trainer(string name, Location spot, string @class)
        {
            Name = name;
            Spot = spot;
            Class = @class;
        }

        public string Name { get; set; }
        public Location Spot { get; set; }
        public string Class { get; set; }

        public override void Load(XmlNode xml)
        {
            foreach (XmlNode childNode in xml.ChildNodes)
            {
                if (childNode.Name.Equals("Name"))
                    Name = childNode.InnerText;
                if (childNode.Name.Equals("Spot"))
                {
                    string temp = childNode.InnerText;
                    string correctString = temp;
                    if (Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) != ".")
                    {
                        correctString = temp.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    }
                    string xyz = correctString;
                    string[] split = xyz.Split(new[] {' '});
                    if (split.Length > 2)
                    {
                        Spot = new Location((float) Convert.ToDouble((split[0])), (float) Convert.ToDouble((split[1])),
                                            (float) Convert.ToDouble((split[2])));
                    }
                }
                if (childNode.Name.Equals("Class"))
                    Class = childNode.InnerText;
            }
        }

        public override string Save()
        {
            try
            {
                string xml = "<Trainer>";
                xml += "<Name>" + Name + "</Name>";
                string temp = Spot.X + " " + Spot.Y + " " + Spot.Z;
                string correctString =
                    Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) != "."
                        ? temp.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".")
                        : temp;
                xml += "<Spot>" + correctString + "</Spot>";
                xml += "<Class>" + Class + "</Class>";
                xml += "</Trainer>";
                return xml;
            }
            catch
            {
                return "";
            }
        }
    }
}