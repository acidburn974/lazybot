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
using System.Net.Sockets;
using System.Threading;
using LazyEvo.Forms.Helpers;
using LazyEvo.Public;
using LazyLib;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.Plugins
{
    internal class Relogger
    {
        private const string Logout = "/logout";
        private const string Gluedialogbackground = "GlueDialogBackground";
        private const string Accountloginloginbutton = "AccountLoginLoginButton";
        private const string Accountloginaccountedit = "AccountLoginAccountEdit";
        private const string Accountloginpasswordedit = "AccountLoginPasswordEdit";
        private const string Tokenenterdialog = "TokenEnterDialog";

        private const string Wowaccountselectdialogbackgroundacceptbutton =
            "WoWAccountSelectDialogBackgroundAcceptButton";

        private const string Charselectenterworldbutton = "CharSelectEnterWorldButton";
        private const string WwwGoogleCom = "www.google.com";

        private const string WoWAccountSelectDialogBackgroundContainerButton =
            "WoWAccountSelectDialogBackgroundContainerButton{0}";

        private const string Wowaccountselectdialogbackground = "WoWAccountSelectDialogBackground";
        private static Thread _relogger;
        public static bool PeriodicLogoutActive;

        public static void CheckForDis()
        {
            if (!ObjectManager.InGame && LazyForms.MainForm.ShouldRelog && ReloggerSettings.ReloggingEnabled)
            {
                try
                {
                    Frame glue = InterfaceHelper.GetFrameByName(Gluedialogbackground);
                    if (glue != null)
                    {
                        if (glue.IsVisible)
                        {
                            if (_relogger == null || !_relogger.IsAlive)
                            {
                                Logging.Write("Oooh no disconnect");
                                _relogger = new Thread(DoReconnect) {IsBackground = true};
                                _relogger.Start();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.Write("Error relogging: " + e);
                }
            }
        }

        private static void DoReconnect()
        {
            if (Relog())
            {
                Logging.Write(LogType.Good, "Relogging worked :)");
            }
            else
            {
                Logging.Write("Could not relog :(");
            }
        }

        public static bool LogOutFor(int mins)
        {
            if (ObjectManager.InGame)
            {
                PeriodicLogoutActive = true;
                KeyHelper.ChatboxSendText(Logout);
            }

            var t = new Ticker(mins*60000);
            while (!t.IsReady) Thread.Sleep(10000);

            return Relog();
        }

        public static bool Relog()
        {
            try
            {
                PeriodicLogoutActive = false;
                Thread.Sleep(2500);
                while (!CheckConnection())
                {
                    Thread.Sleep(5000);
                }

                if (InterfaceHelper.GetFrameByName(Gluedialogbackground).IsVisible)
                {
                    KeyHelper.SendEnter();
                }
                Thread.Sleep(500);
                if (InterfaceHelper.GetFrameByName(Accountloginloginbutton).IsVisible)
                {
                    InterfaceHelper.GetFrameByName(Accountloginaccountedit).SetEditBoxText(ReloggerSettings.AccountName);
                    Thread.Sleep(3000);
                    InterfaceHelper.GetFrameByName(Accountloginpasswordedit).LeftClick();
                    Thread.Sleep(1500);
                    InterfaceHelper.GetFrameByName(Accountloginpasswordedit).SetEditBoxText(ReloggerSettings.AccountPw);
                    Thread.Sleep(1000);
                    InterfaceHelper.GetFrameByName(Accountloginloginbutton).LeftClick();
                }
                Thread.Sleep(5000);
                try
                {
                    if (InterfaceHelper.GetFrameByName(Wowaccountselectdialogbackground).IsVisible)
                    {
                        InterfaceHelper.GetFrameByName(string.Format(WoWAccountSelectDialogBackgroundContainerButton,
                                                                     ReloggerSettings.AccountAccount)).LeftClick();
                        Thread.Sleep(2000);
                        InterfaceHelper.GetFrameByName(Wowaccountselectdialogbackgroundacceptbutton).LeftClick();
                    }
                }
                catch
                {
                }
                if (InterfaceHelper.GetFrameByName(Tokenenterdialog).IsVisible)
                {
                    Logging.Write("Can't log in with authenticator attached");
                    return false;
                }
                while (!InterfaceHelper.GetFrameByName(Charselectenterworldbutton).IsVisible)
                    Thread.Sleep(3000);

                InterfaceHelper.GetFrameByName(Charselectenterworldbutton).LeftClick();
                Thread.Sleep(7000);
                while (!ObjectManager.InGame)
                {
                    Thread.Sleep(1000);
                }
                Logging.Write(LogType.Good, "Relogging worked :)");

                Latency.Sleep(5000);

                LazyForms.MainForm.StartBotting();

                Thread.Sleep(5000);
                var timeOut = new Ticker(8000);
                while (!timeOut.IsReady)
                {
                    if (ObjectManager.MyPlayer.IsMoving)
                        break;
                }
                if (timeOut.IsReady)
                {
                    Logging.Write(LogType.Warning, "We did not start moving, restarting");
                    LazyForms.MainForm.StopBotting(true);
                    Thread.Sleep(2000);
                    LazyForms.MainForm.StartBotting();
                }
                return true;
            }
            catch (Exception e)
            {
                Logging.Write("Error when relogging: " + e);
            }
            return false;
        }

        private static bool CheckConnection()
        {
            try
            {
                var clnt = new TcpClient(WwwGoogleCom, 80);
                clnt.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}