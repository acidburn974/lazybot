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

using LazyLib.Wow;

namespace LazyEvo.Forms.Helpers
{
    internal class RefreshGui
    {
        public static void Refresh()
        {
            if (ObjectManager.Initialized)
            {
                if (ObjectManager.MyPlayer != null && ObjectManager.MyPlayer.IsValid)
                {
                    try
                    {
                        LazyForms.MainForm.UpdateProgressBar(LazyForms.MainForm.MainPBPlayerHP,
                                                             ObjectManager.MyPlayer.Health);
                        LazyForms.MainForm.UpdateProgressBar(LazyForms.MainForm.MainPBPlayerXP,
                                                             ObjectManager.MyPlayer.ExperiencePercentage);
                        LazyForms.MainForm.UpdateTextLabel(LazyForms.MainForm.MainLBPlayerHP,
                                                           ObjectManager.MyPlayer.Health + "%");
                        LazyForms.MainForm.UpdateTextLabel(LazyForms.MainForm.MainLBPlayerXP,
                                                           ObjectManager.MyPlayer.ExperiencePercentage + "%");
                        LazyForms.MainForm.UpdateGroupControl(LazyForms.MainForm.MainGPPlayer,
                                      ObjectManager.MyPlayer.Name);
                        switch (ObjectManager.MyPlayer.PowerTypeId)
                        {
                            case (uint) Constants.UnitPower.UnitPower_Energy:
                                LazyForms.MainForm.UpdateTextLabel(LazyForms.MainForm.MainLBPowerType, "Energy:");
                                LazyForms.MainForm.UpdateTextLabel(LazyForms.MainForm.MainLBPlayerPower,
                                                                   ObjectManager.MyPlayer.Energy + "%");
                                LazyForms.MainForm.UpdateProgressBar(LazyForms.MainForm.MainPBPlayerPower,
                                                                     ObjectManager.MyPlayer.Energy);
                                break;
                            case (uint) Constants.UnitPower.UnitPower_Mana:
                                LazyForms.MainForm.UpdateTextLabel(LazyForms.MainForm.MainLBPowerType, "Mana:");
                                LazyForms.MainForm.UpdateTextLabel(LazyForms.MainForm.MainLBPlayerPower,
                                                                   ObjectManager.MyPlayer.Mana + "%");
                                LazyForms.MainForm.UpdateProgressBar(LazyForms.MainForm.MainPBPlayerPower,
                                                                     ObjectManager.MyPlayer.Mana);
                                break;
                            case (uint) Constants.UnitPower.UnitPower_Rage:
                                LazyForms.MainForm.UpdateTextLabel(LazyForms.MainForm.MainLBPowerType, "Rage:");
                                LazyForms.MainForm.UpdateTextLabel(LazyForms.MainForm.MainLBPlayerPower,
                                                                   ObjectManager.MyPlayer.Rage + "%");
                                LazyForms.MainForm.UpdateProgressBar(LazyForms.MainForm.MainPBPlayerPower,
                                                                     ObjectManager.MyPlayer.Rage);
                                break;
                            case (uint) Constants.UnitPower.UnitPower_RunicPower:
                                LazyForms.MainForm.UpdateTextLabel(LazyForms.MainForm.MainLBPowerType, "Runic Power:");
                                LazyForms.MainForm.UpdateTextLabel(LazyForms.MainForm.MainLBPlayerPower,
                                                                   ObjectManager.MyPlayer.RunicPower + "%");
                                LazyForms.MainForm.UpdateProgressBar(LazyForms.MainForm.MainPBPlayerPower,
                                                                     ObjectManager.MyPlayer.RunicPower);
                                break;
                            case (uint) Constants.UnitPower.UnitPower_Focus:
                                LazyForms.MainForm.UpdateTextLabel(LazyForms.MainForm.MainLBPowerType, "Focus:");
                                LazyForms.MainForm.UpdateTextLabel(LazyForms.MainForm.MainLBPlayerPower,
                                                                   ObjectManager.MyPlayer.Energy + "%");
                                LazyForms.MainForm.UpdateProgressBar(LazyForms.MainForm.MainPBPlayerPower,
                                                                     ObjectManager.MyPlayer.Energy);
                                break;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                LazyForms.MainForm.UpdateGroupControl(LazyForms.MainForm.MainGPPlayer, "-");
                LazyForms.MainForm.UpdateProgressBar(LazyForms.MainForm.MainPBPlayerHP, 0);
                LazyForms.MainForm.UpdateProgressBar(LazyForms.MainForm.MainPBPlayerXP, 0);
                LazyForms.MainForm.UpdateTextLabel(LazyForms.MainForm.MainLBPlayerHP, 0 + "%");
                LazyForms.MainForm.UpdateTextLabel(LazyForms.MainForm.MainLBPlayerXP, 0 + "%");
            }
        }
    }
}