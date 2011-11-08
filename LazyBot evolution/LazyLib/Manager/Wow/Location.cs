
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
#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using LazyEvo.LGrindEngine;
using LazyLib.Helpers;

#endregion

namespace LazyLib.Wow
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true, Exclude = true)]
    [Serializable]
    public class Location
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "Location" /> class.
        /// </summary>
        /// <param name = "loc">The loc.</param>
        public Location(string loc)
        {
            try
            {
                string[] split = loc.Split(new[] {' '});
                X = (float) Convert.ToDouble(split[0]);
                Y = (float) Convert.ToDouble(split[1]);
                Z = (float) Convert.ToDouble(split[2]);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Location" /> struct.
        /// </summary>
        /// <param name = "x">The X.</param>
        /// <param name = "y">The Y.</param>
        /// <param name = "z">The Z.</param>
        public Location(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Returns the Node's X coordinate.
        /// </summary>
        public float X { get; private set; }

        /// <summary>
        /// Returns the Node's Y coordinate.
        /// </summary>
        public float Y { get; private set; }

        /// <summary>
        /// Returns the Node's Y coordinate.
        /// </summary>
        public float Z { get; private set; }

        /// <summary>
        ///   Gets the bearing.
        /// </summary>
        /// <value>The bearing.</value>
        public float Bearing
        {
            get { return NegativeAngle((float) Math.Atan2((Y - ObjectManager.MyPlayer.Y), (X - ObjectManager.MyPlayer.X))); }
        }


        public bool IsFacingAway
        {
            get
            {
                if (LazyMath.NegativeAngle(Bearing - ObjectManager.MyPlayer.Facing) > 5.5 ||
                    LazyMath.NegativeAngle(Bearing - ObjectManager.MyPlayer.Facing) < 0.6)
                    return true;
                return false;
            }
        }

        public NodeType NodeType { get; set; }


        public Double DistanceToSelf
        {
            get { return LazyMath.Distance3D(this); }
        }

        public Single AngleHorizontal
        {
            get { return LazyMath.CalculateFace(this); }
        }

        public Double DistanceToSelf2D
        {
            get { return LazyMath.Distance2D(this); }
        }

        /// <summary>
        ///   Makes sure that an angle is always positive
        /// </summary>
        /// <param name = "angle">The angle.</param>
        /// <returns></returns>
        private float NegativeAngle(float angle)
        {
            //if the turning angle is negative
            if (angle < 0)
                //add the maximum possible angle (PI x 2) to normalize the negative angle
                angle += (float) (Math.PI*2);
            return angle;
        }

        /// <summary>
        ///   Distances from the location.
        /// </summary>
        /// <param name = "nX">The n X.</param>
        /// <param name = "nY">The n Y.</param>
        /// <param name = "nZ">The n Z.</param>
        /// <returns></returns>
        public double GetDistanceTo(float nX, float nY, float nZ)
        {
            if (nZ == 0 || Z == 0)
                return Math.Sqrt((Math.Pow((X - nX), 2.0) + Math.Pow((Y - nY), 2.0)));

            return Math.Sqrt((Math.Pow((X - nX), 2.0) + Math.Pow((Y - nY), 2.0)) + Math.Pow((Z - nZ), 2.0));
        }

        /// <summary>
        ///   Distances from the location.
        /// </summary>
        /// <param name = "location">The location.</param>
        /// <returns></returns>
        public double GetDistanceTo(Location location)
        {
            if (location.Z == 0 || Z == 0)
                return Math.Sqrt((Math.Pow((X - location.X), 2.0) + Math.Pow((Y - location.Y), 2.0)));

            return
                Math.Sqrt((Math.Pow((X - location.X), 2.0) + Math.Pow((Y - location.Y), 2.0)) +
                          Math.Pow((Z - location.Z), 2.0));
        }

        /// <summary>
        ///   Distances from the location.
        /// </summary>
        /// <param name = "nX">The n X.</param>
        /// <param name = "nY">The n Y.</param>
        /// <param name = "nZ">The n Z.</param>
        /// <returns></returns>
        public double DistanceFrom(float nX, float nY, float nZ)
        {
            if (nZ == 0 || Z == 0)
                return Math.Sqrt((Math.Pow((X - nX), 2.0) + Math.Pow((Y - nY), 2.0)));

            return Math.Sqrt((Math.Pow((X - nX), 2.0) + Math.Pow((Y - nY), 2.0)) + Math.Pow((Z - nZ), 2.0));
        }

        /// <summary>
        ///   Distances from this location
        /// </summary>
        /// <param name = "pos">The pos.</param>
        /// <returns></returns>
        public double DistanceFrom(Location pos)
        {
            if (pos.Z == 0 || Z == 0)
                return Math.Sqrt((Math.Pow((X - pos.X), 2.0) + Math.Pow((Y - pos.Y), 2.0)));

            return
                Math.Sqrt((Math.Pow((X - pos.X), 2.0) + Math.Pow((Y - pos.Y), 2.0)) +
                          Math.Pow((Z - pos.Z), 2.0));
        }

        /// <summary>
        ///   Distances from this location
        /// </summary>
        /// <param name = "pos">The pos.</param>
        /// <returns></returns>
        public double DistanceFromXY(Location pos)
        {
            return Math.Sqrt((Math.Pow((X - pos.X), 2.0) + Math.Pow((Y - pos.Y), 2.0)));
        }

        /// <summary>
        ///   Returns a <see cref = "System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///   A <see cref = "System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("X, Y, Z = [{0}, {1}, {2}]", X, Y, Z);
        }

        public Single Angle(Location from)
        {
            return LazyMath.CalculateFace(from, this);
        }

        public void Face()
        {
            float face;

            if (LazyMath.NegativeAngle(AngleHorizontal - ObjectManager.MyPlayer.Facing) < Math.PI)
            {
                face = LazyMath.NegativeAngle(AngleHorizontal - ObjectManager.MyPlayer.Facing);

                bool moving = ObjectManager.MyPlayer.IsMoving;
                if (face > 1)
                {
                    MoveHelper.ReleaseKeys();
                    moving = false;
                }
                FaceHorizontalWithTimer(face, "Left", moving);
            }
            else
            {
                face = LazyMath.NegativeAngle(ObjectManager.MyPlayer.Facing - AngleHorizontal);

                bool moving = ObjectManager.MyPlayer.IsMoving;
                if (face > 1)
                {
                    MoveHelper.ReleaseKeys();
                    moving = false;
                }
                FaceHorizontalWithTimer(face, "Right", moving);
            }
        }

        public static void FaceAngle(float angle)
        {
            float face;
            if (LazyMath.NegativeAngle(angle - ObjectManager.MyPlayer.Facing) < Math.PI)
            {
                face = LazyMath.NegativeAngle(angle - ObjectManager.MyPlayer.Facing);
                bool moving = ObjectManager.MyPlayer.IsMoving;
                if (face > 1)
                {
                    MoveHelper.ReleaseKeys();
                    moving = false;
                }
                FaceHorizontalWithTimer(face, "Left", moving);
            }
            else
            {
                face = LazyMath.NegativeAngle(ObjectManager.MyPlayer.Facing - angle);
                bool moving = ObjectManager.MyPlayer.IsMoving;
                if (face > 1)
                {
                    MoveHelper.ReleaseKeys();
                    moving = false;
                }
                FaceHorizontalWithTimer(face, "Right", moving);
            }
        }

        public bool IsFacing(float errorMarge)
        {
            return LazyMath.IsFacingH(AngleHorizontal, errorMarge);
        }

        public bool IsFacing()
        {
            return IsFacing(0.1f);
        }

        private static void FaceHorizontalWithTimer(float radius, string key, bool moving)
        {
            if (radius < 0.1f)
                return;
            Int32 turnTime = moving ? 1328 : 980;
            KeyHelper.PressKey(key);
            Thread.Sleep((int) ((radius*turnTime*Math.PI)/10));
            KeyHelper.ReleaseKey(key);
        }

        public static Int32 GetClosestPositionInList(List<Location> waypoints, Location pos)
        {
            int result = 0;
            Double nearest = -1;
            int i = 0;
            foreach (double distance in waypoints.Select(p => LazyMath.Distance3D(pos.X, pos.Y, pos.Z, p.X, p.Y, p.Z)))
            {
                if (nearest == -1 || distance < nearest)
                {
                    result = i;
                    nearest = distance;
                }
                i++;
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Location)) return false;
            return Equals((Location) obj);
        }

        public bool Equals(Location other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.X.Equals(X) && other.Y.Equals(Y) && other.Z.Equals(Z);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = X.GetHashCode();
                result = (result*397) ^ Y.GetHashCode();
                result = (result*397) ^ Z.GetHashCode();
                return result;
            }
        }
    }
}