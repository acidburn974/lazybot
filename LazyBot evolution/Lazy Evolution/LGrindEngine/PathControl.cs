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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;
using LazyEvo.LGrindEngine.NpcClasses;
using LazyLib;
using LazyLib.FSM;
using LazyLib.Wow;

#endregion

namespace LazyEvo.LGrindEngine
{
    internal sealed partial class PathControl : Office2007Form
    {
        #region vars

        private const float ImageSize = 256;
        private const float TileSize = 1600/3;
        private readonly Color _colorMe = Color.Green;
        private readonly Hotkey _f7;
        private readonly Hotkey _f8;
        private readonly PathProfile _pathProfile;
        private Font _fontText;
        private Thread _mapThread;
        private Bitmap _offScreenBmp;
        private Graphics _offScreenDc;
        private Thread _pathLoadedThread;
        private int _refreshTime = 85;
        private double _scale = 3;
        private bool _updateFormSize = true;

        #endregion

        #region methods

        #region Delegates

        /// <summary>
        /// </summary>
        public delegate void WriteDelegate(string message);

        #endregion

        private List<DirectedLazyEdge> _nodesToDraw = new List<DirectedLazyEdge>();

        ///<summary>
        ///  Call first
        ///</summary>
        public PathControl(PathProfile pathProfile)
        {
            InitializeComponent();
            DoubleBuffered = true;
            GraphView.BringToFront();
            _pathProfile = pathProfile;
            _pathLoadedThread = new Thread(LoadPath) {IsBackground = true};
            _pathLoadedThread.Start();
            if (LazySettings.SetupUseHotkeys)
            {
                _f7 = new Hotkey();
                _f7.KeyCode = Keys.F7;
                _f7.Windows = false;
                _f7.Pressed += delegate { AddSpot(); };
                try
                {
                    if (!_f7.GetCanRegister(this))
                    {
                        Logging.Write("Cannot register F7 as hotkey");
                    }
                    else
                    {
                        _f7.Register(this);
                    }
                }
                catch
                {
                    Logging.Write("Cannot register F7 as hotkey");
                }
                _f8 = new Hotkey();
                _f8.KeyCode = Keys.F8;
                _f8.Windows = false;
                _f8.Pressed += delegate { AddNode(); };
                try
                {
                    if (!_f8.GetCanRegister(this))
                    {
                        Logging.Write("Cannot register F8 as hotkey");
                    }
                    else
                    {
                        _f8.Register(this);
                    }
                }
                catch
                {
                    Logging.Write("Cannot register F8 as hotkey");
                }
            }
        }

        public int RefreshTime
        {
            get { return _refreshTime; }
            set { _refreshTime = value; }
        }

        private void LoadPath()
        {
            while (true)
            {
                try
                {
                    List<DirectedLazyEdge> directedLazyEdges = _pathProfile.GetGraph.GetEdges();
                    List<Location> locations = _pathProfile.GetGraph.GetNodes();
                    IEnumerable<DirectedLazyEdge> locationQuery;
                    lock (directedLazyEdges)
                    {
                        locationQuery = from u in directedLazyEdges.Where(i => i.Source.DistanceToSelf2D < 200) select u;
                    }
                    List<DirectedLazyEdge> local =
                        (from pL in locationQuery orderby pL.Source.DistanceToSelf2D select pL).Take(
                            DrawEdgesValue.Value).ToList();
                    foreach (Location location in locations)
                    {
                        if (!local.Any(x => x.Source == location || x.Target == location))
                        {
                            local.Add(new DirectedLazyEdge(location, location));
                        }
                    }
                    _nodesToDraw = local;
                    Thread.Sleep(100);
                }
                catch
                {
                }
            }
// ReSharper disable FunctionNeverReturns
        }

// ReSharper restore FunctionNeverReturns

        private void AddSpot()
        {
            if (_selected != null)
                if (!_selected.Spots.Contains(ObjectManager.MyPlayer.Location))
                {
                    _selected.Spots.Add(ObjectManager.MyPlayer.Location);
                    LBSpotCount.Text = _selected.Spots.Count + "";
                }
        }

