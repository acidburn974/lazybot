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
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.Helpers
{
    public class MarkedNode
    {
        private static readonly Dictionary<string, DateTime> IsMarkedDictionary = new Dictionary<string, DateTime>();

        internal static void MarkNode(PGameObject pGameObject)
        {
            HasToldAbout(pGameObject);
        }

        internal static bool IsMarked(PGameObject target)
        {
            if (target != null)
            {
                try
                {
                    if (HasTold(target.GUID + ""))
                        return true;
                }
                catch (Exception e)
                {
                    Logging.Write("Marked: " + e);
                }
            }
            return false;
        }

        private static bool HasTold(string target)
        {
            try
            {
                if (target != null && IsMarkedDictionary.ContainsKey("GUID" + target))
                {
                    TimeSpan diff = IsMarkedDictionary["GUID" + target] - DateTime.Now;
                    if (diff.TotalSeconds > 0) return true;
                    Unblacklist(target);
                }
            }
            catch (Exception e)
            {
                Logging.Write("Marked: " + e);
            }
            return false;
        }

        private static void Unblacklist(string target)
        {
            lock (IsMarkedDictionary)
            {
                IsMarkedDictionary.Remove("GUID" + target);
            }
        }

        private static void HasToldAbout(PObject target)
        {
            if (target == null)
                return;
            lock (IsMarkedDictionary)
            {
                IsMarkedDictionary["GUID" + target.GUID] = DateTime.Now.AddSeconds(30);
            }
        }
    }
}