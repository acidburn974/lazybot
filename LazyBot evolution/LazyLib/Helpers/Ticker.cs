
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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

#endregion

namespace LazyLib.Helpers
{
    /// <summary>
    ///   Helper class for managing timers of all sorts
    /// </summary>
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    public class Ticker
    {
        #region -- Private Variables --

        private readonly double _countDowntime;

        private bool _varforceReady;

        #endregion

        #region Dll import

        /// <summary>
        ///   The QueryPerformanceCounter function retrieves the current 
        ///   value of the high-resolution performance counter.
        /// </summary>
        /// <param name = "x">
        ///   Pointer to a variable that receives the 
        ///   current performance-counter value, in counts.
        /// </param>
        /// <returns>
        ///   If the function succeeds, the return value is 
        ///   nonzero.
        /// </returns>
        [DllImport("kernel32.dll")]
        private static extern int QueryPerformanceCounter(ref long x);

        /// <summary>
        ///   The QueryPerformanceFrequency function retrieves the 
        ///   frequency of the high-resolution performance counter, 
        ///   if one exists. The frequency cannot change while the 
        ///   system is running.
        /// </summary>
        /// <param name = "x">
        ///   Pointer to a variable that receives 
        ///   the current performance-counter frequency, in counts 
        ///   per second. If the installed hardware does not support 
        ///   a high-resolution performance counter, this parameter 
        ///   can be zero.
        /// </param>
        /// <returns>
        ///   If the installed hardware supports a 
        ///   high-resolution performance counter, the return value 
        ///   is nonzero.
        /// </returns>
        [DllImport("kernel32.dll")]
        private static extern int QueryPerformanceFrequency(ref long x);

        #endregion

        /// <summary>
        ///   Initializes a new instance of the StopWatch class.
        /// </summary>
        /// <exception cref = "NotSupportedException">
        ///   The system does not have a high-resolution 
        ///   performance counter.
        /// </exception>
        public Ticker()
        {
            _countDowntime = 0;
            _varforceReady = false;
            Frequency = GetFrequency();
            Reset();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Ticker" /> class.
        /// </summary>
        /// <param name = "countDowntime">The count downtime.</param>
        public Ticker(double countDowntime)
        {
            _countDowntime = countDowntime*10;
            _varforceReady = false;
            Frequency = GetFrequency();
            Reset();
        }

        /// <summary>
        ///   Gets the ticks left.
        /// </summary>
        /// <value>The ticks left.</value>
        public double TicksLeft
        {
            get
            {
                if (Peek() > _countDowntime)
                {
                    return _countDowntime - Peek();
                }
                return _countDowntime - Peek();
            }
        }

        /// <summary>
        ///   Returns true if the timer is ready
        /// </summary>
        /// <exception cref = "NotSupportedException">
        ///   The system does not have a high-resolution 
        ///   performance counter.
        /// </exception>
        public bool IsReady
        {
            get
            {
                try
                {
                    if (_varforceReady)
                        return true;

                    if (Peek() > _countDowntime)
                        return true;
                }
                catch (Exception)
                {
                    /*LazyLib.Log.Debug(e, "checking if GSpellTimer.isReady)"); */
                }
                return false;
            }
        }

        /// <summary>
        ///   Gets or sets the start time.
        /// </summary>
        /// <value>
        ///   A long that holds the start time.
        /// </value>
        private long StartTime { get; set; }

        /// <summary>
        ///   Gets or sets the frequency of the high-resolution 
        ///   performance counter.
        /// </summary>
        /// <value>
        ///   A long that holds the frequency of the 
        ///   high-resolution performance counter.
        /// </value>
        private long Frequency { get; set; }

        /// <summary>
        ///   Waits this instance.
        /// </summary>
        public void Wait()
        {
            while (true)
            {
                if (Peek() >= _countDowntime)
                    break;

                Thread.Sleep(5);
            }
        }

        /// <summary>
        ///   Resets the stopwatch. This method should be called 
        ///   when you start measuring.
        /// </summary>
        /// <exception cref = "NotSupportedException">
        ///   The system does not have a high-resolution 
        ///   performance counter.
        /// </exception>
        public void Reset()
        {
            StartTime = GetValue();
            _varforceReady = false;
        }

        /// <summary>
        ///   Returns the time that has passed since the Reset() 
        ///   method was called.
        /// </summary>
        /// <remarks>
        ///   The time is returned in tenths-of-a-millisecond. 
        ///   If the Peek method returns '10000', it means the interval 
        ///   took exactely one second.
        /// </remarks>
        /// <returns>
        ///   A long that contains the time that has passed 
        ///   since the Reset() method was called.
        /// </returns>
        /// <exception cref = "NotSupportedException">
        ///   The system does not have a high-resolution performance counter.
        /// </exception>
        public long Peek()
        {
            return (long) (((GetValue() - StartTime)/(double) Frequency)*10000);
        }

        /// <summary>
        ///   Retrieves the current value of the high-resolution 
        ///   performance counter.
        /// </summary>
        /// <exception cref = "NotSupportedException">
        ///   The system does not have a high-resolution 
        ///   performance counter.
        /// </exception>
        /// <returns>
        ///   A long that contains the current performance-counter 
        ///   value, in counts.
        /// </returns>
        public static long GetValue()
        {
            long ret = 0;
            if (QueryPerformanceCounter(ref ret) == 0)
                throw new NotSupportedException("Error while querying " + "the high-resolution performance counter.");
            return ret;
        }

        /// <summary>
        ///   Retrieves the frequency of the high-resolution performance 
        ///   counter, if one exists. The frequency cannot change while 
        ///   the system is running.
        /// </summary>
        /// <exception cref = "NotSupportedException">
        ///   The system does not have a high-resolution 
        ///   performance counter.
        /// </exception>
        /// <returns>
        ///   A long that contains the current performance-counter 
        ///   frequency, in counts per second.
        /// </returns>
        public static long GetFrequency()
        {
            long ret = 0;
            if (QueryPerformanceFrequency(ref ret) == 0)
                throw new NotSupportedException(
                    "Error while querying "
                    + "the performance counter frequency.");
            return ret;
        }

        /// <summary>
        ///   Forces the ready.
        /// </summary>
        public void ForceReady()
        {
            _varforceReady = true;
        }
    }
}