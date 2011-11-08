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
using System.Reflection;
using System.Threading;
using LazyLib;
using LazyLib.Wow;

#endregion

namespace LazyEvo.Public
{
    /// <summary>
    ///   Combat blacklist.
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class PBlackList
    {
// ReSharper disable InconsistentNaming
        private static readonly Dictionary<string, DateTime> blacklist = new Dictionary<string, DateTime>();
// ReSharper restore InconsistentNaming

        /// <summary>
        ///   Determines whether the specified target is blacklisted.
        /// </summary>
        /// <param name = "target">The target.</param>
        /// <returns>
        ///   <c>true</c> if the specified target is blacklisted; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBlacklisted(PUnit target)
        {
            try
            {
                lock (blacklist)
                {
                    if (target != null && blacklist.ContainsKey("GUID" + target.GUID))
                    {
                        TimeSpan diff = blacklist["GUID" + target.GUID] - DateTime.Now;
                        if (diff.TotalSeconds > 0) return true;
                        Unblacklist(target);
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception e)
            {
                Logging.Write("PBlackList: " + e);
            }
            return false;
        }

        /// <summary>
        ///   Blacklists the specified target.
        /// </summary>
        /// <param name = "target">The target.</param>
        /// <param name = "length">The length.</param>
        /// <param name = "writeText">if set to <c>true</c> [write text].</param>
        public static void Blacklist(PUnit target, uint length, bool writeText)
        {
            if (target == null) return;
            lock (blacklist)
            {
                blacklist["GUID" + target.GUID] = DateTime.Now.AddSeconds(length);
            }
            if (writeText)
                Logging.Write("Added GUID: '" + target.GUID + "' to bad list for " + length + " seconds");
        }

        public static void Blacklist(uint guid, uint length, bool writeText)
        {
            lock (blacklist)
            {
                blacklist["GUID" + guid] = DateTime.Now.AddSeconds(length);
            }
            if (writeText)
                Logging.Write("Added GUID: '" + guid + "' to bad list for " + length + " seconds");
        }

        /// <summary>
        ///   Unblacklists the specified target.
        /// </summary>
        /// <param name = "target">The target.</param>
        public static void Unblacklist(PUnit target)
        {
            if (target == null) return;
            lock (blacklist)
            {
                blacklist.Remove("GUID" + target.GUID);
            }
            Logging.Write("Removed: '" + target.GUID + " from badlist'");
        }
    }
}