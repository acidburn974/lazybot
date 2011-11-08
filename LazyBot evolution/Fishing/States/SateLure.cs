
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
using System.Threading;
using LazyLib.FSM;
using LazyLib.Helpers;

namespace LFishingEngine.States
{
    public class StateLure : MainState
    {
        public override int Priority
        {
            get { return Prio.Lure; }
        }

        private readonly Ticker _reLure = new Ticker(600000);
        public StateLure()
        {
            _reLure.ForceReady();
        }

        public override bool NeedToRun
        {
            get
            {
                if (!FishingSettings.UseLure)
                    return false;
                return _reLure.IsReady;
            }
        }

        public override void DoWork()
        {
            KeyHelper.SendKey("Lure");
            Thread.Sleep(3100);
            _reLure.Reset();
        }

        public override string Name()
        {
            return "Lure";
        }
    }
}
