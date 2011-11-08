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

using System.Drawing;
using LazyLib;
using LazyLib.Helpers;
using curmoho;

namespace LazyEvo.Other
{
    internal class Hook
    {
        private static EntryPoint _entryPoint;

        internal static void DoHook()
        {
            if (LazySettings.HookMouse)
            {
                MouseHelper.FunctionToCall curpos = CursorPos;
                MouseHelper.SetDelegate(curpos);
                try
                {
                    MouseHelper.MouseBlockMessage -= BlockMessage;
                    MouseHelper.MouseMoveMessage -= MouseMove;
                    if (_entryPoint != null)
                    {
                        ReleaseMouse();
                    }
                    _entryPoint = EntryPoint.Instance;
                    _entryPoint.Install(Memory.ProcessId, Memory.ProcessHandle);
                    Logging.Write("Background enabled: " + _entryPoint.IsInstalled);
                    MouseHelper.MouseBlockMessage += BlockMessage;
                    MouseHelper.MouseMoveMessage += MouseMove;
                }
                catch
                {
                    Logging.Write("Could not enable background cursor");
                }
            }
        }

        private static void MouseMove(object sender, MouseMoveMessasge e)
        {
            if (LazySettings.HookMouse && _entryPoint != null)
            {
                MoveMouse(e.X, e.Y);
            }
        }

        private static Point CursorPos()
        {
            try
            {
                if (LazySettings.HookMouse && _entryPoint != null)
                {
                    return _entryPoint.GetMouseCursorPos();
                }
            }
            catch
            {
            }
            return new Point(0, 0);
        }

        private static void BlockMessage(object sender, MouseBlocMessasge e)
        {
            if (LazySettings.HookMouse && _entryPoint != null)
            {
                if (e.Block)
                {
                    Block();
                }
                else
                {
                    ReleaseMouse();
                }
            }
        }

        internal static void Block()
        {
            if (LazySettings.HookMouse && _entryPoint != null)
            {
                try
                {
                    if (_entryPoint != null)
                    {
                        if (!_entryPoint.IsInstalled)
                        {
                            try
                            {
                                _entryPoint.Install(Memory.ProcessId, Memory.ProcessHandle);
                            }
                            catch
                            {
                                Logging.Write("Could not enable background curser: Block");
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        internal static void MoveMouse(int x, int y)
        {
            if (LazySettings.HookMouse && _entryPoint != null)
            {
                try
                {
                    if (_entryPoint != null)
                    {
                        _entryPoint.SetCursorPos(x, y);
                    }
                }
                catch
                {
                }
            }
        }

        internal static void ReleaseMouse()
        {
            if (LazySettings.HookMouse && _entryPoint != null)
            {
                try
                {
                    if (_entryPoint != null)
                    {
                        _entryPoint.UnlockCursor();
                    }
                }
                catch
                {
                }
            }
        }

        internal static void Close()
        {
            if (LazySettings.HookMouse && _entryPoint != null)
            {
                try
                {
                    if (_entryPoint != null)
                    {
                        _entryPoint.Dispose();
                    }
                }
                catch
                {
                }
            }
        }
    }
}