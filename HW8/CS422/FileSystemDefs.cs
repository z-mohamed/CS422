using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS422
{
	// Directory abstraction.
	public abstract class Dir422
	{
		public abstract string Name { get; }

		public abstract IList<Dir422> GetDirs();

		public abstract IList<File422> GetFiles();

		public abstract Dir422 Parent { get; }

		public virtual bool ContainsFile(string fileName, bool recursive)
		{
			File422 file = GetFile (fileName);

			if (file != null)
				return true;

			if (!recursive)
				return false;

			IList<Dir422> directories = GetDirs ();

			for(int i =0; i<directories.Count();i++)
			{
				Dir422 dir = directories [i];

				if (dir.ContainsFile (fileName, true))
					return true;
			}

			return false;
		}

		public virtual bool ContainsDir(string dirName, bool recursive)
		{
			Dir422 directory = GetDir (dirName);

			if (directory != null)
				return true;

			if (!recursive)
				return false;

			IList<Dir422> directories = GetDirs ();

			for(int i =0; i<directories.Count();i++)
			{
				Dir422 dir = directories [i];

				if (dir.ContainsDir(dirName, true))
					return true;
			}

			return false; 

		}

		public abstract File422 GetFile(string fileName);

		public abstract Dir422 GetDir(string dirName);

		public abstract File422 CreateFile(string fileName);

		public abstract Dir422 CreateDir(string dirName);
	}
		
	// File abstraction
	public abstract class File422
	{
		public abstract string Name { get; }

		public abstract Dir422 Parent { get; }

		public abstract Stream OpenReadOnly();

		public abstract Stream OpenReadWrite();
	}
		
	// File System abstraction
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

		public override Dir422 Parent 
		{
			get
			{
				string path = m_path.Substring (0, m_path.Count() - Name.Count ()-1);

				return new StdFSDir (path);
			}
		}

		// Not Tested
		public override Stream OpenReadOnly()
		{
			
			return new FileStream(m_path, FileMode.Open, FileAccess.Read);
		}

		// Not Tested
		public override Stream OpenReadWrite()
		{
			return new FileStream (m_path, FileMode.Open, FileAccess.ReadWrite); 
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



	//Implementation of StdFSDir
	public class MemFSDir : Dir422
	{
		//public MemFSDir Parent;
		public IList<MemFSDir> directories{ get; }
		public IList<MemFSFile> files { get; } 

		// Root Ctor
		public MemFSDir()
		{
			Name = "rootDir";
			Parent = null;

			directories = new List<MemFSDir> ();
			files = new List<MemFSFile> ();

		}

		// Regular ctor
		private MemFSDir(string name, MemFSDir _parent)
		{
			Name = name;
			Parent = _parent;

			directories = new List<MemFSDir> ();
			files = new List<MemFSFile> ();


		}

		// Get name of this directory
		public override string Name { get; }


		// Get list of directories in this directory
		public override IList<Dir422> GetDirs()
		{
			List<Dir422> _directories = new List<Dir422> ( );

			foreach(var dir in this.directories)
			{
				_directories.Add (dir);
				
			}
			return _directories;
		}

		// Get list of files in this directory
		public override IList<File422> GetFiles()
		{
			List<File422> _files = new List<File422> ( );

			foreach(var file in this.files)
			{
				_files.Add (file);

			}
			return _files;
		}

		//HOW DO I CHECK FOR ROOT!!!!
		public override Dir422 Parent 
		{
			get;		 
		}
			
		// Get specified directory in this directory.
		public override Dir422 GetDir(string dirName)
		{
			if (Utility.PathCharPresent (dirName))
				return null;

			IList<Dir422> dirs = this.GetDirs ();

			foreach (var dir in dirs)
			{
				if (dir.Name == dirName)
					return dir;
			}

			return null;
		}

		// Get specified file in this directory.
		public override File422 GetFile(string fileName)
		{
			if (Utility.PathCharPresent (fileName))
				return null;

			foreach (var file in this.files)
			{
				if (file.Name == fileName)
					return file;
			}

			return null;
		}

		// Create new file in this directory.
		public override File422 CreateFile(string fileName)
		{
			if (Utility.PathCharPresent (fileName))
				return null;
			
			foreach (var file in this.files)
			{
				if (file.Name == fileName)
					return file;
			}

			File422 new_file = new MemFSFile (fileName, this);

			this.files.Add ((MemFSFile)new_file);

			return new_file;


		}

		// Create new directory in this directory
		public override Dir422 CreateDir(string dirName)
		{
			if (Utility.PathCharPresent (dirName))
				return null;

			IList<Dir422> dirs = this.GetDirs ();

			foreach (var dir in dirs)
			{
				if (dir.Name == dirName)
					return dir;
			}


			Dir422 new_dir = new MemFSDir(dirName,this);

			this.directories.Add ((MemFSDir)new_dir);

			return new_dir;
		}
	}

	//Implementation of MemFSFile
	public class MemFSFile: File422
	{
		MemoryStream stream;
		byte[] buffer = new byte[256];

		//StdFSFile ctor
		public MemFSFile(string name, MemFSDir _parent)
		{
			Name = name;

			Parent = _parent;
			
		}
			
		public override string Name { get; }

		public override Dir422 Parent 
		{
			get;
		}
			
		public override Stream OpenReadOnly()
		{
			stream = new MemoryStream (buffer, false); 
			return  stream;
		}
			
		public override Stream OpenReadWrite()
		{
			stream = new MemoryStream (buffer, true); 
			return  stream;
		}
	}

	//Implementation of MemoryFileSystem
	public class MemoryFileSystem : FileSys422
	{
		private readonly Dir422 m_root;

		//MemoryFileSystem ctor
		public MemoryFileSystem()
		{ 
			m_root = new MemFSDir ();
		}


		public override Dir422 GetRoot()
		{
			return m_root;
		}
	}
}
