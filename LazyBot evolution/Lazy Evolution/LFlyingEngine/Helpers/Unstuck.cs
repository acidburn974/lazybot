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
using System.Threading;
using LazyLib;
using LazyLib.Helpers;

#endregion

namespace LazyEvo.LFlyingEngine.Helpers
{
    internal class Unstuck
    {
        private static readonly Random Ran = new Random();

        internal static void TryUnstuck(bool smallUnstuck)
        {
            Ticker face;
            Logging.Write(LogType.Warning, "Stuck");
            MoveRandom();
            Thread.Sleep(2000);
            MoveHelper.ReleaseKeys();
            if (!smallUnstuck)
            {
                int temp = new Random().Next(1, 3);
                if (temp != 2)
                {
                    MoveHelper.Jump(new Random().Next(2000, 4000));
                }
                else
                {
                    MoveHelper.Down(new Random().Next(1000, 2000));
                }
                MoveRandom();
                Thread.Sleep(1500);
                MoveHelper.ReleaseKeys();
                face = new Ticker(Convert.ToInt32(new Random().Next(50, 200)*Math.PI));
                MoveHelper.RotateRight(true);
                while (!face.IsReady) Thread.Sleep(10);
                MoveHelper.RotateRight(false);
                MoveHelper.Forwards(true);
                Thread.Sleep(new Random().Next(2000, 4000));
                MoveHelper.Forwards(false);
            }
            MoveRandom();
            Thread.Sleep(2000);
            MoveHelper.ReleaseKeys();
            Logging.Write(LogType.Warning, "Done");
        }

        private static void MoveRandom()
        {
            int d = Ran.Next(4);
            if (d == 0 || d == 1)
                MoveHelper.Forwards(true);
            if (d == 1)
                MoveHelper.StrafeRight(true);
            if (d == 2 || d == 3)
                MoveHelper.Backwards(true);
            if (d == 3)
                MoveHelper.StrafeLeft(true);
        }
    }
}