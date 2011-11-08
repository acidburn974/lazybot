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
using DevComponents.DotNetBar;
using LazyEvo.Debug;
using LazyLib;
using LazyLib.Helpers;
using LazyLib.Wow;

namespace LazyEvo.Forms
{
    internal partial class Debug : Office2007Form
    {
        private Frame _item;
        private ListViewItem[] _wowPlayerListViewItemArray;
        private ListViewItem[] _wowTargetListViewItemArray;

        public Debug()
        {
            InitializeComponent();
            LoadListView();
        }

        private void LoadListView()
        {
            // Set the view to show details.
            listView1.View = View.Details;
            listView2.View = View.Details;

            // Add columns for player
            listView1.Columns.Add("Name", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("Value", 200, HorizontalAlignment.Left);

            // add columns for target
            listView2.Columns.Add("Name", 100, HorizontalAlignment.Center);
            listView2.Columns.Add("Value", 200, HorizontalAlignment.Left);

            InitializePlayerListViewItem();
            InitializeTargetListViewItem();
        }

        private void InitializePlayerListViewItem()
        {
            NameValuePair[] pairs = GetPlayerMeNameValuePairs(ObjectManager.MyPlayer);

            _wowPlayerListViewItemArray = new ListViewItem[pairs.Length];

            for (int i = 0; i < pairs.Length; i++)
            {
                var temp = new ListViewItem(pairs[i].Name);
                temp.SubItems.Add(pairs[i].Value);
                _wowPlayerListViewItemArray[i] = temp;
            }

            // Add the items to the ListView.
            listView1.Items.AddRange(_wowPlayerListViewItemArray);
        }

        private void InitializeTargetListViewItem()
        {
            PUnit wu = ObjectManager.MyPlayer.Target;
            var pairs = new NameValuePair[] {};
            if (wu != null && wu.BaseAddress != 0)
            {
                if (wu.Type == 3)
                {
                    pairs = GetTargetNameValuePairs(wu);
                }
                else if (wu.Type == 4)
                {
                    var wpm = (PPlayer) wu;
                    pairs = GetPlayerNameValuePairs(wpm);
                }
            }
            _wowTargetListViewItemArray = new ListViewItem[pairs.Length];

            for (int i = 0; i < pairs.Length; i++)
            {
                var temp = new ListViewItem(pairs[i].Name);
                temp.SubItems.Add(pairs[i].Value);
                _wowTargetListViewItemArray[i] = temp;
            }
            listView2.Items.AddRange(_wowTargetListViewItemArray);
        }

        public void UpdatePlayerTabValues()
        {
            NameValuePair[] playerMePairs = GetPlayerMeNameValuePairs(ObjectManager.MyPlayer);
            for (int j = 0; j < _wowPlayerListViewItemArray.Length; j++)
            {
                _wowPlayerListViewItemArray[j].SubItems.RemoveAt(1);
                _wowPlayerListViewItemArray[j].SubItems.Add(playerMePairs[j].Value);
            }
        }

        public void UpdateTargetTabValues()
        {
            PUnit wu = ObjectManager.MyPlayer.Target;
            if (wu != null)
            {
                if (_wowTargetListViewItemArray != null && _wowTargetListViewItemArray.Length < 2)
                {
                    InitializeTargetListViewItem();
                }
                else
                {
                    var targetPairs = new NameValuePair[] {};
                    if (wu.BaseAddress != 0)
                    {
                        if (wu.Type == 3)
                        {
                            targetPairs = GetTargetNameValuePairs(wu);
                        }
                        else if (wu.Type == 4)
                        {
                            var wpm = (PPlayer) wu;
                            targetPairs = GetPlayerNameValuePairs(wpm);
                        }
                        if (_wowTargetListViewItemArray != null)
                            for (int j = 0; j < _wowTargetListViewItemArray.Length; j++)
                            {
                                try
                                {
                                    _wowTargetListViewItemArray[j].SubItems.RemoveAt(1);
                                    _wowTargetListViewItemArray[j].SubItems.Add(targetPairs[j].Value);
                                }
                                catch
                                {
                                }
                            }
                    }
                }
            }
            else
            {
                // erase the view when the target changes
                _wowTargetListViewItemArray = new ListViewItem[0];
                listView2.Items.Clear();
            }
        }

        private static NameValuePair[] GetTargetNameValuePairs(PUnit target)
        {
            var wuUtils = new PUnitUtils(target.BaseAddress);
            return wuUtils.GetNameValuePairs().ToArray();
        }

        private static NameValuePair[] GetPlayerNameValuePairs(PPlayer me)
        {
            var wpUtils = new PPlayerUtils(me.BaseAddress);
            return wpUtils.GetNameValuePairs().ToArray();
        }

        private static NameValuePair[] GetPlayerMeNameValuePairs(PPlayerSelf me)
        {
            var wpmUtils = new PPlayerSelfUtils(me.BaseAddress);
            return wpmUtils.GetNameValuePairs().ToArray();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ObjectManager.InGame)
            {
                // update current information
                UpdatePlayerTabValues();
                UpdateTargetTabValues();
            }
        }

        private void DebugBtnFindUiClick(object sender, EventArgs e)
        {
            WriteLine("Going to try and find: " + DebugTBUIName.Text);
            DebugTBLog.Clear();
            if (DebugTBUIName.Text != "")
            {
                try
                {
                    _item = InterfaceHelper.GetFrameByName(DebugTBUIName.Text);
                    if (_item != null)
                    {
                        WriteLine("Found the item, dumping info");
                        WriteLine("Visible: " + _item.IsVisible);
                        WriteLine("Text: " + _item.GetText);
                        foreach (Frame child in _item.GetChilds)
                        {
                            WriteLine("Name: " + child.GetName);
                            WriteLine("Visible: " + child.IsVisible);
                            WriteLine("Text: " + child.GetText);
                        }
                    }
                    else
                    {
                        WriteLine("Could not find the item");
                    }
                }
                catch (Exception d)
                {
                    WriteLine("Error when trying to log interface item: " + d);
                }
            }
        }

        private void WriteLine(string text)
        {
            DebugTBLog.AppendText(text + Environment.NewLine);
        }

        private void DebugBtnClickClick(object sender, EventArgs e)
        {
            if (_item != null)
            {
                _item.LeftClick();
            }
        }

        private void UiBtnDumpClick(object sender, EventArgs e)
        {
            MessageBox.Show("This will make LazyBot freeze until done");
            foreach (Frame frame in InterfaceHelper.GetFrames)
            {
                Logging.Debug("Name: " + frame.GetName + " Visible: " + frame.IsVisible);
                foreach (Frame child in frame.GetChilds)
                {
                    Logging.Debug("     Child: Name: " + child.GetName + " Visible: " + child.IsVisible);
                }
            }
        }

        private void Debug_Load(object sender, EventArgs e)
        {
            superTabControl1.SelectedTab = superTabItem1;
        }
    }
}