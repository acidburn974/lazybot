
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

namespace LazyLib.Helpers
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class KeyWrapper
    {
        private const uint PressKeyCode = 0x100;
        private const uint ReleaseKeyCode = 0x101;
        private readonly MicrosoftVirtualKeys _bar;
        private readonly bool _shift;
        private readonly MicrosoftVirtualKeys _wParam;
        private readonly MicrosoftVirtualKeys _wParam2;

        //KeyName, ShiftState, BarState, Char
        public KeyWrapper(string keyName, string shiftState, string barState, string character)
        {
            Name = keyName;
            Key = character;
            Bar = barState;
            //Setup shift
            Special = shiftState;
            if (shiftState.Equals("Ctrl"))
            {
                _shift = true;
                _wParam = MicrosoftVirtualKeys.VK_LCONTROL;
            }
            else if (shiftState.Equals("Shift"))
            {
                _shift = true;
                _wParam = MicrosoftVirtualKeys.VK_SHIFT;
            }
            else if (shiftState.Equals("Alt"))
            {
                _shift = true;
                _wParam = MicrosoftVirtualKeys.Alt;
            }
            else
            {
                _shift = false;
            }
            //Setup barstate
            if (barState.Equals("1") || barState.Equals("Bar1"))
            {
                _bar = MicrosoftVirtualKeys.key1;
            }
            else if (barState.Equals("2") || barState.Equals("Bar2"))
            {
                _bar = MicrosoftVirtualKeys.key2;
            }
            else if (barState.Equals("3") || barState.Equals("Bar3"))
            {
                _bar = MicrosoftVirtualKeys.key3;
            }
            else if (barState.Equals("4") || barState.Equals("Bar4"))
            {
                _bar = MicrosoftVirtualKeys.key4;
            }
            else if (barState.Equals("5") || barState.Equals("Bar5"))
            {
                _bar = MicrosoftVirtualKeys.key5;
            }
            else if (barState.Equals("6") || barState.Equals("Bar6"))
            {
                _bar = MicrosoftVirtualKeys.key6;
            }
            else
            {
                _bar = MicrosoftVirtualKeys.Indifferent;
            }
            if (character.Equals("0") || character.Equals("10"))
                _wParam2 = MicrosoftVirtualKeys.key0;
            else if (character.Equals("1"))
                _wParam2 = MicrosoftVirtualKeys.key1;
            else if (character.Equals("2"))
                _wParam2 = MicrosoftVirtualKeys.key2;
            else if (character.Equals("3"))
                _wParam2 = MicrosoftVirtualKeys.key3;
            else if (character.Equals("4"))
                _wParam2 = MicrosoftVirtualKeys.key4;
            else if (character.Equals("5"))
                _wParam2 = MicrosoftVirtualKeys.key5;
            else if (character.Equals("6"))
                _wParam2 = MicrosoftVirtualKeys.key6;
            else if (character.Equals("7"))
                _wParam2 = MicrosoftVirtualKeys.key7;
            else if (character.Equals("8"))
                _wParam2 = MicrosoftVirtualKeys.key8;
            else if (character.Equals("9"))
                _wParam2 = MicrosoftVirtualKeys.key9;
            else if (character.Equals("-"))
                _wParam2 = MicrosoftVirtualKeys.VK_OEM_MINUS;
            else if (character.Equals("="))
                _wParam2 = MicrosoftVirtualKeys.VK_OEM_PLUS;
            else if (character != "")
            {
                try
                {
                    _wParam2 = (MicrosoftVirtualKeys)Enum.Parse(typeof(MicrosoftVirtualKeys), character, true);
                }
                catch (Exception)
                {
                    Logging.Write(LogType.Warning, "[KeyWrapper] Unsupported key: " + character + " : " + keyName);
                    //MessageBox.Show("Unsupported key: " + @char);
                }
            }
            if (!Enum.IsDefined(typeof (MicrosoftVirtualKeys), _wParam2))
            {
                Logging.Write(LogType.Warning, "[KeyWrapper] Unsupported key: " + _wParam2 + " : " + keyName);
            }
        }

        public string Key { get; private set; }

        public string Special { get; private set; }

        public string Name { get; private set; }

        public string Bar { get; private set; }

        private void ChangeBar()
        {
            if (_bar != MicrosoftVirtualKeys.Indifferent)
            {
                KeyLowHelper.PostMessage(Memory.WindowHandle, PressKeyCode, (IntPtr) MicrosoftVirtualKeys.VK_SHIFT, (IntPtr)0);
                KeyLowHelper.PostMessage(Memory.WindowHandle, PressKeyCode, (IntPtr) _bar, (IntPtr)0);
                KeyLowHelper.PostMessage(Memory.WindowHandle, ReleaseKeyCode, (IntPtr) MicrosoftVirtualKeys.VK_SHIFT, (IntPtr)0);
                KeyLowHelper.PostMessage(Memory.WindowHandle, ReleaseKeyCode, (IntPtr) _bar, (IntPtr)0);
                Thread.Sleep(350);
            }
        }

        public void SendKey()
        {
            Logging.Debug("SendKey: " + Name + " Bar: " +  Bar + " Key: " + Key);
            ChangeBar();
            if (!_shift)
            {
                KeyLowHelper.PostMessage(Memory.WindowHandle, PressKeyCode, (IntPtr) _wParam2, (IntPtr)0);
                KeyLowHelper.PostMessage(Memory.WindowHandle, ReleaseKeyCode, (IntPtr) _wParam2, (IntPtr)0);
            }
            if (_shift)
            {
                KeyLowHelper.PostMessage(Memory.WindowHandle, PressKeyCode, (IntPtr) _wParam, (IntPtr)0);
                KeyLowHelper.PostMessage(Memory.WindowHandle, PressKeyCode, (IntPtr) _wParam2, (IntPtr)0);
                KeyLowHelper.PostMessage(Memory.WindowHandle, ReleaseKeyCode, (IntPtr) _wParam, (IntPtr)0);
                KeyLowHelper.PostMessage(Memory.WindowHandle, ReleaseKeyCode, (IntPtr) _wParam2, (IntPtr)0);
            }
        }

        public void PressKey()
        {
            ChangeBar();
            if (!_shift)
            {
                KeyLowHelper.PostMessage(Memory.WindowHandle, PressKeyCode, (IntPtr) _wParam2, (IntPtr)0);
            }
            if (_shift)
            {
                KeyLowHelper.PostMessage(Memory.WindowHandle, PressKeyCode, (IntPtr) _wParam, (IntPtr)0);
                KeyLowHelper.PostMessage(Memory.WindowHandle, PressKeyCode, (IntPtr) _wParam2, (IntPtr)0);
            }
        }

        public void ReleaseKey()
        {
            ChangeBar();
            if (!_shift)
            {
                KeyLowHelper.PostMessage(Memory.WindowHandle, ReleaseKeyCode, (IntPtr) _wParam2, (IntPtr)0);
            }
            if (_shift)
            {
                KeyLowHelper.PostMessage(Memory.WindowHandle, ReleaseKeyCode, (IntPtr) _wParam, (IntPtr)0);
                KeyLowHelper.PostMessage(Memory.WindowHandle, ReleaseKeyCode, (IntPtr) _wParam2, (IntPtr)0);
            }
        }
    }
}