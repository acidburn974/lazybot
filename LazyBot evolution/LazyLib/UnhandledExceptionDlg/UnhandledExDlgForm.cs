
﻿/*
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
using System.Reflection;
using System.Windows.Forms;

#endregion

namespace LazyLib.Dialogs.UnhandledExceptionDlg
{
    /// <summary>
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public partial class UnhandledExDlgForm : Form
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "UnhandledExDlgForm" /> class.
        /// </summary>
        public UnhandledExDlgForm()
        {
            InitializeComponent();
        }

        private void UnhandledExDlgFormLoad(object sender, EventArgs e)
        {
            buttonNotSend.Focus();
            labelExceptionDate.Text = String.Format(labelExceptionDate.Text, DateTime.Now);
        }
    }
}