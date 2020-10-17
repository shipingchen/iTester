using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using com.csiro.xml.datatype;

namespace ATM 
{
  public class ATMclient : Interfaces.ITestRun
  {
    // for debug
    const bool debug = true;
          
    // Number of transactions
    private const int TRANSAC_NUM = 4;    
          
    public ATMclient()
    {
        if (debug) Console.WriteLine("ATMClient() called");
    }
        
    public void init(Config config)
    {
        if(debug) Console.WriteLine("ATMClient.init() called"); 
    }

    public void preRun()
    {
        if(debug) Console.WriteLine("ATMClient.preRun() called");
    }

    public int run()
    {
        if(debug) Console.WriteLine("ATMClient.run() called");
            
        HelloServer obj = 
        (HelloServer)Activator.GetObject(typeof(ATM.HelloServer),"tcp://localhost:8085/SayHello");
        if (obj == null)
        { 
            System.Console.WriteLine("Could not locate server");
            return 0;
        }
        else 
        { 
            for(int i=1; i<=TRANSAC_NUM; i++)
                Console.WriteLine(obj.HelloMethod("Transaction " + i)); 
        }
        return 1;
    }
        
    public void run(ref string[] pnames, ref long[] pvalues)
    {
        if(debug) Console.WriteLine("ATMClient.run(args) called");
    }

    public void postRun()
    {
        if(debug) Console.WriteLine("ATMClient.postRun() called");      
    }

    public void done()
    {
        if(debug) Console.WriteLine("ATMClient.done() called"); 
    }
  }
}