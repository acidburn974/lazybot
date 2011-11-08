
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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using LazyLib.Helpers;
using LazyLib.LazyRadar.Drawer;
using LazyLib.Wow;

#endregion

namespace LazyLib.LazyRadar
{
    public partial class RadarForm : Office2007Form
    {
        #region vars

        private const string SettingsName = "\\Settings\\lazy_radars.ini";
        private static string OurDirectory;
        private readonly Color _colorMe = Color.Pink;
        private readonly Dictionary<string, bool> _itemsShouldDraw = new Dictionary<string, bool>();
        private readonly List<IDrawItem> _itemsToDraw = new List<IDrawItem>();
        private readonly object _locker = new object();
        private readonly List<IMouseClick> _mouseFunctions = new List<IMouseClick>();
        private readonly IniManager pIniManager;
        public Graphics ScreenDc;
        private Font _fontText;
        private Bitmap _offScreenBmp;
        private double _scale = 1;
        private bool _updateFormSize = true;

        #endregion

        #region methods

        public RadarForm()
        {
            InitializeComponent();
            var executableFileInfo = new FileInfo(Application.ExecutablePath);
            string executableDirectoryName = executableFileInfo.DirectoryName;
            OurDirectory = executableDirectoryName;
            pIniManager = new IniManager(OurDirectory + SettingsName);
        }

        public void Start()
        {
            Show();
        }

        private void MapControlLoad(object sender, EventArgs e)
        {
            _fontText = new Font("Verdana", (float) 6.5);
            Log("Radar starting");
            AddDrawItem(new DrawEnemies());
            AddDrawItem(new DrawFriends());
            AddDrawItem(new DrawObjects());
            AddDrawItem(new DrawUnits());
        }

        public void AddDrawItem(IDrawItem item)
        {
            _itemsToDraw.Add(item);
            var cb = new CheckBoxItem(item.SettingName());
            _itemsShouldDraw.Add(item.SettingName(), false);
            cb.Tag = item.SettingName();
            cb.Text = item.CheckBoxName();
            if (pIniManager.GetBoolean("Radar", item.SettingName(), false))
            {
                cb.Checked = true;
                _itemsShouldDraw[item.SettingName()] = true;
            }
            cb.Click += DrawItemClick;
            ControlSettings.Items.Add(cb);
        }

        public void AddMonitorMouseClick(IMouseClick click)
        {
            _mouseFunctions.Add(click);
        }

        private void DrawItemClick(object sender, EventArgs e)
        {
            var cb = (CheckBoxItem) sender;
            lock (_locker)
            {
                _itemsShouldDraw[(string) cb.Tag] = cb.Checked;
                pIniManager.IniWriteValue("Radar", (string) cb.Tag, cb.Checked.ToString());
            }
        }

        public void StopMap()
        {
            _offScreenBmp = null;
            ScreenDc = null;
        }

        public int OffsetY(float obj, float me)
        {
            return Width/2 - Convert.ToInt32((obj - me)*(float) _scale);
        }

        public int OffsetX(float obj, float me)
        {
            return Height/2 - Convert.ToInt32((obj - me)*(float) _scale);
        }

        private static double ConvertHeading(double heading)
        {
            return heading*-1 - Math.PI/2;
        }

        private void PrintPlayer()
        {
            PrintArrow(_colorMe, Width/2, Height/2, ObjectManager.MyPlayer.Facing, "", "");
           // Logging.Write("PL: " + ObjectManager.MyPlayer.Facing);
        }

        private void MapTimerTick(object sender, EventArgs eventArgs)
        {
            if (ObjectManager.Initialized)
            {
                try
                {
                    if (_updateFormSize)
                    {
                        _offScreenBmp = new Bitmap(Width, Height);
                        _updateFormSize = false;
                    }
                    ScreenDc = Graphics.FromImage(_offScreenBmp);
                    ScreenDc.FillRectangle(new SolidBrush(Color.LightGray),
                                           new Rectangle(0, 0, _offScreenBmp.Width, _offScreenBmp.Height));
                    foreach (IDrawItem drawItem in _itemsToDraw)
                    {
                        lock (_locker)
                        {
                            if (_itemsShouldDraw[drawItem.SettingName()])
                                drawItem.Draw(this);
                        }
                    }
                    PrintPlayer();
                    Graphics clientDc = CreateGraphics();
                    ScreenDc.SmoothingMode = SmoothingMode.HighQuality;
                    clientDc.DrawImage(_offScreenBmp, 0, 0);
                    ScreenDc.Dispose();
                }
                catch (Exception e)
                {
                    Logging.Write("Error in radar: " + e);
                }
            }
        }

