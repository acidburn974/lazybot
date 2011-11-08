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

using LazyEvo.LGrindEngine.Helpers;
using LazyEvo.LGrindEngine.NpcClasses;
using LazyLib;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.Activity
{
    internal class Train
    {
        internal static bool TrainEnabled;
        internal static VendorsEx Trainer;

        internal static void Pulse()
        {
            if (Trainer == null)
            {
                GrindingEngine.Navigator.Stop();
                Trainer = GrindingEngine.CurrentProfile.NpcController.GetTrainer(ObjectManager.MyPlayer.UnitClass);
                GrindingEngine.Navigation.SetNewSpot(Trainer.Location);
                Logging.Write("Going to train at: " + Trainer.Name);
            }
            else if (Trainer.Location.DistanceToSelf2D < 5)
            {
                Logging.Write("Train done, going back");
                GrindingEngine.Navigator.Stop();
                GrindingEngine.Navigation = new GrindingNavigation(GrindingEngine.CurrentProfile);
                GrindingBlackList.Blacklist(Trainer.Name, 300, false);
                SetTrain(false);
            }
            if (GrindingEngine.Navigation.SpotToHit != Trainer.Location)
            {
                Logging.Write("Set spot");
                GrindingEngine.Navigation.SetNewSpot(Trainer.Location);
            }
            GrindingEngine.Navigation.Pulse();
        }

        internal static void SetTrain(bool enable)
        {
            if (enable)
            {
                TrainEnabled = true;
            }
            else
            {
                TrainEnabled = false;
                Trainer = null;
            }
        }
    }
}