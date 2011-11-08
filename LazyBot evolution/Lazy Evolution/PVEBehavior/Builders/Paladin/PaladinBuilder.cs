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
using System.Linq;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using LazyEvo.PVEBehavior.Behavior;
using Type = LazyEvo.PVEBehavior.Behavior.Type;

namespace LazyEvo.PVEBehavior.Builders
{
    internal partial class PaladinBuilder : Office2007Form
    {
        private List<AddToBehavior> _items;

        public PaladinBuilder()
        {
            InitializeComponent();
        }

        private void BuilderLoad(object sender, EventArgs e)
        {
            TBName.Text = "Paladin";
            _items = Paladin.Load();
            foreach (AddToBehavior addToBehavior in _items.Where(addToBehavior => addToBehavior.Spec == Spec.Normal))
            {
                Normal.Items.Add(addToBehavior, false);
            }

            foreach (AddToBehavior addToBehavior in _items.Where(addToBehavior => addToBehavior.Spec == Spec.Tree1))
            {
                Spec1.Items.Add(addToBehavior, false);
            }

            foreach (AddToBehavior addToBehavior in _items.Where(addToBehavior => addToBehavior.Spec == Spec.Tree2))
            {
                Spec2.Items.Add(addToBehavior, false);
            }

            foreach (AddToBehavior addToBehavior in _items.Where(addToBehavior => addToBehavior.Spec == Spec.Tree3))
            {
                Spec3.Items.Add(addToBehavior, false);
            }
            foreach (AddToBehavior addToBehavior in _items.Where(addToBehavior => addToBehavior.Spec == Spec.Special))
            {
                CBSelectSpecial.Items.Add(addToBehavior);
            }
            foreach (AddToBehavior addToBehavior in _items.Where(addToBehavior => addToBehavior.Spec == Spec.Special2))
            {
                CBSelectSpecial2.Items.Add(addToBehavior);
            }
            foreach (AddToBehavior addToBehavior in _items.Where(addToBehavior => addToBehavior.Spec == Spec.Special3))
            {
                CBSelectSpecial3.Items.Add(addToBehavior);
            }
            CBSelectSpecial.SelectedIndex = 0;
            CBSelectSpecial2.SelectedIndex = 0;
            CBSelectSpecial3.SelectedIndex = 0;
            RBSpec1.Checked = true;
        }

        private void RbSpecChanged(object sender, EventArgs e)
        {
            Spec1.Enabled = RBSpec1.Checked;
            Spec2.Enabled = RBSpec2.Checked;
            Spec3.Enabled = RBSpec3.Checked;

            if (!Spec1.Enabled)
                for (int i = 0; i < Spec1.Items.Count; i++)
                    Spec1.SetItemChecked(i, false);

            if (!Spec2.Enabled)
                for (int i = 0; i < Spec2.Items.Count; i++)
                    Spec2.SetItemChecked(i, false);

            if (!Spec3.Enabled)
                for (int i = 0; i < Spec3.Items.Count; i++)
                    Spec3.SetItemChecked(i, false);
        }

        private void BtnCreateClick(object sender, EventArgs e)
        {
            if (File.Exists(PVEBehaviorCombat.OurDirectory + "\\Behaviors\\" + TBName.Text + ".xml"))
            {
                DialogResult result = MessageBoxEx.Show("Behavior exist - overwrite?", "Behavior exist - overwrite?",
                                                        MessageBoxButtons.OKCancel);
                switch (result)
                {
                    case DialogResult.Cancel:
                        return;
                }
            }
            var controller = new BehaviorController
                                 {
                                     SendPet = false,
                                     UseAutoAttack = true,
                                     PullDistance = 9,
                                     PrePullDistance = 30,
                                     CombatDistance = 3,
                                     GlobalCooldown = BeGlobalCooldown.Value,
                                     Name = TBName.Text,
                                     BuffController = new RuleController(),
                                     PrePullController = new RuleController(),
                                     PullController = new RuleController(),
                                     RestController = new RuleController(),
                                     CombatController = new RuleController()
                                 };
            for (int i = 0; i < Normal.Items.Count; i++)
            {
                if (Normal.GetItemChecked(i))
                {
                    var addToBehavior = (AddToBehavior) Normal.Items[i];
                    AddToController(addToBehavior, controller);
                }
            }
            for (int i = 0; i < Spec1.Items.Count; i++)
            {
                if (Spec1.GetItemChecked(i))
                {
                    var addToBehavior = (AddToBehavior) Spec1.Items[i];
                    AddToController(addToBehavior, controller);
                }
            }
            for (int i = 0; i < Spec2.Items.Count; i++)
            {
                if (Spec2.GetItemChecked(i))
                {
                    var addToBehavior = (AddToBehavior) Spec2.Items[i];
                    AddToController(addToBehavior, controller);
                }
            }
            for (int i = 0; i < Spec3.Items.Count; i++)
            {
                if (Spec3.GetItemChecked(i))
                {
                    var addToBehavior = (AddToBehavior) Spec3.Items[i];
                    AddToController(addToBehavior, controller);
                }
            }
            AddToController((AddToBehavior) CBSelectSpecial.SelectedItem, controller);
            AddToController((AddToBehavior) CBSelectSpecial2.SelectedItem, controller);
            AddToController((AddToBehavior) CBSelectSpecial3.SelectedItem, controller);
            controller.Save();
            PveBehaviorSettings.LoadedBeharvior = TBName.Text;
            PveBehaviorSettings.SaveSettings();
            MessageBoxEx.Show("Created behavior, re-open the behavior settings window to load it");
        }

        private static void AddToController(AddToBehavior addToBehavior, BehaviorController controller)
        {
            switch (addToBehavior.Type)
            {
                case Type.Combat:
                    controller.CombatController.AddRule(addToBehavior.Rule);
                    break;
                case Type.Pull:
                    controller.PullController.AddRule(addToBehavior.Rule);
                    break;
                case Type.Buff:
                    controller.BuffController.AddRule(addToBehavior.Rule);
                    break;
                case Type.Rest:
                    controller.RestController.AddRule(addToBehavior.Rule);
                    break;
                case Type.PrePull:
                    controller.PrePullController.AddRule(addToBehavior.Rule);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}