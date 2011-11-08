
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
///DO NOT EDIT THIS CLASSS!
namespace LazyLib.Helpers
{
    /// <summary>
    /// Declaration of an Rectangle(Wow window).
    /// </summary>
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    };

    internal class GamePosition
    {
        [DllImport("user32")]
        public static extern int GetWindowRect(IntPtr hwnd, out RECT lpRect);

        public static void Findpos(IntPtr hwnd)
        {
            RECT r;
            GetWindowRect(Memory.WindowHandle, out r);
            int x = r.left;
            int y = r.top;
            Width = (r.left - r.right)*-1;
            Height = (r.top - r.bottom)*-1;
            int centerWidth = (Width) / 2;
            int centerHeigth = (Height) / 2;
            GetCenterX = x + centerWidth;
            GetCenterY = y + centerHeigth;
        }

        public static int GetCenterX { get; private set; }
        public static int GetCenterY { get; private set; }
        public static int Width { get; private set; }
        public static int Height { get; private set; }
    }
}