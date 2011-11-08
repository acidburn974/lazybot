using System.Linq;
using LazyEvo.LFlyingEngine.Helpers;
using LazyLib;
using LazyLib.Wow;

namespace LazyEvo.LFlyingEngine.Activity
{
    internal class ToTown
    {
        internal static bool ToTownEnabled;
        internal static bool ToTownDoMail;
        internal static bool ToTownDoRepair;
        internal static bool ToTownDoVendor;
        private static int _toTownNearestWaypointIndex = -1;
        private static bool _reversed;
        internal static bool FollowingWaypoints;

        internal static void Pulse()
        {
            if (_toTownNearestWaypointIndex == -1)
            {
                _toTownNearestWaypointIndex =
                    Location.GetClosestPositionInList(FlyingEngine.CurrentProfile.WaypointsNormal,
                                                      FlyingEngine.CurrentProfile.WaypointsToTown[0]);
            }
            if (((_toTownNearestWaypointIndex != -1) &&
                 (FlyingEngine.Navigation.CurrentFlyingWaypointsType == FlyingWaypointsType.Normal)) &&
                (FlyingEngine.Navigation.CurrentWaypointIndex == _toTownNearestWaypointIndex))
            {
                FlyingEngine.Navigator.Stop();
                FlyingEngine.Navigation.Reset();
                FlyingEngine.Navigation = new FlyingNavigation(FlyingEngine.CurrentProfile.WaypointsToTown, false,
                                                               FlyingWaypointsType.ToTown);
                Logging.Write("Following ToTown waypoints");
                FollowingWaypoints = true;
            }
            if ((FlyingEngine.Navigation.CurrentFlyingWaypointsType == FlyingWaypointsType.ToTown) &&
                FlyingEngine.Navigation.IsLastWaypoints && FlyingEngine.Navigation.NextWaypointDistance <= 12 &&
                !_reversed)
            {
                _reversed = true;
                FlyingEngine.Navigator.Stop();
                FlyingEngine.Navigation.Reset();
                IOrderedEnumerable<Location> locations = from p in FlyingEngine.CurrentProfile.WaypointsToTown
                                                         orderby p.DistanceToSelf
                                                         select p;
                FlyingEngine.Navigation = new FlyingNavigation(locations.ToList(), false, FlyingWaypointsType.ToTown);
                Logging.Write("Following ToTown waypoints back");
            }
            if ((FlyingEngine.Navigation.CurrentFlyingWaypointsType == FlyingWaypointsType.ToTown) &&
                FlyingEngine.Navigation.IsLastWaypoints && FlyingEngine.Navigation.NextWaypointDistance <= 12 &&
                _reversed)
            {
                FollowingWaypoints = false;
                Logging.Write("ToTown done, following normal waypoints");
                FlyingEngine.Navigation.Reset();
                FlyingEngine.Navigation = new FlyingNavigation(FlyingEngine.CurrentProfile.WaypointsNormal, true,
                                                               FlyingWaypointsType.Normal);
                FlyingEngine.Navigation.UseNearestWaypoint(-1);
                SetToTown(false);
            }
        }

        internal static void SetToTown(bool enable)
        {
            if (enable)
            {
                ToTownEnabled = true;
                ToTownDoRepair = true;
                ToTownDoVendor = true;
                ToTownDoMail = LazySettings.ShouldMail;
                _toTownNearestWaypointIndex = -1;
                _reversed = false;
            }
            else
            {
                ToTownEnabled = false;
                _toTownNearestWaypointIndex = -1;
                _reversed = false;
            }
        }
    }
}