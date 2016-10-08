using System;
using System.IO;

namespace CS422
{
	public class ConcatStream : System.IO.Stream
	{
		//member variables

		//refrences to first and second stream
		Stream frst;
		Stream scnd;

		//tracks position of ConcatStream
		long positionCS;

		//length of ConcatStream
		long length;

		//fixedLength
		long fixLength;

		//identifies the ctor that initilized the class by the number of arguments used
		int ctor;



		//CONSTRUCTOR 1
		public ConcatStream(Stream first, Stream second)
		{
			// set 1st ref
			frst = first;

			//if the first stream dosent support Length property throw ArgumentException
			if(!verifyStreamLengthProp(frst))
				throw new ArgumentException("First stream must support Length property!");

			//set 2nd ref
			scnd = second;

			//if the second stream supports the Length property we can
			// Calculate the ConcatStream Length
			if(verifyStreamLengthProp(scnd))
				length = frst.Length + scnd.Length;

			//if a stream supports seeking, seek to begin
			seek2Begin();


			positionCS = 0;
			ctor = 2;
		}

		//CONSTRUCTOR 2
		public ConcatStream(Stream first, Stream second, long fixedLength)
		{

			// set 1st ref
			frst = first;

			//if the first stream dosent support Length property throw ArgumentException
			if(!verifyStreamLengthProp(frst))
				throw new ArgumentException("First stream must support Length property!");

			//set 2nd ref
			scnd = second;

			//if the second stream supports the Length property we can
			// Calculate the ConcatStream Length
			if(verifyStreamLengthProp(scnd))
				length = frst.Length + scnd.Length;


			//if a stream supports seeking, seek streams to beginining 
			seek2Begin();

			positionCS = 0;
			fixLength = fixedLength;
			ctor = 3;
		}


		public override long Length
		{
			get
			{
				//if 2nd constructor was used
				if (ctor == 3)
				{
					//if second stream has a length property
					if (verifyStreamLengthProp (scnd))
					{
						//update length in case streams were modifies inbetween calls
						length = frst.Length + scnd.Length;
						return length;
					}
					// cant calculate length so just return the fixedLength argument
					else
						return fixLength;
				}
				// 1st constructor was used
				else
				{
					//if second stream has a length property
					if (verifyStreamLengthProp (scnd))
					{
					    //update length in case streams were modifies inbetween calls
						length = frst.Length + scnd.Length;
						return length;
					}
					//Kobe
					else
						throw new NotSupportedException ("Not Supported");
				}
			}
		}

		public override void SetLength (long value)
		{
			// Set Length is only supported if ConcatStream
			//          CanSeek and CanWrite.
			if (CanWrite && CanSeek)
			{
				//if specified value is larger then current length of stream
				//expand stream
				if (value > length)
				{
					//calculate new size of second stream
					value = value - frst.Length;

					//expand scnd stream
					scnd.SetLength(value);

					//setLength
					length = frst.Length + scnd.Length;
				}
				//else if specified value is less than current length of stream
				//truncate it
				else if(value < length)
				{
					// if specified value is greater than length
					// of first stream
					if (value > frst.Length)
					{
						//calculate new size of second stream
						value = value - frst.Length;

						//truncate scnd stream
						scnd.SetLength(value);

						//setLength
						length = frst.Length + scnd.Length;
					}
					// it must be less than or equal to 
					// the first stream
					else
					{
						//truncate scnd stream
						scnd.SetLength(0);
				
						//truncate first stream
						frst.SetLength(value);

						//setLength
						length = frst.Length;
					}
				}
				//else specified value is equal to stream length
				//so do nothing
				else
				{
					// This is me doing nothing...
					// why did I bother...

				}
			}
			// Kobe
			else
				throw new NotSupportedException ("Not Supported");
		}


		//Stream must Support Seeking to get or set position
		public override long Position
		{
			get
			{
				if(CanSeek)
					return positionCS;
				//Kobe
				else
					throw new NotSupportedException ("Not Supported");
			}
			set
			{
				if(CanSeek)
				{
					positionCS = value;
					setStreamPosition (frst, scnd, positionCS);
				}
				//Kobe
				else
					throw new NotSupportedException ("Not Supported");
			}
		}


		public override int Read (byte[] buffer, int offset, int count)
		{
			int read;

			// only bother with a read call if both stream support reading
			if (frst.CanRead && scnd.CanRead)
			{
				read = readWriteHandler (buffer, offset, count, "read");

				// return number of bytes read
				return read;
			}

			else
			{
				//Kobe
				throw new NotSupportedException ();

			}
		}


		public override void Write (byte[] buffer, int offset, int count)
		{
			if (frst.CanWrite && scnd.CanWrite)
			{
				
				readWriteHandler (buffer, offset, count, "write");

				//stream might have expanded so we update length
				if(verifyStreamLengthProp(scnd))
					length = frst.Length + scnd.Length;

			}

			else
			{
				//Kobe
				throw new NotSupportedException ();

			}
		}
			
