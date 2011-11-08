
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
using System.Threading;
using LazyLib.ActionBar;
using LazyLib.Helpers;

#endregion

namespace LazyLib.Wow
{
    /// <summary>
    ///   Representing a unit ingame
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class PUnit : PObject
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "PUnit" /> class.
        /// </summary>
        /// <param name = "baseAddress">The base address.</param>
        public PUnit(uint baseAddress)
            : base(baseAddress)
        {
        }

        /// <summary>
        ///   Determines whether [is facing away].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is facing away]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsFacingAway
        {
            get
            {
                if (MoveHelper.NegativeAngle(Facing - ObjectManager.MyPlayer.Facing) > 5.5 ||
                    MoveHelper.NegativeAngle(Facing - ObjectManager.MyPlayer.Facing) < 0.6)
                    return true;
                return false;
            }
        }

        /// <summary>
        ///   Gets the facing PI.
        /// </summary>
        /// <value>The facing PI.</value>
        public double FacingPI
        {
            get
            {
                float wowFacing =
                    MoveHelper.NegativeAngle(
                        (float) Math.Atan2((Y - ObjectManager.MyPlayer.Y), (X - ObjectManager.MyPlayer.X)));
                return wowFacing;
            }
        }

        /*
        /// <summary>
        ///   Gets a value indicating whether this unit is not dying.
        /// </summary>
        /// <remarks>
        ///   If the mob does not use health for 9 seconds this returns true
        /// </remarks>
        /// <value>
        ///   <c>true</c> if this instance is not dying; otherwise, <c>false</c>.
        /// </value>
        public bool IsNotDying
        {
            get
            {
                if (_isNotDuying == null)
                {
                    _isNotDuying = new Ticker(10000*12);
                }
                if (_oldHealth != Health)
                {
                    _oldHealth = Health;
                    _isNotDuying.Reset();
                }
                if (_isNotDuying.IsReady)
                    return true;
                return false;
            }
        } */

