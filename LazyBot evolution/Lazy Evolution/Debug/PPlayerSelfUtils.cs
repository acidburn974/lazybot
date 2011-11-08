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
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.Debug
{
    internal class PPlayerSelfUtils : PPlayerSelf
    {
        public PPlayerSelfUtils(uint baseAddress)
            : base(baseAddress)
        {
        }

        public List<NameValuePair> GetNameValuePairs()
        {
            Boolean flag = BaseAddress != 0;
            var nvList = new List<NameValuePair>();

            var aBase = new PPlayerUtils(BaseAddress);
            List<NameValuePair> baseNvList = aBase.GetNameValuePairs();
            nvList.AddRange(baseNvList);

            nvList.Add(new NameValuePair("", ""));
            nvList.Add(new NameValuePair("PPlayerSelf", ""));
            nvList.Add(new NameValuePair("ZoneId", ZoneId.ToString()));
            nvList.Add(new NameValuePair("Blood1", BloodRune1Ready.ToString()));
            nvList.Add(new NameValuePair("Blood2", BloodRune2Ready.ToString()));
            nvList.Add(new NameValuePair("Unholy1", UnholyRune1Ready.ToString()));
            nvList.Add(new NameValuePair("Unholy2", UnholyRune2Ready.ToString()));
            nvList.Add(new NameValuePair("Frost1", FrostRune1Ready.ToString()));
            nvList.Add(new NameValuePair("Frost2", FrostRune2Ready.ToString()));
            nvList.Add(new NameValuePair("Backspace", Inventory.FreeBagSlots.ToString()));

            return nvList;
        }
    }
}