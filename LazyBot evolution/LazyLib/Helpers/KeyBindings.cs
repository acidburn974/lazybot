
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
using System.Linq;
using LazyLib.Wow;

namespace LazyLib.Helpers
{
    public struct KeyBinding
    {
        public String Command;
        public String Key;
    }

    public static class KeyBindings
    {
        private static List<KeyBinding> _bindings =  new List<KeyBinding>();
        internal static void LoadBindings()
        {
            var result = new List<KeyBinding>();
            var numOfBindings = Memory.ReadRelative<uint>((uint)Pointers.KeyBinding.NumKeyBindings);
            var firstBind = Memory.Read<uint>(numOfBindings + (uint)Pointers.KeyBinding.First);
            uint nextBind = firstBind;
            while (nextBind != 0)
            {
                string key = Memory.ReadUtf8(Memory.Read<uint>(nextBind + (uint)Pointers.KeyBinding.Key), 100);
                string command = Memory.ReadUtf8(Memory.Read<uint>(nextBind + (uint)Pointers.KeyBinding.Command), 100);
                if (key.Length > 0 && command.Length > 0)
                {
                    var newKey = new KeyBinding { Command = command, Key = key };
                   // Logging.Write(string.Format("Command: {0} Key {1}", command, key));
                    result.Add(newKey);
                }
                nextBind = Memory.Read<uint>(nextBind + Memory.Read<uint>(numOfBindings + (uint)Pointers.KeyBinding.Next) + 4);
            }
            _bindings = result;
        }

        internal static List<KeyBinding> GetBindings()
        {
            return _bindings;
        }

        internal static List<String> GetKeysForCommand(string action)
        {
            return (from binding in GetBindings() where binding.Command == action select binding.Key).ToList();
        }

        internal static List<String> GetCommandsForKey(string key)
        {
            return (from binding in GetBindings() where binding.Key == key select binding.Command).ToList();

        }

        public static bool CheckBind(string command, string key)
        {
            //Logging.Debug("Checking key: " + key);
            List<string> keysForCommand = GetKeysForCommand(command);
            if (!keysForCommand.Contains(key))
            {
                Logging.Write(LogType.Error, "Key: " + command.ToLower() + " potentially bound incorrectly, should be: " + key.ToLower());
                return false;
            }
            return true;
        }
    }
}