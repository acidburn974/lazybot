
﻿/*
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
#region

using System.Reflection;
using System.Threading;
using LazyLib.Helpers;

#endregion

namespace LazyLib.Wow
{
    /// <summary>
    ///   Representing a player
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class PPlayer : PUnit
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PPlayer" /> class.
        /// </summary>
        /// <param name = "baseAddress">The base address.</param>
        public PPlayer(uint baseAddress)
            : base(baseAddress)
        {
        }

        /// <summary>
        ///   Returns Player race
        /// </summary>
        public string PlayerRace
        {
            get
            {
                long faction = Faction;
                if (faction.Equals((long) Constants.PlayerFactions.Human))
                    return "Human";
                if (faction.Equals((long) Constants.PlayerFactions.BloodElf))
                    return "Blood Elf";
                if (faction.Equals((long) Constants.PlayerFactions.Dwarf))
                    return "Dwarf";
                if (faction.Equals((long) Constants.PlayerFactions.Gnome))
                    return "Gnome";
                if (faction.Equals((long) Constants.PlayerFactions.NightElf))
                    return "Night Elf";
                if (faction.Equals((long) Constants.PlayerFactions.Orc))
                    return "Orc";
                if (faction.Equals((long) Constants.PlayerFactions.Tauren))
                    return "Tauren";
                if (faction.Equals((long) Constants.PlayerFactions.Troll))
                    return "Troll";
                if (faction.Equals((long) Constants.PlayerFactions.Undead))
                    return "Undead";
                if (faction.Equals((long) Constants.PlayerFactions.Draenei))
                    return "Draenei";
                if (faction.Equals((long) Constants.PlayerFactions.Worgen))
                    return "Worgen";
                if (faction.Equals((long) Constants.PlayerFactions.Goblin))
                    return "Goblin";
                return "Unknown";
            }
        }

        /// <summary>
        ///   Returns faction group (Alliance || Horde)
        /// </summary>
        public string PlayerFaction
        {
            get
            {
                switch (PlayerRace)
                {
                    case "Human":
                    case "Dwarf":
                    case "Gnome":
                    case "Night Elf":
                    case "Draenei":
                    case "Worgen":
                        return "Alliance";
                    case "Orc":
                    case "Undead":
                    case "Tauren":
                    case "Troll":
                    case "Blood Elf":
                    case "Goblin":
                        return "Horde";
                }
                return "Unknown";
            }
        }


        ///<summary>
        ///  Returns the name of the player.
        ///</summary>
        ///<returns></returns>
        public override string Name
        {
            get
            {
                try
                {                  
                    var var1 = Memory.ReadRelative<int>((((uint) Pointers.UnitName.PlayerNameCachePointer) +
                                                  (uint) Pointers.UnitName.PlayerNameMaskOffset));
                    if (var1 == -1)
                        return "Unknown Player";
                    //here we're getting the pointer to the start of the linked List
                    var var2 = Memory.ReadRelative<int>((((uint) Pointers.UnitName.PlayerNameCachePointer) +
                                                  (uint) Pointers.UnitName.PlayerNameBaseOffset));
                    var1 &= (int) GUID;
                    var1 += var1*2;
                    var1 = (var2 + (var1*4) + 4);
                    var1 = Memory.Read<int>((uint) (var1 + 4));

                    //iterate through the linked List until the current entry has
                    //the same GUID as the object whose name we want
                    while (Memory.Read<int>((uint) var1) != (int) GUID)
                    {
                        var var3 =
                            Memory.ReadRelative<int>((((uint) Pointers.UnitName.PlayerNameCachePointer) +
                                                      (uint) Pointers.UnitName.PlayerNameBaseOffset));
                        var2 = (int) GUID;
                        var2 &=
                            Memory.ReadRelative<int>((((uint) Pointers.UnitName.PlayerNameCachePointer) +
                                                      (uint) Pointers.UnitName.PlayerNameMaskOffset));
                        var2 += var2*2;
                        var2 = Memory.Read<int>((uint) (var3 + (var2*4)));
                        var2 += var1;
                        var1 = Memory.Read<int>((uint) (var2 + 4));
                    }

                    //now that we have the correct entry in the linked List,
                    //read its name from entry+0x20
                    return Memory.ReadUtf8((uint) (var1 + (uint) Pointers.UnitName.PlayerNameStringOffset), 40); 
                }
                catch
                {
                    return "Error when reading player name";
                }
            }
        }
    }
}