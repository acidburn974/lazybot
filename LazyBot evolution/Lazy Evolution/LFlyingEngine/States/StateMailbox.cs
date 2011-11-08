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

using LazyEvo.LFlyingEngine.Activity;
using LazyLib;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.Helpers.Mail;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.States
{
    internal class StateMailbox : MainState
    {
        private const int SearchDistance = 30;
        private readonly Ticker _mailTimeout = new Ticker(300000);
        private readonly PGameObject _mailbox = new PGameObject(0);

        public StateMailbox()
        {
            _mailTimeout.ForceReady();
        }

        public override bool NeedToRun
        {
            get
            {
                if (ObjectManager.MyPlayer.IsDead || FlyingEngine.CurrentProfile.WaypointsToTown.Count == 0)
                {
                    return false;
                }
                if (!ToTown.ToTownEnabled)
                {
                    return false;
                }
                if (!LazySettings.ShouldMail || string.IsNullOrEmpty(LazySettings.MailTo))
                {
                    return false;
                }
                if (!ToTown.FollowingWaypoints)
                {
                    return false;
                }
                if (!_mailTimeout.IsReady)
                {
                    return false;
                }
                _mailbox.BaseAddress = 0;
                foreach (PGameObject obj in ObjectManager.GetGameObject)
                {
                    if (obj.GameObjectType == 19 && obj.Location.DistanceToSelf2D < SearchDistance)
                    {
                        _mailbox.BaseAddress = obj.BaseAddress;
                        break;
                    }
                }
                return (_mailbox.BaseAddress != 0);
            }
        }

        public override int Priority
        {
            get { return Prio.MailBox; }
        }

        public override void DoWork()
        {
            Logging.Write("Found a mailbox, lets do something");
            FlyingEngine.Navigator.Stop();
            if (ApproachPosFlying.Approach(_mailbox.Location, 12))
            {
                MoveHelper.MoveToLoc(_mailbox.Location, 5);
                MailManager.DoMail();
            }
            ToTown.ToTownDoMail = true;
            _mailTimeout.Reset();
        }

        public override string Name()
        {
            return "Mailbox";
        }
    }
}