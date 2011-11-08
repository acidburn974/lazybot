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

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LazyEvo
{
    internal class Hotkey : IMessageFilter
    {
        #region Interop

        private const uint WM_HOTKEY = 0x312;

        private const uint MOD_ALT = 0x1;
        private const uint MOD_CONTROL = 0x2;
        private const uint MOD_SHIFT = 0x4;
        private const uint MOD_WIN = 0x8;

        private const uint ERROR_HOTKEY_ALREADY_REGISTERED = 1409;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int UnregisterHotKey(IntPtr hWnd, int id);

        #endregion

        private const int maximumID = 0xBFFF;
        private static int currentID;

        private bool alt;
        private bool control;

        [XmlIgnore] private int id;
        private Keys keyCode;
        [XmlIgnore] private bool registered;
        private bool shift;
        [XmlIgnore] private Control windowControl;
        private bool windows;

        public Hotkey()
            : this(Keys.None, false, false, false, false)
        {
            // No work done here!
        }

        public Hotkey(Keys keyCode, bool shift, bool control, bool alt, bool windows)
        {
            // Assign properties
            KeyCode = keyCode;
            Shift = shift;
            Control = control;
            Alt = alt;
            Windows = windows;

            // Register us as a message filter
            Application.AddMessageFilter(this);
        }

        public bool Empty
        {
            get { return keyCode == Keys.None; }
        }

        public bool Registered
        {
            get { return registered; }
        }

        public Keys KeyCode
        {
            get { return keyCode; }
            set
            {
                // Save and reregister
                keyCode = value;
                Reregister();
            }
        }

        public bool Shift
        {
            get { return shift; }
            set
            {
                // Save and reregister
                shift = value;
                Reregister();
            }
        }

        public bool Control
        {
            get { return control; }
            set
            {
                // Save and reregister
                control = value;
                Reregister();
            }
        }

        public bool Alt
        {
            get { return alt; }
            set
            {
                // Save and reregister
                alt = value;
                Reregister();
            }
        }

        public bool Windows
        {
            get { return windows; }
            set
            {
                // Save and reregister
                windows = value;
                Reregister();
            }
        }

        #region IMessageFilter Members

        public bool PreFilterMessage(ref Message message)
        {
            // Only process WM_HOTKEY messages
            if (message.Msg != WM_HOTKEY)
            {
                return false;
            }

            // Check that the ID is our key and we are registerd
            if (registered && (message.WParam.ToInt32() == id))
            {
                // Fire the event and pass on the event if our handlers didn't handle it
                return OnPressed();
            }
            else
            {
                return false;
            }
        }

        #endregion

        public event HandledEventHandler Pressed;

        ~Hotkey()
        {
            // Unregister the hotkey if necessary
            if (Registered)
            {
                Unregister();
            }
        }

        public Hotkey Clone()
        {
            // Clone the whole object
            return new Hotkey(keyCode, shift, control, alt, windows);
        }

        public bool GetCanRegister(Control windowControl)
        {
            // Handle any exceptions: they mean "no, you can't register" :)
            try
            {
                // Attempt to register
                if (!Register(windowControl))
                {
                    return false;
                }

                // Unregister and say we managed it
                Unregister();
                return true;
            }
            catch (Win32Exception)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
        }

        public bool Register(Control windowControl)
        {
            // Check that we have not registered
            if (registered)
            {
                throw new NotSupportedException("You cannot register a hotkey that is already registered");
            }

            // We can't register an empty hotkey
            if (Empty)
            {
                throw new NotSupportedException("You cannot register an empty hotkey");
            }

            // Get an ID for the hotkey and increase current ID
            id = currentID;
            currentID = currentID + 1%maximumID;

            // Translate modifier keys into unmanaged version
            uint modifiers = (Alt ? MOD_ALT : 0) | (Control ? MOD_CONTROL : 0) |
                             (Shift ? MOD_SHIFT : 0) | (Windows ? MOD_WIN : 0);

            // Register the hotkey
            if (RegisterHotKey(windowControl.Handle, id, modifiers, keyCode) == 0)
            {
                // Is the error that the hotkey is registered?
                if (Marshal.GetLastWin32Error() == ERROR_HOTKEY_ALREADY_REGISTERED)
                {
                    return false;
                }
                else
                {
                    throw new Win32Exception();
                }
            }

            // Save the control reference and register state
            registered = true;
            this.windowControl = windowControl;

            // We successfully registered
            return true;
        }

        public void Unregister()
        {
            try
            {
                // Check that we have registered
                if (!registered)
                {
                    throw new NotSupportedException("You cannot unregister a hotkey that is not registered");
                }

                // It's possible that the control itself has died: in that case, no need to unregister!
                if (!windowControl.IsDisposed)
                {
                    // Clean up after ourselves
                    if (UnregisterHotKey(windowControl.Handle, id) == 0)
                    {
                        throw new Win32Exception();
                    }
                }

                // Clear the control reference and register state
                registered = false;
                windowControl = null;
            }
            catch
            {
            }
        }

        private void Reregister()
        {
            // Only do something if the key is already registered
            if (!registered)
            {
                return;
            }

            // Save control reference
            Control windowControl = this.windowControl;

            // Unregister and then reregister again
            Unregister();
            Register(windowControl);
        }

        private bool OnPressed()
        {
            // Fire the event if we can
            var handledEventArgs = new HandledEventArgs(false);
            if (Pressed != null)
            {
                Pressed(this, handledEventArgs);
            }

            // Return whether we handled the event or not
            return handledEventArgs.Handled;
        }

        public override string ToString()
        {
            // We can be empty
            if (Empty)
            {
                return "(none)";
            }

            // Build key name
            string keyName = Enum.GetName(typeof (Keys), keyCode);
            ;
            switch (keyCode)
            {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    // Strip the first character
                    keyName = keyName.Substring(1);
                    break;
                default:
                    // Leave everything alone
                    break;
            }

            // Build modifiers
            string modifiers = "";
            if (shift)
            {
                modifiers += "Shift+";
            }
            if (control)
            {
                modifiers += "Control+";
            }
            if (alt)
            {
                modifiers += "Alt+";
            }
            if (windows)
            {
                modifiers += "Windows+";
            }

            // Return result
            return modifiers + keyName;
        }
    }
}