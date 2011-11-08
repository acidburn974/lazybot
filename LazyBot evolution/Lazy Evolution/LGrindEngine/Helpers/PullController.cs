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
using System.Linq;
using System.Threading;
using LazyEvo.Public;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.Helpers
{
    internal class PullController
    {
        internal static List<PUnit> ValidUnits = new List<PUnit>();
        private static Thread _searchThread;

        public static void Start()
        {
            _searchThread = new Thread(FindMobToPull) {IsBackground = true};
            _searchThread.Name = "FindMobToPull";
            _searchThread.Start();
        }

        public static void Stop()
        {
            if (_searchThread != null && _searchThread.IsAlive)
            {
                _searchThread.Abort();
                _searchThread = null;
            }
        }

        private static void FindMobToPull()
        {
            while (true)
            {
                var tmpValidTargets = new List<PUnit>();
                List<PUnit> units = ObjectManager.GetUnits;
                SubProfile currentSubprofile = GrindingEngine.CurrentProfile.GetSubProfile();
                foreach (PUnit pUnit in units.Where(u => u.IsValid))
                {
                    try
                    {
                        if (IsValidTarget(pUnit) &&
                            !PPullBlackList.IsBlacklisted(pUnit) &&
                            !PBlackList.IsBlacklisted(pUnit)
                            && pUnit.Target.Type != 4)
                        {
                            if (GrindingSettings.SkipMobsWithAdds)
                            {
                                List<PUnit> closeUnits = ObjectManager.CheckForMobsAtLoc(pUnit.Location,
                                                                                         GrindingSettings.
                                                                                             SkipAddsDistance, false);
                                if (closeUnits.Count >= GrindingSettings.SkipAddsCount)
                                {
                                    continue;
                                }
                            }
                            PUnit unit = pUnit;
                            if (
                                currentSubprofile.Spots.Any(
                                    s => unit.Location.DistanceFrom(s) < currentSubprofile.SpotRoamDistance))
                            {
                                if (!pUnit.IsPet) //Do not move, takes a long time
                                {
                                    tmpValidTargets.Add(pUnit);
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                IOrderedEnumerable<PUnit> sortednumQuery =
                    from t in tmpValidTargets
                    orderby t.Location.DistanceToSelf
                    select t;

                ValidUnits = sortednumQuery.ToList();
                Thread.Sleep(500);
            }
        }

        private static bool IsValidTarget(PUnit unitTofind)
        {
            if (unitTofind.IsPlayer || unitTofind.IsTagged || unitTofind.IsDead || unitTofind.IsTotem)
            {
                return false;
            }
            SubProfile currentProfile = GrindingEngine.CurrentProfile.GetSubProfile();
            if (unitTofind.Level < currentProfile.MobMinLevel ||
                unitTofind.Level > currentProfile.MobMaxLevel)
                return false;
            if (currentProfile.Ignore != null)
            {
                if (currentProfile.Ignore.Any(tstIgnore => tstIgnore == unitTofind.Name))
                {
                    return false;
                }
            }
            if (currentProfile.Factions != null)
            {
                if (currentProfile.Factions.Any(faction => unitTofind.Faction == faction))
                {
                    return true;
                }
            }
            return false;
        }
    }
}