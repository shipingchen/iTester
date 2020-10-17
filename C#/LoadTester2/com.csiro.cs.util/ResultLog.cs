using System;
using System.Threading;


namespace com.csiro.cs.util
{
	/// <summary>
	/// High resolution log class for logging testing results
	/// </summary>
	[Serializable]
	public class ResultLog
	{
		const bool debug = false;

		string  name = "";
		int		counter = 0;			// Sample conuter
		double	sumValue = 0;			// Sume of sample values
		double	maxValue = 0;			// so that it can be reset with a bigger value
		double	minValue = 999999999;	// so that it can be reset with a samller value;
		int[]	valueDistribution;

		double	disUnit = 20;			// default 20 msec, due to the timer resoluation (20 msec).
		int		disSize = 50;			// default 50 bucks for distribution.
		int		showSize = 50;
		double  disMax;					// disMax = disUnit * disSize;
		double	startValue = 0;
		object  lockObj = new object();  // lock object for syncronize

		/// <summary>
		/// the default constructor is for xml serialization
		/// </summary>
		public ResultLog() :this ("serialization")
		{
		}

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="name">the name of this result</param>
		public ResultLog(string name) 
			:this(name, 20, 0, 1000,50)
			// bucketSize = 20; startCaptureValue = 0; maxCaptureValue = 1000; bucketNumber = 50;
		{
		}
		public ResultLog(string name, double bucketSize) 
			:this(name, bucketSize, 0, 1000, 50)
			// startCaptureValue = 0; maxCaptureValue = 1000; bucketNumber = 50;
		{
		}
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="name">the name of this result</param>
		/// <param name="bucketSize">the size of each counter bucket</param>
		/// <param name="startCaptureValue">The minimal value of counter buckets</param>
		public ResultLog(string name, double bucketSize, double startCaptureValue) 
			: this(name, bucketSize, startCaptureValue, 50, 50)
		{
		}
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="name">the name of this result</param>
		/// <param name="bucketSize">the size of each counter bucket</param>
		/// <param name="startCaptureValue">The minimal value of counter buckets</param>
		/// <param name="maxCaptureValue">The maximal value of counter buckets</param>
		public ResultLog(string name, double bucketSize, double startCaptureValue, double maxCaptureValue)
			: this(name,bucketSize, startCaptureValue, maxCaptureValue, 50)
		{
		}
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="name">the name of this result</param>
		/// <param name="bucketSize">the size of each counter bucket</param>
		/// <param name="startCaptureValue">The minimal value of counter buckets</param>
		/// <param name="maxCaptureValue">The maximal value of counter buckets</param>
		/// <param name="bucketNumber">the amount of buckets to show off</param>
		public ResultLog(string name, double disUnit, double startCaptureValue, double maxCaptureValue, int bucketNumber)
		{
			this.name = name;
			this.disUnit = disUnit;
			startValue = startCaptureValue;
			disSize = (int)((maxCaptureValue - startCaptureValue) / disUnit + 0.5);
			if(disSize<bucketNumber) disSize= bucketNumber;
			showSize = bucketNumber;
/*
			if(debug) 
			{
				Console.WriteLine("name              = " + name);
				Console.WriteLine("disUnit           = " + disUnit);
				Console.WriteLine("startCaptureValue = " + startCaptureValue);
				Console.WriteLine("maxCaptureValue   = " + maxCaptureValue);
				Console.WriteLine("bucketNumber      = " + bucketNumber);
				Console.WriteLine("disSize           = " + disSize);
				Console.WriteLine("disUnit*disSize   = " + disUnit*disSize);
			}
*/
			init();
		}

		private void init()
		{
			disMax = disUnit*disSize + startValue;
			valueDistribution = new int[disSize+2];

			for(int i=0; i<=disSize; i++)
				valueDistribution[i] = 0;
		}

		/// <summary>
		/// add a value into this log
		/// This method IS thread-safe, don't worry :-)
		/// </summary>
		/// <param name="value1">one sample to add to the current resultlog</param>
		public void add(double value1)
		{
			Monitor.Enter (lockObj);

			sumValue += value1;
			counter++;

			if(value1 > maxValue) maxValue = value1;
			if(value1 < minValue) minValue = value1;

			setDistribution(value1);
			Monitor.Exit(lockObj);
		}

