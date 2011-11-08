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

namespace LazyEvo.LGrindEngine.NpcClasses
{
    internal partial class TrainerDialog : Office2007Form
    {
        public string Class;
        public bool Ok;

        public TrainerDialog()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            Class = CClass.SelectedItem.ToString();
            Ok = true;
            Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            Ok = false;
            Close();
        }
    }
}