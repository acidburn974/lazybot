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
using System.Text;
using System.Threading;
using CSScriptLibrary;
using LazyLib;

namespace LazyEvo.Other
{
    internal class ScriptRunner
    {
        public static void RunCode(string name, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return;
            }
            try
            {
                code = GetCode(code);
                var script = new AsmHelper(CSScript.LoadMethod(code));
                script.Invoke("*.Run");
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception e)
            {
                Logging.Write(LogType.Error, "Error running script: {0}:{1} ", name, e.Message);
            }
        }

        public static bool ShouldRun(string name, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            try
            {
                code = GetCode(code);
                var script = new AsmHelper(CSScript.LoadMethod(code));
                return (bool) script.Invoke("*.ShouldRun");
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception e)
            {
                Logging.Write(LogType.Error, "Error running script (ShouldRun): {0}:{1} ", name, e.Message);
            }
            return false;
        }

        private static string GetCode(string code)
        {
            if (code.ToUpper().Contains("using".ToUpper()))
            {
                throw new Exception("You are not allowed to use 'using' in your scripts");
            }
            if (code.ToUpper().Contains("System.".ToUpper()))
            {
                throw new Exception("You are not allowed to use 'System.' in your scripts");
            }
            var st = new StringBuilder();
            st.AppendLine("using LazyLib.Helpers;");
            st.AppendLine("using LazyLib.Wow;");
            st.AppendLine("using LazyLib.Combat;");
            st.AppendLine("using LazyLib.ActionBar;");
            st.AppendLine("using LazyEvo.Public;");
            st.AppendLine("using System.Threading;");
            st.AppendLine("private static PPlayerSelf Player = ObjectManager.MyPlayer;");
            st.AppendLine("private static PUnit Target = ObjectManager.MyPlayer.Target;");
            st.AppendLine("private static void CastSpell(string spell)");
            st.AppendLine("{");
            st.AppendLine("BarMapper.CastSpell(spell);");
            st.AppendLine("}");
            st.AppendLine("private static bool IsSpellReadyByName(string spell)");
            st.AppendLine("{");
            st.AppendLine("return BarMapper.IsSpellReadyByName(spell);");
            st.AppendLine("}");
            code = st + code;
            return code;
        }
    }
}