        private void ReleaseHotKeys()
        {
            if (LazySettings.SetupUseHotkeys)
            {
                if (_f7.Registered)
                    _f7.Unregister();
                if (_f8.Registered)
                    _f8.Unregister();
            }
        }

        public void Start()
        {
            Show();
        }

        private void MapControlLoad(object sender, EventArgs e)
        {
            _fontText = new Font("Verdana", (float) 6.5);
            DoLoad();
            _mapThread = new Thread(UpdateLoop) {IsBackground = true};
            _mapThread.IsBackground = true;
            _mapThread.Start();
            superTabControl1.SelectedTabIndex = 0;
        }

        // Public, kill map
        ///<summary>
        ///  Stops the map
        ///</summary>
        public void StopMap()
        {
            if (_pathLoadedThread != null)
            {
                _pathLoadedThread.Abort();
                _pathLoadedThread = null;
            }
            if (_mapThread != null)
            {
                _mapThread.Abort();
                _mapThread = null;
            }
            _offScreenBmp = null;
            _offScreenDc = null;
            GC.Collect();
        }

        private int OffsetY(float obj, float me)
        {
            return GraphView.Width/2 - Convert.ToInt32((obj - me)*(ImageSize/TileSize)*(float) _scale);
        }

        private int OffsetX(float obj, float me)
        {
            return GraphView.Height/2 - Convert.ToInt32((obj - me)*(ImageSize/TileSize)*(float) _scale);
        }

        // Wow heading to C# heading
        private double ConvertHeading(double heading)
        {
            return heading*-1 - Math.PI/2;
        }

        // Print self
        private void PrintPlayer()
        {
            try
            {
                PrintArrow(_colorMe, GraphView.Width/2, GraphView.Height/2, ObjectManager.MyPlayer.Facing, "", "");
            }
            catch (Exception)
            {
            }
        }

        private void PrintTarget()
        {
            try
            {
                if (ObjectManager.MyPlayer.Target.IsValid)
                {
                    PrintArrow(Color.Red, OffsetY(ObjectManager.MyPlayer.Target.Y, ObjectManager.MyPlayer.Location.Y),
                               OffsetX(ObjectManager.MyPlayer.Target.X, ObjectManager.MyPlayer.Location.X),
                               ObjectManager.MyPlayer.Target.Facing, "", "");
                }
            }
            catch (Exception)
            {
            }
        }

        private void PrintWay()
        {
            Point p;
            Point n;
            float yLoc = ObjectManager.MyPlayer.Location.Y;
            float xLoc = ObjectManager.MyPlayer.Location.X;
            foreach (DirectedLazyEdge lEdge in _nodesToDraw)
            {
                Location node = lEdge.Source;
                n = new Point(OffsetY(lEdge.Source.Y, yLoc),
                              OffsetX(lEdge.Source.X, xLoc));
                p = new Point(OffsetY(lEdge.Target.Y, yLoc),
                              OffsetX(lEdge.Target.X, xLoc));
                Color color = node.NodeType.Equals(NodeType.GroundMount) ? Color.White : Color.Red;
                PrintCircle(color, OffsetY(node.Y, yLoc), OffsetX(node.X, xLoc), "");
                _offScreenDc.DrawLine(new Pen(Color.Blue), n, p);
            }
        }

