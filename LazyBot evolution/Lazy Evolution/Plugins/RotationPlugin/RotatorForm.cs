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
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using LazyEvo.Forms.Helpers;
using LazyEvo.PVEBehavior;
using LazyEvo.PVEBehavior.Behavior;
using LazyEvo.PVEBehavior.Behavior.Conditions;
using LazyLib;
using LazyLib.ActionBar;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.Plugins.RotationPlugin
{
    internal partial class RotatorForm : Office2007Form
    {
        private readonly List<Hotkey> _hotKeys = new List<Hotkey>();
        internal string OurDirectory;
        internal RotationManagerController RotationManagerController;
        private bool _firstTime = true;
        private Rotation _rotation;
        private Thread _rotationThread;
        private RotatorStatus status;

        public RotatorForm()
        {
            InitializeComponent();
            PveBehaviorSettings.LoadSettings();
            Geometry.GeometryFromString(GeomertrySettings.RotatorForm, this);
        }

        private void RotatorFormLoad(object sender, EventArgs e)
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            RotationSettings.LoadSettings();
            if (RotationManagerController == null)
            {
                if (File.Exists(OurDirectory + "\\Rotations\\" + RotationSettings.LoadedRotationManager + ".xml"))
                {
                    RotationManagerController = new RotationManagerController();
                    RotationManagerController.Load(OurDirectory + "\\Rotations\\" +
                                                   RotationSettings.LoadedRotationManager + ".xml");
                }
                else
                {
                    Logging.Write("Could not load the rotation manager");
                    RotationManagerController = null;
                }
            }
        }

        private void CbOpenRotationManagerClick(object sender, EventArgs e)
        {
            Hide();
            var form = new RotationManagerForm(RotationManagerController);
            form.Show();
            form.Closed += ShowAgain;
        }

        private void ShowAgain(object sender, EventArgs e)
        {
            Show();
        }

        private void StartMonitoringCheckedChanged(object sender, EventArgs e)
        {
            CBOpenRotationManager.Enabled = !StartMonitoring.Checked;
            if (StartMonitoring.Checked)
            {
                if (!File.Exists(OurDirectory + "\\Rotations\\" + RotationSettings.LoadedRotationManager + ".xml"))
                {
                    Logging.Write(LogType.Error, "No rotation loaded");
                    StartMonitoring.Checked = false;
                    return;
                }
                if (!ObjectManager.InGame)
                {
                    Logging.Write(LogType.Error, "Please enter the game");
                    StartMonitoring.Checked = false;
                    return;
                }
                Start();
            }
            else
            {
                Stop();
                Logging.Write(LogType.Info, "Stopped rotator");
            }
        }

        private void Start()
        {
            if (_firstTime)
            {
                LoadFirstTime();
                _firstTime = false;
            }
            if (File.Exists(OurDirectory + "\\Rotations\\" + RotationSettings.LoadedRotationManager + ".xml"))
            {
                RotationManagerController = new RotationManagerController();
                RotationManagerController.Load(OurDirectory + "\\Rotations\\" + RotationSettings.LoadedRotationManager +
                                               ".xml");
            }
            foreach (Rotation rotation in RotationManagerController.Rotations.Where(r => r.Active))
            {
                try
                {
                    CheckBuffAndKeys(rotation.Rules.GetRules);
                    var hotkey = new Hotkey();
                    hotkey.Windows = rotation.Windows;
                    hotkey.Shift = rotation.Shift;
                    hotkey.Alt = rotation.Alt;
                    hotkey.Control = rotation.Ctrl;
                    Rotation rotation1 = rotation;
                    hotkey.KeyCode = (Keys) RotationSettings.KeysList.FirstOrDefault(k => k.Text == rotation1.Key).Code;
                    hotkey.Pressed += delegate { StartRotation(rotation1.Name); };
                    if (!hotkey.GetCanRegister(this))
                    {
                        Logging.Write("Cannot register {0} as hotkey", rotation.Key);
                    }
                    else
                    {
                        hotkey.Register(this);
                        _hotKeys.Add(hotkey);
                    }
                }
                catch
                {
                    Logging.Write("Cannot register {0} as hotkey", rotation.Key);
                }
            }
        }

        private static void LoadFirstTime()
        {
            Langs.Load();
            KeyHelper.LoadKeys();
            BarMapper.MapBars();
        }

        private static void CheckBuffAndKeys(IEnumerable<Rule> rules)
        {
            foreach (Rule rule in rules)
            {
                if (rule.IsScript)
                    continue;
                rule.BotStarting();
                if (!rule.Action.DoesKeyExist)
                    Logging.Write(LogType.Warning, "Key: " + rule.Action.Name + " does not exist on your bars");
                foreach (AbstractCondition abstractCondition in rule.GetConditions)
                {
                    if (abstractCondition is BuffCondition)
                    {
                        if (!String.IsNullOrEmpty(((BuffCondition) abstractCondition).GetBuffName()))
                            if (!BarMapper.DoesBuffExist(((BuffCondition) abstractCondition).GetBuffName()))
                                Logging.Write(LogType.Warning,
                                              "Buff: " + ((BuffCondition) abstractCondition).GetBuffName() +
                                              " does not exist in HasWellKnownBuff will not detect it correctly");
                    }
                }
            }
        }

        private void StartRotation(string name)
        {
            if (_rotationThread != null && _rotationThread.IsAlive)
            {
                _rotationThread.Abort();
                _rotationThread = null;
            }
            if (RotationManagerController.Rotations.FirstOrDefault(r => r.Name == name) != _rotation)
            {
                _rotation = RotationManagerController.Rotations.FirstOrDefault(r => r.Name == name);
                _rotationThread = new Thread(DoRotation) {IsBackground = true};
                _rotationThread.Start();
                Logging.Write(LogType.Info, "Started rotator");
                UpdateStatus(true);
            }
            else
            {
                _rotation = null;
                Logging.Write(LogType.Info, "Stopped rotator");
                UpdateStatus(false);
            }
        }

        private void DoRotation()
        {
            List<Rule> rules = _rotation.Rules.GetRules;
            rules.Sort();
            while (true)
            {
                try
                {
                    if (ObjectManager.MyPlayer.HasTarget)
                    {
                        foreach (Rule rule in rules.Where(rule => rule.IsOk))
                        {
                            PUnit pUnit = ObjectManager.MyPlayer.Target;
                            if (pUnit.IsValid && pUnit.IsAlive)
                            {
                                rule.ExecuteAction(_rotation.GlobalCooldown);
                                break;
                            }
                        }
                    }
                    Thread.Sleep(10);
                    Application.DoEvents();
                }
                catch (ThreadAbortException)
                {
                }
                catch (Exception e)
                {
                    Logging.Write("Exception in rotation: " + e);
                }
            }
        }

        private void Stop()
        {
            if (_rotationThread != null && _rotationThread.IsAlive)
            {
                _rotationThread.Abort();
                _rotationThread = null;
            }
            foreach (Hotkey hotKey in _hotKeys)
            {
                try
                {
                    if (hotKey.Registered)
                    {
                        hotKey.Unregister();
                    }
                }
                catch
                {
                }
            }
            _hotKeys.Clear();
        }

        private void RotatorFormFormClosing(object sender, FormClosingEventArgs e)
        {
            GeomertrySettings.RotatorForm = Geometry.GeometryToString(this);
            Stop();
        }

        private void UpdateStatus(bool running)
        {
            if (status != null && !status.IsDisposed)
            {
                status.UpdateStatus(running);
            }
        }

        private void CBShowStatusWindow_CheckedChanged(object sender, EventArgs e)
        {
            if (CBShowStatusWindow.Checked)
            {
                status = new RotatorStatus();
                status.Show();
            }
            else
            {
                status.Close();
            }
        }
    }
}