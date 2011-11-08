
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
    public class InterfaceHelper
    {
        private static Dictionary<String, Frame> _allFrames = new Dictionary<String, Frame>();
        private static Thread _updateThread;

        public static Int32 WindowWidth
        {
            get
            {
                Rectangle r;
                GetClientRect(Memory.WindowHandle, out r);
                return Math.Abs(r.Right - r.Left);
            }
        }

        public static Int32 WindowHeight
        {
            get
            {
                Rectangle r;
                GetClientRect(Memory.WindowHandle, out r);
                return Math.Abs(r.Bottom - r.Top);
            }
        }

        /// <summary>
        /// Gets all frames
        /// </summary>
        /// <value>All frames.</value>
        public static List<Frame> GetFrames
        {
            get { return _allFrames.Values.ToList(); }
        }

        /// <summary>
        /// Gets the frame the mouse is focusing ingame
        /// </summary>
        /// <value>The get mouse focus.</value>
        public static Frame GetMouseFocus
        {
            get
            {
                return
                    new Frame(
                        Memory.Read<uint>(Memory.ReadRelative<uint>((uint) Pointers.UiFrame.CurrentFramePtr) +
                                          (uint) Pointers.UiFrame.CurrentFrameOffset));
            }
        }

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        internal static void UpdateThread()
        {
            try
            {
                while (true)
                {
                    ReloadFrames();
                    Thread.Sleep(8000);
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception e)
            {
                //Logging.Write("Exception when updating interface: " + e);
            }
        }

        internal static void StartUpdate()
        {
            if(_updateThread != null)
            {
                _updateThread.Abort();
                _updateThread = null;
            }
            _updateThread = new Thread(UpdateThread) {IsBackground = true, Name = "InterfaceUpdater"};
            _updateThread.Start();
        }

        internal static void StopUpdate()
        {
            if (_updateThread == null || !_updateThread.IsAlive)
                return;
            _updateThread.Abort();
            _updateThread = null;
        }

        /// <summary>
        /// Updates the internal list with all frames
        /// </summary>
        public static void ReloadFrames()
        {
            var allFrames = new Dictionary<String, Frame>();
            var @base = Memory.ReadRelative<uint>((uint) Pointers.UiFrame.FrameBase);
            var currentFrame = Memory.Read<uint>(@base + (uint) Pointers.UiFrame.FirstFrame);
            while (currentFrame != 0)
            {
                var f = new Frame(currentFrame);
                if (!allFrames.ContainsKey(f.GetName))
                    allFrames.Add(f.GetName, f);
                currentFrame = Memory.Read<uint>(currentFrame + Memory.Read<uint>(@base + (uint) Pointers.UiFrame.NextFrame) + 4);
                Thread.Sleep(1);
            }
            _allFrames = allFrames;
        }

        /// <summary>
        /// Gets the name of the frame by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static Frame GetFrameByName(String name)
        {
            try
            {
                if (_allFrames[name] != null)
                    return _allFrames[name];
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}