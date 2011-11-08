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
using LazyEvo.Public;
using LazyLib;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine
{
    internal class SubProfile
    {
        private readonly Random _ran = new Random();
        public List<uint> Factions;
        public List<string> Ignore;
        private int _currentHotSpotIndex = -1;

        public SubProfile()
        {
            Name = "Unnamed";
            PlayerMinLevel = 0;
            PlayerMaxLevel = 99;
            MobMinLevel = 0;
            MobMaxLevel = 99;
            SpotRoamDistance = 40;
            Order = false;
            Spots = new List<Location>();
            Factions = new List<uint>();
            Ignore = new List<string>();
        }

        public string Name { get; set; }
        public int PlayerMinLevel { get; set; }
        public int PlayerMaxLevel { get; set; }
        public int MobMinLevel { get; set; }
        public int MobMaxLevel { get; set; }
        public int SpotRoamDistance { get; set; }
        public List<Location> Spots { get; set; }
        public bool Order { get; set; }

        public Location CurrentSpot
        {
            get
            {
                if (Spots.Count == 0)
                    LazyHelpers.StopAll("Subprofile: " + Name + " does not have any spots");
                if (_currentHotSpotIndex == -1)
                    return Spots[0];
                if (Spots.Count == 1)
                    return Spots[0];
                return Spots[_currentHotSpotIndex];
            }
        }

        public Location ClosestSpot
        {
            get
            {
                if (Spots.Count == 0)
                    LazyHelpers.StopAll("Subprofile: " + Name + " does not have any spots");
                double closest = double.MaxValue;
                Location toRe = null;
                foreach (Location location in Spots)
                {
                    if (location.DistanceToSelf < closest)
                    {
                        closest = location.DistanceToSelf;
                        toRe = location;
                    }
                }
                return toRe;
            }
        }


        public Location GetNextHotSpot()
        {
            if (Spots.Count == 0)
            {
                LazyHelpers.StopAll("Subprofile: " + Name + " does not have any spots");
            }
            if (Spots.Count == 1)
                return Spots[0];
            if (Order)
            {
                _currentHotSpotIndex++;
                if (_currentHotSpotIndex >= Spots.Count)
                    _currentHotSpotIndex = 0;
            }
            else
            {
                _currentHotSpotIndex = _ran.Next(Spots.Count);
            }
            // Logging.Write("Index: " + _currentHotSpotIndex);
            if (_currentHotSpotIndex >= Spots.Count)
            {
                Logging.Write(LogType.Warning, "Could not find a valid spot - spot bot and load a valid profile");
                return new Location(0, 0, 0);
            }
            return Spots[_currentHotSpotIndex];
        }
    }
}