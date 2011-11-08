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
using System.Threading;
using LazyEvo.LGrindEngine.Helpers;
using LazyEvo.Public;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.States
{
    internal class StateMoving : MainState
    {
        private static Ticker _jumpRandomly = new Ticker(4000);
        private readonly Random _random = new Random();

        public override int Priority
        {
            get { return Prio.Moving; }
        }

        public override bool NeedToRun
        {
            get { return true; }
        }

        public override string Name()
        {
            return "Moving";
        }

        public override void DoWork()
        {
            if (ObjectManager.MyPlayer.IsAlive)
                CombatHandler.RunningAction();
            if (GrindingSettings.Jump && _jumpRandomly.IsReady)
            {
                MoveHelper.Jump();
                _jumpRandomly = new Ticker(_random.Next(4, 8)*1000);
            }
            GrindingEngine.Navigation.Pulse();
            Thread.Sleep(10);
        }
    }
}