
﻿/*
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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using LazyLib;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.IEngine;
using LazyLib.LazyRadar;
using LazyLib.LazyRadar.Drawer;
using LazyLib.Wow;
using LFishingEngine.States;

namespace LFishingEngine
{
    internal class FishingEngine : ILazyEngine
    {   
        internal static string OurDirectory;
        private static List<MainState> FishingStates { get; set; }
        #region ILazyEngine Members

        public string Name
        {
            get { return "Fishing Engine"; }
        }

        public List<MainState> States
        {
            get { return FishingStates; }
        }

        public Form Settings
        {
            get
            {
                return new Settings();
            }
        }

        public Form ProfileForm
        {
            get
            {
                return new Form();
            }
        }

        public void Load()
        {
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            FishingSettings.LoadSettings();
        }

        public bool EngineStart()
        {
            KeyHelper.AddKey("Lure", "None", FishingSettings.LureBar, FishingSettings.LureKey);
            if (!ObjectManager.InGame)
            {
                Logging.Write(LogType.Info, "Enter game before starting the bot");
                return false;
            }
            if (!ObjectManager.MyPlayer.IsAlive)
            {
                Logging.Write(LogType.Info, "Please ress before starting the bot");
                return false;
            }
            FishingStates = new List<MainState>
                    {
                        new StateFish(),
                        new StateLure(),
                    };
            return true;
        }

        public void EngineStop() { }
        public void Close() { }
        public void Pause() { }
        public void Resume() { }
        public List<IDrawItem> GetRadarDraw()
        {
            return new List<IDrawItem>();
        }

        public List<IMouseClick> GetRadarClick()
        {
            return new List<IMouseClick>();
        }

        #endregion
    }
}