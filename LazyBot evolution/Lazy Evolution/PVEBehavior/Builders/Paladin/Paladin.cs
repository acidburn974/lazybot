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
    internal class Paladin
    {
        public static List<AddToBehavior> Load()
        {
            var add = new List<AddToBehavior>();

            // Pull actions1
            int spellId = 31935; // Avenger's Shield - Prot spec
            string spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Pull, Spec.Tree2, new Rule(spell, new ActionSpell(spell), 1,
                                                                             new List<AbstractCondition>
                                                                                 {
                                                                                     new HealthPowerCondition(
                                                                                         ConditionTargetEnum.Target,
                                                                                         ConditionTypeEnum.Health,
                                                                                         ConditionEnum.MoreThan, 0)
                                                                                 })));

            spellId = 20271; // Judgement
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Pull, Spec.Normal, new Rule(spell, new ActionSpell(spell), 2,
                                                                              new List<AbstractCondition>
                                                                                  {
                                                                                      new HealthPowerCondition(
                                                                                          ConditionTargetEnum.Target,
                                                                                          ConditionTypeEnum.Health,
                                                                                          ConditionEnum.MoreThan, 0)
                                                                                  })));

            const string name = "Auto Attack";
            add.Add(new AddToBehavior(name, Type.Pull, Spec.Normal, new Rule(name, new ActionSpell(name), 3,
                                                                             new List<AbstractCondition>
                                                                                 {
                                                                                     new HealthPowerCondition(
                                                                                         ConditionTargetEnum.Target,
                                                                                         ConditionTypeEnum.Health,
                                                                                         ConditionEnum.MoreThan, 0)
                                                                                 })));

            // Combat Actions
            spellId = 633; // Lay on Hands
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 1,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 10),
                                                                                        new BuffCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            BuffConditionEnum.
                                                                                                DoesNotHave,
                                                                                            BuffValueEnum.Id, "25771")
                                                                                    })));


            spellId = 642; // Divine Shield
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 2,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 15),
                                                                                        new BuffCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            BuffConditionEnum.
                                                                                                DoesNotHave,
                                                                                            BuffValueEnum.Id, "25771")
                                                                                    })));

            spellId = 31850; // Ardent Defender - Prot spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree2, new Rule(spell, new ActionSpell(spell), 3,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new HealthPowerCondition(
                                                                                           ConditionTargetEnum.Player,
                                                                                           ConditionTypeEnum.Health,
                                                                                           ConditionEnum.LessThan, 10)
                                                                                   })));

            spellId = 1022; // Hand of Protection
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 4,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 15),
                                                                                        new BuffCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            BuffConditionEnum.
                                                                                                DoesNotHave,
                                                                                            BuffValueEnum.Id, "25771")
                                                                                    })));

            spellId = 498; // Divine Protection
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 5,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 85)
                                                                                    })));

            spellId = 853; // Hammer of Justice
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 6,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 35)
                                                                                    })));

            spellId = 19750; // Flash of Light
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 7,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 25)
                                                                                    })));

            spellId = 82326; // Divine Light
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 8,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 35)
                                                                                    })));

            spellId = 635; // Holy Light
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 9,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 50)
                                                                                    })));

            spellId = 85673; // Word of Glory
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 10,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 55),
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.HolyPower,
                                                                                            ConditionEnum.MoreThan, 1)
                                                                                    })));

            spellId = 86150; // Guardian of Ancient Kings
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 11,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new CombatCountCondition(
                                                                                            ConditionEnum.MoreThan, 1)
                                                                                    })));

            spellId = 85285; // Rebuke - Ret spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree3, new Rule(spell, new ActionSpell(spell), 12,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new FunctionsCondition(
                                                                                           ConditionTargetEnum.Target,
                                                                                           FunctionsConditionEnum.Is,
                                                                                           FunctionEnum.Casting)
                                                                                   })));

            //TODO
            /*
            spellId = 20066; // Repentance - Ret spec
            spell = BarMapper.GetNameFromSpell(spellId);
            core.setAction(actName, MyCC.ActionType.Spell, spellId.ToString(), MyCC.ActionMoment.Combat, MyCC.Unit.An_add, 0, true);
            core.addCondition(actName, MyCC.Unit.Target, MyCC.ActionCond.Creature_Type, "=", "Demon", "", MyCC.ActionMoment.Combat);
            core.addCondition(actName, MyCC.Unit.Target, MyCC.ActionCond.Creature_Type, "=", "Dragon", "", MyCC.ActionMoment.Combat);
            core.addCondition(actName, MyCC.Unit.Target, MyCC.ActionCond.Creature_Type, "=", "Giant", "", MyCC.ActionMoment.Combat);
            core.addCondition(actName, MyCC.Unit.Target, MyCC.ActionCond.Creature_Type, "=", "Humanoid", "", MyCC.ActionMoment.Combat);
            core.addCondition(actName, MyCC.Unit.Target, MyCC.ActionCond.Creature_Type, "=", "Undead", "", MyCC.ActionMoment.Combat);
            core.setConfig(actName, MyCC.ActionMoment.Combat, MyCC.Spec.Spec3, true); */

            spellId = 31884; // Avenging Wrath
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 13,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new CombatCountCondition(
                                                                                            ConditionEnum.MoreThan, 1)
                                                                                    })));

            spellId = 85696; // Zealotry - Ret spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree3, new Rule(spell, new ActionSpell(spell), 14,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new HealthPowerCondition(
                                                                                           ConditionTargetEnum.Player,
                                                                                           ConditionTypeEnum.HolyPower,
                                                                                           ConditionEnum.EqualTo, 3)
                                                                                   })));

            spellId = 26573; // Consecration
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 15,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new CombatCountCondition(
                                                                                            ConditionEnum.MoreThan, 2)
                                                                                    })));

            spellId = 53600; // Shield of the Righteous - Prot spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree2, new Rule(spell, new ActionSpell(spell), 16,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new HealthPowerCondition(
                                                                                           ConditionTargetEnum.Player,
                                                                                           ConditionTypeEnum.HolyPower,
                                                                                           ConditionEnum.EqualTo, 3)
                                                                                   })));

            spellId = 84963; // Inquisition
            spell = BarMapper.GetNameFromSpell(spellId);
            //TODO
            /*core.setAction(actName, MyCC.ActionType.Spell, spellId.ToString(), MyCC.ActionMoment.Combat, MyCC.Unit.Target, 0, true);
            core.addCondition(actName, MyCC.Unit.Player, MyCC.ActionCond.Holy_Power, "=", "3", "", MyCC.ActionMoment.Combat);
            core.addCondition(actName, MyCC.Unit.Player, MyCC.ActionCond.Aura, "=", "90174", "", MyCC.ActionMoment.Combat);
            core.addCondition(actName, MyCC.Unit.Player, MyCC.ActionCond.Aura_Seconds_left, "<", "4", "Inquisition", MyCC.ActionMoment.Combat, true);
            core.setConfig(actName, MyCC.ActionMoment.Combat, MyCC.Spec.General, true); */

            spellId = 24275; // Hammer of Wrath
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 17,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.LessThan, 25)
                                                                                    })));

            spellId = 879; // Exorcism
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 18,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new BuffCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            BuffConditionEnum.HasBuff,
                                                                                            BuffValueEnum.Id, "59578")
                                                                                    })));

            spellId = 85256; // Templar's Verdict - Ret spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 19,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            ConditionTypeEnum.HolyPower,
                                                                                            ConditionEnum.MoreThan, 2)
                                                                                    })));

            spellId = 53595; // Hammer of the Righteous - Prot spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree2, new Rule(spell, new ActionSpell(spell), 20,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new CombatCountCondition(
                                                                                           ConditionEnum.MoreThan, 1)
                                                                                   })));

            spellId = 35395; // Crusader Strike
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 21,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Target,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.MoreThan, 0)
                                                                                    })));

            spellId = 20271; // Judgement
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 22,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Target,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.MoreThan, 0)
                                                                                    })));

            spellId = 31935; // Avenger's Shield - Prot spec
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Tree2, new Rule(spell, new ActionSpell(spell), 23,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new HealthPowerCondition(
                                                                                           ConditionTargetEnum.Target,
                                                                                           ConditionTypeEnum.Health,
                                                                                           ConditionEnum.MoreThan, 0)
                                                                                   })));

            spellId = 2812; // Holy Wrath
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Combat, Spec.Normal, new Rule(spell, new ActionSpell(spell), 24,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new HealthPowerCondition(
                                                                                            ConditionTargetEnum.Target,
                                                                                            ConditionTypeEnum.Health,
                                                                                            ConditionEnum.MoreThan, 0)
                                                                                    })));

            // Buff Actions
            spellId = 20154; // Seal of Righteousness
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Buff, Spec.Special, new Rule(spell, new ActionSpell(spell), 1,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new BuffCondition(
                                                                                           ConditionTargetEnum.Player,
                                                                                           BuffConditionEnum.DoesNotHave,
                                                                                           BuffValueEnum.Name,
                                                                                           "Seal of Righteousness")
                                                                                   })));

            spellId = 20165; // Seal of Insight
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Buff, Spec.Special, new Rule(spell, new ActionSpell(spell), 2,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new BuffCondition(
                                                                                           ConditionTargetEnum.Player,
                                                                                           BuffConditionEnum.DoesNotHave,
                                                                                           BuffValueEnum.Name,
                                                                                           "Seal of Insight")
                                                                                   })));

            spellId = 31801; // Seal of Truth
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Buff, Spec.Special, new Rule(spell, new ActionSpell(spell), 3,
                                                                               new List<AbstractCondition>
                                                                                   {
                                                                                       new BuffCondition(
                                                                                           ConditionTargetEnum.Player,
                                                                                           BuffConditionEnum.DoesNotHave,
                                                                                           BuffValueEnum.Name,
                                                                                           "Seal of Truth")
                                                                                   })));

            spellId = 465; // Devotion Aura
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Buff, Spec.Special2, new Rule(spell, new ActionSpell(spell), 4,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new BuffCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            BuffConditionEnum.
                                                                                                DoesNotHave,
                                                                                            BuffValueEnum.Name,
                                                                                            "Devotion Aura")
                                                                                    })));

            spellId = 7294; // Retribution Aura
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Buff, Spec.Special2, new Rule(spell, new ActionSpell(spell), 5,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new BuffCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            BuffConditionEnum.
                                                                                                DoesNotHave,
                                                                                            BuffValueEnum.Name,
                                                                                            "Retribution Aura")
                                                                                    })));

            spellId = 19746; // Concentration Aura
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Buff, Spec.Special2, new Rule(spell, new ActionSpell(spell), 6,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new BuffCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            BuffConditionEnum.
                                                                                                DoesNotHave,
                                                                                            BuffValueEnum.Name,
                                                                                            "Concentration Aura")
                                                                                    })));

            spellId = 32223; // Crusader Aura
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Buff, Spec.Special2, new Rule(spell, new ActionSpell(spell), 7,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new BuffCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            BuffConditionEnum.
                                                                                                DoesNotHave,
                                                                                            BuffValueEnum.Name,
                                                                                            "Crusader Aura")
                                                                                    })));

            spellId = 20217; // Blessing of Kings
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Buff, Spec.Special3, new Rule(spell, new ActionSpell(spell), 8,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new BuffCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            BuffConditionEnum.
                                                                                                DoesNotHave,
                                                                                            BuffValueEnum.Name,
                                                                                            "Blessing of Kings")
                                                                                    })));

            spellId = 19740; // Blessing of Might
            spell = BarMapper.GetNameFromSpell(spellId);
            add.Add(new AddToBehavior(spell, Type.Buff, Spec.Special3, new Rule(spell, new ActionSpell(spell), 9,
                                                                                new List<AbstractCondition>
                                                                                    {
                                                                                        new BuffCondition(
                                                                                            ConditionTargetEnum.Player,
                                                                                            BuffConditionEnum.
                                                                                                DoesNotHave,
                                                                                            BuffValueEnum.Name,
                                                                                            "Blessing of Might")
                                                                                    })));
            return add;
        }
    }
}