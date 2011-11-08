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
using System.IO;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using System.Xml;
using LazyEvo.LGrindEngine.Helpers;
using LazyEvo.LGrindEngine.NpcClasses;
using LazyEvo.Public;
using LazyLib;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine
{
    internal class PathProfile
    {
        private readonly QuickGraph _starNode;
        private PathControl _pathControl;
        private string _pathName;
        private List<SubProfile> _subProfile;

        public PathProfile()
        {
            _starNode = new QuickGraph();
            _subProfile = new List<SubProfile>();
            TrainList = new List<Train>();
            NpcController = new NpcController();
        }

        internal NpcController NpcController { get; set; }
        internal List<Train> TrainList { get; private set; }

        internal QuickGraph GetGraph
        {
            get { return _starNode; }
        }

        internal List<SubProfile> GetSubProfiles
        {
            get { return _subProfile; }
        }

        internal void AddTrain(Train add)
        {
            TrainList.Add(add);
        }

        internal SubProfile GetSubProfile()
        {
            foreach (SubProfile subProfile in _subProfile)
            {
                if (ObjectManager.MyPlayer.Level <= subProfile.PlayerMaxLevel &&
                    ObjectManager.MyPlayer.Level >= subProfile.PlayerMinLevel)
                {
                    return subProfile;
                }
            }
            LazyHelpers.StopAll("No more subprofiles"); //TODO!
            return new SubProfile();
        }

        public void New()
        {
            _subProfile.Clear();
            _starNode.New();
            NpcController = new NpcController();
        }

        public Location GetClosest(Location loc)
        {
            return _starNode.GetClosest(loc);
        }

        public void Open()
        {
            if (_pathControl == null || _pathControl.IsDisposed)
            {
                _pathControl = new PathControl(this);
                _pathControl.Start();
            }
        }

        internal void AddSubProfile(SubProfile profile)
        {
            if (!_subProfile.Contains(profile))
                _subProfile.Add(profile);
        }

        internal void ClearSubProfile()
        {
            _subProfile.Clear();
        }

        public List<Location> FindShortsPath(Location start, Location end)
        {
            return _starNode.FindPath(start, end);
        }

        public void Save()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "File (*.xml)|*.xml";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveProfile(saveFileDialog.FileName);
            }
        }

        internal void SaveProfile(string fileName)
        {
            string xml = @"<?xml version=""1.0""?>";
            xml += "<LazyProfile>";
            xml += "<PathName>" + Path.GetFileName(fileName) + "</PathName>";
            xml += "<Vendors>";
            foreach (VendorsEx npc in NpcController.Npc)
            {
                string temp = string.Format("X=\"{0}\" Y=\"{1}\" Z=\"{2}\"", npc.Location.X, npc.Location.Y,
                                            npc.Location.Z);
                string correctString =
                    Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) != "."
                        ? temp.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".")
                        : temp;
                if (npc.VendorType == VendorType.Train)
                {
                    xml += string.Format("<Vendor Name=\"{0}\" Type=\"{1}\" TrainClass=\"{2}\" EntryId=\"{3}\" {4} />",
                                         SecurityElement.Escape(npc.Name), npc.VendorType, npc.TrainClass, npc.EntryId,
                                         correctString);
                }
                else
                {
                    xml += string.Format("<Vendor Name=\"{0}\" Type=\"{1}\" EntryId=\"{2}\" {3} />",
                                         SecurityElement.Escape(npc.Name), npc.VendorType, npc.EntryId, correctString);
                }
            }
            xml += "</Vendors>";
            foreach (SubProfile subProfile in _subProfile)
            {
                xml += "<SubProfile>";
                xml += "<Name>" + subProfile.Name + "</Name>";
                xml += "<MinLevel>" + subProfile.PlayerMinLevel + "</MinLevel>";
                xml += "<MaxLevel>" + subProfile.PlayerMaxLevel + "</MaxLevel>";
                xml += "<MobMinLevel>" + subProfile.MobMinLevel + "</MobMinLevel>";
                xml += "<MobMaxLevel>" + subProfile.MobMaxLevel + "</MobMaxLevel>";
                xml += "<SpotRoamDistance>" + subProfile.SpotRoamDistance + "</SpotRoamDistance>";
                xml += "<Order>" + subProfile.Order + "</Order>";
                xml += "<Factions>";
                int o = 0;
                foreach (uint t in subProfile.Factions)
                {
                    if (o == 0)
                        xml += t;
                    else
                        xml += " " + t;
                    o++;
                }
                xml += "</Factions>";
                xml += "<Ignores>";
                o = 0;
                foreach (string t in subProfile.Ignore)
                {
                    if (o == 0)
                        xml += t;
                    else
                        xml += "|" + t;
                    o++;
                }
                xml += "</Ignores>";
                foreach (Location t in subProfile.Spots)
                {
                    string temp = t.X + " " + t.Y + " " + t.Z;
                    string correctString = "";
                    if (Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) != ".")
                    {
                        correctString = temp.Replace(
                            CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
                    }
                    else
                    {
                        correctString = temp;
                    }
                    xml += "<Spot>" + correctString + "</Spot>";
                }
                xml += "</SubProfile>";
            }
            xml += "</LazyProfile>";
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Save(fileName);
            _starNode.SaveGraph(fileName + ".path");
        }

        public void Load()
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "File (*.xml)|*.xml";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string profileToLoad = dlg.FileName;
                if (profileToLoad.Contains(".xml"))
                {
                    GrindingSettings.Profile = profileToLoad;
                    GrindingSettings.SaveSettings();
                    LoadNoDialog(profileToLoad);
                }
            }
        }

        public bool LoadNoDialog(string profileToLoad)
        {
            try
            {
                _subProfile = new List<SubProfile>();
                TrainList = new List<Train>();
                NpcController = new NpcController();
                if (!File.Exists(profileToLoad))
                {
                    Logging.Write("Could not find file: " + profileToLoad);
                    return false;
                }
                // Settings.SaveProfile("GraphProfile", profileToLoad); //TODO
                XmlDocument doc;
                try
                {
                    doc = new XmlDocument();
                    doc.Load(profileToLoad);
                }
                catch (Exception e)
                {
                    Logging.Write("Error in loaded profile: " + e);
                    return false;
                }
                if (doc.GetElementsByTagName("LazyProfile")[0] == null)
                {
                    MessageBox.Show("The profile you tried to load is not a valid profile for this engine");
                    return false;
                }
                LoadVendorsEx(doc);
                LoadSubProfile(doc);
                LoadVendorOld(doc);
                LoadPath(profileToLoad, doc);
            }
            catch (Exception e)
            {
                Logging.Write("Exception when loading profile: " + e);
                return false;
            }
            return true;
        }

        private void LoadVendorsEx(XmlDocument doc)
        {
            try
            {
                XmlNodeList vendors = doc.GetElementsByTagName("Vendors");
                if (vendors.Count != 0)
                {
                    NpcController.LoadXml(vendors[0]);
                }
            }
            catch (Exception e)
            {
                Logging.Write("Could not load Vendors: " + e);
            }
        }

        private void LoadPath(string profileToLoad, XmlDocument doc)
        {
            XmlNodeList pName = doc.GetElementsByTagName("PathName");
            _pathName = pName[0].ChildNodes[0].Value;
            string pathLoc = Path.GetDirectoryName(profileToLoad) + Path.DirectorySeparatorChar + _pathName + ".path";
            if (File.Exists(profileToLoad + ".path"))
            {
                _starNode.LoadGraph(profileToLoad + ".path");
                return;
            }
            if (File.Exists(profileToLoad.Replace(".xml", string.Empty) + ".path"))
            {
                _starNode.LoadGraph(profileToLoad + ".path");
                return;
            }
            if (File.Exists(pathLoc))
            {
                _starNode.LoadGraph(pathLoc);
            }
            else
            {
                Logging.Write("Could not finde: " + pathLoc + " no path loaded");
            }
        }

        private void LoadSubProfile(XmlDocument doc)
        {
            XmlNodeList subProfiles = doc.GetElementsByTagName("SubProfile");
            foreach (XmlNode subProfile in subProfiles)
            {
                var sub = new SubProfile();
                foreach (XmlNode child in subProfile.ChildNodes)
                {
                    if (child.Name.Equals("Name"))
                        sub.Name = child.InnerText;
                    if (child.Name.Equals("MinLevel"))
                        sub.PlayerMinLevel = Convert.ToInt32(child.InnerText);
                    if (child.Name.Equals("MaxLevel"))
                        sub.PlayerMaxLevel = Convert.ToInt32(child.InnerText);
                    if (child.Name.Equals("MobMinLevel"))
                        sub.MobMinLevel = Convert.ToInt32(child.InnerText);
                    if (child.Name.Equals("MobMaxLevel"))
                        sub.MobMaxLevel = Convert.ToInt32(child.InnerText);
                    if (child.Name.Equals("SpotRoamDistance"))
                        sub.SpotRoamDistance = Convert.ToInt32(child.InnerText);
                    if (child.Name.Equals("Order"))
                        sub.Order = Convert.ToBoolean(child.InnerText);
                    if (child.Name.Equals("Factions"))
                    {
                        string temp = child.InnerText;
                        string[] split = temp.Split(new[] {' '});
                        sub.Factions.AddRange(from s in split where s != "" select Convert.ToUInt32(s));
                    }
                    if (child.Name.Equals("Ignores"))
                    {
                        string temp = child.InnerText;
                        string[] split = temp.Split(new[] {'|'});
                        sub.Ignore.AddRange(from s in split where s != "" select s);
                    }
                    if (child.Name.Equals("Spot"))
                    {
                        string temp = child.InnerText;
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
                            sub.Spots.Add(wayPointPos);
                        }
                        else
                        {
                            var wayPointPos = new Location((float) Convert.ToDouble((split[0])),
                                                           (float) Convert.ToDouble((split[1])),
                                                           (float) Convert.ToDouble((0)));
                            sub.Spots.Add(wayPointPos);
                        }
                    }
                    if (child.Name.Equals("Vendor"))
                    {
                        var v = new Vendor();
                        v.Load(child);
                        NpcController.AddNpc(new VendorsEx(VendorType.Repair, v.Name, v.Spot, int.MinValue));
                    }
                }
                _subProfile.Add(sub);
            }
        }


        private void LoadVendorOld(XmlDocument doc)
        {
            try
            {
                XmlNodeList traList = doc.GetElementsByTagName("Vendor");
                foreach (XmlNode tra in traList)
                {
                    var v = new Vendor();
                    v.Load(tra);
                    if (!string.IsNullOrEmpty(v.Name) && v.Spot != null)
                    {
                        NpcController.AddNpc(new VendorsEx(VendorType.Repair, v.Name, v.Spot, int.MinValue));
                    }
                }
            }
            catch
            {
            }
        }
    }
}