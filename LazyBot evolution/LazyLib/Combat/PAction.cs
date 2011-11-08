
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
using System.Reflection;
using LazyLib.ActionBar;
using LazyLib.Wow;

#endregion

namespace LazyLib.Combat
{
    /// <summary>
    ///   Abstract main class.
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public abstract class PAction : IComparable<PAction>, IComparer<PAction>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PAction" /> class.
        /// </summary>
        /// <param name = "priority">The priority.</param>
        /// <param name = "spellName">Name of the spell.</param>
        /// <param name = "range">The range.</param>
        protected PAction(int priority = 0, string spellName = null, int range = 5)
        {
            Priority = priority;
            SpellName = spellName;
            Range = range;
        }

        public virtual int Priority { get; private set; }
        public virtual string SpellName { get; private set; }
        public virtual int Range { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether this instance is wanted.
        /// </summary>
        /// <value><c>true</c> if this instance is wanted; otherwise, <c>false</c>.</value>
        public virtual bool IsWanted
        {
            get { return IsReady; }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is ready.
        /// </summary>
        /// <remarks>
        ///   Base class autochecks IsSpellReadyByName(SpellName) and !ObjectManager.MyPlayer.IsCasting
        /// </remarks>
        /// <value><c>true</c> if this instance is ready; otherwise, <c>false</c>.</value>
        public virtual bool IsReady
        {
            get { return BarMapper.IsSpellReadyByName(SpellName) && !ObjectManager.MyPlayer.IsCasting; }
        }

        public virtual bool WaitUntilReady
        {
            get { return false; }
        }

        #region IComparable<PAction> Members

        public int CompareTo(PAction other)
        {
            if (other == null)
            {
                Logging.Write("Tried to compare null PAction to another - check class code!");
                return 0;
            }
            return -Priority.CompareTo(other.Priority);
        }

        #endregion

        #region IComparer<PAction> Members

        public int Compare(PAction x, PAction y)
        {
            return -x.Priority.CompareTo(y.Priority);
        }

        #endregion

        /// <summary>
        ///   Execute the actions.
        /// </summary>
        public virtual void Execute()
        {
            BarMapper.CastSpell(SpellName);
        }

        /// <summary>
        /// Returns a BarSpell
        /// </summary>
        /// <returns></returns>
        public BarSpell GetKey()
        {
            return BarMapper.GetSpellByName(SpellName);
        }
    }
}