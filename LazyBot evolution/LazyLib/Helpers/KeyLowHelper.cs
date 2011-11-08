
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
using System.Runtime.InteropServices;

namespace LazyLib.Helpers
{
    public class KeyLowHelper
    {
        private const uint PressKeyCode = 0x100;
        private const uint ReleaseKeyCode = 0x101;

        public const uint WmKeydown = 0x0100;
        public const uint WmKeyup = 0x0101;
        public const uint WmChar = 0x0102;

        [DllImport("user32.dll", EntryPoint = "PostMessage")]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        ///   Send
        /// </summary>
        public static void PressKey(MicrosoftVirtualKeys key)
        {
            PostMessage(Memory.WindowHandle, PressKeyCode, (IntPtr)key, (IntPtr)0);
        }

        public static void ReleaseKey(MicrosoftVirtualKeys key)
        {
            PostMessage(Memory.WindowHandle, ReleaseKeyCode, (IntPtr)key, (IntPtr)0);
        }

        public static void SendEnter()
        {
            PostMessage(Memory.WindowHandle, PressKeyCode, (IntPtr)13, (IntPtr)0);
            PostMessage(Memory.WindowHandle, ReleaseKeyCode, (IntPtr)13, (IntPtr)0);
        }

        public static void SendV()
        {
            PostMessage(Memory.WindowHandle, PressKeyCode, (IntPtr)0x56, (IntPtr)0);
            PostMessage(Memory.WindowHandle, ReleaseKeyCode, (IntPtr)0x56, (IntPtr)0);
        }
    }
}