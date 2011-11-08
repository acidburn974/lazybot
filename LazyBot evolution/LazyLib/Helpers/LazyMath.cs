
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
using System;
using LazyLib.Wow;

namespace LazyLib.Helpers
{
    internal class LazyMath
    {
        public static float NegativeValue(float value)
        {
            if (value < 0)
                value = value*-1;
            return value;
        }

        public static float NegativeAngle(float angle)
        {
            //if the turning angle is negative
            if (angle < 0)
                //add the maximum possible angle (PI x 2) to normalize the negative angle
                angle += (float) (Math.PI*2);
            return angle;
        }

        public static Double Distance3D(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            float dx = x1 - x2;
            float dy = y1 - y2;
            float dz = z1 - z2;

            return Math.Sqrt(((dx*dx) + (dy*dy) + (dz*dz)));
        }

        public static Double Distance2D(float x1, float y1, float x2, float y2)
        {
            float dx = x1 - x2;
            float dy = y1 - y2;

            return Math.Sqrt(((dx*dx) + (dy*dy)));
        }

        public static Double Distance3D(float x, float y, float z)
        {
            float dx = x - ObjectManager.MyPlayer.X;
            float dy = y - ObjectManager.MyPlayer.Y;
            float dz = z - ObjectManager.MyPlayer.Z;

            return Math.Sqrt(((dx*dx) + (dy*dy) + (dz*dz)));
        }

        public static Double Distance2D(float x, float y)
        {
            float dx = x - ObjectManager.MyPlayer.X;
            float dy = y - ObjectManager.MyPlayer.Y;

            return Math.Sqrt(((dx*dx) + (dy*dy)));
        }

        public static Double Distance2D(Location pos)
        {
            return Distance2D(pos.X, pos.Y, ObjectManager.MyPlayer.X, ObjectManager.MyPlayer.Y);
        }

        public static Double Distance3D(Location pos)
        {
            return Distance3D(pos.X, pos.Y, pos.Z, ObjectManager.MyPlayer.X, ObjectManager.MyPlayer.Y,
                              ObjectManager.MyPlayer.Z);
        }

        public static Double Distance2D(Location from, Location to)
        {
            return Distance2D(from.X, from.Y, to.X, to.Y);
        }

        public static Double Distance3D(Location from, Location to)
        {
            return Distance3D(from.X, from.Y, from.Z, to.X, to.Y, to.Z);
        }

        public static Single CalculateFace(Single toX, Single toY, Single fromX, Single fromY)
        {
            Single angle =
                Convert.ToSingle(Math.Atan2(Convert.ToDouble(toY) - Convert.ToDouble(fromY),
                                            Convert.ToDouble(toX) - Convert.ToDouble(fromX)));
            angle = NegativeAngle(angle);
            return angle;
        }

        public static float CalculateFace(Location from, Location to)
        {
            return CalculateFace(to.X, to.Y, from.X, from.Y);
        }

        public static Single CalculateFace(Single x, Single y)
        {
            return CalculateFace(x, y, ObjectManager.MyPlayer.X, ObjectManager.MyPlayer.Y);
        }

        public static float CalculateFace(Location to)
        {
            return CalculateFace(to.X, to.Y, ObjectManager.MyPlayer.X, ObjectManager.MyPlayer.Y);
        }

        public static bool IsFacingH(float angle, float errorMarge)
        {
            float face;

            if (NegativeAngle(angle - ObjectManager.MyPlayer.Facing) < Math.PI)
            {
                face = NegativeAngle(angle - ObjectManager.MyPlayer.Facing);

                if (face < errorMarge)
                    return true;
            }
            else
            {
                face = NegativeAngle(ObjectManager.MyPlayer.Facing - angle);

                if (face < errorMarge)
                    return true;
            }

            return false;
        }
    }
}