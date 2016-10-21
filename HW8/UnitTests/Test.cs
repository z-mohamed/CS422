using NUnit.Framework;
using System;
using System.Collections.Generic;
using CS422;

namespace UnitTests
{
	[TestFixture ()]
	public class Test
	{
		// S denotes Success 
		// F denotes Failure 
		// These are used at the end of test names
		// e.g., StdCreateS || StdCreateF

		//               //
		string good_path = 
			"/home/zak/Repos/CS422/HW8/CS422/rootDir";

		string bad_path = 
			"/home/zak/Repos/CS422/HW8/CS422/rootDir1";

		[Test ()]
		// 
		public void StdFSCreateS()
		{
			StandardFileSystem obj = StandardFileSystem.Create(good_path);

			Assert.NotNull (obj);
			
		}

		[Test ()]
		public void StdFSCreateF()
		{
			StandardFileSystem obj = StandardFileSystem.Create(bad_path);

			Assert.Null (obj);

		}

		[Test ()]
		public void NameFromPathS()
		{
			string name = Utility.NameFromPath (good_path);

			Assert.AreEqual (name, "rootDir");
		}


		[Test ()] 
		public void PathCharPresentS()
		{
			string back_slash_test = "some\\place";

			bool fs_present = Utility.PathCharPresent (good_path); 	
			bool bs_present = Utility.PathCharPresent (back_slash_test);
		
			Assert.IsTrue (fs_present);
			Assert.IsTrue (bs_present);
		}

		[Test ()]
		public void PathCharPresentF()
		{
			string name = "something";

			bool no_path_char_present = Utility.PathCharPresent (name); 	

			Assert.IsFalse (no_path_char_present);
		}

		[Test ()]
		public void GetParentS()
		{
			StdFSDir son = new StdFSDir (good_path);

			//StdFSDir dad = son.Parent;

			Assert.AreEqual ("CS422", son.Parent.Name);

		}

	
		[Test ()]
		public void GetDirectorieS()
		{
			StdFSDir dir = new StdFSDir (good_path);

			IList<Dir422> directories = dir.GetDirs ();

			Assert.AreEqual (6, directories.Count);

		}

		[Test ()]
		public void GetFilesS()
		{
			StdFSDir dir = new StdFSDir (good_path);

			IList<File422> files = dir.GetFiles ();

			Assert.AreEqual (4, files.Count);

		}


		[Test ()]
		public void GetDirS()
		{
			StdFSDir dir = new StdFSDir (good_path);

			Dir422 gotten_dir = dir.GetDir ("d3");

			Assert.AreEqual ("d3", gotten_dir.Name);
		}

		[Test ()]
		public void GetDirContainsPathCharF()
		{
			StdFSDir dir = new StdFSDir (good_path);

			Dir422 gotten_dir = dir.GetDir ("d/3");

			Assert.Null(gotten_dir);

			gotten_dir = dir.GetDir ("d\\3");

			Assert.Null(gotten_dir);
		}

		[Test ()]
		public void GetDirDosentExistF()
		{
			StdFSDir dir = new StdFSDir (good_path);

			Dir422 gotten_dir = dir.GetDir ("d7");

			Assert.Null (gotten_dir);
		}

		public void GetFileS()
		{
			StdFSDir dir = new StdFSDir (good_path);

			File422 gotten_file = dir.GetFile ("f1");

			Assert.AreEqual ("f1", gotten_file.Name);
		}

		[Test ()]
		public void GetFileContainsPathCharF()
		{
			StdFSDir dir = new StdFSDir (good_path);

			File422 gotten_file = dir.GetFile ("f/3");

			Assert.Null(gotten_file);

			gotten_file = dir.GetFile ("f\\3");

			Assert.Null(gotten_file);
		}

		[Test ()]
		public void GetFileDosentExistF()
		{
			StdFSDir dir = new StdFSDir (good_path);

			File422 gotten_file = dir.GetFile ("f7");

			Assert.Null (gotten_file);
		}

		[Test ()]
		public void CreateFileS()
		{
			StdFSDir dir = new StdFSDir (good_path);

			dir.CreateFile ("f4");

			File422 gotten_file = dir.GetFile ("f4");

			Assert.AreEqual ("f4", gotten_file.Name);
		}

		[Test ()]
		public void CreateDirS()
		{
			StdFSDir dir = new StdFSDir (good_path);

			dir.CreateDir ("d6");

			Dir422 gotten_file = dir.GetDir ("d6");

			Assert.AreEqual ("d6", gotten_file.Name);
		}
		/*
		[Test ()]
		public void TestName()
		{

		}*/


	}
}

