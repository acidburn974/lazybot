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
    public enum PetConditionEnum
    {
        DoesHave = 1,
        DoesNotHave = 2,
    }

    internal class PetCondition : AbstractCondition
    {
        public PetCondition()
        {
            Condition = PetConditionEnum.DoesNotHave;
        }

        public PetCondition(PetConditionEnum petConditionEnum)
        {
            Condition = petConditionEnum;
        }

        private PetConditionEnum Condition { get; set; }

        public override string Name
        {
            get { return "Has Pet"; }
        }

        public override string XmlName
        {
            get { return "PetCondition"; }
        }

        public override string GetXML
        {
            get
            {
                string xml = "<Condition>" + Condition + "</Condition>";
                return xml;
            }
        }

        public override bool IsOk
        {
            get
            {
                if (Condition.Equals(PetConditionEnum.DoesHave))
                {
                    if (ObjectManager.MyPlayer.HasLivePet)
                        return true;
                    return false;
                }
                else
                {
                    if (!ObjectManager.MyPlayer.HasLivePet)
                        return true;
                    return false;
                }
            }
        }

        public override List<Node> GetNodes()
        {
            var re = new List<Node>();
            CreateCondition(re);
            return re;
        }

        private void CreateCondition(List<Node> re)
        {
            var conditionTarget = new Node();
            conditionTarget.Text = "Condition";
            conditionTarget.Nodes.Add(CreateRadioButton("DoesHave", "Has Pet", "PetConditionEnum",
                                                        Condition.Equals(PetConditionEnum.DoesHave)));
            conditionTarget.Nodes.Add(CreateRadioButton("DoesNotHave", "Does not have Pet", "PetConditionEnum",
                                                        Condition.Equals(PetConditionEnum.DoesNotHave)));
            conditionTarget.Expanded = true;
            re.Add(conditionTarget);
        }

        public override void NodeClick(Node node)
        {
            if (node != null)
            {
                if (node.Tag != null)
                {
                    if (node.Tag.Equals("PetConditionEnum"))
                    {
                        Condition = (PetConditionEnum) Enum.Parse(typeof (PetConditionEnum), node.Name);
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
                    Condition = (PetConditionEnum) Enum.Parse(typeof (PetConditionEnum), node.InnerText);
                }
            }
        }
    }
}