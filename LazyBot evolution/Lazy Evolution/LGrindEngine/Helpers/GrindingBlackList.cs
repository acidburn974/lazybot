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

#region

using System;
using System.Collections.Generic;
using LazyLib;
using LazyLib.Wow;

#endregion

namespace LazyEvo.LGrindEngine.Helpers
{
    internal class GrindingBlackList
    {
        internal static List<Location> BadNodes = new List<Location>();
        private static readonly Dictionary<string, DateTime> blacklist = new Dictionary<string, DateTime>();

        public static bool IsBlacklisted(PGameObject target)
        {
            if (target != null)
            {
                try
                {
                    if (IsBlacklisted(target.GUID + ""))
                        return true;
                    if (IsBlacklisted(target.Name))
                        return true;
                }
                catch (Exception e)
                {
                    Logging.Write("IsObjectBlacklisted: " + e);
                }
            }
            return false;
        }

        public static bool IsBlacklisted(string target)
        {
            try
            {
                if (target != null && blacklist.ContainsKey("GUID" + target))
                {
                    TimeSpan diff = blacklist["GUID" + target] - DateTime.Now;
                    if (diff.TotalSeconds > 0) return true;
                    Unblacklist(target);
                }
            }
            catch (Exception e)
            {
                Logging.Write("IsObjectBlacklisted: " + e);
            }
            return false;
        }

        public static void Blacklist(string target, uint length, bool writeText)
        {
            if (target == null) return;
            lock (blacklist)
            {
                blacklist["GUID" + target] = DateTime.Now.AddSeconds(length);
            }
            if (writeText)
                Logging.Write("Added GUID: '" + target + "' to bad list for " + length + " seconds");
        }

        public static void Unblacklist(string target)
        {
            lock (blacklist)
            {
                blacklist.Remove("GUID" + target);
            }
            Logging.Write("Removed: '" + target + " from badlist'");
        }

        public static void Blacklist(PObject target, uint length, bool writeText)
        {
            if (target == null)
                return;
            Blacklist(target.GUID + "", length, writeText);
        }

        public static void Unblacklist(PObject target)
        {
            if (target == null)
                return;
            Unblacklist(target.GUID + "");
        }
    }
}