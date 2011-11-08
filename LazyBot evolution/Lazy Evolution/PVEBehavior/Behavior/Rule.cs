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
using System.Linq;
using System.Xml;
using LazyEvo.Other;
using LazyEvo.PVEBehavior.Behavior.Conditions;
using LazyLib;
using LazyLib.Helpers;
using LazyLib.Wow;

#endregion

namespace LazyEvo.PVEBehavior.Behavior
{
    public enum Target
    {
        None = 0,
        Self = 1,
        Pet = 2,
        Enemy = 3,
        Unchanged = 4,
    }

    internal class Rule : IComparable<Rule>, IComparer<Rule>
    {
        private readonly List<AbstractCondition> _conditions = new List<AbstractCondition>();
        public int GlobalCooldown;

        public Rule()
        {
            MatchAll = true;
            ShouldTarget = Target.Unchanged;
        }

        public Rule(string name, Action action, int priority)
        {
            Name = name;
            Action = action;
            Priority = priority;
            MatchAll = true;
            ShouldTarget = Target.Unchanged;
        }

        public Rule(string name, Action action, int priority, List<AbstractCondition> conditions)
        {
            Name = name;
            Action = action;
            _conditions = conditions;
            Priority = priority;
            MatchAll = true;
            ShouldTarget = Target.Enemy;
        }

        public Rule(string name, Action action, int priority, List<AbstractCondition> conditions, Target target)
        {
            Name = name;
            Action = action;
            _conditions = conditions;
            Priority = priority;
            MatchAll = true;
            ShouldTarget = target;
        }

        public Rule(string name, string script)
        {
            Name = name;
            Script = script;
        }

        public bool IsScript
        {
            get { return !string.IsNullOrEmpty(Script); }
        }

        public string Name { get; set; }
        public string Script { get; set; }
        public bool MatchAll { get; set; }
        public Action Action { get; set; }
        public Target ShouldTarget { get; set; }
        public int Priority { get; set; }

        public List<AbstractCondition> GetConditions
        {
            get
            {
                lock (_conditions)
                {
                    return _conditions;
                }
            }
        }

        public bool IsOk
        {
            get
            {
                if (IsScript)
                {
                    if (PveBehaviorSettings.AllowScripts)
                    {
                        return ScriptRunner.ShouldRun(Name, Script);
                    }
                    return false;
                }
                if (Action == null)
                    return false;
                if (!Action.IsReady)
                    return false;
                if (GetConditions.Count == 0)
                    return true;
                if (!MatchAll)
                {
                    bool re = GetConditions.Any(condition => condition.IsOk);
                    //Logging.Debug("Evalution Any: " + Name + " " + re);
                    return re;
                }
                bool red = GetConditions.All(condition => condition.IsOk);
                //Logging.Debug("Evalution All: " + Name + " " + red);
                return red;
            }
        }

        #region IComparable<Rule> Members

        public int CompareTo(Rule other)
        {
            if (other == null)
            {
                Logging.Write("Tried to compare null Rule to another - check class code!");
                return 0;
            }
            return Priority.CompareTo(other.Priority);
        }

        #endregion

        #region IComparer<Rule> Members

        public int Compare(Rule x, Rule y)
        {
            return x.Priority.CompareTo(y.Priority);
        }

        #endregion

        public void BotStarting()
        {
            foreach (TickerCondition condition in _conditions.OfType<TickerCondition>())
            {
                (condition).ForceReady();
            }
        }

        public void AddCondition(AbstractCondition condition)
        {
            lock (_conditions)
            {
                _conditions.Add(condition);
            }
        }

        public void ClearConditions()
        {
            lock (_conditions)
            {
                _conditions.Clear();
            }
        }

        public string SaveAction()
        {
            if (Action != null)
            {
                string xml = "<Action>";
                xml += Action.GetXml();
                xml += "</Action>";
                return xml;
            }
            return "";
        }

        public void LoadAction(XmlNode node)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                switch (childNode.Name)
                {
                    case "Type":
                        if (childNode.InnerText.Equals("ActionSpell"))
                        {
                            Action = new ActionSpell();
                            Action.Load(node);
                        }
                        if (childNode.InnerText.Equals("ActionKey"))
                        {
                            Action = new ActionKey();
                            Action.Load(node);
                        }
                        break;
                }
            }
        }

        public void ExecuteAction(int globalCooldown)
        {
            try
            {
                if (IsScript)
                {
                    RunScriptAction();
                }
                if (Action == null) return;
                switch (ShouldTarget)
                {
                    case Target.Enemy:
                        if (ObjectManager.MyPlayer.Target.IsValid)
                            break;
                        ObjectManager.GetClosestAttacker.TargetHostile();
                        break;
                    case Target.Pet:
                        if (ObjectManager.MyPlayer.HasLivePet)
                            ObjectManager.MyPlayer.Pet.TargetFriend();
                        break;
                    case Target.Self:
                        ObjectManager.MyPlayer.TargetSelf();
                        break;
                    case Target.None:
                        if (ObjectManager.MyPlayer.HasTarget)
                            KeyHelper.SendKey("ESC");
                        break;
                    case Target.Unchanged:
                        break;
                }
                Action.Execute(globalCooldown);
                foreach (TickerCondition condition in _conditions.OfType<TickerCondition>())
                {
                    (condition).Reset();
                }
            }
            catch (Exception e)
            {
                // Logging.Write("Error executing action: " + e);
            }
        }

        private void RunScriptAction()
        {
            ScriptRunner.RunCode(Name, Script);
        }
    }
}