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

using System.Reflection;
using LazyEvo.Forms.Helpers;

namespace LazyEvo.Public
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class LazyForm
    {
        public static int Deaths;
        public static int Loots;
        public static int Kills;
        public static string TimeToLevel;
        public static string LPH;
        public static int Harvests;
        public static string Engine;

        public static void UpdateStatsText(string text)
        {
            LazyForms.MainForm.UpdateStatsText(text);
        }
    }
}