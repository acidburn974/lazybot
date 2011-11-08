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

#region

using System;
using System.Threading;
using LazyLib;
using LazyLib.Helpers;
using LazyLib.Wow;

#endregion

namespace LazyEvo.LFlyingEngine.Helpers
{
    internal class HelperFunctions
    {
        public static void ResetRedMessage()
        {
            Logging.Write("Resetting red message");
            KeyHelper.SendKey("F1");
            Thread.Sleep(100);
            KeyHelper.SendKey("Attack1");
        }

        /// <summary>
        ///   Moves in 3D
        /// </summary>
        /// <param name = "targetCoord">The target coord.</param>
        /// <param name="zMaxAvoidiance"></param>
        public static void Move3D(Location targetCoord, int zMaxAvoidiance)
        {
            var targetGLoc = new Location(targetCoord.X, targetCoord.Y, targetCoord.Z);
            if (!targetCoord.IsFacing(0.3f))
            {
                targetGLoc.Face();
            }
            if (targetCoord.Z - ObjectManager.MyPlayer.Location.Z > zMaxAvoidiance) //go up
            {
                KeyHelper.ReleaseKey("X");
                KeyHelper.PressKey("Space");
            }
            else if (targetCoord.Z - ObjectManager.MyPlayer.Location.Z < -zMaxAvoidiance) //go down
            {
                KeyHelper.ReleaseKey("Space");
                KeyHelper.PressKey("X");
            }
            else
            {
                KeyHelper.ReleaseKey("Space");
                KeyHelper.ReleaseKey("X");
            }
        }

        /// <summary>
        ///   Clamps the specified in val.
        /// </summary>
        /// <param name = "inVal">The in val.</param>
        /// <param name = "min">The min.</param>
        /// <param name = "max">The max.</param>
        public static void Clamp(ref int inVal, int min, int max)
        {
            inVal = Math.Min(max, inVal);
            inVal = Math.Max(min, inVal);
        }

        /// <summary>
        ///   Pauses the specified min MS.
        /// </summary>
        /// <param name = "minMs">The min MS.</param>
        /// <param name = "maxMs">The max MS.</param>
        /// <returns></returns>
        public static int Pause(int minMs, int maxMs)
        {
            return new Random().Next(minMs, maxMs);
        }
    }
}