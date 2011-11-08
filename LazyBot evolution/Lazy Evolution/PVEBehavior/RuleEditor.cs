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
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using LazyEvo.Forms.Helpers;
using LazyEvo.PVEBehavior.Behavior;
using LazyEvo.PVEBehavior.Behavior.Conditions;
using LazyLib;
using Action = LazyEvo.PVEBehavior.Behavior.Action;

#endregion

namespace LazyEvo.PVEBehavior
{
    internal partial class RuleEditor : Office2007RibbonForm
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        private readonly bool _targetVisible;
        public Rule Rule;
        public bool Save;
        private AbstractCondition selected;

        public RuleEditor(Rule rule, bool targetVisible)
        {
            InitializeComponent();
            Rule = rule;
            Geometry.GeometryFromString(GeomertrySettings.RuleEditor, this);
            _targetVisible = targetVisible;
        }

        private void RuleEditor_Load(object sender, EventArgs e)
        {
            switch (Rule.ShouldTarget)
            {
                case Target.None:
                    RBNone.Checked = true;
                    break;
                case Target.Enemy:
                    RBEnemy.Checked = true;
                    break;
                case Target.Pet:
                    RBPet.Checked = true;
                    break;
                case Target.Self:
                    RBSelf.Checked = true;
                    break;
                case Target.Unchanged:
                    RBUnchanged.Checked = true;
                    break;
                default:
                    RBUnchanged.Checked = true;
                    break;
            }
            GPTarget.Visible = _targetVisible;
            SWMatchConditions.Value = Rule.MatchAll;
            TBRuleName.Text = Rule.Name;
            List<AbstractCondition> con = Rule.GetConditions;
            Action action = Rule.Action;
            ComBBar.SelectedIndex = 0;
            ComBSpecail.SelectedIndex = 0;
            ComBTimes.Value = 1;
            if (action is ActionSpell)
            {
                CBCastSpell.Checked = true;
                var actionSpell = (ActionSpell) action;
                TBSpellName.Text = actionSpell.Name;
            }
            else if (action is ActionKey)
            {
                CBSendKey.Checked = true;
                var actionkey = (ActionKey) action;
                TBKeyName.Text = actionkey.Name;
                TBKey.Text = actionkey.Key;
                ComBBar.SelectedIndex = ComBBar.FindStringExact(actionkey.Bar);
                ComBSpecail.SelectedIndex = ComBSpecail.FindStringExact(actionkey.Special);
                ComBTimes.Value = actionkey.Times;
            }
            else
            {
                CBCastSpell.Checked = true;
            }
            foreach (AbstractCondition condition in con)
            {
                LoadCondition(condition.Name, condition);
            }
            Save = false;
        }

        private void LoadCondition(string name, AbstractCondition condition)
        {
            var node = new Node();
            node.Text = name;
            node.Tag = condition;
            AddNode(node);
        }

        private void AddCondition(string name, AbstractCondition condition)
        {
            var node = new Node();
            node.Text = name;
            node.Tag = condition;
            AddNode(node);
        }

        private void AddNode(Node node)
        {
            AllConditions.BeginUpdate();
            AllConditions.Nodes.Add(node);
            AllConditions.EndUpdate();
        }

