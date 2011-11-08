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
using System.Windows.Forms;
using LazyEvo.LFlyingEngine.Helpers;
using LazyLib;
using LazyLib.LazyRadar;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.Radar
{
    internal class MouseHandler : IMouseClick
    {
        #region IMouseClick Members

        public void Click(RadarForm form, MouseEventArgs e)
        {
            Point cursorPosition = form.PointToClient(Cursor.Position);
            var cursorRect = new Rectangle(cursorPosition.X, cursorPosition.Y, 5, 5);
            try
            {
                foreach (PGameObject node in ObjectManager.GetGameObject)
                {
                    if (FindNode.IsHerb(node) || FindNode.IsMine(node))
                    {
                        float x = form.OffsetX(node.Location.X, ObjectManager.MyPlayer.Location.X);
                        float y = form.OffsetY(node.Location.Y, ObjectManager.MyPlayer.Location.Y);
                        var objRect = new Rectangle((int) y, (int) x, 5, 5);
                        if (Rectangle.Intersect(objRect, cursorRect) != Rectangle.Empty)
                        {
                            if (FlyingBlackList.IsBlacklisted(node))
                            {
                                FlyingBlackList.Unblacklist(node);
                            }
                            else
                            {
                                Logging.Write("Added the node to the permanent blacklist");
                                FlyingBlackList.AddBadNode(node);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        #endregion
    }
}