		/// <summary>
		/// add a whole resultlog into this one
		/// This method IS thread-safe, don't worry :-)
		/// </summary>
		/// <param name="rlog"></param>
 		public void add(ResultLog rlog)
		{
			if(rlog!=null)
			{
				Monitor.Enter (lockObj);
				sumValue += rlog.sumValue;
				counter += rlog.counter;

				if(rlog.maxValue > maxValue) this.maxValue = rlog.maxValue;
				if(rlog.minValue < minValue) this.minValue = rlog.minValue;

				for(int i=0; i<=disSize; i++)
					this.valueDistribution[i] += rlog.valueDistribution[i];
				Monitor.Exit(lockObj);
			}
		}

		/// <summary>
		/// add a whole array of resultlogs into this one
		/// This method IS thread-safe
		/// </summary>
		/// <param name="rlogs">An array of resultlogs</param>
		public void add(ResultLog[] rlogs)
		{
			if(rlogs!=null)
			{
				for(int i=0;i<rlogs.Length;i++)
				{
					add(rlogs[i]);
				}
			}
		}

		/// <summary>
		/// get average value of all samples
		/// </summary>
		/// <returns>average value of all samples</returns>
		public string getName()
		{
			return name;
		}

		/// <summary>
		/// get average value of all samples
		/// </summary>
		/// <returns>average value of all samples</returns>
		public double getAve()
		{
			return (counter == 0) ? 0 : sumValue/counter ;
		}

		/// <summary>
		/// get the sum of total samples
		/// </summary>
		/// <returns>the sum of total samples</returns>
		public double getTotal()
		{
			return sumValue;
		}
        /// <summary>
        /// the amount of samples
        /// </summary>
        /// <returns></returns>
		public long getCounter()
		{
			return counter;
		}
    
		/// <summary>
		/// the max value of samples
		/// </summary>
		/// <returns></returns>
		public double getMax()
		{
			return maxValue;
		}
    
		/// <summary>
		/// the minimal value of samples
		/// </summary>
		/// <returns></returns>
		public double getMin()
		{
			return minValue;
		}

		/// <summary>
		/// the x-th value
		/// </summary>
		/// <returns></returns>
		public double getVauleByPercent(double x)
		{
			int i=0;

			for(int sum=0; i<valueDistribution.Length; i++)
			{
				sum += valueDistribution[i];
				// Console.WriteLine(" sum = " + sum + " counter = " + counter + "sum/counter = " + (double)sum/(double)counter);
				if(((double)sum/(double)counter)>x) 
				{
					break;
				}
			}

			if(i==0) i=1;

			return (disUnit*i - disUnit/2.0);
		}
		
		/// <summary>
		/// print settings to the console
		/// </summary>
		public void printSetting() 
		{
			printSetting(System.Console.Out);
		}
		
		/// <summary>
		/// print settings to a textwriter
		/// </summary>
		/// <param name="writer">The destination to print</param>
		public void printSetting(System.IO.TextWriter writer) 
		{
			if(writer!=null)
			{
				writer.WriteLine( "====== " + name + " Settings ======");
				writer.WriteLine ("disUnit = " + disUnit);
				writer.WriteLine ("disSize = " + disSize);
				writer.WriteLine ("disStart= " + startValue);
				writer.WriteLine ("disMax  = " + disMax);
			}
		}
		
