using System;

namespace CS422
{
	public class IndexedNumsStream : System.IO.Stream
	{
		
		byte length;

		long position;


		public IndexedNumsStream (long lengthIn)
		{
			if (lengthIn < 0) 
			{
				lengthIn = 0;
			}

			length = (byte)lengthIn;

			//Why is the setting the position not necessary
			position = (long)0;
			
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
			if (value < 0) 
			{
				value = 0;
			}

			length = (byte)value;
		}	


		public override long Position 
		{
			get 
			{
				return position;
			}

			set 
			{
				if (value < 0) 
				{
					value = 0;
				} 
				else if (value > length) 
				{
					value = length;

				}

				position = value;
			}
		}


		public override int Read (byte[] buffer, int offset, int count)
		{
			
			for (long i = offset; i < count && (byte)position < length; i++)
			{
				
				long value = position % (long)256;

				buffer [i] = (byte) value;

				position++;
			}

			return count;
		}


		public override void Write (byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException ();
		}


		public override long Seek (long offset, System.IO.SeekOrigin origin)
		{
			throw new NotImplementedException ();
		}



		public override void Flush ()
		{
			throw new NotImplementedException ();
		}


		public override bool CanRead 
		{
			get 
			{
				throw new NotImplementedException ();
			}
		}


		public override bool CanSeek 
		{
			get 
			{
				throw new NotImplementedException ();
			}
		}


		public override bool CanWrite 
		{
			get 
			{
				throw new NotImplementedException ();
			}
		}
	}
}