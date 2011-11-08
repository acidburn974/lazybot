
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

 using System.Reflection;
using LazyLib.Helpers;

namespace LazyLib.ActionBar
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class BarItem
    {
        private readonly KeyWrapper _wrap;

        public BarItem(int id, int bar, int key)
        {
            ItemId = id;
            Bar = bar;
            Key = key;
            _wrap = new KeyWrapper("Unkown item", "none", bar.ToString(), key.ToString());
        }

        public int ItemId { get; private set; }
        public int Bar { get; set; }
        public int Key { get; set; }

        public void SendItem()
        {
            _wrap.SendKey();
        }
    }
}