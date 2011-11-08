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

using System.Collections.Generic;
using System.Threading;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.Helpers
{
    public enum FlyingWaypointsType
    {
        Normal = 0,
        ToTown = 1,
    }

    internal class FlyingNavigation
    {
        private readonly List<Location> _waypoints = new List<Location>();
        private bool _loopWaypoints;

        public FlyingNavigation(IEnumerable<Location> waypoints, bool loopWaypoints, FlyingWaypointsType type)
        {
            _waypoints = new List<Location>(waypoints);
            _loopWaypoints = loopWaypoints;
            CurrentFlyingWaypointsType = type;
            UseNearestWaypoint();
        }

        public FlyingWaypointsType CurrentFlyingWaypointsType { get; private set; }
        public int CurrentWaypointIndex { get; private set; }

        public int GetNearestWaypointIndex
        {
            get { return Location.GetClosestPositionInList(_waypoints, ObjectManager.MyPlayer.Location); }
        }

        public bool IsLastWaypoints
        {
            get { return (CurrentWaypointIndex >= (_waypoints.Count - 1)); }
        }

        public Location NextPos
        {
            get { return _waypoints[CurrentWaypointIndex]; }
        }

        public double NextWaypointDistance
        {
            get { return NextPos.DistanceToSelf2D; }
        }

        public void FaceNextWaypoint()
        {
            NextPos.Face();
        }

        public void Pulse()
        {
            if (Mount.IsMounted() && !ObjectManager.MyPlayer.IsFlying && !ObjectManager.MyPlayer.InVashjir)
            {
                KeyHelper.PressKey("Space");
                while (!ObjectManager.MyPlayer.IsFlying && Mount.IsMounted())
                {
                    Thread.Sleep(5);
                }
                KeyHelper.ReleaseKey("Space");
            }
            if (NextWaypointDistance <= 18)
                SetNextWaypoint();
            if (!NextPos.Equals(FlyingEngine.Navigator.GetDestination))
                FlyingEngine.Navigator.SetDestination(NextPos);
            FlyingEngine.Navigator.Start();
        }

        public void Reset()
        {
            _waypoints.Clear();
            _loopWaypoints = false;
            CurrentWaypointIndex = 0;
        }

        public void SetNextWaypoint()
        {
            if (!IsLastWaypoints)
                CurrentWaypointIndex++;
            else if (IsLastWaypoints && _loopWaypoints)
                CurrentWaypointIndex = 0;
            else
                MoveHelper.ReleaseKeys();
        }

        public void UseNearestWaypoint(int radius = -1)
        {
            if (radius == -1)
            {
                CurrentWaypointIndex = GetNearestWaypointIndex;
                SetNextWaypoint();
            }
            else
            {
                double num = _waypoints[CurrentWaypointIndex].DistanceToSelf;
                int currentWaypointIndex = CurrentWaypointIndex;
                for (int i = CurrentWaypointIndex - radius; i < (CurrentWaypointIndex + radius); i++)
                {
                    if (i > 0 && i < _waypoints.Count)
                    {
                        if (_waypoints[i].DistanceToSelf >= num)
                            continue;
                        num = _waypoints[i].DistanceToSelf;
                        currentWaypointIndex = i;
                    }
                    else
                    {
                        CurrentWaypointIndex = GetNearestWaypointIndex;
                        return;
                    }
                }
                CurrentWaypointIndex = currentWaypointIndex;
            }
        }
    }
}