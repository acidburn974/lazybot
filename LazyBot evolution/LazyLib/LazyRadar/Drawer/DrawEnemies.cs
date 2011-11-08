
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
using System.Linq;
using LazyLib.Wow;

namespace LazyLib.LazyRadar.Drawer
{
    internal class DrawEnemies : IDrawItem
    {
        private readonly Color _colorEnemies = Color.Red;

        #region IDrawItem Members

        public void Draw(RadarForm form)
        {
            string othBot;
            foreach (
                PPlayer play in
                    ObjectManager.GetPlayers.Where(
                        cur => !cur.PlayerFaction.Equals(ObjectManager.MyPlayer.PlayerFaction)))
            {
                if (play.GUID.Equals(ObjectManager.MyPlayer.GUID))
                    continue;
                string othTop = play.Name;
                othBot = " Lvl: " + play.Level;
                othBot = othBot.TrimEnd();
                form.PrintArrow(_colorEnemies, form.OffsetY(play.Location.Y, ObjectManager.MyPlayer.Location.Y),
                                form.OffsetX(play.Location.X, ObjectManager.MyPlayer.Location.X),
                                play.Facing, othTop, othBot);
            }
        }

        public string SettingName()
        {
            return "DrawEnemies";
        }

        public string CheckBoxName()
        {
            return "Show enemies";
        }

        #endregion
    }
}