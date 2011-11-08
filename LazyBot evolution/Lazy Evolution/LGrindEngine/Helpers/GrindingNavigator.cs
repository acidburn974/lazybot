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
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.Helpers
{
    internal class GrindingNavigator
    {
        private static readonly Ticker StuckTimer = new Ticker(300000); //5 min
        private readonly Ticker _stuckTimer = new Ticker(3000);
        private Location _destination = new Location(0, 0, 0);
        private Thread _navigatorThread;
        private double _stopDistance = 1;

        internal bool IsRunning
        {
            get
            {
                return _navigatorThread != null &&
                       _navigatorThread.IsAlive;
            }
        }

        internal Location GetDestination
        {
            get { return _destination; }
        }

        internal void Start()
        {
            if (IsRunning)
                return;
            _navigatorThread = new Thread(NavigatorLoop);
            _navigatorThread.Name = "NavigatorThread";
            _navigatorThread.Start();
            _navigatorThread.IsBackground = true;
            //Logging.Debug("Navigator thread started");
        }

        internal void Stop()
        {
            if (IsRunning)
            {
                try
                {
                    _navigatorThread.Abort();
                    _navigatorThread = null;
                    // Logging.Debug("Navigator thread stopped");
                }
                catch
                {
                }
            }
            MoveHelper.ReleaseKeys();
        }

        internal void SetDestination(Single x, Single y, Single z)
        {
            _destination = new Location(x, y, z);
        }

        internal void SetDestination(Location pos)
        {
            _destination = pos;
        }

        internal void SetStopDistance(double dis)
        {
            _stopDistance = dis;
        }

        private void NavigatorLoop()
        {
            int stuck = 0;
            while (true)
            {
                double destinationDistance = _destination.DistanceToSelf2D;
                if (Stuck.IsStuck && _stuckTimer.IsReady)
                {
                    Unstuck.TryUnstuck();
                    _stuckTimer.Reset();
                    stuck++;
                }
                if (StuckTimer.IsReady)
                {
                    stuck = 0;
                }
                if (stuck > 6)
                {
                    LazyHelpers.StopAll("Stuck more than 6 times in 5 min");
                }
                if (destinationDistance > _stopDistance)
                {
                    if (!_destination.IsFacing(0.2f))
                    {
                        _destination.Face();
                    }
                    KeyHelper.PressKey("Up");
                }
                else
                {
                    MoveHelper.ReleaseKeys();
                }
                Thread.Sleep(10);
            }
// ReSharper disable FunctionNeverReturns
        }

// ReSharper restore FunctionNeverReturns
    }
}