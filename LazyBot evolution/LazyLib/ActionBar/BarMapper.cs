
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
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using LazyLib.Helpers;
using LazyLib.Wow;

#endregion

namespace LazyLib.ActionBar
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class BarMapper
    {
        private static Dictionary<Int32, String> _spellDatabase;
        private static readonly List<WowKey> LoadedKeys = new List<WowKey>();
        private static readonly List<BarItem> BarItems = new List<BarItem>();
        private static readonly IDictionary<string, BarSpell> Spells = new Dictionary<string, BarSpell>();
        private static readonly Dictionary<Int32, String> SpellsUsed = new Dictionary<int, string>();
        public static int SpellsLoaded
        {
            get { return Spells.Count; }
        }

        #region LoadKeys

        public static string GetNameFromSpell(int spellId)
        {
            Load();
            try
            {
                if (_spellDatabase.ContainsKey(spellId))
                {
                    return _spellDatabase[spellId];
                }
                return string.Empty;
            } catch (Exception)
            {
                Logging.Write("Error find name of spell: " + spellId);
                return string.Empty;
            }
        }

        private static void Load()
        {
            if (_spellDatabase == null)
            {
                try
                {
                    _spellDatabase = new Dictionary<int, string>();
                    string[] spellsSplit = Resource.Spells.Split('\n');
                    foreach (string s in spellsSplit)
                    {
                        if (s.Contains("="))
                        {
                            int id = Convert.ToInt32(s.Split('=')[0]);
                            string name = s.Split('=')[1].Replace("\n", "").Replace("\r", "");
                            if (!_spellDatabase.ContainsKey(id))
                                _spellDatabase.Add(id, name);
                        }
                    }
                    Logging.Debug("[Mapper] We loaded " + _spellDatabase.Count + " spells");
                }
                catch (Exception e)
                {
                    Logging.Write(LogType.Error, "[Mapper] Spells could not be loaded, LazyBot is fubar :( " + e);
                }
            }
        }

        public static void MapBars()
        {
            LoadedKeys.Clear();
            BarItems.Clear();
            Spells.Clear();
            const int barSize = 0x30;
            int maxSlots = 5*12;
            switch (ObjectManager.MyPlayer.UnitClass)
            {
                case Constants.UnitClass.UnitClass_Warrior:
                    maxSlots = 8 * 12;
                    break;
                case Constants.UnitClass.UnitClass_Rogue:
                    maxSlots = 6 * 12;
                    break;
                case Constants.UnitClass.UnitClass_Priest:
                    maxSlots = 6 * 12;
                    break;
                case Constants.UnitClass.UnitClass_Druid:
                    maxSlots = 8 * 12;
                    break;
            }
            Int32 currentSlot = 1;
            Int32 currentBar = 1;
            for (uint i = 0; i < maxSlots; i++)
            {
                if (currentSlot > 12)
                {
                    currentBar++;
                    currentSlot = 1;
                }
                var actionId = Memory.ReadRelative<UInt32>((uint) Pointers.ActionBar.ActionBarFirstSlot + (0x4*i) + barSize);
                if (actionId != 0)
                {
                    LoadedKeys.Add(new WowKey(actionId, currentBar, currentSlot));
                }
                currentSlot++;
            }
            var bonusBar = Memory.ReadRelative<Int32>((uint) Pointers.ActionBar.ActionBarBonus);
            if (bonusBar == 0)
            {
                for (uint i = 0; i < 12; i++)
                {
                    var actionId = Memory.ReadRelative<UInt32>((uint) Pointers.ActionBar.ActionBarFirstSlot + (0x4*i));
                    if (actionId != 0)
                    {
                        LoadedKeys.Add(new WowKey(actionId, 0, (int) i + 1));
                    }
                    currentSlot++;
                }
            }
            else
            {
                for (uint i = 0; i < 12; i++)
                {
                    var actionId = Memory.ReadRelative<UInt32>((uint) Pointers.ActionBar.ActionBarFirstSlot + (0x4*i) + (uint) barSize*6 + (((uint) bonusBar - 1)*barSize));
                    if (actionId != 0)
                    {
                        LoadedKeys.Add(new WowKey(actionId, 0, (int) i + 1));
                    }
                    currentSlot++;
                }
            } 
            //Load all spells 
            Load();
            LoadedKeys.Reverse(); //Reverse the list to get bar 1 first
            //Now lets find names and assign the correct once)
            foreach (WowKey wowKey in LoadedKeys)
            {
                string name = String.Empty;
                //First udjust the bar and the 0 key
                if(wowKey.Bar > 5)
                {
                    wowKey.Bar = 0;
                }
                wowKey.Bar = wowKey.Bar + 1;
                if (wowKey.Key == 10)
                    wowKey.Key = 0;
                if (wowKey.Key > 10)
                    continue;
                //Now lets sort by spells and items
                if (wowKey.Type.Equals(KeyType.Spell))
                {
                    name = GetNameFromSpell(wowKey.SpellId);
                    if (name != String.Empty)
                    {
                        if (!Spells.ContainsKey(name))
                        {
                            Logging.Debug("Found key: " + name + " : " + wowKey.Bar + " : " + wowKey.Key);
                            Spells.Add(name, new BarSpell(wowKey.SpellId, wowKey.Bar, wowKey.Key, name));
                        }
                        else
                        {
                            Logging.Debug("Key: " + name + " : " + wowKey.Bar + " : " + wowKey.Key + " is a duplicate");
                        }
                    }
                }
                if (wowKey.Type.Equals(KeyType.Item))
                {
                    BarItems.Add(new BarItem(wowKey.ItemId, wowKey.Bar, wowKey.Key));
                    Logging.Debug(string.Format("Found item: {0} : {1} : {2}", ItemHelper.GetNameById((uint) wowKey.ItemId), wowKey.Bar, wowKey.Key));
                }
            }
            LoadedKeys.Clear();
            GC.Collect();
        }
        #endregion

        public static Boolean HasSpellById(int spellId)
        {
            return Spells.Any(spell => spell.Value.SpellId.Equals(spellId));
        }

        public static Boolean HasSpellByName(String spellName)
        {
            return (from barSpell in Spells where barSpell.Key == spellName select barSpell.Value).FirstOrDefault() != null;
        }

        public static Boolean HasItemById(Int32 itemId)
        {
            return BarItems.Any(a => a.ItemId.Equals(itemId));
        }

        public static BarSpell GetSpellById(Int32 spellId)
        {
            return (from spell in Spells where spell.Value.SpellId.Equals(spellId) select spell.Value).FirstOrDefault();
        }

        public static BarSpell GetSpellByName(String spellName)
        {
            return (from barSpell in Spells where barSpell.Key == spellName select barSpell.Value).FirstOrDefault() ?? new BarSpell(0, 0, 0, "Unknown Spell");
        }

        public static BarItem GetItemById(Int32 itemId)
        {
            return BarItems.FirstOrDefault(barItem => barItem.ItemId.Equals(itemId));
        }

        public static bool IsSpellReadyByName(string name)
        {
            if ((from barSpell in Spells where barSpell.Key == name select barSpell.Value).FirstOrDefault() != null)
            {
                return IsSpellReady(GetSpellByName(name).SpellId);
            }
            return false;
        }

        public static bool IsSpellReadyById(int id)
        {
            return IsSpellReady(id);
        }

        public static void CastSpell(string spellName)
        {
            BarSpell spell = (from barSpell in Spells where barSpell.Key == spellName select barSpell.Value).FirstOrDefault();
            if (spell != null)
            {
                Logging.Write("[Mapper]Casting " + spellName);
                spell.CastSpell();
            }
        }

        [DllImport("KERNEL32")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private static bool IsSpellReady(int spellidToCheck)
        {
            long frequency;
            long perfCount;
            QueryPerformanceFrequency(out frequency);
            QueryPerformanceCounter(out perfCount);
            //Current time in ms
            long currentTime = (perfCount*1000)/frequency;
            //Get first list object
            var currentListObject = Memory.ReadRelative<uint>((uint) Pointers.SpellCooldown.CooldPown + 0x8);
            while ((currentListObject != 0) && ((currentListObject & 1) == 0))
            {
                var spellId = Memory.Read<uint>(currentListObject + 0x8);

                if (spellId == spellidToCheck)
                {
                    //Start time of the spell cooldown in ms        
                    var startTime = Memory.Read<uint>(currentListObject + 0x10);
                    //Cooldown of spells with gcd
                    var cooldown1 = Memory.Read<int>(currentListObject + 0x14);
                    //Cooldown of spells without gcd
                    var cooldown2 = Memory.Read<int>(currentListObject + 0x20);
                    int cooldownLength = Math.Max(cooldown1, cooldown2);
                    if ((startTime + cooldownLength) > currentTime)
                    {
                        return false;
                    }
                }
                currentListObject = Memory.Read<uint>(currentListObject + 4);
            }
            return true;
        }

        /*
        internal static bool IsUsabelAction(int slot)
        {
            var foo = (uint)Pointers.ActionBar.IsUsableAction + (uint)slot * 0x04;
            var isUsabelAction = Memory.Read<int>(ObjectManager.BaseAddressModule + foo);
            foo = (uint)Pointers.ActionBar.IsUsableActionNoMana + (uint)slot * 0x04;
            var isUsabelActionNoMana = Memory.Read<int>(ObjectManager.BaseAddressModule + foo);

            if (isUsabelAction == 0 && isUsabelActionNoMana == 0)
            {
                Logging.Write("Slot: " + slot + " is usabel");
                return true;
            }
            Logging.Write("Slot: " + slot + " is not usabel");
            return false;
        }
         */

        public static bool HasBuff(PUnit check, string name)
        {
            List<int> ids = GetIdsFromName(name);
            if (ObjectManager.Initialized)
                if (check.HasBuff(ids))
                    return true;
            return false;
        }

        public static bool DoesBuffExist(string name)
        {
            if (GetIdsFromName(name).Count != 0)
            {
                return true;
            }
            return false;
        }
        
        public static int GetIdByName(string spellName)
        {
            var list = GetIdsFromName(spellName);
            if (list.Count != 0)
            {
                return list[0];
            }
            return 0;
        }
        
        public static int GetIdFromName(string name)
        {
            var list = GetIdsFromName(name);
            if (list.Count != 0)
            {
                return list[0];
            }
            return 0;
        }

        public static List<int> GetIdsFromName(string name)
        {
            Load();
            try
            {
                if (SpellsUsed.ContainsValue(name))
                {
                    return SpellsUsed.Where(spell => spell.Value == name).Select(spell => spell.Key).ToList();
                }
                List<int> idsFromName = _spellDatabase.Where(spell => spell.Value == name).Select(spell => spell.Key).ToList();
                foreach (var i in idsFromName)
                {
                    SpellsUsed.Add(i, name);
                }
                return idsFromName;
            }
            catch (Exception)
            {
                return new List<int> {0};
            }
        }
    }
}