		/// <summary>
		/// print the result to a textwriter
		/// </summary>
		/// <param name="writer">The destination to print</param>
		/// <param name="bPrintDistrib">if true, print the distribution of result</param>
		/// <param name="bPrintAllNonZero">if print all the non-zero result
		/// if (bPrintAllNonZero is true) print all non-zero distribution;
		/// if (bPrintAllNonZero is false) only top DistributionCount(default is 50) fullest distribution printed out </param>
		public void printResult(System.IO.TextWriter writer,bool bPrintDistrib,bool bPrintAllNonZero)
		{
			if(writer != null)
			{
				writer.WriteLine( getResultHeader);
				writer.WriteLine ("counter  = " + counter);
				writer.WriteLine ("minValue = " + (counter>0? minValue:0));
				writer.WriteLine ("aveValue = " + getAve());
				writer.WriteLine ("maxValue = " + maxValue);
			
				if( bPrintDistrib )
				{
					// printAllDistribution(writer);
					_printDistribution(writer, true);
				}

				writer.WriteLine();
			}
		}
		/// <summary>
		/// print out the result to the console
		/// only top DistributionCount(default is 50) fullest distribution printed out
		/// </summary>
		/// <param name="bPrintDistrib">if true, print the distribution of result</param>
		public void printResult(bool bPrintDistrib) 
		{
			printResult(System.Console.Out, bPrintDistrib, false);
		}

		/// <summary>
		/// print the result to the console
		/// only top DistributionCount(default is 50) fullest distribution printed out 
		/// </summary>
		public void printDistribution() 
		{
			_printDistribution(System.Console.Out, false ); // only top 50 printed out
		}

		public void printAllDistribution(System.IO.TextWriter writer)
		{
			writer.WriteLine("----------------------------------------.");
			writer.WriteLine("No.\t\t| Value\t\t| Count\t|"); 
			writer.WriteLine("----------------------------------------|");
			for(int i=0; i<=disSize; i++)
			{
				writer.WriteLine (i + "\t\t " + (i*disUnit + startValue) + "\t\t " + valueDistribution[i]+ "\t");
			}
		}

		#region private methods

		private string getResultHeader
		{
			get
			{
				return "====== " + name +  " distribution ======" ;
			}
		}

		private void _printDistribution(System.IO.TextWriter writer, bool bPrintAllNonZero)
		{
			int val = 1;

			if(!bPrintAllNonZero)
			{
				if(disSize > showSize)
				{
					val = getShowValue();
				}
				if(0==val) val = 1;
			}
			writer.WriteLine("----------------------------------------.");
			writer.WriteLine("No.\t\t| Value\t\t| Count\t|"); 
			writer.WriteLine("----------------------------------------|");
			for(int i=0; i<=disSize; i++)
			{
				if(valueDistribution[i] >=val)
					writer.WriteLine (i + "\t\t " 
						+ (i*disUnit + startValue) + "\t\t " + valueDistribution[i]+ "\t");
			}
			writer.WriteLine("----------------------------------------'");
		}

		private int getShowValue()
		{
			int val = counter/showSize;
			int num = getNumGreaterThanThis(val);
			if(num == showSize) return val;
			else if(num < showSize) 
			{
				if(val <=0) return 0;
				else return getShowValue(0,val-1);
			}
			else // num > showSize 
				return getShowValue(val,counter/2);
		}

		private int getShowValue(int minval,int maxvalue)
		{
			if(minval>=maxvalue) return minval;
			int val = (minval+maxvalue)/2;
			int num = getNumGreaterThanThis(val);
			if(num == showSize) return val;
			else if(num < showSize) 
			{
				if(val <= minval+1) return minval;
				else return getShowValue(minval,val-1);
			}
			else // num > showSize 
			{
				//num = getNumGreaterThanThis(val+1);
				//if(num<)
				if(val+1==maxvalue)
				{
					if(getNumGreaterThanThis(maxvalue)<showSize) return val;
					else return maxvalue;
				}
				else 
					return getShowValue(val,maxvalue);
			}
		}

		private int getNumGreaterThanThis(int val)
		{
			int count = 0;
			for(int i=0; i<=disSize; i++)
			{
				if( valueDistribution[i] >= val ) count ++;
			}
			return count;
		}

		// private methods
		private void setDistribution(double value1) 
		{
			if(value1<startValue)  value1 = startValue;
			else if(value1>disMax) value1 = disMax;

			int index = (int)((value1-startValue)/disUnit);
			valueDistribution[index]++;
			// if(debug) System.Console.WriteLine ("setLatencyDis(): valueDistribution[ " + index + " ] = " + valueDistribution[index]);
		}
		#endregion

	}
}
