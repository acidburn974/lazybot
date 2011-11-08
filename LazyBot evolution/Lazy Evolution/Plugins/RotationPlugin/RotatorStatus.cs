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
using LazyEvo.Forms.Helpers;
using LazyLib;

namespace LazyEvo.Plugins.RotationPlugin
{
    public partial class RotatorStatus : Form
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        public RotatorStatus()
        {
            InitializeComponent();
            Geometry.GeometryFromString(GeomertrySettings.RotatorStatus, this);
        }

        public void UpdateStatus(bool running)
        {
            if (InvokeRequired)
            {
                Invoke(
                    new MethodInvoker(
                        delegate { UpdateStatus(running); }));
            }
            else
            {
                if (running)
                {
                    labelX1.Text = "Running";
                }
                else
                {
                    labelX1.Text = "Stopped";
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if ((int) m.Result == HTCLIENT)
                        m.Result = (IntPtr) HTCAPTION;
                    return;
            }
            base.WndProc(ref m);
        }

        private void RotatorStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            GeomertrySettings.RotatorStatus = Geometry.GeometryToString(this);
        }
    }
}