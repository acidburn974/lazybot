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
using System.Linq;
using System.Xml;
using LazyEvo.PVEBehavior.Behavior;
using LazyEvo.PVEBehavior.Behavior.Conditions;
using LazyLib;

namespace LazyEvo.Plugins.RotationPlugin
{
    public class Rotation
    {
        internal bool Active;
        internal bool Alt;
        internal bool Ctrl;
        internal int GlobalCooldown;
        internal string Key;
        internal string Name;
        internal RuleController Rules;
        internal bool Shift;
        internal bool Windows;

        public Rotation()
        {
            Rules = new RuleController();
            GlobalCooldown = 1600;
            Key = string.Empty;
        }

        public void ResetControllers()
        {
            Rules = new RuleController();
        }

        public void Load(XmlNode rotation)
        {
            Rules = new RuleController();
            GlobalCooldown = 1600;
            try
            {
                foreach (XmlNode childNode in rotation.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "Name":
                            Name = childNode.InnerText;
                            break;
                        case "GlobalCooldown":
                            GlobalCooldown = Convert.ToInt32(childNode.InnerText);
                            break;
                        case "Active":
                            Active = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "Ctrl":
                            Ctrl = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "Alt":
                            Alt = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "Shift":
                            Shift = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "Key":
                            Key = childNode.InnerText;
                            break;
                        case "Windows":
                            Windows = Convert.ToBoolean(childNode.InnerText);
                            break;
                        case "Rules":
                            LoadController(childNode.ChildNodes, Rules);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.Write("Something went wrong when loading Rotation: " + e);
            }
        }

        internal void LoadController(XmlNodeList xmlNodeList, RuleController ruleController)
        {
            foreach (XmlNode xmlMainNode in xmlNodeList)
            {
                if (xmlMainNode.Name.Equals("Rule"))
                {
                    //We got our hands on a rule, lets load it.
                    var rule = new Rule();
                    foreach (XmlNode childNode in xmlMainNode)
                    {
                        switch (childNode.Name)
                        {
                            case "Name":
                                rule.Name = childNode.InnerText;
                                break;
                            case "Script":
                                rule.Script = childNode.InnerText;
                                break;
                            case "MatchAll":
                                rule.MatchAll = Convert.ToBoolean(childNode.InnerText);
                                break;
                            case "ShouldTarget":
                                rule.ShouldTarget =
                                    (Target) Enum.Parse(typeof (Target), childNode.InnerText);
                                break;
                            case "Priority":
                                rule.Priority = Convert.ToInt32(childNode.InnerText);
                                break;
                            case "Action":
                                rule.LoadAction(childNode);
                                break;
                            default:
                                AbstractCondition condition = LoadConditions(childNode);
                                if (condition != null)
                                    rule.AddCondition(condition);
                                break;
                        }
                    }
                    ruleController.AddRule(rule);
                }
            }
        }

        internal AbstractCondition LoadConditions(XmlNode xmlNode)
        {
            AbstractCondition condition;
            switch (xmlNode.Name)
            {
                case "HealthPowerCondition":
                    condition = new HealthPowerCondition();
                    condition.LoadData(xmlNode);
                    return condition;
                case "BuffCondition":
                    condition = new BuffCondition();
                    condition.LoadData(xmlNode);
                    return condition;
                case "CombatCountCondition":
                    condition = new CombatCountCondition();
                    condition.LoadData(xmlNode);
                    return condition;
                case "DistanceToTargetCondition":
                    condition = new DistanceToTarget();
                    condition.LoadData(xmlNode);
                    return condition;
                case "SoulShardCountCondition":
                    condition = new SoulShardCountCondition();
                    condition.LoadData(xmlNode);
                    return condition;
                case "HealthStoneCountCondition":
                    condition = new HealthStoneCount();
                    condition.LoadData(xmlNode);
                    return condition;
                case "ComboPointsCondition":
                    condition = new ComboPointsCondition();
                    condition.LoadData(xmlNode);
                    return condition;
                case "MageWaterCondition":
                    condition = new MageWaterCondition();
                    condition.LoadData(xmlNode);
                    return condition;
                case "MageFoodCondition":
                    condition = new MageFoodCondition();
                    condition.LoadData(xmlNode);
                    return condition;
                case "TempEnchantCondition":
                    condition = new TempEnchantCondition();
                    condition.LoadData(xmlNode);
                    return condition;
                case "RuneCondition":
                    condition = new RuneCondition();
                    condition.LoadData(xmlNode);
                    return condition;
                case "PotentialAddsCondition":
                    condition = new PotentialAddsCondition();
                    condition.LoadData(xmlNode);
                    return condition;
                case "FunctionCondition":
                    condition = new FunctionsCondition();
                    condition.LoadData(xmlNode);
                    return condition;
                case "TickerCondition":
                    condition = new TickerCondition();
                    condition.LoadData(xmlNode);
                    return condition;
                case "PetCondition":
                    condition = new PetCondition();
                    condition.LoadData(xmlNode);
                    return condition;
                case "SpellCondition":
                    condition = new SpellCondition();
                    condition.LoadData(xmlNode);
                    return condition;
            }
            return null;
        }

        private string SaveRule(Rule rule)
        {
            string xml = "<Rule>";
            xml += "<Name>" + rule.Name + "</Name>";
            xml += "<Script><![CDATA[" + rule.Script + "]]></Script>";
            xml += "<MatchAll>" + rule.MatchAll + "</MatchAll>";
            xml += "<ShouldTarget>" + rule.ShouldTarget + "</ShouldTarget>";
            xml += "<Priority>" + rule.Priority + "</Priority>";
            xml += rule.SaveAction();
            foreach (AbstractCondition condition in rule.GetConditions)
            {
                xml += "<" + condition.XmlName + ">";
                xml += condition.GetXML;
                xml += "</" + condition.XmlName + ">";
            }
            xml += "</Rule>";
            return xml;
        }

        internal string Save()
        {
            string xml = string.Empty;
            xml += "<Rotation>";
            xml += "<Name>" + Name + "</Name>";
            xml += "<Active>" + Active + "</Active>";
            xml += "<Ctrl>" + Ctrl + "</Ctrl>";
            xml += "<Alt>" + Alt + "</Alt>";
            xml += "<Shift>" + Shift + "</Shift>";
            xml += "<Windows>" + Windows + "</Windows>";
            xml += "<Key>" + Key + "</Key>";
            xml += "<GlobalCooldown>" + GlobalCooldown + "</GlobalCooldown>";
            if (Rules != null)
            {
                xml += "<Rules>";
                xml = Rules.GetRules.Aggregate(xml, (current, rule) => current + SaveRule(rule));
                xml += "</Rules>";
            }
            xml += "</Rotation>";
            return xml;
        }
    }
}