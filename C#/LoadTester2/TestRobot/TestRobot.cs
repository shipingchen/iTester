using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using com.csiro.xml.datatype;


namespace TestRobot
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class TestRobot
	{
		static int portID = 8086;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main(string[] args)
		{
			Console.Write("To register TestRobotImp as a service");
 
			TcpServerChannel channel = new TcpServerChannel(portID);
			ChannelServices.RegisterChannel(channel);
			RemotingConfiguration.RegisterWellKnownServiceType(
				typeof(TestRobotImp), "TestRobot",
				WellKnownObjectMode.SingleCall);

			Console.WriteLine(".........................OK");

			Console.WriteLine("Hit to exit");
			Console.ReadLine();
		}
	}
}
