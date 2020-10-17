using System;
using com.csiro.xml.datatype;


namespace Interfaces
{
	/// <summary>
	/// This contains interfaces required by a test reobot 
	/// </summary>
	public interface ITestRobot: IDisposable
	{
		string  ping(string msg);	// for test
		bool doit(Config cmd);		// General cmd API
	}
}
