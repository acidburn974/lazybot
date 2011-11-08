
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
using System.Reflection;

namespace LazyLib.Wow
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class Constants
    {
        #region ChatType enum

        /// <summary>
        ///   The different eChat types
        /// </summary>
        public enum ChatType : byte
        {
            Addon = 0,
            Say = 1,
            Party = 2,
            Raid = 3,
            Guild = 4,
            Officer = 5,
            Yell = 6,
            Whisper = 7,
            WhisperMob = 8,
            WhisperInform = 9,
            Emote = 10,
            TextEmote = 11,
            MonsterSay = 12,
            MonsterParty = 13,
            MonsterYell = 14,
            MonsterWhisper = 15,
            MonsterEmote = 16,
            Channel = 17,
            ChannelJoin = 18,
            ChannelLeave = 19,
            ChannelList = 20,
            ChannelNotice = 21,
            ChannelNoticeUser = 22,
            Afk = 23,
            Dnd = 24,
            Ignored = 25,
            Skill = 26,
            Loot = 27,
            //29
            //30
            //31
            //32
            //33
            //34
            //35
            //36
            //37
            //38
            BgEventNeutral = 35,
            BgEventAlliance = 36,
            BgEventHorde = 37,
            CombatFactionChange = 38,
            RaidLeader = 39,
            RaidWarning = 40,
            RaidWarningWidescreen = 41,
            //42
            Filtered = 43,
            Battleground = 44,
            BattlegroundLeader = 45,
            Restricted = 46,
            RealId = 53,
        } ;

        #endregion

        #region Classification enum

        public enum Classification
        {
            Normal = 0,
            Elite = 1,
            RareElite = 2,
            WorldBoss = 3,
            Rare = 4
        }

        #endregion

        #region CreatureType enum

        public enum CreatureType
        {
            Unknown = 0,
            Beast,
            Dragon,
            Demon,
            Elemental,
            Giant,
            Undead,
            Humanoid,
            Critter,
            Mechanical,
            NotSpecified,
            Totem,
            NonCombatPet,
            GasCloud
        }

        #endregion

        #region KeyType enum

        public enum KeyType : uint
        {
            Spell = 0,
            GeneralMacro = 64,
            ToonSpecificMacro = 65,
            Item = 128
        }

        #endregion

        #region ObjType enum

        public enum ObjType : uint
        {
            OT_NONE = 0,
            OT_ITEM = 1,
            OT_CONTAINER = 2,
            OT_UNIT = 3,
            OT_PLAYER = 4,
            OT_GAMEOBJ = 5,
            OT_DYNOBJ = 6,
            OT_CORPSE = 7,
            OT_FORCEDWORD = 0xFFFFFFFF
        }

        #endregion

        #region ObjectType enum

        public enum ObjectType : uint
        {
            Object = 0,
            Item = 1,
            Container = 2,
            Unit = 3,
            Player = 4,
            GameObject = 5,
            DynamicObject = 6,
            Corpse = 7,
            AiGroup = 8,
            AreaTrigger = 9
        }

        #endregion

        #region PlayerFactions enum

        public enum PlayerFactions : uint
        {
            Human = 1,
            Orc = 2,
            Dwarf = 3,
            NightElf = 4,
            Undead = 5,
            Tauren = 6,
            Gnome = 115,
            Troll = 116,
            BloodElf = 1610,
            Draenei = 1629,
            Worgen = 2203,
            Goblin = 2204,
        }

        #endregion

        #region ShapeshiftForm enum

        public enum ShapeshiftForm
        {
            Normal = 0,
            Cat = 1,
            TreeOfLife = 2,
            Travel = 3,
            Aqua = 4,
            Bear = 5,
            Ambient = 6,
            Ghoul = 7,
            DireBear = 8,
            CreatureBear = 14,
            CreatureCat = 15,
            GhostWolf = 16,
            BattleStance = 17,
            DefensiveStance = 18,
            BerserkerStance = 19,
            EpicFlightForm = 27,
            Shadow = 28,
            Stealth = 30,
            Moonkin = 31,
        }

        #endregion

        #region UnitClass enum

        public enum UnitClass
        {
            UnitClass_Unknown = 0,
            UnitClass_Warrior = 1,
            UnitClass_Paladin = 2,
            UnitClass_Hunter = 3,
            UnitClass_Rogue = 4,
            UnitClass_Priest = 5,
            UnitClass_DeathKnight = 6,
            UnitClass_Shaman = 7,
            UnitClass_Mage = 8,
            UnitClass_Warlock = 9,
            UnitClass_Druid = 11,
        }

        #endregion

        #region UnitDynamicFlags enum

        public enum UnitDynamicFlags
        {
            None = 0,
            Lootable = 0x1,
            TrackUnit = 0x2,
            TaggedByOther = 0x4,
            TaggedByMe = 0x8,
            SpecialInfo = 0x10,
            Dead = 0x20,
            ReferAFriendLinked = 0x40,
            IsTappedByAllThreatList = 0x80,
        }

        #endregion

        #region UnitFlags enum

        public enum UnitFlags : uint
        {
            None = 0,
            Sitting = 0x1,
            //SelectableNotAttackable_1 = 0x2,
            Influenced = 0x4, // Stops movement packets
            PlayerControlled = 0x8, // 2.4.2
            Totem = 0x10,
            Preparation = 0x20, // 3.0.3
            PlusMob = 0x40, // 3.0.2
            //SelectableNotAttackable_2 = 0x80,
            NotAttackable = 0x100,
            //Flag_0x200 = 0x200,
            Looting = 0x400,
            PetInCombat = 0x800, // 3.0.2
            PvPFlagged = 0x1000,
            Silenced = 0x2000, //3.0.3
            //Flag_14_0x4000 = 0x4000,
            //Flag_15_0x8000 = 0x8000,
            //SelectableNotAttackable_3 = 0x10000,
            Pacified = 0x20000, //3.0.3
            Stunned = 0x40000,
            CanPerformAction_Mask1 = 0x60000,
            Combat = 0x80000, // 3.1.1
            TaxiFlight = 0x100000, // 3.1.1
            Disarmed = 0x200000, // 3.1.1
            Confused = 0x400000, //  3.0.3
            Fleeing = 0x800000,
            Possessed = 0x1000000, // 3.1.1
            NotSelectable = 0x2000000,
            Skinnable = 0x4000000,
            Mounted = 0x8000000,
            //Flag_28_0x10000000 = 0x10000000,
            Dazed = 0x20000000,
            Sheathe = 0x40000000,
            //Flag_31_0x80000000 = 0x80000000,
        }

        #endregion

        #region UnitGender enum

        public enum UnitGender
        {
            UnitGender_Male = 0,
            UnitGender_Female = 1,
            UnitGender_Unknown = 2,
        }

        #endregion

        #region UnitPower enum

        public enum UnitPower
        {
            UnitPower_Mana = 0,
            UnitPower_Rage = 1,
            UnitPower_Focus = 2,
            UnitPower_Energy = 3,
            UnitPower_Runes = 5,
            UnitPower_RunicPower = 6,
            UnitPower_SoulShard = 8,
            UnitPower_Eclipse = 9,
            UnitPower_HolyPower = 10,
            UnitPower_Max = 7,
        }

        #endregion

        #region UnitRace enum

        public enum UnitRace
        {
            UnitRace_Human = 1,
            UnitRace_Orc,
            UnitRace_Dwarf,
            UnitRace_NightElf,
            UnitRace_Undead,
            UnitRace_Tauren,
            UnitRace_Gnome,
            UnitRace_Troll,
            UnitRace_Goblin,
            UnitRace_BloodElf,
            UnitRace_Draenei,
            UnitRace_FelOrc,
            UnitRace_Naga,
            UnitRace_Broken,
            UnitRace_Skeleton = 15,
        }

        #endregion
    }
}