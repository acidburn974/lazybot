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
using System.Drawing;
using System.Xml;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using DevComponents.Editors;
using LazyLib.Wow;

#endregion

namespace LazyEvo.PVEBehavior.Behavior.Conditions
{
    internal class ComboPointsCondition : AbstractCondition
    {
        private IntegerInput valueInput;

        public ComboPointsCondition()
        {
            Condition = ConditionEnum.MoreThan;
            Value = 4;
        }

        private ConditionEnum Condition { get; set; }
        private int Value { get; set; }

        public override string Name
        {
            get { return "Combo Points"; }
        }

        public override string XmlName
        {
            get { return "ComboPointsCondition"; }
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
                    return ObjectManager.MyPlayer.ComboPoints == Value;
                if (Condition.Equals(ConditionEnum.LessThan))
                    return ObjectManager.MyPlayer.ComboPoints < Value;
                if (Condition.Equals(ConditionEnum.MoreThan))
                    return ObjectManager.MyPlayer.ComboPoints > Value;
                return false;
            }
        }

        public override List<Node> GetNodes()
        {
            var re = new List<Node>();
            CreateCondition(re);
            CreateValue(re);
            CreateText(re);
            return re;
        }

        private void CreateText(List<Node> re)
        {
            var info = new Node();
            info.Text = "Info";
            var labelX = new LabelX();
            labelX.AutoSize = true;
            labelX.MaximumSize = new Size(300, 500);
            labelX.Text = "This condition will allow you to check the number of combopoints on your current target.";
            labelX.Visible = true;
            labelX.BackColor = Color.Transparent;
            info.Nodes.Add(CreateControl("Info", "Info", labelX));
            info.Expanded = true;
            re.Add(info);
        }

        private void CreateCondition(List<Node> re)
        {
            var condition = new Node();
            condition.Text = "Combo Points ";
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
            value.Text = "value";
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