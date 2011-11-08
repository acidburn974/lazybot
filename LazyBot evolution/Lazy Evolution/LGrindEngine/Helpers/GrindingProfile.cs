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
using System.Globalization;
using System.Linq;
using System.Xml;
using LazyLib;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.Helpers
{
    internal class GrindingProfile
    {
        public List<uint> Factions = new List<uint>();
        public List<String> Ignore = new List<string>();
        public bool NaturalRun = true;
        public Boolean Reverse;
        public int RoamDistance = 35;
        public int TargetMaxLevel = 200;
        public int TargetMinLevel;
        private XmlDocument _doc;
        private List<Location> _ghostWaypoints = new List<Location>();
        private List<Location> _toTownWaypoints = new List<Location>();
        private List<Location> _waypoints = new List<Location>();
        public string Profile { get; private set; }
        public string VendorName { get; set; }

        public List<Location> WaypointsGhost
        {
            get { return _ghostWaypoints; }
        }

        public List<Location> WaypointsNormal
        {
            get { return _waypoints; }
        }

        public List<Location> WaypointsToTown
        {
            get { return _toTownWaypoints; }
        }

        public int GetNearestNormalIndex
        {
            get { return Location.GetClosestPositionInList(_waypoints, ObjectManager.MyPlayer.Location); }
        }

        public int GetNearestGhostIndex
        {
            get { return Location.GetClosestPositionInList(_ghostWaypoints, ObjectManager.MyPlayer.Location); }
        }

        public int GetNearestToTownIndex
        {
            get { return Location.GetClosestPositionInList(_toTownWaypoints, ObjectManager.MyPlayer.Location); }
        }

        public void LoadDefault()
        {
            _waypoints = new List<Location>();
            _ghostWaypoints = new List<Location>();
            _toTownWaypoints = new List<Location>();
            TargetMaxLevel = 99;
            RoamDistance = 35;
            NaturalRun = true;
        }

        /// <summary>
        ///   Loads the file.
        /// </summary>
        /// <param name = "fileName">Name of the file.</param>
        public void LoadFile(string fileName)
        {
            try
            {
                Profile = fileName;
                _doc = new XmlDocument();
                _doc.Load(fileName);
            }
            catch (Exception e)
            {
                Logging.Write("Error in loaded profile: " + e);
            }

            _waypoints = new List<Location>();
            _ghostWaypoints = new List<Location>();
            try
            {
                XmlNodeList minLevel = _doc.GetElementsByTagName("MinLevel");
                TargetMinLevel = Convert.ToInt32(minLevel[0].ChildNodes[0].Value);
                XmlNodeList maxLevel = _doc.GetElementsByTagName("MaxLevel");
                TargetMaxLevel = Convert.ToInt32(maxLevel[0].ChildNodes[0].Value);
            }
            catch
            {
            }
            try
            {
                XmlNodeList vName = _doc.GetElementsByTagName("VendorName");
                VendorName = vName[0].ChildNodes[0].Value;
            }
            catch
            {
            }
            if (TargetMaxLevel == 0)
                TargetMaxLevel = 99;
            try
            {
                XmlNodeList faction = _doc.GetElementsByTagName("Factions");
                string temp = faction[0].InnerText;
                string[] split = temp.Split(new[] {' '});
                Factions.AddRange(from s in split where s != "" select Convert.ToUInt32(s));
            }
            catch
            {
            }

            try
            {
                XmlNodeList ignores = _doc.GetElementsByTagName("Ignore");
                string temp = ignores[0].InnerText;
                string[] split = temp.Split(new[] {'#'});
                Ignore.AddRange(from s in split where s != "" select s);
            }
            catch
            {
            }
            try
            {
                XmlNodeList roamDistance = _doc.GetElementsByTagName("RoamDistance");
                RoamDistance = Convert.ToInt32(roamDistance[0].ChildNodes[0].Value);
            }
            catch
            {
            }
            if (RoamDistance == 0.0f)
                RoamDistance = 35;
            try
            {
                XmlNodeList naturalRun = _doc.GetElementsByTagName("NaturalRun");
                NaturalRun = Convert.ToBoolean(naturalRun[0].ChildNodes[0].Value);
            }
            catch
            {
            }

            try
            {
                XmlNodeList vendor = _doc.GetElementsByTagName("GhostWaypoint");
                if (vendor.Count != 0)
                    foreach (XmlNode point in vendor)
                    {
                        string temp = point.ChildNodes[0].Value;

                        string correctString = temp;

                        if (Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) != ".")
                        {
                            correctString = temp.Replace(".",
                                                         CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                        }


                        string xyz = correctString;
                        string[] split = xyz.Split(new[] {' '});
                        if (split.Length > 2)
                        {
                            var wayPointPos = new Location((float) Convert.ToDouble((split[0])),
                                                           (float) Convert.ToDouble((split[1])),
                                                           (float) Convert.ToDouble((split[2])));
                            _ghostWaypoints.Add(wayPointPos);
                        }
                        else
                        {
                            var wayPointPos = new Location((float) Convert.ToDouble((split[0])),
                                                           (float) Convert.ToDouble((split[1])),
                                                           (float) Convert.ToDouble((0)));
                            _ghostWaypoints.Add(wayPointPos);
                        }
                    }
            }
            catch (Exception e)
            {
                Logging.Write("Error in ghost waypoints: " + e);
            }

            //Load Waypoints
            try
            {
                XmlNodeList waypoint = _doc.GetElementsByTagName("Waypoint");
                foreach (XmlNode point in waypoint)
                {
                    string temp = point.ChildNodes[0].Value;
                    string correctString = temp;

                    if (Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) != ".")
                    {
                        correctString = temp.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    }


                    string xyz = correctString;
                    string[] split = xyz.Split(new[] {' '});
                    if (split.Length > 2)
                    {
                        var wayPointPos = new Location((float) Convert.ToDouble((split[0])),
                                                       (float) Convert.ToDouble((split[1])),
                                                       (float) Convert.ToDouble((split[2])));
                        _waypoints.Add(wayPointPos);
                    }
                    else
                    {
                        var wayPointPos = new Location((float) Convert.ToDouble((split[0])),
                                                       (float) Convert.ToDouble((split[1])),
                                                       (float) Convert.ToDouble((0)));
                        _waypoints.Add(wayPointPos);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.Write("Error in loading waypoints " + e);
            }
            try
            {
                XmlNodeList waypoint = _doc.GetElementsByTagName("ToTown");
                foreach (XmlNode point in waypoint)
                {
                    string temp = point.ChildNodes[0].Value;
                    string correctString = temp;

                    if (Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) != ".")
                    {
                        correctString = temp.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    }


                    string xyz = correctString;
                    string[] split = xyz.Split(new[] {' '});
                    if (split.Length > 2)
                    {
                        var wayPointPos = new Location((float) Convert.ToDouble((split[0])),
                                                       (float) Convert.ToDouble((split[1])),
                                                       (float) Convert.ToDouble((split[2])));
                        _toTownWaypoints.Add(wayPointPos);
                    }
                    else
                    {
                        var wayPointPos = new Location((float) Convert.ToDouble((split[0])),
                                                       (float) Convert.ToDouble((split[1])),
                                                       (float) Convert.ToDouble((0)));
                        _toTownWaypoints.Add(wayPointPos);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.Write("Error in loading waypoints " + e);
            }
        }

        /// <summary>
        ///   Adds the single way point.
        /// </summary>
        /// <param name = "position">The position.</param>
        public void AddSingleWayPoint(Location position)
        {
            _waypoints.Add(position);
        }

        /// <summary>
        ///   Adds the single way point.
        /// </summary>
        /// <param name = "position">The position.</param>
        public void AddSingleGhostWayPoint(Location position)
        {
            _ghostWaypoints.Add(position);
        }

        public void AddSingleToTownWayPoint(Location position)
        {
            _toTownWaypoints.Add(position);
        }

        /// <summary>
        ///   Saves the file.
        /// </summary>
        public void SaveFile(string saveFile)
        {
            string xml = @"<?xml version=""1.0""?>";
            xml += "<Profile>";
            xml += "<MinLevel>" + TargetMinLevel + "</MinLevel>";
            xml += "<MaxLevel>" + TargetMaxLevel + "</MaxLevel>";
            xml += "<RoamDistance>" + RoamDistance + "</RoamDistance>";
            xml += "<NaturalRun>" + NaturalRun + "</NaturalRun>";
            xml += "<VendorName>" + VendorName + "</VendorName>";
            string correctString;
            string temp;
            xml += "<Factions>";
            int o = 0;
            foreach (uint t in Factions)
            {
                if (o == 0)
                    xml += t;
                else
                    xml += " " + t;
                o++;
            }
            xml += "</Factions>";
            xml += "<Ignore>";
            o = 0;
            foreach (string t in Ignore)
            {
                if (o == 0)
                    xml += t;
                else
                    xml += "#" + t;
                o++;
            }
            xml += "</Ignore>";
            foreach (Location t in _waypoints)
            {
                temp = t.X + " " + t.Y + " " + t.Z;
                correctString = Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) != "."
                                    ? temp.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".")
                                    : temp;
                xml += "<Waypoint>" + correctString + "</Waypoint>";
            }
            foreach (Location t in _toTownWaypoints)
            {
                temp = t.X + " " + t.Y + " " + t.Z;
                correctString = Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) != "."
                                    ? temp.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".")
                                    : temp;
                xml += "<ToTown>" + correctString + "</ToTown>";
            }
            foreach (Location t in _ghostWaypoints)
            {
                temp = t.X + " " + t.Y + " " + t.Z;
                correctString = Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) != "."
                                    ? temp.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".")
                                    : temp;
                xml += "<GhostWaypoint>" + correctString + "</GhostWaypoint>";
            }
            xml += "</Profile>";
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Save(saveFile);
        }

        /// <summary>
        /// Gets the list sorted after distance.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public List<Location> GetListSortedAfterDistance(Location location)
        {
            return GetListSortedAfterDistance(location, _waypoints);
        }

        /// <summary>
        /// Returns a list with Waypoints sorted after _distance to the location
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="waypoints">The waypoints</param>
        /// <returns>List of waypoints, sorted by current position.</returns>
        public List<Location> GetListSortedAfterDistance(Location location, List<Location> waypoints)
        {
            var returnList = new List<Location>();

            if (waypoints.Count == 0)
                return returnList;

            Location closestPosition =
                waypoints[Location.GetClosestPositionInList(_toTownWaypoints, ObjectManager.MyPlayer.Location)];

            int toPlaceInList = 0;

            // Find the closest waypoint to our current position.
            for (int i = 0; i < waypoints.Count; i++)
                if (closestPosition == waypoints[i])
                    toPlaceInList = i;

            // ReMake the list from our current position till the end.
            for (int i = toPlaceInList; i < waypoints.Count; i++)
                returnList.Add(waypoints[i]);

            // Add the start of the list till our current position.
            for (int i = 0; i < toPlaceInList; i++)
                returnList.Add(waypoints[i]);
            return returnList;
        }
    }
}