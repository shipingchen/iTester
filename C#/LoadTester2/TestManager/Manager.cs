using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using com.csiro.xml.datatype;
using Interfaces;


namespace TestManager
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Manager
	{
		[STAThread]
		static void Main(string[] args)
		{
			Manager testManager = new Manager(args);

			testManager.init();
			testManager.test();
		}

		public Manager(string[] args)
		{
			// process args
			for(int i=0; i<args.Length; i++)
			{
				if(args[i].Equals("-f")) this.fileName	= args[++i];
				if(args[i].Equals("-t")) this.testName	= args[++i];
			}

			if(debug)
			{
				Console.WriteLine("fileName = " + this.fileName);
			}

			// load config file -> obj
			try
			{
				if(debug) Console.WriteLine( "To open file: " + this.fileName);
				XmlTextReader xmlReader = new XmlTextReader(this.fileName);
				XmlSerializer xmlSer = new XmlSerializer(typeof(Config));

				config = (Config) xmlSer.Deserialize(xmlReader);
				xmlReader.Close();

				if(debug) DataUtility.print(config);
			}
			catch(Exception e)
			{
				Console.WriteLine("Failed to open the file, due to " + e.ToString());
				Environment.Exit(1);
			}

			if(debug) DataUtility.print(config);
		}

		public void test()
		{
			string msg = "Hi testRobot";
			Console.WriteLine("To call robot.ping(" + msg + ")");
			Console.WriteLine("reply = " + this.robot.ping("Hi testRobot"));

			Console.WriteLine("To call robot.doit(cmd)");
			Console.WriteLine("reply = " + robot.doit(this.config));
		}

		public void init()
		{
			if(debug) Console.Write("To locate the server on " + url);

			TcpClientChannel channel = new TcpClientChannel();
			ChannelServices.RegisterChannel(channel);
			robot = (ITestRobot) Activator.GetObject(typeof(ITestRobot),url);

			if(robot==null)
			{
				Console.WriteLine("Cannot locate the server");
				return;
			}

			if(debug) Console.WriteLine("................OK");
		}

		static bool debug = true;
		static string url = "tcp://localhost:8086/TestRobot";

		string fileName = "config.xml";
		string testName = "MyTest";
		Config config;
		ITestRobot robot;
	}
}
