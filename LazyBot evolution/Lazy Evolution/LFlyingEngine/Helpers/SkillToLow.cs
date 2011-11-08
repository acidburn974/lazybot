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
using LazyLib;

namespace LazyEvo.LFlyingEngine.Helpers
{
    public class SkillToLow
    {
        private static readonly Dictionary<string, DateTime> blacklist = new Dictionary<string, DateTime>();

        public static bool IsBlacklisted(string target)
        {
            try
            {
                if (blacklist.ContainsKey(target))
                {
                    TimeSpan diff = blacklist[target] - DateTime.Now;
                    if (diff.TotalSeconds > 0)
                    {
                        return true;
                    }
                    Unblacklist(target);
                }
                // Check();
            }
            catch (Exception e)
            {
                Logging.Write("IsObjectBlacklisted: " + e);
            }
            return false;
        }

        public static void Blacklist(string target, uint length)
        {
            try
            {
                if (!blacklist.ContainsKey(target))
                {
                    lock (blacklist)
                    {
                        blacklist[target] = DateTime.Now.AddSeconds(length);
                    }
                }
            }
            catch
            {
            }
        }

        private static void Unblacklist(string target)
        {
            lock (blacklist)
            {
                blacklist.Remove(target);
            }
        }
    }
}