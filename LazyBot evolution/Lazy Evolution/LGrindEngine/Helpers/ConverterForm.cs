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
using System.Windows.Forms;
using System.Xml;
using DevComponents.DotNetBar;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.Helpers
{
    public partial class ConverterForm : Office2007Form
    {
        public List<uint> Factions = new List<uint>();
        private XmlDocument _doc;
        private string _folderSelected;
        private List<Location> _ghost = new List<Location>();
        private int _roamDistance;
        private List<Location> _toTownWaypoints = new List<Location>();
        private List<Location> _waypoints = new List<Location>();
        private string _xmlFile;

        public ConverterForm()
        {
            InitializeComponent();
        }

        private void ButtonX1Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog
                          {
                              InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                              Filter = @"Profiles (*.xml)|*.xml"
                          };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _xmlFile = dlg.FileName;
                if (!_xmlFile.Contains(".xml"))
                {
                    MessageBox.Show("Please select a valid profile type.");
                }
            }
        }

        private void ButtonX2Click(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                _folderSelected = folderBrowserDialog1.SelectedPath;
            }
        }

        private void ButtonX3Click(object sender, EventArgs h)
        {
            if (string.IsNullOrEmpty(_folderSelected))
            {
                MessageBox.Show("Please select a folder.");
                return;
            }
            if (string.IsNullOrEmpty(_xmlFile))
            {
                MessageBox.Show("Please select a profile to convert.");
                return;
            }
            _waypoints = new List<Location>();
            _ghost = new List<Location>();
            _toTownWaypoints = new List<Location>();
            try
            {
                _doc = new XmlDocument();
                _doc.Load(_xmlFile);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error in loaded profile: " + e);
            }
            try
            {
                foreach (XmlNode rootNode in _doc.ChildNodes)
                {
                    foreach (XmlNode childNode in rootNode.ChildNodes)
                    {
                        switch (childNode.Name)
                        {
                            case XmlStruct.Waypoint:
                                _waypoints.Add(GetCorrectLocation((childNode.InnerText)));
                                break;
                            case XmlStruct.ToTown:
                                _toTownWaypoints.Add(GetCorrectLocation((childNode.InnerText)));
                                break;
                            case XmlStruct.Ghost:
                                _ghost.Add(GetCorrectLocation((childNode.InnerText)));
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error in converting profile " + e);
            }
            try
            {
                XmlNodeList roamDistance = _doc.GetElementsByTagName("RoamDistance");
                _roamDistance = Convert.ToInt32(roamDistance[0].ChildNodes[0].Value);
            }
            catch
            {
            }
            if (_roamDistance == 0.0f)
                _roamDistance = 35;
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
            if (_waypoints.Count == 0)
            {
                MessageBox.Show("Profile should have more than 1 waypoint");
            }
            var pathProfile = new PathProfile();
            var subProfile = new SubProfile();
            for (int i = 0; i < _waypoints.Count; i++)
            {
                pathProfile.GetGraph.AddNodeNoConnection(_waypoints[i]);
                if (i == _waypoints.Count - 1)
                {
                    pathProfile.GetGraph.AddEdge(_waypoints[i], _waypoints[0]);
                }
                else
                {
                    pathProfile.GetGraph.AddNodeNoConnection(_waypoints[i + 1]);
                    pathProfile.GetGraph.AddEdge(_waypoints[i], _waypoints[i + 1]);
                }
                subProfile.Spots.Add(new Location(_waypoints[i].X - 2, _waypoints[i].Y - 2, _waypoints[i].Z));
            }
            for (int i = 0; i < _ghost.Count; i++)
            {
                pathProfile.GetGraph.AddNodeNoConnection(_ghost[i]);
                if (i == _ghost.Count - 1)
                {
                }
                else
                {
                    pathProfile.GetGraph.AddNodeNoConnection(_ghost[i + 1]);
                    pathProfile.GetGraph.AddEdge(_ghost[i], _ghost[i + 1]);
                }
            }
            for (int i = 0; i < _toTownWaypoints.Count; i++)
            {
                pathProfile.GetGraph.AddNodeNoConnection(_toTownWaypoints[i]);
                if (i == _toTownWaypoints.Count - 1)
                {
                }
                else
                {
                    pathProfile.GetGraph.AddNodeNoConnection(_toTownWaypoints[i + 1]);
                    pathProfile.GetGraph.AddEdge(_toTownWaypoints[i], _toTownWaypoints[i + 1]);
                }
            }
            if (_ghost.Count != 0)
            {
                pathProfile.GetGraph.AddEdge(_ghost[0], GetListSortedAfterDistance(_ghost[0], _waypoints)[0]);
            }
            if (_toTownWaypoints.Count != 0)
            {
                pathProfile.GetGraph.AddEdge(_toTownWaypoints[0],
                                             GetListSortedAfterDistance(_toTownWaypoints[0], _waypoints)[0]);
            }
            subProfile.SpotRoamDistance = _roamDistance;
            foreach (uint faction in Factions)
            {
                subProfile.Factions.Add(faction);
            }
            subProfile.MobMaxLevel = 99;
            subProfile.MobMinLevel = 0;
            subProfile.PlayerMaxLevel = 99;
            subProfile.PlayerMinLevel = 0;
            subProfile.Order = true;
            subProfile.Name = "Converted profile";
            pathProfile.AddSubProfile(subProfile);
            pathProfile.SaveProfile(_folderSelected + "\\Converted.xml");
            MessageBox.Show("Convertion done");
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

            IOrderedEnumerable<Location> orderedObjects = from p in waypoints orderby p.DistanceFrom(location) select p;
            return orderedObjects.ToList();
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

        #region Nested type: XmlStruct

        private struct XmlStruct
        {
            public const string Waypoint = "Waypoint";
            public const string ToTown = "Vendor";
            public const string Ghost = "GhostWaypoint";
        }

        #endregion
    }
}