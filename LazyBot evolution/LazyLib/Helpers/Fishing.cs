
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
using System.Runtime.InteropServices;
using System.Threading;
using LazyLib.Wow;

namespace LazyLib.Helpers
{
    public class Fishing
    {
        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);
        private static bool _triedWorldToScreen;
        private static bool _tryingSearch;
        private static bool _saidSomethingManager;
        private static readonly Ticker TimeOut = new Ticker(2000);
        public static bool FindBobberAndClick(bool waitForLoot)
        {
            PGameObject bobber = null;
            Thread.Sleep(1000);
            _triedWorldToScreen = false;
            _saidSomethingManager = false;
            _tryingSearch = false;
            while(ObjectManager.MyPlayer.IsCasting)
            {
                if (bobber != null)
                {
                    if (!_saidSomethingManager)
                    {
                        Logging.Write("Located bobber in objectmanager");
                        _saidSomethingManager = true;
                    }
                    if(LazySettings.BackgroundMode)
                    {
                        if (bobber.IsBobbing)
                        {
                            bobber.Interact(false);
                            Thread.Sleep(1500);
                            if (waitForLoot)
                            {
                                while (ObjectManager.MyPlayer.LootWinOpen && !TimeOut.IsReady)
                                    Thread.Sleep(100);
                                Thread.Sleep(1300);
                            }
                            return true;
                        }
                    } else
                    {
                        if (!Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(bobber.GUID))
                        {
                            if (!_triedWorldToScreen)
                            {
                                Logging.Write("Trying world to screen");
                                FindTheBobber(bobber.Location, bobber.GUID);
                                _triedWorldToScreen = true;
                            } else
                            {
                                if (!_tryingSearch)
                                {
                                    Logging.Write("Trying search");
                                    _tryingSearch = true;
                                }
                                FindBobberSearch();
                            }
                            Thread.Sleep(100);
                        } else
                        {
                            if (bobber.IsBobbing)
                            {
                                KeyHelper.SendKey("InteractWithMouseOver");
                                Thread.Sleep(1500);
                                if (waitForLoot)
                                {
                                    while (ObjectManager.MyPlayer.LootWinOpen && !TimeOut.IsReady)
                                        Thread.Sleep(100);
                                    Thread.Sleep(1300);
                                }
                                return true;
                            }
                        }
                    }
                    Thread.Sleep(1000);
                } else
                {
                    bobber = Bobber();
                }
                Thread.Sleep(100);
            }
            return false;
        }

        private static void FindBobberSearch()
        {
            GamePosition.Findpos(Memory.WindowHandle);
            int xOffset = -200;
            int yOffset = -200;
            while (!IsMouseOverBobber())
            {
                MoveMouse(GamePosition.GetCenterX + xOffset, GamePosition.GetCenterY + yOffset);
                Thread.Sleep(10);
                if (IsMouseOverBobber())
                {
                    return;
                }
                xOffset = xOffset + 10;
                if (xOffset >= 200)
                {
                    yOffset = yOffset + 10;
                    xOffset = -200;
                    if (yOffset > 200)
                    {
                        break;
                    }
                }
            }
        }

        private static void FindTheBobber(Location loc, ulong guid)
        {
            Point worldToScreen = Camera.World2Screen.WorldToScreen(loc, true);
            MoveMouse(worldToScreen.X, worldToScreen.Y);
            Thread.Sleep(50);
            if (IsMouseOverBobber())
                return;
            MoveMouse(worldToScreen.X, worldToScreen.Y - 15);
            Thread.Sleep(50);
            if (IsMouseOverBobber())
                return;
            MoveMouse(worldToScreen.X, worldToScreen.Y + 15);
            Thread.Sleep(50);
            if (IsMouseOverBobber())
                return;
            Thread.Sleep(50);
            MoveMouse(worldToScreen.X - 15, worldToScreen.Y);
            Thread.Sleep(50);
            if (IsMouseOverBobber())
                return;
            MoveMouse(worldToScreen.X + 15, worldToScreen.Y);
            Thread.Sleep(50);
            if (IsMouseOverBobber())
                return;
            MoveMouse(worldToScreen.X, worldToScreen.Y + 35);
            Thread.Sleep(50);
            if (IsMouseOverBobber())
                return;
            MoveMouse(worldToScreen.X, worldToScreen.Y + 40);
            Thread.Sleep(50);
            if (IsMouseOverBobber())
                return;
            MoveMouse(worldToScreen.X, worldToScreen.Y + 45);
            Thread.Sleep(50);
            if (IsMouseOverBobber())
                return;
            MoveMouse(worldToScreen.X + 35, worldToScreen.Y);
            Thread.Sleep(50);
            if (IsMouseOverBobber())
                return;
            MoveMouse(worldToScreen.X + 40, worldToScreen.Y);
            Thread.Sleep(50);
            if (IsMouseOverBobber())
                return;
            MoveMouse(worldToScreen.X + 45, worldToScreen.Y);
            Thread.Sleep(50);
            if (IsMouseOverBobber())
                return;

            int xOffset =  -60;
            int yOffset =  -60;
            MoveMouse(worldToScreen.X, worldToScreen.Y);
            while (!IsMouseOverBobber())
            {
                MoveMouse(worldToScreen.X + xOffset, worldToScreen.Y + yOffset);
                Thread.Sleep(10);
                if (IsMouseOverBobber())
                {
                    return;
                }
                xOffset = xOffset + 10;
                if (xOffset >= 60)
                {
                    yOffset = yOffset + 10;
                    xOffset = -60;
                    if (yOffset > 60)
                    {
                        break;
                    }
                }
            }
        }

        private static bool IsMouseOverBobber()
        {
            if(Memory.ReadRelative<uint>((uint) Pointers.Globals.CursorType) == 53)
            {
                return true;
            }
            return false;
        }

        private static void MoveMouse(int x, int y)
        {
            if (LazySettings.HookMouse)
            {
                MouseHelper.MoveMouseToPosHooked(x, y);
            }
            else
            {
                SetCursorPos(x, y);
            }
        }
               
        private static PGameObject Bobber()
        {
            return ObjectManager.GetGameObject.FirstOrDefault(pGameObject => pGameObject.DisplayId == 0x29c);
        }
    }
}