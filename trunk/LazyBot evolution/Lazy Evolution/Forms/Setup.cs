/*
This file is part of LazyBot.

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
using System.Diagnostics;
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using LazyEvo.Classes;
using LazyEvo.Forms.Helpers;
using LazyEvo.Plugins;
using LazyLib;
using LazyLib.Helpers;
using LazyLib.Helpers.Mail;
using LazyLib.Helpers.Vendor;

#endregion

namespace LazyEvo.Forms
{
    internal partial class Setup : Office2007Form
    {
        #region Field Setup and load

        public Setup()
        {
            InitializeComponent();
            Hide();
        }

        private void Form1Load(object sender, EventArgs e)
        {
            LoadSettings();
            HideAgain();
            PluginsList.Items.Clear();
            LoadP();
        }

        #endregion

        #region LoadSettings and SaveSettings

        private void LoadMailList()
        {
            MailList.Load();
            foreach (string mail in MailList.GetList)
            {
                AddMail(mail);
            }
        }

        private void SaveMailList()
        {
            MailList.Clear();
            foreach (Node node in ListMailItems.Nodes)
            {
                MailList.AddMail(node.Tag.ToString());
            }
            MailList.Save();
        }

        private void LoadProtectedList()
        {
            ProtectedList.Load();
            foreach (string pro in ProtectedList.GetList)
            {
                AddProtected(pro);
            }
        }

        private void SaveProtectedList()
        {
            ProtectedList.Clear();
            foreach (Node node in ListProtectedItems.Nodes)
            {
                ProtectedList.AddProtected(node.Tag.ToString());
            }
            ProtectedList.Save();
        }


        public void LoadSettings()
        {
            SetupDebugMode.Checked = LazySettings.DebugMode;
            SetupTBUserName.Text = LazySettings.UserName;
            SetupTBPassword.Text = LazySettings.Password;
            SetupUseHotkeys.Checked = LazySettings.SetupUseHotkeys;
            SetupCBStopAfter.Checked = LazySettings.StopAfterBool;
            SetupTBStopAfter.Text = LazySettings.StopAfter;
            SetupTBLogOutOnFollow.Text = LazySettings.LogOutOnFollowTime;
            SetupCBSoundFollow.Checked = LazySettings.SoundFollow;
            SetupCBSoundWhisper.Checked = LazySettings.SoundWhisper;
            SetupCBSoundStop.Checked = LazySettings.SoundStop;
            SetupCBShutdown.Checked = LazySettings.Shutdown;
            SetupCBBackground.Checked = LazySettings.BackgroundMode;
            SetupCBLogoutOnFollow.Checked = LazySettings.LogoutOnFollow;
            CBHookMouse.Checked = LazySettings.HookMouse;
            Latency.Value = LazySettings.Latency;
            //
            CombatCBEat.Checked = LazySettings.CombatBoolEat;
            CombatCBDrink.Checked = LazySettings.CombatBoolDrink;
            CombatTBEatAt.Text = LazySettings.CombatEatAt;
            CombatTBDrinkAt.Text = LazySettings.CombatDrinkAt;

            KeysGroundMountBar.SelectedIndex = KeysGroundMountBar.FindStringExact(LazySettings.KeysGroundMountBar);
            KeysGroundMountKey.SelectedIndex = KeysGroundMountKey.FindStringExact(LazySettings.KeysGroundMountKey);
            KeysAttack1Bar.SelectedIndex = KeysAttack1Bar.FindStringExact(LazySettings.KeysAttack1Bar);
            KeysAttack1Key.SelectedIndex = KeysAttack1Key.FindStringExact(LazySettings.KeysAttack1Key);
            KeysEatBar.SelectedIndex = KeysEatBar.FindStringExact(LazySettings.KeysEatBar);
            KeysEatKey.SelectedIndex = KeysEatKey.FindStringExact(LazySettings.KeysEatKey);
            KeysDrinkBar.SelectedIndex = KeysDrinkBar.FindStringExact(LazySettings.KeysDrinkBar);
            KeysDrinkKey.SelectedIndex = KeysDrinkKey.FindStringExact(LazySettings.KeysDrinkKey);
            KeysInteractKey.SelectedIndex = KeysInteractKey.FindStringExact(LazySettings.KeysInteractKeyText);
            KeysInteractTarget.SelectedIndex = KeysInteractTarget.FindStringExact(LazySettings.KeysInteractTargetText);
            KeysStafeLeftKey.SelectedIndex = KeysStafeLeftKey.FindStringExact(LazySettings.KeysStafeLeftKeyText);
            KeysStafeRightKey.SelectedIndex = KeysStafeRightKey.FindStringExact(LazySettings.KeysStafeRightKeyText);
            KeysTargetLast.SelectedIndex = KeysTargetLast.FindStringExact(LazySettings.KeysTargetLastTargetText);

            //Vendor
            CBDoVendor.Checked = LazySettings.ShouldVendor;
            CBDoRepair.Checked = LazySettings.ShouldRepair;
            CBSellCommon.Checked = LazySettings.SellCommon;
            CBSellUnCommon.Checked = LazySettings.SellUncommon;
            CBSellPoor.Checked = LazySettings.SellPoor;
            IMinFreeBagSlots.Text = LazySettings.FreeBackspace;

            //Mail
            CBMail.Checked = LazySettings.ShouldMail;
            TBMailTo.Text = LazySettings.MailTo;
            MacroForMail.Checked = LazySettings.MacroForMail;
            KeysMailMacroBar.SelectedIndex = KeysMailMacroBar.FindStringExact(LazySettings.KeysMailMacroBar);
            KeysMailMacroKey.SelectedIndex = KeysMailMacroKey.FindStringExact(LazySettings.KeysMailMacroKey);

            //Relogger
            SetupTBRelogUsername.Text = ReloggerSettings.AccountName;
            SetupTBRelogPW.Text = ReloggerSettings.AccountPw;
            SetupTBRelogCharacter.Text = ReloggerSettings.CharacterName;
            SetupCBRelogEnableRelogger.Checked = ReloggerSettings.ReloggingEnabled;
            SetupCBRelogEnablePeriodicRelog.Checked = ReloggerSettings.PeriodicReloggingEnabled;
            SetupIIRelogLogOutAfter.Value = ReloggerSettings.PeriodicLogOut;
            SetupIIRelogLogInAfter.Value = ReloggerSettings.PeriodicLogIn;
            SetupIIRelogLogAccount.Value = ReloggerSettings.AccountAccount;

            //Language
            if (LazySettings.Language != LazySettings.LazyLanguage.Unknown)
            {
                ClientLanguage.SelectedIndex = ClientLanguage.FindStringExact(LazySettings.Language.ToString());
            }
            else
            {
                ClientLanguage.SelectedIndex = 0;
            }
            LoadMailList();
            LoadProtectedList();
        }

        public void SaveSettings()
        {
            LazySettings.DebugMode = SetupDebugMode.Checked;
            LazySettings.UserName = SetupTBUserName.Text;
            LazySettings.Password = SetupTBPassword.Text;
            LazySettings.SetupUseHotkeys = SetupUseHotkeys.Checked;
            LazySettings.StopAfterBool = SetupCBStopAfter.Checked;
            LazySettings.StopAfter = SetupTBStopAfter.Text;
            LazySettings.LogOutOnFollowTime = SetupTBLogOutOnFollow.Text;
            LazySettings.SoundFollow = SetupCBSoundFollow.Checked;
            LazySettings.SoundWhisper = SetupCBSoundWhisper.Checked;
            LazySettings.SoundStop = SetupCBSoundStop.Checked;
            LazySettings.Shutdown = SetupCBShutdown.Checked;
            LazySettings.BackgroundMode = SetupCBBackground.Checked;
            LazySettings.LogoutOnFollow = SetupCBLogoutOnFollow.Checked;
            LazySettings.HookMouse = CBHookMouse.Checked;
            LazySettings.Latency = Latency.Value;
            //
            LazySettings.CombatBoolEat = CombatCBEat.Checked;
            LazySettings.CombatBoolDrink = CombatCBDrink.Checked;
            LazySettings.CombatEatAt = CombatTBEatAt.Text;
            LazySettings.CombatDrinkAt = CombatTBDrinkAt.Text;

            LazySettings.KeysGroundMountBar = KeysGroundMountBar.SelectedItem.ToString();
            LazySettings.KeysGroundMountKey = KeysGroundMountKey.SelectedItem.ToString();
            LazySettings.KeysAttack1Bar = KeysAttack1Bar.SelectedItem.ToString();
            LazySettings.KeysAttack1Key = KeysAttack1Key.SelectedItem.ToString();
            LazySettings.KeysEatBar = KeysEatBar.SelectedItem.ToString();
            LazySettings.KeysEatKey = KeysEatKey.SelectedItem.ToString();
            LazySettings.KeysDrinkBar = KeysDrinkBar.SelectedItem.ToString();
            LazySettings.KeysDrinkKey = KeysDrinkKey.SelectedItem.ToString();
            LazySettings.KeysStafeLeftKeyText = KeysStafeLeftKey.SelectedItem.ToString();
            LazySettings.KeysStafeRightKeyText = KeysStafeRightKey.SelectedItem.ToString();
            LazySettings.KeysInteractKeyText = KeysInteractKey.SelectedItem.ToString();
            LazySettings.KeysInteractTargetText = KeysInteractTarget.SelectedItem.ToString();
            LazySettings.KeysTargetLastTargetText = KeysTargetLast.SelectedItem.ToString();

            //Mail
            LazySettings.ShouldMail = CBMail.Checked;
            LazySettings.MailTo = TBMailTo.Text;
            LazySettings.MacroForMail = MacroForMail.Checked;
            LazySettings.KeysMailMacroBar = KeysMailMacroBar.SelectedItem.ToString();
            LazySettings.KeysMailMacroKey = KeysMailMacroKey.SelectedItem.ToString();
            //Vendor
            LazySettings.ShouldVendor = CBDoVendor.Checked;
            LazySettings.ShouldRepair = CBDoRepair.Checked;
            LazySettings.SellCommon = CBSellCommon.Checked;
            LazySettings.SellUncommon = CBSellUnCommon.Checked;
            LazySettings.SellPoor = CBSellPoor.Checked;
            LazySettings.FreeBackspace = IMinFreeBagSlots.Text;

            //Relogger
            ReloggerSettings.AccountName = SetupTBRelogUsername.Text;
            ReloggerSettings.AccountPw = SetupTBRelogPW.Text;
            ReloggerSettings.CharacterName = SetupTBRelogCharacter.Text;
            ReloggerSettings.ReloggingEnabled = SetupCBRelogEnableRelogger.Checked;
            ReloggerSettings.PeriodicReloggingEnabled = SetupCBRelogEnablePeriodicRelog.Checked;
            ReloggerSettings.PeriodicLogIn = SetupIIRelogLogInAfter.Value;
            ReloggerSettings.PeriodicLogOut = SetupIIRelogLogOutAfter.Value;
            ReloggerSettings.AccountAccount = SetupIIRelogLogAccount.Value;
            ReloggerSettings.SaveSettings();

            //Language
            string clientLanguage = ClientLanguage.SelectedItem.ToString();
            var lazyLanguage =
                (LazySettings.LazyLanguage) Enum.Parse(typeof (LazySettings.LazyLanguage), clientLanguage);
            if (LazySettings.Language != lazyLanguage)
            {
                if (!ItemDatabase.IsOpen)
                {
                    ItemDatabase.Open();
                }
                ItemDatabase.ClearDatabase();
            }
            LazySettings.Language = lazyLanguage;
            LazySettings.SaveSettings();
            SaveMailList();
            SaveProtectedList();
        }

        #endregion

        #region UpdateControls

        public void EnableBtn(ButtonX buttonX)
        {
            if (buttonX.InvokeRequired)
            {
                buttonX.BeginInvoke(
                    new MethodInvoker(
                        delegate { EnableBtn(buttonX); }));
            }
            else
            {
                buttonX.Enabled = true;
            }
        }


        private void HideAgain()
        {
            if (InvokeRequired)
            {
                BeginInvoke(
                    new MethodInvoker(
                        delegate { HideAgain(); }));
            }
            else
            {
                Hide();
            }
        }

        public void UpdateTextLabel(LabelX labelX, string text)
        {
            if (labelX.InvokeRequired)
            {
                labelX.BeginInvoke(
                    new MethodInvoker(
                        delegate { UpdateTextLabel(labelX, text); }));
            }
            else
            {
                labelX.Text = text;
            }
        }

        public void UpdateProgressBar(ProgressBarX progressBarX, int healtPercentage)
        {
            if (progressBarX.InvokeRequired)
            {
                progressBarX.BeginInvoke(
                    new MethodInvoker(
                        delegate { UpdateProgressBar(progressBarX, healtPercentage); }));
            }
            else
            {
                progressBarX.Value = healtPercentage;
            }
        }

        /// <summary>
        ///   Updates the text.
        /// </summary>
        /// <param name = "lab">The lab.</param>
        /// <param name = "text">The text.</param>
        public void UpdateText(ButtonX lab, string text)
        {
            if (lab.InvokeRequired)
            {
                lab.Invoke(
                    new MethodInvoker(
                        delegate { UpdateText(lab, text); }));
            }
            else
            {
                lab.Text = text;
            }
        }

        public void UpdateTitle(string text)
        {
            if (InvokeRequired)
            {
                Invoke(
                    new MethodInvoker(
                        delegate { UpdateTitle(text); }));
            }
            else
            {
                Text = text;
            }
        }

        public void DoRefresh()
        {
            if (InvokeRequired)
            {
                Invoke(
                    new MethodInvoker(
                        DoRefresh));
            }
            else
            {
                Refresh();
            }
        }

        #endregion

        #region ClosingLaunch

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            LoadSettings();
            e.Cancel = true;
        }

        #endregion

        private void SetupCbBackgroundCheckedChanged(object sender, EventArgs e)
        {
            if (SetupCBBackground.Checked && !LazySettings.BackgroundMode)
            {
                DialogResult result =
                    MessageBox.Show(
// ReSharper disable LocalizableElement
                        "Enabling this will make the bot manipulate wow in a way that could be detected if warden gets an update. The chance of this getting detected is between now and never. You will have to decide for yourself.",
// ReSharper restore LocalizableElement
                        "", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                if (result.ToString() == "Yes")
                {
                }
                else
                {
                    SetupCBBackground.Checked = false;
                }
            }
        }

        private void CloseForm(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.mmo-lazybot.com/forum");
        }

        private void LoadP()
        {
            foreach (var keyValuePair in PluginCompiler.Assemblys)
            {
                var temp = new CustomPlugin(keyValuePair.Key, keyValuePair.Value.GetName());
                PluginsList.Items.Add(temp);
                PluginsList.SetItemChecked(PluginsList.Items.Count - 1, LoadPluginSettings(temp.AssemblyName));
            }
        }

        private static void WritePluginSettings(string name, bool enabled)
        {
            try
            {
                var pIniManager = new IniManager(LazyForms.OurDirectory + "\\Settings\\lazy_plugins.ini");
                pIniManager.IniWriteValue("Plugins", name, enabled.ToString());
            }
            catch
            {
            }
        }

        private static bool LoadPluginSettings(string name)
        {
            try
            {
                var pIniManager = new IniManager(LazyForms.OurDirectory + "\\Settings\\lazy_plugins.ini");
                return pIniManager.GetBoolean("Plugins", name, false);
            }
            catch
            {
            }
            return false;
        }

        private void PluginsListItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                var p = (CustomPlugin) PluginsList.Items[e.Index];
                WritePluginSettings(p.AssemblyName, e.NewValue.Equals(CheckState.Checked));
                if (e.NewValue.Equals(CheckState.Checked))
                {
                    if (!PluginCompiler.LoadedPlugins.Contains(p.AssemblyName))
                        PluginCompiler.PluginLoad(p.AssemblyName);
                }
                else
                {
                    if (PluginCompiler.LoadedPlugins.Contains(p.AssemblyName))
                        PluginCompiler.PluginUnload(p.AssemblyName);
                }
            }
            catch (Exception d)
            {
                Logging.Write("Could not load plugin: " + d);
            }
        }

        private void BtnAddMailItemClick(object sender, EventArgs e)
        {
            if (TBMailName.Text != "")
            {
                AddMail(TBMailName.Text);
                TBMailName.Text = "";
            }
        }

        private void AddMail(string name)
        {
            var add = new Node(name);
            add.Tag = name;
            ListMailItems.BeginUpdate();
            ListMailItems.Nodes.Add(add);
            ListMailItems.EndUpdate();
        }

        private void BtnRemoveMailItemClick(object sender, EventArgs e)
        {
            if (ListMailItems.SelectedNode != null)
            {
                ListMailItems.Nodes.Remove(ListMailItems.SelectedNode);
            }
        }

        private void BtnAddProtected_Click(object sender, EventArgs e)
        {
            if (TBProtectedName.Text != "")
            {
                AddProtected(TBProtectedName.Text);
                TBProtectedName.Text = "";
            }
        }

        private void BtnRemoveProtected_Click(object sender, EventArgs e)
        {
            if (ListProtectedItems.SelectedNode != null)
            {
                ListProtectedItems.Nodes.Remove(ListProtectedItems.SelectedNode);
            }
        }

        private void AddProtected(string name)
        {
            var add = new Node(name);
            add.Tag = name;
            ListProtectedItems.BeginUpdate();
            ListProtectedItems.Nodes.Add(add);
            ListProtectedItems.EndUpdate();
        }

        private void superTabControlPanel4_Click(object sender, EventArgs e)
        {
        }

        private void SetupCBRelogEnablePeriodicRelog_CheckedChanged(object sender, EventArgs e)
        {
            if (SetupCBStopAfter.Checked && SetupCBRelogEnablePeriodicRelog.Checked)
            {
                SetupCBStopAfter.Checked = false;
            }
            if (SetupCBRelogEnablePeriodicRelog.Checked)
            {
                SetupIIRelogLogInAfter.Enabled = true;
                SetupIIRelogLogOutAfter.Enabled = true;
            }
            else
            {
                SetupIIRelogLogInAfter.Enabled = false;
                SetupIIRelogLogOutAfter.Enabled = false;
            }
        }

        private void SetupCBRelogEnableRelogger_CheckedChanged(object sender, EventArgs e)
        {
            if (SetupCBRelogEnableRelogger.Checked)
            {
                SetupRelogLoginData.Visible = true;
            }
            else
            {
                SetupCBRelogEnablePeriodicRelog.Checked = false;
                SetupRelogLoginData.Visible = false;
            }
        }

        private void SetupCBStopAfter_CheckedChanged(object sender, EventArgs e)
        {
            if (SetupCBRelogEnablePeriodicRelog.Checked && SetupCBStopAfter.Checked)
            {
                SetupCBRelogEnablePeriodicRelog.Checked = false;
            }
        }

        private void CBHookMouse_CheckedChanged(object sender, EventArgs e)
        {
            if (CBHookMouse.Checked != LazySettings.HookMouse)
            {
                if (CBHookMouse.Checked && !LazySettings.HookMouse)
                {
                    DialogResult result =
                        MessageBox.Show(
                            // ReSharper disable LocalizableElement
                            "Enabling this will make the bot manipulate wow in a way that could be detected if warden gets an update. The chance of this getting detected is between now and never. You will have to decide for yourself.",
                            // ReSharper restore LocalizableElement
                            "", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);
                    if (result.ToString() == "Yes")
                    {
                    }
                    else
                    {
                        CBHookMouse.Checked = false;
                    }
                }
                Logging.Write(LogType.Info, "Please restart the bot and the client for this to take effect");
            }
        }

        private void buttonReloggerClearData_Click(object sender, EventArgs e)
        {
            ReloggerSettings.AccountName = string.Empty;
            SetupTBRelogUsername.Text = string.Empty;
            ReloggerSettings.AccountPw = string.Empty;
            SetupTBRelogPW.Text = string.Empty;
            ReloggerSettings.AccountAccount = 1;
            //ReloggerSettings._CharacterName = "";
            SetupTBRelogCharacter.Text = "";
            ReloggerSettings.PeriodicLogIn = 30;
            SetupIIRelogLogInAfter.Value = 30;
            ReloggerSettings.PeriodicLogOut = 60;
            SetupIIRelogLogOutAfter.Value = 60;
            ReloggerSettings.PeriodicReloggingEnabled = false;
            SetupCBRelogEnablePeriodicRelog.Checked = false;
            ReloggerSettings.ReloggingEnabled = false;
            SetupCBRelogEnableRelogger.Checked = false;
        }

        private void CBMail_CheckedChanged(object sender, EventArgs e)
        {
            if (!CBDoVendor.Checked && CBMail.Checked)
            {
                MessageBox.Show(
                    "You need to also enable 'To Town on full bags' in the settings (Vendor tab) to enable mailing");
            }
        }

        private void MacroForMail_CheckedChanged(object sender, EventArgs e)
        {
            if (MacroForMail.Checked != LazySettings.MacroForMail && MacroForMail.Checked)
            {
                MessageBox.Show(
                    "You should create a macro: /script SendMailNameEditBox:SetText(\"RECEIVERNAME\") and place it on a bar");
            }
        }

        private void ClientLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }

    internal class Plugin
    {
        public Plugin(string name, string fileName)
        {
            FileName = fileName;
            Name = name;
        }

        public string FileName { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return FileName + "(" + Name + ")";
        }
    }
}