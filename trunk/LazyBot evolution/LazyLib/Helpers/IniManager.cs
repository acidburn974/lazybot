/*
This file is part of LazyBot.

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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace LazyLib.Helpers
{
    /// <summary>
    ///   Use INI files to save and load data.
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class IniManager
    {
        public string FilePath;
        public IniManager(string fileName)
        {
            if (!Directory.Exists(Directory.GetParent(fileName).FullName))
                Directory.CreateDirectory(Directory.GetParent(fileName).FullName);
            FilePath = fileName;
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
                                                             string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                                                          string key, string def, StringBuilder retVal,
                                                          int size, string filePath);

        public void IniWriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, FilePath);
        }

        public void IniWriteValue(string section, string key, bool value)
        {
            IniWriteValue(section, key, value.ToString());
        }

        public void IniWriteValue(string section, string key, double value)
        {
            IniWriteValue(section, key, value.ToString());
        }

        public void IniWriteValue(string section, string key, int value)
        {
            IniWriteValue(section, key, value.ToString());
        }

        public string IniReadValue(string section, string key)
        {
            var temp = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", temp,
                                    255, FilePath);
            return temp.ToString();
        }

        public string GetString(string section, string key, string def)
        {
            if (IniReadValue(section, key).Equals(""))
                return def;
            return IniReadValue(section, key);
        }

        public int GetInt(string section, string key, int def)
        {
            try
            {
                return Convert.ToInt32(IniReadValue(section, key));
            }
            catch
            {
                return def;
            }
        }

        public bool GetBoolean(string section, string key, bool def)
        {
            try
            {
                return Convert.ToBoolean(IniReadValue(section, key));
            }
            catch
            {
                return def;
            }
        }
    }
}