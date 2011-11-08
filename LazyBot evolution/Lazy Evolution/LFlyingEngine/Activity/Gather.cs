using System;
using System.Linq;
using System.Threading;
using LazyEvo.LFlyingEngine.Helpers;
using LazyEvo.LFlyingEngine.States;
using LazyEvo.Public;
using LazyLib;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.Activity
{
    internal class Gather
    {
        private static readonly Ticker TimeOut = new Ticker(4500);
        private static readonly Ticker StopASec = new Ticker(1000*6);
        private static Ticker _reLure;
        private static readonly Ticker Face = new Ticker(150);
        private static readonly Ticker ToLongRun = new Ticker(10000*6);
        private static double _oldDis;
        private static double _distanceX;
        private static double _distanceY;

        private static bool ApprochNode(PGameObject harvest)
        {
            var harvestModified = new PGameObject(harvest.BaseAddress);
            int num = Convert.ToInt32(FlyingSettings.ApproachModifier);
            var modifiedPos = new Location(harvestModified.X, harvestModified.Y, (harvestModified.Z + num));
            if (!ObjectManager.MyPlayer.IsFlying)
            {
                modifiedPos = new Location(harvestModified.X, harvestModified.Y, harvestModified.Z);
            }
            var timeout = new Ticker(8000);
            double num2 = modifiedPos.DistanceToSelf;
            bool jumped = false;
            StopASec.Reset();
            while (modifiedPos.DistanceToSelf2D > 10.0)
            {
                FlyingEngine.Navigator.SetDestination(modifiedPos);
                if (PlayerToClose(20, harvest))
                {
                    ToldAboutNode.TellAbout("Player to close", harvest);
                    return false;
                }
                if (timeout.IsReady)
                {
                    if (FlyingSettings.AutoBlacklist)
                    {
                        Logging.Write("Blacklisting node for-ever");
                        FlyingBlackList.AddBadNode(harvest.Location);
                    }
                    FlyingEngine.Navigator.Stop();
                    ToldAboutNode.TellAbout("node blacklisted", harvest);
                    return false;
                }
                if (StopASec.IsReady)
                {
                    Logging.Write("Check spin");
                    FlyingEngine.Navigator.Stop();
                    MoveHelper.ReleaseKeys();
                    harvest.Location.Face();
                    StopASec.Reset();
                    FlyingEngine.Navigator.Start();
                }
                if (FlyingBlackList.IsBlacklisted(harvest))
                {
                    ToldAboutNode.TellAbout("node blacklisted", harvest);
                    return false;
                }
                if (modifiedPos.DistanceToSelf < num2)
                {
                    num2 = modifiedPos.DistanceToSelf;
                    timeout.Reset();
                }
                if (modifiedPos.DistanceToSelf > 2.0)
                {
                    FlyingEngine.Navigator.Start();
                }
                else
                {
                    FlyingEngine.Navigator.Stop();
                }
                if (Stuck.IsStuck)
                {
                    Unstuck.TryUnstuck(false);
                }
                if (!Mount.IsMounted())
                {
                    Logging.Write("We got dismounted, abort");
                    return false;
                }
                if (!jumped && modifiedPos.DistanceToSelf2D > 20 && !ObjectManager.MyPlayer.IsFlying &&
                    ObjectManager.MyPlayer.IsMounted && !ObjectManager.MyPlayer.InVashjir)
                {
                    Logging.Write("Running on the ground, lets jump");
                    FlyingEngine.Navigator.Stop();
                    MoveHelper.Jump(1000);
                    FlyingEngine.Navigator.Start();
                    jumped = true;
                }
                Thread.Sleep(100);
            }
            FlyingEngine.Navigator.Stop();
            return (harvestModified.Location.DistanceToSelf2D < 10.0);
        }

        public static bool GatherNode(PGameObject harvest)
        {
            if (FindNode.IsSchool(harvest))
            {
                return GatherFishNode(harvest);
            }
            if (ApprochNode(harvest))
            {
                Logging.Write("We approached the node");
                HitTheNode(harvest);
                if (!CheckMobs(harvest))
                {
                    return false;
                }
                if (FlyingBlackList.IsBlacklisted(harvest))
                {
                    ToldAboutNode.TellAbout("is blacklisted", harvest);
                    return false;
                }
                if (MoveHelper.NegativeValue(ObjectManager.MyPlayer.Location.Z - FindNode.GetLocation(harvest).Z) > 1)
                {
                    Logging.Write("Descending");
                    DescentToNode(harvest);
                }
                if (FlyingBlackList.IsBlacklisted(harvest))
                {
                    ToldAboutNode.TellAbout("is blacklisted", harvest);
                    return false;
                }
                if (FindNode.GetLocation(harvest).DistanceToSelf2D > 5)
                {
                    ApproachPosFlying.Approach(harvest.Location, 4);
                }
                if (FindNode.GetLocation(harvest).DistanceToSelf > 10)
                {
                    Logging.Write("Could not get to the node");
                    return false;
                }
                if (!DismountAndHarvest(harvest, TimeOut))
                {
                    return false;
                }
                return true;
            }
            ToldAboutNode.TellAbout("we never approached the node", harvest);
            return false;
        }

        private static bool CheckMobs(PGameObject harvest)
        {
            int mobs =
                ObjectManager.GetUnits.Where(
                    unit => unit.Location.DistanceFrom(harvest.Location) < 25 && unit.Reaction == Reaction.Hostile).
                    Count();
            if ((mobs > 0) && HasRessSickness())
            {
                ToldAboutNode.TellAbout("there are mobs close to the node and we have ress sickness", harvest);
                FlyingBlackList.Blacklist(harvest, 120, true);
                return false;
            }
            if (FlyingSettings.AvoidElites &&
                ObjectManager.GetUnits.Where(
                    unit =>
                    unit.Location.DistanceFrom(harvest.Location) < 30 && unit.IsElite &&
                    unit.Reaction == Reaction.Hostile).Count() != 0)
            {
                Logging.Write("Elite at node, not landing");
                FlyingBlackList.Blacklist(harvest, 120, true);
                return false;
            }
            if (Convert.ToInt32(FlyingSettings.MaxUnits) < mobs)
            {
                Logging.Write("To many units at node.");
                FlyingBlackList.Blacklist(harvest, 120, true);
                return false;
            }
            return true;
        }

        internal static bool GatherFishNode(PGameObject node)
        {
            if (_reLure == null)
            {
                _reLure = new Ticker(600000);
                _reLure.ForceReady();
            }
            FlyingEngine.Navigator.Stop();
            var combat = new StateCombat();
            int nearestIndexInPositionList =
                Location.GetClosestPositionInList(FlyingEngine.CurrentProfile.WaypointsNormal, node.Location);
            Location position = FlyingEngine.CurrentProfile.WaypointsNormal[nearestIndexInPositionList];
            if (!ApproachPosFlying.Approach(position, 5))
            {
                return false;
            }
            node.Location.Face();
            if (Bobber() != null)
            {
                Logging.Write("Someone is fishing, break");
                return false;
            }
            if (!CheckMobs(node))
            {
                return false;
            }
            if (FlyingBlackList.IsBlacklisted(node))
            {
                ToldAboutNode.TellAbout("is blacklisted", node);
                return false;
            }
            DescentToSchool(node);
            Mount.Dismount();
            var timeout = new Ticker((FlyingSettings.MaxTimeAtSchool*60)*1000);
            var checkIfValid = new Ticker(8000);
            while (node.IsValid)
            {
                while (combat.NeedToRun)
                {
                    combat.DoWork();
                    timeout.Reset();
                }
                if (checkIfValid.IsReady)
                {
                    if (ObjectManager.GetObjects.FirstOrDefault(u => u.BaseAddress == node.BaseAddress) == null)
                    {
                        break;
                    }
                    checkIfValid.Reset();
                }
                if (FlyingSettings.Lure && _reLure.IsReady)
                {
                    KeyHelper.SendKey("Lure");
                    Thread.Sleep(3500);
                    _reLure.Reset();
                }
                if (timeout.IsReady)
                {
                    return false;
                }
                if (ObjectManager.MyPlayer.IsSwimming)
                {
                    MoveHelper.Jump(1500);
                    Thread.Sleep(1000);
                    KeyHelper.SendKey("Waterwalk");
                    Thread.Sleep(2000);
                    MoveHelper.Jump(1500);
                    Thread.Sleep(1500);
                    if (ObjectManager.MyPlayer.IsSwimming)
                    {
                        return false;
                    }
                }
                node.Location.Face();
                var timeout3 = new Ticker(4000);
                while (!timeout3.IsReady && (node.Location.DistanceToSelf2D < 14))
                {
                    MoveHelper.Backwards(true);
                    Thread.Sleep(20);
                }
                MoveHelper.ReleaseKeys();
                timeout3.Reset();
                node.Location.Face();
                while (!timeout3.IsReady && (node.Location.DistanceToSelf2D > 16))
                {
                    MoveHelper.Forwards(true);
                    Thread.Sleep(20);
                }
                MoveHelper.ReleaseKeys();
                KeyHelper.SendKey("Fishing");
                Thread.Sleep(1500);
                Fishing.FindBobberAndClick(FlyingSettings.WaitForLoot);
                Thread.Sleep(100);
            }
            return true;
        }

        private static PGameObject Bobber()
        {
            return ObjectManager.GetGameObject.FirstOrDefault(pGameObject => pGameObject.DisplayId == 0x29c);
        }

        internal static bool PlayerToClose(int distance, PGameObject gameObject)
        {
            if (!FlyingSettings.AvoidPlayers)
                return false;
            if (
                ObjectManager.GetPlayers.Where(obj => !obj.Name.Equals(ObjectManager.MyPlayer.Name))
                    .Any(obj => FindNode.GetLocation(gameObject).GetDistanceTo(obj.Location) < distance))
            {
                Logging.Write("Player to close to node");
                FlyingBlackList.Blacklist(gameObject, 300, false);
                return true;
            }
            return false;
        }

        internal static bool CheckFight(PGameObject gameObject)
        {
            if (CheckDruidFight(gameObject))
                return true;
            if (ObjectManager.ShouldDefend && !ObjectManager.MyPlayer.IsInFlightForm)
                return true;
            return false;
        }


        internal static bool HasRessSickness()
        {
            return ObjectManager.MyPlayer.HasBuff(15007);
        }


        internal static bool CheckDruidFight(PGameObject gameObject)
        {
            if (ObjectManager.ShouldDefend && ObjectManager.MyPlayer.IsInFlightForm)
            {
                if (FlyingSettings.DruidAvoidCombat)
                {
                    Logging.Write("Druid - avoiding combat - blacklisting node");
                    FlyingBlackList.Blacklist(gameObject, 300, false);
                    return true;
                }
                Mount.Dismount();
                return false;
            }
            return false;
        }

        internal static void HitTheNode(PGameObject nodeToHarvest)
        {
            Face.Reset();
            ToLongRun.Reset();
            _oldDis = FindNode.GetLocation(nodeToHarvest).DistanceToSelf;
            while (!ToLongRun.IsReady)
            {
                MoveHelper.Forwards(true);
                _distanceX =
                    Math.Round(Math.Abs(FindNode.GetLocation(nodeToHarvest).Y - ObjectManager.MyPlayer.Location.Y));
                _distanceY =
                    Math.Round(Math.Abs(FindNode.GetLocation(nodeToHarvest).X - ObjectManager.MyPlayer.Location.X));
                if (_distanceX <= 3 && _distanceY <= 3)
                    break;
                if (Face.IsReady)
                {
                    FindNode.GetLocation(nodeToHarvest).Face();
                    Face.Reset();
                }
                Thread.Sleep(2);
                if (_oldDis < FindNode.GetLocation(nodeToHarvest).DistanceToSelf)
                    break;
            }
            if (nodeToHarvest.Location.DistanceToSelf2D > 2.9)
            {
                if (nodeToHarvest.Location.DistanceToSelf2D > 2)
                {
                    Thread.Sleep(150);
                }
                else if (nodeToHarvest.Location.DistanceToSelf2D > 1)
                {
                    Thread.Sleep(100);
                }
            }
            MoveHelper.ReleaseKeys();
        }

        /// <summary>
        ///   Descents to node.
        /// </summary>
        internal static void DescentToNode(PGameObject nodeToHarvest)
        {
            var ticker = new Ticker(6000);
            if (ObjectManager.MyPlayer.InVashjir)
            {
                MoveHelper.Down(true);
                DescentToNodeVashir(nodeToHarvest);
                MoveHelper.Down(false);
            }
            else
            {
                if (!ObjectManager.MyPlayer.IsFlying) //To avoid sitting on the ground
                {
                    return;
                }
                MoveHelper.Down(true);
                while (ObjectManager.MyPlayer.IsFlying && !ticker.IsReady)
                {
                    Thread.Sleep(10);
                }
                MoveHelper.Down(false);
            }
        }

        //Ugly ugly ugly but we have to support vashir
        internal static void DescentToNodeVashir(PGameObject nodeToHarvest)
        {
            var ticker = new Ticker(20*1000);
            var timerDiff = new Ticker(500);
            float diffSelf = 3;
            Location oldPos = ObjectManager.MyPlayer.Location;
            float diffZ = MoveHelper.NegativeValue(nodeToHarvest.Location.Z - ObjectManager.MyPlayer.Location.Z);
            while (diffZ > 2 && !ticker.IsReady && diffSelf > 0.3)
            {
                if (FlyingBlackList.IsBlacklisted(nodeToHarvest))
                    return;
                diffZ = MoveHelper.NegativeValue(ObjectManager.MyPlayer.Location.Z - nodeToHarvest.Location.Z);
                if (timerDiff.IsReady)
                {
                    diffSelf = MoveHelper.NegativeValue(oldPos.Z - ObjectManager.MyPlayer.Location.Z);
                    timerDiff.Reset();
                    oldPos = ObjectManager.MyPlayer.Location;
                }
                Thread.Sleep(10);
            }
        }

        //Ugly ugly ugly but we have to support vashir
        internal static void DescentToSchool(PGameObject nodeToHarvest)
        {
            var ticker = new Ticker(20*1000);
            var timerDiff = new Ticker(500);
            float diffSelf = 3;
            Location oldPos = ObjectManager.MyPlayer.Location;
            float diffZ = MoveHelper.NegativeValue(nodeToHarvest.Location.Z - ObjectManager.MyPlayer.Location.Z);
            while (ObjectManager.MyPlayer.IsFlying && diffZ > 2 && !ticker.IsReady && diffSelf > 0.3)
            {
                if (FlyingBlackList.IsBlacklisted(nodeToHarvest))
                    return;
                diffZ = MoveHelper.NegativeValue(ObjectManager.MyPlayer.Location.Z - nodeToHarvest.Location.Z);
                if (timerDiff.IsReady)
                {
                    diffSelf = MoveHelper.NegativeValue(oldPos.Z - ObjectManager.MyPlayer.Location.Z);
                    timerDiff.Reset();
                    oldPos = ObjectManager.MyPlayer.Location;
                }
                Thread.Sleep(10);
            }
        }

        internal static bool DismountAndHarvest(PGameObject harvest, Ticker timeOut)
        {
            if (!LazySettings.BackgroundMode && !harvest.Location.IsFacing())
            {
                harvest.Location.Face();
            }
            if (Mount.IsMounted() && !ObjectManager.MyPlayer.IsInFlightForm)
            {
                Mount.Dismount();
                timeOut.Reset();
                while (ObjectManager.MyPlayer.IsMoving && !timeOut.IsReady)
                    Thread.Sleep(100);
                Thread.Sleep(500);
            }
            Logging.Debug("Going to do harvest now");
            harvest.Interact(true);
            Latency.Sleep(ObjectManager.MyPlayer.UnitRace != "Tauren" ? 750 : 500);
            if (!ObjectManager.MyPlayer.IsCasting && ObjectManager.MyPlayer.UnitRace != "Tauren")
            {
                harvest.Interact(true);
                Latency.Sleep(750);
            }
            if (CheckFight(harvest))
            {
                ToldAboutNode.TellAbout("we are in combat", harvest);
                return false;
            }
            timeOut.Reset();
            while (ObjectManager.MyPlayer.IsCasting && !timeOut.IsReady)
            {
                if (CheckFight(harvest))
                {
                    ToldAboutNode.TellAbout("we are in combat", harvest);
                    return false;
                }
                Thread.Sleep(100);
            }
            if (CheckFight(harvest))
            {
                ToldAboutNode.TellAbout("we are in combat", harvest);
                return false;
            }
            if (Langs.SkillToLow(ObjectManager.MyPlayer.RedMessage))
            {
                Logging.Write("Skill to low");
                HelperFunctions.ResetRedMessage();
                if (FindNode.IsMine(harvest) || FindNode.IsHerb(harvest))
                {
                    SkillToLow.Blacklist(harvest.Name, 240);
                }
                return false;
            }
            return true;
        }
    }
}