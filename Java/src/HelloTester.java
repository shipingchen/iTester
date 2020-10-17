/*
 * @(#) Client.java	10/06/04
 *
 * Copyright 2002 CSIRO, Australia. All rights reserved.
 * CSIRO PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * This 
 *
 * @author  Dr. Shiping Chen
 * @version 1.0 10/06/04
 *
 */

import java.util.*;
import com.shiping.test.*;

public class HelloTester implements IRunner 
{
   private String ID;
   private Properties prop;
   private Random r = new Random();

   public void init(Properties prop)
   {
      this.prop = prop;
      this.ID = Thread.currentThread().getName();
   }
	
   public void preRun() {}
	
   public void run() throws Exception
   {	
      try  {
         int t = ((int)(r.nextGaussian()+2.0))*2000;
         System.out.println(" Hello " + ID + " to sleep for " + t + " ms" );
	 Thread.currentThread().sleep(t);
      } catch(Exception e) {}
   }

   public void postRun() {}

   public void done() {}
}
