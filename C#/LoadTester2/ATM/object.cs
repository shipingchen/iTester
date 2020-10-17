using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace ATM 
{
  public class HelloServer : MarshalByRefObject 
  {
    public HelloServer() 
    { 
        Console.WriteLine("ATM Server activated..."); 
    }

    public String HelloMethod(String name) 
    {
      Console.WriteLine("{0} Received", name);
      return name + " - Acknowledged by server";
    }
  }
}