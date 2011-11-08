
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

using System.Collections.Generic;
using System.Reflection;

#endregion

namespace LazyLib.Wow
{
    /// <summary>
    ///   Contains all information related to a WowItem.
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class PItem : PObject
    {
        public PItem(uint baseAddress)
            : base(baseAddress)
        {
        }

        /// <summary>
        ///   The item's remaining durability.
        /// </summary>
        public int Durability
        {
            get { return GetStorageField<int>((uint) Descriptors.eItemFields.ITEM_FIELD_DURABILITY); }
        }

        public ulong Info
        {
            get { return GetStorageField<ulong>(0x4); }
        }

        /// <summary>
        ///   Returns Display ID
        /// </summary>
        /// <value>The display id.</value>
        public uint EntryId
        {
            get
            {
                try
                {
                    return GetStorageField<uint>((uint) Descriptors.eObjectFields.OBJECT_FIELD_ENTRY);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///   Returns durability as percentage
        /// </summary>
        public float GetDurabilityPercentage
        {
            get
            {
                try
                {
                    return ((Durability)/((float) MaximumDurability))*100;
                }
                catch
                {
                    return 0;
                }
            }
        }

        public List<uint> TempEnchants
        {
            get
            {
                var toReturn = new List<uint>();
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_2_1));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_2_3));
                return toReturn;
            }
        }

        /// <summary>
        ///   Gets the tempory enchants idøs.
        /// </summary>
        /// <value>The enchant id's</value>
        public List<uint> Enchants
        {
            get
            {
                var toReturn = new List<uint>();
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_1_1));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_1_3));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_2_1));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_2_3));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_3_1));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_3_3));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_4_1));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_4_3));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_5_1));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_5_3));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_6_1));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_6_3));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_7_1));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_7_3));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_8_1));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_8_3));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_9_1));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_9_3));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_10_1));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_10_3));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_11_1));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_11_3));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_12_1));
                toReturn.Add(GetStorageField<uint>((uint) Descriptors.eItemFields.ITEM_FIELD_ENCHANTMENT_12_3));
                return toReturn;
            }
        }

        /// <summary>
        ///   Gets the contained.
        /// </summary>
        /// <value>The contained.</value>
        public ulong Contained
        {
            get { return GetStorageField<ulong>((uint) Descriptors.eItemFields.ITEM_FIELD_CONTAINED); }
        }

        /// <summary>
        ///   The item's maximum durability.
        /// </summary>
        public int MaximumDurability
        {
            get { return GetStorageField<int>((uint) Descriptors.eItemFields.ITEM_FIELD_MAXDURABILITY); }
        }

        /// <summary>
        ///   The amount of items stacked.
        /// </summary>
        public int StackCount
        {
            get { return GetStorageField<int>((uint) Descriptors.eItemFields.ITEM_FIELD_STACK_COUNT); }
        }

        /// <summary>
        ///   The amount of charges this item has.
        /// </summary>
        public int Charges
        {
            get { return GetStorageField<int>((uint) Descriptors.eItemFields.ITEM_FIELD_SPELL_CHARGES); }
        }

        /// <summary>
        ///   Does the item have charges?
        /// </summary>
        public bool HasCharges
        {
            get { return Charges > 0 ? true : false; }
        }
    }
}