        private void PrintSpots()
        {
            try
            {
                foreach (SubProfile sub in _pathProfile.GetSubProfiles)
                {
                    IEnumerable<Location> locationQuery = from u in sub.Spots select u;
                    IEnumerable<Location> sortedLocationQuery =
                        (from pL in locationQuery
                         orderby pL.DistanceToSelf2D
                         select pL).Take(20);
                    foreach (Location node in sortedLocationQuery)
                    {
                        try
                        {
                            if (CBShowPullZones.Checked)
                            {
                                PrintTransparentCircle(OffsetY(node.Y, ObjectManager.MyPlayer.Location.Y),
                                                       OffsetX(node.X, ObjectManager.MyPlayer.Location.X), "",
                                                       sub.SpotRoamDistance);
                            }
                        }
                        catch
                        {
                        }
                        Color color = Color.ForestGreen;
                        if (_spotSelectedForEdit == node)
                            color = Color.DarkOrange;
                        PrintCircle(color, OffsetY(node.Y, ObjectManager.MyPlayer.Location.Y),
                                    OffsetX(node.X, ObjectManager.MyPlayer.Location.X), "");
                    }
                }
                if (Engine.Running)
                {
                    try
                    {
                        PrintCircle(Color.GreenYellow,
                                    OffsetY(_pathProfile.GetSubProfile().CurrentSpot.Y,
                                            ObjectManager.MyPlayer.Location.Y),
                                    OffsetX(_pathProfile.GetSubProfile().CurrentSpot.X,
                                            ObjectManager.MyPlayer.Location.X), "");
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        //Used to print all data to map
        private void UpdateLoop()
        {
            while (true)
            {
                if (ObjectManager.Initialized)
                {
                    try
                    {
                        if (_updateFormSize)
                        {
                            _offScreenBmp = new Bitmap(GraphView.Width, GraphView.Height);
                            _updateFormSize = false;
                        }
                        _offScreenDc = Graphics.FromImage(_offScreenBmp);

                        _offScreenDc.FillRectangle(new SolidBrush(Color.Black),
                                                   new Rectangle(0, 0, _offScreenBmp.Width, _offScreenBmp.Height));

                        PrintTarget();
                        PrintPlayer();
                        PrintWay();
                        PrintSpots();
                        PrintSelected();
                        Graphics clientDc = GraphView.CreateGraphics();
                        clientDc.DrawImage(_offScreenBmp, 0, 0);
                    }
                    catch (ThreadAbortException)
                    {
                    }
                    catch (Exception e)
                    {
                        Logging.Debug("Error in radar: " + e);
                    }
                    Thread.Sleep(_refreshTime);
                }
                else
                    Thread.Sleep(1000);
            }
// ReSharper disable FunctionNeverReturns
        }

// ReSharper restore FunctionNeverReturns

        private void PrintSelected()
        {
            foreach (Location lEdge in _selectedList)
            {
                Location node = lEdge;
                Color color = Color.DodgerBlue;
                PrintCircle(color, OffsetY(node.Y, ObjectManager.MyPlayer.Location.Y),
                            OffsetX(node.X, ObjectManager.MyPlayer.Location.X), "");
            }
        }

        // Prints specified color arrow to screen facing heading (0 - right, PI/2 - down, PI - left, 3PI/2 - up)
        private void PrintArrow(Color color, int x, int y, double heading /*radians*/,
                                string topString, string botString)
        {
            try
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
                _offScreenDc.DrawLines(new Pen(color), arrow);

                //Print Top String
                if (topString.Length > 0)
                {
                    _offScreenDc.DrawString(topString, _fontText, new SolidBrush(color),
                                            new PointF(x - topString.Length*(float) 2.2, y - 15));
                }
                //Print Bottom String
                if (botString.Length > 0)
                {
                    _offScreenDc.DrawString(botString, _fontText, new SolidBrush(color),
                                            new PointF(x - botString.Length*2, y + 6));
                }
                const int radius = 3;
                var redBrush = new SolidBrush(color);
                //Do whatever should be printed
                _offScreenDc.DrawEllipse(new Pen(color), x - radius, y - radius, 2*radius, 2*radius);
                _offScreenDc.FillEllipse(redBrush, x - radius, y - radius, 2*radius, 2*radius);
            }
            catch (Exception)
            {
            }
        }

        // Print circle
        private void PrintCircle(Color color, int x, int y, string name)
        {
            try
            {
                const int radius = 3;
                var redBrush = new SolidBrush(color);
                //Do whatever should be printed
                _offScreenDc.DrawEllipse(new Pen(color), x - radius, y - radius, 2*radius, 2*radius);
                _offScreenDc.FillEllipse(redBrush, x - radius, y - radius, 2*radius, 2*radius);

                _offScreenDc.DrawString(name, _fontText, new SolidBrush(color),
                                        new PointF(x - name.Length*2, y - 15));
            }
            catch (Exception)
            {
            }
        }

        private void PrintTransparentCircle(int x, int y, string name, int radius)
        {
            try
            {
                radius = Convert.ToInt32((radius*_scale)/3);
                //Do whatever should be printed
                _offScreenDc.DrawEllipse(new Pen(Color.ForestGreen), x - radius, y - radius, 2*radius, 2*radius);
                Color c2 = Color.FromArgb(50, Color.Green);
                _offScreenDc.FillEllipse(new SolidBrush(c2), x - radius, y - radius, 2*radius, 2*radius);

                _offScreenDc.DrawString(name, _fontText, new SolidBrush(Color.ForestGreen),
                                        new PointF(x - name.Length*2, y - 15));
            }
            catch (Exception)
            {
            }
        }

        public void Log(string message)
        {
            Logging.Write(message);
        }

        #endregion

        #region onEvent

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

        #endregion

        private readonly List<Location> _selectedList = new List<Location>();
        private Rectangle _rect = new Rectangle(-1, -1, -1, -1);
        private SubProfile _selected;
        private Location _spotSelectedForEdit;

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
            ReleaseHotKeys();
            StopMap();
        }

        private void BtnLoadClick(object sender, EventArgs e)
        {
            _pathProfile.Load();
            DoLoad();
        }

        private void DoLoad()
        {
            DisableSubProfiles();
            ListSubProfiles.BeginUpdate();
            ListSubProfiles.Nodes.Clear();
            ListSubProfiles.EndUpdate();
            foreach (SubProfile subProfile in _pathProfile.GetSubProfiles)
            {
                AddNode(subProfile.Name, subProfile);
            }
            int vendor = 0;
            int trainer = 0;
            foreach (VendorsEx npc in _pathProfile.NpcController.Npc)
            {
                if (npc.VendorType == VendorType.Repair)
                {
                    vendor++;
                }
                if (npc.VendorType == VendorType.Train)
                {
                    trainer++;
                }
            }
            LBVendorCount.Text = vendor.ToString();
            LBTrainerCount.Text = trainer.ToString();
            SelectNodeType.SelectedIndex = 0;
        }

        private void DisableSubProfiles()
        {
            TBName.Enabled = false;
            PMinLevel.Enabled = false;
            PMaxLevel.Enabled = false;
            UMaxLevel.Enabled = false;
            UMinLevel.Enabled = false;
            CBSpotOrder.Enabled = false;
            SpotRoamDistance.Enabled = false;
            TBFactionList.Enabled = false;
            BtnAddIgnore.Enabled = false;
            TBIgnore.Enabled = false;
        }

        private void SaveSubProfiles()
        {
            _pathProfile.ClearSubProfile();
            foreach (Node node1 in ListSubProfiles.Nodes)
            {
                if (node1.Tag is SubProfile)
                {
                    _pathProfile.AddSubProfile((SubProfile) node1.Tag);
                }
            }
        }

        private void BtnSaveClick(object sender, EventArgs e)
        {
            SaveSubProfiles();
            _pathProfile.Save();
        }

        private void CbRecordGraphCheckedChanged(object sender, EventArgs e)
        {
            if (CBRecordGraph.Checked)
                _pathProfile.GetGraph.RecordMesh();
            else
                _pathProfile.GetGraph.StopRecordMesh();
        }

        private void BtnAddNodeClick(object sender, EventArgs e)
        {
            AddNode();
        }

        private void AddNode()
        {
            _pathProfile.GetGraph.AddNode(ObjectManager.MyPlayer.Location);
        }

        private void BtnAddClick(object sender, EventArgs e)
        {
            foreach (Node node1 in ListSubProfiles.Nodes)
            {
                if (node1.Tag is SubProfile)
                {
                    if (_selected == node1.Tag)
                    {
                        node1.Text = _selected.Name;
                    }
                }
            }
            _selected = new SubProfile();
            AddNode(_selected.Name, _selected);
            UpdateFields(_selected);
            ListSubProfiles.SelectedIndex = ListSubProfiles.Nodes.Count - 1;
        }

        private void AddNode(string name, SubProfile profile)
        {
            var node = new Node();
            node.Text = name;
            node.Tag = profile;
            AddNode(node);
        }

        private void AddNode(Node node)
        {
            ListSubProfiles.BeginUpdate();
            ListSubProfiles.Nodes.Add(node);
            ListSubProfiles.EndUpdate();
        }

        private void UpdateFields(SubProfile subProfile)
        {
            try
            {
                SaveSubProfiles();
                TBName.Text = subProfile.Name;
                PMaxLevel.Value = subProfile.PlayerMaxLevel;
                PMinLevel.Value = subProfile.PlayerMinLevel;
                UMaxLevel.Value = subProfile.MobMaxLevel;
                UMinLevel.Value = subProfile.MobMinLevel;
                SpotRoamDistance.Value = subProfile.SpotRoamDistance;
                LBFactionCount.Text = subProfile.Factions.Count + "";
                LBSpotCount.Text = subProfile.Spots.Count + "";
                CBSpotOrder.Checked = subProfile.Order;
                TBFactionList.Text = subProfile.Factions.Aggregate(string.Empty,
                                                                   (current, faction) =>
                                                                   current + string.Format("{0} ", faction));
                TBIgnore.Text = _selected.Ignore.Aggregate(string.Empty,
                                                           (current, faction) =>
                                                           current + string.Format("{0}|", faction));
            }
            catch (Exception e)
            {
                Logging.Write("Exception when updatingFields: " + e);
            }
        }

        private void ListSubProfilesNodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            Node node = e.Node;
            if (node.Tag is SubProfile)
            {
                SelectNode(node);
            }
        }

        private void SelectNode(Node node)
        {
            if (_selected != null)
            {
                _selected.Name = TBName.Text;
                _selected.PlayerMaxLevel = PMaxLevel.Value;
                _selected.PlayerMinLevel = PMinLevel.Value;
                _selected.MobMaxLevel = UMaxLevel.Value;
                _selected.MobMinLevel = UMinLevel.Value;
                _selected.Order = CBSpotOrder.Checked;
                _selected.SpotRoamDistance = SpotRoamDistance.Value;
                foreach (Node node1 in ListSubProfiles.Nodes)
                {
                    if (node1.Tag is SubProfile)
                    {
                        if (_selected == node1.Tag)
                        {
                            node1.Text = _selected.Name;
                        }
                    }
                }
            }
            ListSubProfiles.BeginUpdate();
            _selected = (SubProfile) node.Tag;
            UpdateFields(_selected);
            ListSubProfiles.EndUpdate();
            TBName.Enabled = true;
            PMinLevel.Enabled = true;
            PMaxLevel.Enabled = true;
            CBSpotOrder.Enabled = true;
            BtnFaction.Enabled = true;
            BtnAddSpot.Enabled = true;
            UMaxLevel.Enabled = true;
            UMinLevel.Enabled = true;
            SpotRoamDistance.Enabled = true;
            TBFactionList.Enabled = true;
            BtnAddIgnore.Enabled = true;
            TBIgnore.Enabled = true;
        }

        private void BtnFactionClick(object sender, EventArgs e)
        {
            if (ObjectManager.MyPlayer.Target != null)
            {
                if (!_selected.Factions.Contains(ObjectManager.MyPlayer.Target.Faction))
                {
                    _selected.Factions.Add(ObjectManager.MyPlayer.Target.Faction);
                    LBFactionCount.Text = _selected.Factions.Count + "";
                    TBFactionList.Text = _selected.Factions.Aggregate(string.Empty,
                                                                      (current, faction) =>
                                                                      current + string.Format("{0} ", faction));
                }
            }
        }

        private void BtnAddSpotClick(object sender, EventArgs e)
        {
            AddSpot();
        }

        private void TbNameTextChanged(object sender, EventArgs e)
        {
            _selected.Name = TBName.Text;
        }

        private void MinLevelValueChanged(object sender, EventArgs e)
        {
            _selected.PlayerMinLevel = PMinLevel.Value;
        }

        private void MaxLevelValueChanged(object sender, EventArgs e)
        {
            _selected.PlayerMaxLevel = PMaxLevel.Value;
        }

        private void CbSpotOrderCheckedChanged(object sender, EventArgs e)
        {
            _selected.Order = CBSpotOrder.Checked;
        }

        private void GraphViewClick(object sender, EventArgs e)
        {
            try
            {
                Point cursorPosition = PointToClient(Cursor.Position);
                var cursorRect = new Rectangle(cursorPosition.X - 6, cursorPosition.Y - 20, 6, 6);
                IEnumerable<Location> test = _pathProfile.GetGraph.GetNodes();
                float myX = ObjectManager.MyPlayer.Location.X;
                float myY = ObjectManager.MyPlayer.Location.Y;
                foreach (Location node in test)
                {
                    float offsetX = OffsetX(node.X, myX);
                    float offsetY = OffsetY(node.Y, myY);
                    var objRect = new Rectangle((int) offsetY, (int) offsetX, 6, 6);
                    if (Rectangle.Intersect(objRect, cursorRect) != Rectangle.Empty)
                    {
                        lock (_selectedList)
                        {
                            if (!_selectedList.Contains(node))
                            {
                                _selectedList.Add(node);
                            }
                        }
                    }
                }
                //Remove spots
                foreach (SubProfile sub in _pathProfile.GetSubProfiles)
                {
                    IEnumerable<Location> locationQuery = from u in sub.Spots select u;
                    IEnumerable<Location> sortedLocationQuery =
                        (from pL in locationQuery
                         orderby pL.DistanceToSelf2D
                         select pL).Take(20);
                    foreach (Location spot in sortedLocationQuery)
                    {
                        float offsetX = OffsetX(spot.X, myX);
                        float offsetY = OffsetY(spot.Y, myY);
                        var objRect = new Rectangle((int) offsetY, (int) offsetX, 6, 6);
                        if (Rectangle.Intersect(objRect, cursorRect) != Rectangle.Empty)
                        {
                            _spotSelectedForEdit = spot;
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void BtnRemoveNodeClick(object sender, EventArgs e)
        {
            lock (_selectedList)
            {
                foreach (Location location in _selectedList)
                {
                    _pathProfile.GetGraph.RemoveNode(location);
                }
                _selectedList.Clear();
            }
        }

        private void BtnNewClick(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result.ToString() == "Yes")
            {
                _pathProfile.New();
                ListSubProfiles.BeginUpdate();
                ListSubProfiles.Nodes.Clear();
                ListSubProfiles.EndUpdate();
                TBName.Text = "";
                LBFactionCount.Text = "0";
                LBSpotCount.Text = "0";
                LBVendorCount.Text = "0";
                LBTrainerCount.Text = "0";
                TBName.Enabled = false;
                PMinLevel.Enabled = false;
                PMaxLevel.Enabled = false;
                CBSpotOrder.Enabled = false;
                BtnFaction.Enabled = false;
                BtnAddSpot.Enabled = false;
            }
        }

        private void BtnAddNodeNoClick(object sender, EventArgs e)
        {
            _pathProfile.GetGraph.AddNodeNoConnection(ObjectManager.MyPlayer.Location);
        }

        private void BtnAddNodeConnectionClick(object sender, EventArgs e)
        {
            lock (_selectedList)
            {
                var connected = new List<DirectedLazyEdge>();
                for (int i = 0; i < _selectedList.Count; i++)
                {
                    Location loc = _selectedList[i];
                    foreach (Location location in _selectedList.Where(l => l != loc))
                    {
                        Location location1 = location;
                        if (
                            !connected.Any(
                                k =>
                                (k.Source == loc && k.Target == location1) || (k.Source == location1 && k.Target == loc)))
                        {
                            _pathProfile.GetGraph.AddConnection(loc, location);
                            connected.Add(new DirectedLazyEdge(loc, location));
                        }
                    }
                }
                _selectedList.Clear();
            }
        }

        private void NodeDistanceValueChanged(object sender, EventArgs e)
        {
            _pathProfile.GetGraph.SetNodeDistance(INodeDistance.Value);
        }

        private void BtnAddRepairClick(object sender, EventArgs e)
        {
            try
            {
                if (ObjectManager.MyPlayer.Target != null && _pathProfile != null)
                {
                    _pathProfile.NpcController.AddNpc(new VendorsEx(VendorType.Repair,
                                                                    ObjectManager.MyPlayer.Target.Name,
                                                                    ObjectManager.MyPlayer.Target.Location,
                                                                    ObjectManager.MyPlayer.Target.Entry));
                    LBVendorCount.Text =
                        _pathProfile.NpcController.Npc.Where(npc => npc.VendorType == VendorType.Repair).ToList().Count.
                            ToString();
                }
            }
            catch (Exception d)
            {
                Logging.Write("Exception when BtnAddRepair_Click: " + d);
            }
        }

        private void BtnAddTrainerClick(object sender, EventArgs e)
        {
            if (ObjectManager.MyPlayer.Target != null)
            {
                PUnit tar = ObjectManager.MyPlayer.Target;
                var tr = new TrainerDialog();
                tr.ShowDialog();
                if (tr.Ok)
                {
                    _pathProfile.NpcController.AddNpc(new VendorsEx(VendorType.Train, tar.Name, tar.Location,
                                                                    (TrainClass)
                                                                    Enum.Parse(typeof (TrainClass), tr.Class, true),
                                                                    tar.Entry));
                    LBTrainerCount.Text =
                        _pathProfile.NpcController.Npc.Where(npc => npc.VendorType == VendorType.Train).ToList().Count.
                            ToString();
                }
                UpdateFields(_selected);
            }
        }

        private void SelectEngineSelectedIndexChanged(object sender, EventArgs e)
        {
            switch (SelectNodeType.SelectedItem.ToString())
            {
                case "Normal":
                    _pathProfile.GetGraph.SetNodeType(NodeType.Normal);
                    break;
                case "Ground mount":
                    _pathProfile.GetGraph.SetNodeType(NodeType.GroundMount);
                    break;
            }
        }

        private void CbTopMostCheckedChanged(object sender, EventArgs e)
        {
            TopMost = CBTopMost.Checked;
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            if (ListSubProfiles.SelectedNode != null)
            {
                Node toRemove = null;
                foreach (Node node1 in ListSubProfiles.Nodes)
                {
                    if (node1.Tag.Equals(ListSubProfiles.SelectedNode.Tag))
                    {
                        toRemove = node1;
                    }
                }
                if (toRemove != null)
                {
                    ListSubProfiles.Nodes.Remove(toRemove);
                    _selected = new SubProfile();
                    UpdateFields(_selected);
                    DisableSubProfiles();
                }
            }
        }

        private void ListSubProfilesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListSubProfiles.SelectedNode != null)
            {
                if (ListSubProfiles.SelectedNode.Tag is SubProfile)
                {
                    SelectNode(ListSubProfiles.SelectedNode);
                }
            }
        }

        private void UMinLevelValueChanged(object sender, EventArgs e)
        {
            _selected.MobMinLevel = UMinLevel.Value;
        }

        private void UMaxLevelValueChanged(object sender, EventArgs e)
        {
            _selected.MobMaxLevel = UMaxLevel.Value;
        }

        private void SpotRoamDistanceValueChanged(object sender, EventArgs e)
        {
            _selected.SpotRoamDistance = SpotRoamDistance.Value;
        }

        private void GraphViewMouseClick(object sender, MouseEventArgs e)
        {
            _rect = new Rectangle(-1, -1, -1, -1);
        }

        private void GraphViewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_rect.X == -1 && _rect.Y == -1)
                {
                    // Logging.Write("Reset");
                    _rect = new Rectangle(e.X, e.Y, 0, 0);
                }
                var testRec = new Rectangle(_rect.Left, _rect.Top, e.X - _rect.Left, e.Y - _rect.Top);
                //  Logging.Write(e.X + " " + e.Y + " " + " " + _rect.Left + " " + _rect.Top);
                if (testRec.Width < 0 && testRec.Height < 0)
                {
                    _rect = new Rectangle(e.X, e.Y, _rect.Left, _rect.Top);
                }
                else if (testRec.Width < 0 && testRec.Height > 0)
                {
                    _rect = new Rectangle(e.X - _rect.Left, e.Y, Math.Abs(_rect.Left), e.Y - _rect.Top);
                }
                else
                {
                    _rect = new Rectangle(_rect.Left, _rect.Top, Math.Abs(e.X - _rect.Left), Math.Abs(e.Y - _rect.Top));
                }
                //Logging.Write(_rect.X + " " + _rect.Y + " " + " " + _rect.Width + " " + _rect.Height);
                IEnumerable<Location> test = _pathProfile.GetGraph.GetNodes();
                foreach (Location node in test)
                {
                    float offsetX = OffsetX(node.X, ObjectManager.MyPlayer.Location.X);
                    float offsetY = OffsetY(node.Y, ObjectManager.MyPlayer.Location.Y);
                    var objRect = new Rectangle((int) offsetY - 6, (int) offsetX - 6, 6, 6);
                    if (Rectangle.Intersect(objRect, _rect) != Rectangle.Empty)
                    {
                        lock (_selectedList)
                        {
                            if (!_selectedList.Contains(node))
                            {
                                _selectedList.Add(node);
                            }
                        }
                        //Logging.Write("Selected: " + node.ToString());
                    }
                }
            }
        }

