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
#region

using System;
using System.Reflection;
using LazyLib.Helpers;

#endregion

namespace LazyLib.Wow
{
    /// <summary>
    ///   Representing a game object (Nodes)
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class PGameObject : PObject
    {
        public PGameObject(uint baseAddress)
            : base(baseAddress)
        {
        }

        /// <summary>
        ///   Gets the name of the object.
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get
            {
                try
                {
                    return
                        Memory.ReadUtf8(
                            Memory.Read<uint>(
                                Memory.Read<uint>(BaseAddress + (uint)Pointers.UnitName.ObjectName1) +
                                (uint)Pointers.UnitName.ObjectName2), 100);
                }
                catch
                {
                    return "Failed";
                }
            }
        }

        /// <summary>
        ///   The GameObject's Display ID.
        /// </summary>
        public int DisplayId
        {
            get { return GetStorageField<int>((uint) Descriptors.eGameObjectFields.GAMEOBJECT_DISPLAYID); }
        }

        /// <summary>
        ///   The GameObject's faction.
        /// </summary>
        public int Faction
        {
            get { return GetStorageField<int>((uint) Descriptors.eGameObjectFields.GAMEOBJECT_FACTION); }
        }

        /// <summary>
        /// Gets a value indicating whether the fishing bobber is bobbing.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if fishing bobber is bobbing; otherwise, <c>false</c>.
        /// </value>
        public bool IsBobbing
        {
            get { return Memory.Read<byte>(BaseAddress + (uint)Pointers.Globals.IsBobbing) != 0; }
        }

        public int GameObjectType
        {
            get
            {
                int field = GetStorageField<int>((uint) Descriptors.eGameObjectFields.GAMEOBJECT_BYTES_1);
                return ((field >> 8) & 0xFF);
            }
        }

        /// <summary>
        ///   The GameObject's level.
        /// </summary>
        public new int Level
        {
            get { return GetStorageField<int>((uint) Descriptors.eGameObjectFields.GAMEOBJECT_LEVEL); }
        }

        /// <summary>
        ///   Returns the GameObject's X position.
        /// </summary>
        public override float X
        {
            get { return Memory.Read<float>(BaseAddress + (uint) Pointers.WowObject.GameObjectX); }
        }

        /// <summary>
        ///   Returns the GameObject's Y position.
        /// </summary>
        public override float Y
        {
            get { return Memory.Read<float>(BaseAddress + (uint) Pointers.WowObject.GameObjectY); }
        }

        /// <summary>
        ///   Returns the GameObject's Z position.
        /// </summary>
        public override float Z
        {
            get { return Memory.Read<float>(BaseAddress + (uint) Pointers.WowObject.GameObjectZ); }
        }

        public override float Facing
        {
            get
            {
                return ObjectHelper.GetFacing(BaseAddress);
            }
        }

        public override Location Location
        {
            get { return new Location(X, Y, Z); }
        }
    }
}