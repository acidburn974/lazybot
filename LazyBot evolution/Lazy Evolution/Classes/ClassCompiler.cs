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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LazyEvo.Forms.Helpers;
using LazyEvo.PVEBehavior;
using LazyLib;
using LazyLib.Combat;

namespace LazyEvo.Classes
{
    internal class CustomClass
    {
        public CustomClass(string assemblyName, string assemblyFunctionName)
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

    internal class ClassCompiler
    {
        internal static Dictionary<String, CombatEngine> Assemblys = new Dictionary<String, CombatEngine>();

        internal static Assembly CompileFile(string path)
        {
            var compilFile = new FileInfo(path);
            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
            var parameters = new CompilerParameters();
            AppDomain myDomain = AppDomain.CurrentDomain;
            Assembly[] assembliesLoaded = myDomain.GetAssemblies();
            foreach (Assembly myAssembly in assembliesLoaded)
            {
                parameters.ReferencedAssemblies.Add(myAssembly.Location);
            }
            CompilerResults results = codeProvider.CompileAssemblyFromFile(parameters, path);
            if (results.Errors.Count > 0)
            {
                Logging.Write("[Compiler] Error when compiling " + compilFile.Name + " :");
                foreach (CompilerError compErr in results.Errors)
                {
                    Logging.Write("Line number " + compErr.Line + ", Error Number: " + compErr.ErrorNumber + ", '" +
                                  compErr.ErrorText);
                }
                return null;
            }
            return results.CompiledAssembly;
        }

        internal static void RecompileAll()
        {
            Assemblys.Clear();
            var pveBehavior = new PVEBehaviorCombat();
            Assemblys.Add(pveBehavior.Name, pveBehavior);
            try
            {
                if (!Directory.Exists(LazyForms.OurDirectory + "\\Classes"))
                    return;
                var di = new DirectoryInfo(LazyForms.OurDirectory + "\\Classes");
                foreach (FileInfo fi in di.GetFiles())
                {
                    if (fi.Extension.ToLower() == ".cs")
                    {
                        Assembly assembly = CompileFile(fi.FullName);
                        if (assembly != null)
                        {
                            String fName = fi.Name.Replace(fi.Extension, String.Empty);
                            if (Assemblys.ContainsKey(fName))
                                Assemblys.Remove(fName);
                            foreach (Type t in assembly.GetTypes())
                            {
                                if (t.IsSubclassOf(typeof (CombatEngine)) && t.IsClass)
                                {
                                    object obj = Activator.CreateInstance(t);
                                    var combatEngine = (CombatEngine) obj;
                                    Assemblys.Add(fName, combatEngine);
                                    Logging.Write("[Compiler] Compiled: " + fName);
                                }
                            }
                        }
                    }
                    if (fi.Extension.ToLower() == ".dll")
                    {
                        Assembly assembly = Assembly.LoadFrom(fi.FullName);
                        if (assembly != null)
                        {
                            String fName = fi.Name.Replace(fi.Extension, String.Empty);
                            if (Assemblys.ContainsKey(fName))
                                Assemblys.Remove(fName);
                            foreach (Type t in assembly.GetTypes())
                            {
                                if (t.IsSubclassOf(typeof (CombatEngine)) && t.IsClass)
                                {
                                    object obj = Activator.CreateInstance(t);
                                    var combatEngine = (CombatEngine) obj;
                                    Assemblys.Add(fName, combatEngine);
                                    Logging.Write("[Compiler] Loaded: " + fName);
                                }
                            }
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