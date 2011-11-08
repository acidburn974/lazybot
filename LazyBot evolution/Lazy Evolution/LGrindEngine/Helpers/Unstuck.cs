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
using LazyLib;
using LazyLib.Helpers;

namespace LazyEvo.LGrindEngine.Helpers
{
    internal class Unstuck
    {
        private static Int32 _lastStuckTickcount;
        private static Boolean _lastStuckDirection = true;

        internal static void TryUnstuck()
        {
            Logging.Write(LogType.Warning, "Stuck");
            Thread.Sleep(2000);
            MoveHelper.ReleaseKeys();
            if (_lastStuckTickcount + 5000 < Environment.TickCount)
            {
                Logging.Debug("Jump");
                MoveHelper.Forwards(true);
                MoveHelper.Jump();
                MoveHelper.Forwards(false);
                Thread.Sleep(1500);
            }
            else
            {
                Logging.Debug("Lets unstuck");
                MoveHelper.ReleaseKeys();
                if (_lastStuckDirection)
                    MoveHelper.RotateLeft(true);
                else
                    MoveHelper.RotateRight(true);

                _lastStuckDirection = !_lastStuckDirection;
                Thread.Sleep(500);
                MoveHelper.ReleaseKeys();
                MoveHelper.Forwards(true);
                Thread.Sleep(1000);
                MoveHelper.Forwards(false);
            }
            _lastStuckTickcount = Environment.TickCount;
        }
    }
}