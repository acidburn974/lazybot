
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
using LazyLib.Wow;

namespace LazyLib.Helpers
{
    public class ValidateKeys
    {
        public static bool Validate()
        {
            KeyBindings.LoadBindings();
            bool result = true;
            if(!KeyBindings.CheckBind("ACTIONPAGE1", "SHIFT-1")) 
                result = false;
            if(!KeyBindings.CheckBind("ACTIONPAGE2", "SHIFT-2")) 
                result = false;
            if(!KeyBindings.CheckBind("ACTIONPAGE3", "SHIFT-3")) 
                result = false;
            if(!KeyBindings.CheckBind("ACTIONPAGE4", "SHIFT-4")) 
                result = false;
            if(!KeyBindings.CheckBind("ACTIONPAGE5", "SHIFT-5")) 
                result = false;
            if (!KeyBindings.CheckBind("ACTIONPAGE6", "SHIFT-6"))
                result = false;
            foreach (var keyWrapper in KeyHelper.KeysList)
            {
                switch (keyWrapper.Key)
                {
                    case KeyHelper.InteractWithMouseover:
                        if (!KeyBindings.CheckBind("INTERACTMOUSEOVER", keyWrapper.Value.Key.ToUpper()))
                            result = false;
                        break;
                    case KeyHelper.TargetLastTarget:
                        if (!KeyBindings.CheckBind("TARGETLASTTARGET", keyWrapper.Value.Key.ToUpper()))
                            result = false;
                        break;
                    case KeyHelper.InteractTarget:
                        if (!KeyBindings.CheckBind("INTERACTTARGET", keyWrapper.Value.Key.ToUpper()))
                            result = false;
                        break;
                    case "X":
                        if (!KeyBindings.CheckBind("SITORSTAND", keyWrapper.Value.Key.ToUpper()))
                            result = false;
                        break;
                    case "Up":
                        if (!KeyBindings.CheckBind("MOVEFORWARD", keyWrapper.Value.Key.ToUpper()))
                            result = false;
                        break;
                    case "Down":
                        if (!KeyBindings.CheckBind("MOVEBACKWARD", keyWrapper.Value.Key.ToUpper()))
                            result = false;
                        break;
                    case "Left":
                        if (!KeyBindings.CheckBind("TURNLEFT", keyWrapper.Value.Key.ToUpper()))
                            result = false;
                        break;
                    case "Right":
                        if (!KeyBindings.CheckBind("TURNRIGHT", keyWrapper.Value.Key.ToUpper()))
                            result = false;
                        break;
                    case "Space":
                        if (!KeyBindings.CheckBind("JUMP", keyWrapper.Value.Key.ToUpper()))
                            result = false;
                        break;
                }
            }
            return result;
        }

        public static bool AutoLoot
        {
            get
            {

                return
                    Memory.Read<uint>(
                        Memory.ReadRelative<uint>((uint)Pointers.AutoLoot.Pointer) +
                        (uint)Pointers.AutoLoot.Offset) == 1;

            }
        }

        public static bool ClickToMove
        {
            get
            {

                return
                    Memory.Read<uint>(
                        Memory.ReadRelative<uint>((uint)Pointers.ClickToMove.Pointer) +
                        (uint)Pointers.ClickToMove.Offset) == 1;

            }
        } 
    }
}
