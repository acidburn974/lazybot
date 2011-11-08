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

namespace LazyEvo.PVEBehavior.Behavior
{
    public enum Spec
    {
        Normal = 1,
        Tree1 = 2,
        Tree2 = 3,
        Tree3 = 4,
        Special = 5,
        Special2 = 6,
        Special3 = 7,
    }

    public enum Type
    {
        Combat = 1,
        Pull = 2,
        Buff = 3,
        Rest = 4,
        PrePull = 5
    }

    internal class AddToBehavior
    {
        public AddToBehavior(string name, Type type, Spec spec, Rule rule)
        {
            Name = name;
            Type = type;
            Spec = spec;
            Rule = rule;
        }

        public string Name { get; set; }
        public Type Type { get; set; }
        public Spec Spec { get; set; }
        public Rule Rule { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}