using NUnit.Framework;
using System;
using CS422;

namespace UnitTests
{
	[TestFixture ()]
	public class Test
	{
		private const string good_path = "/home/zak/Repos/CS422/HW9/CS422/rootDir";

		[Test ()]
		public void RootDaddyNull ()
		{
			StandardFileSystem root = StandardFileSystem.Create (good_path);

			Assert.IsNull (root.GetRoot().Parent);
		}

		[Test ()]
		public void StdDirDaddyNotNull ()
		{
			Dir422 dir = new StdFSDir (good_path);
			Assert.AreEqual("CS422",dir.Parent.Name);
		}
	}
}

