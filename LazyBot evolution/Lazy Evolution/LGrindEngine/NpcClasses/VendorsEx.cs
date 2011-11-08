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

using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.NpcClasses
{
    public enum VendorType
    {
        Unknown = 0,
        Repair = 1,
        Food = 2,
        Train = 3,
    }

    public enum TrainClass
    {
        Unknown = 0,
        Warrior = 1,
        Paladin = 2,
        Hunter = 3,
        Rogue = 4,
        Priest = 5,
        DeathKnight = 6,
        Shaman = 7,
        Mage = 8,
        Warlock = 9,
        Druid = 11,
    }

    internal class VendorsEx
    {
        public VendorsEx(VendorType vendorType, string name, Location location, int entryId)
        {
            VendorType = vendorType;
            Name = name;
            Location = location;
            TrainClass = TrainClass.Unknown;
            EntryId = entryId;
        }

        public VendorsEx(VendorType vendorType, string name, Location location, TrainClass trainClass, int entryId)
        {
            VendorType = vendorType;
            Name = name;
            Location = location;
            TrainClass = trainClass;
            EntryId = entryId;
        }

        public VendorType VendorType { get; private set; }
        public string Name { get; private set; }
        public Location Location { get; private set; }
        public TrainClass TrainClass { get; private set; }
        public int EntryId { get; set; }
    }
}