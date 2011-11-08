
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
using System.Reflection;
using System.Threading;
using LazyLib.Wow;

namespace LazyLib.Helpers
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class MoveHelper
    {
        private static bool _runForwards;
        private static bool _runBackwards;
        private static bool _strafeLeft;
        private static bool _strafeRight;
        private static bool _rotateLeft;
        private static bool _rotateRight;
        private static bool _down;
        private static readonly Ticker KeyT = new Ticker(50);
        private static bool _oldRunForwards;
        private static bool _oldRunBackwards;
        private static bool _oldStrafeLeft;
        private static bool _oldStrafeRight;
        private static bool _oldRotateLeft;
        private static bool _oldRotateRight;
        private static bool _oldDown;
        private static readonly Random Ran = new Random();

        public static void ReleaseKeys()
        {
            StopMove();
            if (ObjectManager.Initialized)
            {
                KeyHelper.ReleaseKey("Space");
                KeyHelper.ReleaseKey("X");
                KeyHelper.ReleaseKey("Up");
                KeyHelper.ReleaseKey("Down");
                KeyHelper.ReleaseKey("Left");
                KeyHelper.ReleaseKey("Right");
                KeyHelper.ReleaseKey("Q");
                KeyHelper.ReleaseKey("E");
            }
        }

        public static void PushKeys()
        {
            if (_oldRunForwards != _runForwards)
            {
                KeyT.Wait();
                KeyT.Reset();
                if (_runForwards)
                    KeyHelper.PressKey("Up");
                else
                    KeyHelper.ReleaseKey("Up");
            }
            if (_oldRunBackwards != _runBackwards)
            {
                KeyT.Wait();
                KeyT.Reset();
                if (_runBackwards)
                    KeyHelper.PressKey("Down");
                else
                    KeyHelper.ReleaseKey("Down");
            }
            if (_oldDown != _down)
            {
                KeyT.Wait();
                KeyT.Reset();
                if (_down)
                    KeyHelper.PressKey("X");
                else
                    KeyHelper.ReleaseKey("X");
            }
            if (_oldStrafeLeft != _strafeLeft)
            {
                KeyT.Wait();
                KeyT.Reset();
                if (_strafeLeft)
                    KeyHelper.PressKey("Q");
                else
                    KeyHelper.ReleaseKey("Q");
                //PLazyBot.Scripter.WriteLine("RotateLeft: " + rotateLeft);
            }
            if (_oldStrafeRight != _strafeRight)
            {
                KeyT.Wait();
                KeyT.Reset();
                if (_strafeRight)
                    KeyHelper.PressKey("E");
                else
                    KeyHelper.ReleaseKey("E");
                //PLazyBot.Scripter.WriteLine("RotateLeft: " + rotateLeft);
            }
            if (_oldRotateLeft != _rotateLeft)
            {
                KeyT.Wait();
                KeyT.Reset();
                if (_rotateLeft)
                    KeyHelper.PressKey("Left");
                else
                    KeyHelper.ReleaseKey("Left");
                //PLazyBot.Scripter.WriteLine("RotateLeft: " + rotateLeft);
            }
            if (_oldRotateRight != _rotateRight)
            {
                KeyT.Wait();
                KeyT.Reset();
                if (_rotateRight)
                    KeyHelper.PressKey("Right");
                else
                    KeyHelper.ReleaseKey("Right");
                //PLazyBot.Scripter.WriteLine("RotateRight: " + rotateRight);
            }

            _oldRunForwards = _runForwards;
            _oldRunBackwards = _runBackwards;
            _oldStrafeLeft = _strafeLeft;
            _oldStrafeRight = _strafeRight;
            _oldRotateLeft = _rotateLeft;
            _oldRotateRight = _rotateRight;
            _oldDown = _down;
        }

        public static float NegativeValue(float value)
        {
            if (value < 0)
                value = value*-1;
            return value;
        }

        /// <summary>
        ///   Negatives the angle.
        /// </summary>
        /// <param name = "angle">The angle.</param>
        /// <returns></returns>
        public static float NegativeAngle(float angle)
        {
            //if the turning angle is negative
            if (angle < 0)
                //add the maximum possible angle (PI x 2) to normalize the negative angle
                angle += (float) (Math.PI*2);
            return angle;
        }

        public static void Jump()
        {
            KeyHelper.SendKey("Space");
        }

        public static void Jump(int milli)
        {
            KeyHelper.PressKey("Space");
            Thread.Sleep(milli);
            KeyHelper.ReleaseKey("Space");
        }

        public static void Down(int milli)
        {
            KeyHelper.PressKey("X");
            Thread.Sleep(milli);
            KeyHelper.ReleaseKey("X");
        }

        public static void ResyncKeys()
        {
            KeyT.ForceReady();
            PushKeys();
        }

        public static void MoveRandom()
        {
            int d = Ran.Next(4);
            if (d == 0 || d == 1)
                Forwards(true);
            if (d == 1)
                StrafeRight(true);
            if (d == 2 || d == 3)
                Backwards(true);
            if (d == 3)
                StrafeLeft(true);
        }

        public static void StrafeLeft(bool go)
        {
            _strafeLeft = go;
            if (go)
                _strafeRight = false;
            PushKeys();
        }

        public static void StrafeRight(bool go)
        {
            _strafeRight = go;
            if (go)
                _strafeLeft = false;
            PushKeys();
        }

        public static void RotateLeft(bool go)
        {
            _rotateLeft = go;
            if (go)
                _rotateRight = false;
            PushKeys();
        }


        public static void RotateRight(bool go)
        {
            _rotateRight = go;
            if (go)
                _rotateLeft = false;
            PushKeys();
        }

        public static void Forwards(bool go)
        {
            _runForwards = go;
            if (go)
                _runBackwards = false;
            PushKeys();
        }

        public static void Backwards(bool go)
        {
            _runBackwards = go;
            if (go)
                _runForwards = false;
            PushKeys();
        }

        public static void Down(bool go)
        {
            _down = go;
            PushKeys();
        }

        public static void StopRotate()
        {
            _rotateLeft = false;
            _rotateRight = false;
            PushKeys();
        }

        public static void StopMove()
        {
            _runForwards = false;
            _runBackwards = false;
            _strafeLeft = false;
            _strafeRight = false;
            _rotateLeft = false;
            _rotateRight = false;
            _down = false;
            PushKeys();
            KeyHelper.ReleaseKey("X");
            KeyHelper.ReleaseKey("Space");
        }

        public static void Stop()
        {
            StopMove();
            StopRotate();
        }

        public bool IsMoving()
        {
            return _runForwards || _runBackwards || _strafeLeft || _strafeRight;
        }

        public bool IsRotating()
        {
            return _rotateLeft || _rotateRight;
        }

        public bool IsRotatingLeft()
        {
            return _rotateLeft;
        }

        public bool IsRotatingRight()
        {
            return _rotateLeft;
        }

        public static bool MoveToUnit(PUnit targetObject, double distance, bool dummy)
        {
            return MoveToUnit(targetObject, distance);
        }

        /// <summary>
        ///   Moves to the unit.
        /// </summary>
        /// <param name = "targetObject">The unit to approach.</param>
        /// <param name = "distance">The distance.</param>
        /// <returns>Returns true on sucess</returns>
        public static bool MoveToUnit(PUnit targetObject, double distance)
        {
            //Start by facing
            Location oldPos = ObjectManager.MyPlayer.Location;
            Forwards(false);
            var timer = new Ticker(1000*1.1);
            var timerWaypoint = new Ticker(1000*16);
            int stuck = 0;
            while (targetObject.DistanceToSelf > distance && !timerWaypoint.IsReady)
            {
                targetObject.Face();
                Forwards(true);
                if (ObjectManager.MyPlayer.Location.GetDistanceTo(oldPos) > 1)
                {
                    oldPos = ObjectManager.MyPlayer.Location;
                    timer.Reset();
                }
                Forwards(true);
                if (ObjectManager.MyPlayer.Location.GetDistanceTo(oldPos) < 1 && timer.IsReady)
                {
                    if (stuck > 3)
                        return false;
                    Logging.Write("[Move]I am stuck " + stuck);
                    switch (stuck)
                    {
                        case 0:
                            Forwards(false);
                            Forwards(true);
                            Thread.Sleep(50);
                            Jump();
                            Thread.Sleep(800);
                            break;
                        case 1:
                            Forwards(false);
                            Forwards(true);
                            StrafeLeft(true);
                            Thread.Sleep(800);
                            break;
                        case 2:
                            Forwards(false);
                            Forwards(true);
                            StrafeLeft(true);
                            Thread.Sleep(800);
                            break;
                        case 3:
                            Forwards(false);
                            Forwards(true);
                            StrafeRight(true);
                            Thread.Sleep(800);
                            break;
                    }
                    Thread.Sleep(200);
                    stuck++;
                    ReleaseKeys();
                    Thread.Sleep(500);
                    timer.Reset();
                }
                Thread.Sleep(10);
            }
            if (timerWaypoint.IsReady && targetObject.DistanceToSelf > distance)
            {
                Logging.Write("Approach: " + targetObject.Name + " failed");
                Forwards(false);
                return false;
            }
            Forwards(false);
            ReleaseKeys();
            targetObject.Face();
            return true;
        }

        /// <summary>
        ///   Moves to the location
        /// </summary>
        /// <param name = "targetObject">The target object.</param>
        /// <param name = "distance">The distance.</param>
        /// <returns></returns>
        public static bool MoveToLoc(Location targetObject, double distance)
        {
            return MoveToLoc(targetObject, distance, false, false);
        }

        /// <summary>
        ///   Moves to the location
        /// </summary>
        /// <param name = "targetObject">The target object.</param>
        /// <param name = "distance">The distance.</param>
        /// <param name = "continueMove"></param>
        /// <param name = "breakOnCombat"></param>
        /// <returns></returns>
        public static bool MoveToLoc(Location targetObject, double distance, bool continueMove, bool breakOnCombat)
        {
            Location oldPos = ObjectManager.MyPlayer.Location;
            var timer = new Ticker(1000*1.1);
            var timerWaypoint = new Ticker(1000*30);
            int stuck = 0;
            while (targetObject.DistanceToSelf > distance && !timerWaypoint.IsReady)
            {
                targetObject.Face();
                Forwards(true);
                if (ObjectManager.MyPlayer.Location.GetDistanceTo(oldPos) > 1)
                {
                    oldPos = ObjectManager.MyPlayer.Location;
                    timer.Reset();
                }
                if (breakOnCombat && ObjectManager.GetAttackers.Count != 0 && ObjectManager.ShouldDefend)
                {
                    Forwards(false);
                    return false;
                }
                Forwards(true);
                if (ObjectManager.MyPlayer.Location.GetDistanceTo(oldPos) < 1 && timer.IsReady)
                {
                    if (stuck > 3)
                        return false;
                    Logging.Write("[Move]I am stuck " + stuck);
                    switch (stuck)
                    {
                        case 0:
                            Forwards(false);
                            Forwards(true);
                            Thread.Sleep(50);
                            Jump();
                            Thread.Sleep(800);
                            break;
                        case 1:
                            Forwards(false);
                            Forwards(true);
                            StrafeLeft(true);
                            Thread.Sleep(800);
                            break;
                        case 2:
                            Forwards(false);
                            Forwards(true);
                            StrafeLeft(true);
                            Thread.Sleep(800);
                            break;
                        case 3:
                            Forwards(false);
                            Forwards(true);
                            StrafeRight(true);
                            Thread.Sleep(800);
                            break;
                    }
                    Thread.Sleep(200);
                    stuck++;
                    ReleaseKeys();
                    Thread.Sleep(500);
                    timer.Reset();
                }
                Thread.Sleep(10);
            }
            if (timerWaypoint.IsReady && targetObject.DistanceToSelf > distance)
            {
                Logging.Write("Approach: " + targetObject + " failed");
                Forwards(false);
                return false;
            }
            if (!continueMove)
            {
                Forwards(false);
                ReleaseKeys();
            }
            targetObject.Face();
            return true;
        }
    }
}