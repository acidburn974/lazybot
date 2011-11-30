
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
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using LazyLib.Helpers;

#endregion

namespace LazyLib.Wow
{
    /// <summary>
    ///   Contains all information for a WowObject.
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class PObject
    {
        private static Point _oldPoint;

        public PObject(uint baseAddress)
        {
            BaseAddress = baseAddress;
        }

        public uint BaseAddress { get; set; }

        /// <summary>
        ///   Object's GUID.
        /// </summary>
        public virtual ulong GUID
        {
            get
            {
                if (IsValid)
                    return GetStorageField<ulong>((uint) Descriptors.eObjectFields.OBJECT_FIELD_GUID);
                return ulong.MinValue;
            }
        }

        public bool IsValid
        {
            get { return BaseAddress != uint.MinValue; }
        }

        /// <summary>
        ///   Object's Type.
        /// </summary>
        public int Type
        {
            get { return Memory.Read<int>(BaseAddress + 0x14); }
        }

        /// <summary>
        ///   Object's Entry.
        /// </summary>
        public int Entry
        {
            get { return GetStorageField<int>((uint) Descriptors.eObjectFields.OBJECT_FIELD_ENTRY); }
        }

        /// <summary>
        ///   Object's level.
        /// </summary>
        public int Level
        {
            get { return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_LEVEL); }
        }


