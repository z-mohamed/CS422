using System;
using System.IO;
namespace CS422
{
	public class IndexedNumsStream : System.IO.Stream
	{
		
		byte length;

		long position;


		public IndexedNumsStream (long lengthIn)
		{
			//check for negative length, if so clamp to zero
			if (lengthIn < 0) 
			{
				lengthIn = 0;
			}

			//set length
			length = (byte)lengthIn;

			//set position
			position = 0;
			
		}


		public override long Length 
		{
			get 
			{
				return (long)length;	
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
			length = (byte) value;
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
				else if (value > (long)length) 
				{
					value = (long)length;

				}

				//set position
				position = value;
			}
		}


		public override int Read (byte[] buffer, int offset, int count)
		{
			long value;
			int i;

			//Keeep reading while count req. hasn't been meet
			for (i = 0; i < count; i++)
			{
				
				value = position % (long)256;

				buffer[offset+i] = (byte)value;

				position++;
			}
			//increment i so number of bytes read is not zero-based
			i++;
			
			return i;
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