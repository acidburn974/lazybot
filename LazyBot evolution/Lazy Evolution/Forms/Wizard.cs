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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using LazyEvo.Classes;
using LazyLib;

namespace LazyEvo.Forms
{
    internal partial class Wizard : Office2007Form
    {
        public Wizard()
        {
            InitializeComponent();
        }

        private void Wizard1WizardPageChanged(object sender, WizardPageChangeEventArgs e)
        {
            switch (Wizard1.SelectedPageIndex)
            {
                case 0:
                    Size = new Size(549, 220);
                    break;
                case 1:
                    Size = new Size(549, 220);
                    break;
                case 2:
                    Size = new Size(565, 665);
                    break;
                case 3:
                    Size = new Size(565, 665);
                    break;
                case 4:
                    Size = new Size(565, 665);
                    break;
                case 5:
                    Size = new Size(565, 665);
                    break;
                case 6:
                    Size = new Size(565, 665);
                    break;
                case 7:
                    Size = new Size(565, 665);
                    break;
                case 8:
                    Size = new Size(565, 665);
                    break;
                case 9:
                    LoadSettings();
                    Size = new Size(565, 521);
                    break;
                case 10:
                    LoadCustomClasses();
                    LoadEngines();
                    Size = new Size(565, 421);
                    break;
            }
        }

        private void WizardLoad(object sender, EventArgs e)
        {
            Size = new Size(549, 220);
        }

        private void LoadEngines()
        {
            SelectedEngine.Items.Clear();
            EngineCompiler.RecompileAll();
            //Now lets add the compiled dlls/files to our engine system
            foreach (var assembly in EngineCompiler.Assemblys)
            {
                var cs = new CustomEngine(assembly.Key, assembly.Value.Name);
                SelectedEngine.Items.Add(cs);
            }
            if (SelectedEngine.Items.Count != 0)
                SelectedEngine.SelectedIndex = 0;
        }

        private void LoadCustomClasses()
        {
            SelectCombat.Items.Clear();
            ClassCompiler.RecompileAll();
            //Now lets add the compiled dlls/files to our combat system
            foreach (var assembly in ClassCompiler.Assemblys)
            {
                var cs = new CustomClass(assembly.Key, assembly.Value.Name);
                SelectCombat.Items.Add(cs);
            }
            if (SelectCombat.Items.Count != 0)
                SelectCombat.SelectedIndex = 0;
        }

        private void LoadSettings()
        {
            SetupUseHotkeys.Checked = LazySettings.SetupUseHotkeys;
            SetupTBLogOutOnFollow.Text = LazySettings.LogOutOnFollowTime;
            SetupCBSoundFollow.Checked = LazySettings.SoundFollow;
            SetupCBSoundWhisper.Checked = LazySettings.SoundWhisper;
            SetupCBSoundStop.Checked = LazySettings.SoundStop;
            SetupCBBackground.Checked = LazySettings.BackgroundMode;
            SetupCBLogoutOnFollow.Checked = LazySettings.LogoutOnFollow;
            ClientLanguage.SelectedIndex = 0;
        }

        private void SaveSettings()
        {
            LazySettings.SetupUseHotkeys = SetupUseHotkeys.Checked;
            LazySettings.LogOutOnFollowTime = SetupTBLogOutOnFollow.Text;
            LazySettings.SoundFollow = SetupCBSoundFollow.Checked;
            LazySettings.SoundWhisper = SetupCBSoundWhisper.Checked;
            LazySettings.SoundStop = SetupCBSoundStop.Checked;
            LazySettings.HookMouse = SetupCBBackground.Checked;
            LazySettings.BackgroundMode = false;
            LazySettings.LogoutOnFollow = SetupCBLogoutOnFollow.Checked;
            string clientLanguage = ClientLanguage.SelectedItem.ToString();
            var lazyLanguage =
                (LazySettings.LazyLanguage) Enum.Parse(typeof (LazySettings.LazyLanguage), clientLanguage);
            LazySettings.Language = lazyLanguage;
            LazySettings.SaveSettings();
        }

        private void Wizard1FinishButtonClick(object sender, CancelEventArgs e)
        {
            LazySettings.SelectedCombat = SelectCombat.Text;
            LazySettings.SelectedEngine = SelectEngine.Text;
            LazySettings.FirstRun = false;
            LazySettings.SaveSettings();
            Close();
        }

        private void Wizard1WizardPageChanging(object sender, WizardCancelPageChangeEventArgs e)
        {
            if (Wizard1.SelectedPageIndex == 9)
            {
                SaveSettings();
            }
        }

        private void Wizard1CancelButtonClick(object sender, CancelEventArgs e)
        {
            Close();
        }

        private void SetupCbBackgroundCheckedChanged(object sender, EventArgs e)
        {
            if (SetupCBBackground.Checked && !LazySettings.HookMouse)
            {
                DialogResult result =
                    MessageBox.Show(
                        // ReSharper disable LocalizableElement
                        "Enabling this will make the bot manipulate wow in a way that could be detected if warden gets an update. The chance of this getting detected is between now and never. You will have to decide for yourself.",
                        // ReSharper restore LocalizableElement
                        "", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                if (result.ToString() != "Yes")
                {
                    SetupCBBackground.Checked = false;
                }
            }
        }

        private void labelX14_Click(object sender, EventArgs e)
        {
        }

        private void labelX23_Click(object sender, EventArgs e)
        {
        }
    }
}