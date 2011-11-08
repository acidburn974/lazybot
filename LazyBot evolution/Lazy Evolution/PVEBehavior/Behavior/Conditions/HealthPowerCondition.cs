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
    public enum ConditionTypeEnum
    {
        Health,
        Mana,
        Energy,
        Rage,
        RunicPower,
        Happiness,
        Eclipse,
        Focus,
        HolyPower,
    }

    internal class HealthPowerCondition : AbstractCondition
    {
        private IntegerInput valueInput;

        public HealthPowerCondition()
        {
            ConditionTarget = ConditionTargetEnum.Player;
            ConditionType = ConditionTypeEnum.Health;
            Condition = ConditionEnum.EqualTo;
            Value = 50;
        }

        public HealthPowerCondition(ConditionTargetEnum conditionTarget, ConditionTypeEnum conditionType,
                                    ConditionEnum condition, int value)
        {
            ConditionTarget = conditionTarget;
            ConditionType = conditionType;
            Condition = condition;
            Value = value;
        }

        public override string Name
        {
            get { return "Health/Power"; }
        }

        public override string XmlName
        {
            get { return "HealthPowerCondition"; }
        }

        public override string GetXML
        {
            get
            {
                string xml = "<ConditionTarget>" + ConditionTarget + "</ConditionTarget>";
                xml += "<ConditionType>" + ConditionType + "</ConditionType>";
                xml += "<Condition>" + Condition + "</Condition>";
                xml += "<Value>" + Value + "</Value>";
                return xml;
            }
        }

        public override bool IsOk
        {
            get
            {
                //Logging.Write("     Evaluating HealthPowerCondition: ConditionTarget: " + ConditionTarget + " ConditionType:" + ConditionType + " Condition: " +Condition + " Value: " + Value);
                PUnit target = null;
                switch (ConditionTarget)
                {
                    case ConditionTargetEnum.Player:
                        target = ObjectManager.MyPlayer;
                        break;
                    case ConditionTargetEnum.Pet:
                        target = ObjectManager.MyPlayer.Pet;
                        break;
                    case ConditionTargetEnum.Target:
                        target = ObjectManager.MyPlayer.Target;
                        break;
                }
                if (target == null)
                {
                    //Logging.Write("     Evaluating HealthPowerCondition: false");
                    return false;
                }
                int value = int.MinValue;
                switch (ConditionType)
                {
                    case ConditionTypeEnum.Energy:
                        value = target.Energy;
                        break;
                    case ConditionTypeEnum.Health:
                        value = target.Health;
                        break;
                    case ConditionTypeEnum.Mana:
                        value = target.Mana;
                        break;
                    case ConditionTypeEnum.Rage:
                        value = target.Rage;
                        break;
                    case ConditionTypeEnum.RunicPower:
                        value = target.RunicPower;
                        break;
                    case ConditionTypeEnum.Eclipse:
                        value = target.Eclipse;
                        break;
                    case ConditionTypeEnum.HolyPower:
                        value = target.HolyPower;
                        break;
                    case ConditionTypeEnum.Focus:
                        value = target.Focus;
                        break;
                }
                if (value == int.MinValue)
                {
                    //Logging.Write("     Evaluating HealthPowerCondition: false");
                    return false;
                }
                switch (Condition)
                {
                    case ConditionEnum.EqualTo:
                        if (value == Value)
                            return true;
                        return false;
                    case ConditionEnum.LessThan:
                        if (value < Value)
                            return true;
                        return false;
                    case ConditionEnum.MoreThan:
                        if (value > Value)
                            return true;
                        return false;
                }
                //Logging.Write("     Evaluating HealthPowerCondition: true");
                return true;
            }
        }

        private ConditionTargetEnum ConditionTarget { get; set; }
        private ConditionTypeEnum ConditionType { get; set; }
        private ConditionEnum Condition { get; set; }
        private int Value { get; set; }

        public override void LoadData(XmlNode xmlNode)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.Name.Equals("ConditionTarget"))
                {
                    ConditionTarget = (ConditionTargetEnum) Enum.Parse(typeof (ConditionTargetEnum), node.InnerText);
                }
                else if (node.Name.Equals("ConditionType"))
                {
                    ConditionType = (ConditionTypeEnum) Enum.Parse(typeof (ConditionTypeEnum), node.InnerText);
                }
                else if (node.Name.Equals("Condition"))
                {
                    Condition = (ConditionEnum) Enum.Parse(typeof (ConditionEnum), node.InnerText);
                }
                else if (node.Name.Equals("Value"))
                {
                    Value = Convert.ToInt32(node.InnerText);
                }
            }
        }

        private void IntegerInput_ValueChanged(object sender, EventArgs e)
        {
            Value = valueInput.Value;
        }

        public override List<Node> GetNodes()
        {
            var re = new List<Node>();
            CreateConditionTarget(re);
            CreateConditionType(re);
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
            labelX.Text = "This condition will allow you to check Health and Power values. <br/> ";
            labelX.Visible = true;
            labelX.BackColor = Color.Transparent;
            info.Nodes.Add(CreateControl("Info", "Info", labelX));
            info.Expanded = true;
            re.Add(info);
        }

        private void CreateValue(List<Node> re)
        {
            var value = new Node();
            value.Text = "Value in %";
            valueInput = new IntegerInput();
            valueInput.Value = Value;
            valueInput.ValueChanged += IntegerInput_ValueChanged;
            value.Nodes.Add(CreateControl("Value", "Value", valueInput));
            value.Expanded = true;
            re.Add(value);
        }

        private void CreateCondition(List<Node> re)
        {
            var condition = new Node();
            condition.Text = "Condition";
            condition.Nodes.Add(CreateRadioButton("LessThan", "Less Than ", "ConditionEnum",
                                                  Condition.Equals(ConditionEnum.LessThan)));
            condition.Nodes.Add(CreateRadioButton("EqualTo", "Equal To", "ConditionEnum",
                                                  Condition.Equals(ConditionEnum.EqualTo)));
            condition.Nodes.Add(CreateRadioButton("MoreThan", "More Than", "ConditionEnum",
                                                  Condition.Equals(ConditionEnum.MoreThan)));
            condition.Expanded = true;
            re.Add(condition);
        }

        private void CreateConditionType(List<Node> re)
        {
            var conditionType = new Node();
            conditionType.Text = "Type";
            conditionType.Nodes.Add(CreateRadioButton("Health", "ConditionTypeEnum",
                                                      ConditionType.Equals(ConditionTypeEnum.Health)));
            conditionType.Nodes.Add(CreateRadioButton("Mana", "ConditionTypeEnum",
                                                      ConditionType.Equals(ConditionTypeEnum.Mana)));
            conditionType.Nodes.Add(CreateRadioButton("Energy", "ConditionTypeEnum",
                                                      ConditionType.Equals(ConditionTypeEnum.Energy)));
            conditionType.Nodes.Add(CreateRadioButton("Rage", "ConditionTypeEnum",
                                                      ConditionType.Equals(ConditionTypeEnum.Rage)));
            conditionType.Nodes.Add(CreateRadioButton("RunicPower", "Runic Power", "ConditionTypeEnum",
                                                      ConditionType.Equals(ConditionTypeEnum.RunicPower)));
            conditionType.Nodes.Add(CreateRadioButton("Eclipse", "ConditionTypeEnum",
                                                      ConditionType.Equals(ConditionTypeEnum.Eclipse)));
            conditionType.Nodes.Add(CreateRadioButton("Focus", "ConditionTypeEnum",
                                                      ConditionType.Equals(ConditionTypeEnum.Focus)));
            conditionType.Nodes.Add(CreateRadioButton("HolyPower", "Holy Power", "ConditionTypeEnum",
                                                      ConditionType.Equals(ConditionTypeEnum.HolyPower)));
            conditionType.Expanded = true;
            re.Add(conditionType);
        }

        private void CreateConditionTarget(List<Node> re)
        {
            var conditionTarget = new Node();
            conditionTarget.Text = "Check";
            conditionTarget.Nodes.Add(CreateRadioButton("Player", "ConditionTargetEnum",
                                                        ConditionTarget.Equals(ConditionTargetEnum.Player)));
            conditionTarget.Nodes.Add(CreateRadioButton("Pet", "ConditionTargetEnum",
                                                        ConditionTarget.Equals(ConditionTargetEnum.Pet)));
            conditionTarget.Nodes.Add(CreateRadioButton("Target", "ConditionTargetEnum",
                                                        ConditionTarget.Equals(ConditionTargetEnum.Target)));
            conditionTarget.Expanded = true;
            re.Add(conditionTarget);
        }

        public override void NodeClick(Node node)
        {
            if (node != null)
            {
                if (node.Tag != null)
                {
                    if (node.Tag.Equals("ConditionTargetEnum"))
                    {
                        ConditionTarget = (ConditionTargetEnum) Enum.Parse(typeof (ConditionTargetEnum), node.Name);
                    }
                    if (node.Tag.Equals("ConditionTypeEnum"))
                    {
                        ConditionType = (ConditionTypeEnum) Enum.Parse(typeof (ConditionTypeEnum), node.Name);
                    }
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
    }
}