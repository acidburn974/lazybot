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
using System.Threading;
using LazyEvo.Public;
using LazyLib;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.Helpers
{
    internal class Mount
    {
        public static Location LastHarvest;
        private static readonly Ticker DismountTimer = new Ticker(100);
        private static readonly Ticker MountTimeOut = new Ticker(18000);
        private static readonly Ticker MountTimer = new Ticker(2400);

        public static bool IsMounted()
        {
            return ObjectManager.MyPlayer.IsMounted || ObjectManager.MyPlayer.HasBuff(55173);
        }

        public static bool MountUp()
        {
            if (IsMounted())
                return true;
            FlyingEngine.Navigator.Stop();
            MoveHelper.ReleaseKeys();
            if (ObjectManager.ShouldDefend)
                return false;
            if (ObjectManager.MyPlayer.IsSwimming && !ObjectManager.MyPlayer.InVashjir)
            {
                Logging.Write("We got into the water, lets swim up and see if we can mount");
                MoveHelper.Jump(5000);
                Thread.Sleep(1000);
            }
            MoveHelper.ReleaseKeys();
            Latency.Sleep(ObjectManager.MyPlayer.IsInCombat ? 1000 : 500);
            KeyHelper.SendKey("FMount");
            MountTimer.Reset();
            while (!MountTimer.IsReady && !IsMounted())
            {
                if (ObjectManager.ShouldDefend)
                    return false;
                Thread.Sleep(10);
            }
            Latency.Sleep(200);
            int tickCount = Environment.TickCount;
            if (ObjectManager.ShouldDefend)
                return false;
            if (!IsMounted())
            {
                if (ObjectManager.ShouldDefend || ObjectManager.MyPlayer.IsDead)
                    return false;
                TryUnstuck(tickCount);
            }
            if (Langs.MountCantMount(ObjectManager.MyPlayer.RedMessage))
            {
                LazyHelpers.StopAll("Cannot mount inside");
                HelperFunctions.ResetRedMessage();
            }
            if (ObjectManager.ShouldDefend || ObjectManager.MyPlayer.IsDead)
                return false;
            if (!IsMounted())
            {
                Latency.Sleep(2500);
                if (!IsMounted())
                {
                    if (ObjectManager.ShouldDefend || ObjectManager.MyPlayer.IsDead)
                        return false;
                    LazyHelpers.StopAll("Could not mount");
                    return false;
                }
            }
            return true;
        }

        private static void TryUnstuck(int tickCount)
        {
            MountTimeOut.Reset();
            while (!IsMounted() && !MountTimeOut.IsReady)
            {
                if ((Environment.TickCount - tickCount) > 3000)
                {
                    if (!ObjectManager.ShouldDefend)
                    {
                        MoveHelper.RotateRight(true);
                        while ((Environment.TickCount - tickCount) < 3500)
                        {
                            Thread.Sleep(100);
                        }
                        Thread.Sleep(300);
                        MoveHelper.StopMove();
                        MoveHelper.Forwards(true);
                        if (ObjectManager.ShouldDefend)
                        {
                            MoveHelper.StopMove();
                            return;
                        }
                        MoveHelper.Jump(1000);
                        while ((Environment.TickCount - tickCount) < 9000)
                        {
                            Thread.Sleep(100);
                        }
                        MoveHelper.StopMove();
                    }
                    else
                    {
                        return;
                    }
                }
                if (!IsMounted())
                {
                    Thread.Sleep(500);
                    KeyHelper.SendKey("FMount");
                    MountTimer.Reset();
                    while (!MountTimer.IsReady && !IsMounted())
                    {
                        if (ObjectManager.ShouldDefend || ObjectManager.MyPlayer.IsDead)
                            return;
                        Thread.Sleep(100);
                    }
                    Latency.Sleep(0);
                }
            }
            return;
        }

        public static void Dismount()
        {
            if (IsMounted() && DismountTimer.IsReady)
            {
                KeyHelper.SendKey("FMount");
                DismountTimer.Reset();
            }
        }
    }
}