        private void AllConditions_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            Node node = e.Node;
            if (node.Tag is AbstractCondition)
            {
                AllConditions.BeginUpdate();
                selected = (AbstractCondition) node.Tag;
                ConditionEditor.Nodes.Clear();
                foreach (Node conNode in selected.GetNodes())
                {
                    ConditionEditor.Nodes.Add(conNode);
                }
                AllConditions.EndUpdate();
            }
        }

        private void ConditionEditor_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            if (selected != null)
            {
                selected.NodeClick(e.Node);
            }
        }

        private void CBCastSpell_CheckedChanged(object sender, EventArgs e)
        {
            if (CBCastSpell.Checked)
            {
                TBKeyName.Enabled = false;
                ComBTimes.Enabled = false;
                ComBBar.Enabled = false;
                ComBSpecail.Enabled = false;
                TBSpellName.Enabled = true;
                TBKey.Enabled = false;
                CBSendKey.Checked = false;
            }
            else
            {
                CBSendKey.Checked = true;
                CBCastSpell.Checked = false;
            }
        }

        private void CBSendKey_CheckedChanged(object sender, EventArgs e)
        {
            if (CBSendKey.Checked)
            {
                TBKeyName.Enabled = true;
                ComBTimes.Enabled = true;
                ComBBar.Enabled = true;
                ComBSpecail.Enabled = true;
                TBSpellName.Enabled = false;
                TBKey.Enabled = true;
                CBCastSpell.Checked = false;
            }
            else
            {
                CBCastSpell.Checked = true;
                CBSendKey.Checked = false;
            }
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (CBCastSpell.Checked)
            {
                if (TBSpellName.Text == "")
                {
                    superTooltip1.SetSuperTooltip(TBSpellName,
                                                  new SuperTooltipInfo("", "", "You need to type a spell name.", null,
                                                                       null, eTooltipColor.Gray));
                    superTooltip1.ShowTooltip(TBSpellName);
                    return;
                }
                Rule.Action = new ActionSpell(TBSpellName.Text);
            }
            if (CBSendKey.Checked)
            {
                if (TBKeyName.Text == "")
                {
                    superTooltip1.SetSuperTooltip(TBKeyName,
                                                  new SuperTooltipInfo("", "", "You need to type a key name.", null,
                                                                       null, eTooltipColor.Gray));
                    superTooltip1.ShowTooltip(TBKeyName);
                    return;
                }
                if (TBKey.Text == "")
                {
                    superTooltip1.SetSuperTooltip(TBKeyName,
                                                  new SuperTooltipInfo("", "", "You need to type a key.", null, null,
                                                                       eTooltipColor.Gray));
                    superTooltip1.ShowTooltip(TBKeyName);
                    return;
                }
                Rule.Action = new ActionKey(TBKeyName.Text, ComBBar.SelectedItem.ToString(), TBKey.Text,
                                            ComBSpecail.SelectedItem.ToString(), ComBTimes.Value);
            }
            if (TBRuleName.Text == "")
            {
                superTooltip1.SetSuperTooltip(TBSpellName,
                                              new SuperTooltipInfo("", "", "Please give the rule a name.", null, null,
                                                                   eTooltipColor.Gray));
                superTooltip1.ShowTooltip(TBSpellName);
                return;
            }
            if (AllConditions.Nodes.Count == 0)
            {
                superTooltip1.SetSuperTooltip(TBSpellName,
                                              new SuperTooltipInfo("", "", "Please create one condition", null, null,
                                                                   eTooltipColor.Gray));
                superTooltip1.ShowTooltip(TBSpellName);
                return;
            }
            Rule.MatchAll = SWMatchConditions.Value;
            Rule.Name = TBRuleName.Text;
            Rule.ClearConditions();
            foreach (Node node in AllConditions.Nodes)
            {
                Rule.AddCondition((AbstractCondition) node.Tag);
            }
            if (RBEnemy.Checked)
                Rule.ShouldTarget = Target.Enemy;
            if (RBNone.Checked)
                Rule.ShouldTarget = Target.None;
            if (RBPet.Checked)
                Rule.ShouldTarget = Target.Pet;
            if (RBSelf.Checked)
                Rule.ShouldTarget = Target.Self;
            if (RBUnchanged.Checked)
                Rule.ShouldTarget = Target.Unchanged;
            Save = true;
            Close();
        }

        private void BCancel_Click(object sender, EventArgs e)
        {
            Save = false;
            Close();
        }

        private void BtnRemoveCon_Click(object sender, EventArgs e)
        {
            if (AllConditions.SelectedNode != null)
            {
                AllConditions.Nodes.Remove(AllConditions.SelectedNode);
                ConditionEditor.Nodes.Clear();
            }
        }

        private void BuffDetection_Click(object sender, EventArgs e)
        {
            AddCondition(BuffDetection.Text, new BuffCondition());
        }

        private void AddCHealthPower_Click(object sender, EventArgs e)
        {
            AddCondition("Health/Power", new HealthPowerCondition());
        }

        private void CombatCount_Click(object sender, EventArgs e)
        {
            AddCondition(CombatCount.Text, new CombatCountCondition());
        }

        private void DistanceToTarget_Click(object sender, EventArgs e)
        {
            AddCondition(DistanceToTarget.Text, new DistanceToTarget());
        }

        private void SoulShardCount_Click(object sender, EventArgs e)
        {
            AddCondition(SoulShardCount.Text, new SoulShardCountCondition());
        }

        private void ComboPointsCondition_Click(object sender, EventArgs e)
        {
            AddCondition(ComboPointsCondition.Text, new ComboPointsCondition());
        }

        private void MageWaterCondition_Click(object sender, EventArgs e)
        {
            AddCondition(MageWaterCondition.Text, new MageWaterCondition());
        }

        private void MageFoodCondition_Click(object sender, EventArgs e)
        {
            AddCondition(MageFoodCondition.Text, new MageFoodCondition());
        }

        private void HealthStoneCount_Click(object sender, EventArgs e)
        {
            AddCondition(HealthStoneCount.Text, new HealthStoneCount());
        }

        private void HasTempEnchant_Click(object sender, EventArgs e)
        {
            AddCondition(HasTempEnchant.Text, new TempEnchantCondition());
        }

        private void RuneCondition_Click(object sender, EventArgs e)
        {
            AddCondition(RuneCondition.Text, new RuneCondition());
        }

        private void PotentialAddsCondition_Click(object sender, EventArgs e)
        {
            AddCondition(PotentialAdds.Text, new PotentialAddsCondition());
        }

        private void Functions_Click(object sender, EventArgs e)
        {
            AddCondition(Functions.Text, new FunctionsCondition());
        }

        private void Ticker_Click(object sender, EventArgs e)
        {
            AddCondition(Ticker.Text, new TickerCondition());
        }

        private void HasPet_Click(object sender, EventArgs e)
        {
            AddCondition(HasPet.Text, new PetCondition());
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
        }

        private void TBKey_TextChanged(object sender, EventArgs e)
        {
        }

        private void BtnSpellDetection_Click(object sender, EventArgs e)
        {
            AddCondition(BtnSpellDetection.Text, new SpellCondition());
        }

        private void RuleEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            GeomertrySettings.RuleEditor = Geometry.GeometryToString(this);
        }
    }
}