using System;

namespace CS422
{
	public class IndexedNumsStream : System.IO.Stream
	{
		
		byte length;


		public IndexedNumsStream (long length)
		{
			
		}


		public override long Length {
			get {
				throw new NotImplementedException ();
			}
		}


		public override void SetLength (long value)
		{
			throw new NotImplementedException ();
		}	


		public override long Position {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
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


		public override int Read (byte[] buffer, int offset, int count)
		{
			return 0;
		}


		public override void Write (byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException ();
		}


		public override long Seek (long offset, System.IO.SeekOrigin origin)
		{
			throw new NotImplementedException ();
		}
	}
}