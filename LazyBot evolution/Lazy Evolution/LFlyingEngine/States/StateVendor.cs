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

using LazyEvo.LFlyingEngine.Activity;
using LazyEvo.LFlyingEngine.Helpers;
using LazyLib;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.Helpers.Vendor;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.States
{
    internal class StateVendor : MainState
    {
        private const int SearchDistance = 12;
        private readonly PUnit _npc = new PUnit(0);

        public override bool NeedToRun
        {
            get
            {
                if (ObjectManager.MyPlayer.IsDead || FlyingEngine.CurrentProfile.WaypointsToTown.Count == 0)
                {
                    return false;
                }
                if (!ToTown.ToTownEnabled)
                {
                    return false;
                }
                if ((!LazySettings.ShouldVendor && !LazySettings.ShouldRepair) ||
                    string.IsNullOrEmpty(FlyingEngine.CurrentProfile.VendorName))
                {
                    return false;
                }
                if (!ToTown.ToTownDoRepair && !ToTown.ToTownDoVendor)
                {
                    return false;
                }
                if (!ToTown.FollowingWaypoints)
                {
                    return false;
                }
                _npc.BaseAddress = 0;
                foreach (PUnit unit in ObjectManager.GetUnits)
                {
                    if (unit.Name.Equals(FlyingEngine.CurrentProfile.VendorName) &&
                        unit.Location.DistanceToSelf2D < SearchDistance)
                    {
                        if (!FlyingBlackList.IsBlacklisted(unit.GUID))
                        {
                            _npc.BaseAddress = unit.BaseAddress;
                            break;
                        }
                    }
                }
                return (_npc.BaseAddress != 0);
            }
        }

        public override int Priority
        {
            get { return Prio.Vendor; }
        }

        public override void DoWork()
        {
            FlyingEngine.Navigator.Stop();
            if (ApproachPosFlying.Approach(_npc.Location, 12))
            {
                MoveHelper.MoveToLoc(_npc.Location, 5);
                VendorManager.DoSell(_npc);
            }
            FlyingBlackList.Blacklist(_npc, 200, true);
            Logging.Write("[Vendor]Vendor done");
        }

        public override string Name()
        {
            return "Vendor";
        }
    }
}