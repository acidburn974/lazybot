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
using LazyLib.Wow;

#endregion

namespace LazyEvo.PVEBehavior.Behavior.Conditions
{
    public enum FunctionsConditionEnum
    {
        Is = 1,
        Not = 2,
    }

    public enum FunctionEnum
    {
        InCombat = 1,
        Casting = 2,
        FacingAway = 3,
        Fleeing = 4,
        IsStunned = 5,
        IsPlayer = 6,
        IsPet = 7,
        IsAutoAttacking = 8,
    }

    internal class FunctionsCondition : AbstractCondition
    {
        public FunctionsCondition()
        {
            ConditionTarget = ConditionTargetEnum.Target;
            Condition = FunctionsConditionEnum.Is;
            Function = FunctionEnum.Casting;
        }

        public FunctionsCondition(ConditionTargetEnum target, FunctionsConditionEnum doing, FunctionEnum function)
        {
            ConditionTarget = target;
            Condition = doing;
            Function = function;
        }

        private FunctionsConditionEnum Condition { get; set; }
        private ConditionTargetEnum ConditionTarget { get; set; }
        private FunctionEnum Function { get; set; }

        public override string Name
        {
            get { return "Function"; }
        }

        public override string XmlName
        {
            get { return "FunctionCondition"; }
        }

        public override string GetXML
        {
            get
            {
                string xml = "<Condition>" + Condition + "</Condition>";
                xml += "<ConditionTarget>" + ConditionTarget + "</ConditionTarget>";
                xml += "<ConditionFunction>" + Function + "</ConditionFunction>";
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
                bool value;
                if (Condition.Equals(FunctionsConditionEnum.Is))
                    value = true;
                else
                    value = false;
                switch (Function)
                {
                    case FunctionEnum.Casting:
                        if (target.IsCasting == value)
                            return true;
                        return false;
                    case FunctionEnum.FacingAway:
                        if (target.IsFacingAway == value)
                            return true;
                        return false;
                    case FunctionEnum.InCombat:
                        if (target.IsInCombat == value)
                            return true;
                        return false;
                    case FunctionEnum.IsAutoAttacking:
                        if (target.IsAutoAttacking == value)
                            return true;
                        return false;
                    case FunctionEnum.IsPet:
                        if (target.IsPet == value)
                            return true;
                        return false;
                    case FunctionEnum.IsPlayer:
                        if (target.IsPlayer == value)
                            return true;
                        return false;
                    case FunctionEnum.Fleeing:
                        if (target.IsFleeing == value)
                            return true;
                        return false;
                    case FunctionEnum.IsStunned:
                        if (target.IsStunned == value)
                            return true;
                        return false;
                }
                return false;
            }
        }

        public override List<Node> GetNodes()
        {
            var re = new List<Node>();
            CreateConditionTarget(re);
            CreateCondition(re);
            CreateFunction(re);
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
            labelX.Text = "This condition will allow you to call a true/false function.";
            labelX.Visible = true;
            labelX.BackColor = Color.Transparent;
            info.Nodes.Add(CreateControl("Info", "Info", labelX));
            info.Expanded = true;
            re.Add(info);
        }


        private void CreateFunction(List<Node> re)
        {
            var conditionTarget = new Node();
            conditionTarget.Text = "Function";
            conditionTarget.Nodes.Add(CreateRadioButton("InCombat", "In Combat", "FunctionEnum",
                                                        Function.Equals(FunctionEnum.InCombat)));
            conditionTarget.Nodes.Add(CreateRadioButton("Casting", "FunctionEnum", Function.Equals(FunctionEnum.Casting)));
            conditionTarget.Nodes.Add(CreateRadioButton("FacingAway", "Facing Away", "FunctionEnum",
                                                        Function.Equals(FunctionEnum.FacingAway)));
            conditionTarget.Nodes.Add(CreateRadioButton("Fleeing", "Fleeing", "FunctionEnum",
                                                        Function.Equals(FunctionEnum.Fleeing)));
            conditionTarget.Nodes.Add(CreateRadioButton("IsStunned", "Stunned", "FunctionEnum",
                                                        Function.Equals(FunctionEnum.IsStunned)));
            conditionTarget.Nodes.Add(CreateRadioButton("IsPlayer", "Player", "FunctionEnum",
                                                        Function.Equals(FunctionEnum.IsPlayer)));
            conditionTarget.Nodes.Add(CreateRadioButton("IsPet", "Pet", "FunctionEnum",
                                                        Function.Equals(FunctionEnum.IsPet)));
            conditionTarget.Nodes.Add(CreateRadioButton("IsAutoAttacking", "Auto Attacking", "FunctionEnum",
                                                        Function.Equals(FunctionEnum.IsAutoAttacking)));
            conditionTarget.Expanded = true;
            re.Add(conditionTarget);
        }

        private void CreateCondition(List<Node> re)
        {
            var conditionTarget = new Node();
            conditionTarget.Text = "Condition";
            conditionTarget.Nodes.Add(CreateRadioButton("Is", "FunctionsConditionEnum",
                                                        Condition.Equals(FunctionsConditionEnum.Is)));
            conditionTarget.Nodes.Add(CreateRadioButton("Not", "FunctionsConditionEnum",
                                                        Condition.Equals(FunctionsConditionEnum.Not)));
            conditionTarget.Expanded = true;
            re.Add(conditionTarget);
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
                    if (node.Tag.Equals("FunctionsConditionEnum"))
                    {
                        Condition = (FunctionsConditionEnum) Enum.Parse(typeof (FunctionsConditionEnum), node.Name);
                    }
                    if (node.Tag.Equals("FunctionEnum"))
                    {
                        Function = (FunctionEnum) Enum.Parse(typeof (FunctionEnum), node.Name);
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
                if (node.Name.Equals("Condition"))
                {
                    Condition = (FunctionsConditionEnum) Enum.Parse(typeof (FunctionsConditionEnum), node.InnerText);
                }
                if (node.Name.Equals("ConditionFunction"))
                {
                    Function = (FunctionEnum) Enum.Parse(typeof (FunctionEnum), node.InnerText);
                }
            }
        }
    }
}