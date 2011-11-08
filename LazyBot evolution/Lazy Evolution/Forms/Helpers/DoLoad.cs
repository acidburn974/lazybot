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
using LazyEvo.Other;
using LazyLib;
using LazyLib.Helpers;
using LazyLib.Helpers.Vendor;
using LazyLib.Wow;

namespace LazyEvo.Forms.Helpers
{
    internal class DoLoad
    {
        public static void Load()
        {
            var loadit = new Thread(LoadTheShit) {IsBackground = true};
            loadit.Name = "LoadTheShit";
            loadit.Start();
        }

        public static void Close()
        {
            ObjectManager.Close();
            ItemDatabase.Close();
            Hook.ReleaseMouse();
            Logging.Debug("Done closing");
            Environment.Exit(0);
        }

        private static void LoadTheShit()
        {
            ObjectManager.MakeReady();
            Logging.Write("Visit www.mmo-lazybot.com for support.");
            Logging.Write("LazyBot is free software!");
            Logging.Write("Keys should be placed on bar 1-6 and position 1-9!");
            LoadNow();
        }

        private static void LoadNow()
        {
            if (Program.AttachTo != -1)
            {
                ObjectManager.Initialize(Program.AttachTo);
                try
                {
                    if (LazySettings.HookMouse)
                    {
                        Hook.DoHook();
                    }
                }
                catch
                {
                }
                Chat.NewChatMessage += LazyForms.MainForm.ChatNewChatMessage;
            }
            Langs.Load();
            ThreadManager.Start();
            ItemDatabase.Open();
            LazyForms.MainForm.LicenseOk();
        }
    }
}