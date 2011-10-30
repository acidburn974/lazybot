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

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using LazyLib.Helpers.Mail;
using LazyLib.Wow;

namespace LazyLib.Helpers.Vendor
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class VendorManager
    {
        private static readonly List<string> Sold = new List<string>();
        public static void DoSell(PUnit vendor)
        {
            try
            {
                ProtectedList.Load();
                MailList.Load();
                MoveHelper.MoveToUnit(vendor, 3);
                vendor.Location.Face();
                vendor.Interact(false);
                Thread.Sleep(1000);
                if(ObjectManager.MyPlayer.Target != vendor)
                {
                    vendor.Location.Face();
                    vendor.Interact(false);
                    Thread.Sleep(1000);
                }
                MouseHelper.Hook();
                MailManager.OpenAllBags();
                if (LazySettings.ShouldVendor)
                {
                    Logging.Write("[Vendor]Going to sell items");
                    Sell();
                }
                if (LazySettings.ShouldRepair)
                {
                    Repair();
                }
            } finally
            {
                MailManager.CloseAllBags();
                MouseHelper.ReleaseMouse();
            }
        }

        private static void Sell()
        {
            Sold.Clear();
            LoadWowHead();
            SellLoop();
        }

        private static void LoadWowHead()
        {
            foreach (PItem item in Inventory.GetItemsInBags)
            {
                if (ItemDatabase.GetItem(item.EntryId.ToString()) == null)
                {
                    Dictionary<string, string> dictionary = WowHeadData.GetWowHeadItem(item.EntryId);
                    if (dictionary != null)
                    {
                        string name = dictionary["name"];
                        string quality = dictionary["quality"];
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(quality))
                        {
                            ItemDatabase.PutItem(item.EntryId.ToString(), name, quality);
                        }
                    }
                }
            }
        }

        private static void SellLoop()
        {
            int containerIndex = 1;
            int positionInBag = 1;
            while (containerIndex != 6)
            {
                Frame containerFrame = InterfaceHelper.GetFrameByName("ContainerFrame" + containerIndex);
                if (containerFrame != null)
                {
                    int slots = GetSlotCount(containerIndex);
                    Logging.Write("Found ContainerFrame with Slot count: " + slots);
                    while (positionInBag != slots + 1)
                    {
                        string itemStr = "ContainerFrame" + containerIndex + "Item" + positionInBag;
                        Frame itemOb = InterfaceHelper.GetFrameByName(itemStr);
                        if (itemOb != null)
                        {
                            itemOb.HoverHooked();
                            Thread.Sleep(170);
                            try
                            {
                                Frame toolTip = InterfaceHelper.GetFrameByName("GameTooltip");
                                if (toolTip != null)
                                {
                                    Frame childObject = toolTip.GetChildObject("GameTooltipTextLeft1");
                                    if (childObject != null)
                                    {
                                        if (ShouldSell(childObject.GetText))
                                        {
                                            Logging.Write("Selling: " + childObject.GetText);
                                            Thread.Sleep(150);
                                            itemOb.RightClickHooked();
                                            Thread.Sleep(150);
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Logging.Write("Exception when pasing gametooltip: " + e);
                            }
                        }
                        positionInBag++;
                    }
                    if (positionInBag == slots + 1)
                    {
                        positionInBag = 1;
                        containerIndex++;
                    }
                }
                else
                {
                    containerIndex++;
                }
            }
        }

        private static bool ShouldSell(string sellName)
        {
            try
            {
                foreach (PItem item in Inventory.GetItemsInBags)
                {
                    try
                    {
                        if (ItemDatabase.GetItem(item.EntryId.ToString()) != null)
                        {
                            string name = ItemDatabase.GetItem(item.EntryId.ToString())["item_name"].ToString();
                            string quality = ItemDatabase.GetItem(item.EntryId.ToString())["item_quality"].ToString();
                            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(quality))
                            {
                                if(name != sellName)
                                {
                                    continue; 
                                }
                                if (MailList.ShouldMail(name))
                                {
                                    continue;
                                }
                                if (ProtectedList.ShouldVendor(name))
                                {
                                    switch (quality)
                                    {
                                        case "Poor":
                                            if (LazySettings.SellPoor)
                                                return true;
                                            break;
                                        case "Common":
                                            if (LazySettings.SellCommon)
                                                return true;
                                            break;
                                        case "UnCommon":
                                            if (LazySettings.SellUncommon)
                                                return true;
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                Logging.Write(string.Format("[Vendor]Could not detect the name of: {0} is wowhead down?", item.EntryId));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.Debug("Exception in ShouldSell (Loop): {0}", e);
                    }
                }
            }
            catch (Exception e)
            {
                Logging.Debug("Exception in ShouldSell: {0}", e);
            }
            return false;
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


        private static void Repair()
        {
           Frame frameByName = InterfaceHelper.GetFrameByName("MerchantRepairAllButton");
           if(frameByName != null)
           {
               frameByName.LeftClick();
           }
        }
    }
}
