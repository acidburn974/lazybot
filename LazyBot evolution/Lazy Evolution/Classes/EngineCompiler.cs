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

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LazyEvo.Forms.Helpers;
using LazyEvo.LFlyingEngine;
using LazyEvo.LGrindEngine;
using LazyLib;
using LazyLib.IEngine;

#endregion

namespace LazyEvo.Classes
{
    internal class CustomEngine
    {
        public CustomEngine(string assemblyName, string assemblyFunctionName)
        {
            AssemblyName = assemblyName;
            AssemblyFunctionName = assemblyFunctionName;
        }

        public string AssemblyName { get; private set; }
        public string AssemblyFunctionName { get; private set; }

        public override string ToString()
        {
            return AssemblyFunctionName;
        }
    }

    internal class EngineCompiler
    {
        internal static Dictionary<String, ILazyEngine> Assemblys = new Dictionary<String, ILazyEngine>();

        internal static void RecompileAll()
        {
            Assemblys.Clear();
            var grindingEngine = new GrindingEngine();
            Assemblys.Add(grindingEngine.Name, grindingEngine);
            var flyingEngine = new FlyingEngine();
            Assemblys.Add(flyingEngine.Name, flyingEngine);
            try
            {
                if (!Directory.Exists(LazyForms.OurDirectory + "\\Engines"))
                    return;
                var di = new DirectoryInfo(LazyForms.OurDirectory + "\\Engines");
                foreach (FileInfo fi in di.GetFiles())
                {
                    if (fi.Extension.ToLower() == ".dll")
                    {
                        try
                        {
                            Assembly assembly = Assembly.LoadFrom(fi.FullName);
                            if (assembly != null)
                            {
                                String fName = fi.Name.Replace(fi.Extension, String.Empty);
                                if (Assemblys.ContainsKey(fName))
                                    Assemblys.Remove(fName);
                                foreach (Type t in assembly.GetTypes())
                                {
                                    if (t.GetInterface("ILazyEngine") != null && t.IsClass)
                                    {
                                        object obj = Activator.CreateInstance(t);
                                        var combatEngine = (ILazyEngine) obj;
                                        Assemblys.Add(fName, combatEngine);
                                        Logging.Write("[ECompiler] Loaded: " + fName);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.Write("[Compiler] Exception: " + ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Write("[Compiler] Exception: " + ex);
            }
        }
    }
}