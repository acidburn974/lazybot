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
using System.Xml;
using DevComponents.AdvTree;
using LazyLib.Wow;

#endregion

namespace LazyEvo.PVEBehavior.Behavior.Conditions
{
    public enum TempEnchantConditionEnum
    {
        DoesHave = 1,
        DoesNotHave = 2,
    }

    public enum WeaponEnum
    {
        MainHand = 1,
        OffHand = 2,
    }

    internal class TempEnchantCondition : AbstractCondition
    {
        public TempEnchantCondition()
        {
            Weapon = WeaponEnum.MainHand;
            Condition = TempEnchantConditionEnum.DoesNotHave;
        }

        private TempEnchantConditionEnum Condition { get; set; }
        private WeaponEnum Weapon { get; set; }

        public override string Name
        {
            get { return "Has temporary enchant"; }
        }

        public override string XmlName
        {
            get { return "TempEnchantCondition"; }
        }

        public override string GetXML
        {
            get
            {
                string xml = "<Condition>" + Condition + "</Condition>";
                xml += "<Weapon>" + Weapon + "</Weapon>";
                return xml;
            }
        }

        public override bool IsOk
        {
            get
            {
                if (Weapon.Equals(WeaponEnum.MainHand))
                {
                    if (Condition.Equals(TempEnchantConditionEnum.DoesHave))
                        if (ObjectManager.MyPlayer.MainHandHasTempEnchant)
                            return true;
                    if (Condition.Equals(TempEnchantConditionEnum.DoesNotHave))
                        if (!ObjectManager.MyPlayer.MainHandHasTempEnchant)
                            return true;
                }
                else
                {
                    if (Condition.Equals(TempEnchantConditionEnum.DoesHave))
                        if (ObjectManager.MyPlayer.OffHandHasTempEnchant)
                            return true;
                    if (Condition.Equals(TempEnchantConditionEnum.DoesNotHave))
                        if (!ObjectManager.MyPlayer.OffHandHasTempEnchant)
                            return true;
                }
                return false;
            }
        }

        public override List<Node> GetNodes()
        {
            var re = new List<Node>();
            CreateCondition(re);
            CreateTarget(re);
            return re;
        }

        private void CreateTarget(List<Node> re)
        {
            var conditionTarget = new Node();
            conditionTarget.Text = "Weapon";
            conditionTarget.Nodes.Add(CreateRadioButton("MainHand", "Main Hand", "WeaponEnum",
                                                        Weapon.Equals(WeaponEnum.MainHand)));
            conditionTarget.Nodes.Add(CreateRadioButton("OffHand", "Off Hand", "WeaponEnum",
                                                        Weapon.Equals(WeaponEnum.OffHand)));
            conditionTarget.Expanded = true;
            re.Add(conditionTarget);
        }

        private void CreateCondition(List<Node> re)
        {
            var conditionTarget = new Node();
            conditionTarget.Text = "Condition";
            conditionTarget.Nodes.Add(CreateRadioButton("DoesHave", "Has temporary enchant", "TempEnchantConditionEnum",
                                                        Condition.Equals(TempEnchantConditionEnum.DoesHave)));
            conditionTarget.Nodes.Add(CreateRadioButton("DoesNotHave", "Does not have temporary enchant",
                                                        "TempEnchantConditionEnum",
                                                        Condition.Equals(TempEnchantConditionEnum.DoesNotHave)));
            conditionTarget.Expanded = true;
            re.Add(conditionTarget);
        }

        public override void NodeClick(Node node)
        {
            if (node != null)
            {
                if (node.Tag != null)
                {
                    if (node.Tag.Equals("TempEnchantConditionEnum"))
                    {
                        Condition = (TempEnchantConditionEnum) Enum.Parse(typeof (TempEnchantConditionEnum), node.Name);
                    }
                    if (node.Tag.Equals("WeaponEnum"))
                    {
                        Weapon = (WeaponEnum) Enum.Parse(typeof (WeaponEnum), node.Name);
                    }
                }
            }
        }

        public override void LoadData(XmlNode xmlNode)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.Name.Equals("Condition"))
                {
                    Condition = (TempEnchantConditionEnum) Enum.Parse(typeof (TempEnchantConditionEnum), node.InnerText);
                }
                else if (node.Name.Equals("Weapon"))
                {
                    Weapon = (WeaponEnum) Enum.Parse(typeof (WeaponEnum), node.InnerText);
                }
            }
        }
    }
}