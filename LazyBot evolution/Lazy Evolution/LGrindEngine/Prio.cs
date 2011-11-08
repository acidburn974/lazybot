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

namespace LazyEvo.LGrindEngine
{
    internal class Prio
    {
        internal static int Resurrect = 1001;
        internal static int Combat = 1000;
        internal static int Resting = 999;
        internal static int Vendor = 502;
        internal static int Train = 501;
        internal static int ToTown = 500;
        internal static int Mailbox = 250;
        internal static int LootAround = 50;
        internal static int Targetting = 40;
        internal static int Mount = 20;
        internal static int Moving = int.MinValue + 1;
    }
}