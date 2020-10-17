//	Copyright (c) 2001 CSIRO. All Rights Reserved.
//
//	History:
//		10/08/2001	Shiping	Initial coding based on the existing code
//		24/09/2004	Shiping added timeout funaction and init();
//
//

package com.shiping.util;


/** This class implements a simple timer
*/
public class Timer
{
	final boolean debug = false;

	private boolean	started;
	
	private long	startTime;
	private long	endTime;
	private long	interval;
	private long 	timeout;

	public Timer()
	{
		init();
	}
	
	public Timer(long timeout)
	{
		init();
		this.timeout = timeout;
	}	

	private void init()
	{
		this.started  	= false;
		this.startTime 	= 0L;
		this.endTime   	= 0L;
		this.interval	= 0L;
		
		this.timeout	= 0L;	
	}
	
	public void start()
	{
		started = true;
		startTime = System.currentTimeMillis();
	}
	
	public void stop()
	{
		if(started)
		{
			endTime = System.currentTimeMillis();
			interval = endTime - startTime;
			started  = false;
		}
		else
		{
			System.err.println("The timer has not been started.");
		}
	}

	public long getTime()
	{
		return interval;
	}

	public boolean isTimeout()
	{
		endTime = System.currentTimeMillis();
		interval = endTime - startTime;
		if(debug) this.print();

		if( interval<timeout) return false;
		else                  return true;
	}

	public boolean isOK()
	{
		return !this.isTimeout();
	}
	
	public void printTime()
	{
		System.out.println(interval);
	}

	public void printTime(String msg)
	{
		System.out.println(msg + "\t" + interval);
	}

	public void print()
	{
		System.out.println(timeout + " > " + interval + " = " + endTime + " - " + startTime);
	}
}
