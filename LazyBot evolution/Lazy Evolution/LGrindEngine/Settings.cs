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
using DevComponents.DotNetBar;
using LazyEvo.LGrindEngine.Helpers;

namespace LazyEvo.LGrindEngine
{
    internal partial class Settings : Office2007Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void SettingsLoad(object sender, EventArgs e)
        {
            GrindingSettings.LoadSettings();
            GrinderCBSkin.Checked = GrindingSettings.Skin;
            GrinderCBLoot.Checked = GrindingSettings.Loot;
            GrinderCBWaitForLoot.Checked = GrindingSettings.WaitForLoot;
            GrinderCBStopLootOnFull.Checked = GrindingSettings.StopLootOnFull;
            GrinderCBJumpRandomly.Checked = GrindingSettings.Jump;
            GrinderIntAppRange.Value = GrindingSettings.ApproachRange;
            GrinderCBSkipPullOnAdds.Checked = GrindingSettings.SkipMobsWithAdds;
            GrinderIntSkipAddsDis.Value = GrindingSettings.SkipAddsDistance;
            GrinderIntSkipAddMaxCount.Value = GrindingSettings.SkipAddsCount;
            UseMount.Checked = GrindingSettings.Mount;
            CBTrain.Checked = GrindingSettings.ShouldTrain;
        }

        private void SaveSettingsClick(object sender, EventArgs e)
        {
            GrindingSettings.Skin = GrinderCBSkin.Checked;
            GrindingSettings.Loot = GrinderCBLoot.Checked;
            GrindingSettings.WaitForLoot = GrinderCBWaitForLoot.Checked;
            GrindingSettings.StopLootOnFull = GrinderCBStopLootOnFull.Checked;
            GrindingSettings.Jump = GrinderCBJumpRandomly.Checked;
            GrindingSettings.ApproachRange = GrinderIntAppRange.Value;
            GrindingSettings.SkipMobsWithAdds = GrinderCBSkipPullOnAdds.Checked;
            GrindingSettings.SkipAddsDistance = GrinderIntSkipAddsDis.Value;
            GrindingSettings.SkipAddsCount = GrinderIntSkipAddMaxCount.Value;
            GrindingSettings.Mount = UseMount.Checked;
            GrindingSettings.ShouldTrain = CBTrain.Checked;
            GrindingSettings.SaveSettings();
            Close();
        }
    }
}