
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
using LazyLib.Wow;

#endregion

namespace LazyLib.ActionBar
{
    /// <summary>
    ///   Represents a spell ingame
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class BarSpell
    {
        private Ticker _globalCooldown;

        public BarSpell(int id, int bar, int key, string name)
        {
            SpellId = id;
            Bar = bar;
            _globalCooldown = new Ticker(1600);
            Key = key;
            Name = name;
            KeyHelper.AddKey(name, "", Bar.ToString(), Key.ToString());
        }

        public int SpellId { get; private set; }
        public int Bar { get; set; }
        public int Key { get; set; }
        public string Name { get; private set; }
        public int Cooldown { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [does key exist].
        /// </summary>
        /// <value><c>true</c> if [does key exist]; otherwise, <c>false</c>.</value>
        public bool DoesKeyExist
        {
            get { return BarMapper.HasSpellByName(Name); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is ready.
        /// </summary>
        /// <value><c>true</c> if this instance is ready; otherwise, <c>false</c>.</value>
        public bool IsReady
        {
            get { return BarMapper.IsSpellReadyById(SpellId); }
        }

        /// <summary>
        /// Sets the cooldown.
        /// </summary>
        /// <param name="cooldown">The cooldown.</param>
        public void SetCooldown(int cooldown)
        {
            _globalCooldown = new Ticker(cooldown);
            Cooldown = cooldown;
        }

        /// <summary>
        ///   Casts the spell.
        /// </summary>
        public void CastSpell()
        {
            KeyHelper.SendKey(Name);
            _globalCooldown.Reset();

            while (ObjectManager.MyPlayer.IsCasting || !_globalCooldown.IsReady)
            {
                Thread.Sleep(10);
            }
        }
    }
}