
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
#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using LazyLib.Helpers;
using LazyLib.IEngine;
using LazyLib.Wow;

#endregion

namespace LazyLib.FSM
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class Engine
    {
        public static bool Paused;
        public static MainState LastState;
        private static Thread _workerThread;
        public static List<MainState> States { get; private set; }
        public static bool Running { get; private set; }
        public static event EventHandler<NotifyStateChanged> StateChange;
        private static ILazyEngine _engine;
        public static void StartEngine(ILazyEngine engine)
        {
            _engine = engine;
            Clear();
            foreach (MainState mainState in _engine.States)
            {
                AddState(mainState);
            }
            lock (States)
            {
                States.Add(new StateIdle());
                LastState = States[States.Count() - 1]; //Set the last state to StateIdle
            }
            States.Sort();
            Logging.Write("[Engine]Initializing");
            Paused = false;
            Running = true;
            // Leave it as a background thread.
            _workerThread = new Thread(Run) {IsBackground = true};
            _workerThread.Name = "Engine";
            _workerThread.SetApartmentState(ApartmentState.STA);
            _workerThread.Start();
            Logging.Write("[Engine]Started bot thread");          
        }

        public static void AddState(MainState state)
        {
            if (States == null)
                States = new List<MainState>();
            if (!States.Contains(state))
                States.Add(state);
        }

        public static void Clear()
        {
            if (States != null)
                States.Clear();
        }

        private static void Run()
        {
            try
            {
                while (Running)
                {
                    try
                    {
                        if (Paused)
                        {
                            _engine.Pause();
                            while (Paused)
                            {
                                Thread.Sleep(100);
                            }
                            _engine.Resume();
                        }
                        while (ChatQueu.QueueCount != 0)
                        {
                            _engine.Pause();
                            KeyHelper.ChatboxSendText(ChatQueu.GetItem);
                            Thread.Sleep(100);
                            _engine.Resume();
                        }                     
                        foreach (MainState state in States.Where(state => state.NeedToRun))
                        {
                            if (LastState != state)
                            {
                                if (StateChange != null)
                                {
                                    StateChange(new object(), new NotifyStateChanged(state.Name()));
                                    Logging.Debug("State changed: " + state.Name());
                                }
                            }
                            state.DoWork();
                            LastState = state;
                            break;
                        }
                    }
                    catch (ThreadAbortException)
                    {
                    }
                    catch (ThreadStateException h)
                    {
                        Logging.Write("Thread in odd state, restarting: " + h);
                    }
                    catch (Exception e)
                    {
                        if (ObjectManager.InGame)
                        {
                            Logging.Write("[Engine] Exception " + e);
                        }
                        Thread.Sleep(4000);
                    }
                    Thread.Sleep(1);
                    Application.DoEvents();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception e)
            {
                Logging.Write("[Engine]Botting error: " + e);
            }
            finally
            {
                Running = false;
            }
        }

        public static void Pause()
        {
            if (!Running)
                return;
            Paused = !Paused;
            Logging.Write(Paused ? "Paused bot" : "Resumed bot");
        }


        public static void StopEngine()
        {
            if (!Running)
            {
                return;
            }
            Running = false;
            if (_workerThread.IsAlive)
            {
                _workerThread.Abort();
            }
            // Clear out the thread object and let the garbage collector get it
            _workerThread = null;
            States = null;
            LastState = null;
            _engine = null;
            GC.Collect();
        }

        #region Nested type: NotifyStateChanged

        [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
        public class NotifyStateChanged : EventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="NotifyStateChanged"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            public NotifyStateChanged(string name)
            {
                Name = name;
            }

            /// <summary>
            /// Gets the message.
            /// </summary>
            /// <value>The message.</value>
            public string Name { get; set; }
        }



        #endregion
    }
}