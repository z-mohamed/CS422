﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS422
{
	// Represents a directory in the filesystem
	public abstract class Dir422
	{
		public abstract string Name { get; }

		public abstract IList<Dir422> GetDirs();

		public abstract IList<File422> GetFiles();

		public abstract Dir422 Parent { get; }

		public abstract bool ContainsFile(string fileName, bool recursive);

		public abstract bool ContainsDir(string dirName, bool recursive);

		public abstract File422 GetFile(string fileName);

		public abstract Dir422 GetDir(string dirName);

		public abstract File422 CreateFile(string fileName);

		public abstract Dir422 CreateDir(string dirName);
	}
		
	// Represents a file in the filesystem
	public abstract class File422
	{
		public abstract string Name { get; }

		public abstract Dir422 Parent { get; }

		public abstract Stream OpenReadOnly();

		public abstract Stream OpenReadWrite();

	}
		
	// Represents the file systeem.
	public abstract class FileSys422
	{
		public abstract Dir422 GetRoot();


		public virtual bool Contains(File422 file)
		{
			return Contains(file.Parent);

		}
			
		public virtual bool Contains ( Dir422 dir)
		{
			// Get to the root directory
			while (dir.Parent != null)
			{
				dir = dir.Parent;
			}

			// if its the same root as the File system
			return ReferenceEquals(dir, GetRoot());
		}
	}

	//Implementation of StdFSDir
	public class StdFSDir : Dir422
	{
		private string m_path;

		//StdFSDir ctor
		public StdFSDir(string path)
		{
			if (!Directory.Exists(path))
			{
				throw new ArgumentException();
			}
				
			m_path = path;

			Name = Utility.NameFromPath (path);
		}

		// Get name of this directory
		public override string Name { get; }


		// Get list of directories in this directory
		public override IList<Dir422> GetDirs()
		{
			List<Dir422> directories = new List<Dir422> ( );

			foreach ( var directory in Directory.GetDirectories ( m_path ) )
			{
				directories.Add ( new StdFSDir ( directory ) );
			}

			return directories;
		}

		// Get list of files in this directory
		public override IList<File422> GetFiles()
		{
			List<File422> files = new List<File422> ( );

			foreach ( var file in Directory.GetFiles ( m_path ) )
			{
				files.Add ( new StdFSFile ( file ) );
			}

			return files;
		}
			
		//HOW DO I CHECK FOR ROOT!!!!
		public override Dir422 Parent 
		{
			get 
			{
				DirectoryInfo parent_info = Directory.GetParent (m_path);

				string parent_path = parent_info.FullName;

				StdFSDir parent = new StdFSDir (parent_path);

				return parent;
			}		 
		}

		
		public override bool ContainsFile(string fileName, bool recursive)
		{
			throw new NotImplementedException();
		}


		public override bool ContainsDir(string dirName, bool recursive)
		{
			throw new NotImplementedException();
		}
			

		// Get specified directory in this directory.
		public override Dir422 GetDir(string dirName)
		{
			if (Utility.PathCharPresent (dirName))
				return null;
			
			string[] directories = Directory.GetDirectories (m_path);

			for(int i = 0; i < directories.Count(); i++)
			{
				if( dirName == Utility.NameFromPath(directories[i]))
					return new StdFSDir(directories[i]);
			}

			return null;
		}

		// Get specified file in this directory.
		public override File422 GetFile(string fileName)
		{
			if (Utility.PathCharPresent (fileName))
				return null;

			string[] files = Directory.GetFiles (m_path);

			for(int i = 0; i < files.Count(); i++)
			{
				if( fileName == Utility.NameFromPath(files[i]))
					return new StdFSFile(files[i]);
			}

			return null;
		}

		// Create new file in this directory.
		public override File422 CreateFile(string fileName)
		{
			if (Utility.PathCharPresent (fileName))
				return null;
			
			string path = m_path + "/" + fileName; 

			File.Create (path);

			return new StdFSFile (path);
		}
			
		// Create new directory in this directory
		public override Dir422 CreateDir(string dirName)
		{
			if (Utility.PathCharPresent (dirName))
				return null;

			string path = m_path + "/" + dirName; 

			Directory.CreateDirectory (path);

			return new StdFSDir (path);
		}
	}


	//Implementation of StdFSFile
	public class StdFSFile : File422
	{
		private string m_path;

		//StdFSFile ctor
		public StdFSFile(string path)
		{
			if (!File.Exists(path))
			{
				throw new ArgumentException();
			}

			m_path = path;

			Name = Utility.NameFromPath (m_path);
		}


		public override string Name { get; }

		// NOT IMPLEMENTED!!
		public override Dir422 Parent {
			get;
		}


		public override Stream OpenReadOnly()
		{
			return new FileStream(m_path, FileMode.Open, FileAccess.Read);
		}


		public override Stream OpenReadWrite()
		{
			throw new NotImplementedException();
		}
	}



	//Implementation of StandardFileSystem
	public class StandardFileSystem : FileSys422
	{
		private readonly Dir422 m_root;

		//StandardFileSystem ctor
		private StandardFileSystem(string path)
		{
			// No! Root will be given Parent! 
			m_root = new StdFSDir (path);
		}


		public override Dir422 GetRoot()
		{
			return m_root;
		}

		// Initilize File System
		public static StandardFileSystem Create(string rootDir)
		{

			if (!Directory.Exists(rootDir))
			{
				return null;
			}

			return new StandardFileSystem (rootDir);
		}
	}
}