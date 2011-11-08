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
using System.Linq;
using LazyLib;
using LazyLib.Wow;

#endregion

namespace LazyEvo.LFlyingEngine.Helpers
{
    internal class FlyingBlackList
    {
        internal static List<Location> BadNodes = new List<Location>();
        private static readonly Dictionary<ulong, DateTime> blacklist = new Dictionary<ulong, DateTime>();
        /*
        private static void Check()
        {
            try
            {
                lock (BadNodes)
                {
                    List<string> remove = (from node in blacklist let diff = node.Value - DateTime.Now where diff.TotalSeconds < 0 select node.Key).ToList();
                    foreach (var node in remove)
                    {
                        Unblacklist(node);
                    }
                }
            }
            catch (Exception e)
            {
                    
            }
        }
        */

        public static void Load()
        {
            BadNodes = FlyingEngine.CurrentProfile.GetBadNodes;
        }

        private static bool IsBadNode(PObject checkNode)
        {
            if (checkNode != null)
            {
                try
                {
                    lock (BadNodes)
                    {
                        if (BadNodes.Any(node => FindNode.GetLocation(checkNode).GetDistanceTo(node) < 5))
                            return true;
                    }
                }
                catch
                {
                }
            }
            return false;
        }

        public static void AddBadNode(PObject pObject)
        {
            AddBadNode(pObject.Location);
        }

        public static void AddBadNode(Location location)
        {
            FlyingEngine.CurrentProfile.AddBadNode(location);
            FlyingEngine.CurrentProfile.SaveFile(FlyingEngine.CurrentProfile.FileName);
            Load();
        }

        public static bool IsBlacklisted(PGameObject target)
        {
            if (target != null)
            {
                try
                {
                    if (IsBadNode(target))
                        return true;
                    if (IsBlacklisted(target.GUID))
                        return true;
                }
                catch (Exception e)
                {
                    Logging.Write("IsObjectBlacklisted: " + e);
                }
            }
            return false;
        }

        public static bool IsBlacklisted(ulong target)
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

        public static void Blacklist(ulong target, uint length, bool writeText)
        {
            try
            {
                if (!blacklist.ContainsKey(target))
                {
                    lock (blacklist)
                    {
                        blacklist[target] = DateTime.Now.AddSeconds(length);
                    }
                    // if (writeText)
                    //     Logging.Write("Added '" + target + "' to bad list for " + length + " seconds");
                }
            }
            catch
            {
            }
        }

        public static void Unblacklist(ulong target)
        {
            //Logging.Write("Removed: '" + target + " from badlist'");
            lock (blacklist)
            {
                blacklist.Remove(target);
            }
        }

        public static void Blacklist(PObject target, uint length, bool writeText)
        {
            Blacklist(target.GUID, length, writeText);
        }

        public static void Unblacklist(PObject target)
        {
            if (target == null)
                return;
            Unblacklist(target.GUID);
            FlyingEngine.CurrentProfile.RemoveBadNode(target.Location);
            FlyingEngine.CurrentProfile.SaveFile(FlyingEngine.CurrentProfile.FileName);
        }
    }
}