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
using LazyLib.Helpers;

#endregion

namespace LazyLib.Wow
{
    internal class Faction
    {
        public static Reaction GetReaction(PUnit localObj, PUnit mobObj)
        {
            try
            {
                if (localObj.Faction < 1 || mobObj.Faction < 1)
                {
                    return Reaction.Missing;
                }

                return FactionHelper.FindReactionFromFactions(localObj.Faction, mobObj.Faction);
            }
            catch (Exception)
            {
                //Logging.Write("Exception when comparing: " + localObj.Name + " : " + mobObj.Name);
                return Reaction.Missing;
            }
        }

  
    }
}