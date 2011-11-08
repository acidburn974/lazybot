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
using System.Windows.Forms;
using System.Xml;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using LazyLib.ActionBar;

#endregion

namespace LazyEvo.PVEBehavior.Behavior.Conditions
{
    public enum SpellConditionEnum
    {
        Ready = 1,
        NotReady = 2,
    }

    internal class SpellCondition : AbstractCondition
    {
        private TextBox _valueInput;

        public SpellCondition()
        {
            Condition = SpellConditionEnum.Ready;
        }

        private SpellConditionEnum Condition { get; set; }
        private string Value { get; set; }

        public override string Name
        {
            get { return "Spell Detection"; }
        }

        public override string XmlName
        {
            get { return "SpellCondition"; }
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
                switch (Condition)
                {
                    case SpellConditionEnum.Ready:
                        return BarMapper.IsSpellReadyByName(Value);
                    case SpellConditionEnum.NotReady:
                        return !BarMapper.IsSpellReadyByName(Value);
                }
                return false;
            }
        }

        public override List<Node> GetNodes()
        {
            var re = new List<Node>();
            CreateConditionCondition(re);
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
                "This condition will allow you to check i a spell is ready";
            labelX.Visible = true;
            labelX.BackColor = Color.Transparent;
            info.Nodes.Add(CreateControl("Info", "Info", labelX));
            info.Expanded = true;
            re.Add(info);
        }

        private void CreateConditionCondition(List<Node> re)
        {
            var conditionTarget = new Node();
            conditionTarget.Text = "Condition";
            conditionTarget.Nodes.Add(CreateRadioButton("Ready", "Ready", "SpellConditionEnum",
                                                        Condition.Equals(SpellConditionEnum.Ready)));
            conditionTarget.Nodes.Add(CreateRadioButton("NotReady", "Not ready", "SpellConditionEnum",
                                                        Condition.Equals(SpellConditionEnum.NotReady)));
            conditionTarget.Expanded = true;
            re.Add(conditionTarget);
        }

        private void CreateConditionValue(List<Node> re)
        {
            var conditionTarget = new Node();
            conditionTarget.Text = "Name";
            _valueInput = new TextBox();
            _valueInput.Text = Value;
            _valueInput.TextChanged += TB_ValueChanged;
            conditionTarget.Nodes.Add(CreateControl("Value", "Value", _valueInput));
            conditionTarget.Expanded = true;
            ;
            re.Add(conditionTarget);
        }

        private void TB_ValueChanged(object sender, EventArgs e)
        {
            Value = _valueInput.Text;
        }

        public override void NodeClick(Node node)
        {
            if (node != null)
            {
                if (node.Tag != null)
                {
                    if (node.Tag.Equals("SpellConditionEnum"))
                    {
                        Condition = (SpellConditionEnum) Enum.Parse(typeof (SpellConditionEnum), node.Name);
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
                if (node.Name.Equals("Condition"))
                {
                    Condition = (SpellConditionEnum) Enum.Parse(typeof (SpellConditionEnum), node.InnerText);
                }
                else if (node.Name.Equals("Value"))
                {
                    Value = node.InnerText;
                }
            }
        }
    }
}