		public override long Seek (long offset, SeekOrigin origin)
		{
			if (frst.CanSeek && scnd.CanSeek)
			{
				switch (origin)
				{
				//set position to beginning
				case SeekOrigin.Begin:
					positionCS = 0 + offset;
					setStreamPosition (frst, scnd, positionCS);
					break;

					//do nothing position is already current
				case SeekOrigin.Current:
					positionCS += offset;
					setStreamPosition (frst, scnd, positionCS);
					break;

					//set position to end of stream
				case SeekOrigin.End:
					positionCS = frst.Length + scnd.Length - 2;
					positionCS += offset;
					setStreamPosition (frst, scnd, positionCS);
					break;

				default:
					break;
				}

				//seek to offset from position
				//positionCS += offset;

				return positionCS;
			}

			else
			{
				//Kobe
				throw new NotSupportedException ();

			}
		}


		// Take with a grain of salt
		public override void Flush ()
		{
			frst.Flush ();
			scnd.Flush ();
			positionCS = 0;
			setStreamPosition (frst, scnd, positionCS);
		}

		//ConcatStream supports read only if both streams can read
		public override bool CanRead
		{
			get
			{
				if (frst.CanRead && scnd.CanRead)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		//ConcatStream supports seek only if both streams can seek
		public override bool CanSeek
		{
			get
			{
				if (frst.CanSeek && scnd.CanSeek)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		//ConcatStream supports write only if both streams can write
		public override bool CanWrite
		{
			get
			{
				if (frst.CanWrite && scnd.CanWrite)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}


		//utility functions

		//Verifies that  Length property of stream exists
		private bool verifyStreamLengthProp(Stream testStream)
		{
			long test;
			bool result;

			try
			{
				test = testStream.Length;
				result = true;
			}
			catch (NotSupportedException)
			{
				result = false;
			}

			return result;
		}

		// if seeking is supported for either stream, 
		// use it to seek to the begininig of either stream
		private void seek2Begin()
		{
			if (frst.CanSeek)
			{
				frst.Seek (0, SeekOrigin.Begin);
			}

			if (scnd.CanSeek)
			{
				frst.Seek(0, SeekOrigin.Begin);
			}
		}

		// given the position of the ConcatStream this function alters the position 
		// of the first and second stream
		// to insure that they are consistent
		private void setStreamPosition (Stream givenStream1, Stream givenStream2, long positionCS)
		{
			long streamSplit = givenStream1.Length - 1;

			if (positionCS <= streamSplit)
			{
				givenStream1.Position = positionCS;
				givenStream2.Position = 0;
			}
			else
			{
				givenStream1.Position = streamSplit;

				givenStream2.Position = positionCS - streamSplit -1;
			}
		}


		//checks if a write call can be completed
		bool canWrite()
		{
			//compute split index of ConcatStream
			long streamSplit = frst.Length - 1;

			bool result;

			//if we're dealing with the second stream
			if (positionCS > streamSplit)
			{
				//if second stream can seek
				if (scnd.CanSeek)
				{
					//seek to position that is relative to the current ConcatStream position
					scnd.Seek (positionCS - frst.Length, SeekOrigin.Begin);

					result = true;

				}
				//if position is on the nose (already at the correct position)
				else if (scnd.Position == positionCS - frst.Length)
				{
					result = true;
				}
				//cant write. FAIL!
				else
				{
					result = false;
				}
			}
			//We're dealing with the first  stream
			else
			{
				//if the postion is off
				if(frst.Position != positionCS)
				{
					//correct it
					frst.Seek (positionCS, SeekOrigin.Begin);
				}

				result = true;
			}


			return result;
		}


		//read and write handler
		private int readWriteHandler(byte[] buffer, int offset, int count, string command)
		{
			int i;
			for(i = 0; i < count; i++)
			{
				//if position in ConcatStream is within the Length of the first stream
				if (positionCS < frst.Length)
				{
					if (command == "read")
					{
						// read one byte from first stream
						int read = frst.Read(buffer,offset+i,1);

						//if nothing was read break
						if (read == 0)
							break;
					}

					else if (command == "write")
					{

						if(canWrite())
						{
							//write byte to first stream
							frst.Write(buffer, offset + i, 1);
						}
						else
						{
							//Not Kobe. Kobe is alway in position.
							throw new NotSupportedException ("Cannot write at current Position");
						}

					}
				}
				else
				{
					if (command == "read")
					{
						// read one byte from second stream
						int read = scnd.Read(buffer,offset+i,1);

						//if nothing was read break
						if (read == 0)
							break;
					}

					else if (command =="write")
					{
						if(canWrite())
						{
							//write byte to second stream
							scnd.Write(buffer, offset + i, 1);
						}

						else
						{
							//Not Kobe. Kobe is alway in position.
							throw new Exception ("Error Cannot Write. Position Properties out of Sort");
						}


					}
				}
				//increase position in ConcatStream
				positionCS++;
			}


			//return number of bytes read/written
			return i;



		}
	}
}
