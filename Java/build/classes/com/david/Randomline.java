/*
*
*  	28th August 2008 David Ma
*	Returns a random line from a text file
*
*/

package com.david;

import java.io.*;
import java.util.*;
 
public class Randomline 
{
 
	// Returns the string for the given line number
    public static String getLineNumber(int number, String list) throws FileNotFoundException 
	{
		try
		{
			// Reads the file  "list.txt"
			BufferedReader br = new BufferedReader(new FileReader(list));
			String txtLine = "";
			
			// Cycles through the file for the given number of times
			for(int i=0; i!=number; i++)
			{
				txtLine = br.readLine();
			}
			
			// Returns string for the given line 
			return txtLine;
		}
		catch (IOException e)
		{
			System.out.println(e);
			return null;
		}
    }
    
	// Returns the amount of lines for a given text file
    public static int countLines(String fileName) throws FileNotFoundException 
	{
		try
		{
			// Reads the given text file
			BufferedReader br = new BufferedReader(new FileReader(fileName));
			String txtLine = "";
			int count = 0;
			txtLine = br.readLine(); 
			
			//While it isn't end of the file, cycle through each line and increment count
			while(txtLine != null)
			{
				txtLine = br.readLine();
				count++;
			}
			
			//Return the amount of lines
			return count;
		}
		catch (IOException e)
		{
			System.out.println(e);
			return 0;
		}
    }
    
    public static String getLine(String list) throws Exception 
	{
		// Gets number of lines
        int numberLines = countLines(list);
        
		// Create a random generator
		Random generator = new Random();
        
		//Use generator to get a random number between  0 and number of lines
		int randomNumber = generator.nextInt(numberLines)+1;
        
		//Get string for that given line
		String randomLine = getLineNumber(randomNumber, list);
        
		// Return string
		return randomLine;
    }
}
