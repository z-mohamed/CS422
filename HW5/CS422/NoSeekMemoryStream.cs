using System;
using System.IO;
namespace CS422
{

	/// <summary>
	/// Represents a memory stream that does not support seeking, but otherwise has
	/// functionality identical to the MemoryStream class.
	/// </summary>


	public class NoSeekMemoryStream : MemoryStream
	{
		public NoSeekMemoryStream(byte[] buffer) :base(buffer)
		{
		}


		// Override necessary properties and methods to ensure that this stream functions
		// just like the MemoryStream class, but throws a NotSupportedException when seeking
		// is attempted (you'll have to override more than just the Seek function!)
		public NoSeekMemoryStream(byte[] buffer, int offset, int count):base(buffer)
		{


		}

		//Position Properties are only supported if you can Seek
		public override long Position 
		{
			get
			{
				throw new NotSupportedException ("Not Supported");
			
			}

			set
			{
				throw new NotSupportedException ("Not Supported");
			}
		}

		//Length getter is only supported if you can Seek
		public override long Length
		{
			get
			{

				throw new NotSupportedException ("Not Supported");
			}
		}

		// A stream must support both writing and seeking for SetLength to work.
		public override void SetLength (long value)
		{
			throw new NotSupportedException ("Not Supported");
		}

		// The whole point
		public override long Seek (long offset, SeekOrigin origin)
		{
			throw new NotSupportedException ("Not Supported");

		}
			
		public override bool CanSeek
		{
			get
			{
					return false;
			}
		}
	}
}
