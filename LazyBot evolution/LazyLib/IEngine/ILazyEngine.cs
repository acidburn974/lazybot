
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
using System.Reflection;
using System.Windows.Forms;
using LazyLib.FSM;
using LazyLib.LazyRadar;
using LazyLib.LazyRadar.Drawer;

namespace LazyLib.IEngine
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public interface ILazyEngine
    {
        string Name { get; }
        List<MainState> States { get; }
        Form Settings { get; }
        Form ProfileForm { get; }

        /// <summary>
        /// Load the basic stuff that will not change and does not depend on attach state of the bot
        /// </summary>
        void Load();

        /// <summary>
        /// Should return true if its ok to start the bot... do you start logic here and add you states to the engine.
        /// </summary>
        /// <returns></returns>
        bool EngineStart();

        void EngineStop();
        void Close();
        void Pause();
        void Resume();
        List<IDrawItem> GetRadarDraw();
        List<IMouseClick> GetRadarClick();
    }
}