        /// <summary>
        ///   Gets the type of the power.
        /// </summary>
        /// <value>The type of the power.</value>
        public string PowerType
        {
            get
            {
                switch (PowerTypeId)
                {
                    case (uint) Constants.UnitPower.UnitPower_Energy:
                        return "Energy";
                    case (uint) Constants.UnitPower.UnitPower_Focus:
                        return "Focus";
                    case (uint) Constants.UnitPower.UnitPower_Mana:
                        return "Mana";
                    case (uint) Constants.UnitPower.UnitPower_Rage:
                        return "Rage";
                    case (uint) Constants.UnitPower.UnitPower_RunicPower:
                        return "Runic Power";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        ///   Gets the unit race.
        /// </summary>
        /// <value>The unit race.</value>
        public string UnitRace
        {
            get
            {
                string race;
                switch (RaceId)
                {
                    case (uint) Constants.UnitRace.UnitRace_Human:
                        race = @"Human";
                        break;
                    case (uint) Constants.UnitRace.UnitRace_Orc:
                        race = @"Orc";
                        break;
                    case (uint) Constants.UnitRace.UnitRace_Dwarf:
                        race = @"Dwarf";
                        break;
                    case (uint) Constants.UnitRace.UnitRace_NightElf:
                        race = @"Night Elf";
                        break;
                    case (uint) Constants.UnitRace.UnitRace_Undead:
                        race = @"Undead";
                        break;
                    case (uint) Constants.UnitRace.UnitRace_Tauren:
                        race = @"Tauren";
                        break;
                    case (uint) Constants.UnitRace.UnitRace_Gnome:
                        race = @"Gnome";
                        break;
                    case (uint) Constants.UnitRace.UnitRace_Troll:
                        race = @"Troll";
                        break;
                    case (uint) Constants.UnitRace.UnitRace_Goblin:
                        race = @"Goblin";
                        break;
                    case (uint) Constants.UnitRace.UnitRace_BloodElf:
                        race = @"Blood Elf";
                        break;
                    case (uint) Constants.UnitRace.UnitRace_Draenei:
                        race = @"Draenei";
                        break;
                    case (uint) Constants.UnitRace.UnitRace_FelOrc:
                        race = @"Fel Orc";
                        break;
                    case (uint) Constants.UnitRace.UnitRace_Naga:
                        race = @"Naga";
                        break;
                    case (uint) Constants.UnitRace.UnitRace_Broken:
                        race = @"Broken";
                        break;
                    case (uint) Constants.UnitRace.UnitRace_Skeleton:
                        race = @"Skeleton";
                        break;
                    default:
                        race = @"Unknown";
                        break;
                }
                return race;
            }
        }

        /// <summary>
        /// Returns the object's speed.
        /// </summary>
        public Single Speed
        {
            get
            {
                var pointer = Memory.Read<uint>(BaseAddress + (uint) Pointers.UnitSpeed.Pointer1);
                var speed = Memory.Read<float>(pointer + (uint) Pointers.UnitSpeed.Pointer2);
                return speed;
            }
        }

        /// <summary>
        /// Returns True if unit is moving, else False.
        /// </summary>
        public bool IsMoving
        {
            get { return (Speed > 0); }
        }

        /// <summary>
        ///   Gets the gender.
        /// </summary>
        /// <value>The gender.</value>
        public string Gender
        {
            get
            {
                string gender;
                switch (GenderId)
                {
                    case (uint) Constants.UnitGender.UnitGender_Male:
                        gender = @"Male";
                        break;
                    case (uint) Constants.UnitGender.UnitGender_Female:
                        gender = @"Female";
                        break;
                    default:
                        gender = @"Unknown";
                        break;
                }
                return gender;
            }
        }

        private uint InfoFlags
        {
            get
            {
                try
                {
                    return GetStorageField<UInt32>((uint) Descriptors.eUnitFields.UNIT_FIELD_BYTES_0);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public uint RaceId
        {
            get { return ((InfoFlags >> 0) & 0xFF); }
        }

        public uint UnitClassId
        {
            get { return ((InfoFlags >> 8) & 0xFF); }
        }

        public uint GenderId
        {
            get { return ((InfoFlags >> 16) & 0xFF); }
        }

        public uint PowerTypeId
        {
            get { return ((InfoFlags >> 24) & 0xFF); }
        }

        /// <summary>
        ///   Gets the class.
        /// </summary>
        /// <value>The class.</value>
        public string Class
        {
            get
            {
                string stringClass;
                switch (UnitClassId)
                {
                    case (uint) Constants.UnitClass.UnitClass_Warrior:
                        stringClass = @"Warrior";
                        break;
                    case (uint) Constants.UnitClass.UnitClass_Paladin:
                        stringClass = @"Paladin";
                        break;
                    case (uint) Constants.UnitClass.UnitClass_Hunter:
                        stringClass = @"Hunter";
                        break;
                    case (uint) Constants.UnitClass.UnitClass_Rogue:
                        stringClass = @"Rogue";
                        break;
                    case (uint) Constants.UnitClass.UnitClass_Priest:
                        stringClass = @"Priest";
                        break;
                    case (uint) Constants.UnitClass.UnitClass_Shaman:
                        stringClass = @"Shaman";
                        break;
                    case (uint) Constants.UnitClass.UnitClass_Mage:
                        stringClass = @"Mage";
                        break;
                    case (uint) Constants.UnitClass.UnitClass_Warlock:
                        stringClass = @"Warlock";
                        break;
                    case (uint) Constants.UnitClass.UnitClass_Druid:
                        stringClass = @"Druid";
                        break;
                    case (uint) Constants.UnitClass.UnitClass_DeathKnight:
                        stringClass = @"Death Knight";
                        break;
                    default:
                        stringClass = @"Unknown";
                        break;
                }
                return stringClass;
            }
        }

        public Constants.UnitClass UnitClass
        {
            get
            {
                switch (UnitClassId)
                {
                    case (uint)Constants.UnitClass.UnitClass_Warrior:
                        return Constants.UnitClass.UnitClass_Warrior;
                    case (uint)Constants.UnitClass.UnitClass_Paladin:
                        return Constants.UnitClass.UnitClass_Paladin;
                    case (uint)Constants.UnitClass.UnitClass_Hunter:
                        return Constants.UnitClass.UnitClass_Hunter;
                    case (uint)Constants.UnitClass.UnitClass_Rogue:
                        return Constants.UnitClass.UnitClass_Rogue;
                    case (uint)Constants.UnitClass.UnitClass_Priest:
                        return Constants.UnitClass.UnitClass_Priest;
                    case (uint)Constants.UnitClass.UnitClass_Shaman:
                        return Constants.UnitClass.UnitClass_Shaman;
                    case (uint)Constants.UnitClass.UnitClass_Mage:
                        return Constants.UnitClass.UnitClass_Mage;
                    case (uint)Constants.UnitClass.UnitClass_Warlock:
                        return Constants.UnitClass.UnitClass_Warlock;
                    case (uint)Constants.UnitClass.UnitClass_Druid:
                        return Constants.UnitClass.UnitClass_Druid;
                    case (uint)Constants.UnitClass.UnitClass_DeathKnight:
                        return Constants.UnitClass.UnitClass_DeathKnight;
                    default:
                        throw new Exception("Unknown class");
                }
            }
        }

        /// <summary>
        /// Return the unit classification
        /// </summary>
        /// <value>The classification.</value>
        public Constants.Classification Classification
        {
            get
            {
                var v1 = Memory.Read<uint>(BaseAddress + (uint) Pointers.CgUnitCGetCreatureRank.Offset1);
                return
                    (Constants.Classification) Memory.Read<uint>(v1 + (uint) Pointers.CgUnitCGetCreatureRank.Offset2);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is elite.
        /// </summary>
        /// <value><c>true</c> if this instance is elite; otherwise, <c>false</c>.</value>
        public bool IsElite
        {
            get { return Classification.Equals(Constants.Classification.Elite); }
        }

        /// <summary>
        /// Return the unit type
        /// </summary>
        /// <value>The type of the creature.</value>
        public Constants.CreatureType CreatureType
        {
            get
            {
                var v4 = Memory.Read<uint>(BaseAddress + (uint) Pointers.CgUnitCGetCreatureType.Offset1);
                return
                    (Constants.CreatureType) Memory.Read<uint>(v4 + (uint) Pointers.CgUnitCGetCreatureType.Offset2);
            }
        }

        /// <summary>
        ///   Gets the reaction.
        /// </summary>
        /// <value>The reaction.</value>
        public Reaction Reaction
        {
            get { return Wow.Faction.GetReaction(ObjectManager.MyPlayer, this); }
        }

        /// <summary>
        ///   Gets a value indicating whether this unit is a player.
        /// </summary>
        /// <value><c>true</c> if this instance is player; otherwise, <c>false</c>.</value>
        public bool IsPlayer
        {
            get { return ObjectManager.GetPlayers.Any(player => player.GUID.Equals(GUID)); }
        }

        /// <summary>
        /// Returns the current ShapeshiftForm.
        /// </summary>
        public Constants.ShapeshiftForm ShapeshiftForm
        {
            get
            {
                return
                    (Constants.ShapeshiftForm)
                    Memory.Read<byte>(
                        Memory.Read<uint>(BaseAddress + (uint) Pointers.ShapeshiftForm.BaseAddressOffset1) +
                        (uint) Pointers.ShapeshiftForm.BaseAddressOffset2);
            }
        }

        /// <summary>
        ///   Determines whether the specified unit is pet.
        /// </summary>
        /// <value><c>true</c> if this instance is pet; otherwise, <c>false</c>.</value>
        /// <returns>
        ///   <c>true</c> if the specified unit is pet; otherwise, <c>false</c>.
        /// </returns>
        public bool IsPet
        {
            get
            {
                try
                {
                    return ObjectManager.GetPlayers.Where(cur => cur.HasLivePet).Any(cur => cur.PetGUID == GUID);
                } catch
                {
                    return false;
                }
            }
        }

        public bool IsTotem
        {
             get { return CreatureType == Constants.CreatureType.Totem; }
        }

        /// <summary>
        ///   Retuns the current Target of the unit
        ///   Return a new PUnit if null, you can check if the PUnit is valid using the IsValid property.
        /// </summary>
        public virtual PUnit Target
        {
            get
            {
                try
                {
                    if (TargetGUID.Equals(ObjectManager.MyPlayer.GUID))
                        return ObjectManager.MyPlayer;
                    foreach (PUnit u in ObjectManager.GetUnits)
                    {
                        try
                        {
                            if (u.GUID.Equals(TargetGUID))
                                return u;
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
                return new PUnit(uint.MinValue);
            }
        }

        public bool HasTarget
        {
             get
             {
                 try
                 {
                     if (TargetGUID.Equals(ObjectManager.MyPlayer.GUID))
                         return true;
                     if (ObjectManager.GetUnits.Any(u => u.GUID.Equals(TargetGUID)))
                     {
                         return true;
                     }
                 }
                 catch (Exception)
                 {
                 }
                 return false;
             }
        }

        /// <summary>
        ///   Gets a value indicating whether this unit is in flight form.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this unit is in flight form; otherwise, <c>false</c>.
        /// </value>
        public bool IsInFlightForm
        {
            get { return HasBuff(33943) || HasBuff(40120) || AquaticForm; }
        }

        public bool TravelForm
        {
            get { return HasBuff(783) || HasBuff(2645); }
        }

        public bool AquaticForm
        {
            get { return HasBuff(1066) || ShapeshiftForm.Equals(Constants.ShapeshiftForm.Aqua); }
        }

        /// <summary>
        ///   Gets a value indicating whether this instance is mounted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is mounted; otherwise, <c>false</c>.
        /// </value>
        public bool IsMounted
        {
            get
            {
                try
                {
                    if (IsInFlightForm)
                        return true;
                    if (TravelForm)
                        return true;
                    if (AquaticForm)
                        return true;
                    var mountid = GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_MOUNTDISPLAYID);
                    return (mountid != 0);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///   True if this unit is lootable right now.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is lootable; otherwise, <c>false</c>.
        /// </value>
        public bool IsLootable
        {
            get
            {
                try
                {
                    switch (GetDynFlags)
                    {
                        case 1:
                        case 13:
                            return true;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///   True if this monster has been tagged by another Player
        /// </summary>
        /// <value><c>true</c> if this instance is tagged; otherwise, <c>false</c>.</value>
        public bool IsTagged
        {
            get
            {
                if (GetDynFlags == 4)
                    return true;
                return false;
            }
        }

        /// <summary>
        ///   True if this monster has been tagged by you
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is tagged by me; otherwise, <c>false</c>.
        /// </value>
        public bool IsTaggedByMe
        {
            get
            {
                if (GetDynFlags == 8 || GetDynFlags == 13 || GetDynFlags == 1 || GetDynFlags == 12)
                    return true;
                return false;
            }
        }


        /*
        The Flags can be seen as:
        01: has loot
        02: ???
        04: locked to a Player
        08: locked to you
        If the function returns 13 (01 * 04 * 08) the mob is locked to you and is lootable.
        If it is returns 12 (04 & 08) you are either still fighting it, or you have already looted it.
        */

        /// <summary>
        ///   Gets the get dyn flags.
        /// </summary>
        /// <value>The get dyn flags.</value>
        public int GetDynFlags
        {
            get
            {
                try
                {
                    return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_DYNAMIC_FLAGS);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///   Returns true if the unit is in combat
        /// </summary>
        /// <value><c>true</c> if [in combat]; otherwise, <c>false</c>.</value>
        public bool IsInCombat
        {
            get { return Convert.ToBoolean(Flags & (uint) Constants.UnitFlags.Combat); }
        }


        /// <summary>
        ///   Gets a value indicating whether this unit is fleeing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is fleeing; otherwise, <c>false</c>.
        /// </value>
        public bool IsFleeing
        {
            get { return Convert.ToBoolean(Flags & (uint)Constants.UnitFlags.Fleeing); }
        }

        /// <summary>
        ///   Gets a value indicating whether this unit is stunned.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is stunned; otherwise, <c>false</c>.
        /// </value>
        public bool IsStunned
        {
            get { return Convert.ToBoolean(Flags & (uint)Constants.UnitFlags.Stunned); }
        }

        /// <summary>
        ///   Returns true if auto attack in enabled
        /// </summary>
        /// <value><c>true</c> if [auto attack]; otherwise, <c>false</c>.</value>
        public bool IsAutoAttacking
        {
            get
            {
                return ((Memory.Read<int>(BaseAddress + (uint) Pointers.AutoAttack.AutoAttackFlag)) |
                        Memory.Read<int>(BaseAddress + (uint) Pointers.AutoAttack.AutoAttackMask)) != 0;
            }
        }

        /// <summary>
        ///   Returns true if swimming
        /// </summary>
        /// <value><c>true</c> if [swimming]; otherwise, <c>false</c>.</value>
        public bool IsSwimming
        {
            get
            {
                var p1 = Memory.Read<uint>(BaseAddress + (uint)Pointers.Swimming.Pointer);
                var p2 = Memory.Read<uint>(p1 + (uint)Pointers.Swimming.Offset);
                return (p2 & (uint)Pointers.Swimming.Mask) != 0;
            }
        }

        /// <summary>
        /// Return True if unit is Flying, else False.
        /// </summary>
        public bool IsFlying
        {
            get
            {
                var p1 = Memory.Read<uint>(BaseAddress + (uint) Pointers.IsFlying.Pointer);
                var p2 = Memory.Read<uint>(p1 + (uint) Pointers.IsFlying.Offset);
                return (p2 & (uint) Pointers.IsFlying.Mask) != 0;
            }
        }

        /// <summary>
        ///   Returns true if the unit is Skinnable
        /// </summary>
        /// <value><c>true</c> if skinnable; otherwise, <c>false</c>.</value>
        public bool IsSkinnable
        {
            get { return Convert.ToBoolean(Flags & 0x4000000); }
        }

        ///<summary>
        ///  Returns the current unit field flag.
        ///</summary>
        ///<value>
        ///  8: PVP Enabled
        ///  10: totem?!
        ///  40: elite? 
        ///  800: fighing
        ///  1000: in pvp
        ///  8000: ???
        ///  40000: immobile (Player dead / stunned = C0000)
        ///  80000:  in melee
        ///  4000000: Skinnable
        ///  20000000: dazed
        ///</value>
        private long Flags
        {
            get
            {
                try
                {
                    return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_FLAGS);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///   Faction template id of this unit
        /// </summary>
        /// <value>The faction.</value>
        public uint Faction
        {
            get
            {
                try
                {
                    return GetStorageField<UInt32>((uint) Descriptors.eUnitFields.UNIT_FIELD_FACTIONTEMPLATE);
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///   True if unit is ghost
        /// </summary>
        /// <value><c>true</c> if this instance is ghost; otherwise, <c>false</c>.</value>
        public bool IsGhost
        {
            get
            {
                if (HealthPoints == 1)
                    return true;
                return false;
            }
        }

        /// <summary>
        ///   Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name
        {
            get
            {
                try
                {
                    return
                        Memory.ReadUtf8(
                            Memory.Read<uint>(
                                Memory.Read<uint>(BaseAddress + (uint) Pointers.UnitName.UnitName1) +
                                (uint) Pointers.UnitName.UnitName2), 256);
                }
                catch (Exception)
                {
                }
                return "Read failed";
            }
        }

        /// <summary>
        ///   Checks if Unit is casting and returns an int of the spellID being cast
        /// </summary>
        public int CastingId
        {
            get
            {
                try
                {
                    uint pointer = BaseAddress + (uint) Pointers.CastingInfo.IsCasting;
                    //Log.log(pointer.ToString());
                    return Memory.Read<int>(pointer);
                }
                catch (Exception)
                {
                }
                return 0;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether this unit is casting.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this unit is casting; otherwise, <c>false</c>.
        /// </value>
        public bool IsCasting
        {
            get
            {
                if (CastingId == 0 && ChanneledCastingId == 0)
                    return false;
                return true;
            }
        }

        /// <summary>
        ///   Gets the channeled casting id.
        /// </summary>
        /// <value>The channeled casting id.</value>
        public int ChanneledCastingId
        {
            get
            {
                uint pointer = BaseAddress + (uint) Pointers.CastingInfo.ChanneledCasting;
                //Log.log(pointer.ToString());
                return Memory.Read<int>(pointer);
            }
        }

        /// <summary>
        ///   Is this unit a critter?
        /// </summary>
        public bool Critter
        {
            get { return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_CRITTER) == 1 ? true : false; }
        }

        /// <summary>
        ///   The GUID of the object this unit is charmed by.
        /// </summary>
        public ulong CharmedBy
        {
            get { return GetStorageField<ulong>((uint) Descriptors.eUnitFields.UNIT_FIELD_CHARMEDBY); }
        }

        /// <summary>
        ///   The GUID of the object this unit is summoned by.
        /// </summary>
        public ulong SummonedBy
        {
            get { return GetStorageField<ulong>((uint) Descriptors.eUnitFields.UNIT_FIELD_SUMMONEDBY); }
        }

        /// <summary>
        ///   The GUID of the object this unit was created by.
        /// </summary>
        public ulong CreatedBy
        {
            get { return GetStorageField<ulong>((uint) Descriptors.eUnitFields.UNIT_FIELD_CREATEDBY); }
        }

        /// <summary>
        ///   The unit's health.
        /// </summary>
        public int HealthPoints
        {
            get { return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_HEALTH); }
        }

        /// <summary>
        ///   The unit's maximum health.
        /// </summary>
        public int MaximumHealthPoints
        {
            get { return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_MAXHEALTH); }
        }

        /// <summary>
        ///   True if this unit is dead
        /// </summary>
        /// <value><c>true</c> if this instance is dead; otherwise, <c>false</c>.</value>
        public bool IsDead
        {
            get { return !IsAlive; }
        }

        public bool IsAlive
        {
            get
            {
                if (HealthPoints == 0)
                    return false;
                if (HasBuff(new List<int>() { 8326, 9036, 20584 }))
                    return false;
                return true;
            }
        }


        /// <summary>
        ///   The unit's health as a percentage.
        /// </summary>
        public int Health
        {
            get
            {
                try
                {
                    return (100*HealthPoints)/MaximumHealthPoints;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///   The unit's health as a percentage.
        /// </summary>
        public int Mana
        {
            get
            {
                try
                {
                    return (100*ManaPoints)/MaximumManaPoints;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///   The unit's base health.
        /// </summary>
        public int BaseHealth
        {
            get { return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_BASE_HEALTH); }
        }

        /// <summary>
        ///   The unit's base health.
        /// </summary>
        public int BaseMana
        {
            get { return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_BASE_MANA); }
        }

        /// <summary>
        ///   The unit's mana.
        /// </summary>
        public int ManaPoints
        {
            get { return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_POWER1); }
        }

        /// <summary>
        ///   The unit's rage.
        /// </summary>
        public int Rage
        {
            get
            {
                try
                {
                    if (UnitClass == Constants.UnitClass.UnitClass_Druid)
                    {
                        return DruidRage;
                    }
                }
                catch (Exception e)
                {
                    
                }
                return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_POWER1)/10;
            }
        }

        /// <summary>
        ///   The unit's focus.
        /// </summary>
        public int Focus
        {
            get { return GetStorageField<int>((uint)Descriptors.eUnitFields.UNIT_FIELD_POWER1); }
        }

        /// <summary>
        ///   The unit's Eclipse power.
        /// </summary>
        public int Eclipse
        {
            get { return GetStorageField<int>((uint)Descriptors.eUnitFields.UNIT_FIELD_POWER4); }
        }

        /// <summary>
        ///   The unit's Max Eclipse power.
        /// </summary>
        public int MaximumEclipse
        {
            get { return GetStorageField<int>((uint)Descriptors.eUnitFields.UNIT_FIELD_MAXPOWER4); }
        }

        /// <summary>
        ///   The unit's Soul Shards.
        /// </summary>
        public int SoulShard
        {
            get { return GetStorageField<int>((uint)Descriptors.eUnitFields.UNIT_FIELD_POWER2); }
        }

        /// <summary>
        ///   The unit's Max Soul Shards.
        /// </summary>
        public int MaximumSoulShard
        {
            get { return GetStorageField<int>((uint)Descriptors.eUnitFields.UNIT_FIELD_MAXPOWER2); }
        }

        /// <summary>
        ///   The unit's Holy Power.
        /// </summary>
        public int HolyPower
        {
            get { return GetStorageField<int>((uint)Descriptors.eUnitFields.UNIT_FIELD_POWER2); }
        }

        /// <summary>
        ///   The unit's Max Holy Power.
        /// </summary>
        public int MaximumHolyPower
        {
            get { return GetStorageField<int>((uint)Descriptors.eUnitFields.UNIT_FIELD_MAXPOWER2); }
        }

        /// <summary>
        ///   The unit's energy.
        /// </summary>
        public int Energy
        {
            get
            {
                try
                {
                    if(UnitClass == Constants.UnitClass.UnitClass_Druid)
                    {
                        return DruidEnergy;
                    }
                }
                catch (Exception e)
                {
                    
                }
                return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_POWER1);
            }
        }

        private int DruidEnergy
        {
            get
            {
                return GetStorageField<int>((uint)Descriptors.eUnitFields.UNIT_FIELD_POWER3);
            }
        }

        private int DruidEnergyMax
        {
            get
            {
                return GetStorageField<int>((uint)Descriptors.eUnitFields.UNIT_FIELD_MAXPOWER3);
            }
        }

        private int DruidRage
        {
            get
            {
                return GetStorageField<int>((uint)Descriptors.eUnitFields.UNIT_FIELD_POWER2) / 10;
            }
        }

        private int DruidRageMax
        {
            get
            {
                return GetStorageField<int>((uint)Descriptors.eUnitFields.UNIT_FIELD_MAXPOWER5);
            }
        }

        /// <summary>
        ///   The unit's happinnes.
        /// </summary>
        public int Happinnes
        {
            get { return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_POWER4); }
        }

        /// <summary>
        ///   The unit's runic power.
        /// </summary>
        public int RunicPower
        {
            get { return GetStorageField<int>((uint)Descriptors.eUnitFields.UNIT_FIELD_POWER1) / 10; }
        }

        /// <summary>
        ///   The unit's maximum mana.
        /// </summary>
        public int MaximumManaPoints
        {
            get { return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_MAXPOWER1); }
        }

        /// <summary>
        ///   The unit's maximum rage.
        /// </summary>
        public int MaximumRage
        {
            get
            {
                try
                {
                    if (UnitClass == Constants.UnitClass.UnitClass_Druid)
                    {
                        return DruidRageMax;
                    }
                }
                catch (Exception e)
                {
                    
                }
                return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_MAXPOWER1);
            }
        }

        /// <summary>
        ///   The unit's maximum focus.
        /// </summary>
        public int MaximumFocus
        {
            get { return GetStorageField<int>((uint)Descriptors.eUnitFields.UNIT_FIELD_MAXPOWER1); }
        }

        /// <summary>
        ///   The unit's maximum energy.
        /// </summary>
        public int MaximumEnergy
        {
            get
            {
                try
                {
                    if (UnitClass == Constants.UnitClass.UnitClass_Druid)
                    {
                        return DruidEnergyMax;
                    }
                }
                catch (Exception e)
                {
                    
                }
                return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_MAXPOWER1);
            }
        }

        /// <summary>
        ///   The unit's maximum runic power.
        /// </summary>
        public int MaximumRunicPower
        {
            get { return GetStorageField<int>((uint)Descriptors.eUnitFields.UNIT_FIELD_MAXPOWER1); }
        }

        /// <summary>
        ///   The unit's level.
        /// </summary>
        public new int Level
        {
            get { return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_LEVEL); }
        }

        /// <summary>
        ///   The unit's DisplayID.
        /// </summary>
        public int DisplayId
        {
            get { return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_DISPLAYID); }
        }

        /// <summary>
        ///   The mount display of the mount the unit is mounted on.
        /// </summary>
        public int MountDisplayId
        {
            get { return GetStorageField<int>((uint) Descriptors.eUnitFields.UNIT_FIELD_MOUNTDISPLAYID); }
        }

        /// <summary>
        ///   The GUID of the object this unit is targeting.
        /// </summary>
        public ulong TargetGUID
        {
            get { return GetStorageField<ulong>((uint) Descriptors.eUnitFields.UNIT_FIELD_TARGET); }
        }

        /// <summary>
        ///   True if this unit is targeting me.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is targeting me; otherwise, <c>false</c>.
        /// </value>
        public bool IsTargetingMe
        {
            get
            {
                if (Target != null && Target.TargetGUID.Equals(ObjectManager.MyPlayer.GUID))
                    return true;
                return false;
            }
        }

        /// <summary>
        ///   True if I have a pet and this unit is targeting it.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is targeting my pet; otherwise, <c>false</c>.
        /// </value>
        public bool IsTargetingMyPet
        {
            get
            {
                if (!ObjectManager.MyPlayer.HasLivePet) return false;
                if (Target != null && Target.TargetGUID.Equals(ObjectManager.MyPlayer.TargetGUID))
                    return true;
                return false;
            }
        }

        /// <summary>
        ///   Returns the GUID of our pet
        /// </summary>
        /// <remarks>
        ///   Does not look at non combat pets
        /// </remarks>
        public virtual ulong PetGUID
        {
            get
            {
                try
                {
                    if (HasLivePet)
                    {
                        return Pet.GUID;
                    }
                } catch { }
                return 0;
            }
        }

        /// <summary>
        ///   Returns true if one of the objects is summond by the Player
        /// </summary>
        /// <remarks>
        ///   Does not look at non combat pets!
        /// </remarks>
        public bool HasLivePet
        {
            get
            {
                try
                {
                    if (Pet != null)
                        return true;
                    return false;
                } catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///   Returns our pet
        /// </summary>
        /// <remarks>
        ///   Does not return non combat pets!
        /// </remarks>
        public PUnit Pet
        {
            get
            {
                try
                {
                    foreach (PUnit obj in ObjectManager.GetObjects.OfType<PUnit>())
                    {
                        if (obj.SummonedBy.Equals(GUID))
                        {
                            return obj;
                        }
                    }
                }
                catch (Exception)
                {
                }
                return null;
            }
        }

        /// <summary>
        ///   Gets the distance to self.
        /// </summary>
        /// <value>The distance to self.</value>
        public double DistanceToSelf
        {
            get { return Location.DistanceToSelf; }
        } 
        /// <summary>
        /// Determines whether [has well known buff] [the specified buff name].
        /// </summary>
        /// <param name="buffName">Name of the buff.</param>
        /// <returns>
        /// 	<c>true</c> if [has well known buff] [the specified buff name]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasWellKnownBuff(string buffName)
        {
            return HasBuff(BarMapper.GetIdByName(buffName));
        }

        public bool HasWellKnownBuff(string buffName, bool playerIsOwner)
        {
            return HasBuff(BarMapper.GetIdByName(buffName), playerIsOwner);
        }

        /// <summary>
        ///   Check to see if this unit currently has the specified buff
        /// </summary>
        /// <param name = "buff">Buff id's.</param>
        /// <returns>
        ///   <c>true</c> if the specified unit has the buff; otherwise, <c>false</c>.
        /// </returns>
        public bool HasBuff(int[] buff)
        {
            List<int> auras = GetBuffs();
            try
            {
                if (buff.Any(u => auras.Contains(u)))
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        ///   Check to see if this unit currently has the specified buff
        /// </summary>
        /// <param name = "buff">Buff id.</param>
        /// <returns>
        ///   <c>true</c> if the specified unit has the buff; otherwise, <c>false</c>.
        /// </returns>
        public bool HasBuff(int buff)
        {
            List<int> auras = GetBuffs();
            if (auras.Contains(buff)) return true;
            return false;
        }

        public bool HasBuff(int buff, bool playerShouldBeOwner)
        {
            if (!playerShouldBeOwner)
            {
                return HasBuff(buff);
            }
            try
            {
                var auras = GetAuras;
                return auras.Any(woWAura => buff == woWAura.SpellId && (woWAura.OwnerGUID == ObjectManager.MyPlayer.GUID) || (woWAura.OwnerGUID == ObjectManager.MyPlayer.PetGUID));
            } catch
            {
                return false;
            }
        }

        public bool HasBuff(string buff)
        {
            List<int> auras = GetBuffs();
            List<int> buf = BarMapper.GetIdsFromName(buff);
            if (auras.Any(buf.Contains)) 
                return true;
            return false;
        }

        public bool HasBuff(string buff, bool playerShouldBeOwner)
        {
            if(!playerShouldBeOwner)
            {
                return HasBuff(buff);
            }
            var auras = GetAuras;
            List<int> buf = BarMapper.GetIdsFromName(buff);
            return auras.Any(woWAura => buf.Contains(woWAura.SpellId) && (woWAura.OwnerGUID == ObjectManager.MyPlayer.GUID) || (woWAura.OwnerGUID == ObjectManager.MyPlayer.PetGUID));
        }

        /// <summary>
        ///   Check to see if this unit currently has the specified buff
        /// </summary>
        /// <param name = "buffs">Buff list.</param>
        /// <returns>
        ///   <c>true</c> if the specified unit has the buff; otherwise, <c>false</c>.
        /// </returns>
        public bool HasBuff(List<int> buffs)
        {
            try
            {
                if (buffs.Any(HasBuff))
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }


        [DllImport("KERNEL32")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);
        public uint BuffTimeLeft(int spellId)
        {
            try
            {
                return (from woWAura in GetAuras where woWAura.SpellId == spellId select woWAura.SecondsLeft).FirstOrDefault();
            }
            catch 
            {
                return 0;
            }
        }

        public IEnumerable<WoWAura> GetAuras
        {
            get
            {
                var auraCount = Memory.Read<int>(BaseAddress + (uint)Pointers.UnitAuras.AuraCount1);
                if (auraCount == -1)
                {
                    auraCount = Memory.Read<int>(BaseAddress + (uint)Pointers.UnitAuras.AuraCount2);
                }
                var result = new List<WoWAura>();
                long frequency;
                long perfCount;
                QueryPerformanceFrequency(out frequency);
                QueryPerformanceCounter(out perfCount);
                long currentTime = (perfCount * 1000) / frequency;
                //Current time in ms
                for (uint i = 0; i < auraCount; i++)
                {
                    int localSpellId;
                    byte stackCount;
                    uint timeLeft;
                    ulong ownerGuid;
                    if (Memory.Read<int>(BaseAddress + (uint)Pointers.UnitAuras.AuraCount1) == -1)
                    {
                        var auraTable = Memory.Read<uint>(BaseAddress + (uint)Pointers.UnitAuras.AuraTable2);
                        localSpellId = Memory.Read<int>(auraTable + (uint)Pointers.UnitAuras.AuraSize * i + (int)Pointers.UnitAuras.AuraSpellId);
                        stackCount = Memory.Read<byte>((auraTable + ((uint) Pointers.UnitAuras.AuraSize*i)) + (uint) Pointers.UnitAuras.AuraStack);
                        timeLeft = Memory.Read<uint>((auraTable + ((uint) Pointers.UnitAuras.AuraSize*i)) + (uint)Pointers.UnitAuras.TimeLeft);
                        ownerGuid = Memory.Read<ulong>(auraTable + (uint)Pointers.UnitAuras.AuraSize * i);
                    } else
                    {
                        localSpellId = Memory.Read<int>(BaseAddress + (uint)Pointers.UnitAuras.AuraTable1 + (uint)Pointers.UnitAuras.AuraSize * i + (int)Pointers.UnitAuras.AuraSpellId);
                        stackCount = Memory.Read<byte>((BaseAddress + (uint)Pointers.UnitAuras.AuraTable1 + ((uint)Pointers.UnitAuras.AuraSize * i)) + (uint)Pointers.UnitAuras.AuraStack);
                        timeLeft = Memory.Read<uint>((BaseAddress + (uint)Pointers.UnitAuras.AuraTable1 + ((uint)Pointers.UnitAuras.AuraSize * i)) + (uint)Pointers.UnitAuras.TimeLeft);
                        ownerGuid = Memory.Read<ulong>((BaseAddress + (uint)Pointers.UnitAuras.AuraTable1 + ((uint)Pointers.UnitAuras.AuraSize * i)));
                    }
                    if (localSpellId != 0)
                    {
                        timeLeft = (uint)(timeLeft - currentTime) / 1000;
                        var aura = new WoWAura {SpellId = localSpellId, Stack = stackCount, SecondsLeft = timeLeft, OwnerGUID = ownerGuid};
                        result.Add(aura);
                    }
                }
                return result;
            }
        }

        /// <summary>
        ///   Returns ArrayList with SpellID's of Auras on the unit
        /// </summary>
        public List<int> GetBuffs()
        {
            return GetAuras.Select(woWAura => woWAura.SpellId).ToList();
        }

        /// <summary>
        /// Returns Number of stacks SpellID has
        /// </summary>
        /// <param name="spellId"></param>
        /// <returns></returns>
        public int BuffStacks(int spellId)
        {
            try
            {
                return (from woWAura in GetAuras where woWAura.SpellId == spellId select woWAura.Stack).FirstOrDefault();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        ///   Target a friend unit with TargetFriend
        /// </summary>
        /// <returns>true if sucess</returns>
        public bool TargetFriend()
        {
            if (ObjectManager.MyPlayer.TargetGUID.Equals(GUID))
                return true;
            Logging.Write("[Unit]TargetingF: " + Name);
            if (IsDead)
                return TargetDead();
            Face();
            var timer = new Ticker(600);
            Thread.Sleep(500);
            while (!ObjectManager.MyPlayer.TargetGUID.Equals(GUID) && !timer.IsReady)
            {
                KeyHelper.SendKey("TargetFriend");
                Thread.Sleep(1000);
            }
            if (ObjectManager.MyPlayer.TargetGUID.Equals(GUID))
            {
                Face();
                return true;
            }
            Logging.Write("[Unit]Could not targetF: " + Name);
            return false;
        }

        /// <summary>
        ///   Interacts with the unit.
        /// </summary>
        public void InteractWithTarget()
        {
            KeyHelper.SendKey("InteractTarget");
        }

        public void Interact()
        {
            InteractWithTarget();
        }

        /// <summary>
        ///   Target a hostile unit with TargetHostile
        /// </summary>
        /// <returns>true if sucess</returns>
        public bool TargetHostile()
        {
            if (ObjectManager.MyPlayer.TargetGUID.Equals(GUID))
                return true;
            Logging.Write("[Unit]TargetingH: " + Name);
            if (IsDead)
                return TargetDead();
            if (LazySettings.BackgroundMode)
            {
                return HostileBackgroundTargetting();
            }
            return HostileTabTargetting();
            /*

            var timer = new Ticker(2600);
            Face();
            while (!ObjectManager.MyPlayer.TargetGUID.Equals(GUID) && !timer.IsReady)
            {
                KeyHelper.SendKey("TargetEnemy");
                Thread.Sleep(1200);
            }
            Thread.Sleep(100);
            if (ObjectManager.MyPlayer.TargetGUID.Equals(GUID))
            {
                Face();
                return true;
            }
            Logging.Write("[Unit]Could not targetH: " + Name);
            return false; */
        }

        private bool HostileBackgroundTargetting()
        {
            var t = new Ticker(4 * 1000);
            while (!t.IsReady)
            {
                if (ObjectManager.MyPlayer.TargetGUID.Equals(GUID))
                {
                    return true;
                }
                if (!Location.IsFacing())
                {
                    Location.Face();
                }
                Memory.Write(Memory.BaseAddress + (uint)Pointers.Globals.MouseOverGUID, GUID);
                Thread.Sleep(50);
                KeyHelper.SendKey("InteractWithMouseOver");
                Thread.Sleep(500);
            }
            return false;
        }

        /// <summary>
        /// Returns True if targettind succeed, else False.
        /// </summary>
        private bool HostileTabTargetting()
        {
            var t = new Ticker(4 * 1000);
            while (!t.IsReady)
            {
                if (ObjectManager.MyPlayer.TargetGUID.Equals(GUID))
                {
                    return true;
                }
                if (!Location.IsFacing())
                {
                    Location.Face();
                }
                KeyHelper.SendKey("TargetEnemy");
                Thread.Sleep(700);

            }
            return false;
        }

        private bool TargetDead()
        {
            return Interact(false);
        }

        /// <summary>
        ///   Faces the unit.
        /// </summary>
        public void Face()
        {
            if (!Location.IsFacing())
            {
                Location.Face();
            }
        }

        public bool IsSpiritHealer
        {
            get
            {

                return (GetStorageField<uint>((uint)Descriptors.eUnitFields.UNIT_NPC_FLAGS) & (uint)UnitNPCFlags.UNIT_NPC_FLAG_SPIRITHEALER) != 0;

            }
        }

        public bool IsInnkeeper
        {
            get
            {

                return (GetStorageField<uint>((uint)Descriptors.eUnitFields.UNIT_NPC_FLAGS) & (uint)UnitNPCFlags.UNIT_NPC_FLAG_INNKEEPER) != 0;

            }
        }

        public bool IsFlightmaster
        {
            get
            {

                return (GetStorageField<uint>((uint)Descriptors.eUnitFields.UNIT_NPC_FLAGS) & (uint)UnitNPCFlags.UNIT_NPC_FLAG_FLIGHTMASTER) != 0;

            }
        }

        public bool IsTrainerMyClass
        {
            get
            {

                return (GetStorageField<uint>((uint)Descriptors.eUnitFields.UNIT_NPC_FLAGS) & (uint)UnitNPCFlags.UNIT_NPC_FLAG_TRAINER_CLASS) != 0;

            }
        }

        public bool CanRepair
        {
            get
            {

                return (GetStorageField<uint>((uint)Descriptors.eUnitFields.UNIT_NPC_FLAGS) & (uint)UnitNPCFlags.UNIT_NPC_FLAG_REPAIR) != 0;

            }
        }

        public bool IsVendorReagent
        {
            get
            {

                return (GetStorageField<uint>((uint)Descriptors.eUnitFields.UNIT_NPC_FLAGS) & (uint)UnitNPCFlags.UNIT_NPC_FLAG_VENDOR_REAGENT) != 0;

            }
        }

        public bool IsVendorFood
        {
            get
            {

                return (GetStorageField<uint>((uint)Descriptors.eUnitFields.UNIT_NPC_FLAGS) & (uint)UnitNPCFlags.UNIT_NPC_FLAG_VENDOR_FOOD) != 0;

            }
        }

        public bool IsVendor
        {
            get
            {

                return (GetStorageField<uint>((uint)Descriptors.eUnitFields.UNIT_NPC_FLAGS) & (uint)UnitNPCFlags.UNIT_NPC_FLAG_VENDOR) != 0;

            }
        }

        public bool IsBanker
        {
            get
            {

                return (GetStorageField<uint>((uint)Descriptors.eUnitFields.UNIT_NPC_FLAGS) & (uint)UnitNPCFlags.UNIT_NPC_FLAG_BANKER) != 0;

            }
        }

        public bool IsAuctioneer
        {
            get
            {

                return (GetStorageField<uint>((uint)Descriptors.eUnitFields.UNIT_NPC_FLAGS) & (uint)UnitNPCFlags.UNIT_NPC_FLAG_AUCTIONEER) != 0;

            }
        }

        public struct WoWAura
        {
            public int SpellId;
            public short Stack;
            public uint SecondsLeft;
            public ulong OwnerGUID;
        }

        internal enum UnitNPCFlags
        {
            UNIT_NPC_FLAG_AUCTIONEER = 0x200000,
            UNIT_NPC_FLAG_BANKER = 0x20000,
            UNIT_NPC_FLAG_BATTLEMASTER = 0x100000,
            UNIT_NPC_FLAG_FLIGHTMASTER = 0x2000,
            UNIT_NPC_FLAG_GOSSIP = 1,
            UNIT_NPC_FLAG_INNKEEPER = 0x10000,
            UNIT_NPC_FLAG_NONE = 0,
            UNIT_NPC_FLAG_PETITIONER = 0x40000,
            UNIT_NPC_FLAG_QUESTGIVER = 2,
            UNIT_NPC_FLAG_REPAIR = 0x1000,
            UNIT_NPC_FLAG_SPIRITGUIDE = 0x8000,
            UNIT_NPC_FLAG_SPIRITHEALER = 0x4000,
            UNIT_NPC_FLAG_STABLEMASTER = 0x400000,
            UNIT_NPC_FLAG_TABARDDESIGNER = 0x80000,
            UNIT_NPC_FLAG_TRAINER = 0x10,
            UNIT_NPC_FLAG_TRAINER_CLASS = 0x20,
            UNIT_NPC_FLAG_TRAINER_PROFESSION = 0x40,
            UNIT_NPC_FLAG_UNK1 = 4,
            UNIT_NPC_FLAG_UNK2 = 8,
            UNIT_NPC_FLAG_VENDOR = 0x80,
            UNIT_NPC_FLAG_VENDOR_AMMO = 0x100,
            UNIT_NPC_FLAG_VENDOR_FOOD = 0x200,
            UNIT_NPC_FLAG_VENDOR_POISON = 0x400,
            UNIT_NPC_FLAG_VENDOR_REAGENT = 0x800
        }
    }
}