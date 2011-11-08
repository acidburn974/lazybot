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
using System.IO;
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using LazyEvo.PVEBehavior.Behavior;
using LazyEvo.PVEBehavior.Builders;

namespace LazyEvo.PVEBehavior
{
    internal partial class BehaviorForm : Office2007Form
    {
        private readonly string _ourDirectory;
        private BehaviorController _behavior;
        private bool _isLoading;

        public BehaviorForm(BehaviorController behaviorController)
        {
            InitializeComponent();
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            _ourDirectory = executableDirectoryName;
            PveBehaviorSettings.LoadSettings();
            _behavior = behaviorController ?? new BehaviorController();
        }

        private void BehaviorFormLoad(object sender, EventArgs e)
        {
            _isLoading = true;
            LoadBehaviors();
            _isLoading = false;
            SelectEngine.SelectedIndex = 0;
            BtnAllowScripts.Checked = PveBehaviorSettings.AllowScripts;
        }

        private void BehaviorFormFormClosing(object sender, FormClosingEventArgs e)
        {
            PveBehaviorSettings.AllowScripts = BtnAllowScripts.Checked;
            PveBehaviorSettings.SaveSettings();
            AskForSave();
        }

        private void AskForSave()
        {
            DialogResult dr = MessageBox.Show("Save currently selected behavior?", "Save", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    SaveCurrentBehavior();
                    break;
            }
        }

        private void BtnBehaviorGenerator_Click(object sender, EventArgs e)
        {
            switch (SelectEngine.SelectedIndex)
            {
                case 0:
                    var dk = new DeathknightBuilder();
                    dk.Show();
                    break;
                case 1:
                    var pa = new PaladinBuilder();
                    pa.Show();
                    break;
            }
        }

        private void BtnAllowScripts_CheckedChanged(object sender, EventArgs e)
        {
            if (BtnAllowScripts.Checked && BtnAllowScripts.Checked != PveBehaviorSettings.AllowScripts)
            {
                MessageBox.Show(
                    "This opens a potential security hole if you use behaviors from a unknown 3d party as it allows C# code to be run");
            }
        }

        #region Behavior

