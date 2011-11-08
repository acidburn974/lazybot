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
using DevComponents.Editors;
using LazyLib.Wow;

#endregion

namespace LazyEvo.PVEBehavior.Behavior.Conditions
{
    internal class HealthStoneCount : AbstractCondition
    {
        private IntegerInput valueInput;

        public HealthStoneCount()
        {
            Condition = ConditionEnum.LessThan;
            Value = 3;
        }

        private ConditionEnum Condition { get; set; }
        private int Value { get; set; }

        public override string Name
        {
            get { return "Healthstone Count"; }
        }

        public override string XmlName
        {
            get { return "HealthStoneCountCondition"; }
        }

        public override string GetXML
        {
            get
            {
                string xml = "<Condition>" + Condition + "</Condition>";
                xml += "<Value>" + Value + "</Value>";
                return xml;
            }
        }

        public override bool IsOk
        {
            get
            {
                if (Condition.Equals(ConditionEnum.EqualTo))
                    return ObjectManager.MyPlayer.HealthStoneCount == Value;
                if (Condition.Equals(ConditionEnum.LessThan))
                {
                    return ObjectManager.MyPlayer.HealthStoneCount < Value;
                }
                if (Condition.Equals(ConditionEnum.MoreThan))
                    return ObjectManager.MyPlayer.HealthStoneCount > Value;
                return false;
            }
        }

        public override List<Node> GetNodes()
        {
            var re = new List<Node>();
            CreateCondition(re);
            CreateValue(re);
            return re;
        }

        private void CreateCondition(List<Node> re)
        {
            var condition = new Node();
            condition.Text = "Healthstone count ";
            condition.Nodes.Add(CreateRadioButton("LessThan", "Less Than ", "ConditionEnum",
                                                  Condition.Equals(ConditionEnum.LessThan)));
            condition.Nodes.Add(CreateRadioButton("EqualTo", "Equal To", "ConditionEnum",
                                                  Condition.Equals(ConditionEnum.EqualTo)));
            condition.Nodes.Add(CreateRadioButton("MoreThan", "More Than", "ConditionEnum",
                                                  Condition.Equals(ConditionEnum.MoreThan)));
            condition.Expanded = true;
            re.Add(condition);
        }

        private void IntegerInput_ValueChanged(object sender, EventArgs e)
        {
            Value = valueInput.Value;
        }

        private void CreateValue(List<Node> re)
        {
            var value = new Node();
            value.Text = "Value";
            valueInput = new IntegerInput();
            valueInput.Value = Value;
            valueInput.ValueChanged += IntegerInput_ValueChanged;
            value.Nodes.Add(CreateControl("Value", "Value", valueInput));
            value.Expanded = true;
            re.Add(value);
        }

        public override void NodeClick(Node node)
        {
            if (node != null)
            {
                if (node.Tag != null)
                {
                    if (node.Tag.Equals("ConditionEnum"))
                    {
                        Condition = (ConditionEnum) Enum.Parse(typeof (ConditionEnum), node.Name);
                    }
                    if (node.Tag.Equals("Value"))
                    {
                        var integerInput = (IntegerInput) node.HostedControl;
                        Value = integerInput.Value;
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
                    Condition = (ConditionEnum) Enum.Parse(typeof (ConditionEnum), node.InnerText);
                }
                else if (node.Name.Equals("Value"))
                {
                    Value = Convert.ToInt32(node.InnerText);
                }
            }
        }
    }
}