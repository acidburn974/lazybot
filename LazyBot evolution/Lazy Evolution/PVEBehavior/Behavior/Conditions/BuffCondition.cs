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
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using LazyLib;
using LazyLib.Wow;

#endregion

namespace LazyEvo.PVEBehavior.Behavior.Conditions
{
    public enum BuffConditionEnum
    {
        HasBuff = 1,
        DoesNotHave = 2,
    }

    public enum BuffOwnerConditionEnum
    {
        Owner = 1,
        NotOwner = 2,
        DoesNotMatter = 3,
    }

    public enum BuffValueEnum
    {
        Name = 1,
        Id = 2,
    }

    internal class BuffCondition : AbstractCondition
    {
        private TextBox valueInput;

        public BuffCondition()
        {
            ConditionTarget = ConditionTargetEnum.Player;
            Condition = BuffConditionEnum.HasBuff;
            ValueType = BuffValueEnum.Id;
            OwnerCondition = BuffOwnerConditionEnum.DoesNotMatter;
        }

        public BuffCondition(ConditionTargetEnum conditionTargetEnum, BuffConditionEnum buffConditionEnum,
                             BuffValueEnum id, string value)
        {
            ConditionTarget = conditionTargetEnum;
            Condition = buffConditionEnum;
            ValueType = id;
            Value = value;
            OwnerCondition = BuffOwnerConditionEnum.DoesNotMatter;
        }

        private ConditionTargetEnum ConditionTarget { get; set; }
        private BuffOwnerConditionEnum OwnerCondition { get; set; }
        private BuffConditionEnum Condition { get; set; }
        private BuffValueEnum ValueType { get; set; }
        private string Value { get; set; }

        public override string Name
        {
            get { return "Buff detection"; }
        }

        public override string XmlName
        {
            get { return "BuffCondition"; }
        }

        public override string GetXML
        {
            get
            {
                string xml = "<ConditionTarget>" + ConditionTarget + "</ConditionTarget>";
                xml += "<Condition>" + Condition + "</Condition>";
                xml += "<OwnerCondition>" + OwnerCondition + "</OwnerCondition>";
                xml += "<ValueType>" + ValueType + "</ValueType>";
                xml += "<Value>" + Value + "</Value>";
                return xml;
            }
        }

        public override bool IsOk
        {
            get
            {
                PUnit target = null;
                //Logging.Write("      Evaluating BuffCondition");
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
                    //Logging.Write("     Evaluating BuffCondition: false");
                    return false;
                }
                switch (Condition)
                {
                    case BuffConditionEnum.DoesNotHave:
                        if (ValueType.Equals(BuffValueEnum.Name))
                        {
                            if (HasBuffByName(target, Value))
                                return false;
                        }
                        else
                        {
                            if (HasBuffId(target, Value))
                                return false;
                        }
                        break;
                    case BuffConditionEnum.HasBuff:
                        if (ValueType.Equals(BuffValueEnum.Name))
                        {
                            if (!HasBuffByName(target, Value))
                                return false;
                        }
                        else
                        {
                            if (!HasBuffId(target, Value))
                                return false;
                        }
                        break;
                }
                // Logging.Write("     Evaluating BuffCondition: true");
                return true;
            }
        }

        public override List<Node> GetNodes()
        {
            var re = new List<Node>();
            CreateConditionTarget(re);
            CreateConditionCondition(re);
            CreateConditionOwnerCondition(re);
            CreateConditionValue(re);
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
            labelX.Text =
                "This condition will allow you to check for buffs on a specific unit/player. <br/><br/>You can get the id's from wowhead.com by searching for the name or use the 'Log own buffs' button in the debug tab<br/><br/> You can enter multiple id's separating each id using ',' (Do not use spaces). <br/><br/> If you are using the name function the bot will warn you if the name is invalid";
            labelX.Visible = true;
            labelX.BackColor = Color.Transparent;
            info.Nodes.Add(CreateControl("Info", "Info", labelX));
            info.Expanded = true;
            re.Add(info);
        }

        private void CreateConditionTarget(List<Node> re)
        {
            var conditionTarget = new Node();
            conditionTarget.Text = "Check if";
            conditionTarget.Nodes.Add(CreateRadioButton("Player", "ConditionTargetEnum",
                                                        ConditionTarget.Equals(ConditionTargetEnum.Player)));
            conditionTarget.Nodes.Add(CreateRadioButton("Pet", "ConditionTargetEnum",
                                                        ConditionTarget.Equals(ConditionTargetEnum.Pet)));
            conditionTarget.Nodes.Add(CreateRadioButton("Target", "ConditionTargetEnum",
                                                        ConditionTarget.Equals(ConditionTargetEnum.Target)));
            conditionTarget.Expanded = true;
            re.Add(conditionTarget);
        }

        private void CreateConditionOwnerCondition(List<Node> re)
        {
            var conditionTarget = new Node();
            conditionTarget.Text = "Owner";
            conditionTarget.Nodes.Add(CreateRadioButton("Owner", "I am owner", "BuffConditionOwnerEnum",
                                                        OwnerCondition.Equals(BuffOwnerConditionEnum.Owner)));
            conditionTarget.Nodes.Add(CreateRadioButton("NotOwner", "Other owner", "BuffConditionOwnerEnum",
                                                        OwnerCondition.Equals(BuffOwnerConditionEnum.NotOwner)));
            conditionTarget.Nodes.Add(CreateRadioButton("DoesNotMatter", "Does not matter", "BuffConditionOwnerEnum",
                                                        OwnerCondition.Equals(BuffOwnerConditionEnum.DoesNotMatter)));
            conditionTarget.Expanded = true;
            re.Add(conditionTarget);
        }

