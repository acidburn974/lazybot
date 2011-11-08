
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
using System.Reflection;
using LazyLib.Wow;

#endregion

namespace LazyLib.Helpers
{
    /// <summary>
    ///   Activated if a new chat message arives ingame
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class GChatEventArgs : EventArgs
    {
        /// <summary>
        ///   Gets or sets the MSG.
        /// </summary>
        /// <value>The MSG.</value>
        public ChatMsg Msg { get; set; }
    }

    /// <summary>
    ///   A Chat Msg
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public struct ChatMsg
    {
        public string Channel;
        public string Msg;
        public string Player;
        public Constants.ChatType Type;
    }

    /// <summary>
    ///   Handles the chat in wow
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class Chat
    {
        private object _chatReaderLock = new object();
        private List<ChatMsg> _listCompleteChat;
        private List<string> _listCurrentChat;
        private List<string> _listLastChat;
        private List<ChatMsg> _listLastestChat;

        /// <summary>
        ///   Occurs when [new chat message].
        /// </summary>
        [ObfuscationAttribute(Feature = "renaming")]
        public static event EventHandler<GChatEventArgs> NewChatMessage;
        /// <summary>
        ///   Prepares the reading.
        /// </summary>
        public void PrepareReading()
        {
            _listCurrentChat = new List<string>();
            _listCompleteChat = new List<ChatMsg>();
            _listLastestChat = new List<ChatMsg>();
        }

        /// <summary>
        ///   Reads the chat.
        /// </summary>
        public void ReadChat()
        {
            try
            {
                if (_chatReaderLock == null)
                {
                    PrepareReading();
                    _chatReaderLock = new object();
                }
                lock (_chatReaderLock)
                {

                    _listCurrentChat.Clear();

                    for (int i = 0; i < 60; i++)
                    {
                        _listCurrentChat.Add(
                            Memory.ReadUtf8StringRelative(Convert.ToUInt32((uint) Pointers.Chat.ChatStart +
                                                                           (i*
                                                                            (uint)
                                                                            Pointers.Chat.OffsetToNextMsg)),
                                                          512));
                    }

                    if (_listLastChat == null) _listLastChat = new List<string>(_listCurrentChat);
                    FindNewMessages();
                    _listLastChat = new List<string>(_listCurrentChat);
                }
            } catch {}
        }

        /// <summary>
        ///   Finds the new messages.
        /// </summary>
        private void FindNewMessages()
        {
            for (int i = 0; i < 60; i++)
            {
                if (_listCurrentChat[i] != _listLastChat[i])
                {
                    ChatMsg msg = ParseChatMsg(_listCurrentChat[i]);
                    if (NewChatMessage != null)
                    {
                        NewChatMessage(this, new GChatEventArgs {Msg = msg});
                    }
                    _listCompleteChat.Add(msg);
                    _listLastestChat.Add(msg);
                }
            }
        }

        /// <summary>
        ///   Parses the chat MSG.
        /// </summary>
        /// <param name = "strChatMsg">The STR chat MSG.</param>
        /// <returns></returns>
        private static ChatMsg ParseChatMsg(string strChatMsg)
        {
            try
            {
                //Type: [1], Channel: [], Player Name: [Fdgfisdgn], Sender GUID: [06800000021AF61B], Text: [CatchMe!]
                ChatMsg sMsg;
                int pos1 = strChatMsg.IndexOf("Type: [") + 7;
                int pos2 = strChatMsg.IndexOf("]", pos1);
                if (!(pos1 > 0 || pos2 > 0)) return new ChatMsg();
                sMsg.Type = (Constants.ChatType) Int32.Parse(strChatMsg.Substring(pos1, pos2 - pos1));

                pos1 = strChatMsg.IndexOf("Channel: [") + 10;
                pos2 = strChatMsg.IndexOf("]", pos1);
                sMsg.Channel = strChatMsg.Substring(pos1, pos2 - pos1);

                pos1 = strChatMsg.IndexOf("Player Name: [") + 14;
                pos2 = strChatMsg.IndexOf("]", pos1);
                sMsg.Player = strChatMsg.Substring(pos1, pos2 - pos1);

                pos1 = strChatMsg.IndexOf("Sender GUID: [") + 14;
                pos2 = strChatMsg.IndexOf("]", pos1);
                strChatMsg.Substring(pos1, pos2 - pos1);

                pos1 = strChatMsg.IndexOf("Text: [") + 7;
                pos2 = pos1;
                while (true)
                {
                    int tmp = strChatMsg.IndexOf("]", pos2 + 1);
                    if (tmp == -1) break;
                    pos2 = tmp;
                }
                sMsg.Msg = strChatMsg.Substring(pos1, pos2 - pos1);

                return sMsg;
            }
            catch
            {
                var sMsg = new ChatMsg();
                return sMsg;
            }
        }
    }
}