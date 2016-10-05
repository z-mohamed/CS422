using NUnit.Framework;
using System;
using CS422;
using System.IO;

namespace UnitTests
{
	[TestFixture ()]
	public class Test
	{
		[Test ()]
		public void staticStreamSetUpGood ()
		{
			byte[] writeBuffer = new byte[14];
			byte[] readBuffer1 = new byte[14];
			byte[] readBuffer2 = new byte[14];

			MemoryStream s1 = new MemoryStream();
			MemoryStream s2 = new MemoryStream();
			ConcatStream testCS = new ConcatStream(s1,s2);


			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}

			s1.Write (writeBuffer, 0, 6);

			s2.Write (writeBuffer, 6, 8);

			s1.Seek (0, SeekOrigin.Begin);
			s2.Seek (0, SeekOrigin.Begin);

			s1.Read (readBuffer1, 0, 6);
			s2.Read (readBuffer2, 6, 8);




			for (int i = 0; i < writeBuffer.Length; i++) 
			{
				if (i <= 5) 
				{
					Assert.AreEqual (readBuffer1 [i], writeBuffer [i]);
				} 
				else 
				{
					Assert.AreEqual (readBuffer2 [i], writeBuffer [i]);
				}
			}


		}

		[Test ()]
		public void basicConcatStreamRead()
		{
			byte[] writeBuffer = new byte[14];
			byte[] readBuffer = new byte[14];

			MemoryStream s1 = new MemoryStream();
			MemoryStream s2 = new MemoryStream();
			ConcatStream testCS = new ConcatStream(s1,s2);


			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}

			s1.Write (writeBuffer, 0, 6);

			s2.Write (writeBuffer, 6, 8);

			s1.Seek (0, SeekOrigin.Begin);
			s2.Seek (0, SeekOrigin.Begin);

			testCS.Read (readBuffer, 0, 14);

			for (int i = 0; i < readBuffer.Length; i++) 
			{
				Assert.AreEqual(readBuffer[i], writeBuffer [i]);
			}


		

		}

		[Test ()]
		public void basicConcatStreamSeek()
		{
			byte[] writeBuffer = new byte[14];
			byte[] readBuffer = new byte[14];

			MemoryStream s1 = new MemoryStream();
			MemoryStream s2 = new MemoryStream();
			ConcatStream testCS = new ConcatStream(s1,s2);


			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}

			s1.Write (writeBuffer, 0, 6);

			s2.Write (writeBuffer, 6, 8);

			s1.Seek (0, SeekOrigin.Begin);
			s2.Seek (0, SeekOrigin.Begin);

			testCS.Read (readBuffer, 0, 14);


			Assert.AreEqual (14, testCS.Position);

			testCS.Seek (0, SeekOrigin.Begin);

			Assert.AreEqual(0, testCS.Position);
			Assert.AreEqual(0, s1.Position);
			Assert.AreEqual(0, s2.Position);



		}

		[Test ()]
		public void basicConcatStreamWrite()
		{
			//set up buffers
			byte[] writeBuffer = new byte[14];
			byte[] readBuffer1 = new byte[14];
			byte[] readBuffer2 = new byte[14];


			//setUp streams
			MemoryStream s1 = new MemoryStream();
			MemoryStream s2 = new MemoryStream();
			ConcatStream testCS = new ConcatStream(s1,s2);


			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}


			s1.Write (writeBuffer, 0, 6);

			s2.Write (writeBuffer, 6, 8);

			s1.Seek (0, SeekOrigin.Begin);
			s2.Seek (0, SeekOrigin.Begin);

			for(int i = 14; i < 28; i++)
			{
				writeBuffer[i-14] = Convert.ToByte(i);
			}


			testCS.Write (writeBuffer, 0, 14);

			s1.Seek (0, SeekOrigin.Begin);
			s2.Seek (0, SeekOrigin.Begin);

			s1.Read (readBuffer1, 0, 6);
			s2.Read (readBuffer2, 6, 8);




			for (int i = 0; i < writeBuffer.Length; i++) 
			{
				if (i <= 5) 
				{
					Assert.AreEqual (readBuffer1 [i], writeBuffer [i]);
				} 
				else 
				{
					Assert.AreEqual (readBuffer2 [i], writeBuffer [i]);
				}
			}




		}

		[Test]
		public void randomConcatStreamRead()
		{
			
			Random rnd =  new Random();

			int x = 0;
			int ceiling = 14;
			int offset = 0;

			x = rnd.Next (x-1,ceiling);

			//set up buffers
			byte[] writeBuffer = new byte[14];
			byte[] readBuffer = new byte[14];


			//setUp streams
			MemoryStream s1 = new MemoryStream ();
			MemoryStream s2 = new MemoryStream ();
			ConcatStream testCS = new ConcatStream (s1, s2);


			for (int i = 0; i < 14; i++) 
			{
				writeBuffer [i] = Convert.ToByte (i);
			}


			s1.Write (writeBuffer, 0, 6);

			s2.Write (writeBuffer, 6, 8);

			s1.Seek (0, SeekOrigin.Begin);
			s2.Seek (0, SeekOrigin.Begin);
			bool flag = true;
			while (flag) 
			{
				testCS.Read (readBuffer, offset, x);
				offset = offset + x;
				//ceiling--;
				int temp = x;
				x = rnd.Next (x,ceiling);
				if (x == temp)
					flag = false;
			}

			Assert.AreEqual (0, ceiling);

			for (int i = 0; i < readBuffer.Length; i++) 
			{
				Assert.AreEqual(readBuffer[i], writeBuffer [i]);
			}



		}

	}
}