        private void GraphViewMouseUp(object sender, MouseEventArgs e)
        {
            _rect = new Rectangle(-1, -1, -1, -1);
        }

        private void PathControlMouseUp(object sender, MouseEventArgs e)
        {
            _rect = new Rectangle(0, 0, 0, 0);
        }

        private void OtherAddSpotClick(object sender, EventArgs e)
        {
            AddSpot();
        }

        private void BtnRemoveSpotClick(object sender, EventArgs e)
        {
            if (_spotSelectedForEdit != null)
            {
                Location remove = null;
                foreach (SubProfile sub in _pathProfile.GetSubProfiles)
                {
                    IEnumerable<Location> locationQuery = from u in sub.Spots select u;
                    IEnumerable<Location> sortedLocationQuery =
                        (from pL in locationQuery
                         orderby pL.DistanceToSelf2D
                         select pL).Take(50);
                    foreach (Location spot in sortedLocationQuery)
                    {
                        if (spot == _spotSelectedForEdit)
                        {
                            remove = spot;
                            break;
                        }
                    }
                    if (remove != null)
                    {
                        sub.Spots.Remove(remove);
                        break;
                    }
                }
            }
        }

        private void BtnClearSelectionClick(object sender, EventArgs e)
        {
            _selectedList.Clear();
        }

