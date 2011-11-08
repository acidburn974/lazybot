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

namespace LazyEvo.LGrindEngine.Helpers
{
    internal class Mount
    {
        private static readonly Ticker DismountTimer = new Ticker(100);

        public static bool IsMounted()
        {
            return ObjectManager.MyPlayer.IsMounted;
        }

        public static bool MountUp()
        {
            if (IsMounted())
                return true;
            if (ObjectManager.ShouldDefend)
                return false;
            GrindingEngine.Navigator.Stop();
            MoveHelper.ReleaseKeys();
            Thread.Sleep(1000);
            KeyHelper.SendKey("GMount");
            int tickCount = Environment.TickCount;
            if (ObjectManager.ShouldDefend)
                return false;
            while (!IsMounted())
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
                        MoveHelper.StopMove();
                        MoveHelper.Forwards(true);
                        if (ObjectManager.ShouldDefend)
                        {
                            MoveHelper.StopMove();
                            return false;
                        }
                        MoveHelper.Jump(1000);
                        while ((Environment.TickCount - tickCount) < 9000)
                        {
                            Thread.Sleep(100);
                        }
                        MoveHelper.StopMove();
                    }
                    return false;
                }
                Thread.Sleep(100);
            }
            if (Langs.MountCantMount(ObjectManager.MyPlayer.RedMessage))
            {
                ResetRedMessage();
                LazyHelpers.StopAll("Cannot mount inside, fix the profile");
            }
            if (!IsMounted())
            {
                Thread.Sleep(2500);
                if (!IsMounted())
                {
                    return false;
                }
            }
            return true;
        }

        public static void ResetRedMessage()
        {
            Logging.Write("Resetting red message");
            KeyHelper.SendKey("F1");
            Thread.Sleep(100);
            KeyHelper.SendKey("Attack1");
        }

        public static void Dismount()
        {
            if (IsMounted() && DismountTimer.IsReady)
            {
                KeyHelper.SendKey("GMount");
                DismountTimer.Reset();
            }
        }
    }
}