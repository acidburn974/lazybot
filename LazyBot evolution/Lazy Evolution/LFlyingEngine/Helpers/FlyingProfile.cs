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
using System.Globalization;
using System.Text;
using System.Xml;
using LazyLib;
using LazyLib.Wow;

#endregion

namespace LazyEvo.LFlyingEngine.Helpers
{
    /// <summary>
    ///   A profile
    /// </summary>
    internal class FlyingProfile
    {
        private List<Location> _badNodes = new List<Location>();
        private XmlDocument _doc;
        private List<Location> _toTownWaypoints = new List<Location>();
        private List<Location> _waypoints = new List<Location>();
        public string VendorName { get; set; }
        public bool NaturalRun { get; set; }

        public List<Location> GetBadNodes
        {
            get { return _badNodes; }
        }

        /// <summary>
        ///   Gets the waypoints.
        /// </summary>
        /// <value>The waypoints.</value>
        public List<Location> WaypointsNormal
        {
            get { return _waypoints; }
        }

        public List<Location> WaypointsToTown
        {
            get { return _toTownWaypoints; }
        }

        /// <summary>
        ///   Gets the profile.
        /// </summary>
        /// <value>The profile.</value>
        public string FileName { get; private set; }

        public int GetNearestWaypointIndex
        {
            get { return Location.GetClosestPositionInList(_waypoints, ObjectManager.MyPlayer.Location); }
        }

        public void AddBadNode(Location badNode)
        {
            if (!_badNodes.Contains(badNode))
            {
                _badNodes.Add(badNode);
            }
        }

        public void LoadDefault()
        {
            _waypoints = new List<Location>();
            _badNodes = new List<Location>();
            _toTownWaypoints = new List<Location>();
        }

        /// <summary>
        ///   Loads the file.
        /// </summary>
        /// <param name = "fileName">Name of the file.</param>
        public bool LoadFile(string fileName)
        {
            try
            {
                FileName = fileName;
                _doc = new XmlDocument();
                _doc.Load(fileName);
            }
            catch (Exception e)
            {
                Logging.Write("Error in loaded profile: " + e);
                return false;
            }

            _waypoints = new List<Location>();
            _badNodes = new List<Location>();
            _toTownWaypoints = new List<Location>();
            try
            {
                foreach (XmlNode rootNode in _doc.ChildNodes)
                {
                    foreach (XmlNode childNode in rootNode.ChildNodes)
                    {
                        switch (childNode.Name)
                        {
                            case XmlStruct.VendorName:
                                VendorName = childNode.InnerText;
                                break;
                            case XmlStruct.NaturalRun:
                                NaturalRun = Convert.ToBoolean(childNode.InnerText);
                                break;
                            case XmlStruct.Waypoint:
                                _waypoints.Add(GetCorrectLocation((childNode.InnerText)));
                                break;
                            case XmlStruct.ToTown:
                                _toTownWaypoints.Add(GetCorrectLocation((childNode.InnerText)));
                                break;
                            case XmlStruct.BadLocation:
                                AddBadNode(GetCorrectLocation((childNode.InnerText)));
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logging.Write("Error in loading profile " + e);
                return false;
            }
            if (_badNodes.Count == 0 && _waypoints.Count != 0)
            {
                //Lets check if the user used the badNodes.xml file at some point
                List<Location> badNodes = BadNodes.GetBadNodeList();
                foreach (Location badNode in badNodes)
                {
                    if (!_badNodes.Contains(badNode) && (_waypoints[0].DistanceFromXY(badNode) < 3000))
                    {
                        AddBadNode(badNode);
                    }
                }
            }
            return true;
        }

        /// <summary>
        ///   Adds the single way point.
        /// </summary>
        /// <param name = "position">The position.</param>
        public void AddSingleWayPoint(Location position)
        {
            _waypoints.Add(position);
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
            if (!string.IsNullOrEmpty(saveFile))
            {
                var xml = new StringBuilder();
                xml.AppendFormat(@"<?xml version=""1.0""?>");
                xml.AppendFormat("<{0}>", XmlStruct.Root);
                xml.AppendFormat("<{0}>{1}</{0}>", XmlStruct.VendorName, VendorName);
                xml.AppendFormat("<{0}>{1}</{0}>", XmlStruct.NaturalRun, NaturalRun);
                foreach (Location t in _waypoints)
                {
                    xml.AppendFormat("<{0}>{1}</{0}>", XmlStruct.Waypoint, GetCorrectString(t));
                }
                foreach (Location t in _toTownWaypoints)
                {
                    xml.AppendFormat("<{0}>{1}</{0}>", XmlStruct.ToTown, GetCorrectString(t));
                }
                foreach (Location t in _badNodes)
                {
                    xml.AppendFormat("<{0}>{1}</{0}>", XmlStruct.BadLocation, GetCorrectString(t));
                }
                xml.AppendFormat("</{0}>", XmlStruct.Root);
                var doc = new XmlDocument();
                doc.LoadXml(xml.ToString());
                doc.Save(saveFile);
            }
        }

        private static string GetCorrectString(Location t)
        {
            string temp = t.X + " " + t.Y + " " + t.Z;
            return temp.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
        }

        private static Location GetCorrectLocation(string location)
        {
            string[] split = location.Split(new[] {' '});
            if (split.Length > 2)
            {
                var wayPointPos = new Location((float) Convert.ToDouble(split[0], CultureInfo.InvariantCulture),
                                               (float) Convert.ToDouble(split[1], CultureInfo.InvariantCulture),
                                               (float) Convert.ToDouble(split[2], CultureInfo.InvariantCulture));
                return wayPointPos;
            }
            else
            {
                var wayPointPos = new Location((float) Convert.ToDouble(split[0], CultureInfo.InvariantCulture),
                                               (float) Convert.ToDouble(split[1], CultureInfo.InvariantCulture),
                                               (float) Convert.ToDouble((0)));
                return wayPointPos;
            }
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
            if (waypoints.Count == 0)
                return new List<Location>();
            var returnList = new List<Location>();

            if (waypoints.Count == 0)
                return returnList;

            Location closestPosition = waypoints[Location.GetClosestPositionInList(waypoints, location)];

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

        public void RemoveBadNode(Location location)
        {
            if (_badNodes.Contains(location))
            {
                _badNodes.Remove(location);
            }
        }

        #region Nested type: XmlStruct

        private struct XmlStruct
        {
            public const string NaturalRun = "NaturalRun";
            public const string Root = "Profile";
            public const string VendorName = "VendorName";
            public const string Waypoint = "Waypoint";
            public const string ToTown = "ToTown";
            public const string BadLocation = "BadLocation";
        }

        #endregion
    }
}