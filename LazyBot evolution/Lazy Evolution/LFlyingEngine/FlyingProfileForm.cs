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
using System.Windows.Forms;
using DevComponents.DotNetBar;
using LazyEvo.LFlyingEngine.Helpers;
using LazyLib;
using LazyLib.Wow;

#endregion

namespace LazyEvo.LFlyingEngine
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    internal partial class FlyingProfileForm : Office2007Form
    {
        private static volatile Dictionary<Thread, bool> _endThread = new Dictionary<Thread, bool>();
        private readonly Hotkey _normalKey = new Hotkey();
        private readonly Hotkey _vendorKey = new Hotkey();
        private bool _autoRecord;
        private string _profileToLoad = "none";
        private FlyingWaypointsType _waypointSelected;
        private Thread _waypointThread;

        public FlyingProfileForm()
        {
            _waypointThread = new Thread(WaypointThread);
            _waypointThread.Name = "WaypointThread";
            InitializeComponent();
            profileTypeComboBox.SelectedIndex = 0;
            try
            {
                _normalKey.KeyCode = Keys.Z;
                _normalKey.Alt = true;
                _normalKey.Windows = false;

                _vendorKey.KeyCode = Keys.C;
                _vendorKey.Alt = true;
                _vendorKey.Windows = false;

                _normalKey.Pressed += NormalKeyHotKeyPressed;
                _vendorKey.Pressed += VendorHotKeyPressed;
            }
            catch (Exception)
            {
            }
        }

        public static float ProgressBarCounter { get; private set; }

        private void FlyingProfileFormLoad(object sender, EventArgs e)
        {
            if (FlyingEngine.CurrentProfile != null)
            {
                UpdateControls();
            }
        }

        private void VendorHotKeyPressed(object sender, EventArgs e)
        {
            if (hotkeySwitchButton.Value)
            {
                FlyingEngine.CurrentProfile.AddSingleToTownWayPoint(ObjectManager.MyPlayer.Location);
                Logging.Write(LogType.Info,
                              "Added Vendor Waypoint at X: " + ObjectManager.MyPlayer.Location.X + " Y: " +
                              ObjectManager.MyPlayer.Location.Y + " Z: " + +ObjectManager.MyPlayer.Location.Z);
                UpdateWaypointsCount();
            }
        }

        private void NormalKeyHotKeyPressed(object sender, EventArgs e)
        {
            if (hotkeySwitchButton.Value)
            {
                FlyingEngine.CurrentProfile.AddSingleWayPoint(ObjectManager.MyPlayer.Location);
                Logging.Write(LogType.Info,
                              "Added Normal Waypoint at X: " + ObjectManager.MyPlayer.Location.X + " Y: " +
                              ObjectManager.MyPlayer.Location.Y + " Z: " +
                              +ObjectManager.MyPlayer.Location.Z);
                UpdateWaypointsCount();
            }
        }

        public static float UpdateProgressBar(int value)
        {
            return ProgressBarCounter = value;
        }

        public static float UpdateProgressBar(List<Location> waypoints, int positionInprofile)
        {
            return ProgressBarCounter = positionInprofile/(float) waypoints.Count*(float) 100.0;
        }

        public static float UpdateProgressBar(List<Location> waypoints, Location currentLocation)
        {
            bool found = false;
            double distance = 9999999;
            for (int i = 0; (i < waypoints.Count && !found); i++)
                if (waypoints[i].DistanceFrom(currentLocation) < distance)
                {
                    distance = waypoints[i].DistanceFrom(currentLocation);
                    ProgressBarCounter = (i/(float) waypoints.Count)*(float) 100.0;
                }
                else if (distance < waypoints[i].DistanceFrom(currentLocation))
                    found = true;
            return ProgressBarCounter;
        }


        private void RecordSwitchButtonValueChanged(object sender, EventArgs e)
        {
            _autoRecord = recordSwitchButton.Value;
            if (_autoRecord)
                StartThread(ref _waypointThread, WaypointThread, "Waypoint Thread", false);
            else
                StopThread(_waypointThread);
        }

        private void CreateProfileButtonClick(object sender, EventArgs e)
        {
            FlyingEngine.CurrentProfile = new FlyingProfile();
            _profileToLoad = "none";
            UpdateControls();
        }

        private void LoadProfileButtonClick(object sender, EventArgs e)
        {
            LoadProfile();
        }

        private void LoadProfile()
        {
            var dlg = new OpenFileDialog
                          {
                              InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                              Filter = @"Profiles (*.xml)|*.xml"
                          };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _profileToLoad = dlg.FileName;
                if (_profileToLoad.Contains(".xml"))
                {
                    FlyingEngine.CurrentProfile = new FlyingProfile();
                    FlyingEngine.CurrentProfile.LoadFile(_profileToLoad);
                    FlyingSettings.Profile = _profileToLoad;
                    FlyingSettings.SaveSettings();
                    UpdateControls();
                }
                else
                    Logging.Write(LogType.Warning, "Please select a valid profile type.");
            }
        }

        private void UpdateControls()
        {
            UpdateWaypointsCount();
            vendorNameBox.Text = FlyingEngine.CurrentProfile.VendorName;
            CBNaturalRun.Checked = FlyingEngine.CurrentProfile.NaturalRun;
        }

        private void SaveProfileButtonClick(object sender, EventArgs e)
        {
            SaveProfile();
        }

        private void SaveProfile()
        {
            var dlg = new SaveFileDialog
                          {
                              InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                              Filter = @"Profiles (*.xml)|*.xml"
                          };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                FlyingEngine.CurrentProfile.SaveFile(dlg.FileName);
                FlyingSettings.Profile = dlg.FileName;
                FlyingSettings.SaveSettings();
            }
        }

        private void UpdateWaypointsCount()
        {
            UpdateText(normalCountBox, Convert.ToString(FlyingEngine.CurrentProfile.WaypointsNormal.Count));
            UpdateText(toTownCountBox, Convert.ToString(FlyingEngine.CurrentProfile.WaypointsToTown.Count));
            if (FlyingEngine.CurrentProfile.VendorName != null)
                UpdateText(vendorNameBox, FlyingEngine.CurrentProfile.VendorName);
        }

        public void UpdateText(Control control, string text)
        {
            if (control.InvokeRequired)
                control.BeginInvoke(new MethodInvoker(() => UpdateText(control, text)));
            else
                control.Text = text;
        }

        private void VendorNameBoxTextChanged(object sender, EventArgs e)
        {
            if (vendorNameBox.Text.Trim().Length > 0)
                FlyingEngine.CurrentProfile.VendorName = vendorNameBox.Text;
        }

        private void DeleteNormalButtonClick(object sender, EventArgs e)
        {
            if (CheckProfile())
            {
                FlyingEngine.CurrentProfile.WaypointsNormal.Clear();
                UpdateWaypointsCount();
            }
        }

        private void AddNormalButtonClick(object sender, EventArgs e)
        {
            if (CheckProfile())
            {
                FlyingEngine.CurrentProfile.AddSingleWayPoint(ObjectManager.MyPlayer.Location);
                UpdateWaypointsCount();
            }
        }

        private bool CheckProfile()
        {
            if (FlyingEngine.CurrentProfile == null)
            {
                MessageBox.Show("Load or create a new profile");
                return false;
            }
            return true;
        }

        private void DeleteLastNormalButtonClick(object sender, EventArgs e)
        {
            if (CheckProfile())
            {
                if (FlyingEngine.CurrentProfile.WaypointsNormal.Count > 0)
                    FlyingEngine.CurrentProfile.WaypointsNormal.RemoveAt(
                        FlyingEngine.CurrentProfile.WaypointsNormal.Count - 1);
                UpdateWaypointsCount();
            }
        }

        private void DeleteToTownButtonClick(object sender, EventArgs e)
        {
            if (CheckProfile())
            {
                FlyingEngine.CurrentProfile.WaypointsToTown.Clear();
                UpdateWaypointsCount();
            }
        }

        private void AddToTownButtonClick(object sender, EventArgs e)
        {
            if (CheckProfile())
            {
                FlyingEngine.CurrentProfile.AddSingleToTownWayPoint(ObjectManager.MyPlayer.Location);
                UpdateWaypointsCount();
            }
        }

        private void DeleteLastToTownButtonClick(object sender, EventArgs e)
        {
            if (CheckProfile())
            {
                if (FlyingEngine.CurrentProfile.WaypointsToTown.Count > 0)
                    FlyingEngine.CurrentProfile.WaypointsToTown.RemoveAt(
                        FlyingEngine.CurrentProfile.WaypointsToTown.Count - 1);
                UpdateWaypointsCount();
            }
        }

        private void SetVendorNameButtonClick(object sender, EventArgs e)
        {
            vendorNameBox.Text = ObjectManager.MyPlayer.Target.Name;
        }

        private void WaypointThread()
        {
            if (FlyingEngine.CurrentProfile == null)
                FlyingEngine.CurrentProfile = new FlyingProfile();
            switch (_waypointSelected)
            {
                case FlyingWaypointsType.Normal:
                    FlyingEngine.CurrentProfile.AddSingleWayPoint(ObjectManager.MyPlayer.Location);
                    Logging.Write(LogType.Info,
                                  "Added Normal Waypoint at X: " + ObjectManager.MyPlayer.Location.X + " Y: " +
                                  ObjectManager.MyPlayer.Location.Y + " Z: " + +ObjectManager.MyPlayer.Location.Z);
                    break;
                case FlyingWaypointsType.ToTown:
                    FlyingEngine.CurrentProfile.AddSingleToTownWayPoint(ObjectManager.MyPlayer.Location);
                    Logging.Write(LogType.Info,
                                  "Added ToTown Waypoint at X: " + ObjectManager.MyPlayer.Location.X + " Y: " +
                                  ObjectManager.MyPlayer.Location.Y + " Z: " + +ObjectManager.MyPlayer.Location.Z);
                    break;
                default:
                    FlyingEngine.CurrentProfile.AddSingleWayPoint(ObjectManager.MyPlayer.Location);
                    Logging.Write(LogType.Info,
                                  "Added Normal Waypoint at X: " + ObjectManager.MyPlayer.Location.X + " Y: " +
                                  ObjectManager.MyPlayer.Location.Y + " Z: " + +ObjectManager.MyPlayer.Location.Z);
                    break;
            }
            UpdateWaypointsCount();
            Location old = ObjectManager.MyPlayer.Location;
            while (!EndThread(_waypointThread))
            {
                int dis = distanceInput.Value;

                if (ObjectManager.MyPlayer.Location.DistanceFromXY(old) > dis)
                {
                    switch (_waypointSelected)
                    {
                        case FlyingWaypointsType.Normal:
                            FlyingEngine.CurrentProfile.AddSingleWayPoint(ObjectManager.MyPlayer.Location);
                            Logging.Write(LogType.Info,
                                          "Added Normal Waypoint at X: " + ObjectManager.MyPlayer.Location.X + " Y: " +
                                          ObjectManager.MyPlayer.Location.Y + " Z: " +
                                          +ObjectManager.MyPlayer.Location.Z);
                            break;
                        case FlyingWaypointsType.ToTown:
                            FlyingEngine.CurrentProfile.AddSingleToTownWayPoint(ObjectManager.MyPlayer.Location);
                            Logging.Write(LogType.Info,
                                          "Added ToTown Waypoint at X: " + ObjectManager.MyPlayer.Location.X + " Y: " +
                                          ObjectManager.MyPlayer.Location.Y + " Z: " +
                                          +ObjectManager.MyPlayer.Location.Z);
                            break;
                        default:
                            FlyingEngine.CurrentProfile.AddSingleWayPoint(ObjectManager.MyPlayer.Location);
                            Logging.Write(LogType.Info,
                                          "Added Normal Waypoint at X: " + ObjectManager.MyPlayer.Location.X + " Y: " +
                                          ObjectManager.MyPlayer.Location.Y + " Z: " +
                                          +ObjectManager.MyPlayer.Location.Z);
                            break;
                    }
                    old = ObjectManager.MyPlayer.Location;
                    UpdateWaypointsCount();
                }
            }
        }

        private void ProfileTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            _waypointSelected = (FlyingWaypointsType) profileTypeComboBox.SelectedIndex;
        }

        private void GrindingProfileFormFormClosing(object sender, FormClosingEventArgs e)
        {
            TerminateThread(_waypointThread);
            if (_normalKey.Registered)
                _normalKey.Unregister();
            if (_vendorKey.Registered)
                _vendorKey.Unregister();
        }

        private void StartThread(ref Thread thread, Action work, string name, bool backgroundThread)
        {
            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    thread.Join();
                    GC.Collect();
                }
                thread = new Thread(new ThreadStart(work)) {Name = name, IsBackground = backgroundThread};
                thread.Start();
            }
        }

        private bool EndThread(Thread thread)
        {
            bool tmp = false;
            if (thread != null)
                _endThread.TryGetValue(thread, out tmp);
            return tmp;
        }

        private void StopThread(Thread target)
        {
            try
            {
                if (target == null) return;
                _endThread[target] = true;
                target.Join();
                _endThread[target] = false;
                GC.Collect();
            }
            catch (ThreadStateException)
            {
            }
        }

        private void TerminateThread(Thread thread)
        {
            try
            {
                if (thread == null)
                    return;
                thread.Abort();
                thread.Join();
                GC.Collect();
            }
            catch (ThreadStateException)
            {
            }
        }

        private void TestNormalButtonClick(object sender, EventArgs e)
        {
            FlyingEngine.CurrentMode = Mode.TestNormal;
            Logging.Write(LogType.Warning, "Set Flying mode to TestNormal, start the bot to test");
        }

        private void TestToTownButtonClick(object sender, EventArgs e)
        {
            FlyingEngine.CurrentMode = Mode.TestToTown;
            Logging.Write(LogType.Warning, "Set Flying mode to TestTown, start the bot to test");
        }

        private void BtnOnlineProfiles_Click(object sender, EventArgs e)
        {
            var flyingProfiles = new FlyingProfiles();
            flyingProfiles.Show();
            flyingProfiles.ProfileDownload += ProfileChanged;
        }

        private void ProfileChanged(object sender, EProfileDownloaded e)
        {
            string profileToLoad = e.Path;
            if (profileToLoad.Contains(".xml"))
            {
                FlyingEngine.CurrentProfile = new FlyingProfile();
                FlyingEngine.CurrentProfile.LoadFile(profileToLoad);
                FlyingSettings.Profile = profileToLoad;
                FlyingSettings.SaveSettings();
                UpdateControls();
            }
            else
            {
                MessageBox.Show("Could not load the downloaded profile, invalid profile type");
            }
        }

        private void CBNaturalRun_CheckedChanged(object sender, EventArgs e)
        {
            if (FlyingEngine.CurrentProfile != null)
            {
                FlyingEngine.CurrentProfile.NaturalRun = CBNaturalRun.Checked;
            }
        }

        private void loadProfileButton1_Click(object sender, EventArgs e)
        {
            LoadProfile();
        }

        private void saveProfileButton1_Click(object sender, EventArgs e)
        {
            SaveProfile();
        }
    }
}