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
using System.Linq;
using LazyEvo.LGrindEngine.Activity;
using LazyEvo.LGrindEngine.Helpers;
using LazyEvo.LGrindEngine.NpcClasses;
using LazyLib;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.Wow;
using Train = LazyEvo.LGrindEngine.Activity.Train;

namespace LazyEvo.LGrindEngine.States
{
    internal class StateToTown : MainState
    {
        public override int Priority
        {
            get { return Prio.ToTown; }
        }

        public override bool NeedToRun
        {
            get
            {
                if (GrindingEngine.CurrentProfile.NpcController == null)
                {
                    return false;
                }
                if (Inventory.FreeBagSlots < Convert.ToInt32(LazySettings.FreeBackspace) && LazySettings.ShouldVendor &&
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
                if (GrindingEngine.ShouldTrain && GrindingSettings.ShouldTrain && !Train.TrainEnabled)
                {
                    Logging.Write("Going to train new skills");
                    Train.SetTrain(true);
                }
                if (Train.TrainEnabled && !ToTown.ToTownEnabled)
                {
                    if (!ObjectManager.MyPlayer.IsDead &&
                        GrindingEngine.CurrentProfile.NpcController.GetTrainer(ObjectManager.MyPlayer.UnitClass) != null)
                    {
                        if (Train.TrainEnabled)
                        {
                            Train.Pulse();
                        }
                    }
                }
                if (ToTown.ToTownEnabled)
                {
                    if (!ObjectManager.MyPlayer.IsDead &&
                        GrindingEngine.CurrentProfile.NpcController.Npc.Where(npc => npc.VendorType == VendorType.Repair)
                            .ToList().Count != 0)
                    {
                        if (ToTown.ToTownEnabled)
                        {
                            ToTown.Pulse();
                        }
                    }
                }
                return false;
            }
        }

        public override string Name()
        {
            return "ToTown";
        }

        public override void DoWork()
        {
        }
    }
}