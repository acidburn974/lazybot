
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
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using LazyLib.Wow;

namespace LazyLib.Helpers.Mail
{
    public enum AddedItemsStatus
    {
        Error = 0,
        ClickedAll = 1,
        ClickedSomething = 2,
        ClickedNothing = 3,
    }

    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class MailManager
    {

        private static int _containerIndex;
        private static int _positionInBag;

        /// <summary>
        /// Does the mailing.
        /// </summary>
        public static void DoMail()
        {
            MailList.Load();
            Thread.Sleep(1000);
            MouseHelper.Hook();
            if (!MakeMailReady())
            {
                MouseHelper.ReleaseMouse();
                return;
            }
            AddedItemsStatus answer = ClickItems(true, 12);
            bool done = false;
            while (true)
            {
                switch (answer)
                {
                    case AddedItemsStatus.ClickedAll:
                        Logging.Write("Mail full sending");
                        MailFrame.ClickSend();
                        Thread.Sleep(3500);
                        if (LazySettings.MacroForMail)
                        {
                            SetMailNameUsingMacro();
                        }
                        else
                        {
                            MailFrame.SetReceiverHooked(LazySettings.MailTo);
                        }
                        Thread.Sleep(500);
                        break;
                    case AddedItemsStatus.ClickedSomething:
                        Logging.Write("Mail partly full sending and stopping loop");
                        MailFrame.ClickSend();
                        Thread.Sleep(500);
                        done = true;
                        break;
                    case AddedItemsStatus.Error:
                        //Continue the loop as it could just be a single read error.
                        break;
                    default:
                        done = true;
                        break;
                }
                if (done)
                    break;
                Thread.Sleep(1000);
                Application.DoEvents();
                answer = ClickItems(false, 12);
            }
            MailFrame.Close();
            MouseHelper.ReleaseMouse();
            CloseAllBags();
            Logging.Write("Brok loop with: " + answer);
        }

        /// <summary>
        /// Targest the mailbox, clicks the send mail tab and types to name we should send to.
        /// </summary>
        private static bool MakeMailReady()
        {
            try
            {
                TargetMailBox();
                Thread.Sleep(2000);
                OpenAllBags();
                Thread.Sleep(500);
                if (LazySettings.MacroForMail)
                {
                    return SetMailNameUsingMacro();
                }
                if (!ClickSendMailTab())
                {
                    return false;
                }
                Thread.Sleep(500);
                MailFrame.SetReceiverHooked(LazySettings.MailTo);
                Thread.Sleep(500);
                return true;
            } catch(Exception e)
            {
                Logging.Write("Exception MakeMailReady: " + e);
                return false;
            }
        }

        private static bool SetMailNameUsingMacro()
        {
            MailFrame.ClickMailFrame();
            Thread.Sleep(1000);
            MailFrame.ClickInboxTab();
            Thread.Sleep(500);
            KeyHelper.SendKey("MacroForMail");
            Thread.Sleep(500);
            if (!ClickSendMailTab())
            {
                return false;
            }
            Thread.Sleep(500);
            return true;
        }

        private static bool ClickSendMailTab()
        {
            MailFrame.ClickMailFrame();
            Thread.Sleep(1000);
            MailFrame.ClickSendMailTabHooked();
            Thread.Sleep(500);
            if (!MailFrame.CurrentTabIsSendMail)
            {
                Thread.Sleep(500);
                MailFrame.ClickSendMailTabHooked();
                Thread.Sleep(500);
                if (!MailFrame.CurrentTabIsSendMail)
                {
                    Logging.Write("Could not find mail frame");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns an enum based on how it went, send reset if you want to start from the beginning of the bags.
        /// </summary>
        /// <param name="reset">if set to <c>true</c> [reset].</param>
        /// <param name="howMany">How many should we click before returning ClickedAll.</param>
        /// <returns></returns>
        public static AddedItemsStatus ClickItems(bool reset, int howMany)
        {
            Logging.Write("Called addedToMail");
            if (reset)
            {
                _containerIndex = 1;
                _positionInBag = 1;
            }
            int clicked = 0;

            while (_containerIndex != 6)
            {
                Frame containerFrame = InterfaceHelper.GetFrameByName("ContainerFrame" + _containerIndex);
                if (containerFrame != null)
                {
                    int slots = GetSlotCount(_containerIndex);
                    Logging.Write("Found ContainerFrame with Slot count: " + slots);
                    while (_positionInBag != slots + 1 && clicked != howMany)
                    {
                        string itemStr = "ContainerFrame" + _containerIndex + "Item" + _positionInBag;
                        Frame itemOb = InterfaceHelper.GetFrameByName(itemStr);
                        if (itemOb != null)
                        {
                            itemOb.HoverHooked();
                            Thread.Sleep(150);
                            try
                            {
                                Frame toolTip = InterfaceHelper.GetFrameByName("GameTooltip");
                                if (toolTip != null)
                                {
                                    if (MailList.ShouldMail(toolTip.GetChildObject("GameTooltipTextLeft1").GetText))
                                    {
                                        Logging.Write("Adding: " +
                                                      toolTip.GetChildObject("GameTooltipTextLeft1").GetText);
                                        Thread.Sleep(150);
                                        itemOb.RightClickHooked();
                                        Thread.Sleep(150);
                                        clicked++;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Logging.Write("Exception when pasing gametooltip: " + e);
                            }
                        }
                        _positionInBag++;
                    }
                    if (_positionInBag == slots + 1)
                    {
                        _positionInBag = 1;
                        _containerIndex++;
                    }
                    if (clicked == howMany)
                        return AddedItemsStatus.ClickedAll;
                }
                else
                {
                    _containerIndex++;
                    return AddedItemsStatus.Error;
                }
            }
            if (clicked != 0)
                return AddedItemsStatus.ClickedSomething;
            return AddedItemsStatus.ClickedNothing;
        }

        /// <summary>
        /// Gets the slot count.
        /// </summary>
        /// <returns></returns>
        private static int GetSlotCount(int item)
        {
            try
            {
                if (item == 1)
                    return 16;
                if (item == 2)
                    return Inventory.Bag1.Slots;
                if (item == 3)
                    return Inventory.Bag2.Slots;
                if (item == 4)
                    return Inventory.Bag3.Slots;
                if (item == 5)
                    return Inventory.Bag4.Slots;
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// Targets the mail box.
        /// </summary>
        /// <returns></returns>
        public static bool TargetMailBox()
        {
            foreach (PGameObject node in ObjectManager.GetGameObject)
            {
                if (node.GameObjectType == 19 && node.Location.DistanceToSelf < 6)
                {
                    node.Location.Face();
                    Thread.Sleep(100);
                    node.Interact(false);
                    if (!MailFrame.Open)
                    {
                        node.Interact(false);
                        if (!MailFrame.Open)
                        {
                            RetryMailOpen(node);
                            if (!MailFrame.Open)
                            {
                                RetryMailOpen(node);
                                if (!MailFrame.Open)
                                {
                                    RetryMailOpen(node);
                                }
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        private static void RetryMailOpen(PGameObject node)
        {
            MoveHelper.StrafeLeft(true);
            Thread.Sleep(500);
            MoveHelper.StrafeLeft(false);
            node.Location.Face();
            Thread.Sleep(100);
            node.Interact(false);
            Thread.Sleep(1500);
        }

        /// <summary>
        /// Opens all bags.
        /// </summary>
        public static void OpenAllBags()
        {
            try
            {
                if (!InterfaceHelper.GetFrameByName("ContainerFrame1").IsVisible)
                {
                    InterfaceHelper.GetFrameByName("MainMenuBarBackpackButton").LeftClick();
                }
            }
            catch { }
            try
            {
                if (!InterfaceHelper.GetFrameByName("ContainerFrame2").IsVisible)
                {
                    InterfaceHelper.GetFrameByName("CharacterBag0Slot").LeftClick();
                }
            }
            catch { }
            try
            {
                if (!InterfaceHelper.GetFrameByName("ContainerFrame3").IsVisible)
                {
                    InterfaceHelper.GetFrameByName("CharacterBag1Slot").LeftClick();
                }
            }
            catch { }
            try
            {
                if (!InterfaceHelper.GetFrameByName("ContainerFrame4").IsVisible)
                {
                    InterfaceHelper.GetFrameByName("CharacterBag2Slot").LeftClick();
                }
            }
            catch { }
            try
            {
                if (!InterfaceHelper.GetFrameByName("ContainerFrame5").IsVisible)
                {
                    InterfaceHelper.GetFrameByName("CharacterBag3Slot").LeftClick();
                }
            }
            catch { }
        }

        public static void CloseAllBags()
        {
            try
            {
                if (InterfaceHelper.GetFrameByName("ContainerFrame1").IsVisible)
                {
                    InterfaceHelper.GetFrameByName("MainMenuBarBackpackButton").LeftClick();
                }
            }
            catch { }
            try
            {
                if (InterfaceHelper.GetFrameByName("ContainerFrame2").IsVisible)
                {
                    InterfaceHelper.GetFrameByName("CharacterBag0Slot").LeftClick();
                }
            }
            catch { }
            try
            {
                if (InterfaceHelper.GetFrameByName("ContainerFrame3").IsVisible)
                {
                    InterfaceHelper.GetFrameByName("CharacterBag1Slot").LeftClick();
                }
            }
            catch { }
            try
            {
                if (InterfaceHelper.GetFrameByName("ContainerFrame4").IsVisible)
                {
                    InterfaceHelper.GetFrameByName("CharacterBag2Slot").LeftClick();
                }
            }
            catch { }
            try
            {
                if (InterfaceHelper.GetFrameByName("ContainerFrame5").IsVisible)
                {
                    InterfaceHelper.GetFrameByName("CharacterBag3Slot").LeftClick();
                }
            }
            catch { }
        }
    }
}