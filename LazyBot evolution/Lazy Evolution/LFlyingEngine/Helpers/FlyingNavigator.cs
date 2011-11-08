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

namespace LazyEvo.LFlyingEngine.Helpers
{
    internal class FlyingNavigator
    {
        private static Location _destination = new Location(0, 0, 0);
        private static Thread _navigatorThread;
        private static double _stopDistance = 2;
        private static readonly Ticker StuckTimer = new Ticker(300000); //5 min

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
            try
            {
                if (IsRunning)
                    return;
                _navigatorThread = new Thread(NavigatorLoop);
                _navigatorThread.Name = "NavigatorThread";
                _navigatorThread.Start();
                _navigatorThread.IsBackground = true;
            }
            catch (Exception)
            {
                _navigatorThread = null;
            }
            //Logging.Debug("Navigator thread started");
        }

        internal void Stop()
        {
            try
            {
                if (IsRunning)
                {
                    _navigatorThread.Abort();
                    _navigatorThread = null;
                    //Logging.Debug("Navigator thread stopped");
                }
            }
            catch (Exception)
            {
                _navigatorThread = null;
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
                double destinationDistance = _destination.Z != 0
                                                 ? _destination.DistanceToSelf
                                                 : _destination.DistanceToSelf2D;
                if (destinationDistance > _stopDistance)
                {
                    if (Stuck.IsStuck)
                    {
                        Unstuck.TryUnstuck(stuck < 2);
                        MoveHelper.ReleaseKeys();
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
                    if (_destination.DistanceToSelf2D > 60)
                        HelperFunctions.Move3D(_destination, 30);
                    else if (_destination.DistanceToSelf2D > 30)
                        HelperFunctions.Move3D(_destination, 20);
                    else if (_destination.DistanceToSelf2D > 20)
                        HelperFunctions.Move3D(_destination, 8);
                    else
                        HelperFunctions.Move3D(_destination, 6);
                    KeyHelper.PressKey("Up");
                }
                else
                {
                    KeyHelper.PressKey("Up");
                    MoveHelper.ReleaseKeys();
                    KeyHelper.ReleaseKey("Up");
                }
                Thread.Sleep(150);
            }
// ReSharper disable FunctionNeverReturns
        }

// ReSharper restore FunctionNeverReturns

        public static bool StillMoving()
        {
            var maxwait = new Ticker(850);
            Location orig = ObjectManager.MyPlayer.Location;
            Location next = orig;
            while (orig.DistanceFrom(next) < 0.15 && !maxwait.IsReady)
            {
                Thread.Sleep(10);
                next = ObjectManager.MyPlayer.Location;
            }
            if (orig.DistanceFrom(next) > 0.15)
                return true;
            //Changed to 0.1, if it where flying directly into a wall i would never figure out that it where stuck
            return false;
        }
    }
}