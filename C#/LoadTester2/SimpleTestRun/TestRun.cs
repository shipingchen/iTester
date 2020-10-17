using System;

using com.csiro.xml.datatype;

namespace SimpleTestRun
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class TestRun : Interfaces.ITestRun
	{
		// for debug
		const bool debug = true;
		string threadID;

		Random r;
		int sleepTime;

		public TestRun()
		{
			this.threadID = System.Threading.Thread.CurrentThread.Name;
			if(debug) print("SimpleTestRun() called");

			int seed = Int32.Parse(threadID);
			r = new Random(seed);
		}

		public void init(Config config)
		{
			if(debug) print("SimpleTestRun.init() called");	
		}

		public void preRun()
		{
			if(debug) print("SimpleTestRun.preRun() called");
			sleepTime = r.Next(2000);
			System.Threading.Thread.Sleep(sleepTime);	// to simulate thinking time
		}

		public int run()
		{
			if(debug) print("SimpleTestRun.run() called");
			sleepTime = r.Next(4000);
			System.Threading.Thread.Sleep(sleepTime);	// to simulate thinking time

			int index = r.Next(4);
			return index;
		}

		public void run(ref string[] pnames, ref long[] pvalues)
		{
			if(debug) print("SimpleTestRun.run(ref string[] pnames, ref long[] pvalues) called");
			sleepTime = r.Next(4000);
			System.Threading.Thread.Sleep(sleepTime);	// to simulate thinking time
		}

		public void postRun()
		{
			if(debug) print("SimpleTestRun.postRun() called");
			sleepTime = r.Next(2000);
			System.Threading.Thread.Sleep(sleepTime);	// to simulate thinking time			
		}

		public void done()
		{
			if(debug) print("SimpleTestRun.done() called");	
		}

		private void print(string msg)
		{
			Console.WriteLine(this.threadID + ": " + msg);
		}
	}
}
