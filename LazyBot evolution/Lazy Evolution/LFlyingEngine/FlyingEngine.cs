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
using System.IO;
using System.Windows.Forms;
using LazyEvo.LFlyingEngine.Activity;
using LazyEvo.LFlyingEngine.Helpers;
using LazyEvo.LFlyingEngine.Radar;
using LazyEvo.LFlyingEngine.States;
using LazyEvo.Public;
using LazyLib;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.IEngine;
using LazyLib.LazyRadar;
using LazyLib.LazyRadar.Drawer;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine
{
    public enum Mode
    {
        Normal = 1,
        TestNormal = 2,
        TestToTown = 3,
    }

    internal class FlyingEngine : ILazyEngine
    {
        internal static string OurDirectory;
        internal static FlyingProfile CurrentProfile;
        internal static FlyingNavigation Navigation;
        internal static FlyingNavigator Navigator;
        internal static Mode CurrentMode;
        private static Form _settings;
        private static Form _profile;
        private static DateTime _startTime;
        private static int _harvest;
        private static int _kills;
        private static int _death;
        private bool _naviRunning;
        private static List<MainState> FlyingStates { get; set; }

        #region ILazyEngine Members

        public string Name
        {
            get { return "Flying Engine"; }
        }

        public List<MainState> States
        {
            get { return FlyingStates; }
        }

        public Form Settings
        {
            get
            {
                _settings = new Settings();
                return _settings;
            }
        }

        public Form ProfileForm
        {
            get
            {
                _profile = new FlyingProfileForm();
                return _profile;
            }
        }

        public void Load()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            FlyingSettings.LoadSettings();
            CurrentMode = Mode.Normal;
            if (!string.IsNullOrEmpty(FlyingSettings.Profile) && File.Exists(FlyingSettings.Profile))
            {
                CurrentProfile = new FlyingProfile();
                CurrentProfile.LoadFile(FlyingSettings.Profile);
            }
            else
            {
                CurrentProfile = null;
                Logging.Write("Could not load a valid flying profile");
            }
        }

        public bool EngineStart()
        {
            FindNode.LoadHarvest();
            FlyingSettings.LoadSettings();
            KeyHelper.AddKey("FMount", "None", FlyingSettings.FlyingMountBar, FlyingSettings.FlyingMountKey);
            KeyHelper.AddKey("Lure", "None", FlyingSettings.LureBar, FlyingSettings.LureKey);
            KeyHelper.AddKey("Waterwalk", "None", FlyingSettings.WaterwalkBar, FlyingSettings.WaterwalkKey);
            KeyHelper.AddKey("CombatStart", "None", FlyingSettings.ExtraBar, FlyingSettings.ExtraKey);
            if (!ObjectManager.InGame)
            {
                Logging.Write(LogType.Info, "Enter game before starting the bot");
                return false;
            }
            if (ObjectManager.MyPlayer.IsGhost)
            {
                Logging.Write(LogType.Info, "Please ress before starting the bot");
                return false;
            }
            if (CurrentProfile == null)
            {
                Logging.Write(LogType.Info, "Please load a profile");
                return false;
            }
            if (CurrentProfile.WaypointsNormal.Count < 2)
            {
                Logging.Write(LogType.Info, "Profile should have more than 2 waypoints");
                return false;
            }
            Navigation = new FlyingNavigation(CurrentProfile.WaypointsNormal, true, FlyingWaypointsType.Normal);
            Navigator = new FlyingNavigator();
            ToTown.SetToTown(false);
            switch (CurrentMode)
            {
                case Mode.Normal:
                    FlyingStates = new List<MainState>
                                       {
                                           new StateMount(),
                                           new StateMoving(),
                                           new StateGather(),
                                           new StateCombat(),
                                           new StateRess(),
                                           new StateResting(),
                                           new StateMailbox(),
                                           new StateToTown(),
                                           new StateVendor(),
                                           new StateFullBags(),
                                       };
                    break;
                case Mode.TestNormal:
                    Logging.Write(LogType.Warning,
                                  "Starting flying engine in TestNormal mode, next start will be in normal mode");
                    FlyingStates = new List<MainState>
                                       {
                                           new StateMount(),
                                           new StateMoving(),
                                           new StateFullBags(),
                                       };
                    break;
                case Mode.TestToTown:
                    Logging.Write(LogType.Warning,
                                  "Starting flying engine in TestToTown mode, next start will be in normal mode");
                    FlyingStates = new List<MainState>
                                       {
                                           new StateMount(),
                                           new StateMoving(),
                                           new StateCombat(),
                                           new StateMailbox(),
                                           new StateToTown(),
                                           new StateVendor(),
                                           new StateFullBags(),
                                       };
                    ToTown.SetToTown(true); //Set town mode to true
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Stuck.Run();
            FlyingBlackList.Load();
            CloseWindows();
            CurrentMode = Mode.Normal;
            _harvest = 0;
            _kills = 0;
            _death = 0;
            _startTime = DateTime.Now;
            UpdateStats(0, 0, 0);
            return true;
        }

        public void EngineStop()
        {
            Stuck.Stop();
            Navigator.Stop();
        }

        public void Close()
        {
            FlyingSettings.SaveSettings();
            Navigator.Stop();
            CloseWindows();
        }

        public void Pause()
        {
            if (Navigator.IsRunning)
            {
                _naviRunning = true;
                Navigator.Stop();
            }
        }

        public void Resume()
        {
            if (_naviRunning)
            {
                Navigator.Start();
                _naviRunning = false;
            }
        }

        public List<IDrawItem> GetRadarDraw()
        {
            return new List<IDrawItem> {new DrawNodes(), new DrawWaypoints()};
        }

        public List<IMouseClick> GetRadarClick()
        {
            return new List<IMouseClick> {new MouseHandler()};
        }

        #endregion

        public static void UpdateStats(int harvest, int kills, int death)
        {
            _harvest += harvest;
            _kills += kills;
            _death += death;
            TimeSpan duration = DateTime.Now - _startTime;
            double time = (duration.Milliseconds); // hours
            string harvestHours = string.Empty;
            if (time != 0.0)
            {
                double sessionTime = duration.TotalSeconds;
                harvestHours = Math.Round(_harvest/sessionTime*3600, 2).ToString();
            }
            LazyForm.Deaths = _death;
            LazyForm.Kills = _kills;
            LazyForm.Harvests = _harvest;
            LazyForm.LPH = harvestHours;
            LazyForm.UpdateStatsText(string.Format("Loots: {0} - Kills: {1} - Deaths: {2} - Harvests/Hour: {3}",
                                                   _harvest, _kills, _death, harvestHours));
        }

        public void UpdateState(string text)
        {
            LazyForm.UpdateStatsText(text);
        }

        private static void CloseWindows()
        {
            if (_profile != null && !_profile.IsDisposed)
            {
                _profile.Close();
            }
            if (_settings != null && !_settings.IsDisposed)
            {
                _settings.Close();
            }
        }

        public bool LoadProfile(string path)
        {
            return CurrentProfile.LoadFile(path);
        }
    }
}