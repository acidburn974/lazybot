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

using LazyEvo.LGrindEngine.NpcClasses;
using LazyLib;

namespace LazyEvo.LGrindEngine.Activity
{
    internal class ToTown
    {
        internal static bool ToTownEnabled;
        internal static bool ToTownDoMail;
        internal static bool ToTownDoRepair;
        internal static bool ToTownDoVendor;
        internal static VendorsEx Vendor;

        internal static void Pulse()
        {
            if (Vendor == null)
            {
                GrindingEngine.Navigator.Stop();
                Vendor = GrindingEngine.CurrentProfile.NpcController.GetNearestRepair();
                GrindingEngine.Navigation.SetNewSpot(Vendor.Location);
                Logging.Write("Going to vendor at: " + Vendor.Name);
            }
            if (GrindingEngine.Navigation.SpotToHit != Vendor.Location)
            {
                GrindingEngine.Navigation.SetNewSpot(Vendor.Location);
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
            }
            else
            {
                ToTownEnabled = false;
                Vendor = null;
            }
        }
    }
}