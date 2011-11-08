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
using System.Threading;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.Helpers
{
    internal class GrindingNavigation
    {
        private readonly Ticker _standingStillToLong = new Ticker(3000);
        public GrindingWaypointType CurrentGrindingWaypointsType = GrindingWaypointType.Normal;
        private Int32 _currentWaypointIndex;
        private List<Location> _path;

        public GrindingNavigation(PathProfile pathProfile)
        {
            PathProfile = pathProfile;
            SpotToHit = pathProfile.GetSubProfile().GetNextHotSpot();
            _currentWaypointIndex = 0;
            _path = pathProfile.FindShortsPath(ObjectManager.MyPlayer.Location, SpotToHit);
            SetNextWaypoint();
        }

        private PathProfile PathProfile { get; set; }
        public Location SpotToHit { get; private set; }

        public Location NextPos
        {
            get { return _path[_currentWaypointIndex]; }
        }

        public Location GetNearestWaypoint
        {
            get { return _path[GetNearestWaypointIndex]; }
        }

        public Boolean IsLastWaypoints
        {
            get
            {
                if (_currentWaypointIndex >= _path.Count - 1)
                    return true;
                return false;
            }
        }

        public Double NextWaypointDistance
        {
            get { return NextPos.DistanceToSelf2D; }
        }

        public int GetNearestWaypointIndex
        {
            get { return Location.GetClosestPositionInList(_path, ObjectManager.MyPlayer.Location); }
        }

        public int CurrentWaypointIndex
        {
            get { return _currentWaypointIndex; }
        }

        public Location WayPoint(int index)
        {
            return _path[index];
        }

        public void FaceNextWaypoint()
        {
            NextPos.Face();
        }

        public void SetNewSpot(Location location)
        {
            SpotToHit = location;
            _path = PathProfile.FindShortsPath(ObjectManager.MyPlayer.Location, location);
            _currentWaypointIndex = 0;
            GrindingEngine.Navigator.SetDestination(location);
        }

        public void SetNextWaypoint()
        {
            if (!IsLastWaypoints)
            {
                _currentWaypointIndex++;
            }
            else if (IsLastWaypoints)
            {
                Reset();
                GrindingEngine.Navigator.SetDestination(SpotToHit);
            }
        }

        public void Reset()
        {
            SpotToHit = PathProfile.GetSubProfile().GetNextHotSpot();
            _path = PathProfile.FindShortsPath(ObjectManager.MyPlayer.Location, SpotToHit);
            _currentWaypointIndex = 0;
        }

        public void UseNearestWaypoint()
        {
            _currentWaypointIndex = GetNearestWaypointIndex;
        }

        public void UseNextNearestWaypoint()
        {
            _path = PathProfile.FindShortsPath(ObjectManager.MyPlayer.Location, SpotToHit);
            _currentWaypointIndex = 0;
        }

        public void Pulse()
        {
            if (GrindingEngine.Navigator.GetDestination.DistanceToSelf2D < 1.1 && _standingStillToLong.IsReady)
            {
                Reset();
            }
            if (ObjectManager.MyPlayer.IsMoving)
            {
                _standingStillToLong.Reset();
            }
            if (IsLastWaypoints)
            {
                if (SpotToHit.DistanceToSelf2D < 10)
                {
                    SetNextWaypoint();
                }
                else
                {
                    if (!SpotToHit.Equals(GrindingEngine.Navigator.GetDestination))
                    {
                        GrindingEngine.Navigator.SetDestination(SpotToHit);
                    }
                }
                GrindingEngine.Navigator.Start();
                return;
            }
            if (ObjectManager.MyPlayer.IsAlive)
            {
                if (NextPos.NodeType == NodeType.GroundMount)
                {
                    if (GrindingSettings.Mount && !ObjectManager.MyPlayer.IsMounted)
                    {
                        Mount.MountUp();
                    }
                }
                else if (NextPos.NodeType == NodeType.Normal)
                {
                    if (ObjectManager.MyPlayer.IsMounted)
                    {
                        GrindingEngine.Navigator.Stop();
                        Mount.Dismount();
                        Thread.Sleep(500);
                    }
                }
            }
            if (NextWaypointDistance < 3)
                SetNextWaypoint();
            if (!NextPos.Equals(GrindingEngine.Navigator.GetDestination))
                GrindingEngine.Navigator.SetDestination(NextPos);
            GrindingEngine.Navigator.Start();
        }
    }
}