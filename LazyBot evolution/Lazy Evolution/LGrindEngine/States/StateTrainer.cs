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
using LazyEvo.LGrindEngine.Activity;
using LazyEvo.LGrindEngine.Helpers;
using LazyLib;
using LazyLib.ActionBar;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.States
{
    internal class StateTrainer : MainState
    {
        private const int SearchDistance = 12;
        private readonly PUnit _npc = new PUnit(0);

        public override bool NeedToRun
        {
            get
            {
                if (ObjectManager.MyPlayer.IsDead)
                {
                    return false;
                }
                if (!GrindingSettings.ShouldTrain)
                {
                    return false;
                }
                if (!Train.TrainEnabled)
                {
                    return false;
                }
                if (Train.Trainer == null)
                {
                    return false;
                }
                _npc.BaseAddress = 0;
                foreach (PUnit unit in ObjectManager.GetUnits)
                {
                    if (Train.Trainer.EntryId != int.MinValue)
                    {
                        if (unit.Entry == Train.Trainer.EntryId && unit.Location.DistanceToSelf2D < SearchDistance)
                        {
                            if (!GrindingBlackList.IsBlacklisted(unit.Name))
                            {
                                _npc.BaseAddress = unit.BaseAddress;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (unit.Name.Equals(Train.Trainer.Name) && unit.Location.DistanceToSelf2D < SearchDistance)
                        {
                            if (!GrindingBlackList.IsBlacklisted(unit.Name))
                            {
                                _npc.BaseAddress = unit.BaseAddress;
                                break;
                            }
                        }
                    }
                }
                return (_npc.BaseAddress != 0);
            }
        }

        public override int Priority
        {
            get { return Prio.Train; }
        }

        public override void DoWork()
        {
            Logging.Write("[Train]Found the trainer");
            GrindingEngine.Navigator.Stop();
            MoveHelper.MoveToLoc(_npc.Location, 5);
            DoTrain();
            Logging.Write("Re-mapping the bars");
            BarMapper.MapBars();
            Logging.Write("[Train]Train done");
            GrindingEngine.Navigator.Stop();
            GrindingEngine.Navigation = new GrindingNavigation(GrindingEngine.CurrentProfile);
            Train.SetTrain(false);
            GrindingEngine.ShouldTrain = false;
        }

        private void DoTrain()
        {
            int clickTimes = GrindingShouldTrain.TrainCount();
            MoveHelper.MoveToUnit(_npc, 3);
            _npc.Location.Face();
            _npc.Interact(false);
            Thread.Sleep(1200);
            Logging.Write("Going to train: " + clickTimes + " new skill(s)");
            for (int i = 0; i <= clickTimes; i++)
            {
                try
                {
                    InterfaceHelper.GetFrameByName("ClassTrainerTrainButton");
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Logging.Write("Error when clicking train button: " + e);
                }
            }
        }

        public override string Name()
        {
            return "Train";
        }
    }
}