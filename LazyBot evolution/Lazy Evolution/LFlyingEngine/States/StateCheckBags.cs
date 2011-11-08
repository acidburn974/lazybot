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

using LazyEvo.LFlyingEngine.Helpers;
using LazyEvo.Public;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.States
{
    internal class StateFullBags : MainState
    {
        public override int Priority
        {
            get { return Prio.BagsFull; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (FlyingSettings.StopOnFullBags)
                {
                    return Langs.BagsFull(ObjectManager.MyPlayer.RedMessage) && !ObjectManager.ShouldDefend;
                }
                return false;
            }
        }

        public override void DoWork()
        {
            HelperFunctions.ResetRedMessage();
            if (!Mount.IsMounted())
            {
                Mount.MountUp();
                MoveHelper.Jump(3000);
            }
            LazyHelpers.StopAll("Bags are full, stopping");
        }

        public override string Name()
        {
            return "Full bags";
        }
    }
}