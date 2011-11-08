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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using LazyLib;
using LazyLib.Wow;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Serialization;

namespace LazyEvo.LGrindEngine
{
    public class QuickGraph
    {
        private int _distance = 6;
        private AdjacencyGraph<Location, DirectedLazyEdge> _graph;
        private NodeType _nodeType = NodeType.Normal;
        private Thread _record;

        public QuickGraph()
        {
            New();
        }

        public void New()
        {
            _graph = new AdjacencyGraph<Location, DirectedLazyEdge>();
        }

        public void AddNode(Location loc)
        {
            loc.NodeType = _nodeType;
            _graph.AddVertex(loc);
            ConnectNode(loc);
        }

        public void ConnectNode(Location toAdd)
        {
            foreach (Location n in _graph.Vertices)
            {
                if (n != toAdd)
                {
                    if (toAdd.DistanceFrom(n) <= _distance + 2)
                    {
                        AddEdge(toAdd, n);
                    }
                }
            }
        }

        public List<DirectedLazyEdge> GetEdges()
        {
            var re = new List<DirectedLazyEdge>();
            re = _graph.Edges.ToList();
            return re;
        }

        public List<Location> GetNodes()
        {
            var re = new List<Location>();
            re = _graph.Vertices.ToList();
            return re;
        }

        public void AddEdge(Location source, Location target)
        {
            if (_graph.ContainsVertex(source) && _graph.ContainsVertex(target))
            {
                var edge = new DirectedLazyEdge(source, target);
                _graph.AddEdge(edge);
                edge = new DirectedLazyEdge(target, source);
                _graph.AddEdge(edge);
            }
            else
            {
                Logging.Write("Vertex: " + source + " : " + target);
            }
        }

        public List<Location> FindPath(Location sourced, Location targetd)
        {
            try
            {
                Func<DirectedLazyEdge, double> cityDistances = GetDistance;
                // a delegate that gives the distance between cities
                Location so = GetClosest(sourced);
                Location tar = GetClosest(targetd);
                TryFunc<Location, IEnumerable<DirectedLazyEdge>> tryGetPath = null;
                try
                {
                    tryGetPath = _graph.ShortestPathsDijkstra(cityDistances, so);
                }
                catch (Exception e)
                {
                    Logging.Write("Could not create path: " + e);
                }
                var loc = new List<Location>();
                if (tryGetPath != null)
                {
                    IEnumerable<DirectedLazyEdge> path;
                    if (tryGetPath(tar, out path))
                        loc.AddRange(path.Select(e => e.Source));
                }
                return loc;
            }
            catch (ArgumentException)
            {
                Logging.Write(LogType.Warning, "Could not create path, make sure you got a path loaded and it is valid");
                return new List<Location>();
            }
        }

        public void SaveGraph(string file)
        {
            using (FileStream stream = File.Open(file, FileMode.OpenOrCreate, FileAccess.Write))
                _graph.SerializeToBinary(stream);
        }

        public void LoadGraph(string file)
        {
            FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read);
            var formatter = new BinaryFormatter();
            _graph = (AdjacencyGraph<Location, DirectedLazyEdge>) formatter.Deserialize(stream);
        }

        public static double GetDistance(DirectedLazyEdge edge)
        {
            return edge.Source.GetDistanceTo(edge.Target);
        }

        public Location GetClosest(Location loc)
        {
            double closest = double.MaxValue;
            Location re = null;
            foreach (Location n in _graph.Vertices)
            {
                if (n.DistanceFromXY(loc) < closest)
                {
                    closest = n.DistanceFromXY(loc);
                    re = n;
                }
            }
            return re;
        }

        public void RemoveNode(Location loc)
        {
            lock (_graph)
            {
                List<DirectedLazyEdge> toRemove =
                    _graph.Edges.Where(lazyEdge => lazyEdge.Source.Equals(loc) || lazyEdge.Target.Equals(loc)).ToList();
                foreach (DirectedLazyEdge directedLazyEdge in toRemove)
                {
                    _graph.RemoveEdge(directedLazyEdge);
                }

                foreach (Location node in _graph.Vertices)
                {
                    if (node.Equals(loc))
                    {
                        _graph.RemoveVertex(node);
                        break;
                    }
                }
            }
        }

        public void RecordMesh()
        {
            if (_record == null || !_record.IsAlive)
            {
                _record = new Thread(DoRecording);
                _record.IsBackground = true;
                _record.Start();
            }
        }

        public void StopRecordMesh()
        {
            if (_record != null && _record.IsAlive)
            {
                _record.Abort();
                _record = null;
            }
        }

        private void DoRecording()
        {
            Location old = ObjectManager.MyPlayer.Location;
            AddNode(old);
            Location check;
            bool toClose = false;
            //Waypoint Distances
            while (true)
            {
                try
                {
                    if (ObjectManager.Initialized)
                    {
                        check = ObjectManager.MyPlayer.Location;
                        if (check.DistanceFrom(old) >= _distance)
                        {
                            foreach (Location n in _graph.Vertices)
                            {
                                if (check.DistanceFrom(n) < 4 - 1)
                                {
                                    toClose = true;
                                }
                            }
                            if (!toClose)
                            {
                                AddNode(check);
                                old = check;
                            }
                            toClose = false;
                        }
                        Thread.Sleep(100);
                    }
                }
                catch (Exception e)
                {
                    Logging.Debug(e.ToString());
                }
            }
        }

        public void SetNodeType(NodeType type)
        {
            _nodeType = type;
        }

        public void SetNodeDistance(int distance)
        {
            _distance = distance;
        }

        public void AddNodeNoConnection(Location toAdd)
        {
            toAdd.NodeType = _nodeType;
            _graph.AddVertex(toAdd);
        }

        public void AddConnection(Location a, Location b)
        {
            try
            {
                AddEdge(a, b);
            }
            catch
            {
            }
        }
    }
}