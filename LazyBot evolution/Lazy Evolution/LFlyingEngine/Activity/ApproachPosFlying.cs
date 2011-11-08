using System.Threading;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.Activity
{
    internal class ApproachPosFlying
    {
        public static bool Approach(Location location, int distance)
        {
            var timeout = new Ticker(6000);
            double num2 = location.DistanceToSelf;
            MoveHelper.StopMove();
            location.Face();
            FlyingEngine.Navigator.Start();
            FlyingEngine.Navigator.SetDestination(location);
            while (location.DistanceToSelf2D > distance)
            {
                if (timeout.IsReady)
                {
                    FlyingEngine.Navigator.Stop();
                    return false;
                }
                if (location.DistanceToSelf < num2)
                {
                    num2 = location.DistanceToSelf;
                    timeout.Reset();
                }
                if (location.DistanceToSelf < distance)
                {
                    FlyingEngine.Navigator.Stop();
                }
                Thread.Sleep(10);
            }
            FlyingEngine.Navigator.Stop();
            Descent();
            return true;
        }

        private static void Descent()
        {
            var ticker = new Ticker(6000);
            MoveHelper.Down(true);
            if (ObjectManager.MyPlayer.InVashjir)
            {
                //ugly vashjir method
                var timerDiff = new Ticker(1000);
                float diffSelf = 3;
                Location oldPos = ObjectManager.MyPlayer.Location;
                while (!ticker.IsReady && diffSelf > 0.3)
                {
                    if (timerDiff.IsReady)
                    {
                        diffSelf = MoveHelper.NegativeValue(oldPos.Z - ObjectManager.MyPlayer.Location.Z);
                        timerDiff.Reset();
                        oldPos = ObjectManager.MyPlayer.Location;
                    }
                    Thread.Sleep(10);
                }
            }
            else
            {
                while (ObjectManager.MyPlayer.IsFlying && !ticker.IsReady)
                {
                    Thread.Sleep(10);
                }
            }
            MoveHelper.Down(false);
        }
    }
}