
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

using System;
using LazyLib.Helpers;

#endregion

namespace LazyLib.Wow
{
    internal class Faction
    {
        public static Reaction GetReaction(PUnit localObj, PUnit mobObj)
        {
            try
            {
                if (localObj.Faction < 1 || mobObj.Faction < 1)
                {
                    return Reaction.Missing;
                }

                return FindReactionFromFactions(localObj.Faction, mobObj.Faction);
            }
            catch (Exception)
            {
                //Logging.Write("Exception when comparing: " + localObj.Name + " : " + mobObj.Name);
                return Reaction.Missing;
            }
        }

        private static Reaction FindReactionFromFactions(uint localFaction, uint mobFaction)
        {
            var startIndex = Memory.ReadRelative<uint>((uint) Pointers.Reaction.FactionStartIndex);
            var totalFactions = Memory.ReadRelative<uint>((uint) Pointers.Reaction.FactionTotal);
            var factionStartPoint = Memory.ReadRelative<uint>((uint) Pointers.Reaction.FactionPointer);
            uint? localHash = null;
            uint? mobHash = null;

            Reaction reaction;

            if (localFaction >= startIndex && localFaction <= totalFactions)
            {
                if (mobFaction >= startIndex && mobFaction < totalFactions)
                {
                    localHash = factionStartPoint + ((localFaction - startIndex)*4);
                    mobHash = factionStartPoint + ((mobFaction - startIndex)*4);
                }
            }
            if (localHash != null)
            {
                reaction = CompareFactionHash(localHash, mobHash);
            }
            else
            {
                reaction = Reaction.Unknown;
            }
            return reaction;
        }

        private static bool TestBits(uint lBitAddr, uint rBitAddr)
        {
            var lBitParam = Memory.Read<uint>(lBitAddr);
            var rBitParam = Memory.Read<uint>(rBitAddr);
            //Logging.Write((lBitParam & rBitParam) + "");
            if ((lBitParam & rBitParam) != 0) return true;

            return false;
        }

        private static bool HashCompare(int hashIndex, byte[] localBitHash, int mobHashCheck)
        {
            const int hashIndexInc = 4;

            int hashCompare = BitConverter.ToInt32(localBitHash, hashIndex);

            for (uint i = 0; i < 4; i++)
            {
                if (hashCompare == mobHashCheck)
                    return true;

                hashIndex += hashIndexInc;
                hashCompare = BitConverter.ToInt32(localBitHash, hashIndex);

                if (hashCompare == 0)
                    break;
            }
            return false;
        }

        private static Reaction CompareFactionHash(uint? hash1, uint? hash2)
        {
            if (hash1 != null && hash2 != null)
            {
                byte[] localBitHash = Memory.ReadBytes((uint) hash1, 64);
                byte[] mobBitHash = Memory.ReadBytes((uint) hash2, 64);

                int mobHashCheck = BitConverter.ToInt32(mobBitHash, 0x04);
                if (TestBits((uint) BitConverter.ToInt32(localBitHash, 0) + (uint) Pointers.Reaction.HostileOffset1,
                             (uint) BitConverter.ToInt32(mobBitHash, 0) + (uint) Pointers.Reaction.HostileOffset2))
                    return Reaction.Hostile;


                if (HashCompare(0x18, localBitHash, mobHashCheck))
                    return Reaction.Hostile;


                if (TestBits((uint) BitConverter.ToInt32(localBitHash, 0) + (uint) Pointers.Reaction.FriendlyOffset1,
                             (uint) BitConverter.ToInt32(mobBitHash, 0) + (uint) Pointers.Reaction.FriendlyOffset2))
                    return Reaction.Friendly;


                if (HashCompare(0x28, localBitHash, mobHashCheck))
                    return Reaction.Friendly;
            }

            return Reaction.Neutral;
        }
    }

    public enum Reaction
    {
        Unknown = -1,
        Hostile = 1,
        Neutral = 3,
        Friendly = 4,
        Missing = -2
    }
}