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
using LazyEvo.LFlyingEngine.Activity;
using LazyEvo.LFlyingEngine.Helpers;
using LazyEvo.Public;
using LazyLib;
using LazyLib.FSM;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.States
{
    internal class StateGather : MainState
    {
        private static readonly Ticker TimeOut = new Ticker(2000);
        private static Location _nodeLoc = new Location(0, 0, 0);
        private static int _approachTimes;
        private readonly PGameObject _node = new PGameObject(0);

        public StateGather()
        {
            _node = new PGameObject(0);
        }

        public override bool NeedToRun
        {
            get
            {
                if (ToTown.ToTownEnabled)
                {
                    return false;
                }
                _node.BaseAddress = 0;
                PGameObject node = FindNode.SearchForNode();
                if (node != null)
                {
                    if (FlyingBlackList.IsBlacklisted(node) || SkillToLow.IsBlacklisted(node.Name))
                        return false;
                    if (node.Location.DistanceToSelf2D > 10 && !Mount.IsMounted())
                        return false;
                    _node.BaseAddress = node.BaseAddress;
                }
                return _node.BaseAddress != 0;
            }
        }

        public override int Priority
        {
            get { return Prio.Gathering; }
        }

        public override void DoWork()
        {
            Logging.Debug("Approaching: " + _node.Location);
            if (_nodeLoc == _node.Location)
            {
                if (_approachTimes > 6)
                {
                    Logging.Write("We tried to approach the same node more than 6 times... does not make sense abort");
                    FlyingBlackList.Blacklist(_node, 120, true);
                    return;
                }
                _approachTimes++;
            }
            else
            {
                _nodeLoc = _node.Location;
                _approachTimes = 0;
            }
            if (Gather.GatherNode(_node))
            {
                FlyingEngine.UpdateStats(1, 0, 0);
                if (FlyingSettings.WaitForLoot)
                {
                    while (ObjectManager.MyPlayer.LootWinOpen && !TimeOut.IsReady)
                        Thread.Sleep(100);
                    Latency.Sleep(1300);
                }
                if (ObjectManager.MyPlayer.IsInFlightForm)
                {
                    MoveHelper.Jump();
                }
            }
            else
            {
                if (ObjectManager.ShouldDefend && Mount.IsMounted())
                {
                    Logging.Write("ShouldDefend while mounted? Odd");
                    FlyingBlackList.Blacklist(_node, 120, true);
                }
            }
            if (!ObjectManager.MyPlayer.InVashjir && ObjectManager.MyPlayer.IsSwimming)
            {
                Logging.Write("We got into the water, blacklisting node");
                FlyingBlackList.AddBadNode(_node);
                KeyHelper.SendKey("Space");
                Thread.Sleep(3000);
                KeyHelper.ReleaseKey("Space");
            }
            if (!ObjectManager.ShouldDefend && !ObjectManager.MyPlayer.IsInCombat)
            {
                LootedBlacklist.Looted(_node);
                if (ObjectManager.MyPlayer.InVashjir)
                {
                    KeyHelper.SendKey("Space");
                    Thread.Sleep(1000);
                    KeyHelper.ReleaseKey("Space");
                }
            }
            if (!Mount.IsMounted())
            {
                CombatHandler.RunningAction();
                CombatHandler.Rest();
            }
            Stuck.Reset();
            if (FlyingEngine.CurrentProfile.NaturalRun)
            {
                FlyingEngine.Navigation.UseNearestWaypoint();
            }
            else
            {
                FlyingEngine.Navigation.UseNearestWaypoint(10);
            }
        }

        public override string Name()
        {
            return "Gathering";
        }
    }
}