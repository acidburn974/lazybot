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

using System;

namespace LazyEvo.Debug
{
    internal class NameValuePair
    {
        private readonly String _mValue = "";
        public String MName = "";

        public NameValuePair(String name, String value)
        {
            MName = name;
            _mValue = value;
        }

        public String Name
        {
            get { return MName; }
        }

        public String Value
        {
            get { return _mValue; }
        }
    }
}