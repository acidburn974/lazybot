/*
This file is part of LazyBot.

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

using System.Collections.Generic;
using System.Reflection;
using LazyLib.Helpers.Vendor;

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
    }
}