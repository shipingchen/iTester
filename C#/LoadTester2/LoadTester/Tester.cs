///
/// LoadTester v1.0
/// 
/// Copryright 2005 CSIRO ICT Centre. All rights reserved.
///
///		20/03/2005 Shiping	inital code based previous internal testing code developed by Shiping and Bo
///							with the following enhancements:
///							- use a univeral configuration schame
///							- add serveral interfaces for ITestRun, i.e. preRun, postRun, done etc.
///							- ?
///							- ?
///
///

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Reflection;
using System.Threading;

using com.csiro.xml.datatype;
using com.csiro.cs.util;


namespace LoadTester
{
	/// <summary>
	/// Summary description for 
	/// </summary>
	class Tester
	{
		public Tester(string[] args)
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

			// get testing data from config
			com.csiro.xml.datatype.Item item = DataUtility.getTheFirstItem(DataUtility.getTheFirstCatelog(config));

			this.numClients		= Int32.Parse( DataUtility.getPropertyValueByName(item, "numClients", "10"));
			this.testTime		= (ulong) (Int32.Parse( DataUtility.getPropertyValueByName(item, "testTime",  "30"))*1000);
			this.warmupRate		= Double.Parse( DataUtility.getPropertyValueByName(item, "warmupRate", "0.1"));
			this.cooldownRate	= Double.Parse( DataUtility.getPropertyValueByName(item, "cooldownRate", "0.1"));
			this.isPreRun       = Boolean.Parse(DataUtility.getPropertyValueByName(item, "isPreRun", "true"));
			this.isPostRun      = Boolean.Parse(DataUtility.getPropertyValueByName(item, "PostRun", "false"));

			this.testAssemblyName = DataUtility.getPropertyValueByName(item, "testAssemblyName", "MyTest");
			this.testAssemblyTypeName = DataUtility.getPropertyValueByName(item, "testAssemblyTypeName", "Interfaces.ITestRun");

			this.machineName = System.Environment.MachineName;
			this.logPath = DataUtility.getPropertyValueByName(item, "logDirName", ".\\log");

			if(debug)
			{
				Console.WriteLine("machineName          = " + machineName);
				Console.WriteLine("testName             = " + testName);
				Console.WriteLine("numClients           = " + numClients);
				Console.WriteLine("testTime             = " + testTime);
				Console.WriteLine("warmupRate           = " + warmupRate);
				Console.WriteLine("cooldownRate         = " + cooldownRate);
				Console.WriteLine("testAssemblyName     = " + testAssemblyName);
				Console.WriteLine("testAssemblyTypeName = " + testAssemblyTypeName);
				Console.WriteLine("logPath              = " + logPath);
				Console.WriteLine("isPreRun             = " + isPreRun);
				Console.WriteLine("isPostRun            = " + isPostRun);

				Console.WriteLine("");
				Console.WriteLine("");
			}
		}

		public void usgae()
		{
			Console.WriteLine("Usage: LocalTester -f config.xml");
		}


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Tester tester = new Tester(args);

