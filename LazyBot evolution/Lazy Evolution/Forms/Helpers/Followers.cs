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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using LazyLib;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.Forms.Helpers
{
    internal class Followers
    {
        private const int Distance = 40;
        private static readonly Dictionary<UInt64, Int32> FollowersList = new Dictionary<UInt64, Int32>();
        private static readonly Ticker Timer = new Ticker(5000);

        private static readonly SoundPlayer SoundPlayer = new SoundPlayer();

        internal static void CheckFollow()
        {
            try
            {
                if (Engine.Running && Timer.IsReady)
                {
                    Timer.Reset();
                    foreach (
                        PPlayer player in
                            ObjectManager.GetPlayers.Where(
                                player =>
                                player.GUID != ObjectManager.MyPlayer.GUID && player.Name != ObjectManager.MyPlayer.Name)
                        )
                    {
                        if (player.GUID == ObjectManager.MyPlayer.GUID)
                            continue;

                        if (player.Name == ObjectManager.MyPlayer.Name)
                            continue;

                        if (player.Location.DistanceToSelf < Distance && !FollowersList.ContainsKey(player.GUID))
                        {
                            Logging.Write("New player around: " + player.Name);
                            FollowersList.Add(player.GUID, Environment.TickCount);
                        }
                        else if (player.Location.DistanceToSelf >= Distance && FollowersList.ContainsKey(player.GUID))
                        {
                            Logging.Write("Removed player: " + player.Name);
                            FollowersList.Remove(player.GUID);
                        }
                        else if (player.Location.DistanceToSelf < Distance && FollowersList.ContainsKey(player.GUID) &&
                                 FollowersList[player.GUID] +
                                 (Convert.ToInt32(LazySettings.LogOutOnFollowTime)*60*1000) < Environment.TickCount)
                        {
                            FollowersList[player.GUID] = Environment.TickCount;

                            Logging.Write(LogType.Warning,
                                          string.Format(player.Name + " has been following me for {0} minutes !",
                                                        LazySettings.LogOutOnFollowTime));
                            if (LazySettings.SoundFollow)
                                PlayerSound();
                            if (LazySettings.LogoutOnFollow)
                            {
                                LazyForms.MainForm.StopBotting(true);
                                Thread.Sleep(3000);
                                KeyHelper.ChatboxSendText("/logout");
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private static void PlayerSound()
        {
            if (File.Exists(LazySettings.OurDirectory + @"\palert.wav"))
            {
                SoundPlayer.SoundLocation = LazySettings.OurDirectory + @"\palert.wav";
                SoundPlayer.Play();
            }
        }
    }
}