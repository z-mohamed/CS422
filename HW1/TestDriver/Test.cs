using NUnit.Framework;
using System;
using CS422;

namespace TestDriver
{
	[TestFixture ()]
	public class Test
	{
		[Test ()]
		public void TestCase ()
		{
			System.IO.TextWriter x = Console.Out;

			CS422.NumberedTextWriter test = new NumberedTextWriter (x,71);

			using (x)
			{
				test.WriteLine ("Hello World");

				test.WriteLine ("Hello World");

				test.WriteLine ("Hello World");

				test.WriteLine ("Hello World");

				test.WriteLine ("Hello World");
			}
		}

		public void TestCase2 ()
		{
			
		}
	}
}