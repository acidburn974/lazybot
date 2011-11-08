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
using LazyLib.Wow;

#endregion

namespace LazyEvo.LFlyingEngine.Helpers
{
    internal class Stuck
    {
        private static bool _pIsStuck;
        private static Thread _thread = new Thread(Loop);

        private static float _oldX;
        private static float _oldY;
        private static float _oldZ;
        private static float _dx;
        private static float _dy;
        private static float _dz;

        public static bool IsStuck
        {
            get
            {
                if (ObjectManager.MyPlayer.Speed == 0 || !ObjectManager.MyPlayer.IsMounted)
                    return false;

                return _pIsStuck;
            }
        }

        public static void Reset()
        {
            _oldX = 0;
            _oldY = 0;
            _oldZ = 0;
        }

        private static void Loop()
        {
            try
            {
                while (true)
                {
                    if (ObjectManager.MyPlayer.Speed == 0)
                    {
                        Thread.Sleep(1000);
                        _oldX = 0;
                        _oldY = 0;
                        _oldZ = 0;
                        continue;
                    }
                    try
                    {
                        _oldX = ObjectManager.MyPlayer.X;
                        _oldY = ObjectManager.MyPlayer.Y;
                        _oldZ = ObjectManager.MyPlayer.Z;

                        Thread.Sleep(1200);
                        if (ObjectManager.MyPlayer.Speed == 0)
                        {
                            continue;
                        }
                        _dx = _oldX - ObjectManager.MyPlayer.X;
                        _dy = _oldY - ObjectManager.MyPlayer.Y;
                        _dz = _oldZ - ObjectManager.MyPlayer.Z;

                        Double distance = Math.Sqrt((_dx*_dx) + (_dy*_dy) + (_dz*_dz));
                        _pIsStuck = distance < 2;
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            catch
            {
            }
        }

        private static void Start()
        {
            try
            {
                if (_thread != null && _thread.IsAlive)
                    return;
                _thread = new Thread(Loop) {IsBackground = true};
                _thread.Name = "Stuck";
                _thread.Start();
            }
            catch
            {
            }
        }

        internal static void Stop()
        {
            try
            {
                if (_thread != null)
                    if (_thread.IsAlive)
                        _thread.Abort();

                _pIsStuck = false;
            }
            catch
            {
            }
            _pIsStuck = false;
        }

        internal static void Run()
        {
            Start();
            /*
            try
            {
                while(true)
                {
                    try
                    {
                        if (ObjectManager.MyPlayer.Speed > 0)
                            Start();
                        else
                            Stop();
                    }
                    catch
                    {
                        //Empty
                    }
                    Thread.Sleep(200);
                }
            } catch
            {
                //Empty
            } */
        }
    }
}