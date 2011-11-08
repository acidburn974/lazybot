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

using System.Linq;
using LazyEvo.LGrindEngine.Activity;
using LazyEvo.LGrindEngine.Helpers;
using LazyEvo.Public;
using LazyLib;
using LazyLib.FSM;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.States
{
    internal class StatePull : MainState
    {
        private PUnit _unit;

        public override int Priority
        {
            get { return Prio.Targetting; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (ToTown.ToTownEnabled)
                    return false;
                if (ObjectManager.ShouldDefend)
                {
                    return false;
                }
                _unit = null;
                _unit = DefendAgainst();
                if (_unit != null)
                    return true;
                foreach (PUnit validUnit in PullController.ValidUnits)
                {
                    if (validUnit.DistanceToSelf < GrindingSettings.ApproachRange)
                    {
                        _unit = validUnit;
                        break;
                    }
                }
                return _unit != null;
            }
        }

        public override string Name()
        {
            return "Pull";
        }

        public override void DoWork()
        {
            if (ObjectManager.ShouldDefend)
            {
                Logging.Write("Not pulling, we are in combat");
                return;
            }
            GrindingEngine.Navigator.Stop();
            if (ObjectManager.ShouldDefend)
            {
                Logging.Write("Not pulling, we are in combat");
                return;
            }
            CombatHandler.StartCombat(_unit);
            GrindingEngine.UpdateStats(0, 1, 0);
            GrindingEngine.Navigation.UseNextNearestWaypoint();
        }

        private static PUnit DefendAgainst()
        {
            PUnit defendUnit = null;
            if (ObjectManager.ShouldDefend)
            {
                if (!PBlackList.IsBlacklisted(ObjectManager.GetClosestAttacker))
                {
                    defendUnit = ObjectManager.GetClosestAttacker;
                }
                else
                {
                    foreach (PUnit un in ObjectManager.GetAttackers.Where(un => !PBlackList.IsBlacklisted(un)))
                    {
                        defendUnit = un;
                    }
                }
                return defendUnit;
            }
            return defendUnit;
        }
    }
}