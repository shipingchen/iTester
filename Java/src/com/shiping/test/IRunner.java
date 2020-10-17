/*
 * @(#) ITestDriver.java
 *
 * Copyright 2004 CSIRO, Australia. All rights reserved.
 * CSIRO PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 */
package com.shiping.test;

import java.util.Properties;

/**
 * This 
 *
 * @author  Shiping
 * @version 1.0 20/05/02
 */

public interface IRunner
{
	public void init(Properties p) 	throws Exception;
	public void preRun()	     		throws Exception;
	public void run()			throws Exception;
	public void postRun()        		throws Exception;
}
