
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
using System.Drawing;
using LazyLib.Wow;

namespace LazyLib.LazyRadar.Drawer
{
    internal class DrawUnits : IDrawItem
    {
        private readonly Color _colorUnits = Color.BlueViolet;

        #region IDrawItem Members

        public void Draw(RadarForm form)
        {
            const string othTop = "";
            string othBot;
            foreach (PUnit mob in ObjectManager.GetUnits)
            {
                if (mob.GUID.Equals(ObjectManager.MyPlayer.GUID))
                    continue;

                if (mob.IsPlayer)
                    continue;

                if (mob.IsDead)
                    continue;

                othBot = mob.Name + " ";
                othBot = othBot.TrimEnd();

                form.PrintArrow(_colorUnits,
                                form.OffsetY(mob.Location.Y, ObjectManager.MyPlayer.Location.Y),
                                form.OffsetX(mob.Location.X, ObjectManager.MyPlayer.Location.X),
                                mob.Facing, othTop, othBot);
            }
        }

        public string SettingName()
        {
            return "DrawUnits";
        }

        public string CheckBoxName()
        {
            return "Show units";
        }

        #endregion
    }
}