
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml;

namespace LazyLib.Helpers
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class WowHeadData
    {
        public static string GetWowHeadSpell(double id)
        {
            string output = GetWebResponseSpell(id);
            if (output == null)
            {
                return null;
            }
            if (
                output.Equals(
                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?><wowhead><error>Item not found!</error></wowhead>"))
            {
                output = GetWebResponseSpell(id);
            }
            if (output != null)
            {
                string name;
                try
                {
                    int pos1 = output.IndexOf("<title>") + 7;
                    int pos2 = output.IndexOf("- Spell", pos1);
                    name = output.Substring(pos1, pos2 - pos1);
                }
                catch (Exception)
                {
                    //Log.log("Couldn't get item from WowHead (maintenance?), skipping" + e);
                    return null;
                }
                return name;
            }
            return null;
        }

        internal static string GetWebResponseSpell(double id)
        {
            try
            {
                string parameters = "?spell=" + id + "&xml";
                var uri = new Uri("http://www.wowhead.com/" + parameters);
                var request = (HttpWebRequest) WebRequest.Create(uri);
                request.Method = WebRequestMethods.Http.Get;
                var response = (HttpWebResponse) request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                string output = reader.ReadToEnd();
                response.Close();
                return output;
            }
            catch
            {
                return null;
            }
        }

        public static Dictionary<string, string> GetWowHeadItem(double id)
        {
            string output = GetWebResponse(id);
            if (output == null)
            {
                return null;
            }

            if (
                output.Equals(
                    "<?xml version=\"1.0\" encoding=\"UTF-8\"?><wowhead><error>Item not found!</error></wowhead>"))
            {
                output = GetWebResponse(id);
            }
            if (output != null)
            {
                var item = new Dictionary<string, string>();
                try
                {
                    item = ProcessWhData(output);
                }
                catch (Exception)
                {
                    //Log.log("Couldn't get item from WowHead (maintenance?), skipping" + e);
                    return null;
                }
                return item;
            }
            return null;
        }

        internal static string GetWebResponse(double id)
        {
            try
            {
                string parameters = "?item=" + id + "&xml";
                Uri uri;
                switch (LazySettings.Language)
                {
                    case LazySettings.LazyLanguage.Russian:
                        uri = new Uri("http://ru.wowhead.com/" + parameters);
                        break;
                    case LazySettings.LazyLanguage.German:
                        uri = new Uri("http://de.wowhead.com/" + parameters);
                        break;
                    case LazySettings.LazyLanguage.French:
                        uri = new Uri("http://fr.wowhead.com/" + parameters);
                        break;
                    case LazySettings.LazyLanguage.Spanish:
                        uri = new Uri("http://es.wowhead.com/" + parameters);
                        break;
                    default:
                        uri = new Uri("http://www.wowhead.com/" + parameters);
                        break;
                }
                var request = (HttpWebRequest) WebRequest.Create(uri);
                request.Method = WebRequestMethods.Http.Get;
                var response = (HttpWebResponse) request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                string output = reader.ReadToEnd();
                response.Close();
                return output;
            }
            catch
            {
                return null;
            }
        }

        private static Dictionary<string, string> ProcessWhData(string data)
        {
            var rd = new Dictionary<string, string>();           
            string name = string.Empty;
            string quality = string.Empty;

            data = data.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");

            XmlDocument whData = new XmlDocument();
            whData.LoadXml(@data);
            XmlNode whRoot = whData.SelectSingleNode("wowhead");

            if (whRoot != null)
                foreach (XmlNode node in whRoot.ChildNodes)
                {
                    if (node.Name == "item")
                    {
                        foreach (XmlNode node2 in node.ChildNodes)
                        {
                            if (node2.Name == "name")
                            {
                                name = node2.InnerText;
                            }
                            if (node2.Name == "quality")
                            {
                                quality = node2.InnerText;
                            }
                        }
                    }
                }
            rd.Add("name", name);
            rd.Add("quality", quality);
            return rd;
        }
    }
}