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
using LazyEvo.LGrindEngine.Helpers;
using LazyEvo.Public;
using LazyLib;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine.Activity
{
    internal class LootAndSkin
    {
        private static readonly Ticker TimeOut = new Ticker(2000);

        public static void DoWork(PUnit unit)
        {
            MoveHelper.ReleaseKeys();
            if (!unit.IsLootable)
                return;
            Logging.Write("Looting: " + unit.Name);
            if (unit.DistanceToSelf > 5)
            {
                MoveHelper.MoveToLoc(unit.Location, 4, false, true);
            }
            if (ObjectManager.ShouldDefend)
            {
                Logging.Write("Skipping loot, we got into combat");
                return;
            }
            Thread.Sleep(200);
            if (ObjectManager.MyPlayer.HasLivePet)
                Thread.Sleep(700);
            if (ObjectManager.MyPlayer.Target != unit)
            {
                unit.Interact(false);
            }
            else
            {
                KeyHelper.SendKey("InteractTarget");
            }
            if (ObjectManager.ShouldDefend)
                return;
            TimeOut.Reset();
            while (!ObjectManager.MyPlayer.LootWinOpen && !TimeOut.IsReady)
                Thread.Sleep(100);
            TimeOut.Reset();
            if (GrindingSettings.Skin || GrindingSettings.WaitForLoot)
            {
                while (ObjectManager.MyPlayer.LootWinOpen && !TimeOut.IsReady)
                    Thread.Sleep(100);
                Thread.Sleep(1300);
            }
            else
            {
                Thread.Sleep(200);
            }
            GrindingEngine.UpdateStats(1, 0, 0);
            PBlackList.Blacklist(unit, 300, false);
            if (unit.IsSkinnable && GrindingSettings.Skin)
            {
                Logging.Write("Skinning");
                KeyHelper.SendKey("TargetLastTarget");
                Thread.Sleep(1000);
                if (!ObjectManager.MyPlayer.Target.IsValid)
                {
                    unit.Interact(false);
                }
                else
                {
                    KeyHelper.SendKey("InteractTarget");
                }
                TimeOut.Reset();
                while (!ObjectManager.MyPlayer.LootWinOpen && !TimeOut.IsReady)
                {
                    Thread.Sleep(100);
                }
                if (GrindingSettings.WaitForLoot)
                {
                    Thread.Sleep(500);
                }
                GrindingEngine.UpdateStats(1, 0, 0);
            }
        }

        public static void DoLootAfterCombat(PUnit unit)
        {
            if (!GrindingSettings.Loot)
                return;
            if (ObjectManager.ShouldDefend)
                return;
            Thread.Sleep(500);
            if (!ObjectManager.MyPlayer.Target.IsValid && unit.IsLootable)
            {
                KeyHelper.SendKey("TargetLastTarget");
            }
            Thread.Sleep(500);
            if (ObjectManager.MyPlayer.Target.IsValid)
            {
                DoWork(ObjectManager.MyPlayer.Target);
            }
        }
    }
}