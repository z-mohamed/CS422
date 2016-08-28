using System;

namespace CS422
{
	public class NumberedTextWriter : System.IO.TextWriter
	{
		System.IO.TextWriter wrapper;

		int currentLineNum = 1 ;


		public NumberedTextWriter(System.IO.TextWriter wrapThis)
		{
			wrapper = wrapThis;
		}


		public NumberedTextWriter(System.IO.TextWriter wrapThis, int startingLineNumber)
		{
			wrapper = wrapThis;

			currentLineNum = startingLineNumber;
		}


		public override System.Text.Encoding Encoding
		{
			get{ return wrapper.Encoding; }
		}


		public override void WriteLine(string value)
		{
			string _2prepend =  currentLineNum.ToString()+":"+ " " + value;

			wrapper.WriteLine(_2prepend);

			currentLineNum++;
		}
	}
}