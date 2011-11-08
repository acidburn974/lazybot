
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
#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using LazyLib.Helpers;
using LazyLib.Wow;

#endregion

namespace LazyLib.Combat
{
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public abstract class CombatEngine
    {
        public const int MeleeDistance = 3;
        private readonly List<PAction> _queuedBuffs = new List<PAction>();
        public List<PAction> DamageActions;
        public List<PAction> SelfBuffActions;
        public List<PAction> SelfHealActions;

        public abstract string Name { get; }

        public virtual bool StartOk
        {
            get { return true; }
        }

        public virtual void BotStarted()
        {
        }

        public virtual void RunningAction()
        {
        }

        public abstract PullResult Pull(PUnit target);

        public virtual void Rest()
        {
        }

        public virtual void CombatDone()
        {
        }

        public abstract void Combat(PUnit target);
        public abstract Form Settings();

        public virtual void OnRess()
        {
        }

        public virtual void SettingsClosed()
        {
        }

        public virtual void LogicSelfBuff()
        {
            try
            {
                _queuedBuffs.Clear();
                SelfBuffActions.Sort();
                foreach (PAction selfBuffAction in
                    SelfBuffActions.Where(
                        selfBuffAction =>
                        selfBuffAction.IsWanted && (selfBuffAction.IsReady || selfBuffAction.WaitUntilReady)))
                {
                    _queuedBuffs.Add(selfBuffAction);
                }
                if (_queuedBuffs.Count != 0)
                    MoveHelper.ReleaseKeys();
                foreach (PAction queuedBuff in _queuedBuffs)
                {
                    queuedBuff.Execute();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception e)
            {
                Log("Error in LogicSelfBuff please check class code: " + e);
            }
        }

        public virtual void LogicSelfHeal()
        {
            try
            {
                SelfHealActions.Sort();
                PAction pAction = (from a in SelfHealActions
                                   where a.IsWanted && (a.IsReady || a.WaitUntilReady)
                                   select a).FirstOrDefault();
                if (pAction != null)
                    pAction.Execute();
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception e)
            {
                Log("Error in LogicSelfHeal please check class code: " + e);
            }
        }

        public virtual void LogicAttack(PUnit target)
        {
            try
            {
                DamageActions.Sort();
                PAction pAction = (from a in DamageActions
                                   where a.IsWanted && (a.IsReady || a.WaitUntilReady)
                                   select a).FirstOrDefault();
                if (pAction != null)
                    pAction.Execute();
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception e)
            {
                Log("Error in LogicAttack please check class code: " + e);
            }
        }

        /// <summary>
        ///   Writes to log
        /// </summary>
        /// <param name = "message">The message.</param>
        public static void Log(string message)
        {
            Logging.Write(message);
        }

        /// <summary>
        ///   Writes to log
        /// </summary>
        /// <param name = "message">The message.</param>
        /// <param name = "type">The  logtype.</param>
        public static void Log(string message, LogType type)
        {
            Logging.Write(type, message);
        }

        /// <summary>
        ///   Debugs the specified message.
        /// </summary>
        /// <param name = "message">The message.</param>
        public static void Debug(string message)
        {
            Logging.Debug(message);
        }
    }

    [Obfuscation(Feature = "renaming", ApplyToMembers = false)]
    public enum CombatResult
    {
        Unknown = 1,
        Success = 2,
        Bugged = 3,
        SuccessWithAdd = 4,
        Died = 5,
        OtherPlayerTag = 7,
        Pet = 8,
        Failed = 9,
    }

    [Obfuscation(Feature = "renaming", ApplyToMembers = false)]
    public enum PullResult
    {
        Success = 1,
        CouldNotPull = 2,
    }
}