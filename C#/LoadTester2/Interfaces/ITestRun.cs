using System;
using com.csiro.xml.datatype;



namespace Interfaces
{
	/// <summary>
	/// This contains all interfaces required by LoadTester2
	/// </summary>
	public interface ITestRun
	{
		void	init(Config confg);								// prepare at begining of test
		void	preRun();										// do something before each test run
		int		run();											// test run for single measurment
		void	run(ref string[] pname, ref long[] pvalue);		// test run for multiple measurement
		void	postRun();										// do something after each test run
		void	done();											// do something at end of test
	}
}
