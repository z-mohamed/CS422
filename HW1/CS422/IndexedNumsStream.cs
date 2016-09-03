using System;
using System.IO;
namespace CS422
{
	public class IndexedNumsStream : System.IO.Stream
	{
		
		long length;

		long position;


		public IndexedNumsStream (long lengthIn)
		{
			//check for negative length, if so clamp to zero
			if (lengthIn < 0) 
			{
				lengthIn = 0;
			}

			//set length
			length = lengthIn;

			//set position
			position = 0;
			
		}


		public override long Length 
		{
			get 
			{
				return length;	
			}
		}


		public override void SetLength (long value)
		{
			//check if value is negative, if so clamp to zero
			if (value < 0) 
			{
				value = 0;
			}

			//set length
			length =  value;
		}	


		public override long Position 
		{
			get 
			{
				return position;
			}

			set 
			{
				//check if value is negative, if so clamp to zero
				if (value < 0) 
				{
					value = 0;
				} 
				//check if value is greater than length, if so clamp it to length
				else if (value > length) 
				{
					value = length;

				}

				//set position
				position = value;
			}
		}


		public override int Read (byte[] buffer, int offset, int count)
		{
			int read = 0;

			//Keeep reading while count req. hasn't been meet
			for (int i = 0; i < count ; i++)
			{
				var value = (byte) (position % 256);


				buffer[offset + i] = value;
				read++;
				position++;

				//if position moves past length of stream stop reading
				if (position >= length) 
				{
					break;
				}
			}

			return read;
		}


		public override void Write (byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException ();
		}


		public override long Seek (long offset, SeekOrigin origin)
		{
			switch (origin) 
			{
			//set position to beginning
			case SeekOrigin.Begin:
				position = 0;
				break;
			
			//do nothing position is already current
			case SeekOrigin.Current:
				break;
			
			//set position to end of stream
			case SeekOrigin.End:
				position = length;
				break;

			default:
				break;
			}

			//seek to offset from position
			position += offset;

			return position;

		}



		public override void Flush ()
		{
			throw new NotImplementedException ();
		}


		public override bool CanRead 
		{
			get 
			{
				return true;
			}
		}


		public override bool CanSeek 
		{
			get 
			{
				return true;
			}
		}


		public override bool CanWrite 
		{
			get 
			{
				return false;
			}
		}
	}
}