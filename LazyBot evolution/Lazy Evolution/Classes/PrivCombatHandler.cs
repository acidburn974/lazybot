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
using System.Threading;
using LazyEvo.Forms;
using LazyEvo.Public;
using LazyLib;
using LazyLib.Combat;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.Classes
{
    internal class PrivCombatHandler
    {
        private static Thread _combatLoopThread = new Thread(ExitCombat);
        private static Thread _combatThread = new Thread(ExitCombat);
        private static readonly PUnit Unit = new PUnit(0);
        private static CombatResult _combatResult;

        internal static void InvokeCombatStatusChanged(GCombatEventArgs e)
        {
            CombatHandler.InvokeCombatStatusChanged(e);
        }

        internal static void Stop()
        {
            if (_combatLoopThread.IsAlive)
                _combatLoopThread.Abort();
            if (_combatThread.IsAlive)
                _combatThread.Abort();
            try
            {
                _combatLoopThread.Abort();
            }
            catch
            {
            }
            try
            {
                _combatThread.Abort();
            }
            catch
            {
            }
        }

        private static void ExitCombat()
        {
            try
            {
                _combatLoopThread.Abort();
            }
            catch
            {
            }
            try
            {
                _combatThread.Abort();
            }
            catch
            {
            }
            MoveHelper.ReleaseKeys();
            if (_combatResult == CombatResult.OtherPlayerTag)
            {
                PBlackList.Blacklist(Unit, 1200, false);
            }
            if (_combatResult == CombatResult.Bugged && ObjectManager.MyPlayer.HasLivePet)
            {
                KeyHelper.SendKey("PetFollow");
            }
            if (Unit.IsDead)
            {
                _combatResult = CombatResult.Success;
            }
            if (!ObjectManager.MyPlayer.IsAlive)
            {
                _combatResult = CombatResult.Died;
            }
            Logging.Write("Combat done, result : " + _combatResult);
            InvokeCombatStatusChanged(new GCombatEventArgs(CombatType.CombatDone, Unit));
            CombatDone();
            try
            {
                Stop();
            }
            catch
            {
            }
        }


        private static void CombatThread()
        {
            try
            {
                Logging.Write("Started combat engine");
                if (ObjectManager.MyPlayer.IsMounted && !ObjectManager.MyPlayer.TravelForm)
                    KeyHelper.SendKey("GMount");
                MoveHelper.ReleaseKeys();
                if (DefendAgainst() == null)
                {
                    Logging.Write("Pulling: " + Unit.Name + " " + Unit.GUID);
                    MoveHelper.MoveToUnit(Unit, 30);
                    if (!Unit.TargetHostile())
                    {
                        if (ObjectManager.GetAttackers.Count == 0)
                            PPullBlackList.Blacklist(Unit, 800, true);
                    }
                    Unit.Face();
                    MoveHelper.ReleaseKeys();
                    PullResult result = Pull();
                    Logging.Write("Pull result: " + result);
                    if (result.Equals(PullResult.CouldNotPull))
                    {
                        PPullBlackList.Blacklist(Unit, 800, true);
                        return;
                    }
                    if (PPullBlackList.IsBlacklisted(Unit))
                    {
                        return;
                    }
                }
                else
                {
                    Logging.Write("Got into combat with: " + Unit.Name);
                    Unit.TargetHostile();
                    Unit.Face();
                }
                Ticker combatTimeout;
                if (ObjectManager.MyPlayer.Level > 10)
                {
                    combatTimeout = new Ticker(20*1000);
                }
                else
                {
                    combatTimeout = new Ticker(40*1000);
                }
                while (!Unit.IsDead)
                {
                    _combatLoopThread = new Thread(DoCombat) {IsBackground = true};
                    _combatLoopThread.Name = "DoCombat";
                    _combatLoopThread.SetApartmentState(ApartmentState.STA);
                    _combatLoopThread.Start();
                    while (_combatLoopThread.IsAlive)
                    {
                        Thread.Sleep(50);
                        if (!Langs.TrainingDummy(Unit.Name) && combatTimeout.IsReady && Unit.Health > 85)
                        {
                            Logging.Write("Combat took to long, bugged - blacklisting");
                            _combatResult = CombatResult.Bugged;
                            if (!PBlackList.IsBlacklisted(Unit))
                                PBlackList.Blacklist(Unit, 1200, false);
                            return;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        internal static void StartCombat(PUnit u)
        {
            if (ObjectManager.MyPlayer.IsDead)
            {
                Stop();
                return;
            }
            Unit.BaseAddress = u.BaseAddress;
            InvokeCombatStatusChanged(new GCombatEventArgs(CombatType.CombatStarted));
            _combatThread = new Thread(CombatThread) {IsBackground = true};
            _combatThread.Name = "CombatThread";
            _combatThread.Start();
            _combatResult = CombatResult.Unknown;
            while (_combatThread.IsAlive)
            {
                try
                {
                    if (Unit.IsDead)
                    {
                        _combatResult = CombatResult.Success;
                        break;
                    }

                    if (!Unit.IsValid || PBlackList.IsBlacklisted(Unit))
                    {
                        if (!Langs.TrainingDummy(ObjectManager.MyPlayer.Target.Name))
                        {
                            _combatResult = CombatResult.Bugged;
                            break;
                        }
                    }

                    if (ObjectManager.MyPlayer.IsDead)
                    {
                        _combatResult = CombatResult.Died;
                        break;
                    }

                    if (Unit.IsPet || Unit.IsTotem)
                    {
                        Logging.Write("We are attacking a totem or a pet... doh");
                        _combatResult = CombatResult.Bugged;
                        break;
                    }

                    if (!Langs.TrainingDummy(Unit.Name) && Unit.IsTagged && !Unit.IsTaggedByMe && !Unit.IsTargetingMe &&
                        Unit != ObjectManager.MyPlayer)
                    {
                        Logging.Write("Other player tag");
                        _combatResult = CombatResult.OtherPlayerTag;
                        break;
                    }
                }
                catch (Exception e)
                {
                    Logging.Write("Exeption in combat handler: " + e);
                }
                Thread.Sleep(160);
            }
            ExitCombat();
        }

        private static PUnit DefendAgainst()
        {
            PUnit defendUnit = null;
            if (ObjectManager.ShouldDefend)
            {
                if (!PBlackList.IsBlacklisted(ObjectManager.GetClosestAttacker))
                {
                    defendUnit = ObjectManager.GetClosestAttacker;
                }
                else
                {
                    foreach (PUnit un in ObjectManager.GetAttackers.Where(un => !PBlackList.IsBlacklisted(un)))
                    {
                        defendUnit = un;
                    }
                }
                return defendUnit;
            }
            return null;
        }

        private static void DoCombat()
        {
            Main.CombatEngine.Combat(Unit);
        }

        internal static void RunningAction()
        {
            Main.CombatEngine.RunningAction();
        }

        private static PullResult Pull()
        {
            return Main.CombatEngine.Pull(Unit);
        }

        internal static void OnRess()
        {
            Main.CombatEngine.OnRess();
        }

        internal static void Rest()
        {
            Main.CombatEngine.Rest();
        }

        internal static void CombatDone()
        {
            Main.CombatEngine.CombatDone();
        }
    }
}