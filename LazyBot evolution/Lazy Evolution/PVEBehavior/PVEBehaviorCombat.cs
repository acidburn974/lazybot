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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using LazyEvo.PVEBehavior.Behavior;
using LazyEvo.PVEBehavior.Behavior.Conditions;
using LazyEvo.Public;
using LazyLib;
using LazyLib.ActionBar;
using LazyLib.Combat;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.PVEBehavior
{
    internal class PVEBehaviorCombat : CombatEngine
    {
        private const int Avoidaddsdistance = 30;
        private static readonly KeyWrapper PetAttackKey = new KeyWrapper("PetAttack", "Ctrl", "Indifferent", "1");
        private static readonly KeyWrapper PetFollow = new KeyWrapper("PetFollow", "Ctrl", "Indifferent", "2");
        internal static BehaviorController Behavior;
        internal static string OurDirectory;
        private readonly Ticker _addBackup = new Ticker(4*1000);

        public override string Name
        {
            get { return "Behavior Engine"; }
        }

        public override bool StartOk
        {
            get
            {
                var executableFileInfo = new FileInfo(Application.ExecutablePath);
                string executableDirectoryName = executableFileInfo.DirectoryName;
                OurDirectory = executableDirectoryName;
                PveBehaviorSettings.LoadSettings();
                if (Behavior == null)
                {
                    if (File.Exists(OurDirectory + "\\Behaviors\\" + PveBehaviorSettings.LoadedBeharvior + ".xml"))
                    {
                        Behavior = new BehaviorController();
                        Behavior.Load(OurDirectory + "\\Behaviors\\" + PveBehaviorSettings.LoadedBeharvior + ".xml");
                    }
                    else
                    {
                        Logging.Write("Could not load the behavior, please select a different one");
                        Behavior = null;
                        return false;
                    }
                }
                return true;
            }
        }

        public override void BotStarted()
        {
            if (File.Exists(OurDirectory + "\\Behaviors\\" + PveBehaviorSettings.LoadedBeharvior + ".xml"))
            {
                Behavior = new BehaviorController();
                Behavior.Load(OurDirectory + "\\Behaviors\\" + PveBehaviorSettings.LoadedBeharvior + ".xml");
            }
            CheckBuffAndKeys(Behavior.PullController.GetRules);
            CheckBuffAndKeys(Behavior.CombatController.GetRules);
            CheckBuffAndKeys(Behavior.BuffController.GetRules);
            CheckBuffAndKeys(Behavior.RestController.GetRules);
            Behavior.CombatController.GetRules.Sort();
            Behavior.BuffController.GetRules.Sort();
            Behavior.BuffController.GetRules.Sort();
            Behavior.PrePullController.GetRules.Sort();
            Behavior.PullController.GetRules.Sort();
        }

        private void CheckBuffAndKeys(IEnumerable<Rule> rules)
        {
            if (rules == null)
            {
                return;
            }
            if (rules.Count() == 0)
            {
                return;
            }
            foreach (Rule rule in rules)
            {
                try
                {
                    if (rule.IsScript)
                        continue;
                    rule.BotStarting();
                    if (!rule.Action.DoesKeyExist)
                        Logging.Write(LogType.Warning, "Key: " + rule.Action.Name + " does not exist on your bars");
                    foreach (AbstractCondition abstractCondition in rule.GetConditions)
                    {
                        if (abstractCondition is BuffCondition)
                        {
                            if (!String.IsNullOrEmpty(((BuffCondition) abstractCondition).GetBuffName()))
                                if (!BarMapper.DoesBuffExist(((BuffCondition) abstractCondition).GetBuffName()))
                                    Logging.Write(LogType.Warning,
                                                  "Buff: " + ((BuffCondition) abstractCondition).GetBuffName() +
                                                  " does not exist in HasWellKnownBuff will not detect it correctly");
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.Write("Error checking rule: " + e);
                }
            }
        }

        public override void RunningAction()
        {
            if (ObjectManager.MyPlayer.IsDead)
                return;
            // Logging.Debug("Running started");
            try
            {
                if (Behavior.BuffController.GetRules != null)
                {
                    foreach (Rule rule in
                        Behavior.BuffController.GetRules.Where(rule => rule.IsOk).Where(rule => rule != null))
                    {
                        rule.ExecuteAction(Behavior.GlobalCooldown);
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            //Logging.Debug("Running done");
        }

        private void Buff()
        {
            if (ObjectManager.MyPlayer.IsDead)
                return;
            //Logging.Debug("Buff started");
            foreach (Rule rule in Behavior.BuffController.GetRules.Where(rule => rule.IsOk))
            {
                rule.ExecuteAction(Behavior.GlobalCooldown);
            }
            //Logging.Debug("Buff done");
        }

        public void PrePull(PUnit target)
        {
            //Logging.Debug("Pre pull started");
            if (target.DistanceToSelf > Behavior.PrePullDistance)
            {
                MoveHelper.MoveToUnit(target, Behavior.PrePullDistance);
            }
            foreach (Rule rule in Behavior.PrePullController.GetRules.Where(rule => rule.IsOk))
            {
                rule.ExecuteAction(Behavior.GlobalCooldown);
            }
            //Logging.Debug("Pre pull done");
        }

        public override PullResult Pull(PUnit target)
        {
            //Logging.Debug("Pull started");
            Buff();
            PrePull(target);
            if (Behavior.UseAutoAttack)
            {
                target.InteractWithTarget();
            }
            if (!MoveHelper.MoveToUnit(target, Behavior.PullDistance))
                return PullResult.CouldNotPull;
            if (Behavior.SendPet)
                PetAttackKey.SendKey();
            foreach (Rule rule in Behavior.PullController.GetRules.Where(rule => rule.IsOk))
            {
                target.Face();
                rule.ExecuteAction(Behavior.GlobalCooldown);
            }
            // Logging.Debug("Pull done");
            if (PPullBlackList.IsBlacklisted(target))
            {
                return PullResult.CouldNotPull;
            }
            return PullResult.Success;
        }

        public override void OnRess()
        {
            Buff();
        }

        public override void Rest()
        {
            if (ObjectManager.MyPlayer.IsDead)
                return;
            //Logging.Debug("Rest started");
            Behavior.RestController.GetRules.Sort();
            foreach (Rule rule in Behavior.RestController.GetRules.Where(rule => rule.IsOk))
            {
                rule.ExecuteAction(Behavior.GlobalCooldown);
            }
            Buff();
        }

        public override void Combat(PUnit target)
        {
            if (Behavior.UseAutoAttack)
            {
                target.InteractWithTarget();
            }
            if (Behavior.SendPet)
                PetAttackKey.SendKey();
            //Logging.Debug("Combat started");
            while (true)
            {
                try
                {
                    if (target.DistanceToSelf > Behavior.CombatDistance)
                        MoveHelper.MoveToUnit(target, Behavior.CombatDistance);
                }
                catch
                {
                }
                if (!ObjectManager.MyPlayer.IsValid || ObjectManager.MyPlayer.Target != target)
                    target.TargetHostile();
                if (PveBehaviorSettings.AvoidAddsCombat)
                    ConsiderAvoidAdds(target);
                foreach (Rule rule in Behavior.CombatController.GetRules.Where(rule => rule.IsOk))
                {
                    if (target.IsValid && target.IsAlive)
                    {
                        if (!target.Location.IsFacing())
                        {
                            target.Face();
                        }
                        rule.ExecuteAction(Behavior.GlobalCooldown);
                        break;
                    }
                }
                Thread.Sleep(10);
                Application.DoEvents();
            }
        }

        /*
        * ConsiderAvoidAdds - Avoid Adds
        */

        internal void ConsiderAvoidAdds(PUnit target)
        {
            bool petattacking = false;
            List<PUnit> closeUnits = ObjectManager.CheckForMobsAtLoc(target.Location,
                                                                     PveBehaviorSettings.SkipAddsDis + 5, false);
            if (closeUnits.Count == 0) return;
            PUnit closestAdd = GetClosestBesides(closeUnits, target);
            if (closestAdd == null) return;
            if (closestAdd.GUID == target.GUID) return;
            // Somebody is close enough to maybe jump in.  If the monster is in front of us and close
            // enough, might be time to back it up.
            if (closestAdd.DistanceToSelf < Avoidaddsdistance)
            {
                Logging.Write("Possible add: " + closestAdd.Name + ": " + closestAdd.DistanceToSelf);
                _addBackup.Reset();
                var futility = new Ticker(3000);
                closestAdd.Face();
                MoveHelper.Backwards(true);
                if (ObjectManager.MyPlayer.HasLivePet)
                {
                    petattacking = ObjectManager.MyPlayer.Pet.Target.IsValid;
                    PetFollow.SendKey();
                }

                while (!futility.IsReady)
                {
                    Thread.Sleep(10);
                    closestAdd.Face();
                    if (closestAdd.DistanceToSelf > Avoidaddsdistance + 6.0) // Slack space.
                        break;
                }
                MoveHelper.Backwards(false);
                if (ObjectManager.MyPlayer.HasLivePet && petattacking)
                    PetAttackKey.SendKey();
                _addBackup.Reset();
            }
        }

        private static PUnit GetClosestBesides(IEnumerable<PUnit> list, PUnit ignore)
        {
            double closest = double.MaxValue;
            PUnit toReturn = null;
            foreach (PUnit pUnit in list)
            {
                if (pUnit.DistanceToSelf < closest && (ignore.GUID != pUnit.GUID) && !ignore.IsPet &&
                    !(pUnit.IsInCombat || pUnit.IsTargetingMe || pUnit.IsTargetingMyPet))
                {
                    closest = pUnit.DistanceToSelf;
                    toReturn = pUnit;
                }
            }
            return toReturn;
        }

        public override Form Settings()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            return new BehaviorForm(Behavior);
        }
    }
}