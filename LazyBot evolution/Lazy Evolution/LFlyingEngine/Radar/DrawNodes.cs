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

using System.Drawing;
using LazyEvo.LFlyingEngine.Helpers;
using LazyLib.LazyRadar;
using LazyLib.LazyRadar.Drawer;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.Radar
{
    internal class DrawNodes : IDrawItem
    {
        private readonly Color _colorBadNodes = Color.Red;
        private readonly Color _colorObjects = Color.ForestGreen;

        #region IDrawItem Members

        public void Draw(RadarForm form)
        {
            foreach (PGameObject selectNode in ObjectManager.GetGameObject)
            {
                if (FindNode.IsHerb(selectNode) || FindNode.IsMine(selectNode))
                {
                    if (FlyingBlackList.IsBlacklisted(selectNode))
                    {
                        form.PrintCircle(_colorBadNodes,
                                         form.OffsetY(selectNode.Location.Y, ObjectManager.MyPlayer.Location.Y),
                                         form.OffsetX(selectNode.Location.X, ObjectManager.MyPlayer.Location.X),
                                         selectNode.Name);
                    }
                    else
                    {
                        form.PrintCircle(_colorObjects,
                                         form.OffsetY(selectNode.Location.Y, ObjectManager.MyPlayer.Location.Y),
                                         form.OffsetX(selectNode.Location.X, ObjectManager.MyPlayer.Location.X),
                                         selectNode.Name);
                    }
                }
            }
        }

        public string SettingName()
        {
            return "DrawNodes";
        }

        public string CheckBoxName()
        {
            return "Show nodes";
        }

        #endregion
    }
}