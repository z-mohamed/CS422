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
			get 
			{
				if (true) 
				{
					return length;
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


		public override int Read (byte[] buffer, int offset, int count)
		{
			bool flag = true;
			bool EOS = false;

			// only bother with a call if both stream support reading
			if (frst.CanRead && scnd.CanRead) 
			{
				//if second stream supports Length we can determine End Of Stream
				if (verifyStreamLengthProp (scnd)) 
				{
					EOS = true;
				}

				while (flag) 
				{
					//if position in ConcatStream is within the Length of the first stream
					if (positionCS < frst.Length - 1) 
					{
						// read from first stream
					}
					else 
					{
						//if end of stream is calculatable 
						if (EOS) 
						{
							// calculate end of stream
							long end = frst.Length + scnd.Length;
							//check if we have reached end of stream
							//if so break out of loop
							if (positionCS >= end) 
							{
								break;

							}

						}
						// read from second stream
					}
				}
			} 

			else 
			{
				
				throw new NotImplementedException ();

			}

			return 0;
		}


		public override void Write (byte[] buffer, int offset, int count)
		{
			if (frst.CanWrite && scnd.CanWrite) 
			{
				
				// Implement Write Here

			} 

			else 
			{
				
				throw new NotImplementedException ();

			}
		}


		public override long Seek (long offset, SeekOrigin origin)
		{
			if (frst.CanSeek && scnd.CanSeek) 
			{

				// Implement Seek Here

			} 

			else 
			{
				
				throw new NotImplementedException ();

			}

			return 0;
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


	}
}