        public void PrintArrow(Color color, int x, int y, double heading, string topString, string botString)
        {
            heading = ConvertHeading(heading);
            //Define arrow/rotation
            var arrow = new Point[5];
            arrow[0] = new Point(Convert.ToInt32(x + Math.Cos(heading)*10),
                                 Convert.ToInt32(y + Math.Sin(heading)*10));

            arrow[1] = new Point(Convert.ToInt32(x + Math.Cos(heading + Math.PI*2/3)*2),
                                 Convert.ToInt32(y + Math.Sin(heading + Math.PI*2/3)*2));
            arrow[2] = new Point(x, y);
            arrow[3] = new Point(Convert.ToInt32(x + Math.Cos(heading + Math.PI*2/-3)*2),
                                 Convert.ToInt32(y + Math.Sin(heading + Math.PI*2/-3)*2));
            arrow[4] = new Point(Convert.ToInt32(x + Math.Cos(heading)*10),
                                 Convert.ToInt32(y + Math.Sin(heading)*10));
            //Do whatever should be printed
            //Print arrow
            ScreenDc.DrawLines(new Pen(color), arrow);
            //Print Top String
            if (topString.Length > 0)
            {
                ScreenDc.DrawString(topString, _fontText, new SolidBrush(color),
                                    new PointF(x - topString.Length*(float) 2.2, y - 15));
            }
            //Print Bottom String
            if (botString.Length > 0)
            {
                ScreenDc.DrawString(botString, _fontText, new SolidBrush(color),
                                    new PointF(x - botString.Length*2, y + 6));
            }
            const int radius = 3;
            var redBrush = new SolidBrush(color);
            //Do whatever should be printed
            ScreenDc.DrawEllipse(new Pen(color), x - radius, y - radius, 2*radius, 2*radius);
            ScreenDc.FillEllipse(redBrush, x - radius, y - radius, 2*radius, 2*radius);
            redBrush.Dispose();
        }

        public void PrintCircle(Color color, int x, int y, string name)
        {
            const int radius = 3;
            var redBrush = new SolidBrush(color);
            //Do whatever should be printed
            ScreenDc.DrawEllipse(new Pen(color), x - radius, y - radius, 2*radius, 2*radius);
            ScreenDc.FillEllipse(redBrush, x - radius, y - radius, 2*radius, 2*radius);

            ScreenDc.DrawString(name, _fontText, new SolidBrush(color),
                                new PointF(x - name.Length*2, y - 15));
            redBrush.Dispose();
        }

        public void Log(string message)
        {
            Logging.Write(message);
        }

        #endregion

        #region onEvent

        private void MapMouseClick(object sender, MouseEventArgs e)
        {
            foreach (IMouseClick mouseFunction in _mouseFunctions)
            {
                mouseFunction.Click(this, e);
            }
        }

        private void MapMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                //zoom in
                if (_scale > 2)
                    _scale += 0.6;
                else
                    _scale += 0.3;
            }
            else
            {
                //zoom out
                if (_scale > 2)
                {
                    _scale -= 0.6;
                }
                else if (_scale > 0.3)
                {
                    _scale -= 0.3;
                }
            }
        }

        private void MapControlResizeBegin(object sender, EventArgs e)
        {
            _updateFormSize = true;
        }

        private void MapControlResize(object sender, EventArgs e)
        {
            _updateFormSize = true;
        }

        private void MapControlFormClosed(object sender, FormClosedEventArgs e)
        {
            StopMap();
        }

        #endregion

        private void CbTopMostClick(object sender, EventArgs e)
        {
            TopMost = CBTopMost.Checked;
        }
    }
}