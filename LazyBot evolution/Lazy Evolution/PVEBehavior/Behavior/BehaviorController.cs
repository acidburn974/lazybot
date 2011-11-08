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
using System.IO;
using System.Windows.Forms;
using System.Xml;
using LazyEvo.PVEBehavior.Behavior.Conditions;
using LazyLib;

#endregion

namespace LazyEvo.PVEBehavior.Behavior
{
    internal class BehaviorController
    {
        private XmlDocument _doc;
        internal RuleController CombatController { get; set; }
        internal RuleController PullController { get; set; }
        internal RuleController BuffController { get; set; }
        internal RuleController RestController { get; set; }
        internal RuleController PrePullController { get; set; }
        internal string Name { get; set; }
        internal int CombatDistance { get; set; }
        internal int PullDistance { get; set; }
        internal int PrePullDistance { get; set; }
        internal bool UseAutoAttack { get; set; }
        internal bool SendPet { get; set; }
        internal int GlobalCooldown { get; set; }

        public void ResetControllers()
        {
            CombatController = new RuleController();
            PullController = new RuleController();
            BuffController = new RuleController();
            RestController = new RuleController();
            PrePullController = new RuleController();
        }

        public void Load(string fileToLoad)
        {
            CombatController = new RuleController();
            PullController = new RuleController();
            BuffController = new RuleController();
            RestController = new RuleController();
            PrePullController = new RuleController();
            GlobalCooldown = 1600;
            try
            {
                Name = Path.GetFileNameWithoutExtension(fileToLoad);
                _doc = new XmlDocument();
                _doc.Load(fileToLoad);
            }
            catch (Exception e)
            {
                Logging.Write("Error in loaded behavior: " + e);
                return;
            }
            try
            {
                try
                {
                    CombatDistance = Convert.ToInt32(_doc.GetElementsByTagName("CombatDistance")[0].InnerText);
                    try
                    {
                        PullDistance = Convert.ToInt32(_doc.GetElementsByTagName("PullDistance")[0].InnerText);
                    }
                    catch
                    {
                        PullDistance = 25;
                    }
                    try
                    {
                        PrePullDistance = Convert.ToInt32(_doc.GetElementsByTagName("PrePullDistance")[0].InnerText);
                    }
                    catch
                    {
                        PrePullDistance = 30;
                    }
                    UseAutoAttack = Convert.ToBoolean(_doc.GetElementsByTagName("UseAutoAttack")[0].InnerText);
                    SendPet = Convert.ToBoolean(_doc.GetElementsByTagName("SendPet")[0].InnerText);
                    try
                    {
                        GlobalCooldown = Convert.ToInt32(_doc.GetElementsByTagName("GlobalCooldown")[0].InnerText);
                    }
                    catch
                    {
                        GlobalCooldown = 2000;
                    }
                }
                catch
                {
                }
                LoadController(_doc.GetElementsByTagName("CombatController"), CombatController);
                LoadController(_doc.GetElementsByTagName("BuffController"), BuffController);
                LoadController(_doc.GetElementsByTagName("RestController"), RestController);
                LoadController(_doc.GetElementsByTagName("PullController"), PullController);
                LoadController(_doc.GetElementsByTagName("PrePullController"), PrePullController);
            }
            catch (Exception e)
            {
                Logging.Write("Something went wrong when loading behavior: " + e);
            }
        }

        internal void LoadController(XmlNodeList xmlNodeList, RuleController ruleController)
        {
            foreach (XmlNode xmlMainNode in xmlNodeList)
            {
                foreach (XmlNode xmlNode in xmlMainNode.ChildNodes)
                {
                    if (xmlNode.Name.Equals("Rule"))
                    {
                        //We got our hands on a rule, lets load it.
                        var rule = new Rule();
                        foreach (XmlNode childNode in xmlNode)
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

        internal string SaveRule(Rule rule)
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

        internal void Save()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                if (!Directory.Exists(PVEBehaviorCombat.OurDirectory + "\\Behaviors\\"))
                    Directory.CreateDirectory(PVEBehaviorCombat.OurDirectory + "\\Behaviors\\");
                string xml = @"<?xml version=""1.0""?>";
                xml += "<Behavior>";
                xml += "<CombatDistance>" + CombatDistance + "</CombatDistance>";
                xml += "<PullDistance>" + PullDistance + "</PullDistance>";
                xml += "<PrePullDistance>" + PrePullDistance + "</PrePullDistance>";
                xml += "<UseAutoAttack>" + UseAutoAttack + "</UseAutoAttack>";
                xml += "<SendPet>" + SendPet + "</SendPet>";
                xml += "<GlobalCooldown>" + GlobalCooldown + "</GlobalCooldown>";
                if (CombatController != null)
                {
                    xml += "<PrePullController>";
                    foreach (Rule rule in PrePullController.GetRules)
                    {
                        xml += SaveRule(rule);
                    }
                    xml += "</PrePullController>";
                    xml += "<PullController>";
                    foreach (Rule rule in PullController.GetRules)
                    {
                        xml += SaveRule(rule);
                    }
                    xml += "</PullController>";
                    xml += "<CombatController>";
                    foreach (Rule rule in CombatController.GetRules)
                    {
                        xml += SaveRule(rule);
                    }
                    xml += "</CombatController>";

                    xml += "<BuffController>";
                    foreach (Rule rule in BuffController.GetRules)
                    {
                        xml += SaveRule(rule);
                    }
                    xml += "</BuffController>";
                    xml += "<RestController>";
                    foreach (Rule rule in RestController.GetRules)
                    {
                        xml += SaveRule(rule);
                    }
                    xml += "</RestController>";
                }
                xml += "</Behavior>";
                try
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(xml);
                    doc.Save(PVEBehaviorCombat.OurDirectory + "\\Behaviors\\" + Name + ".xml");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Could not save behavior " + e);
                }
            }
        }
    }
}