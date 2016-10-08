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
			




		[Test ()]
		public void basicNoSeekConcatStreamRead()
		{
			byte[] writeBuffer = new byte[14];
			byte[] writeBuffer2 = new byte[8];
			byte[] readBuffer = new byte[14];

			MemoryStream s1 = new MemoryStream();



			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}

			s1.Write (writeBuffer, 0, 6);

			//s2.Write (writeBuffer, 6, 8);
			for(int i = 0; i < 8; i++)
			{
				writeBuffer2[i] = Convert.ToByte(i+6);
			}

			NoSeekMemoryStream s2 = new NoSeekMemoryStream(writeBuffer2);
			ConcatStream testCS = new ConcatStream(s1,s2);

			s1.Seek (0, SeekOrigin.Begin);
			//s2.Seek (0, SeekOrigin.Begin);

			testCS.Read (readBuffer, 0, 14);

			s1.Seek (0, SeekOrigin.Begin);

			for (int i = 0; i < readBuffer.Length; i++) 
			{
				Assert.AreEqual(readBuffer[i], writeBuffer [i]);
			}
		}


		[Test ()]
		public void basicConcatStreamlengthProperty()
		{

			byte[] writeBuffer = new byte[14];

			MemoryStream s1 = new MemoryStream();
			MemoryStream s2 = new MemoryStream();
			ConcatStream testCS = new ConcatStream(s1,s2);



			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}

			s1.Write (writeBuffer, 0, 6);
			s2.Write (writeBuffer, 6, 8);


			long length = testCS.Length;

			Assert.AreEqual (14, length);
		}

		[Test ()]
		public void fixedConcatStreamlengthProperty()
		{

			byte[] writeBuffer = new byte[14];

			MemoryStream s1 = new MemoryStream();
			NoSeekMemoryStream s2 = new NoSeekMemoryStream(writeBuffer);
			ConcatStream testCS = new ConcatStream(s1,s2, 28);



			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}

			s1.Write (writeBuffer, 0, 6);
			s2.Write (writeBuffer, 6, 8);


			long length = testCS.Length;

			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}


			Assert.AreEqual (28, length);
		}

		[Test ()]
		public void NoSeekMemoryStreamSeek()
		{

			byte[] writeBuffer = new byte[14];


			NoSeekMemoryStream test = new NoSeekMemoryStream(writeBuffer);

			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}



			test.Write (writeBuffer, 0, 6);


			try
			{
				test.Seek(0,SeekOrigin.Begin);
			}
			catch (NotSupportedException e)
			{
				Assert.AreEqual ("Not Supported",e.Message);
			}
		}


		[Test ()]
		public void NoSeekMemoryStreamSetPosition()
		{

			byte[] writeBuffer = new byte[14];


			NoSeekMemoryStream test = new NoSeekMemoryStream(writeBuffer);

			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}



			test.Write (writeBuffer, 0, 6);


			try
			{
				test.Position = 0;
			}
			catch (NotSupportedException e)
			{
				Assert.AreEqual ("Not Supported",e.Message);
			}
		}


		[Test ()]
		public void NoSeekMemoryStreamLengthGet()
		{

			byte[] writeBuffer = new byte[14];


			NoSeekMemoryStream test = new NoSeekMemoryStream(writeBuffer);

			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}



			test.Write (writeBuffer, 0, 6);


			try
			{
				long x;
				x = test.Length;
			}
			catch (NotSupportedException e)
			{
				Assert.AreEqual ("Not Supported",e.Message);
			}
		}



		[Test ()]
		public void NoSeekMemoryStreamLengthSet()
		{

			byte[] writeBuffer = new byte[14];


			NoSeekMemoryStream test = new NoSeekMemoryStream(writeBuffer);

			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}



			test.Write (writeBuffer, 0, 6);


			try
			{
				test.SetLength(7);
			}
			catch (NotSupportedException e)
			{
				Assert.AreEqual ("Not Supported",e.Message);
			}
		}

		[Test ()]
		public void NoSeekMemoryStreamCanSeekProp()
		{

			byte[] writeBuffer = new byte[14];


			NoSeekMemoryStream test = new NoSeekMemoryStream(writeBuffer);

			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}



			test.Write (writeBuffer, 0, 6);


			Assert.AreEqual (false, test.CanSeek);
		}




		[Test ()]
		public void ConcatStreamExpandLength()
		{
			byte[] writeBuffer = new byte[14];

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
			long s2_length = s2.Length;

			testCS.SetLength (15);


			Assert.AreEqual (s2_length + 1, s2.Length);
			Assert.AreEqual (15, testCS.Length);

		}

		[Test ()]
		public void ConcatStreamTruncateLength()
		{
			byte[] writeBuffer = new byte[14];

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

			long s2_length = s2.Length;
			long l = testCS.Length;
			testCS.SetLength (5);


			//Assert.AreNotEqual (s2_length, s2.Length);
			Assert.AreEqual (5, testCS.Length);

		}


		[Test ()]
		public void ConcatStreamExpanded()
		{
			byte[] writeBuffer = new byte[14];

			MemoryStream s1 = new MemoryStream();

			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}

			s1.Write (writeBuffer, 0, 6);

			MemoryStream s2 = new MemoryStream();


			ConcatStream testCS = new ConcatStream(s1,s2);

			testCS.Position = 6;

			long pre_s2_length = s2.Length;

			testCS.Write (writeBuffer, 6, 8);

			long post_s2_length = s2.Length;


			//Assert.AreNotEqual (s2_length, s2.Length);
			Assert.AreEqual (pre_s2_length, 0);

			Assert.AreEqual (post_s2_length, 8);

		}


		[Test ()]
		public void EmptyConcatStreamWrite()
		{
			byte[] writeBuffer = new byte[14];

			MemoryStream s1 = new MemoryStream();
			MemoryStream s2 = new MemoryStream();

			ConcatStream testCS = new ConcatStream(s1,s2);

			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}

			testCS.Write (writeBuffer, 0, writeBuffer.Length);

			long length = testCS.Length;

			Assert.AreEqual (14, length);

			s2.Seek (0, SeekOrigin.Begin);

			for (int i = 0; i < 14; i++) 
			{
				byte[] buffer = new byte[1];
			    s2.Read (buffer, 0, 1);
				Assert.AreEqual (i, buffer [0]);

			}


		}



		//wont catch exceptiopm
		public void fixedConcatStreamlengthProperty2nd()
		{

			byte[] writeBuffer = new byte[14];

			MemoryStream s1 = new MemoryStream();
			NoSeekMemoryStream s2 = new NoSeekMemoryStream(writeBuffer);
			ConcatStream testCS = new ConcatStream(s1,s2);


			long length = testCS.Length;

			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}
				

			try
			{
				long l;
				l = testCS.Length;

			}
			catch (NotSupportedException e)
			{
				Assert.AreEqual ("Length Property is not Supported",e.Message);
			}
		}



		[Test ()]
		public void randomConcatStreamRead()
		{

			Random rnd =  new Random();

			int read = 0;
			int ceiling = 14;
			int offset = 0;
			int floor = 1;
			read = rnd.Next (floor,ceiling - offset);

			//set up buffers
			byte[] writeBuffer = new byte[14];
			byte[] readBuffer = new byte[14];


			//setUp streams
			MemoryStream s1 = new MemoryStream ();
			MemoryStream s2 = new MemoryStream ();

			for (int i = 0; i < 14; i++) 
			{
				writeBuffer [i] = Convert.ToByte (i);
			}


			s1.Write (writeBuffer, 0, 6);
			s2.Write (writeBuffer, 6, 8);

			s1.Seek (0, SeekOrigin.Begin);
			s2.Seek (0, SeekOrigin.Begin);

			ConcatStream testCS = new ConcatStream (s1, s2);

			while (ceiling != offset) 
			{
				
				testCS.Read (readBuffer, offset, read);

				for (int i = 0; i < read; i++) 
				{
					Assert.AreEqual (writeBuffer [offset + i], readBuffer [offset + i]);

				}

				offset += read;

				if(offset != ceiling)
				{
					read = rnd.Next (floor,ceiling - offset);
				}


					
			}


		}


		[Test]
		public void boundaryRead()
		{
			byte[] writeBuffer = new byte[14];
			byte[] readBuffer = new byte[1];

			int read1;
			int read2;
			MemoryStream s1 = new MemoryStream();
			MemoryStream s2 = new MemoryStream();

			for(int i = 0; i < 14; i++)
			{
				writeBuffer[i] = Convert.ToByte(i);
			}

			s1.Write (writeBuffer, 0, 6);

			s2.Write (writeBuffer, 6, 8);

			s1.Seek (0, SeekOrigin.Begin);
			s2.Seek (0, SeekOrigin.Begin);

			ConcatStream testCS = new ConcatStream(s1,s2);

			long length = testCS.Length;

			testCS.Seek (length - 1, SeekOrigin.Begin);

			long position = testCS.Position;

			read1 = testCS.Read (readBuffer, 0, 1);

			read2 = testCS.Read (readBuffer, 0, 1);

			int x = 0;

			Assert.AreEqual (13, readBuffer [0]);


		}

	}
}

