/*
This file is part of LazyBot.

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
    internal static class DoLoad
    {
        public static void Load()
        {
            var loadit = new Thread(LoadTheShit) {IsBackground = true, Name = "LoadTheShit"};
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
            CheckLicense();
        }

        private static void CheckLicense()
        {
            Logging.Write("Getting offsets, please wait.");
            //Lets wait for the license server to do its work
            while (License.GetResult.Equals(ServerResult.Unknown))
                Thread.Sleep(100);
            if (License.GetResult.Equals(ServerResult.Failed))
            {
                Logging.Write(LogType.Error, "Could not get offsets");
                Logging.Write(LogType.Error, License.Reason);
                LazyForms.MainForm.LicenseOk();
                return;
            }
            Logging.Write(LogType.Info, License.MessageFromServer);
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

        public static void ReCheckLicense()
        {
            if (License.GetResult != ServerResult.Sucess)
            {
                License.CheckLicense(LazySettings.UserName, LazySettings.Password);
                var loadit = new Thread(CheckLicense) {IsBackground = true, Name = "ReCheckLicense"};
                loadit.Start();
            }
        }
    }
}