        private void TbIgnoreListTextChanged(object sender, EventArgs e)
        {
            try
            {
                string temp = TBFactionList.Text;
                string[] split = temp.Split(new[] {' '});
                _selected.Factions.Clear();
                _selected.Factions.AddRange(from s in split where s != "" && s != " " select Convert.ToUInt32(s));
                LBFactionCount.Text = _selected.Factions.Count + "";
            }
            catch
            {
            }
        }

        private void BtnAddIgnoreClick(object sender, EventArgs e)
        {
            if (ObjectManager.MyPlayer.Target != null)
            {
                if (!_selected.Ignore.Contains(ObjectManager.MyPlayer.Target.Name))
                {
                    _selected.Ignore.Add(ObjectManager.MyPlayer.Target.Name);
                    TBIgnore.Text = _selected.Ignore.Aggregate(string.Empty,
                                                               (current, faction) =>
                                                               current + string.Format("{0}|", faction));
                    LBIgnoreCount.Text = _selected.Ignore.Count + "";
                }
            }
        }

        private void TbIgnoreTextChanged(object sender, EventArgs e)
        {
            try
            {
                string temp = TBIgnore.Text;
                string[] split = temp.Split(new[] {'|'});
                _selected.Ignore.Clear();
                _selected.Ignore.AddRange(from s in split where s != "" && s != " " select s);
                LBIgnoreCount.Text = _selected.Ignore.Count + "";
            }
            catch
            {
            }
        }
    }
}