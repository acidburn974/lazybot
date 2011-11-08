
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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using LazyLib.Wow;

namespace LazyLib.Helpers
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class Frame
    {
        public UInt32 BaseAddress;

        public Frame(UInt32 baseAddress)
        {
            BaseAddress = baseAddress;
        }

        /*
        public Boolean IsMouseOver
        {
            get
            {
                return (FrameMouseOverPtr == this.baseAddress);
            }
        } */

        public bool IsVisible
        {
            get
            {
                return ((Memory.Read<int>(BaseAddress + (uint) Pointers.UiFrame.Visible) >> (int) Pointers.UiFrame.Visible1) & (int) Pointers.UiFrame.Visible2) == 1;
            }
        }

        public bool IsButtonChecked
        {
            get { return (Memory.Read<uint>(BaseAddress + (uint) Pointers.UiFrame.ButtonChecked) > 0); }
        }

        public bool IsEnabled
        {
            get
            {
                return ((Memory.Read<uint>(BaseAddress + (uint) 200) & (uint) 0xF) == 0);
            }
        }

        //TODO: Optimize this paste from ida
        public bool Enabled
        {
            get
            {
                int num1 = Memory.Read<byte>(BaseAddress + (uint) Pointers.UiFrame.ButtonEnabledPointer);
                const int num4 = (int) Pointers.UiFrame.ButtonEnabledMask;
                num1 &= num4;
                const int num5 = 0;
                num1 = num1 == num5 ? 1 : 0;
                int num3 = num1 == 0 ? 1 : 0;
                return num3 != 0;
            }
        }


        public string GetName
        {
            get { return Memory.ReadUtf8(Memory.Read<uint>(BaseAddress + (uint)Pointers.UiFrame.Name), 132); }
        }

        public string GetText
        {
            get { return Memory.ReadUtf8(Memory.Read<uint>(BaseAddress + (uint) Pointers.UiFrame.LabelText), byte.MaxValue); }
        }

        public string GetEditBoxText
        {
            get { return Memory.ReadUtf8(Memory.Read<uint>(BaseAddress + (uint)Pointers.UiFrame.EditBoxText), byte.MaxValue); }
        }

        internal Single Left
        {
            get
            {
                var a1 = Memory.Read<float>(BaseAddress + (uint) Pointers.UiFrame.FrameLeft);
                return (a1*InterfaceHelper.WindowWidth/Memory.ReadRelative<float>((uint) Pointers.UiFrame.ScrWidth));
            }
        }

        internal Single Right
        {
            get
            {
                var a1 = Memory.Read<float>(BaseAddress + (uint) Pointers.UiFrame.FrameRight);
                return (a1*InterfaceHelper.WindowWidth/Memory.ReadRelative<float>((uint) Pointers.UiFrame.ScrWidth));
            }
        }

        internal Single Top
        {
            get
            {
                var a1 = Memory.Read<float>(BaseAddress + (uint) Pointers.UiFrame.FrameTop);
                return (a1*InterfaceHelper.WindowHeight/Memory.ReadRelative<float>((uint) Pointers.UiFrame.ScrHeight));
            }
        }

        internal Single Bottom
        {
            get
            {
                var a1 = Memory.Read<float>(BaseAddress + (uint) Pointers.UiFrame.FrameBottom);
                return (a1*InterfaceHelper.WindowHeight/Memory.ReadRelative<float>((uint) Pointers.UiFrame.ScrHeight));
            }
        }

        internal Single Width
        {
            get { return Right - Left; }
        }

        internal Single Height
        {
            get { return Top - Bottom; }
        }

        public Int32 CenterX
        {
            get
            {
                var p = new Point();
                ScreenToClient(Memory.WindowHandle, ref p);
                return Math.Abs(p.X) + (int) Left + (int) (Width/2);
            }
        }

        public Int32 CenterY
        {
            get
            {
                var p = new Point();
                ScreenToClient(Memory.WindowHandle, ref p);
                return (Math.Abs(p.Y) + InterfaceHelper.WindowHeight - (int) Top + (int) (Height/2));
            }
        }

        public List<Frame> GetChilds
        {
            get
            {
                var result = new List<Frame>();

                var child = Memory.Read<uint>(BaseAddress + (uint) Pointers.UiFrame.RegionsFirst);

                while (child != 0 && (child & 1) == 0)
                {
                    String s = Memory.ReadUtf8(Memory.Read<uint>(child + (uint)Pointers.UiFrame.Name), 99);

                    if (s.Length > 0)
                        result.Add(new Frame(child));

                    child =
                        Memory.Read<uint>(child + Memory.Read<uint>(BaseAddress + (uint) Pointers.UiFrame.RegionsNext) +
                                          4);
                }

                return result;
            }
        }

        [DllImport("user32")]
        internal static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

        public void LeftClick()
        {
            MouseHelper.MoveMouseToPos(CenterX, CenterY);
            MouseHelper.LeftClick();
            MouseHelper.ReleaseMouse();
        }

        public void RightClick()
        {
            MouseHelper.MoveMouseToPos(CenterX, CenterY);
            MouseHelper.RightClick();
            MouseHelper.ReleaseMouse();
        }

        public void HoverHooked()
        {
            MouseHelper.MoveMouseToPosHooked(CenterX, CenterY);
        }

        public void LeftClickHooked()
        {
            MouseHelper.MoveMouseToPosHooked(CenterX, CenterY);
            MouseHelper.LeftClick();
        }

        public void RightClickHooked()
        {
            MouseHelper.MoveMouseToPosHooked(CenterX, CenterY);
            MouseHelper.RightClick();
        }

        public Frame GetChildObject(String name)
        {
            return GetChilds.FirstOrDefault(f => f.GetName == name);
        }

        public void SetEditBoxText(string text)
        {
            LeftClick();
            if (GetEditBoxText.Length > 0)
            {
                Thread.Sleep(250);
                KeyLowHelper.PressKey(MicrosoftVirtualKeys.VK_LCONTROL);
                KeyLowHelper.PressKey(MicrosoftVirtualKeys.A);
                KeyLowHelper.ReleaseKey(MicrosoftVirtualKeys.A);
                KeyLowHelper.ReleaseKey(MicrosoftVirtualKeys.VK_LCONTROL);
                Thread.Sleep(200);
                KeyLowHelper.PressKey(MicrosoftVirtualKeys.Delete);
                KeyLowHelper.ReleaseKey(MicrosoftVirtualKeys.Delete);
                for (int i = 0; i < 8; i++)
                {
                    KeyLowHelper.PressKey(MicrosoftVirtualKeys.Back);
                    Thread.Sleep(100);
                }
                KeyLowHelper.ReleaseKey(MicrosoftVirtualKeys.Back);
                Thread.Sleep(550);
            }
            KeyHelper.SendTextNow(text);
            Thread.Sleep(500);
        }

        public void SetEditBoxTextHooked(string text)
        {
            LeftClickHooked();
            if (GetEditBoxText.Length > 0)
            {
                Thread.Sleep(250);
                KeyLowHelper.PressKey(MicrosoftVirtualKeys.VK_LCONTROL);
                KeyLowHelper.PressKey(MicrosoftVirtualKeys.A);
                KeyLowHelper.ReleaseKey(MicrosoftVirtualKeys.A);
                KeyLowHelper.ReleaseKey(MicrosoftVirtualKeys.VK_LCONTROL);
                Thread.Sleep(200);
                KeyLowHelper.PressKey(MicrosoftVirtualKeys.Delete);
                KeyLowHelper.ReleaseKey(MicrosoftVirtualKeys.Delete);
                for (int i = 0; i < 8; i++)
                {
                    KeyLowHelper.PressKey(MicrosoftVirtualKeys.Back);
                    Thread.Sleep(100);
                }
                KeyLowHelper.ReleaseKey(MicrosoftVirtualKeys.Back);
                Thread.Sleep(550);
            }
            KeyHelper.SendTextNow(text);
            Thread.Sleep(500);
        }
    }
}