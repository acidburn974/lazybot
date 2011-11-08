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

using System.Threading;
using LazyEvo.LFlyingEngine.Activity;
using LazyEvo.LFlyingEngine.Helpers;
using LazyEvo.Public;
using LazyLib;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.States
{
    internal class StateRess : MainState
    {
        public override int Priority
        {
            get { return Prio.Ress; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (ObjectManager.MyPlayer.IsDead)
                    return true;
                return false;
            }
        }

        public override void DoWork()
        {
            FlyingEngine.Navigator.Stop();
            if (FlyingSettings.StopOnDeath)
            {
                LazyHelpers.StopAll("We died");
            }
            Logging.Write("Going to ress");
            FlyingEngine.UpdateStats(0, 0, 1);
            Location pos = ObjectManager.MyPlayer.Location;
            Frame frameByName = InterfaceHelper.GetFrameByName("StaticPopup1Button1");
            try
            {
                frameByName.LeftClick();
                Thread.Sleep(2000);
                frameByName.LeftClick();
                Thread.Sleep(3000);
                var maxTimeout = new Ticker(8000);
                if (!ObjectManager.MyPlayer.IsGhost && !maxTimeout.IsReady)
                {
                    frameByName.LeftClick();
                    Thread.Sleep(5000);
                }
            }
            catch
            {
            }
            if (FlyingSettings.FindCorpse && Mount.IsMounted())
            {
                KeyHelper.PressKey("Space");
                Thread.Sleep(20000);
                KeyHelper.ReleaseKey("Space");
                var toLong = new Ticker(150000);
                FlyingEngine.Navigator.SetDestination(new Location(pos.X, pos.Y, ObjectManager.MyPlayer.Location.Z));
                FlyingEngine.Navigator.Start();
                while (ObjectManager.MyPlayer.IsGhost)
                {
                    if (pos.DistanceToSelf2D < 20)
                    {
                        FlyingEngine.Navigator.Stop();
                        Logging.Write("Looks like we found our corpse");
                        DescentToCorpse();
                        Thread.Sleep(2000);
                        ApproachPosFlying.Approach(pos, 3);
                        frameByName.LeftClick();
                        var timeout = new Ticker(5000);
                        while (ObjectManager.MyPlayer.IsGhost && !timeout.IsReady)
                        {
                            Thread.Sleep(10);
                        }
                        break;
                    }
                    if (!Mount.IsMounted() && ObjectManager.MyPlayer.IsGhost)
                    {
                        Logging.Write("We are not mounted, cannot find corpse");
                        toLong.ForceReady();
                        break;
                    }
                    if (toLong.IsReady && ObjectManager.MyPlayer.IsGhost)
                    {
                        Logging.Write("We never found our corpse :(");
                        toLong.ForceReady();
                        break;
                    }
                }
            }
            else
            {
                TrySpiritRess();
            }
            FlyingEngine.Navigator.Stop();
            MoveHelper.ReleaseKeys();
            Thread.Sleep(1500);
            if (!ObjectManager.MyPlayer.IsAlive)
            {
                if (FlyingSettings.FindCorpse)
                {
                    Logging.Write("Could not find the corpse... trying to make it anyway");
                    InterfaceHelper.GetFrameByName("GhostFrame").LeftClick();
                    Thread.Sleep(5000);
                    TrySpiritRess();
                    if (!ObjectManager.MyPlayer.IsAlive)
                    {
                        LazyHelpers.StopAll("Could not ress :(");
                    }
                }
                else
                {
                    LazyHelpers.StopAll("Could not ress :(");
                }
            }
            Logging.Write("Ress worked :)");
            FlyingEngine.Navigator.Stop();
            if (ObjectManager.MyPlayer.HasBuff(15007) && FlyingSettings.WaitForRessSick)
            {
                Logging.Write("Waiting for ress sickness");
                Mount.MountUp();
                MoveHelper.Jump(6000);
                if (!Mount.IsMounted())
                {
                    Mount.MountUp();
                    MoveHelper.Jump(6000);
                }
                while (ObjectManager.MyPlayer.HasBuff(15007))
                {
                    Thread.Sleep(5000);
                }
            }
            else
            {
                CombatHandler.RunningAction();
            }
        }

        private static void TrySpiritRess()
        {
            PUnit spirit = FindSpiritHealer();
            if (spirit == null)
            {
                Thread.Sleep(8000);
                spirit = FindSpiritHealer();
                if (spirit == null)
                {
                    LazyHelpers.StopAll("Could not find spirit healer");
                }
            }
            ApproachPosFlying.Approach(spirit.Location, 15);
            spirit.Face();
            DoSpiritRess(spirit);
        }

        private static void DescentToCorpse()
        {
            var ticker = new Ticker(28000);
            var timerDiff = new Ticker(500);
            float diffSelf = 3;
            Location oldPos = ObjectManager.MyPlayer.Location;
            MoveHelper.Down(true);
            while (!ticker.IsReady && diffSelf > 0.3)
            {
                if (timerDiff.IsReady)
                {
                    diffSelf = MoveHelper.NegativeValue(oldPos.Z - ObjectManager.MyPlayer.Location.Z);
                    timerDiff.Reset();
                    oldPos = ObjectManager.MyPlayer.Location;
                }
                Thread.Sleep(10);
            }
            MoveHelper.Down(false);
        }

        private static void DoSpiritRess(PUnit vUnit)
        {
            Log("Going to accept ress sickness");
            Ress(vUnit);
            Thread.Sleep(4000);
            if (ObjectManager.MyPlayer.IsDead || ObjectManager.MyPlayer.IsGhost)
            {
                Ress(vUnit);
            }
            if (ObjectManager.MyPlayer.IsDead || ObjectManager.MyPlayer.IsGhost)
            {
                Ress(vUnit);
            }
            if (!ObjectManager.MyPlayer.IsDead && !ObjectManager.MyPlayer.IsGhost)
            {
                return;
            }
            LazyHelpers.StopAll("Could not ress.");
        }

        private static void Ress(PUnit vUnit)
        {
            MoveHelper.MoveToUnit(vUnit, 2, false);
            Thread.Sleep(1000);
            vUnit.InteractOrTarget(false);
            Thread.Sleep(1000);
            if (ObjectManager.MyPlayer.Target != vUnit)
            {
                vUnit.InteractOrTarget(false);
            }
            Thread.Sleep(1000);
            if (ObjectManager.MyPlayer.Target != vUnit)
            {
                vUnit.InteractOrTarget(false);
            }
            Thread.Sleep(1000);
            if (ObjectManager.MyPlayer.Target != vUnit)
            {
                MoveHelper.ReleaseKeys();
                Thread.Sleep(100);
                KeyHelper.ChatboxSendText("/target " + vUnit.Name + " ;");
            }
            Thread.Sleep(1000);
            vUnit.InteractWithTarget();
            Thread.Sleep(1600);
            Frame frameByName = InterfaceHelper.GetFrameByName("StaticPopup1Button1");
            frameByName.LeftClick();
            var timeout = new Ticker(5000);
            while (ObjectManager.MyPlayer.IsGhost && !timeout.IsReady)
            {
                Thread.Sleep(10);
                frameByName = InterfaceHelper.GetFrameByName("StaticPopup1Button1");
                frameByName.LeftClick();
                Thread.Sleep(1000);
            }
        }

        private static PUnit FindSpiritHealer()
        {
            foreach (PUnit u in ObjectManager.GetUnits)
            {
                try
                {
                    if (u.IsSpiritHealer)
                    {
                        return u;
                    }
                }
                catch
                {
                }
            }
            return null;
        }

        public override string Name()
        {
            return "Ress";
        }
    }
}