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
using LazyEvo.Forms;
using LazyEvo.LFlyingEngine.Activity;
using LazyEvo.LFlyingEngine.Helpers;
using LazyLib;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.States
{
    internal class StateMount : MainState
    {
        public override int Priority
        {
            get { return Prio.Mount; }
        }

        public override bool NeedToRun
        {
            get
            {
                //Lets check if our inventory or gear broke 
                if (Inventory.FreeBagSlots <= Convert.ToInt32(LazySettings.FreeBackspace) && LazySettings.ShouldVendor &&
                    !ToTown.ToTownEnabled)
                {
                    Logging.Write("Inventory full, we are now in to town mode");
                    ToTown.SetToTown(true);
                }
                if (ObjectManager.MyPlayer.ShouldRepair && LazySettings.ShouldRepair && !ToTown.ToTownEnabled)
                {
                    Logging.Write("One or more items broken, we are now in to town mode");
                    ToTown.SetToTown(true);
                }
                return !Mount.IsMounted();
            }
        }

        public override void DoWork()
        {
            FlyingEngine.Navigator.Stop();
            MoveHelper.ReleaseKeys();
            Main.CombatEngine.RunningAction();
            Mount.MountUp();
        }

        public override string Name()
        {
            return "Mounting";
        }
    }
}