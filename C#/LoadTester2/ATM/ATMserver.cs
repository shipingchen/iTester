using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace ATM 
{
  public class ATMserver
  {
    public static int Main(string [] args) 
    {
      TcpChannel chan = new TcpChannel(8085);
      ChannelServices.RegisterChannel(chan);
      
      RemotingConfiguration.RegisterWellKnownServiceType(Type.GetType("ATM.HelloServer,object"), 
      "SayHello", WellKnownObjectMode.Singleton);
      
      System.Console.WriteLine("Hit <enter> to exit...");
      System.Console.ReadLine();
      
      return 0;
    }
  }
}