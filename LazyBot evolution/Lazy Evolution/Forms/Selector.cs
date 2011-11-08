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
using System.Diagnostics;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using LazyEvo.Forms.Helpers;
using LazyLib;
using LazyLib.Helpers;
using LazyLib.Wow;

#endregion

namespace LazyEvo.Forms
{
    internal partial class Selector : Office2007Form
    {
        private Process[] _wowProc = Process.GetProcessesByName("Wow");

        public Selector()
        {
            InitializeComponent();
            Geometry.GeometryFromString(GeomertrySettings.ProcessSelector, this);
        }

        private void BtnAttach_Click(object sender, EventArgs e)
        {
            if (SelectProcess.SelectedItem != null)
            {
                if (SelectProcess.SelectedItem.ToString() != "No game")
                {
                    Program.AttachTo = _wowProc[SelectProcess.SelectedIndex].Id;
                }
                else
                {
                    Program.AttachTo = -1;
                }
                Close();
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            RefreshProcess();
        }

        private void RefreshProcess()
        {
            SelectProcess.Items.Clear();
            _wowProc = Process.GetProcessesByName("Wow");
            foreach (Process proc in _wowProc)
            {
                GetName(proc);
            }
            if (SelectProcess.Items.Count == 0)
                SelectProcess.Items.Add("No game");
            SelectProcess.SelectedIndex = 0;
        }

        private void GetName(Process proc)
        {
            if (Memory.OpenProcess(proc.Id))
            {
                string name = "Not ingame";
                try
                {
                    if (Memory.Read<byte>(Memory.BaseAddress + (uint) PublicPointers.InGame.InGame) == 1)
                    {
                        try
                        {
                            name = Memory.ReadUtf8(Memory.BaseAddress + (uint) PublicPointers.Globals.PlayerName, 256);
                        }
                        catch
                        {
                        }
                    }
                }
                catch
                {
                }
                SelectProcess.Items.Add("[" + proc.Id + "]" + " - " + name);
            }
        }

        private void Selector_Load(object sender, EventArgs e)
        {
            RefreshProcess();
        }

        private void Selector_FormClosing(object sender, FormClosingEventArgs e)
        {
            GeomertrySettings.ProcessSelector = Geometry.GeometryToString(this);
        }
    }
}