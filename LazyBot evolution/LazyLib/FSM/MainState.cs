
﻿/*
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
#region

using System;
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace LazyLib.FSM
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public abstract class MainState : IComparable<MainState>, IComparer<MainState>
    {
        #region Default methods

        public abstract int Priority { get; }

        public abstract bool NeedToRun { get; }

        #region IComparable<MainState> Members

        public int CompareTo(MainState other)
        {
            return -Priority.CompareTo(other.Priority);
        }

        #endregion

        #region IComparer<MainState> Members

        public int Compare(MainState x, MainState y)
        {
            return -x.Priority.CompareTo(y.Priority);
        }

        #endregion

        public abstract void DoWork();

        public abstract string Name();

        #endregion

        #region Log

        public static void Log(string message)
        {
            Logging.Write(message);
        }

        public static void Log(string message, LogType type)
        {
            Logging.Write(type, message);
        }

        public static void Debug(string message)
        {
            Logging.Write(message);
        }

        #endregion
    }
}