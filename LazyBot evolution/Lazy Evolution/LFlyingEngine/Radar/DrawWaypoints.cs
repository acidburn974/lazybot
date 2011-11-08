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

using System.Collections.Generic;
using System.Drawing;
using LazyEvo.LFlyingEngine.Helpers;
using LazyLib.LazyRadar;
using LazyLib.LazyRadar.Drawer;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.Radar
{
    internal class DrawWaypoints : IDrawItem
    {
        private readonly Color _colorToTown = Color.Green;
        private readonly Color _colorWaypoints = Color.Red;

        #region IDrawItem Members

        public void Draw(RadarForm form)
        {
            if (FlyingEngine.CurrentProfile != null)
            {
                FlyingProfile temp = FlyingEngine.CurrentProfile;
                PrintWay(temp.GetListSortedAfterDistance(ObjectManager.MyPlayer.Location), _colorWaypoints, form);
                PrintWay(temp.GetListSortedAfterDistance(ObjectManager.MyPlayer.Location, temp.WaypointsToTown),
                         _colorToTown, form);
            }
        }

        public string SettingName()
        {
            return "DrawWaypoints";
        }

        public string CheckBoxName()
        {
            return "Show waypoints";
        }

        #endregion

        private void PrintWay(List<Location> loc, Color color, RadarForm form)
        {
            if (loc != null)
            {
                PointF[] points;
                Point tempP;
                Location ld;
                if (loc.Count != 0)
                {
                    points = new PointF[loc.Count + 1];
                    int i = 0;
                    foreach (Location lo in loc)
                    {
                        form.PrintCircle(color, form.OffsetY(lo.Y, ObjectManager.MyPlayer.Location.Y),
                                         form.OffsetX(lo.X, ObjectManager.MyPlayer.Location.X), "");
                        tempP = new Point(form.OffsetY(lo.Y, ObjectManager.MyPlayer.Location.Y),
                                          form.OffsetX(lo.X, ObjectManager.MyPlayer.Location.X));
                        points[i] = tempP;
                        i++;
                    }
                    ld = loc[0];
                    tempP = new Point(form.OffsetY(ld.Y, ObjectManager.MyPlayer.Location.Y),
                                      form.OffsetX(ld.X, ObjectManager.MyPlayer.Location.X));
                    points[i] = tempP;
                    form.ScreenDc.DrawLines(new Pen(_colorWaypoints), points);
                }
            }
        }
    }
}