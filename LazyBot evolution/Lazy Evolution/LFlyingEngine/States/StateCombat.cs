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
using LazyEvo.LFlyingEngine.Helpers;
using LazyEvo.Public;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.States
{
    internal class StateCombat : MainState
    {
        private PUnit _unit;

        public override int Priority
        {
            get { return Prio.Combat; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (ObjectManager.MyPlayer.IsMounted)
                    return false;
                _unit = null;
                _unit = DefendAgainst();
                return _unit != null;
            }
        }

        public override string Name()
        {
            return "Combat";
        }

        public override void DoWork()
        {
            FlyingEngine.Navigator.Stop();
            if (FlyingSettings.SendKeyOnStartCombat)
            {
                KeyHelper.SendKey("CombatStart");
            }
            CombatHandler.StartCombat(_unit);
            FlyingEngine.UpdateStats(0, 1, 0);
            if (ObjectManager.MyPlayer.IsDead)
                return;
            CombatHandler.RunningAction();
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