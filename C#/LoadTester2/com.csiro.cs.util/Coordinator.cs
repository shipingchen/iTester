using System;
using System.Threading;

namespace com.csiro.cs.util
{
	/// <summary>
	/// This class is to synchronise the activites between client-threads.
	/// </summary>

	public class Coordinator
	{
		// private
		int		counter;
		int		numClients;

		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="numClients">The amount of clients tobe coordinated</param>
		public Coordinator(int numClients)
		{
			this.numClients = numClients;
			this.counter = 0;
		}

		/// <summary>
		/// increase the counter threads-safely 
		/// </summary>
		public void inc()
		{
			Interlocked.Increment(ref counter);
		}

		/// <summary>
		/// if all the clients are ready
		/// </summary>
		/// <returns>When all the clients are ready return true; otherwise false</returns>
		public bool isReady()
		{
			if(counter>=numClients) return true;
			System.Threading.Thread.Sleep(15);	// 15 msec.
			if(counter>=numClients) return true;
			else					return false;
		}
	}
}
