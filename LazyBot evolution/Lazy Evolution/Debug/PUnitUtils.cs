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
using LazyLib.Wow;

namespace LazyEvo.Debug
{
    internal class PUnitUtils : PUnit
    {
        public PUnitUtils(uint baseAddress)
            : base(baseAddress)
        {
        }

        public List<NameValuePair> GetNameValuePairs()
        {
            Boolean flag = BaseAddress != 0;
            var nvList = new List<NameValuePair>();

            var aBase = new PObjectUtils(BaseAddress);
            List<NameValuePair> baseNvList = aBase.GetNameValuePairs();
            nvList.AddRange(baseNvList);

            nvList.Add(new NameValuePair("", ""));
            nvList.Add(new NameValuePair("PUnit", ""));
            nvList.Add(new NameValuePair("Critter", flag ? "" + Critter : ""));
            nvList.Add(new NameValuePair("IsPlayer", flag ? "" + IsPlayer : ""));
            nvList.Add(new NameValuePair("CharmedBy", flag ? "" + CharmedBy : ""));
            nvList.Add(new NameValuePair("SummonedBy", flag ? "" + SummonedBy : ""));
            nvList.Add(new NameValuePair("CreatedBy", flag ? "" + CreatedBy : ""));
            nvList.Add(new NameValuePair("Health", flag ? "" + Health : ""));
            nvList.Add(new NameValuePair("BaseHealth", flag ? "" + BaseHealth : ""));
            nvList.Add(new NameValuePair("BaseMana", flag ? "" + BaseMana : ""));
            nvList.Add(new NameValuePair("Mana", flag ? "" + Mana : ""));
            nvList.Add(new NameValuePair("Rage", flag ? "" + Rage : ""));
            nvList.Add(new NameValuePair("Focus", flag ? "" + Focus : ""));
            nvList.Add(new NameValuePair("Energy", flag ? "" + Energy : ""));
            nvList.Add(new NameValuePair("Holy Power", flag ? "" + HolyPower : ""));
            nvList.Add(new NameValuePair("Holy Power Max", flag ? "" + MaximumHolyPower : ""));
            nvList.Add(new NameValuePair("Soul Shard", flag ? "" + SoulShard : ""));
            nvList.Add(new NameValuePair("Soul Shard Max", flag ? "" + MaximumSoulShard : ""));
            nvList.Add(new NameValuePair("Eclipse", flag ? "" + Eclipse : ""));
            nvList.Add(new NameValuePair("Eclipse Max", flag ? "" + MaximumEclipse : ""));
            nvList.Add(new NameValuePair("RunicPower", flag ? "" + RunicPower : ""));
            nvList.Add(new NameValuePair("MaximumRage", flag ? "" + MaximumRage : ""));
            nvList.Add(new NameValuePair("MaximumFocus", flag ? "" + MaximumFocus : ""));
            nvList.Add(new NameValuePair("MaximumEnergy", flag ? "" + MaximumEnergy : ""));
            nvList.Add(new NameValuePair("MaximumRunicPower", flag ? "" + MaximumRunicPower : ""));
            nvList.Add(new NameValuePair("Level", flag ? "" + Level : ""));
            nvList.Add(new NameValuePair("Name", flag ? "" + Name : ""));
            nvList.Add(new NameValuePair("Class", flag ? "" + Class : ""));
            nvList.Add(new NameValuePair("CreatureType", flag ? "" + CreatureType : ""));
            nvList.Add(new NameValuePair("Classification", flag ? "" + Classification : ""));
            nvList.Add(new NameValuePair("Speed", flag ? "" + Speed : ""));
            nvList.Add(new NameValuePair("IsMoving", flag ? "" + IsMoving : ""));
            nvList.Add(new NameValuePair("ShapeshiftForm", flag ? "" + ShapeshiftForm : ""));
            nvList.Add(new NameValuePair("IsFlying", flag ? "" + IsFlying : ""));
            nvList.Add(new NameValuePair("Swimming", flag ? "" + IsSwimming : ""));
            nvList.Add(new NameValuePair("Reaction", flag ? "" + Reaction : ""));
            nvList.Add(new NameValuePair("Casting", flag ? "" + IsCasting : ""));
            return nvList;
        }
    }
}