
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

using System.Collections.Generic;
using System.Reflection;

namespace LazyLib.Helpers
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class ChatQueu
    {
        private static readonly List<string> Queue = new List<string>();
        public static string GetItem
        {
            get
            {
                lock (Queue)
                {
                    if (Queue.Count != 0)
                    {
                        string mes = Queue[0];
                        Queue.RemoveAt(0);
                        return mes;
                    }
                }
                return "";
            }
        }

        public static int QueueCount
        {
            get
            {
                lock (Queue)
                {
                    return Queue.Count;
                }
            }
        }

        public static void AddChat(string message)
        {
            lock (Queue)
            {
                Queue.Add(message);
            }
        }
    }
}