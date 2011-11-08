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
using System.Windows.Forms;
using System.Xml;
using LazyLib;

namespace LazyEvo.Plugins.RotationPlugin
{
    internal class RotationManagerController
    {
        internal List<Rotation> Rotations = new List<Rotation>();
        private XmlDocument _doc;
        internal string Name { get; set; }

        public void ResetControllers()
        {
            Rotations = new List<Rotation>();
        }

        public void Load(string fileToLoad)
        {
            try
            {
                Name = Path.GetFileNameWithoutExtension(fileToLoad);
                _doc = new XmlDocument();
                _doc.Load(fileToLoad);
            }
            catch (Exception e)
            {
                Logging.Write("Error loading the rotation manager: " + e);
                return;
            }
            try
            {
                foreach (XmlNode childNode in _doc.GetElementsByTagName("RotationManager")[0])
                {
                    switch (childNode.Name)
                    {
                        case "Rotation":
                            var rotation = new Rotation();
                            rotation.Load(childNode);
                            Rotations.Add(rotation);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.Write("Error loading the rotation manager: " + e);
            }
        }

        internal void Save()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                if (!Directory.Exists(RotationManagerForm.OurDirectory + "\\Rotations\\"))
                    Directory.CreateDirectory(RotationManagerForm.OurDirectory + "\\Rotations\\");
                string xml = @"<?xml version=""1.0""?>";
                xml += "<RotationManager>";
                xml += "<Name>" + Name + "</Name>";
                xml = Rotations.Aggregate(xml, (current, rotation) => current + rotation.Save());
                xml += "</RotationManager>";
                try
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(xml);
                    doc.Save(RotationManagerForm.OurDirectory + "\\Rotations\\" + Name + ".xml");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Could not save rotation " + e);
                }
            }
        }
    }
}