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
 *
 * 	Http GET Test Class
 *       This class will demonstrate a  Java websever stress test implementation using the iTester framework
 *        Has a boolean option to download and output the stream to a file to simulate a real user download.
 *	Copyright 10/09/2008 David Ma
 *
 */

import java.io.*;
import java.net.*;
import java.util.Random;
import java.util.*;
import com.shiping.test.*;

// Import random line class
import com.david.*;

public class HttpTester implements IRunner 
{
   private String ID;
   private Properties prop;
   private Randomline rndline = new Randomline();
   boolean print = true;
   int count = 0;
   
   public void init(Properties prop)
   {
      this.prop = prop;
      this.ID = Thread.currentThread().getName();
   }
	
   public void preRun() {count++;}
	
   public void run() throws Exception
   {
      try{
	 
	  String address = rndline.getLine("url.txt");

	  OutputStream to_file; 
	  
      // Now use the URL class to parse the user-specified URL into
      // its various parts: protocol, host, port, filename.  Check the protocol
      URL url = new URL(address);
      String protocol = url.getProtocol();
      if (!protocol.equals("http"))
	  {
        throw new IllegalArgumentException("URL must use 'http:' protocol");
      }
	  String host = url.getHost();
      int port = url.getPort();
      if (port == -1) port = 80;  // if no port, use the default HTTP port
      String filename = url.getFile();
	  
      // Open a network socket connection to the specified host and port
      Socket socket = new Socket(host, port);
	  
	  if(print)
	  {
		//Get an output stream to write the URL contents to
		
		to_file = new FileOutputStream("./logs/"+host+"_"+ID+"_"+count);
	  }
	  else to_file = System.out;
	  
      // Get input and output streams for the socket
      InputStream from_server = socket.getInputStream();
      PrintWriter to_server = 
       new PrintWriter(new OutputStreamWriter(socket.getOutputStream()));
      
      // Send the HTTP GET command to the Web server, specifying the file.
      // This uses an old and very simple version of the HTTP protocol
      to_server.println("GET " + filename);
      to_server.flush();  // Send it right now!
      System.out.println("Host: " + host + " Port: " + port);
	  
      // Now read the server's response, and write it to the file
	  if(print)
	  {
		byte[] buffer = new byte[16384];
		int bytes_read;
		while((bytes_read = from_server.read(buffer)) != -1)
		{
			to_file.write(buffer, 0, bytes_read);
		}
	  }
      
      // When the server closes the connection, we close our stuff
      socket.close();
	  if(print)
		to_file.close();
		
      } catch(Exception e) {System.err.println(e);}
   }

   public void postRun() {}

   public void done() {}
}
