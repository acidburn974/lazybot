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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LazyEvo.Forms.Helpers;
using LazyEvo.LGrindEngine.Helpers;
using LazyLib;
using LazyLib.Helpers;
using LazyLib.IPlugin;

namespace LazyEvo.Classes
{
    internal class CustomPlugin
    {
        public CustomPlugin(string assemblyName, string assemblyFunctionName)
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

    internal class PluginCompiler
    {
        internal static Dictionary<String, ILazyPlugin> Assemblys = new Dictionary<String, ILazyPlugin>();
        public static List<string> LoadedPlugins = new List<string>();

        internal static void RecompileAll()
        {
            foreach (var lazyPlugin in Assemblys)
            {
                lazyPlugin.Value.PluginUnload();
            }
            Assemblys.Clear();
            var converter = new Converter();
            Assemblys.Add("Converter", converter);

            //Loads lazy data demo
            //added by hertzigger
            LazyEvo.Plugins.LazyData.Loader lazyData = new LazyEvo.Plugins.LazyData.Loader();
            Assemblys.Add("Lazy Data", lazyData);
            try
            {
                if (!Directory.Exists(LazyForms.OurDirectory + "\\Plugins"))
                    return;
                var di = new DirectoryInfo(LazyForms.OurDirectory + "\\Plugins");
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
                                    if (t.GetInterface("ILazyPlugin") != null && t.IsClass)
                                    {
                                        object obj = Activator.CreateInstance(t);
                                        var combatEngine = (ILazyPlugin) obj;
                                        Assemblys.Add(fName, combatEngine);
                                        Logging.Write("[PCompiler] Loaded: " + fName);
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

        public static void StartSavedPlugins()
        {
            foreach (var keyValuePair in Assemblys)
            {
                if (LoadPluginSettings(keyValuePair.Key))
                {
                    if (!LoadedPlugins.Contains(keyValuePair.Value.GetName()))
                        PluginLoad(keyValuePair.Key);
                }
                else
                {
                    if (LoadedPlugins.Contains(keyValuePair.Key))
                        PluginUnload(keyValuePair.Key);
                }
            }
        }

        private static bool LoadPluginSettings(string name)
        {
            try
            {
                var pIniManager = new IniManager(LazyForms.OurDirectory + "\\Settings\\lazy_plugins.ini");
                return pIniManager.GetBoolean("Plugins", name, false);
            }
            catch
            {
            }
            return false;
        }

        public static void PluginLoad(string assemblyName)
        {
            Assemblys[assemblyName].PluginLoad();
            LoadedPlugins.Add(assemblyName);
        }

        public static void PluginUnload(string assemblyName)
        {
            Assemblys[assemblyName].PluginUnload();
            LoadedPlugins.Remove(assemblyName);
        }
    }
}