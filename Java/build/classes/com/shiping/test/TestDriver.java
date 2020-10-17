/*
 * @(#) Client.java	10/06/04
 *
 * Copyright 2002 CSIRO, Australia. All rights reserved.
 * CSIRO PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

package com.shiping.test;

/**
 * This 
 *
 * @author  Dr. Shiping Chen
 * @version 1.0 10/06/04
 */

import java.util.*;
import com.shiping.util.*;


public class TestDriver implements Runnable
{
	final boolean debug = false;
	Properties prop;

	String testName;
	String testClassName;
	long testRuntime;
	long testWarmup;
	long testCooldown;
      	String testReportName;

	public ResultLog testLog;

	public TestDriver(final Properties prop) {
		if (debug)
			System.out.println("TestDriver.TestDriver() called");

		// Here is still running in main thread.
		try {
			this.prop = prop;
			// if(debug) prop.list( System.out );

			testName = prop.getProperty("testName");
			testClassName = prop.getProperty("testClassName");
			testReportName = prop.getProperty("testReportName");

			testRuntime = Integer.parseInt(prop.getProperty("testRuntime"));
			testWarmup = Integer.parseInt(prop.getProperty("testWarmup"));
			testCooldown = Integer.parseInt(prop.getProperty("testCooldown"));

			if (debug) {
				System.out.println("testName     = " + testName);
				System.out.println("testClassName= " + testClassName);
				System.out.println("testRuntime  = " + testRuntime + " msec");
				System.out.println("testWarmup   = " + testWarmup + " %");
				System.out.println("testCooldown = " + testCooldown + " %");
			}

			// instance a gloabl log to holding overall test results
			testLog = new ResultLog();
			if (testName != null)
				testLog.setTestName(testName);
			if (testReportName != null)
				testLog.setTestReportName(testReportName);
		} catch (final Exception e) {
			System.err.println(e.toString());
		}
	}

	public void run() // Here is running within sub-thread.
	{
		if (debug)
			System.out.println(Thread.currentThread().getName() + " started !");

		try {
			// Prepare a local copy of properties
			final Properties localProp = new Properties(prop);

			final String testThreadID = Thread.currentThread().getName();
			localProp.setProperty("testThreadID", testThreadID);

			// To instance a test runner
			if (debug)
				System.out.println("To instance a test class with name = " + testClassName);
			final IRunner testRunner = (IRunner) Class.forName(testClassName).getConstructor().newInstance();

			// To init the test runner
			testRunner.init(localProp);

			if (testWarmup > 0) {
				System.out.println("warm-up......");
				Test(testRunner, (long) (testRuntime * testWarmup / 100.0), false);
			}
			System.out.println("testing......");
			Test(testRunner, (long) (testRuntime * (100.0 - testWarmup - testCooldown) / 100.0), true);
			if (testCooldown > 0) {
				System.out.println("cool-down......");
				Test(testRunner, (long) (testRuntime * testCooldown / 100.0), false);
			}
		} catch (final Exception e) {
			System.err.println(e.toString());
		}
	}

	private void Test(final IRunner testRunner, final long testTime, final boolean isLog) {
		if (debug)
			System.out.println(
					Thread.currentThread().getName() + ": Test() called: testTime = " + testTime + " isLog = " + isLog);

		final ResultLog tmpLog = new ResultLog();

		try {
			final com.shiping.util.Timer testTimer1 = new com.shiping.util.Timer(testTime);
			final com.shiping.util.Timer testTimer2 = new com.shiping.util.Timer();

			for (testTimer1.start(); !testTimer1.isTimeout();) {
				testRunner.preRun();

				testTimer2.start();
				testRunner.run();
				testTimer2.stop();

				testRunner.postRun();
				if (debug)
					System.out.println(Thread.currentThread().getName() + ": Latency = " + testTimer2.getTime());

				if (isLog)
					tmpLog.add(testTimer2.getTime());
			}
		} catch (final Exception e)
		{
			System.err.println(e.toString());
		}
		finally
		{
			// add the local log to the global log
			if(isLog) testLog.add(tmpLog);
		}
	}
}

