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
using System.Globalization;
using System.Xml;
using LazyEvo.LGrindEngine.NpcClasses;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine
{
    internal class NpcController
    {
        public NpcController()
        {
            Npc = new List<VendorsEx>();
        }

        public List<VendorsEx> Npc { get; set; }

        public VendorsEx GetNearestRepair()
        {
            double closest = double.MaxValue;
            VendorsEx result = null;
            foreach (VendorsEx vendor in Npc)
            {
                if (vendor.VendorType == VendorType.Repair)
                {
                    if (vendor.Location.DistanceToSelf2D < closest)
                    {
                        result = vendor;
                        closest = vendor.Location.DistanceToSelf2D;
                    }
                }
            }
            return result;
        }

        public VendorsEx GetTrainer(Constants.UnitClass unitClass)
        {
            double closest = double.MaxValue;
            VendorsEx result = null;
            foreach (VendorsEx vendor in Npc)
            {
                if (vendor.VendorType == VendorType.Train)
                {
                    bool use = false;
                    switch (unitClass)
                    {
                        case Constants.UnitClass.UnitClass_Unknown:
                            if (vendor.TrainClass == TrainClass.Unknown)
                                use = true;
                            break;
                        case Constants.UnitClass.UnitClass_Warrior:
                            if (vendor.TrainClass == TrainClass.Warrior)
                                use = true;
                            break;
                        case Constants.UnitClass.UnitClass_Paladin:
                            if (vendor.TrainClass == TrainClass.Paladin)
                                use = true;
                            break;
                        case Constants.UnitClass.UnitClass_Hunter:
                            if (vendor.TrainClass == TrainClass.Hunter)
                                use = true;
                            break;
                        case Constants.UnitClass.UnitClass_Rogue:
                            if (vendor.TrainClass == TrainClass.Rogue)
                                use = true;
                            break;
                        case Constants.UnitClass.UnitClass_Priest:
                            if (vendor.TrainClass == TrainClass.Priest)
                                use = true;
                            break;
                        case Constants.UnitClass.UnitClass_DeathKnight:
                            if (vendor.TrainClass == TrainClass.DeathKnight)
                                use = true;
                            break;
                        case Constants.UnitClass.UnitClass_Shaman:
                            if (vendor.TrainClass == TrainClass.Shaman)
                                use = true;
                            break;
                        case Constants.UnitClass.UnitClass_Mage:
                            if (vendor.TrainClass == TrainClass.Mage)
                                use = true;
                            break;
                        case Constants.UnitClass.UnitClass_Warlock:
                            if (vendor.TrainClass == TrainClass.Warlock)
                                use = true;
                            break;
                        case Constants.UnitClass.UnitClass_Druid:
                            if (vendor.TrainClass == TrainClass.Druid)
                                use = true;
                            break;
                    }
                    if (use && vendor.Location.DistanceToSelf2D < closest)
                    {
                        result = vendor;
                        closest = vendor.Location.DistanceToSelf2D;
                    }
                }
            }
            return result;
        }

        public void LoadXml(XmlNode xmlNode)
        {
            if (xmlNode != null)
            {
                foreach (XmlNode childNode in xmlNode.ChildNodes)
                {
                    if (childNode.Name == "Vendor")
                    {
                        string name = string.Empty;
                        int entryId = int.MinValue;
                        VendorType type = VendorType.Unknown;
                        string x = string.Empty;
                        string y = string.Empty;
                        string z = string.Empty;
                        TrainClass trainClass = TrainClass.Unknown;
                        foreach (XmlAttribute attribute in childNode.Attributes)
                        {
                            switch (attribute.Name)
                            {
                                case "Name":
                                    name = attribute.InnerText;
                                    break;
                                case "EntryId":
                                    entryId = Convert.ToInt32(attribute.InnerText);
                                    break;
                                case "Type":
                                    type = (VendorType) Enum.Parse(typeof (VendorType), attribute.InnerText, true);
                                    break;
                                case "TrainClass":
                                    trainClass = (TrainClass) Enum.Parse(typeof (TrainClass), attribute.InnerText, true);
                                    break;
                                case "X":
                                    x = GetCorrectString(attribute.InnerText);
                                    break;
                                case "Y":
                                    y = GetCorrectString(attribute.InnerText);
                                    break;
                                case "Z":
                                    z = GetCorrectString(attribute.InnerText);
                                    break;
                            }
                        }
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (trainClass == TrainClass.Unknown)
                            {
                                Npc.Add(new VendorsEx(type, name,
                                                      new Location((float) Convert.ToDouble(x),
                                                                   (float) Convert.ToDouble(y),
                                                                   (float) Convert.ToDouble(z)), entryId));
                            }
                            else
                            {
                                Npc.Add(new VendorsEx(type, name,
                                                      new Location((float) Convert.ToDouble(x),
                                                                   (float) Convert.ToDouble(y),
                                                                   (float) Convert.ToDouble(z)), trainClass, entryId));
                            }
                        }
                    }
                }
            }
        }

        private static string GetCorrectString(string t)
        {
            if (Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) != ".")
            {
                return t.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            }
            return t;
        }

        public void AddNpc(VendorsEx npc)
        {
            Npc.Add(npc);
        }
    }
}