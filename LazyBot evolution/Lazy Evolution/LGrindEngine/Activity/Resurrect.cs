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
using LazyLib;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.Activity
{
    internal class Resurrect
    {
        internal static Boolean ReachedEndGhostWaypoint;
        internal static Int32 NearestWaypointIndexFromCorpse = -1;
        internal static Location CorpsePosition = new Location(0, 0, 0);

        internal static void Reset()
        {
            ReachedEndGhostWaypoint = false;
            NearestWaypointIndexFromCorpse = -1;
            CorpsePosition = new Location(0, 0, 0);
        }

        internal static void Pulse()
        {
            if (CorpsePosition.X == 0 && CorpsePosition.Y == 0 && CorpsePosition.Z == 0)
            {
                GrindingEngine.Navigator.Stop();
                MoveHelper.ReleaseKeys();
                CorpsePosition = ObjectManager.MyPlayer.Location;
                GrindingEngine.UpdateStats(0, 0, 1);
            }
            while (!ObjectManager.MyPlayer.IsGhost)
            {
                Thread.Sleep(1000);
                Frame staticPopup1Button1 = InterfaceHelper.GetFrameByName("StaticPopup1Button1");
                if (staticPopup1Button1 != null && staticPopup1Button1.IsVisible)
                {
                    staticPopup1Button1.LeftClick();
                    Thread.Sleep(2000);
                }
                Thread.Sleep(100);
            }
            if (GrindingEngine.Navigation.SpotToHit != CorpsePosition)
            {
                GrindingEngine.Navigation.SetNewSpot(CorpsePosition);
            }
            GrindingEngine.Navigation.Pulse();
            if (GrindingEngine.Navigation.IsLastWaypoints || CorpsePosition.DistanceToSelf2D < 20)
            {
                Logging.Write("Move to our corpse");
                GrindingEngine.Navigator.Stop();
                MoveHelper.ReleaseKeys();

                MoveHelper.MoveToLoc(CorpsePosition, 3);

                Logging.Write("Lets ress");

                Frame staticPopup1Button1 = InterfaceHelper.GetFrameByName("StaticPopup1Button1");
                var clickTimeout = new Ticker(5*1000);
                bool firstClick = false;
                while (ObjectManager.MyPlayer.IsGhost)
                {
                    if (!firstClick || clickTimeout.IsReady)
                    {
                        if (staticPopup1Button1 != null && staticPopup1Button1.IsVisible)
                        {
                            staticPopup1Button1.LeftClick();
                            firstClick = true;
                            clickTimeout.Reset();
                        }
                    }
                    Thread.Sleep(1000);
                }
                Logging.Write("Ress worked");
                Reset();
                GrindingEngine.Navigator.Stop();
                GrindingEngine.Navigation = new GrindingNavigation(GrindingEngine.CurrentProfile);
                return;
            }
            Thread.Sleep(10);
        }
    }
}