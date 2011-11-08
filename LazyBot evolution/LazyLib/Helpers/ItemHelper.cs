
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
using System;
using System.Collections.Generic;
using System.Reflection;
using LazyLib.Helpers.Vendor;
using LazyLib.Wow;

namespace LazyLib.Helpers
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class ItemHelper
    {
        public static string GetNameById(uint entryId)
        {
            if (ItemDatabase.GetItem(entryId.ToString()) == null)
            {
                Dictionary<string, string> dictionary = WowHeadData.GetWowHeadItem(entryId);
                if (dictionary != null)
                {
                    string name = dictionary["name"]; //Get it from wowhead
                    //string name = GetNameByIdFromCache(entryId);//Get it from cache
                    string quality = dictionary["quality"];
                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(quality))
                    {
                        ItemDatabase.PutItem(entryId.ToString(), name, quality);
                        return name;
                    }
                }
            } else
            {
                return ItemDatabase.GetItem(entryId.ToString())["item_name"].ToString();
            }
            return "Unknown item";
        }

        /*
        //Not sure if i can update this, going to use wowhead instead
        public static string GetNameByIdFromCache(uint itemId)
        {
            try
            {
                var itemStore = (uint) Pointers.Items.Offset;
                string result = "Unknown item";
                var itemMin = Memory.ReadRelative<uint>(0x958AAC);
                var itemMax = Memory.ReadRelative<uint>(0x958AA8);
                var dbBeginOffset = Memory.ReadRelative<uint>(itemStore + 0x2c);
                if (itemId >= itemMin && itemId <= itemMax)
                {
                    uint offset = (itemId - itemMin)*4;
                    var itemIndexOffset = Memory.Read<int>(dbBeginOffset + offset);
                    if (itemIndexOffset != 0)
                    {
                        var stringAddress = Memory.Read<uint>((uint) itemIndexOffset + 0x180);
                        result = Memory.Read<string>(stringAddress);
                    }
                }
                return result;
            } catch (Exception e)
            {
                return "Unknown item";
            }
        } */
    }
}