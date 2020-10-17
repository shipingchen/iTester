using System;
using System.Threading;

namespace com.csiro.cs.util
{
	/// <summary>
	/// A timer interface to provide the basic timing functions:
	/// </summary>

	public interface Timer
	{
		/// <summary>
		/// start the timer
		/// </summary>
		void start();
		/// <summary>
		/// stop the timer
		/// </summary>
		/// <returns>return the time span between start and stop, in ms</returns>
		ulong stop();
		/// <summary>
		/// if the pre-defined time is reached
		/// </summary>
		/// <returns>if the time is reached, return true</returns>
		bool isTime();
	}

	/// <summary>
	/// a Timer implementation with default resolution (20ms) 
	/// </summary>
	public class NormalTimer : Timer
	{
		// private
		bool		started;
		DateTime	startTime;
		DateTime	endTime;
		TimeSpan	timeSpan;

		ulong		alarmTime;
		ulong		interval;

		/// <summary>
		/// constructor for a normal timer
		/// </summary>
 		public NormalTimer()
		{
			started = false;
		}

		/// <summary>
		/// constructor for an alarm timer
		/// </summary>
		/// <param name="t">the setting time of alarm timer</param>
 		public NormalTimer(ulong t)
		{
			started = false;
			alarmTime = t;
		}

		private void print()
		{
			System.Console.WriteLine(interval);
		}
		private void getEscapedTime()
		{
			timeSpan = endTime.Subtract(startTime);
			interval = (ulong)timeSpan.TotalMilliseconds;
		}
		
		/// <summary>
		/// implementation of interface Timer
		/// </summary>
		public void start()
		{
			started = true;
			startTime = DateTime.Now;
		}

		/// <summary>
		/// stop the timer
		/// </summary>
		/// <returns>the interval from the start to stop</returns>
		public ulong stop()
		{
			if(started) 
			{
				endTime = DateTime.Now;
				getEscapedTime();
				started = false;
			}
			else 
			{
				interval = 0;
			}
			return interval;
		}

		/// <summary>
		/// if it is time to stop
		/// </summary>
		/// <returns></returns>
		public bool isTime()
		{
			if(started) 
			{
				endTime = DateTime.Now;
				getEscapedTime();
				if(interval<alarmTime)	return false;
				else					return true;
			}
			else 
			{
				System.Console.WriteLine(interval);
				return false;
			}	
		}
	}

	/// <summary>
	/// a Timer implementation with default resolution (20ms) 
	/// </summary>
	public class TestTimer : Timer
	{
		const bool debug = false;

		// private
		bool		started;
		DateTime	startTime;
		DateTime	endTime;
		TimeSpan	timeSpan;

		ulong		alarmTime;
		ulong		interval;

		ulong		warmupTime = 0;
		ulong		cooldownTime = 0;

		/// <summary>
		/// constructor for a normal timer
		/// </summary>
		public TestTimer()
		{
			started = false;
		}

		/// <summary>
		/// constructor for an alarm timer
		/// </summary>
		/// <param name="t">the setting time of alarm timer</param>
		public TestTimer(ulong testTime)
		{
			started = false;
			alarmTime = testTime;
		}

		public TestTimer(ulong testTime, double warmupRate, double cooldownRate)
		{
			started = false;
			alarmTime = testTime;

			this.warmupTime = (ulong)(testTime*warmupRate);
			this.cooldownTime = (ulong)(testTime*(1.0 - cooldownRate));
		}

		private void print()
		{
			System.Console.WriteLine(interval);
		}
		private void getEscapedTime()
		{
			timeSpan = endTime.Subtract(startTime);
			interval = (ulong)timeSpan.TotalMilliseconds;
		}
		
		/// <summary>
		/// implementation of interface Timer
		/// </summary>
		public void start()
		{
			started = true;
			startTime = DateTime.Now;
		}

		/// <summary>
		/// stop the timer
		/// </summary>
		/// <returns>the interval from the start to stop</returns>
		public ulong stop()
		{
			if(started) 
			{
				endTime = DateTime.Now;
				getEscapedTime();
				started = false;
				return interval;
			}
			else 
			{
				throw new Exception("The timer is not started");
			}		
		}

		/// <summary>
		/// if it is time to stop
		/// </summary>
		/// <returns></returns>
		public bool isTime()
		{
			if(started) 
			{
				endTime = DateTime.Now;
				getEscapedTime();
				if(interval<alarmTime)	return false;
				else					return true;
			}
			else 
			{
				throw new Exception("The timer is not started");
			}
		}

		public bool isTesting()
		{
			if(started)
			{
				if(warmupTime==0 && cooldownTime==0) return true;	// no warmup and cooldown checking

				endTime = DateTime.Now;
				getEscapedTime();
				if(interval<warmupTime) 
				{
					// if(debug) Console.WriteLine("Warmup..");
					return false;
				}
				else if(interval>cooldownTime) 
				{
					// if(debug) Console.WriteLine("Cooldown..");
					return false;
				}
				else
				{
					// if(debug) Console.WriteLine("In Testing...");
					return true;
				}
			}
			else 
			{
				throw new Exception("The timer is not started");
			}
		}
	}
}
