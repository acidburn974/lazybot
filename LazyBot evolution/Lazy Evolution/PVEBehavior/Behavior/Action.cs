/*
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

using System.Xml;

#endregion

namespace LazyEvo.PVEBehavior.Behavior
{
    internal abstract class Action
    {
        public abstract bool IsReady { get; }
        public abstract bool DoesKeyExist { get; }
        public abstract string Name { get; }
        public abstract void Execute(int globalCooldown);
        public abstract string GetXml();
        public abstract void Load(XmlNode node);
    }
}