        private Node _selected;
        private AdvTree _selectedTree;

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
                if (BeTabs.SelectedTab.Name.Equals("TabBuffs"))
                {
                    AddCondition(rule, BeBuffRules);
                }
                if (BeTabs.SelectedTab.Name.Equals("TabPull"))
                {
                    AddCondition(rule, BePullRules);
                }
                if (BeTabs.SelectedTab.Name.Equals("TabRest"))
                {
                    AddCondition(rule, BeRestRules);
                }
                if (BeTabs.SelectedTab.Name.Equals("TabPrePull"))
                {
                    AddCondition(rule, BePrePullRules);
                }
            }
        }

        private void BeComAddRuleClick(object sender, EventArgs e)
        {
            var rule = new Rule();
            var ruleEditor = new RuleEditor(rule, true);
            ruleEditor.Location = Location;
            ruleEditor.ShowDialog();
            if (ruleEditor.Save)
            {
                rule = ruleEditor.Rule;
                if (BeTabs.SelectedTab.Name.Equals("TabCombat"))
                {
                    AddCondition(rule, BeComRules);
                }
                if (BeTabs.SelectedTab.Name.Equals("TabBuffs"))
                {
                    AddCondition(rule, BeBuffRules);
                }
                if (BeTabs.SelectedTab.Name.Equals("TabPull"))
                {
                    AddCondition(rule, BePullRules);
                }
                if (BeTabs.SelectedTab.Name.Equals("TabRest"))
                {
                    AddCondition(rule, BeRestRules);
                }
                if (BeTabs.SelectedTab.Name.Equals("TabPrePull"))
                {
                    AddCondition(rule, BePrePullRules);
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

        private void BeTbNewBehaviorClick(object sender, EventArgs e)
        {
            if (BeTBNewBehavior.Text.Equals("Enter name and press return to create new behavior."))
            {
                BeTBNewBehavior.Text = "";
            }
        }

        private void ClearTree(AdvTree advTree)
        {
            advTree.BeginUpdate();
            advTree.Nodes.Clear();
            advTree.EndUpdate(true);
        }


        private void BeTbNewBehaviorPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                SaveCurrentBehavior();
                _behavior = new BehaviorController {Name = BeTBNewBehavior.Text};
                _behavior.ResetControllers();
                ClearTree(BeComRules);
                ClearTree(BePullRules);
                ClearTree(BePrePullRules);
                ClearTree(BeBuffRules);
                ClearTree(BeRestRules);
                BeTBSelectBehavior.Items.Add(_behavior.Name);
                BeTBSelectBehavior.SelectedIndex = BeTBSelectBehavior.FindStringExact(_behavior.Name);
                BeTBNewBehavior.Text = "Enter name and press return to create new behavior.";
                PveBehaviorSettings.LoadedBeharvior = _behavior.Name;
                SaveCurrentBehavior();
                LoadBehavior();
            }
        }

        private void SaveCurrentBehavior()
        {
            if (_behavior.Name != string.Empty)
            {
                _behavior.ResetControllers();
                foreach (Node node in BeComRules.Nodes)
                {
                    var rule = (Rule) node.Tag;
                    rule.Priority = node.Index;
                    _behavior.CombatController.AddRule(rule);
                }
                foreach (Node node in BeBuffRules.Nodes)
                {
                    var rule = (Rule) node.Tag;
                    rule.Priority = node.Index;
                    _behavior.BuffController.AddRule(rule);
                }
                foreach (Node node in BePullRules.Nodes)
                {
                    var rule = (Rule) node.Tag;
                    rule.Priority = node.Index;
                    _behavior.PullController.AddRule(rule);
                }
                foreach (Node node in BeRestRules.Nodes)
                {
                    var rule = (Rule) node.Tag;
                    rule.Priority = node.Index;
                    _behavior.RestController.AddRule(rule);
                }
                foreach (Node node in BePrePullRules.Nodes)
                {
                    var rule = (Rule) node.Tag;
                    rule.Priority = node.Index;
                    _behavior.PrePullController.AddRule(rule);
                }
                _behavior.CombatDistance = BeCombatDistance.Value;
                _behavior.PullDistance = BePullDistance.Value;
                _behavior.PrePullDistance = BePrePullDistance.Value;
                _behavior.UseAutoAttack = BeEnableAutoAttack.Checked;
                _behavior.SendPet = BeSendPet.Checked;
                _behavior.GlobalCooldown = BeGlobalCooldown.Value;
                _behavior.Save();
            }
        }

        private void BeSaveBeheaviorClick(object sender, EventArgs e)
        {
            SaveCurrentBehavior();
        }

        private void LoadBehaviors()
        {
            //Lets reload the behaviors
            if (Directory.Exists(_ourDirectory + "\\Behaviors"))
            {
                BeTBSelectBehavior.Items.Clear();
                string[] files = Directory.GetFiles(_ourDirectory + "\\Behaviors", "*xml");
                foreach (string file in files)
                {
// ReSharper disable AssignNullToNotNullAttribute
                    BeTBSelectBehavior.Items.Add(Path.GetFileNameWithoutExtension(file));
// ReSharper restore AssignNullToNotNullAttribute
                }
                if (BeTBSelectBehavior.Items.Contains(PveBehaviorSettings.LoadedBeharvior))
                {
                    BeTBSelectBehavior.SelectedIndex =
                        BeTBSelectBehavior.FindStringExact(PveBehaviorSettings.LoadedBeharvior);
                }
            }
            if (string.IsNullOrEmpty(_behavior.Name))
            {
                BeTabs.Enabled = false;
                BeBarRuleModifier.Enabled = false;
                BeGMisc.Enabled = false;
            }
        }

        private void BeTbSelectBehaviorSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(_ourDirectory + "\\Behaviors"))
            {
                if (File.Exists(_ourDirectory + "\\Behaviors\\" + BeTBSelectBehavior.SelectedItem + ".xml"))
                {
                    if (!_isLoading)
                    {
                        AskForSave();
                    }
                    LoadBehavior();
                }
            }
        }

        private void LoadBehavior()
        {
            _behavior = new BehaviorController();
            ClearTree(BeComRules);
            ClearTree(BePullRules);
            ClearTree(BeRestRules);
            ClearTree(BeBuffRules);
            ClearTree(BePrePullRules);
            _behavior.Load(_ourDirectory + "\\Behaviors\\" + BeTBSelectBehavior.SelectedItem + ".xml");
            _behavior.CombatController.GetRules.Sort();
            _behavior.RestController.GetRules.Sort();
            _behavior.PullController.GetRules.Sort();
            _behavior.BuffController.GetRules.Sort();
            _behavior.PrePullController.GetRules.Sort();
            BeCombatDistance.Value = _behavior.CombatDistance;
            BePullDistance.Value = _behavior.PullDistance;
            BePrePullDistance.Value = _behavior.PrePullDistance;
            BeEnableAutoAttack.Checked = _behavior.UseAutoAttack;
            BeSendPet.Checked = _behavior.SendPet;
            BeGlobalCooldown.Value = _behavior.GlobalCooldown;
            foreach (Rule rule in _behavior.CombatController.GetRules)
            {
                AddCondition(rule, BeComRules);
            }
            foreach (Rule rule in _behavior.PullController.GetRules)
            {
                AddCondition(rule, BePullRules);
            }
            foreach (Rule rule in _behavior.RestController.GetRules)
            {
                AddCondition(rule, BeRestRules);
            }
            foreach (Rule rule in _behavior.BuffController.GetRules)
            {
                AddCondition(rule, BeBuffRules);
            }
            foreach (Rule rule in _behavior.PrePullController.GetRules)
            {
                AddCondition(rule, BePrePullRules);
            }
            BeTabs.Enabled = true;
            BeBarRuleModifier.Enabled = true;
            BeGMisc.Enabled = true;
            PveBehaviorSettings.LoadedBeharvior = BeTBSelectBehavior.SelectedItem.ToString();
            PveBehaviorSettings.SaveSettings();
        }

        private void BeBuffRulesNodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            EditRule(e.Node);
        }

        private void BePullRulesNodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            EditRule(e.Node);
        }

        private void BeRestRulesNodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            EditRule(e.Node);
        }

        private void BeComRulesNodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            EditRule(e.Node);
        }

        private void BePrePullRulesNodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
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
                    var ruleEditor = new RuleEditor(rule, true);
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

        private void BeBuffRulesNodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            _selected = e.Node;
            _selectedTree = BeBuffRules;
        }

        private void BePullRulesNodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            _selected = e.Node;
            _selectedTree = BePullRules;
        }

        private void BeRestRulesNodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            _selected = e.Node;
            _selectedTree = BeRestRules;
        }

        private void BePrePullRulesNodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            _selected = e.Node;
            _selectedTree = BePrePullRules;
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

        private void BePullRules_NodeDragFeedback(object sender, TreeDragFeedbackEventArgs e)
        {
            if (e.ParentNode != null)
                e.AllowDrop = false;
        }

        private void BeComRules_NodeDragFeedback(object sender, TreeDragFeedbackEventArgs e)
        {
            if (e.ParentNode != null)
                e.AllowDrop = false;
        }

        private void BeRestRules_NodeDragFeedback(object sender, TreeDragFeedbackEventArgs e)
        {
            if (e.ParentNode != null)
                e.AllowDrop = false;
        }

        private void BeBuffRules_NodeDragFeedback(object sender, TreeDragFeedbackEventArgs e)
        {
            if (e.ParentNode != null)
                e.AllowDrop = false;
        }

        private void BePrePullRules_NodeDragFeedback(object sender, TreeDragFeedbackEventArgs e)
        {
            if (e.ParentNode != null)
                e.AllowDrop = false;
        }

        #endregion
    }
}