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
using LazyEvo.Forms;
using LazyEvo.Forms.Helpers;
using LazyEvo.LFlyingEngine;
using LazyEvo.LGrindEngine;
using LazyLib;

namespace LazyEvo.Public
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class LazyHelpers
    {
        public static void StartBotting()
        {
            LazyForms.MainForm.StartBotting();
        }

        public static void StopAll(string reason)
        {
            LazyForms.MainForm.StopBotting(reason, false);
        }

        public static void StopAll(string reason, bool userStoppedIt)
        {
            LazyForms.MainForm.StopBotting(reason, userStoppedIt);
        }

        public static bool LoadGrindingProfile(string path)
        {
            if (Main.EngineHandler is GrindingEngine)
            {
                var grindingEngine = (GrindingEngine) Main.EngineHandler;
                return grindingEngine.LoadProfile(path);
            }
            return false;
        }

        public static bool LoadFlyingProfile(string path)
        {
            if (Main.EngineHandler is FlyingEngine)
            {
                var flyingEngine = (FlyingEngine) Main.EngineHandler;
                return flyingEngine.LoadProfile(path);
            }
            return false;
        }

        public static void Write(string format, params object[] args)
        {
            Logging.Write(format, args);
        }
    }
}