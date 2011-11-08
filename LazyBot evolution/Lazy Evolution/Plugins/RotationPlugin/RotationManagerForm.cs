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
using LazyEvo.PVEBehavior;
using LazyEvo.PVEBehavior.Behavior;

namespace LazyEvo.Plugins.RotationPlugin
{
    internal partial class RotationManagerForm : Office2007Form
    {
        internal static string OurDirectory;
        private Node _selected;
        private AdvTree _selectedTree;
        private RotationManagerController rotationManagerController;

        public RotationManagerForm(RotationManagerController rotationManagerController)
        {
            InitializeComponent();
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            RotationSettings.LoadSettings();
            this.rotationManagerController = rotationManagerController ?? new RotationManagerController();
        }

        private void BehaviorFormLoad(object sender, EventArgs e)
        {
            LoadRotationManager();
            BtnAllowScripts.Checked = PveBehaviorSettings.AllowScripts;
        }

        private void BehaviorFormFormClosing(object sender, FormClosingEventArgs e)
        {
            PveBehaviorSettings.AllowScripts = BtnAllowScripts.Checked;
            RotationSettings.SaveSettings();
        }

        private void BeComAddRuleClick(object sender, EventArgs e)
        {
            var rotation = new Rotation();
            var rotationForm = new RotationForm(rotation);
            rotationForm.Location = Location;
            rotationForm.ShowDialog();
            if (rotationForm.Save)
            {
                rotation = rotationForm.Rotation;
                if (BeTabs.SelectedTab.Name.Equals("TabRotations"))
                {
                    AddCondition(rotation, BeRotations);
                }
            }
        }

        private void AddCondition(Rotation rotation, AdvTree advTree)
        {
            var node = new Node();
            node.Text = rotation.Name;
            node.Tag = rotation;
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
            if (BeTBNewBehavior.Text.Equals("Enter name and press return to create new."))
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
                SaveRotationManager();
                rotationManagerController = new RotationManagerController {Name = BeTBNewBehavior.Text};
                rotationManagerController.ResetControllers();
                ClearTree(BeRotations);
                BeTBSelectBehavior.Items.Add(rotationManagerController.Name);
                BeTBSelectBehavior.SelectedIndex = BeTBSelectBehavior.FindStringExact(rotationManagerController.Name);
                BeTBNewBehavior.Text = "Enter name and press return to create.";
                RotationSettings.LoadedRotationManager = rotationManagerController.Name;
                SaveRotationManager();
                LoadBehavior();
            }
        }

        private void SaveRotationManager()
        {
            if (rotationManagerController.Name != string.Empty)
            {
                rotationManagerController.ResetControllers();
                foreach (Node node in BeRotations.Nodes)
                {
                    var rotation = (Rotation) node.Tag;
                    rotationManagerController.Rotations.Add(rotation);
                }
                rotationManagerController.Save();
            }
        }

        private void BeSaveBeheaviorClick(object sender, EventArgs e)
        {
            SaveRotationManager();
        }

        private void LoadRotationManager()
        {
            //Lets reload the behaviors
            if (Directory.Exists(OurDirectory + "\\Rotations"))
            {
                BeTBSelectBehavior.Items.Clear();
                string[] files = Directory.GetFiles(OurDirectory + "\\Rotations", "*xml");
                foreach (string file in files)
                {
// ReSharper disable AssignNullToNotNullAttribute
                    BeTBSelectBehavior.Items.Add(Path.GetFileNameWithoutExtension(file));
// ReSharper restore AssignNullToNotNullAttribute
                }
                if (BeTBSelectBehavior.Items.Contains(RotationSettings.LoadedRotationManager))
                {
                    BeTBSelectBehavior.SelectedIndex =
                        BeTBSelectBehavior.FindStringExact(RotationSettings.LoadedRotationManager);
                }
            }
            if (string.IsNullOrEmpty(rotationManagerController.Name))
            {
                BeTabs.Enabled = false;
                BeBarRuleModifier.Enabled = false;
            }
        }

        private void BeTbSelectBehaviorSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(OurDirectory + "\\Rotations"))
            {
                if (File.Exists(OurDirectory + "\\Rotations\\" + BeTBSelectBehavior.SelectedItem + ".xml"))
                {
                    LoadBehavior();
                }
            }
        }

        private void LoadBehavior()
        {
            rotationManagerController = new RotationManagerController();
            ClearTree(BeRotations);
            rotationManagerController.Load(OurDirectory + "\\Rotations\\" + BeTBSelectBehavior.SelectedItem + ".xml");
            foreach (Rotation rule in rotationManagerController.Rotations)
            {
                AddCondition(rule, BeRotations);
            }
            BeTabs.Enabled = true;
            BeBarRuleModifier.Enabled = true;
            RotationSettings.LoadedRotationManager = BeTBSelectBehavior.SelectedItem.ToString();
            RotationSettings.SaveSettings();
        }

        private void BeComRulesNodeDoubleClick(object sender, TreeNodeMouseEventArgs e)
        {
            EditRule(e.Node);
        }

        private void EditRule(Node node)
        {
            if (node.Tag is Rotation)
            {
                var rotation = (Rotation) node.Tag;
                var rotationForm = new RotationForm(rotation);
                rotationForm.Location = Location;
                rotationForm.ShowDialog();
                if (rotationForm.Save)
                {
                    node.Tag = rotation;
                    node.Text = rotation.Name;
                }
            }
        }

        private void BeComRulesNodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            _selected = e.Node;
            _selectedTree = BeRotations;
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

        private void BtnAllowScripts_CheckedChanged(object sender, EventArgs e)
        {
            if (BtnAllowScripts.Checked && BtnAllowScripts.Checked != PveBehaviorSettings.AllowScripts)
            {
                MessageBox.Show(
                    "This opens a potential security hole if you use rotations from a unknown 3d party as it allows C# code to be run");
            }
        }

        private void Save()
        {
            if (rotationManagerController.Name != string.Empty)
            {
                rotationManagerController.ResetControllers();
                foreach (Node node in BeRotations.Nodes)
                {
                    var rotation = (Rotation) node.Tag;
                    // rule.Priority = node.Index;
                    rotationManagerController.Rotations.Add(rotation);
                }
                rotationManagerController.Save();
                RotationSettings.LoadedRotationManager = BeTBSelectBehavior.SelectedItem.ToString();
                RotationSettings.SaveSettings();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void BtnSaveAndClose_Click(object sender, EventArgs e)
        {
            Save();
            Close();
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            if (_selected != null && _selectedTree != null)
            {
                if (_selected.Tag is Rotation)
                {
                    var rotation = (Rotation) _selected.Tag;
                    var rotationCopy = new Rotation();
                    rotationCopy.Active = rotation.Active;
                    rotationCopy.Alt = rotation.Alt;
                    rotationCopy.Ctrl = rotation.Ctrl;
                    rotationCopy.GlobalCooldown = rotation.GlobalCooldown;
                    rotationCopy.Key = rotation.Key;
                    rotationCopy.Name = rotation.Name + " - copy";
                    rotationCopy.Shift = rotation.Shift;
                    rotationCopy.Windows = rotation.Windows;
                    foreach (Rule rule in rotation.Rules.GetRules)
                    {
                        rotationCopy.Rules.AddRule(rule);
                    }
                    AddCondition(rotationCopy, BeRotations);
                }
            }
        }
    }
}