        /// <summary>
        ///   Returns the object's X position.
        /// </summary>
        public virtual float X
        {
            get
            {
                try
                {
                    return Memory.Read<float>(BaseAddress + (uint)Pointers.WowObject.X);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///   Returns the object's Y position.
        /// </summary>
        public virtual float Y
        {
            get
            {
                try
                {
                    return Memory.Read<float>(BaseAddress + (uint)Pointers.WowObject.Y);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///   Returns the object's Z position.
        /// </summary>
        public virtual float Z
        {
            get
            {
                try
                {
                    return Memory.Read<float>(BaseAddress + (uint)Pointers.WowObject.Z);
                }
                catch
                {
                    return 0;
                }
            }
        }


        /// <summary>
        ///   Gets the facing in radins
        /// </summary>
        /// <value>The facing.</value>
        public virtual float Facing
        {
            get
            {
                try
                {
                    return Memory.Read<float>(BaseAddress + (uint)Pointers.WowObject.RotationOffset);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///   Gets the location.
        /// </summary>
        /// <value>The location.</value>
        public virtual Location Location
        {
            get { return new Location(X, Y, Z); }
        }

        /// <summary>
        ///   Determines if the unit is our local player.
        /// </summary>
        public bool IsMe
        {
            get { return GUID == ObjectManager.MyPlayer.GUID ? true : false; }
        }

        #region <Storage Field Methods>

        public uint StorageField
        {
            get { return Memory.Read<uint>(BaseAddress + 0xc); }
        }

        protected T GetStorageField<T>(uint field) where T : struct
        {
            try
            {
               // field = field *4; //TODO ENABLE IF OFFSETS IS NOT MULTIPLIED BY 0x4
                var m_pStorage = Memory.Read<uint>(BaseAddress + 0xc);

                return (T) Memory.ReadObject(m_pStorage + field, typeof (T));
            }
            catch (Exception e)
            {
                Console.WriteLine("DO NOT POST THIS WARNING ON THE FORUM! ONLY DEBUG!: " + e);
                return new T();
            }
        }

        protected T GetStorageField<T>(Descriptors.eObjectFields field) where T : struct
        {
            try
            {
                return GetStorageField<T>((uint) field);
            }
            catch (Exception e)
            {
                Console.WriteLine("DO NOT POST THIS WARNING ON THE FORUM! ONLY DEBUG!: " + e);
                return new T();
            }
        }

        #endregion

        #region <ToUnit()>

        public PUnit ToUnit(PObject obj)
        {
            return new PUnit(obj.BaseAddress);
        }

        public PUnit ToUnit(PItem obj)
        {
            return new PUnit(obj.BaseAddress);
        }

        #endregion

        #region <ToPlayer()>

        public PPlayer ToPlayer(PItem obj)
        {
            return new PPlayer(obj.BaseAddress);
        }

        public PPlayer ToPlayer(PUnit obj)
        {
            return new PPlayer(obj.BaseAddress);
        }

        public PPlayer ToPlayer(PObject obj)
        {
            return new PPlayer(obj.BaseAddress);
        }

        public PPlayer ToPlayer(PPlayer obj)
        {
            return new PPlayer(obj.BaseAddress);
        }

        #endregion

        #region <ToGameObject()>

        public PGameObject ToGameObject(PObject obj)
        {
            return new PGameObject(obj.BaseAddress);
        }

        #endregion

        internal void UpdateBaseAddress(uint address)
        {
            BaseAddress = address;
        }

        public bool Interact(bool multiclick)
        {
            return InteractOrTarget(multiclick);
        }
        
        public void LeftClick()
        {
            if (!LazySettings.HookMouse)
            {
                SetForGround();
            }
            Point worldToScreen = Camera.World2Screen.WorldToScreen(Location, true);
            MoveMouse(worldToScreen.X, worldToScreen.Y);
            Thread.Sleep(50);
            MouseHelper.LeftClick();
            Thread.Sleep(50);
        }

        public bool InteractOrTarget(bool multiclick)
        {
            if (ObjectManager.MyPlayer.TargetGUID == GUID)
            {
                  KeyHelper.SendKey("InteractWithMouseOver");
                  return true;
            }
            if (!LazySettings.BackgroundMode)
            {
                _oldPoint.X = Cursor.Position.X;
                _oldPoint.Y = Cursor.Position.Y;
                MouseHelper.Hook();
                if(!LazySettings.HookMouse)
                {
                    SetForGround();
                }
                FindUsingWorldToScreen();
                bool toReturn = LetsSearch(GUID, multiclick, true);
                MoveMouse(_oldPoint.X, _oldPoint.Y);
                MouseHelper.ReleaseMouse();
                return toReturn;
            }
            //We are using background mode lets do it the easy way.
            Memory.Write(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, GUID);
            Thread.Sleep(50);
            KeyHelper.SendKey("InteractWithMouseOver");
            Thread.Sleep(500);
            if (ObjectManager.MyPlayer.TargetGUID.Equals(GUID))
                return true;
            return false;
        }

        public bool MouseOver()
        {
            FindUsingWorldToScreen();
            bool toReturn = LetsSearch(GUID, false, false);
            return toReturn;
        }

        private void FindUsingWorldToScreen()
        {
            Point worldToScreen = Camera.World2Screen.WorldToScreen(Location, true);
            MoveMouse(worldToScreen.X, worldToScreen.Y);
            Thread.Sleep(50);
            if (Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(GUID))
                return;
            MoveMouse(MouseHelper.MousePosition.X, MouseHelper.MousePosition.Y - 15);
            Thread.Sleep(50);
            if (Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(GUID))
                return;
            MoveMouse(MouseHelper.MousePosition.X, MouseHelper.MousePosition.Y + 15);
            Thread.Sleep(50);
            if (Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(GUID))
                return;
            Thread.Sleep(50);
            MoveMouse(MouseHelper.MousePosition.X - 15, MouseHelper.MousePosition.Y);
            Thread.Sleep(50);
            if (Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(GUID))
                return;
            MoveMouse(MouseHelper.MousePosition.X + 15, MouseHelper.MousePosition.Y);
            Thread.Sleep(50);
            if (Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(GUID))
                return;
            MoveMouse(MouseHelper.MousePosition.X, MouseHelper.MousePosition.Y + 35);
            Thread.Sleep(50);
            if (Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(GUID))
                return;
            MoveMouse(MouseHelper.MousePosition.X, MouseHelper.MousePosition.Y + 40);
            Thread.Sleep(50);
            if (Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(GUID))
                return;
            MoveMouse(MouseHelper.MousePosition.X, MouseHelper.MousePosition.Y + 45);
            Thread.Sleep(50);
            if (Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(GUID))
                return;
        }

        //TODO: Do something to this functions, its freaking ugly
        private static bool LetsSearch(ulong guid, bool multiclick, bool click)
        {
            if (!Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(guid))
            {
                GamePosition.Findpos(Memory.WindowHandle);
                if (!DoSmallestSearch(guid))
                    if (!Search(guid, GamePosition.Width/16))
                        if (!Search(guid, GamePosition.Width/12))
                            if (!Search(guid, GamePosition.Width/10))
                                if (!Search(guid, GamePosition.Width/8))
                                    if (!Search(guid, GamePosition.Width/6))
                                    {
                                        // DoSmallSearch(guid);
                                    }
            }
            if (ObjectManager.GetAttackers.Count != 0 && ObjectManager.ShouldDefend)
                return false;
            if (Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(guid))
            {
                if (click)
                {
                    if (!LazySettings.HookMouse)
                    {
                        SetForGround();
                    }
                    if (multiclick)
                    {
                        MoveMouse(MouseHelper.MousePosition.X, MouseHelper.MousePosition.Y - 15);
                        Thread.Sleep(50);
                        KeyHelper.SendKey("InteractWithMouseOver");
                        Thread.Sleep(50);
                        MoveMouse(MouseHelper.MousePosition.X, MouseHelper.MousePosition.Y + 15);
                        Thread.Sleep(50);
                        KeyHelper.SendKey("InteractWithMouseOver");
                        Thread.Sleep(50);
                        MoveMouse(MouseHelper.MousePosition.X - 15, MouseHelper.MousePosition.Y);
                        Thread.Sleep(50);
                        KeyHelper.SendKey("InteractWithMouseOver");
                        MoveMouse(MouseHelper.MousePosition.X + 15, MouseHelper.MousePosition.Y);
                        Thread.Sleep(50);
                        KeyHelper.SendKey("InteractWithMouseOver");
                        Thread.Sleep(50);
                    }
                    MoveMouse(MouseHelper.MousePosition.X, MouseHelper.MousePosition.Y);
                    Thread.Sleep(50);
                    KeyHelper.SendKey("InteractWithMouseOver");
                    Thread.Sleep(50);
                }
                return true;
            }
            return false;
        }

        private static bool DoSmallestSearch(ulong guid)
        {
            if (ObjectManager.ShouldDefend)
                return true;
            GamePosition.Findpos(Memory.WindowHandle);
            int xOffset = -40;
            int yOffset = -40;
            while (!Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(guid))
            {
                MoveMouse(GamePosition.GetCenterX + xOffset,GamePosition.GetCenterY + yOffset);
                Thread.Sleep(10);
                if (Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(guid))
                    break;
                xOffset = xOffset + 10;
                if (xOffset > 50)
                {
                    yOffset = yOffset + 10;
                    xOffset = -40;
                    if (yOffset > 50)
                    {
                        break;
                    }
                }
            }
            if (Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(guid))
                return true;
            return false;
        }


        private static bool Search(ulong guid, int yValue)
        {
            if (ObjectManager.ShouldDefend)
                return true;
            int xOffset = 0;
            int yOffset = -yValue;
            bool right = true;
            while (!Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(guid))
            {
                MoveMouse(GamePosition.GetCenterX + xOffset, GamePosition.GetCenterY + yOffset);
                Thread.Sleep(10);
                if (Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(guid))
                    break;
                if (right)
                {
                    if (yOffset < 0)
                        xOffset += 15;
                    else
                        xOffset -= 15;
                }
                else
                {
                    if (yOffset < 0)
                        xOffset -= 15;
                    else
                        xOffset += 15;
                }
                yOffset = yOffset + 8;
                if (yOffset > yValue)
                {
                    if (!right)
                    {
                        break;
                    }
                    yOffset = -yValue;
                    right = false;
                }
            }
            if (Memory.ReadObject(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, typeof(ulong)).Equals(guid))
                return true;
            return false;
        }

        private static void MoveMouse(int x, int y)
        {
            if (LazySettings.HookMouse)
            {
                MouseHelper.MoveMouseToPosHooked(x, y);
            } else
            {
                SetCursorPos(x, y);
            }
        }

        private static void SetForGround()
        {
            IntPtr hwnd = Memory.WindowHandle;
            if (hwnd.ToInt32() > 0)
            {
                SetForegroundWindow(hwnd);
                ShowWindow(hwnd, IsIconic(hwnd) ? iRestore : iShow);
            }
        }
        // ReSharper disable InconsistentNaming
        private const int iRestore = 9;
        private const int iShow = 5;

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr Hwnd);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr Hwnd, int iCmdShow);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr Hwnd);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);
    }
}