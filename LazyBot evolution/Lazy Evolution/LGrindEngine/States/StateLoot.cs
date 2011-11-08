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
using LazyEvo.Public;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.States
{
    internal class StateLoot : MainState
    {
        private PUnit _unit;

        public override int Priority
        {
            get { return Prio.LootAround; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (GrindingSettings.StopLootOnFull && Inventory.FreeBagSlots == 0)
                {
                    return false;
                }
                if (ObjectManager.ShouldDefend)
                    return false;
                if (!GrindingSettings.Loot)
                    return false;
                _unit = FindMobToLoot();
                return _unit != null;
            }
        }

        public override string Name()
        {
            return "Loot";
        }

        public override void DoWork()
        {
            GrindingEngine.Navigator.Stop();
            LootAndSkin.DoWork(_unit);
            GrindingEngine.Navigation.UseNextNearestWaypoint();
        }

        private static PUnit FindMobToLoot()
        {
            PUnit toReturn = null;
            foreach (PUnit pUnit in ObjectManager.GetUnits)
            {
                if (pUnit.IsLootable && pUnit.DistanceToSelf < 30 && !PBlackList.IsBlacklisted(pUnit))
                {
                    if (toReturn != null)
                    {
                        if (toReturn.DistanceToSelf > pUnit.DistanceToSelf)
                            toReturn = pUnit;
                    }
                    else
                    {
                        toReturn = pUnit;
                    }
                }
            }
            return toReturn;
        }
    }
}