        private void CreateConditionCondition(List<Node> re)
        {
            var conditionTarget = new Node();
            conditionTarget.Text = "Condition";
            conditionTarget.Nodes.Add(CreateRadioButton("HasBuff", "Has Buff", "BuffConditionEnum",
                                                        Condition.Equals(BuffConditionEnum.HasBuff)));
            conditionTarget.Nodes.Add(CreateRadioButton("DoesNotHave", "Does Not Have Buff", "BuffConditionEnum",
                                                        Condition.Equals(BuffConditionEnum.DoesNotHave)));
            conditionTarget.Expanded = true;
            re.Add(conditionTarget);
        }

        private void CreateConditionValue(List<Node> re)
        {
            var conditionTarget = new Node();
            conditionTarget.Text = "Value";
            conditionTarget.Nodes.Add(CreateRadioButton("Id", "By Id", "BuffValueEnum",
                                                        ValueType.Equals(BuffValueEnum.Id)));
            conditionTarget.Nodes.Add(CreateRadioButton("Name", "By Name", "BuffValueEnum",
                                                        ValueType.Equals(BuffValueEnum.Name)));
            conditionTarget.Expanded = true;
            valueInput = new TextBox();
            valueInput.Text = Value;
            valueInput.TextChanged += TB_ValueChanged;
            conditionTarget.Nodes.Add(CreateControl("Value", "Value", valueInput));
            conditionTarget.Expanded = true;
            re.Add(conditionTarget);
        }

        private void TB_ValueChanged(object sender, EventArgs e)
        {
            Value = valueInput.Text;
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
                    if (node.Tag.Equals("BuffConditionOwnerEnum"))
                    {
                        OwnerCondition = (BuffOwnerConditionEnum) Enum.Parse(typeof (BuffOwnerConditionEnum), node.Name);
                    }
                    if (node.Tag.Equals("BuffConditionEnum"))
                    {
                        Condition = (BuffConditionEnum) Enum.Parse(typeof (BuffConditionEnum), node.Name);
                    }
                    if (node.Tag.Equals("BuffValueEnum"))
                    {
                        ValueType = (BuffValueEnum) Enum.Parse(typeof (BuffValueEnum), node.Name);
                    }
                    if (node.Tag.Equals("Value"))
                    {
                        var textBox = (TextBox) node.HostedControl;
                        Value = textBox.Text;
                    }
                }
            }
        }

        public override void LoadData(XmlNode xmlNode)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.Name.Equals("ConditionTarget"))
                {
                    ConditionTarget = (ConditionTargetEnum) Enum.Parse(typeof (ConditionTargetEnum), node.InnerText);
                }
                else if (node.Name.Equals("OwnerCondition"))
                {
                    OwnerCondition =
                        (BuffOwnerConditionEnum) Enum.Parse(typeof (BuffOwnerConditionEnum), node.InnerText);
                }
                else if (node.Name.Equals("ValueType"))
                {
                    ValueType = (BuffValueEnum) Enum.Parse(typeof (BuffValueEnum), node.InnerText);
                }
                else if (node.Name.Equals("Condition"))
                {
                    Condition = (BuffConditionEnum) Enum.Parse(typeof (BuffConditionEnum), node.InnerText);
                }
                else if (node.Name.Equals("Value"))
                {
                    Value = node.InnerText;
                }
            }
        }

        public string GetBuffName()
        {
            if (ValueType.Equals(BuffValueEnum.Name))
            {
                return Value;
            }
            return string.Empty;
        }

        private bool HasBuffId(PUnit target, string buffIds)
        {
            string[] buffId = buffIds.Split(',');
            foreach (string s in buffId)
            {
                try
                {
                    switch (OwnerCondition)
                    {
                        case BuffOwnerConditionEnum.Owner:
                            if (target.HasBuff(Convert.ToInt32(s), true))
                                return true;
                            break;
                        case BuffOwnerConditionEnum.NotOwner:
                            if (target.HasBuff(Convert.ToInt32(s), false))
                                return true;
                            break;
                        case BuffOwnerConditionEnum.DoesNotMatter:
                            if (target.HasBuff(Convert.ToInt32(s), false))
                                return true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (ThreadAbortException)
                {
                }
                catch (Exception e)
                {
                    Logging.Write("Error checking buff: " + e);
                }
            }
            return false;
        }

        private bool HasBuffByName(PUnit target, string buffIds)
        {
            string[] buffId = buffIds.Split(',');
            foreach (string s in buffId)
            {
                try
                {
                    switch (OwnerCondition)
                    {
                        case BuffOwnerConditionEnum.Owner:
                            if (target.HasBuff(s, true))
                                return true;
                            break;
                        case BuffOwnerConditionEnum.NotOwner:
                            if (target.HasBuff(s, false))
                                return true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception)
                {
                    //Logging.Write("Could not convert " + s + " to int when checking buffs - fix your BuffCondition: ");
                }
            }
            return false;
        }
    }
}