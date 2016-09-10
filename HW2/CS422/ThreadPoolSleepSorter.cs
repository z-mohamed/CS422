using System;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;
namespace CS422
{
	public class ThreadPoolSleepSorter : IDisposable
	{
		
		BlockingCollection<Task> coll =  new BlockingCollection<Task>();
		TextWriter writer;
		Thread t;

		public ThreadPoolSleepSorter (TextWriter output, ushort threadCount)
		{
			//set textwriter
			writer = output;

			//Default thread count = 64
			if (threadCount == 0)
				threadCount = 64;
			
			// Create number of threads specified by thread count 
			for (int i = 0; i < threadCount; i++) 
			{
				t = new Thread (new ParameterizedThreadStart (ThreadDoWork));
				t.Start (coll);

			}
		}


		//Take threads from the blocking collection and put them to work
		public void ThreadDoWork (object _generic_coll)
		{
			var _blocking_coll = (BlockingCollection<Task>) _generic_coll;

			while (!coll.IsCompleted) 
			{
				try
				{
					_blocking_coll.Take().RunTask(writer);

				}
				catch (Exception ex){}
			}

		}

		//for every value in the byte array create a new task and add it to the blocking collection
		public void Sort(byte[] values)
		{
			foreach (byte value in values) 
			{
				coll.Add(new Task { _value = value });

			}

		}

		//Sleep for a specified interval then witeline to textwriter
		public class Task
		{
			public byte _value { get; set;}

			public void RunTask(TextWriter writer)
			{
				Thread.Sleep (_value * 1000);
				writer.WriteLine (_value);


			}
				
		}

		//free all threads
		public void Dispose()
		{
			coll.CompleteAdding();
			t.Join ();

		}
	}
}

