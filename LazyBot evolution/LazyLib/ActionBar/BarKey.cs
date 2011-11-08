
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
using System.Reflection;

#endregion

namespace LazyLib.ActionBar
{
    #region KeyType enum

    public enum KeyType
    {
        Item,
        Spell,
    }

    #endregion
    /// <summary>
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class WowKey
    {
        private readonly UInt32 Id;

        /// <summary>
        /// Initializes a new instance of the <see cref="WowKey"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="bar">The bar.</param>
        /// <param name="key">The key.</param>
        public WowKey(UInt32 id, int bar, int key)
        {
            Id = id;
            Bar = bar;
            Key = key;
        }

        public Int32 Bar { get; set; }
        public Int32 Key { get; set; }

        /// <summary>
        /// Gets the spell id.
        /// </summary>
        /// <value>The spell id.</value>
        public int SpellId
        {
            get { return (int) Id; }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public KeyType Type
        {
            get
            {
                if ((Id & (UInt32) KeyTypeFlag.ITEM) != 0)
                    return KeyType.Item;
                return KeyType.Spell;
            }
        }

        /// <summary>
        /// Gets the item id.
        /// </summary>
        /// <value>The item id.</value>
        public Int32 ItemId
        {
            get
            {
                uint v1 = Id;
// ReSharper disable RedundantAssignment
                return (int) (v1 &= ~(uint) KeyTypeFlag.ITEM);
// ReSharper restore RedundantAssignment
            }
        }

        // ReSharper disable InconsistentNaming

        #region Nested type: KeyTypeFlag

        private enum KeyTypeFlag : uint
        {
            ITEM = 0x80000000,
        }

        #endregion

        // ReSharper restore InconsistentNaming
    }
}