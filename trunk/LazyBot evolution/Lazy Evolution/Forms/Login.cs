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
using System;
using System.Diagnostics;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using LazyLib;

namespace LazyEvo.Forms
{
    internal partial class Login : Office2007Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void ButtonX1Click(object sender, EventArgs e)
        {
            LazySettings.UserName = SetupTBUserName.Text;
            LazySettings.Password = SetupTBPassword.Text;
            LazySettings.SaveSettings();
            Close();
        }

        private void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.mmo-lazybot.com/forum");
        }
    }
}