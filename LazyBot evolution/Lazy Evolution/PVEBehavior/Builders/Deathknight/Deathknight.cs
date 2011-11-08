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

using System.Collections.Generic;
using LazyEvo.PVEBehavior.Behavior;
using LazyEvo.PVEBehavior.Behavior.Conditions;
using LazyLib.ActionBar;

namespace LazyEvo.PVEBehavior.Builders
{
    internal class Deathknight
    {
        public static List<AddToBehavior> Load()
        {
            var add = new List<AddToBehavior>();
            //----------- Pull actions 
            int spellId = 57330;
            string spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Pull, Spec.Normal, new Rule(spell, new ActionSpell(spell), 1)));

            spellId = 49576; // Death Grip
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Pull, Spec.Normal,
                                      new Rule(spell, new ActionSpell(spell), 2,
                                               new List<AbstractCondition>
                                                   {new DistanceToTarget(ConditionEnum.MoreThan, 10)})));

            spellId = 45477; // Icy Touch
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Pull, Spec.Normal, new Rule(spell, new ActionSpell(spell), 3)));

            string name = "Auto Attack";
            add.Add(new AddToBehavior(name, Type.Pull, Spec.Normal, new Rule(name, new ActionSpell(name), 4)));

            //-----------  Combat Actions
            spellId = 46584; // Raise Dead - For unholy spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree3,
                                      new Rule(spell, new ActionSpell(spell), 7,
                                               new List<AbstractCondition>
                                                   {new PetCondition(PetConditionEnum.DoesNotHave)})));

            spellId = 49016; // Unholy Frenzy - Unholy spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree3, new Rule(spell, new ActionSpell(spell), 1,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new HealthPowerCondition(
                                                                                           ConditionTargetEnum.Player,
                                                                                           ConditionTypeEnum.Health,
                                                                                           ConditionEnum.MoreThan, 80),
                                                                                       new CombatCountCondition(
                                                                                           ConditionEnum.MoreThan, 1)
                                                                                   })));

            spellId = 49203; // Hungering Cold - Frost spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree2,
                                      new Rule(spell, new ActionSpell(spell), 4,
                                               new List<AbstractCondition>
                                                   {new CombatCountCondition(ConditionEnum.MoreThan, 1)})));

            spellId = 49222; // Bone Shield - Blood spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree1, new Rule(spell, new ActionSpell(spell), 2,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new HealthPowerCondition(
                                                                                           ConditionTargetEnum.Player,
                                                                                           ConditionTypeEnum.Health,
                                                                                           ConditionEnum.LessThan, 75)
                                                                                   })));

            spellId = 55233; // Vampiric Blood - Blood spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree1, new Rule(spell, new ActionSpell(spell), 1,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new HealthPowerCondition(
                                                                                           ConditionTargetEnum.Player,
                                                                                           ConditionTypeEnum.Health,
                                                                                           ConditionEnum.LessThan, 20)
                                                                                   })));

            spellId = 48982; // Rune Tap - Blood spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree1, new Rule(spell, new ActionSpell(spell), 3,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new HealthPowerCondition(
                                                                                           ConditionTargetEnum.Player,
                                                                                           ConditionTypeEnum.Health,
                                                                                           ConditionEnum.LessThan, 75)
                                                                                   })));

            spellId = 49028; // Dancing Rune Weapon - Blood spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree1, new Rule(spell, new ActionSpell(spell), 4,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new CombatCountCondition(
                                                                                           ConditionEnum.MoreThan, 1)
                                                                                   })));

            spellId = 47568; // Empower Rune Weapon
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 5,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new CombatCountCondition(
                                                                                            ConditionEnum.MoreThan, 1),
                                                                                        new RuneCondition(
                                                                                            ConditionEnum.EqualTo,
                                                                                            RuneEnum.Blood, 0),
                                                                                        new RuneCondition(
                                                                                            ConditionEnum.EqualTo,
                                                                                            RuneEnum.Frost, 0),
                                                                                        new RuneCondition(
                                                                                            ConditionEnum.EqualTo,
                                                                                            RuneEnum.Unholy, 0),
                                                                                    }
                                                                           )));

            spellId = 46584; // Raise Dead
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 6,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new CombatCountCondition(
                                                                                            ConditionEnum.MoreThan, 1),
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 40),
                                                                                    })));

            spellId = 48743; // Death Pact
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 8,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 22)
                                                                                    })));

            spellId = 42650; // Army of the Dead
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 8,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new CombatCountCondition(
                                                                                            ConditionEnum.MoreThan, 2),
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 30)
                                                                                    })));

            spellId = 48792; // Icebound Fortitude
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 9,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 40)
                                                                                    })));

            spellId = 57330; // Horn of Winter
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 10)));

            spellId = 47528; // Mind Freeze
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 11,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new FunctionsCondition(
                                                                                            ConditionTargetEnum.Target,
                                                                                            FunctionsConditionEnum.Is,
                                                                                            FunctionEnum.Casting)
                                                                                    })));

            spellId = 47476; // Strangulate
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 12,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new FunctionsCondition(
                                                                                            ConditionTargetEnum.Target,
                                                                                            FunctionsConditionEnum.Is,
                                                                                            FunctionEnum.Casting)
                                                                                    })));

            spellId = 48707; // Anti-Magic Shell
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 13,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new FunctionsCondition(
                                                                                            ConditionTargetEnum.Target,
                                                                                            FunctionsConditionEnum.Is,
                                                                                            FunctionEnum.Casting)
                                                                                    })));

            spellId = 56815; // Rune Strike
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 14)));

            spellId = 49143; // Frost Strike - Frost Spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree2, new Rule(spell, new ActionSpell(spell), 15)));

            spellId = 47541; // Death Coil
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 14)));

            spellId = 49998; // Death Strike
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 15,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 55)
                                                                                    })));

            spellId = 49020; // Obliterate
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 16,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new BuffCondition(
                                                                                            ConditionTargetEnum.Target,
                                                                                            BuffConditionEnum.HasBuff,
                                                                                            BuffValueEnum.Name,
                                                                                            "Blood Plague"),
                                                                                        new BuffCondition(
                                                                                            ConditionTargetEnum.Target,
                                                                                            BuffConditionEnum.HasBuff,
                                                                                            BuffValueEnum.Name,
                                                                                            "Frost Fever"),
                                                                                    })));

            spellId = 85948; // Festering Strike
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 14)));

            spellId = 49184; // Howling Blast - Frost spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree2, new Rule(spell, new ActionSpell(spell), 15,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new CombatCountCondition(
                                                                                           ConditionEnum.MoreThan, 1),
                                                                                   })));

            spellId = 45477; // Icy Touch
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 16,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new BuffCondition(
                                                                                            ConditionTargetEnum.Target,
                                                                                            BuffConditionEnum.
                                                                                                DoesNotHave,
                                                                                            BuffValueEnum.Name,
                                                                                            "Frost Fever"),
                                                                                    })));

            spellId = 55090; // Scourge Strike - Unholy spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree3, new Rule(spell, new ActionSpell(spell), 17,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new BuffCondition(
                                                                                           ConditionTargetEnum.Target,
                                                                                           BuffConditionEnum.HasBuff,
                                                                                           BuffValueEnum.Name,
                                                                                           "Blood Plague"),
                                                                                       new BuffCondition(
                                                                                           ConditionTargetEnum.Target,
                                                                                           BuffConditionEnum.HasBuff,
                                                                                           BuffValueEnum.Name,
                                                                                           "Frost Fever"),
                                                                                   })));

            spellId = 45462; // Plague Strike
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 18,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new BuffCondition(
                                                                                            ConditionTargetEnum.Target,
                                                                                            BuffConditionEnum.
                                                                                                DoesNotHave,
                                                                                            BuffValueEnum.Name,
                                                                                            "Blood Plague"),
                                                                                    })));

            spellId = 55050; // Heart Strike - Blood spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree1, new Rule(spell, new ActionSpell(spell), 19)));

            spellId = 45902; // Blood Strike
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 14)));

            //-----------  Buff Actions
            spellId = 48263; // Blood Presence
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Buff, Spec.Special, new Rule(spell, new ActionSpell(spell), 1,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new BuffCondition(
                                                                                           ConditionTargetEnum.Player,
                                                                                           BuffConditionEnum.DoesNotHave,
                                                                                           BuffValueEnum.Id, "48263"),
                                                                                   })));

            spellId = 48266; // Frost Presence
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Buff, Spec.Special, new Rule(spell, new ActionSpell(spell), 1,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new BuffCondition(
                                                                                           ConditionTargetEnum.Player,
                                                                                           BuffConditionEnum.DoesNotHave,
                                                                                           BuffValueEnum.Id, "48266"),
                                                                                   })));

            spellId = 48265; // Unholy Presence
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Buff, Spec.Special, new Rule(spell, new ActionSpell(spell), 1,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new BuffCondition(
                                                                                           ConditionTargetEnum.Player,
                                                                                           BuffConditionEnum.DoesNotHave,
                                                                                           BuffValueEnum.Id, "48265"),
                                                                                   })));
            return add;
        }
    }
}