//	Copyright (c) 2003 CSIRO. All Rights Reserved.
//
//	History:
//		10/08/2003	Shiping	Initial coding
//		17/09/2003	Shiping changed the namespace to remoteTimeServices
//		03/03/2004	Shiping changed the interval to double for higher resolution
//		
//

using System;
using System.Runtime.InteropServices;

namespace com.csiro.cs.util
{
	/// <summary>
	/// This is a c# wraper for a highe resolution timer 
	/// </summary>
	public class HighResolutionTimer : Timer
	{
		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out ulong freq);

		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceCounter(out ulong tick);

		[DllImport("Kernel32.dll")]
		static extern uint GetTickCount();

		bool isHRT = false;
		ulong tick;
		ulong freq;					// CPU clock
		double resolution = 1.0;	// msec per tick

		ulong startTick;
		ulong endTick;
		ulong interval;	// msec
		ulong alarmTime = 0;

		/// <summary>
		/// Constructor
		/// </summary>
		public HighResolutionTimer()
		{
			if(QueryPerformanceFrequency( out freq))
			{
				isHRT = true;
				resolution = 1000.0/(double) freq;
			}
			else
			{
				isHRT = false;
				resolution = 1.0;
			}
			//System.Console.Out.WriteLine("resolution= " + resolution);  
		}
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="t">The alarm time, in millisecond</param>
		public HighResolutionTimer(ulong t) : this()
		{
			alarmTime = t;
		}
		/// <summary>
		/// get the resolution of this timer
		/// </summary>
		/// <returns>the resolution</returns>
		public double getResolution()
		{
			return resolution;
		}

		/// <summary>
		/// if the current timer is a high-resolution timer
		/// </summary>
		/// <returns>return true if the current timer is a high-resolution timer; otherwise false</returns>
		public bool isHighResolutionTimer()
		{
			return isHRT;
		}

		/// <summary>
		/// get high-resolution interval
		/// </summary>
		/// <returns>the high-resolution interval, in millisecond</returns>
		public double getSuperHRInterval()
		{
			return ((endTick - startTick)*resolution);
		}

		/// <summary>
		/// implementation of interface Timer 
		/// </summary>
		public void start()
		{
			if(isHRT) 
			{
				QueryPerformanceCounter(out tick);
				startTick = tick;
			}
			else
				startTick = GetTickCount();
		}

		/// <summary>
		/// stop the current timer
		/// </summary>
		/// <returns>the intervals from start to stop</returns>
		public ulong stop()
		{
			if(isHRT) 
			{ 
				QueryPerformanceCounter(out tick);
				endTick = tick;
				interval = (ulong)((endTick - startTick)*resolution);
			}
			else 
			{
				endTick = GetTickCount();
				interval = endTick - startTick;
			}
			return interval;
		}
		
		/// <summary>
		///  if it is time to stop
		/// </summary>
		/// <returns>When it is time to stop return true; otherwise return false</returns>
		public bool isTime()
		{
			ulong tmp;
			if(isHRT) 
			{ 
				QueryPerformanceCounter(out tick);
				endTick = tick;
				tmp = (ulong)((endTick - startTick)*resolution);
			}
			else 
			{
				endTick = GetTickCount();
				tmp = endTick - startTick;
			}
			if(tmp<alarmTime)	return false;
			else	return true;
		}
	} // end of HighResolutionTimer
}
