/////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Copyright (c) 2001 CSIRO. All Rights Reserved.
//
//	History:
//		10/08/2001	Shiping - Initial coding based on the existing code
//		25/10/2001	Shiping - Two changes:
//							1. Add getTotal for TPS calculation
//							2. Remove all synchronized except for addSelf() for reducing overheads
//      	23/05/2002  Shiping - Math.round() is added to calculate average
//		02/07/2002	Shiping - Max and Min is added.
//		10/05/2003	SHiping - Add code for distribution calculation
//                          	  - Rename some varibles and API to make the class general enough for most logging
//		24/09/2004	Shiping - Add print()
//          30/07/2008	Shiping - Add the following interfaces for the students to implement:
//                                -- printReport(); // print test results to a file, whose name is testReportName
//                                -- setCL();       // set the confidential level, such as 95%
//                                -- setER();       // set the error rate, i.e. 
//                                -- getCL();       // get the confidential level
//                                -- getER();       // get the error rate
//
//
//
//////////////////////////////////////////////////////////////////////////////////////////////////

package com.shiping.util;

public class ResultLog
{
	// This class should be independent for the parementers to be logged and
	// can be used in most cases.
	//
	final boolean debug = false;
	
	boolean isDistribution = true;
	boolean isPrintZero    = false;

	String testName;
      String testReportName;

	final int VALUE_MAX =  10000;	// msec, this paraments should be imported from the configure file in future
	final int VALUE_UNIT = 20;	// msec, because the resoluation for java timer is about 20 msec.
	int	VALUE_SIZE = VALUE_MAX/VALUE_UNIT;			// the array size for holding the distribution of the measured values
	int valueDistribution[] = new int[VALUE_SIZE+2];	// we allocate 2 more just in case

	int	counter = 0;		// Sample conuter
	long	sumValue = 0;		// Sume of sample values
	long	maxValue = 0;		// so that it can reset with a bigger value
	long	minValue = 999999999;	// so that it can reset with a samller value
	long	walltime = 0;		// only for JMS batch testings

	double CL = 0.0;
	double ER = 0.0;

	public ResultLog() 
	{
		this.testName = null;
            this.testReportName = null;
		init();
	}

	public void setTestName(String testName)
	{
		this.testName = testName;
	}

	public void setTestReportName(String testReportName)
	{
		this.testReportName = testReportName;
	}

	public void add(long value)
	{
		if(debug) System.out.println("addSample(long value): value = " + value);

		sumValue += value;
		counter++;

		if(value > maxValue) maxValue = value;
		if(value < minValue) minValue = value;

		if(isDistribution) this.setDistribution(value);
	}
        
	synchronized public void add(ResultLog rlog)
	{
		sumValue += rlog.sumValue;
		counter += rlog.counter;

		if(rlog.maxValue > maxValue) this.maxValue = rlog.maxValue;
		if(rlog.minValue < minValue) this.minValue = rlog.minValue;
		if(rlog.walltime > walltime) this.walltime = rlog.walltime;

		for(int i=0; i<=VALUE_SIZE; i++)
			this.valueDistribution[i] += rlog.valueDistribution[i];
	}

	public long getAve()
	{
		return (counter == 0) ? 0 :  Math.round( (double) sumValue/(double)counter );
	}

	// =================================================================================
	// This is a specific method for calculate overall throughout from multiple threads,
	// overall_throughput = sum of the throughput for each threads  
	//
	public long getThroughput()
	{
		return sumValue;
	}

	public void setWalltime(long t)
	{
		walltime = t;
	}

	public long getWalltime()
	{
		return walltime;
	}

	// ===================================================================================
	public long getTotal()
	{
		return sumValue;
	}
        
	public long getCounter()
	{
		return counter;
	}
    
	public long getMax()
	{
		return maxValue;
	}
    
	public long getMin()
	{
		return minValue;
	}

	public static long getCurrentTime()
	{
		return System.currentTimeMillis();
	}

	public void setDistribution(long value) 
	{
		int temp = (int) value;

		if(temp<0) temp = 0;
		else if(temp>VALUE_MAX) temp = VALUE_MAX;

		int index = temp/VALUE_UNIT;
		valueDistribution[index]++;
		// if(debug) System.out.println("setLatencyDis(): valueDistribution[ " + index + " ] = " + valueDistribution[index]);
	}

	public void printDistribution() 
	{
		for(int i=0; i<=VALUE_SIZE; i++)
		{
			if(isPrintZero) 
			{
				System.out.println(i + "\t" + valueDistribution[i]);
			}
			else
			{
				if(valueDistribution[i]!=0) 
					System.out.println((i+1)*VALUE_UNIT + "\t" + valueDistribution[i]);
			}
		}
	}
	
	public void print()
	{
		if(testName!=null) System.out.println("testName = " + testName);
		System.out.println("minValue = " + minValue);
		System.out.println("aveValue = " + getAve());
		System.out.println("maxValue = " + maxValue);
		
		if(isDistribution) printDistribution();
	}

	private void init()
	{
		for(int i=0; i<=VALUE_SIZE; i++)
			valueDistribution[i] = 0;
	}

      public void printReport()	// TBD
      {

	}

      public void setCL(double d)	// TBD
      {
		this.CL = d;
	}

      public void setER(double d)	// TBD
      {
		this.ER = d;
	}

      public double getCL()	// TBD
      {
		return CL;
	}

      public double getER()	// TBD
      {
		return ER;
	}
}
