
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using LazyLib.Helpers;

#endregion

namespace LazyLib.Wow
{
    /// <summary>
    ///   Representing us ingame
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class PPlayerSelf : PPlayer
    {
        private readonly uint[] _healthStone = new uint[]
                                                   {
                                                       36892, 36894, 36893, 36889, 36891, 36890, 22105, 22103, 22104,
                                                       9421,
                                                       19013, 19012, 19011, 19010, 5510, 5509, 5511, 5512, 19005, 19004,
                                                       19009, 19008, 19007
                                                   };

        private readonly uint[] _mageFood = new uint[] { 65499, 43523, 43518, 65517, 65516, 65515, 65500 };

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PPlayerSelf" /> class.
        /// </summary>
        /// <param name = "baseAddress">The base address.</param>
        public PPlayerSelf(uint baseAddress) : base(baseAddress)
        {
        }


        /// <summary>
        ///   Gets a value indicating whether [should repair].
        /// </summary>
        /// <value><c>true</c> if [should repair]; otherwise, <c>false</c>.</value>
        public bool ShouldRepair
        {
            get
            {
                /*
                foreach (uint u in ObjectManager.MyPlayer.GetItemsEquippedId)
                {
                    bool foundit = false;
                    foreach (PItem pItem in ObjectManager.GetItems)
                    {
                        if (pItem.EntryId.Equals(u))
                        {
                            foundit = true;
                            double durability = pItem.GetDurabilityPercentage;
                            if (durability < 10)
                                return true;
                        }
                    }
                    if (!foundit)
                    {
                        if (u != 0)
                        {
                            Logging.Write("Found an item with id " + u +
                                          " that does not exist on your char. Assuming it is broken, going to repair");
                            return true;
                        }
                    }
                } */
                if (InterfaceHelper.GetFrameByName("DurabilityFrame") == null)
                    return false;
                return InterfaceHelper.GetFrameByName("DurabilityFrame").IsVisible;
            }
        }

        public bool HasAttackers
        {
            get { return ObjectManager.GetAttackers.Count != 0; }
        }

        public int CoinAge
        {
            get { return GetStorageField<int>((uint) Descriptors.ePlayerFields.PLAYER_FIELD_COINAGE); }
        }

        /// <summary>
        ///   Gets the get id's of the items equipped
        /// </summary>
        /// <value>The id's</value>
        public List<uint> GetItemsEquippedId
        {
            get
            {
                var toReturn = new List<uint>();
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_1_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_2_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_3_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_4_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_5_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_6_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_7_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_8_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_9_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_10_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_11_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_12_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_13_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_14_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_15_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_16_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_17_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_18_ENTRYID));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_19_ENTRYID));
                return toReturn;
            }
        }

        public bool LootWinOpen
        {
            get { return Memory.Read<uint>(Memory.BaseAddress + (uint) Pointers.Globals.LootWindow) != 0; }
        }

        public bool MainHandHasTempEnchant
        {
            get
            {
                PItem item = ObjectManager.MyPlayer.MainHand;
                if (item == null)
                    return true;
                return item.TempEnchants.Any(oneEnchant => oneEnchant != 0);
            }
        }

        public bool OffHandHasTempEnchant
        {
            get
            {
                PItem item = ObjectManager.MyPlayer.OffHand;
                if (item == null)
                    return true;
                foreach (uint oneEnchant in item.TempEnchants)
                {
                    if (oneEnchant != 0)
                        return true;
                }
                return false;
            }
        }

        internal List<ulong> GUIDOfItemsInBag
        {
            get
            {
                var guids = new List<ulong>();
                const int numberOfItems = 16;
                // this could change on a patch day, it's the number of items stored in a player's backpack
                uint i;
                for (i = 0; i < numberOfItems; i++)
                {
                    guids.Add(GetStorageField<ulong>((uint) Descriptors.ePlayerFields.PLAYER_FIELD_PACK_SLOT_1 + 0x8*i));
                    //Logging.Write(GetStorageField<ulong>((uint)Descriptors.ePlayerFields.PLAYER_FIELD_PACK_SLOT_1 + 0x8 * i) + "");
                }
                return guids;
            }
        }

        internal List<UInt64> GUIDOfBags
        {
            get
            {
                var guids = new List<UInt64>();
                UInt64 bag;
                try
                {
                    bag =
                        Memory.ReadRelative<UInt64>(((uint) Pointers.Container.EquippedBagGUID));
                    guids.Add(bag);
                }
                catch
                {
                }
                try
                {
                    bag =
                        Memory.ReadRelative<UInt64>(((uint) Pointers.Container.EquippedBagGUID + 0x8*1));
                    guids.Add(bag);
                }
                catch
                {
                }
                try
                {
                    bag =
                        Memory.ReadRelative<UInt64>(((uint) Pointers.Container.EquippedBagGUID + 0x8*2));
                    guids.Add(bag);
                }
                catch
                {
                }
                try
                {
                    bag =
                        Memory.ReadRelative<UInt64>(((uint) Pointers.Container.EquippedBagGUID + 0x8*3));
                    guids.Add(bag);
                }
                catch
                {
                }
                return guids;
            }
        }

        /*
        /// <summary>
        ///   Gets the backspace left.
        /// </summary>
        /// <value>The backspace left.</value>
        internal int FreeBagSlots
        {
            get
            {
                List<ulong> bagGuids = GUIDOfBags;
                int items = GetItemsInBags.Count();
                int bagSpace = 0;
                foreach (PContainer container in ObjectManager.GetContainers)
                {
                    if (bagGuids.Contains(container.GUID))
                        bagSpace += container.Slots;
                }
                bagSpace += 16;
                return bagSpace - items;
            }
        } */

        /// <summary>
        ///   Returns a item pointer to the mainhand
        /// </summary>
        /// <value>The main hand.</value>
        public PItem MainHand
        {
            get
            {
                foreach (PItem pItem in ObjectManager.GetItems)
                {
                    if (
                        pItem.EntryId.Equals(
                            GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_16_ENTRYID)))
                    {
                        return pItem;
                    }
                }
                return null;
            }
        }

        /// <summary>
        ///   Return a item pointer to the offhand
        /// </summary>
        /// <value>The off hand.</value>
        public PItem OffHand
        {
            get
            {
                foreach (PItem pItem in ObjectManager.GetItems)
                {
                    if (
                        pItem.EntryId.Equals(
                            GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_17_ENTRYID)))
                    {
                        return pItem;
                    }
                }
                return null;
            }
        }


        /// <summary>
        /// Gets the mage refreshment.
        /// </summary>
        /// <value>The mage refreshment.</value>
        public int MageRefreshment 
        {
            get { return ObjectManager.GetItems.Count(var => _mageFood.Contains(var.EntryId)); }
        }

        /// <summary>
        ///   Gets the health stone count.
        /// </summary>
        /// <value>The health stone count.</value>
        public int HealthStoneCount
        {
            get { return ObjectManager.GetItems.Count(var => _healthStone.Contains(var.EntryId)); }
        }

        /// <summary>
        ///   Current number of combination points displayed
        /// </summary>
        /// <value>The combo points.</value>
        public int ComboPoints
        {
            get { return Memory.ReadRelative<byte>((uint) Pointers.ComboPoints.ComboPoints); }
        }

        /// <summary>
        ///   Gets a value indicating whether [winter grasp is in progress].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [winter grasp in progress]; otherwise, <c>false</c>.
        /// </value>
        public bool WinterGraspInProgress
        {
            get { return HasBuff(37795) || (HasBuff(33280) || HasBuff(55629)); }
        }

        /// <summary>
        ///   Gets a value indicating whether [blood rune1 ready].
        /// </summary>
        /// <value><c>true</c> if [blood rune1 ready]; otherwise, <c>false</c>.</value>
        public bool BloodRune1Ready
        {
            get { return IsRuneReady(0); }
        }

        /// <summary>
        ///   Gets a value indicating whether [blood rune2 ready].
        /// </summary>
        /// <value><c>true</c> if [blood rune2 ready]; otherwise, <c>false</c>.</value>
        public bool BloodRune2Ready
        {
            get { return IsRuneReady(1); }
        }

        /// <summary>
        ///   Gets a value indicating whether [frost rune1 ready].
        /// </summary>
        /// <value><c>true</c> if [frost rune1 ready]; otherwise, <c>false</c>.</value>
        public bool UnholyRune1Ready
        {
            get { return IsRuneReady(2); }
        }

        /// <summary>
        ///   Gets a value indicating whether [frost rune2 ready].
        /// </summary>
        /// <value><c>true</c> if [frost rune2 ready]; otherwise, <c>false</c>.</value>
        public bool UnholyRune2Ready
        {
            get { return IsRuneReady(3); }
        }

        /// <summary>
        ///   Gets a value indicating whether [unholy rune1 ready].
        /// </summary>
        /// <value><c>true</c> if [unholy rune1 ready]; otherwise, <c>false</c>.</value>
        public bool FrostRune1Ready
        {
            get { return IsRuneReady(4); }
        }

        /// <summary>
        ///   Gets a value indicating whether [unholy rune2 ready].
        /// </summary>
        /// <value><c>true</c> if [unholy rune2 ready]; otherwise, <c>false</c>.</value>
        public bool FrostRune2Ready
        {
            get { return IsRuneReady(5); }
        }

        /// <summary>
        ///   Returns current zoneid
        /// </summary>
        public uint ZoneId
        {
            get
            {
                return Memory.ReadRelative<uint>((uint)Pointers.Zone.ZoneID);
            }
        }


        /// <summary>
        /// Gets a value indicating whether [in vashir].
        /// </summary>
        /// <value><c>true</c> if [in vashir]; otherwise, <c>false</c>.</value>
        public bool InVashjir
        {
            get { return (ZoneId == 5145 || ZoneId == 5144 || ZoneId == 5146 || ZoneId == 4815); }
        }

        /// <summary>
        ///   Returns current zonetext
        /// </summary>
        public string ZoneText
        {
            get
            {
                return Memory.ReadUtf8(
                    Memory.ReadRelative<uint>((uint) Pointers.Zone.ZoneText), 40);
            }
        }

        /// <summary>
        ///   Returns current Worldmap
        /// </summary>
        public string WorldMap
        {
            get
            {
                return Memory.ReadUtf8(
                    Memory.ReadRelative<uint>((uint) Pointers.Zone.ZoneText), 40);
            }
        }

        /// <summary>
        ///   Returns the experience as percentage.
        /// </summary>
        /// <value>The experience percentage.</value>
        public int ExperiencePercentage
        {
            get
            {
                try
                {
                    return (100*Experience)/NextLevel;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///   Return the experience points.
        /// </summary>
        /// <value>The experience points.</value>
        public int Experience
        {
            get { return GetStorageField<int>((uint) Descriptors.ePlayerFields.PLAYER_XP); }
        }

        /// <summary>
        ///   Returns experience requires to advance to the next level.
        /// </summary>
        /// <value>The experience points to next level.</value>
        public int NextLevel
        {
            get { return GetStorageField<int>((uint) Descriptors.ePlayerFields.PLAYER_NEXT_LEVEL_XP); }
        }

        /// <summary>
        ///   Returns the last red message.
        /// </summary>
        /// <value>The red message.</value>
        public string RedMessage
        {
            get { return Memory.ReadUtf8StringRelative((uint)Pointers.Globals.RedMessage, 256); }
        }

        /// <summary>
        ///   Gets the get id's of the item
        /// </summary>
        /// <param name = "slot">The slot.</param>
        /// <returns></returns>
        /// <value>The id's</value>
        public uint GetItemBySlot(int slot)
        {
            switch (slot)
            {
                case 1:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_1_ENTRYID);
                case 2:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_2_ENTRYID);
                case 3:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_3_ENTRYID);
                case 4:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_4_ENTRYID);
                case 5:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_5_ENTRYID);
                case 6:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_6_ENTRYID);
                case 7:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_7_ENTRYID);
                case 8:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_8_ENTRYID);
                case 9:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_9_ENTRYID);
                case 10:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_10_ENTRYID);
                case 11:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_11_ENTRYID);
                case 12:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_12_ENTRYID);
                case 13:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_13_ENTRYID);
                case 14:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_14_ENTRYID);
                case 15:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_15_ENTRYID);
                case 16:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_16_ENTRYID);
                case 17:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_17_ENTRYID);
                case 18:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_18_ENTRYID);
                case 19:
                    return GetStorageField<uint>((uint) Descriptors.ePlayerFields.PLAYER_VISIBLE_ITEM_19_ENTRYID);
            }
            return 0;
        }

        /// <summary>
        ///   Targets the self.
        /// </summary>
        public void TargetSelf()
        {
            // KeyHelper.SendKey("F1");
            Thread.Sleep(100);
        }

        private bool IsRuneReady(int runeIndex)
        {
            return (((1 << runeIndex) & Memory.ReadRelative<byte>((uint)Pointers.Runes.RunesOffset)) != 0);
        }
    }
}