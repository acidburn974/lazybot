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
using LazyLib.Helpers;
using LazyLib.Wow;

#endregion

namespace LazyEvo.LFlyingEngine.Helpers
{
    internal class FindNode
    {
        private static List<string> _mines = new List<string>();
        private static List<string> _herbs = new List<string>();
        private static IEnumerable<PGameObject> _cached = new List<PGameObject>();
        private static readonly Ticker ReloadCache = new Ticker(1200);
        //private static List<string> _schools = new List<string>();
        public static void LoadHarvest()
        {
            Mine.Load();
            Herb.Load();
            //School.Load();
            _mines = Mine.GetList();
            _herbs = Herb.GetList();
            //_schools = School.GetList();
            Logging.Debug("Mines: " + _mines.Count + " - Herbs: " + _herbs.Count);
        }

        public static bool IsHerb(PObject node)
        {
            switch (node.Type)
            {
                case (int) Constants.ObjectType.GameObject:
                    if (_herbs.Contains(((PGameObject) node).Name))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        public static bool IsMine(PObject node)
        {
            switch (node.Type)
            {
                case (int) Constants.ObjectType.GameObject:
                    if (_mines.Contains(((PGameObject) node).Name))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        public static bool IsSchool(PObject node)
        {
            switch (node.Type)
            {
                case (int) Constants.ObjectType.GameObject:
                    if (((PGameObject) node).GameObjectType == 25)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        public static string GetName(PObject node)
        {
            switch (node.Type)
            {
                case (int) Constants.ObjectType.Unit:
                    return ((PUnit) node).Name;
                case (int) Constants.ObjectType.GameObject:
                    return ((PGameObject) node).Name;
            }
            return "";
        }

        public static Location GetLocation(PObject node)
        {
            switch (node.Type)
            {
                case (int) Constants.ObjectType.GameObject:
                    return (node).Location;
                case (int) Constants.ObjectType.Unit:
                    return (node).Location;
            }
            return new Location(0, 0, 0);
        }

        public static PGameObject SearchForNode()
        {
            try
            {
                if (ReloadCache.IsReady)
                {
                    _cached = from u in ObjectManager.GetGameObject
                              where
                                  (((_herbs.Contains(u.Name) && FlyingSettings.Herb) ||
                                    (_mines.Contains(u.Name) && FlyingSettings.Mine) ||
                                    (FlyingSettings.Fish && u.GameObjectType == 25 &&
                                     u.Location.DistanceToSelf2D < FlyingSettings.FishApproach)) &&
                                   !FlyingBlackList.IsBlacklisted(u) && !SkillToLow.IsBlacklisted(u.Name) &&
                                   !LootedBlacklist.IsLooted(u))
                              select u;
                    ReloadCache.Reset();
                }
                if (FlyingSettings.Fish)
                {
                    PGameObject school = (from h in _cached where h.GameObjectType == 25 select h).FirstOrDefault();
                    if (school != null)
                    {
                        return school;
                    }
                }
                IOrderedEnumerable<PGameObject> orderedObjects = from p in _cached
                                                                 orderby p.Location.DistanceToSelf
                                                                 select p;
                if (orderedObjects.Count() > 0)
                {
                    PGameObject searchForNode = orderedObjects.ToList()[0];
                    if (!MarkedNode.IsMarked(searchForNode))
                    {
                        Logging.Write(LogType.Info, "Found possible node: {0} : {1}", searchForNode.Name,
                                      searchForNode.GUID);
                        MarkedNode.MarkNode(searchForNode);
                    }
                    return searchForNode;
                }
            }
            catch (Exception e)
            {
                //Logging.Write("SearchForNode: " + e);
            }
            return null;
        }
    }
}