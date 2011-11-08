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
    public enum RuneEnum
    {
        Blood,
        Frost,
        Unholy
    }

    internal class RuneCondition : AbstractCondition
    {
        private RuneEnum Rune;
        private IntegerInput valueInput;

        public RuneCondition()
        {
            Condition = ConditionEnum.MoreThan;
            Value = 1;
            Rune = RuneEnum.Blood;
        }

        public RuneCondition(ConditionEnum conditionEnum, RuneEnum runeEnum, int value)
        {
            Condition = conditionEnum;
            Value = value;
            Rune = runeEnum;
        }

        private ConditionEnum Condition { get; set; }
        private int Value { get; set; }

        public override string Name
        {
            get { return "Rune"; }
        }

        public override string XmlName
        {
            get { return "RuneCondition"; }
        }

        public override string GetXML
        {
            get
            {
                string xml = "<Condition>" + Condition + "</Condition>";
                xml += "<Value>" + Value + "</Value>";
                xml += "<Rune>" + Rune + "</Rune>";
                return xml;
            }
        }

        public override bool IsOk
        {
            get
            {
                int count = 0;
                if (Rune.Equals(RuneEnum.Blood))
                {
                    if (ObjectManager.MyPlayer.BloodRune1Ready)
                        count++;
                    if (ObjectManager.MyPlayer.BloodRune2Ready)
                        count++;
                }
                if (Rune.Equals(RuneEnum.Frost))
                {
                    if (ObjectManager.MyPlayer.FrostRune1Ready)
                        count++;
                    if (ObjectManager.MyPlayer.FrostRune2Ready)
                        count++;
                }
                if (Rune.Equals(RuneEnum.Unholy))
                {
                    if (ObjectManager.MyPlayer.UnholyRune1Ready)
                        count++;
                    if (ObjectManager.MyPlayer.UnholyRune2Ready)
                        count++;
                }
                if (Condition.Equals(ConditionEnum.EqualTo))
                    return count == Value;
                if (Condition.Equals(ConditionEnum.LessThan))
                    return count < Value;
                if (Condition.Equals(ConditionEnum.MoreThan))
                    return count > Value;
                return false;
            }
        }

        public override List<Node> GetNodes()
        {
            var re = new List<Node>();
            CreateCondition(re);
            CreateValue(re);
            CreateRuneCondition(re);
            return re;
        }

        private void CreateRuneCondition(List<Node> re)
        {
            var condition = new Node();
            condition.Text = "Rune(s) ";
            condition.Nodes.Add(CreateRadioButton("Blood", "Blood ", "RuneEnum", Rune.Equals(RuneEnum.Blood)));
            condition.Nodes.Add(CreateRadioButton("Frost", "Frost", "RuneEnum", Rune.Equals(RuneEnum.Frost)));
            condition.Nodes.Add(CreateRadioButton("Unholy", "Unholy", "RuneEnum", Rune.Equals(RuneEnum.Unholy)));
            condition.Expanded = true;
            re.Add(condition);
        }

        private void CreateCondition(List<Node> re)
        {
            var condition = new Node();
            condition.Text = "Player has ";
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
                    if (node.Tag.Equals("RuneEnum"))
                    {
                        Rune = (RuneEnum) Enum.Parse(typeof (RuneEnum), node.Name);
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
                if (node.Name.Equals("Value"))
                {
                    Value = Convert.ToInt32(node.InnerText);
                }
                if (node.Name.Equals("Rune"))
                {
                    Rune = (RuneEnum) Enum.Parse(typeof (RuneEnum), node.InnerText);
                }
            }
        }
    }
}