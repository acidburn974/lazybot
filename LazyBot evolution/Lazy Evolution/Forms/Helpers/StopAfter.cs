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
using System.Threading;
using LazyLib;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.Forms.Helpers
{
    internal class StopAfter
    {
        private static Ticker _stopAfter;

        public static void BotStarted()
        {
            if (LazySettings.StopAfterBool)
            {
                _stopAfter = new Ticker((Convert.ToDouble(LazySettings.StopAfter)*60)*1000);
                _stopAfter.Reset();
                Logging.Write(LogType.Info,
                              "Stop after enabled, will stop in " + Convert.ToDouble(LazySettings.StopAfter) +
                              " minuttes");
            }
        }

        public static void BotStopped()
        {
            _stopAfter = null;
        }

        public static void Monitor()
        {
            if (_stopAfter != null)
            {
                if (LazySettings.StopAfterBool)
                {
                    if (_stopAfter.IsReady && !ObjectManager.ShouldDefend)
                    {
                        LazyForms.MainForm.StopBotting(true);
                        LazyForms.MainForm.ShouldRelog = false;
                        Thread.Sleep(3000);
                        Logging.Write(LogType.Info,
                                      "[Engine]Stop after " + Convert.ToDouble(LazySettings.StopAfter) +
                                      " minuttes done");
                        KeyHelper.ChatboxSendText("/logout");
                        if (LazySettings.Shutdown)
                        {
                            Shutdown.ShutDownComputer();
                        }
                    }
                }
            }
        }
    }
}