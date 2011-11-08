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
using LazyLib;
using LazyLib.Wow;

namespace LazyEvo.LGrindEngine
{
    internal class PlayerData
    {
        private bool _changed;
        private Dictionary<string, string> _dic = new Dictionary<string, string>();

        public void Save()
        {
            lock (this)
            {
                if (!_changed)
                    return;
                if (!Directory.Exists(GrindingEngine.OurDirectory + "\\PlayerData\\"))
                    Directory.CreateDirectory(GrindingEngine.OurDirectory + "\\PlayerData\\");
                string filename = GrindingEngine.OurDirectory + "\\PlayerData\\" + ObjectManager.MyPlayer.Name + ".txt";
                try
                {
                    TextWriter s = File.CreateText(filename);
                    foreach (string key in _dic.Keys)
                    {
                        string val = _dic[key];
                        s.WriteLine(key + "@" + val);
                    }
                    s.Close();
                }
                catch (Exception e)
                {
                    Logging.Write(LogType.Warning, "Could not write player data: " + e);
                }
                _changed = false;
            }
        }

        public void Load()
        {
            string toonName = ObjectManager.MyPlayer.Name;
            lock (this)
            {
                if (toonName == null)
                    return;
                _dic = new Dictionary<string, string>();
                try
                {
                    if (!Directory.Exists(GrindingEngine.OurDirectory + "\\PlayerData\\"))
                        Directory.CreateDirectory(GrindingEngine.OurDirectory + "\\PlayerData\\");
                    string filename = GrindingEngine.OurDirectory + "\\PlayerData\\" + ObjectManager.MyPlayer.Name +
                                      ".txt";
                    TextReader s = File.OpenText(filename);
                    int nr = 0;
                    string line;
                    while ((line = s.ReadLine()) != null)
                    {
                        char[] splitter = {'@'};
                        string[] st = line.Split(splitter);
                        if (st.Length == 2)
                        {
                            string key = st[0];
                            string val = st[1];
                            Set(key, val);
                            nr++;
                        }
                    }
                    Logging.Write("Found player data: " + nr);
                    s.Close();
                }
                catch (Exception)
                {
                    //Logging.Write("No toon data to read yet, but no worries :)");
                }
                _changed = false;
            }
        }

        public string Get(string key)
        {
            string val;
            if (_dic.TryGetValue(key, out val))
            {
                return val;
            }
            return null;
        }

        public void Set(string key, string name)
        {
            if (_dic.ContainsKey(key))
                _dic.Remove(key);
            _dic.Add(key, name);
            _changed = true;
        }

        public List<string> GetKeysContaining(string str)
        {
            return _dic.Keys.Where(key => key.Contains(str)).ToList();
        }
    }
}