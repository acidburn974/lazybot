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
using LazyEvo.Forms.Helpers;
using LazyLib;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.Plugins
{
    internal class PeriodicRelogger
    {
        private static Ticker _logOutAfter;

        public static void BotStarted()
        {
            if (ReloggerSettings.PeriodicReloggingEnabled && ReloggerSettings.ReloggingEnabled)
            {
                _logOutAfter = new Ticker((Convert.ToDouble(ReloggerSettings.PeriodicLogOut)*60)*1000);
                _logOutAfter.Reset();
                Logging.Write(LogType.Info,
                              "Periodic relog enabled. Next logout in: " +
                              Convert.ToDouble(ReloggerSettings.PeriodicLogOut) + " minutes");
            }
        }

        public static void BotStopped()
        {
            _logOutAfter = null;
        }

        public static void Monitor()
        {
            if (_logOutAfter != null)
            {
                if (ReloggerSettings.PeriodicReloggingEnabled)
                {
                    if (_logOutAfter.IsReady && !ObjectManager.ShouldDefend)
                    {
                        LazyForms.MainForm.StopBotting(true);
                        Thread.Sleep(3000);
                        Logging.Write(LogType.Info,
                                      "[Engine] Periodic logout as " + Convert.ToDouble(ReloggerSettings.PeriodicLogOut) +
                                      " minutes have passed");
                        Relogger.LogOutFor(ReloggerSettings.PeriodicLogIn);
                    }
                }
            }
        }
    }
}