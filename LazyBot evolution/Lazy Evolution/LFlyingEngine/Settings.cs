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
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using LazyEvo.LFlyingEngine.Helpers;

namespace LazyEvo.LFlyingEngine
{
    internal partial class Settings : Office2007Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void SettingsLoad(object sender, EventArgs e)
        {
            groupPanel5.Visible = true;
            // superTabItem2.Visible = LazyLib.License.LoadedLicense;
            SetupCBHerb.Checked = FlyingSettings.Herb;
            SetupCBMine.Checked = FlyingSettings.Mine;
            SetupTBApproachModifier.Text = FlyingSettings.ApproachModifier.ToString();
            SetupTBMaxUnits.Text = FlyingSettings.MaxUnits;
            SetupCBStopOnDeath.Checked = FlyingSettings.StopOnDeath;
            SetupCBStopHarvest.Checked = FlyingSettings.StopHarvestWithPlayerAround;
            SetupCBAvoidPlayers.Checked = FlyingSettings.AvoidPlayers;
            CBAutoBlacklist.Checked = FlyingSettings.AutoBlacklist;
            CBAvoidElites.Checked = FlyingSettings.AvoidElites;
            SetupCBFindCorpse.Checked = FlyingSettings.FindCorpse;
            CBStopOnFullBags.Checked = FlyingSettings.StopOnFullBags;
            KeysFlyingMountBar.SelectedIndex = KeysFlyingMountBar.FindStringExact(FlyingSettings.FlyingMountBar);
            KeysFlyingMountKey.SelectedIndex = KeysFlyingMountKey.FindStringExact(FlyingSettings.FlyingMountKey);
            CBRessWait.Checked = FlyingSettings.WaitForRessSick;
            CBWaitForLoot.Checked = FlyingSettings.WaitForLoot;
            CBDruidAvoidCombat.Checked = FlyingSettings.DruidAvoidCombat;
            KeysExtraBar.SelectedIndex = KeysExtraBar.FindStringExact(FlyingSettings.ExtraBar);
            KeysExtraKey.SelectedIndex = KeysExtraKey.FindStringExact(FlyingSettings.ExtraKey);
            CBSendKeyOnStartCombat.Checked = FlyingSettings.SendKeyOnStartCombat;
            //Fish
            CBFish.Checked = FlyingSettings.Fish;
            CBUseLure.Checked = FlyingSettings.Lure;
            SetupTBMaxTimeAtSchool.Value = Convert.ToInt32(FlyingSettings.MaxTimeAtSchool);
            SetupTBFishApproach.Value = Convert.ToInt32(FlyingSettings.FishApproach);
            KeysLureBar.SelectedIndex = KeysLureBar.FindStringExact(FlyingSettings.LureBar);
            KeysLureKey.SelectedIndex = KeysLureKey.FindStringExact(FlyingSettings.LureKey);
            KeysWaterwalkBar.SelectedIndex = KeysWaterwalkBar.FindStringExact(FlyingSettings.WaterwalkBar);
            KeysWaterwalkKey.SelectedIndex = KeysWaterwalkKey.FindStringExact(FlyingSettings.WaterwalkKey);

            LoadHerbList();
            LoadMineList();
            LoadSchoolList();
        }

        private void SaveSettingsClick(object sender, EventArgs e)
        {
            FlyingSettings.Herb = SetupCBHerb.Checked;
            FlyingSettings.Mine = SetupCBMine.Checked;
            FlyingSettings.ApproachModifier = (float) Convert.ToDouble(SetupTBApproachModifier.Text);
            FlyingSettings.MaxUnits = SetupTBMaxUnits.Text;
            FlyingSettings.StopOnDeath = SetupCBStopOnDeath.Checked;
            FlyingSettings.StopHarvestWithPlayerAround = SetupCBStopHarvest.Checked;
            FlyingSettings.AvoidPlayers = SetupCBAvoidPlayers.Checked;
            FlyingSettings.FlyingMountBar = KeysFlyingMountBar.SelectedItem.ToString();
            FlyingSettings.FlyingMountKey = KeysFlyingMountKey.SelectedItem.ToString();
            FlyingSettings.AutoBlacklist = CBAutoBlacklist.Checked;
            FlyingSettings.AvoidElites = CBAvoidElites.Checked;
            FlyingSettings.FindCorpse = SetupCBFindCorpse.Checked;
            FlyingSettings.StopOnFullBags = CBStopOnFullBags.Checked;
            FlyingSettings.WaitForRessSick = CBRessWait.Checked;
            FlyingSettings.WaitForLoot = CBWaitForLoot.Checked;
            FlyingSettings.DruidAvoidCombat = CBDruidAvoidCombat.Checked;
            FlyingSettings.ExtraBar = KeysExtraBar.SelectedItem.ToString();
            FlyingSettings.ExtraKey = KeysExtraKey.SelectedItem.ToString();
            FlyingSettings.SendKeyOnStartCombat = CBSendKeyOnStartCombat.Checked;
            //Fish
            FlyingSettings.Fish = CBFish.Checked;
            FlyingSettings.Lure = CBUseLure.Checked;
            FlyingSettings.MaxTimeAtSchool = SetupTBMaxTimeAtSchool.Value;
            FlyingSettings.FishApproach = SetupTBFishApproach.Value;
            FlyingSettings.LureBar = KeysLureBar.SelectedItem.ToString();
            FlyingSettings.LureKey = KeysLureKey.SelectedItem.ToString();
            FlyingSettings.WaterwalkBar = KeysWaterwalkBar.SelectedItem.ToString();
            FlyingSettings.WaterwalkKey = KeysWaterwalkKey.SelectedItem.ToString();

            FlyingSettings.SaveSettings();
            SaveHerbList();
            SaveMineList();
            SaveSchoolList();
            Close();
        }

