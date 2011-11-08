
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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;
using LazyLib.Wow;

#endregion

namespace LazyLib.Helpers
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public static class KeyHelper
    {
        internal static readonly IDictionary<string, KeyWrapper> KeysList = new Dictionary<string, KeyWrapper>();
        private static readonly Ticker Open = new Ticker(800);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern short GetKeyState(int virtualKeyCode);
        private const string KeyFile = "\\Settings\\Keys.xml";
        internal const string InteractWithMouseover = "InteractWithMouseOver";
        internal const string InteractTarget = "InteractTarget";
        internal const string TargetLastTarget = "TargetLastTarget";
        private static object _lock = new object();
        public static void LoadKeys()
        {
            lock (_lock)
            {
                if (!Directory.Exists(LazySettings.OurDirectory + "\\Settings"))
                    Directory.CreateDirectory(LazySettings.OurDirectory + "\\Settings");
                if (!File.Exists(LazySettings.OurDirectory + KeyFile)) //Lets create default keys if none exist
                {
                    SaveKeys();
                }
                AddKey("Eat", "None", LazySettings.KeysEatBar, LazySettings.KeysEatKey);
                AddKey("Drink", "None", LazySettings.KeysDrinkBar, LazySettings.KeysDrinkKey);
                AddKey("GMount", "None", LazySettings.KeysGroundMountBar, LazySettings.KeysGroundMountKey);
                AddKey("Q", "None", "Indifferent", LazySettings.KeysStafeLeftKeyText);
                AddKey("E", "None", "Indifferent", LazySettings.KeysStafeRightKeyText);
                AddKey("Attack1", "None", LazySettings.KeysAttack1Bar, LazySettings.KeysAttack1Key);
                AddKey("MacroForMail", "None", LazySettings.KeysMailMacroBar, LazySettings.KeysMailMacroKey);
                AddKey(InteractWithMouseover, "None", "Indifferent", LazySettings.KeysInteractKeyText);
                AddKey(InteractTarget, "None", "Indifferent", LazySettings.KeysInteractTargetText);
                AddKey(TargetLastTarget, "None", "Indifferent", LazySettings.KeysTargetLastTargetText);              
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(LazySettings.OurDirectory + KeyFile);
                } catch (Exception e)
                {
                    Logging.Write(LogType.Error, "Could not load keys: " + e);
                    return;
                }
                XmlNodeList keys = doc.GetElementsByTagName("KeyWrapper");
                foreach (XmlNode key in keys)
                {
                    string name = string.Empty;
                    string shiftState = string.Empty;
                    string barState = string.Empty;
                    string character = string.Empty;
                    foreach (XmlNode childNode in key.ChildNodes)
                    {
                        switch (childNode.Name)
                        {
                            case "name":
                                name = childNode.InnerText;
                                break;
                            case "shiftstate":
                                shiftState = childNode.InnerText;
                                break;
                            case "bar":
                                barState = childNode.InnerText;
                                break;
                            case "key":
                                character = childNode.InnerText;
                                break;
                        }
                    }
                    if (!string.IsNullOrEmpty(name))
                    {
                        AddKey(name, shiftState, barState, character);
                    }
                }
            }
        }

        private static void SaveKeys()
        {
            var list = new Dictionary<string, KeyWrapper>();
            list.Add("Up", new KeyWrapper("Up", "None", "Indifferent", "Up"));
            list.Add("Down", new KeyWrapper("Down", "None", "Indifferent", "Down"));
            list.Add("Right", new KeyWrapper("Down", "None", "Indifferent", "Right"));
            list.Add("Left", new KeyWrapper("Down", "None", "Indifferent", "Left"));
            list.Add("Space", new KeyWrapper("Space", "None", "Indifferent", "Space"));
            list.Add("X", new KeyWrapper("X", "None", "Indifferent", "X"));
            list.Add("PetAttack", new KeyWrapper("PetAttack", "Ctrl", "Indifferent", "1"));
            list.Add("PetFollow", new KeyWrapper("PetFollow", "Ctrl", "Indifferent", "2"));
            list.Add("F1", new KeyWrapper("F1", "None", "Indifferent", "F1"));
            list.Add("TargetEnemy", new KeyWrapper("Tab", "None", "Indifferent", "Tab"));
            list.Add("TargetFriend", new KeyWrapper("TargetFriend", "Ctrl", "Indifferent", "Tab"));
            list.Add("ESC", new KeyWrapper("ESC", "None", "Indifferent", "Escape"));
            list.Add("InventoryOpenAll", new KeyWrapper("InventoryOpenAll", "None", "Indifferent", "B"));
            StringBuilder xml = new StringBuilder();
            xml.AppendFormat(@"<?xml version=""1.0""?>");
            xml.AppendFormat("<KeyList>");
            foreach (var keyWrapper in list)
            {
                xml.AppendFormat("<KeyWrapper>");
                xml.AppendFormat("<name>{0}</name>", keyWrapper.Key);
                xml.AppendFormat("<shiftstate>{0}</shiftstate>", keyWrapper.Value.Special);
                xml.AppendFormat("<bar>{0}</bar>", keyWrapper.Value.Bar);
                xml.AppendFormat("<key>{0}</key>", keyWrapper.Value.Key);
                xml.AppendFormat("</KeyWrapper>");
            }
            xml.AppendFormat("</KeyList>");
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml.ToString());
                doc.Save(LazySettings.OurDirectory + KeyFile);
            } catch (Exception e)
            {
                Logging.Write("Could not save the keys: " + e);
            }
        }

        public static void AddKey(string name, string shiftState, string barState, string character)
        {
            lock (_lock)
            {
                if (KeysList.ContainsKey(name))
                    KeysList.Remove(name);
                KeysList.Add(name, new KeyWrapper(name, shiftState, barState, character));
            }
        }

        /// <summary>
        ///   SendKey
        /// </summary>
        /// <param name = "name">
        ///   Key name to send
        /// </param>
        public static void SendKey(string name)
        {
            lock (_lock)
            {
                //LazyBot.Log.Debug("IsCasting spell: " + name);
                if (KeysList.ContainsKey(name))
                {
                    KeyWrapper key = KeysList[name];
                    key.SendKey();
                }
                else
                {
                    Logging.Write("Unknown key: " + name);
                }
            }
        }

        public static bool HasKey(string name)
        {
            return KeysList.ContainsKey(name);
        }

        /// <summary>
        ///   Press and hold a key
        /// </summary>
        /// <param name = "name">
        ///   Key name to press and hold
        /// </param>
        public static void PressKey(string name)
        {
            lock (_lock)
            {               
                if (KeysList.ContainsKey(name))
                {
                   // Logging.Debug("PressKey: " + name);
                    KeyWrapper key = KeysList[name];
                    //Logging.Write(key.Bar + " " + key.Key);
                    key.PressKey();
                }
                else
                {
                    Logging.Write("The key " + name + " could not be send");
                }
            }
        }

        /// <summary>
        ///   Release a held key.
        /// </summary>
        /// <param name = "name">
        ///   Key name to release
        /// </param>
        public static void ReleaseKey(string name)
        {
            lock (_lock)
            {
                if (KeysList.ContainsKey(name))
                {
                  //  Logging.Debug("ReleaseKey: " + name);
                    KeyWrapper key = KeysList[name];
                    key.ReleaseKey();
                }
                else
                {
                    Logging.Write("The key " + name + " could not be send");
                }
            }
        }

        public static void ChatboxSendText(String text)
        {
            if (IsChatboxOpened)
            {
                KeyLowHelper.PressKey(MicrosoftVirtualKeys.VK_LCONTROL);
                KeyLowHelper.PressKey(MicrosoftVirtualKeys.A);
                KeyLowHelper.ReleaseKey(MicrosoftVirtualKeys.A);
                KeyLowHelper.ReleaseKey(MicrosoftVirtualKeys.VK_LCONTROL);
                KeyLowHelper.PressKey(MicrosoftVirtualKeys.Delete);
                KeyLowHelper.ReleaseKey(MicrosoftVirtualKeys.Delete);
                Thread.Sleep(200);
            }
            else
            {
                KeyLowHelper.SendEnter();
                Open.Reset();
                while (!IsChatboxOpened && !Open.IsReady)
                {
                    Thread.Sleep(2);
                }
            }
            SendTextNow(text);
            Thread.Sleep(1000);
            KeyLowHelper.SendEnter();
        }

        public static void SendEnter()
        {
            KeyLowHelper.SendEnter();
        }

        public static void SendTextNow(String text)
        {
            foreach (char c in text)
            {
                KeyLowHelper.SendMessage(Memory.WindowHandle, KeyLowHelper.WmChar, (IntPtr)c, (IntPtr)0);
            }
        }

        public static bool IsChatboxOpened
        {
            get { return Memory.ReadRelative<uint>((uint)Pointers.Globals.ChatboxIsOpen) == 1; }
        }
    }
}