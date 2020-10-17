/*
 * @(#) Client.java	1.0 10/06/04
 *
 * Copyright 2002 CSIRO, Australia. All rights reserved.
 * CSIRO PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */

import com.shiping.test.*;

/**
 * This 
 *
 * @author  Dr. Shiping Chen
 * @version 1.0 10/06/04
 */

class Tester
{
  	public static void main(String[] args) 
	{
		if (args.length<1)
		{
			System.out.println("Usage: java Tester test.prop");
			System.exit(1);
		}
		System.out.println("test config fileName = " + args[0]);

		TestManager manager = new TestManager(args[0]);
		manager.run();
		manager.report();
	}
}
