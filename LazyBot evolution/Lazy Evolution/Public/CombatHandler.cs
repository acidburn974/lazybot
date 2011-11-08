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
using System.Reflection;
using LazyEvo.Classes;
using LazyLib.Wow;

namespace LazyEvo.Public
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public enum CombatType
    {
        CombatStarted,
        CombatDone
    }

    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class GCombatEventArgs : EventArgs
    {
        public GCombatEventArgs(CombatType type)
        {
            CombatType = type;
            Unit = new PUnit(uint.MinValue);
        }

        public GCombatEventArgs(CombatType type, PUnit unit)
        {
            CombatType = type;
            Unit = unit;
        }

        public CombatType CombatType { get; private set; }
        public PUnit Unit { get; private set; }
    }

    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public static class CombatHandler
    {
        public static event EventHandler<GCombatEventArgs> CombatStatusChanged;

        public static void InvokeCombatStatusChanged(GCombatEventArgs e)
        {
            EventHandler<GCombatEventArgs> handler = CombatStatusChanged;
            if (handler != null) handler(null, e);
        }

        public static void Stop()
        {
            PrivCombatHandler.Stop();
        }

        public static void StartCombat(PUnit u)
        {
            PrivCombatHandler.StartCombat(u);
        }

        public static void OnRess()
        {
            PrivCombatHandler.OnRess();
        }

        public static void RunningAction()
        {
            PrivCombatHandler.RunningAction();
        }

        public static void Rest()
        {
            PrivCombatHandler.Rest();
        }
    }
}