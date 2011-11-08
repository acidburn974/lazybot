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

using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using DevComponents.AdvTree;
using DevComponents.DotNetBar;

#endregion

namespace LazyEvo.PVEBehavior.Behavior.Conditions
{
    public enum ConditionTargetEnum
    {
        Player = 0,
        Pet = 1,
        Target = 2,
    }

    public enum ConditionEnum
    {
        LessThan,
        EqualTo,
        MoreThan,
    }

    internal abstract class AbstractCondition
    {
        public abstract string Name { get; }

        public abstract string XmlName { get; }

        public abstract string GetXML { get; }

        public abstract bool IsOk { get; }
        public abstract List<Node> GetNodes();

        public abstract void NodeClick(Node node);
        public abstract void LoadData(XmlNode xmlNode);

        public Node CreateRadioButton(string name, string tag, bool selected = false)
        {
            return CreateRadioButton(name, name, tag, selected);
        }

        public Node CreateRadioButton(string name, string text, string tag, bool selected = false)
        {
            var node = new Node();
            node.CheckBoxStyle = eCheckBoxStyle.RadioButton;
            node.CheckBoxVisible = true;
            node.Expanded = true;
            node.Name = name;
            node.Text = text;
            node.Tag = tag;
            node.Checked = selected;
            node.DragDropEnabled = false;
            return node;
        }

        public Node CreateControl(string name, string tag, Control control)
        {
            var node = new Node();
            node.Expanded = true;
            node.Name = name;
            node.Tag = tag;
            node.HostedControl = control;
            node.DragDropEnabled = false;
            return node;
        }
    }
}