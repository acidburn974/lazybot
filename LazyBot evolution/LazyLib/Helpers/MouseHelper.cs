
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
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using LazyLib.Wow;

namespace LazyLib.Helpers
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class MouseMoveMessasge : EventArgs
    {
        public int X;
        public int Y;
    }

    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class MouseBlocMessasge : EventArgs
    {
        public bool Block;
    }

    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class MouseHelper
    {
        private static Boolean _locked;
        public static event EventHandler<MouseMoveMessasge> MouseMoveMessage;
        public static event EventHandler<MouseBlocMessasge> MouseBlockMessage;
        public delegate Point FunctionToCall();
        internal static FunctionToCall GetPosFromDelegate;
        //[DllImport("user32.dll")]
        //private static extern bool BlockInput(bool fBlockIt);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);

        // ReSharper disable InconsistentNaming
        private const int iRestore = 9;
        private const int iShow = 5;

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr Hwnd);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr Hwnd, int iCmdShow);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr Hwnd);
        // ReSharper restore InconsistentNaming
        public static void SetDelegate(FunctionToCall function)
        {
            GetPosFromDelegate = function;
        }

        public static void BlockInput(bool block)
        {
            if (MouseBlockMessage != null)
            {
                MouseBlockMessage(new object(), new MouseBlocMessasge { Block = block });
            } else
            {
                IntPtr hwnd = Memory.WindowHandle;
                if (hwnd.ToInt32() > 0)
                {
                    SetForegroundWindow(hwnd);
                    ShowWindow(hwnd, IsIconic(hwnd) ? iRestore : iShow);
                }
            }
        }

        public static Point MousePosition
        {
            get
            {
                if (LazySettings.HookMouse)
                {
                    //Logging.Write("" + GetPosFromDelegate());
                    Point debug = GetPosFromDelegate();
                    return GetPosFromDelegate();
                }
                return Cursor.Position;
            }
        }

        public static void MoveMouseToPos(Point p)
        {
            MoveMouseToPos(p.X, p.Y);
        }

        public static void MoveMouseToPos(int x, int y)
        {
            BlockInput(true);
            _locked = true;

            MoveTheCursor(x, y);

            KeyLowHelper.SendMessage(Memory.WindowHandle, 0x200, (IntPtr)0, (IntPtr)0);
            WaitFrameReload();
        }

        private static void MoveTheCursor(int x, int y)
        {
            if (MouseMoveMessage != null)
            {
                MouseMoveMessage(new object(), new MouseMoveMessasge { X = x, Y = y });
            }
            else
            {
                SetCursorPos(x, y);
            }
        }

        public static void ReleaseMouse()
        {
            if (_locked)
            {
                BlockInput(false);
                _locked = false;
            }
        }

        public static void Hook()
        {
            BlockInput(true);
            _locked = true;
        }

        public static void MoveMouseToPosHooked(int x, int y)
        {
            MoveTheCursor(x, y);
            KeyLowHelper.SendMessage(Memory.WindowHandle, 0x200, (IntPtr)0, (IntPtr)0);
        }

        private static void LeftDown()
        {
            KeyLowHelper.SendMessage(Memory.WindowHandle, 0x200, (IntPtr)0, (IntPtr)0);
            KeyLowHelper.SendMessage(Memory.WindowHandle, 0x201, (IntPtr)0, (IntPtr)0);
            WaitFrameReload();
        }

        private static void LeftUp()
        {
            KeyLowHelper.SendMessage(Memory.WindowHandle, 0x200, (IntPtr)0, (IntPtr)0);
            KeyLowHelper.SendMessage(Memory.WindowHandle, 0x202, (IntPtr)0, (IntPtr)0);
            WaitFrameReload();
        }

        public static void LeftClick()
        {
            LeftDown();
            LeftUp();
        }

        private static void RightDown()
        {
            KeyLowHelper.SendMessage(Memory.WindowHandle, 0x200, (IntPtr)0, (IntPtr)0);
            KeyLowHelper.SendMessage(Memory.WindowHandle, 0x204, (IntPtr)0, (IntPtr)0);
            WaitFrameReload();
        }

        private static void RightUp()
        {
            KeyLowHelper.SendMessage(Memory.WindowHandle, 0x200, (IntPtr)0, (IntPtr)0);
            KeyLowHelper.SendMessage(Memory.WindowHandle, 0x205, (IntPtr)0, (IntPtr)0);
            WaitFrameReload();
        }

        public static void RightClick()
        {
            RightDown();
            RightUp();
            //Logging.Write("Right click");
        }

        private static readonly Ticker TimeOut = new Ticker(25);
        public static void WaitFrameReload()
        {
            TimeOut.Reset();
            while (!TimeOut.IsReady)
            {
                Thread.Sleep(1);
            }
        }
    }
}