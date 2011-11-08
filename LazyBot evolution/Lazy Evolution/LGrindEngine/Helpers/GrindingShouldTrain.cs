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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LazyLib;
using LazyLib.Helpers;

namespace LazyEvo.LGrindEngine.Helpers
{
    internal class GrindingShouldTrain
    {
        public static bool ShouldTrain()
        {
            try
            {
                /*
                Logging.Write("Checking if we should train");
                if (!InterfaceHelper.GetFrameByName("SpellBookFrame").IsVisible)
                {
                    InterfaceHelper.GetFrameByName("SpellbookMicroButton").LeftClick();
                }
                Thread.Sleep(100);
                InterfaceHelper.GetFrameByName("SpellBookSkillLineTab2").LeftClick();
                if (CheckPages()) return true;
                InterfaceHelper.GetFrameByName("SpellBookSkillLineTab3").LeftClick();
                if (CheckPages()) return true;
                InterfaceHelper.GetFrameByName("SpellBookSkillLineTab4").LeftClick();
                if (CheckPages()) return true;
                CloseFrame(); */
                return false;
            }
            catch (Exception e)
            {
                Logging.Write("Exception getting Should Train: " + e);
                return false;
            }
        }

        public static int TrainCount()
        {
            try
            {
                Logging.Write("Getting train count");
                if (!InterfaceHelper.GetFrameByName("SpellBookFrame").IsVisible)
                {
                    InterfaceHelper.GetFrameByName("SpellbookMicroButton").LeftClick();
                }
                Thread.Sleep(100);
                int count = 0;
                InterfaceHelper.GetFrameByName("SpellBookSkillLineTab2").LeftClick();
                count += CheckPagesCount();
                InterfaceHelper.GetFrameByName("SpellBookSkillLineTab3").LeftClick();
                count += CheckPagesCount();
                InterfaceHelper.GetFrameByName("SpellBookSkillLineTab4").LeftClick();
                count += CheckPagesCount();
                CloseFrame();
                return count;
            }
            catch (Exception e)
            {
                Logging.Write("Exception getting Train Count: " + e);
                return 0;
            }
        }

        private static bool CheckPages()
        {
            InterfaceHelper.GetFrameByName("SpellBookPrevPageButton").LeftClick();
            if (CheckFrame())
            {
                CloseFrame();
                return true;
            }
            InterfaceHelper.GetFrameByName("SpellBookNextPageButton").LeftClick();
            if (CheckFrame())
            {
                CloseFrame();
                return true;
            }
            return false;
        }

        private static int CheckPagesCount()
        {
            int count = 0;
            string spellName =
                InterfaceHelper.GetFrameByName(string.Format("SpellButton{0}", "1")).GetChilds.Where(
                    spell => spell.GetName == "SpellButton1SpellName").FirstOrDefault().GetText;
            InterfaceHelper.GetFrameByName("SpellBookPrevPageButton").LeftClick();
            count += CheckSpellCount();
            InterfaceHelper.GetFrameByName("SpellBookNextPageButton").LeftClick();
            if (spellName !=
                InterfaceHelper.GetFrameByName(string.Format("SpellButton{0}", "1")).GetChilds.Where(
                    spell => spell.GetName == "SpellButton1SpellName").FirstOrDefault().GetText)
            {
                count += CheckSpellCount();
            }
            return count;
        }

        private static void CloseFrame()
        {
            if (InterfaceHelper.GetFrameByName("SpellBookFrame").IsVisible)
            {
                InterfaceHelper.GetFrameByName("SpellbookMicroButton").LeftClick();
            }
        }

        private static bool CheckFrame()
        {
            for (int i = 0; i <= 12; i++)
            {
                try
                {
                    List<Frame> frames = InterfaceHelper.GetFrameByName(string.Format("SpellButton{0}", i)).GetChilds;
                    foreach (Frame frame in frames)
                    {
                        if (frame.GetName == string.Format("SpellButton{0}SeeTrainerString", i))
                        {
                            if (frame.IsVisible)
                            {
                                return true;
                            }
                            break;
                        }
                    }
                }
                catch
                {
                }
            }
            return false;
        }

        private static int CheckSpellCount()
        {
            int spellsToTrain = 0;
            for (int i = 0; i <= 12; i++)
            {
                try
                {
                    List<Frame> frames = InterfaceHelper.GetFrameByName(string.Format("SpellButton{0}", i)).GetChilds;
                    foreach (Frame frame in frames)
                    {
                        if (frame.GetName == string.Format("SpellButton{0}SeeTrainerString", i))
                        {
                            if (frame.IsVisible)
                            {
                                spellsToTrain++;
                            }
                            break;
                        }
                    }
                }
                catch
                {
                }
            }
            return spellsToTrain;
        }
    }
}