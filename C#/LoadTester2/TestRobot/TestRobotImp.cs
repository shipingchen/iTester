using System;
using System.Diagnostics;
using System.Reflection;
using com.csiro.xml.datatype;


namespace TestRobot
{
	/// <summary>
	/// 
	/// </summary>
	public class TestRobotImp: MarshalByRefObject, Interfaces.ITestRobot
	{
		const bool debug = true;
		Config cmd;


		public TestRobotImp()
		{
			if(debug) Console.WriteLine("TestRobot.TestRobotImp() called");
		}

		public string ping(string msg)
		{
			if(debug) Console.WriteLine("ping( " + msg + " ) is called");

			string reply = "I got: " + msg;
			return reply;
		}

		public bool doit(Config cmd)
		{
			if(debug) 
			{
				Console.WriteLine("TestRobot.doit(cmd) is called");
				DataUtility.print(cmd);
			}

			this.cmd = cmd;

			try
			{
				if(debug) Console.WriteLine("testName = " + cmd.type);
				if(cmd.type.Equals("startTest")) 
				{
					this.startTest();
				}
				else if(cmd.type.Equals("getTestResult"))
				{
					this.getTestResult();
				}
				else
				{
					return false;
				}
			}
			catch(Exception e)
			{
				Console.WriteLine("TestRobot.doit(cmd): " + e.Message);
				return false;
			}

			return true;
		}

		#region Private Memebers

		private void startTest()
		{
			if(debug) Console.WriteLine("TestRobot.startTest() called");
					
			com.csiro.xml.datatype.Item item = DataUtility.getTheFirstItem(DataUtility.getTheFirstCatelog(cmd));

			string workDir = DataUtility.getPropertyValueByName(item, "workDir");
			string testFileName = DataUtility.getPropertyValueByName(item, "testFileName");
			string testConfigFileName = DataUtility.getPropertyValueByName(item, "testConfigFileName");

			if(debug)
			{
				Console.WriteLine("workDir = " + workDir);
				Console.WriteLine("testFileName " + testFileName);
				Console.WriteLine("testConfigFileName = " + testConfigFileName);
			}

			Process myProcess = new Process();
			
			myProcess.StartInfo.WorkingDirectory = workDir;
			myProcess.StartInfo.FileName = testFileName;
			myProcess.StartInfo.Arguments = "-f " +  testConfigFileName + " -t " + cmd.name;
			myProcess.StartInfo.CreateNoWindow = true;

			if(debug) Console.Write("To start: " + myProcess.StartInfo.FileName + " " + myProcess.StartInfo.Arguments);
			myProcess.Start();
			if(debug) Console.WriteLine("......OK");
		}

		private void getTestResult()
		{
			if(debug) Console.WriteLine("TestRobot.getTestResult() called");

			com.csiro.xml.datatype.Item item = DataUtility.getTheFirstItem(DataUtility.getTheFirstCatelog(cmd));
			string srcPath = DataUtility.getPropertyValueByName(item, "srcPath");
			string disPath = DataUtility.getPropertyValueByName(item, "disPath");

			Process myProcess = new Process();

			myProcess.StartInfo.FileName = "xcopy";
			myProcess.StartInfo.Arguments = "/Q /C /E " + srcPath + " " + disPath;
			myProcess.StartInfo.CreateNoWindow = false;

			if(debug) 
			{
				Console.Write("To do: " + myProcess.StartInfo.ToString());
			}
			myProcess.Start();
			if(debug) Console.WriteLine("......OK");
		}

		#endregion


		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion
	}
}
