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
 
/*
 * 	Yahoo Web Search Test Class
 *	Copyright 28/08/2008 David Ma
 *
 * 	v1.01 - Added option to print details
 */
import com.yahoo.search.*;

import java.io.IOException;
import java.util.Random;
import java.util.*;
import com.shiping.test.*;

// Import random line class
import com.david.*;

public class YahooTester implements IRunner 
{
   private String ID;
   private Properties prop;
   boolean print = true;
   boolean print2 = true;
   
   // Declare random line class
   private Randomline rndline = new Randomline();

   public void init(Properties prop)
   {
      this.prop = prop;
      this.ID = Thread.currentThread().getName();
   }
	
   public void preRun() {}
	
   public void run() throws Exception
   {
      try  
	  {
			// Declare new searchclient
			SearchClient client = new SearchClient("javasdktest");
			
			// Get new query from random line class
			String query = rndline.getLine("list.txt");
			
			// Create websearch request for given query
			WebSearchRequest request = new WebSearchRequest(query);
			
			// Get results from websearch request
			WebSearchResults results = client.webSearch(request);
			
			// Display results information
			System.out.println("Searching for: " + query);
			if(print)
			{
				System.out.println("The number of query matches in the database: " + results.getTotalResultsAvailable());
				System.out.println("The number of query matches returned: " + results.getTotalResultsReturned());
				System.out.println("The position of the first result in the overall search: " + results.getFirstResultPosition());
			    
				if(print2)
				{
					//Get result's first page
					WebSearchResult result = results.listResults()[0];
					
					// Display the details for the first page given
					System.out.println("First Page title: " + result.getTitle());
					System.out.println("First Page url: " + result.getUrl());
					System.out.println("First Page summary: " + result.getSummary());
				}
		    }
					
      } catch(Exception e) {System.err.println(e);}
   }

   public void postRun() {}

   public void done() {}
}
