
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
using LazyLib.Wow;

#endregion

namespace LazyLib.Combat
{
    /// <summary>
    ///   Class representing a self buff action.
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class PSelfBuffAction : PAction
    {
        public PSelfBuffAction(int priority = 0, string spellName = null)
            : base(priority, spellName)
        {
        }

        /// <summary>
        ///   Gets a value indicating whether this buff is wanted.
        /// </summary>
        /// <remarks>
        ///   Calls the base class and checks !Me.HasWellKnownBuff(SpellName), override to define your own IsWanted.
        /// </remarks>
        /// <value><c>true</c> if this buff is wanted; otherwise, <c>false</c>.</value>
        public override bool IsWanted
        {
            get
            {
                if (base.IsWanted && !ObjectManager.MyPlayer.HasWellKnownBuff(SpellName))
                    return true;
                return false;
            }
        }
    }
}