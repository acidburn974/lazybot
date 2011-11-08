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
using System.Windows.Forms;
using DevComponents.DotNetBar;
using LazyEvo.Forms.Helpers;
using LazyEvo.PVEBehavior.Behavior;
using LazyLib;

namespace LazyEvo.PVEBehavior
{
    internal partial class ScriptEditor : Office2007RibbonForm
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        public Rule Rule;
        public bool Save;

        public ScriptEditor(Rule rule)
        {
            InitializeComponent();
            Rule = rule;
            Geometry.GeometryFromString(GeomertrySettings.ScriptEditor, this);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if ((int) m.Result == HTCLIENT)
                        m.Result = (IntPtr) HTCAPTION;
                    return;
            }
            base.WndProc(ref m);
        }

        private void BtnSaveClick(object sender, EventArgs e)
        {
            if (TBRuleName.Text == "")
            {
                superTooltip1.SetSuperTooltip(TBRuleName,
                                              new SuperTooltipInfo("", "", "Please give the rule a name.", null, null,
                                                                   eTooltipColor.Gray));
                superTooltip1.ShowTooltip(TBRuleName);
                return;
            }
            Rule.Script = TBScript.Text;
            Rule.Name = TBRuleName.Text;
            Save = true;
            Close();
        }

        private void BCancelClick(object sender, EventArgs e)
        {
            Save = false;
            Close();
        }

        private void ScriptEditor_Load(object sender, EventArgs e)
        {
            TBScript.Text = Rule.Script;
            TBRuleName.Text = Rule.Name;
            if (string.IsNullOrEmpty(Rule.Script))
            {
                var st = new StringBuilder();
                st.AppendLine("public static bool ShouldRun()");
                st.AppendLine("{");
                st.AppendLine("     return true;");
                st.AppendLine("}");
                st.AppendLine("");
                st.AppendLine("public static void Run()");
                st.AppendLine("{");
                st.AppendLine("     Public.Write(\"Running\");");
                st.AppendLine("}");
                TBScript.Text = st.ToString();
            }
        }

        private void ScriptEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            GeomertrySettings.ScriptEditor = Geometry.GeometryToString(this);
        }
    }
}