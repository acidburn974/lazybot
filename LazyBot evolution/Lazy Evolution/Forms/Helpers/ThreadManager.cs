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

using System.Threading;
using LazyEvo.Plugins;
using LazyLib.Helpers;

namespace LazyEvo.Forms.Helpers
{
    internal class ThreadManager
    {
        private static Chat _chat;
        private static Thread _monitor;

        public static void Start()
        {
            _chat = new Chat();
            _chat.PrepareReading();
            _monitor = new Thread(DoWork) {IsBackground = true};
            _monitor.Name = "Monitor";
            _monitor.Start();
        }

        private static void DoWork()
        {
            while (true)
            {
                try
                {
                    if (!Relogger.PeriodicLogoutActive)
                        Relogger.CheckForDis();
                    RefreshGui.Refresh();
                    _chat.ReadChat();
                    Followers.CheckFollow();
                    PeriodicRelogger.Monitor();
                    StopAfter.Monitor();
                    Thread.Sleep(1500);
                }
                catch
                {
                    //Empty
                }
            }
        }
    }
}