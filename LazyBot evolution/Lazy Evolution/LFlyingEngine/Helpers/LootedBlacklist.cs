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
using System.Linq;
using LazyLib;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.Helpers
{
    public class LootedBlacklist
    {
        private static readonly Dictionary<ulong, DateTime> LootedDic = new Dictionary<ulong, DateTime>();

        private static void Check()
        {
            try
            {
                lock (LootedDic)
                {
                    List<ulong> remove =
                        (from node in LootedDic
                         let diff = node.Value - DateTime.Now
                         where diff.TotalSeconds < 0
                         select node.Key).ToList();
                    foreach (ulong node in remove)
                    {
                        Unblacklist(node);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public static bool IsLooted(PGameObject target)
        {
            try
            {
                if (target != null && LootedDic.ContainsKey(target.GUID))
                {
                    Check();
                    if (LootedDic.ContainsKey(target.GUID))
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.Write("IsLooted: " + e);
            }
            return false;
        }

        public static void Looted(PGameObject target)
        {
            if (target == null) return;
            try
            {
                if (!LootedDic.ContainsKey(target.GUID))
                {
                    lock (LootedDic)
                    {
                        LootedDic[target.GUID] = DateTime.Now.AddSeconds(20);
                    }
                }
            }
            catch
            {
            }
        }

        private static void Unblacklist(ulong guid)
        {
            lock (LootedDic)
            {
                LootedDic.Remove(guid);
            }
        }
    }
}