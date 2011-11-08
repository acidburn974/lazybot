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

#region

using System;
using System.Linq;
using System.Threading;
using LazyEvo.Classes;

#endregion

namespace LazyEvo.PluginSystem
{
    internal class PluginManager
    {
        private static Thread _pulseThread = new Thread(Pulse);
        private static bool _keepPulsing;

        private static void Pulse()
        {
            while (_keepPulsing)
            {
                foreach (var lazyPlugin in
                    PluginCompiler.Assemblys.Where(lazyPlugin => PluginCompiler.LoadedPlugins.Contains(lazyPlugin.Key)))
                {
                    lazyPlugin.Value.Pulse();
                }
                Thread.Sleep(200);
            }
        }

        public static void TerminatePulseThread()
        {
            try
            {
                _keepPulsing = false;
                if (_pulseThread == null)
                    return;
                _pulseThread.Abort();
                _pulseThread.Join();
                GC.Collect();
            }
            catch (ThreadStateException)
            {
            }
        }

        public static void StopPulseThread()
        {
            try
            {
                _keepPulsing = false;
                if (_pulseThread == null || !_pulseThread.IsAlive)
                    return;
                _pulseThread.Join();
                GC.Collect();
            }
            catch (ThreadStateException)
            {
            }
        }

        public static void StartPulseThread(bool restartIfRunning)
        {
            if (restartIfRunning)
                TerminatePulseThread();
            if (!_pulseThread.IsAlive)
            {
                _keepPulsing = true;
                _pulseThread = new Thread(Pulse);
                _pulseThread.IsBackground = true;
                _pulseThread.Start();
            }
        }

        public static void BotStart()
        {
            foreach (var lazyPlugin in
                PluginCompiler.Assemblys.Where(lazyPlugin => PluginCompiler.LoadedPlugins.Contains(lazyPlugin.Key)))
            {
                lazyPlugin.Value.BotStart();
            }
        }

        public static void BotStop()
        {
            foreach (var lazyPlugin in
                PluginCompiler.Assemblys.Where(lazyPlugin => PluginCompiler.LoadedPlugins.Contains(lazyPlugin.Key)))
            {
                lazyPlugin.Value.BotStop();
            }
        }
    }
}