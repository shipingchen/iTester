/*
 * @(#) Client.java	10/06/04
 *
 * Copyright 2002 CSIRO, Australia. All rights reserved.
 * CSIRO PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

package com.shiping.test;

import java.lang.*;
import java.util.*;


/**
 * This 
 *
 * @author  Dr. Shiping Chen
 * @version 1.0 10/06/04
 */

import java.util.*;

public class TestManager
{
	final static boolean debug = true;
	Properties prop = new Properties();

	TestDriver testDriver;

	public TestManager(String fileName)
	{
		if(debug) System.out.println("Tester.init(" + fileName + ") called");

		try 
		{
			java.io.FileInputStream in = new java.io.FileInputStream(fileName);
			prop.load(in);
			if(debug) prop.list( System.out );
		}
		catch(Exception e)
		{
			System.out.println(e.toString());
			System.exit(1);
		}
	}

	public void run()
	{
		if(debug) System.out.println("Tester.run() called");
		
		try
		{
			int testThreadNmuber = Integer.parseInt(prop.getProperty("testThreadNmuber"));
			if(debug) System.out.println("testThreadNmuber = " + testThreadNmuber);
			
			Thread threadArray[] = new Thread[testThreadNmuber];
			
			if(debug) System.out.println("To instance a test driver");
 			testDriver = new TestDriver(prop);
			
			for(int i=0; i<testThreadNmuber; i++) 
			{
				threadArray[i] = new Thread(testDriver);
				threadArray[i].start();
			}
			
			for(int i=0; i<testThreadNmuber; i++)
			{
				threadArray[i].join();
				if(debug) System.out.println("Thread-" +i+ " : exited");
			}
		}
		catch(Exception e)
		{
			System.err.println(e.toString());
			System.exit(1);
		}
	}

	public void report()
	{
		if(debug) System.out.println("\nTester.report() called");

		testDriver.testLog.print();
	}
}
