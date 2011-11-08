
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
using System.Reflection;
using System.Threading;
using LazyLib.Wow;

namespace LazyLib.Helpers
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class Inventory
    {
        public static List<ulong> GUIDOfItemsInBag
        {
            get { return ObjectManager.MyPlayer.GUIDOfItemsInBag; }
        }

        private static List<UInt64> GUIDOfBags
        {
            get
            {
                var guids = new List<UInt64>();
                try
                {
                    guids.Add(Bag1GUID);
                }
                catch
                {

                }
                try
                {
                    guids.Add(Bag2GUID);
                }
                catch
                {

                }
                try
                {
                    guids.Add(Bag3GUID);
                }
                catch
                {

                }
                try
                {
                    guids.Add(Bag4GUID);
                }
                catch
                {

                }
                return guids;
            }
        }

        private static UInt64 Bag1GUID
        {
            get { return Memory.ReadRelative<UInt64>(((uint)Pointers.Container.EquippedBagGUID)); }
        }

        private static UInt64 Bag2GUID
        {
            get { return Memory.ReadRelative<UInt64>(((uint)Pointers.Container.EquippedBagGUID + 0x8 * 1)); }
        }

        private static UInt64 Bag3GUID
        {
            get { return Memory.ReadRelative<UInt64>(((uint)Pointers.Container.EquippedBagGUID + 0x8 * 2)); }
        }

        private static UInt64 Bag4GUID
        {
            get { return Memory.ReadRelative<UInt64>(((uint)Pointers.Container.EquippedBagGUID + 0x8 * 3)); }
        }

        public static PContainer Bag1
        {
            get { return ObjectManager.GetContainers.FirstOrDefault(container => Bag1GUID == container.GUID); }
        }

        public static PContainer Bag2
        {
            get { return ObjectManager.GetContainers.FirstOrDefault(container => Bag2GUID == container.GUID); }
        }

        public static PContainer Bag3
        {
            get { return ObjectManager.GetContainers.FirstOrDefault(container => Bag3GUID == container.GUID); }
        }

        public static PContainer Bag4
        {
            get { return ObjectManager.GetContainers.FirstOrDefault(container => Bag4GUID == container.GUID); }
        }

        /// <summary>
        ///   Gets the get items in bags.
        /// </summary>
        /// <value>The get items in bags.</value>
        public static List<PItem> GetItemsInBags
        {
            get
            {
                try
                {
                    List<ulong> bagGuids = new List<ulong>();
                    try
                    {
                        bagGuids = GUIDOfBags;
                    }
                    catch
                    {

                    }
                    var items = new List<PItem>();
                    List<ulong> guids = GUIDOfItemsInBag;
                    foreach (PItem pItem in ObjectManager.GetItems)
                    {
                        if (pItem != null)
                        {
                            try
                            {
                                if (bagGuids.Contains(pItem.Contained) || guids.Contains(pItem.GUID))
                                {
                                    //Logging.Write(pItem.GUID + "");
                                    items.Add(pItem);
                                }
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                    return items;
                }
                catch (Exception e)
                {
                    Logging.Write("Exception in GetItemsInBags  (Cannot complete vendoring :( ) {0}", e);
                    return new List<PItem>();
                }
            }
        }

        /// <summary>
        /// Gets the free bag slots.
        /// </summary>
        /// <value>The free bag slots.</value>
        public static int FreeBagSlots
        {
            get
            {
                try
                {
                    Frame mainMenuBarBackpackButton = InterfaceHelper.GetFrameByName("MainMenuBarBackpackButton");
                    Frame mainMenuBarBackpackButtonCount = mainMenuBarBackpackButton.GetChildObject("MainMenuBarBackpackButtonCount");
                    string s = mainMenuBarBackpackButtonCount.GetText;
                    int i = Convert.ToInt32(s.Split('(')[1].Split(')')[0]);
                    return i;
                }
                catch
                {
                    return int.MaxValue;
                }
            }
        }

        /// <summary>
        /// Closes all bags.
        /// </summary>
        public static void CloseAllBags()
        {
            for (int i = 1; i <= 5; i++)
            {
                Frame containerFrame = InterfaceHelper.GetFrameByName("ContainerFrame" + i);
                Frame containerFrameCloseButton = InterfaceHelper.GetFrameByName("ContainerFrame" + i + "CloseButton");

                if (containerFrame == null &&
                    containerFrameCloseButton == null)
                    continue;
                if (containerFrame != null)
                    if (containerFrame.IsVisible)
                    {
                        containerFrameCloseButton.LeftClick();
                        Thread.Sleep(1500);
                    }
            }
        }

        /// <summary>
        /// Opens the bag by number (starts at 0).
        /// </summary>
        /// <param name="index">The index.</param>
        public static void OpenBagByNumber(int index)
        {
            Frame bagSlot;
            switch (index)
            {
                case 0:
                    bagSlot = InterfaceHelper.GetFrameByName("MainMenuBarBackpackButton");
                    break;
                case 1:
                    bagSlot = InterfaceHelper.GetFrameByName("CharacterBag0Slot");
                    break;
                case 2:
                    bagSlot = InterfaceHelper.GetFrameByName("CharacterBag1Slot");
                    break;
                case 3:
                    bagSlot = InterfaceHelper.GetFrameByName("CharacterBag2Slot");
                    break;
                case 4:
                    bagSlot = InterfaceHelper.GetFrameByName("CharacterBag3Slot");
                    break;
                default:
                    throw new ArgumentException("Number outside bounds");
            }
            if (bagSlot == null)
            {
                Logging.Write("Could not find bag " + index);
                return;
            }
            bagSlot.LeftClick();
            Thread.Sleep(500);
        }
    }
}