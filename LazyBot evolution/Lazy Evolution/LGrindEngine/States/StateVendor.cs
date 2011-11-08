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

using LazyEvo.LGrindEngine.Activity;
using LazyEvo.LGrindEngine.Helpers;
using LazyLib;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.Helpers.Vendor;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.States
{
    internal class StateVendor : MainState
    {
        private const int SearchDistance = 12;
        private readonly PUnit _npc = new PUnit(0);

        public override bool NeedToRun
        {
            get
            {
                if (ObjectManager.MyPlayer.IsDead)
                {
                    return false;
                }
                if (!ToTown.ToTownEnabled)
                {
                    return false;
                }
                if ((!LazySettings.ShouldVendor && !LazySettings.ShouldRepair))
                {
                    return false;
                }
                if (!ToTown.ToTownDoRepair && !ToTown.ToTownDoVendor)
                {
                    return false;
                }
                if (ToTown.Vendor == null)
                {
                    return false;
                }
                _npc.BaseAddress = 0;
                foreach (PUnit unit in ObjectManager.GetUnits)
                {
                    if (ToTown.Vendor.EntryId != int.MinValue)
                    {
                        if (unit.Entry == ToTown.Vendor.EntryId && unit.Location.DistanceToSelf2D < SearchDistance)
                        {
                            if (!GrindingBlackList.IsBlacklisted(unit.Name))
                            {
                                _npc.BaseAddress = unit.BaseAddress;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (unit.Name.Equals(ToTown.Vendor.Name) && unit.Location.DistanceToSelf2D < SearchDistance)
                        {
                            if (!GrindingBlackList.IsBlacklisted(unit.Name))
                            {
                                _npc.BaseAddress = unit.BaseAddress;
                                break;
                            }
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
            GrindingEngine.Navigator.Stop();
            MoveHelper.MoveToLoc(_npc.Location, 5);
            VendorManager.DoSell(_npc);
            Logging.Write("[Vendor]Vendor done");
            GrindingEngine.Navigator.Stop();
            GrindingEngine.Navigation = new GrindingNavigation(GrindingEngine.CurrentProfile);
            ToTown.SetToTown(false);
        }

        public override string Name()
        {
            return "Vendor";
        }
    }
}