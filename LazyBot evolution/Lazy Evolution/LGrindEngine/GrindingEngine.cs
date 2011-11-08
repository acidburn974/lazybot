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
using LazyEvo.LGrindEngine.Activity;
using LazyEvo.LGrindEngine.Helpers;
using LazyEvo.LGrindEngine.Radar;
using LazyEvo.LGrindEngine.States;
using LazyEvo.Public;
using LazyLib;
using LazyLib.FSM;
using LazyLib.IEngine;
using LazyLib.LazyRadar;
using LazyLib.LazyRadar.Drawer;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine
{
    public enum Mode
    {
        Normal = 1,
        TestNormal = 2,
        TestToTown = 3,
    }

    internal class GrindingEngine : ILazyEngine
    {
        internal static string OurDirectory;
        internal static PathProfile CurrentProfile;
        internal static GrindingNavigation Navigation;
        internal static GrindingNavigator Navigator;
        internal static Mode CurrentMode;
// ReSharper disable InconsistentNaming
// ReSharper restore InconsistentNaming
        private static Form _settings;
        private static Form _profile;
        private static int loots;
        private static int _kills;
        private static int _death;
        private static DateTime _startTime;
        private static int _xpCurrent;
        private static int _xpInitial;
        internal static bool ShouldTrain;
        private static List<MainState> _states { get; set; }

        #region ILazyEngine Members

        public string Name
        {
            get { return "Grinding Engine"; }
        }

        public List<MainState> States
        {
            get { return _states; }
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
                if (CurrentProfile == null)
                {
                    CurrentProfile = new PathProfile();
                }
                return new PathControl(CurrentProfile);
            }
        }

        public void Load()
        {
            GrindingSettings.LoadSettings();
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            if (!string.IsNullOrEmpty(GrindingSettings.Profile) && File.Exists(GrindingSettings.Profile))
            {
                CurrentProfile = new PathProfile();
                CurrentProfile.LoadNoDialog(GrindingSettings.Profile);
            }
            else
            {
                CurrentProfile = null;
                Logging.Write(LogType.Error, "Could not load a valid grinding profile");
            }
        }

        public bool EngineStart()
        {
            GrindingSettings.LoadSettings();
            if (!ObjectManager.InGame)
            {
                Logging.Write(LogType.Info, "Enter game before starting the bot");
                return false;
            }
            if (ObjectManager.MyPlayer.IsDead && ObjectManager.MyPlayer.IsGhost)
            {
                Logging.Write(LogType.Info, "Please ress before starting the bot");
                return false;
            }
            if (CurrentProfile == null)
            {
                Logging.Write(LogType.Info, "Please load a profile");
                return false;
            }
            Navigator = new GrindingNavigator();
            Navigation = new GrindingNavigation(CurrentProfile);
            GrindingSettings.LoadSettings();
            ToTown.SetToTown(false);
            switch (CurrentMode)
            {
                case Mode.TestToTown:
                    Logging.Write(LogType.Warning,
                                  "Starting Grinding engine in TestToTown mode, next start will be in normal mode THIS IS TODO");
                    //TODO
                    break;
                default:
                    _states = new List<MainState>
                                  {
                                      new StatePull(),
                                      new StateLoot(),
                                      new StateMoving(),
                                      new StateTrainer(),
                                      new StateResting(),
                                      new StateResurrect(),
                                      new StateCombat(),
                                      new StateToTown(),
                                      new StateVendor(),
                                  };
                    break;
            }
            Stuck.Run();
            CurrentMode = Mode.Normal;
            CombatHandler.CombatStatusChanged += CombatChanged;
            CloseWindows();
            loots = 0;
            _kills = 0;
            _death = 0;
            _xpInitial = ObjectManager.MyPlayer.Experience;
            _startTime = DateTime.Now;
            /*
            if (GrindingSettings.ShouldTrain)
            {
                ShouldTrain = GrindingShouldTrain.ShouldTrain();
            }*/
            PullController.Start();
            return true;
        }

        public void EngineStop()
        {
            PullController.Stop();
            Stuck.Stop();
            Navigator.Stop();
            Navigation = null;
            Navigator = null;
            _states = null;
            CombatHandler.CombatStatusChanged -= CombatChanged;
            GC.Collect();
        }

        public void Close()
        {
            Navigator.Stop();
            GrindingSettings.SaveSettings();
            CloseWindows();
        }

        public void Pause()
        {
            Navigator.Stop();
        }

        public void Resume()
        {
            Navigator.Start();
        }

        public List<IDrawItem> GetRadarDraw()
        {
            return new List<IDrawItem> {new DrawWaypoints()};
        }

        public List<IMouseClick> GetRadarClick()
        {
            return new List<IMouseClick>();
        }

        #endregion

        public bool LoadProfile(string path)
        {
            CurrentProfile.LoadNoDialog(path);
            return true;
        }

        public static void UpdateStats(int loot, int kills, int death)
        {
            loots += loot;
            _kills += kills;
            _death += death;
            TimeSpan duration = DateTime.Now - _startTime;
            string timeToLevel = string.Empty;
            double xpPerHourBySession = 0;
            _xpCurrent = ObjectManager.MyPlayer.Experience;
            if (_xpCurrent < _xpInitial)
            {
                Logging.Write("Ding!");
                _xpInitial = ObjectManager.MyPlayer.Experience;
                _xpCurrent = _xpInitial;
                _startTime = DateTime.Now;
                /*if (GrindingSettings.ShouldTrain)
                {
                    Navigator.Stop();
                    MoveHelper.ReleaseKeys();
                    ShouldTrain = false;
                    //ShouldTrain = GrindingShouldTrain.ShouldTrain();
                }*/
            }
            else
            {
                double xpGained = (_xpCurrent - _xpInitial);
                double time = (duration.Milliseconds); // hours
                if (time != 0.0)
                {
                    try
                    {
                        int toLevelXp = ObjectManager.MyPlayer.NextLevel - ObjectManager.MyPlayer.Experience;
                        double sessionTime = duration.TotalSeconds;
                        double seconds = (toLevelXp*sessionTime/xpGained);
                        xpPerHourBySession = Math.Round(xpGained/sessionTime*3600, 0);
                        TimeSpan t = TimeSpan.FromSeconds(seconds);

                        timeToLevel = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
                    }
                    catch
                    {
                        //Empty
                    }
                }
            }
            LazyForm.Loots = loots;
            LazyForm.Deaths = _death;
            LazyForm.Kills = _kills;
            LazyForm.TimeToLevel = timeToLevel;
            LazyForm.UpdateStatsText(string.Format("Loots:{0} -Kills:{1} -Deaths:{2} -XP/H: {3}-TTL:{4} ", loots, _kills,
                                                   _death, xpPerHourBySession, timeToLevel));
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

        private void CombatChanged(object o, GCombatEventArgs eventArgs)
        {
            if (eventArgs.CombatType.Equals(CombatType.CombatStarted))
            {
                Navigator.Stop();
            }
            if (eventArgs.CombatType.Equals(CombatType.CombatDone) && !ObjectManager.MyPlayer.IsDead)
            {
                LootAndSkin.DoLootAfterCombat(eventArgs.Unit);
            }
        }
    }
}