        private void BtnAddMineClick(object sender, EventArgs e)
        {
            if (TBMineName.Text != "")
            {
                AddMine(TBMineName.Text);
                TBMineName.Text = "";
            }
        }

        private void BtnRemoveMineClick(object sender, EventArgs e)
        {
            if (ListMineItems.SelectedNode != null)
            {
                ListMineItems.Nodes.Remove(ListMineItems.SelectedNode);
            }
        }

        private void AddMine(string name)
        {
            var add = new Node(name);
            add.Tag = name;
            ListMineItems.BeginUpdate();
            ListMineItems.Nodes.Add(add);
            ListMineItems.EndUpdate();
        }

        private void LoadHerbList()
        {
            Herb.Load();
            foreach (string mail in Herb.GetList())
            {
                AddHerb(mail);
            }
        }

        private void SaveHerbList()
        {
            Herb.Clear();
            foreach (Node node in ListHerbItems.Nodes)
            {
                Herb.AddHerb(node.Tag.ToString());
            }
            Herb.Save();
        }

        private void LoadMineList()
        {
            Mine.Load();
            foreach (string pro in Mine.GetList())
            {
                AddMine(pro);
            }
        }


        private void LoadSchoolList()
        {
            /*
            School.Load();
            foreach (string pro in School.GetList())
            {
                AddSchool(pro);
            } */
        }


        private void SaveMineList()
        {
            Mine.Clear();
            foreach (Node node in ListMineItems.Nodes)
            {
                Mine.AddMine(node.Tag.ToString());
            }
            Mine.Save();
        }

        private void SaveSchoolList()
        {
            /*
            School.Clear();
            foreach (Node node in ListSchoolItems.Nodes)
            {
                School.AddSchool(node.Tag.ToString());
            }
            School.Save(); */
        }

        private void SetupCbStopOnDeathCheckedChanged(object sender, EventArgs e)
        {
            if (SetupCBStopOnDeath.Checked)
            {
                SetupCBFindCorpse.Checked = false;
            }
        }

        private void SetupCbFindCorpseCheckedChanged(object sender, EventArgs e)
        {
            if (SetupCBFindCorpse.Checked)
            {
                SetupCBStopOnDeath.Checked = false;
            }
        }

        private void BtnAddHerbClick(object sender, EventArgs e)
        {
            if (TBHerbName.Text != "")
            {
                AddHerb(TBHerbName.Text);
                TBHerbName.Text = "";
            }
        }

        private void BtnRemoveHerbClick(object sender, EventArgs e)
        {
            if (ListHerbItems.SelectedNode != null)
            {
                ListHerbItems.Nodes.Remove(ListHerbItems.SelectedNode);
            }
        }

        private void AddHerb(string name)
        {
            var add = new Node(name);
            add.Tag = name;
            ListHerbItems.BeginUpdate();
            ListHerbItems.Nodes.Add(add);
            ListHerbItems.EndUpdate();
        }

        private void BtnAddSchool_Click(object sender, EventArgs e)
        {
            if (TBSchoolName.Text != "")
            {
                AddSchool(TBSchoolName.Text);
                TBSchoolName.Text = "";
            }
        }

        private void AddSchool(string name)
        {
            var add = new Node(name);
            add.Tag = name;
            ListSchoolItems.BeginUpdate();
            ListSchoolItems.Nodes.Add(add);
            ListSchoolItems.EndUpdate();
        }

        private void BtnRemoveSchool_Click(object sender, EventArgs e)
        {
            if (ListSchoolItems.SelectedNode != null)
            {
                ListSchoolItems.Nodes.Remove(ListSchoolItems.SelectedNode);
            }
        }
    }
}