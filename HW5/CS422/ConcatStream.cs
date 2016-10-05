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

			//if a stream supports seeking, seek to begin  
			seek2Begin();


			positionCS = 0;
			ctor = 2;
		}

		//CONSTRUCTOR 2
		public ConcatStream(Stream first, Stream second, long fixedLength)
		{
			positionCS = 0;
			length = fixedLength;
			ctor = 3;
		}
			



		public override long Length 
		{
			get {
				if (ctor == 3) {
					if (verifyStreamLengthProp (scnd))
						return frst.Length + scnd.Length;
					else
						return length;
				} else {
					if (verifyStreamLengthProp (scnd))
						return frst.Length + scnd.Length;
					else
						throw new NotSupportedException ("Length Property is not Suppoerted");
				}
			}


		}

		public override void SetLength (long value)
		{
			throw new NotImplementedException ();
		}	
			


		public override long Position 
		{
			get 
			{
				return positionCS;
			}
			set
			{

			}
		}


		// TESTTING REQUIRED!!!!!!!!!!!!
		public override int Read (byte[] buffer, int offset, int count)
		{
			int read;

			// end of stream exists flag
			bool EOS = false;

			// only bother with a read call if both stream support reading
			if (frst.CanRead && scnd.CanRead) 
			{
				read = readWriteHandler (buffer, offset, count, "read");
				// return number of bytes read
				return read;
			} 

			else 
			{
				
				throw new NotImplementedException ();

			}
		}


		public override void Write (byte[] buffer, int offset, int count)
		{
			//bool EOS = false;

			if (frst.CanWrite && scnd.CanWrite) 
			{
				
				// Implement Write Here

				int dummy = readWriteHandler (buffer, offset, count, "write");

			} 

			else 
			{
				
				throw new NotImplementedException ();

			}
		}

		// TESTTING REQUIRED!!!!!!!!!!!!
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
				
				throw new NotImplementedException ();

			}

			//return positionCS;
		}


		////ConcatStream does not support flush
		public override void Flush ()
		{
			throw new NotImplementedException ();
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

		//Verify Length property of stream exists
		private bool verifyStreamLengthProp(Stream testStream)
		{
			long test;
			bool result;

			try
			{
				test = testStream.Length;
				result = true;
			}
			catch (NotSupportedException e)
			{
				result = false;
			}

			return result;
		}

		//if seeking is supported it either stream, use it to seek to the begininig of either stream
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

		//Read a single byte from a given stream then returns it
		private byte readByte (Stream given)
		{
			byte[] buffer = new byte[4];

			// read one byte from stream and store it in buffer at position 0
			given.Read(buffer,0,1);

			return buffer[0];
		}

		private void writeByte(Stream given, byte[] buffer, int bufP)
		{
			//write byte to given stream from buffer at position bufp 
			given.Write(buffer, bufP, 1);

		}

		// given the position of the ConcatStream this function alters the position of the first and second stream
		// to insure that they are consistents
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

				givenStream2.Position = positionCS;
			}
		}


		//checks if a write call can be completed
		bool canWrite()
		{
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
				//if position is on the nose (already at the correct position
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
			//assume End of Stream cannot be calculated
			bool EOS = false;

			int i;

			//if second stream supports Length we can determine End Of Stream so set flag to indicate this
			if (verifyStreamLengthProp (scnd)) 
			{
				EOS = true;
			}

			for(i = 0; i < count; i++) 
			{
				//if position in ConcatStream is within the Length of the first stream
				if (positionCS <= frst.Length - 1) 
				{
					if (command == "read") 
					{	
						// read from first stream
						buffer [offset + i] = readByte (frst);
					} 

					else if (command =="write") 
					{

						if(canWrite())
						{
							int bufP = offset + i;
							//write byte to first stream
							writeByte(frst,buffer,bufP);
						}
						else
						{
							throw new Exception ("Error Cannot Write. Position Properties out of Sort");
						}

					}
					//increase position in ConcatStream
					positionCS++;

				}
				else 
				{
					//if end of stream is calculatable 
					if (EOS) 
					{
						// calculate end of stream
						long end = frst.Length + scnd.Length - 1;
						//check if we have reached end of stream
						//if so break out of loop
						if (positionCS > end) 
						{
							break;

						}

					}

					if (command == "read") 
					{	
						// read from first stream
						buffer [offset + i] = readByte (scnd);
					} 

					else if (command =="write") 
					{	
						if(canWrite())
						{	
							//get position of byte to be written
							int bufP = offset + i;
							//write byte to second stream
							writeByte(scnd,buffer,bufP);
						}

						else
						{
							throw new Exception ("Error Cannot Write. Position Properties out of Sort");
						}


					}
					//increase position in ConcatStream
					positionCS++;
				}
			}


			return i;



		}
	}
}