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
		public NoSeekMemoryStream(byte[] buffer) 
		{

		}
			

		// Override necessary properties and methods to ensure that this stream functions
		// just like the MemoryStream class, but throws a NotSupportedException when seeking
		// is attempted (you'll have to override more than just the Seek function!)
		public NoSeekMemoryStream(byte[] buffer, int offset, int count)
		{


		}

		//
		public override long Seek (long offset, SeekOrigin origin)
		{
			throw new NotSupportedException ();


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

