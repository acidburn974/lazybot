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
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using LazyEvo.Forms.Helpers;
using LazyEvo.PVEBehavior;
using LazyEvo.PVEBehavior.Behavior;
using LazyLib;

namespace LazyEvo.Plugins.RotationPlugin
{
    internal partial class RotationForm : Office2007Form
    {
        public Rotation Rotation;
        public bool Save;

        private Node _selected;
        private AdvTree _selectedTree;

        public RotationForm(Rotation rotation)
        {
            InitializeComponent();
            Rotation = rotation;
            Geometry.GeometryFromString(GeomertrySettings.RotationForm, this);
        }

        private void BtnAddScript_Click(object sender, EventArgs e)
        {
            var rule = new Rule();
            var scriptEditor = new ScriptEditor(rule);
            scriptEditor.Location = Location;
            scriptEditor.ShowDialog();
            if (scriptEditor.Save)
            {
                if (BeTabs.SelectedTab.Name.Equals("TabCombat"))
                {
                    AddCondition(rule, BeComRules);
                }
            }
        }

        private void BeComAddRuleClick(object sender, EventArgs e)
        {
            var rule = new Rule();
            var ruleEditor = new RuleEditor(rule, false);
            ruleEditor.Location = Location;
            ruleEditor.ShowDialog();
            if (ruleEditor.Save)
            {
                rule = ruleEditor.Rule;
                if (BeTabs.SelectedTab.Name.Equals("TabCombat"))
                {
                    AddCondition(rule, BeComRules);
                }
            }
        }

        private void AddCondition(Rule rule, AdvTree advTree)
        {
            var node = new Node();
            node.Text = rule.Name;
            node.Tag = rule;
            AddNode(node, advTree);
        }

        private void AddNode(Node node, AdvTree advTree)
        {
            advTree.BeginUpdate();
            advTree.Nodes.Add(node);
            advTree.EndUpdate();
        }

        private void BeComRulesNodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            EditRule(e.Node);
        }

        private void EditRule(Node node)
        {
            if (node.Tag is Rule)
            {
                var rule = (Rule) node.Tag;
                if (string.IsNullOrEmpty(rule.Script))
                {
                    var ruleEditor = new RuleEditor(rule, false);
                    ruleEditor.Location = Location;
                    ruleEditor.ShowDialog();
                    if (ruleEditor.Save)
                    {
                        node.Tag = rule;
                        node.Text = rule.Name;
                    }
                }
                else
                {
                    var scriptEditor = new ScriptEditor(rule);
                    scriptEditor.Location = Location;
                    scriptEditor.ShowDialog();
                    if (scriptEditor.Save)
                    {
                        node.Tag = rule;
                        node.Text = rule.Name;
                    }
                }
            }
        }

        private void BeComRulesNodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            _selected = e.Node;
            _selectedTree = BeComRules;
        }

        private void BeComDeleteRuleClick(object sender, EventArgs e)
        {
            if (_selected != null && _selectedTree != null)
            {
                _selectedTree.Nodes.Remove(_selected);
                _selectedTree = null;
                _selected = null;
            }
        }

        private void BeComRules_NodeDragFeedback(object sender, TreeDragFeedbackEventArgs e)
        {
            if (e.ParentNode != null)
                e.AllowDrop = false;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (TBRotationName.Text == "")
            {
                superTooltip1.SetSuperTooltip(TBRotationName,
                                              new SuperTooltipInfo("", "", "Please give the rotation a name.", null,
                                                                   null, eTooltipColor.Gray));
                superTooltip1.ShowTooltip(TBRotationName);
                return;
            }
            Rotation.Active = CBActive.Checked;
            Rotation.Name = TBRotationName.Text;
            Rotation.Shift = CBShift.Checked;
            Rotation.Alt = CBAlt.Checked;
            Rotation.Windows = CBWin.Checked;
            Rotation.Ctrl = CBCtrl.Checked;
            Rotation.Key = HotKey.SelectedItem.ToString();
            Rotation.ResetControllers();
            Rotation.GlobalCooldown = BeGlobalCooldown.Value;
            foreach (Node node in BeComRules.Nodes)
            {
                var rule = (Rule) node.Tag;
                rule.Priority = node.Index;
                Rotation.Rules.AddRule(rule);
            }
            Save = true;
            Close();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void RotationFormLoad(object sender, EventArgs e)
        {
            TBRotationName.Text = Rotation.Name;
            CBShift.Checked = Rotation.Shift;
            CBAlt.Checked = Rotation.Alt;
            CBWin.Checked = Rotation.Windows;
            CBCtrl.Checked = Rotation.Ctrl;
            CBActive.Checked = Rotation.Active;
            BeGlobalCooldown.Value = Rotation.GlobalCooldown;
            Rotation.Rules.GetRules.Sort();
            foreach (Rule rule in Rotation.Rules.GetRules)
            {
                AddCondition(rule, BeComRules);
            }
            foreach (KeysData item in RotationSettings.KeysList)
            {
                HotKey.Items.Add(item.Text);
            }
            if (HotKey.Items.Contains(Rotation.Key))
            {
                HotKey.SelectedIndex = HotKey.FindStringExact(Rotation.Key);
            }
            else
            {
                HotKey.SelectedIndex = 1;
            }
        }

        private void RotationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GeomertrySettings.RotationForm = Geometry.GeometryToString(this);
        }
    }
}