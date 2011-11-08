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
using System.Globalization;
using System.IO;
using System.Xml;
using LazyLib;
using LazyLib.Wow;

#endregion

namespace LazyEvo.LFlyingEngine.Helpers
{
    internal static class BadNodes
    {
        private static readonly XmlDocument _doc = new XmlDocument();

        public static List<Location> GetBadNodeList()
        {
            var list = new List<Location>();
            if (File.Exists(FlyingEngine.OurDirectory + "\\badNodes.xml"))
            {
                try
                {
                    _doc.Load(FlyingEngine.OurDirectory + "\\badNodes.xml");
                }
                catch (Exception h)
                {
                    Logging.Write(
                        "Could not load badNodes.xml did you add something invalid? Try opening the file in your browser " +
                        h);
                }
            }
            else
            {
                return new List<Location>();
            }
            XmlNodeList badNode = _doc.GetElementsByTagName("bad_location");
            try
            {
                foreach (XmlNode node in badNode)
                {
                    try
                    {
                        string temp = node.ChildNodes[1].InnerText;
                        //Make sure it also works on systems that does not use .
                        string correctString = temp;
                        if (
                            Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator) != ".")
                            correctString = temp.Replace(".", CultureInfo.CurrentCulture.NumberFormat.
                                                                  NumberDecimalSeparator);

                        var bad = new Location(correctString);
                        list.Add(bad);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            return list;
        }
    }
}