			tester.prepare();
			tester.test();
			tester.report();
		}

		public void prepare()
		{
			// load test assembly (DLL)
			try
			{
				if(debug) Console.Out.WriteLine("To load the testing assembly " + testAssemblyName); 
				Assembly assembly = Assembly.Load(this.testAssemblyName);

				this.testAssemblyType = assembly.GetType(this.testAssemblyTypeName);
				System.Diagnostics.Debug.Assert( this.testAssemblyType != null );
			}
			catch(Exception e)
			{
				System.Console.WriteLine("Failed to load: " + testAssemblyName);
				throw e;
			}

			// prepare logs
			this.logInfo = DataUtility.getCatelogByName(config, "Log");
			if(logInfo==null) throw new Exception("No log catelog" + this.fileName);

			this.numLogs = logInfo.Count;
			this.globalLogs = new ResultLog[this.numLogs];
			for(int i=0; i<globalLogs.Length; i++)
			{
				string logName = logInfo[i].name;
				double disUnit = Double.Parse(DataUtility.getPropertyValueByName(logInfo[i], "disUnit", "10"));
				globalLogs[i] = new ResultLog(logName, disUnit, 0, 5000, 500);
			}
		}

		public void test()
		{
			// create and start multiple threads/clients
			//
			Thread[] client = new Thread[numClients];
			for(int i=0; i<numClients; i++)
			{
				client[i] = new Thread( new ThreadStart(this.run) );
				client[i].Name = i.ToString();
				client[i].Start();
			}

			// To wait all threads to finish
			for(int i=0; i<numClients; i++)
			{
				client[i].Join();
			}
		}

		public void report()
		{
			if(debug) Console.WriteLine("report() called");

			// log to disk
			try 
			{
				// Determine whether the directory exists.
				if (!Directory.Exists(this.logPath)) throw new Exception("no dir: " + logPath);
				logPath = logPath + "\\" + this.testName + "-" + this.machineName;

				// Try to create the directory
				Console.Write("To create: " + logPath);
				DirectoryInfo di = Directory.CreateDirectory(logPath);
				Console.WriteLine(".....OK");

				for(int i=0; i<this.globalLogs.Length; i++)
				{
					// if(debug) this.globalLogs[i].printResult(true);

					string logFileName = logPath + "\\" + this.globalLogs[i].getName() + ".txt";
					Console.WriteLine("To write: " + logFileName);
					TextWriter tw = new StreamWriter(logFileName);
					this.globalLogs[i].printResult(tw, true, false);
					tw.Close();
				}

				this.printTPCCSummaryReoprt();
			} 
			catch (Exception e) 
			{
				Console.WriteLine("Tester.Report(): " + e.Message);
			}
		}

		private void printTPCCSummaryReoprt()
		{
			string logFileName = logPath + "\\summary.txt";
			Console.WriteLine("To write: " + logFileName);
			TextWriter tw = new StreamWriter(logFileName);

			tw.WriteLine("Response Time (in msec) \t Average \t 90th \t Max");

			for(int i=0; i<globalLogs.Length; i++)
			{
				tw.WriteLine(globalLogs[i].getName() + "\t" + globalLogs[i].getAve() + 
					                                   "\t" + globalLogs[i].getVauleByPercent(0.9) +
                                                       "\t" + globalLogs[i].getMax() );
			}

			tw.Close();
		}

		// entery point for each thread/client
		public void run()
		{	
			if(debug) Console.WriteLine("Thread-" + Thread.CurrentThread.Name + " started");

			// instance the test runner and call its init()
			Interfaces.ITestRun runner = Activator.CreateInstance(this.testAssemblyType) as Interfaces.ITestRun;
			runner.init(this.config);

			// prepare local logs to avoid conflictions between threads
			// prepare logs
			ResultLog[] localLogs = new ResultLog[this.numLogs];
			for(int i=0; i<globalLogs.Length; i++)
			{
				string logName = logInfo[i].name;
				double disUnit = Double.Parse(DataUtility.getPropertyValueByName(logInfo[i], "disUnit", "10"));
				localLogs[i] = new ResultLog(logName, disUnit, 0, 5000, 500);
			}

			// prepare timers: one for performance measurement
			//                 one for test control
			HighResolutionTimer timer = new HighResolutionTimer();
			com.csiro.cs.util.TestTimer alarm = new com.csiro.cs.util.TestTimer(this.testTime,this.warmupRate, this.cooldownRate);

			// prepare temp varibles
			ulong latency;				// to test performance
			int numTx = 0;				// to test fairness
			int index;					// to identify which log
			int localNumExceptions = 0;	// to record exceptions
			bool _isPreRun = this.isPreRun;
			bool _isPostRun = this.isPostRun;

			for(alarm.start(); !alarm.isTime(); numTx++)
			{				
				try
				{
					if(_isPreRun) runner.preRun();

					timer.start();
						index = runner.run();
					latency = timer.stop();
                    if(alarm.isTesting()) localLogs[0].add(latency); // only log test period

					if(_isPostRun) runner.postRun();
				}
				catch(Exception e)
				{
					Console.WriteLine("LoadTester.Tester.run(): " + e.Message);
					localNumExceptions++;
				}
			}

			try
			{
				this.globalNumExceptions += localNumExceptions;

				if(debug) Console.WriteLine("numTx = " + numTx);
				runner.done();

				// add to global
				lock(this.globalLogs)
				{
					for(int i=0; i<localLogs.Length; i++)
					{
						this.globalLogs[i].add(localLogs[i]);
					}
				}
			}
			catch(Exception e)
			{
				Console.WriteLine("LoadTester.Tester.run(): " + e.Message);
			}
		}

		// constances
		const bool debug = true;

		// file stuff
		string fileName = "config.xml";
		Config config	= null;

		string testAssemblyName		= null;
		string testAssemblyTypeName	= null;
		Type   testAssemblyType		= null;
		
		// testing paramenters
		int		numClients;
		ulong	testTime;
		double	warmupRate;
		double	cooldownRate;
		bool    isPreRun;
		bool    isPostRun;

		// logs
		string		testName = "TPCCTest";
		string		machineName;
		string		logPath;
		Catelog		logInfo;

		int			globalNumExceptions = 0;
		int			numLogs;
		ResultLog[] globalLogs;
	}
}
