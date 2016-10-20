﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS422
{
	/// <summary>
	/// Represents a directory in the filesystem
	/// </summary>
	public abstract class Dir422
	{
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public abstract string Name { get; }

		/// <summary>
		/// Gets the dirs.
		/// </summary>
		/// <returns>The dirs.</returns>
		public abstract IList<Dir422> GetDirs();

		/// <summary>
		/// Gets the files.
		/// </summary>
		/// <returns>The files.</returns>
		public abstract IList<File422> GetFiles();

		/// <summary>
		/// Gets the parent.
		/// </summary>
		/// <value>The parent.</value>
		public abstract Dir422 Parent { get; }


		/// <summary>
		/// Containses the file.
		/// </summary>
		/// <returns><c>true</c>, if file was containsed, <c>false</c> otherwise.</returns>
		/// <param name="fileName">File name.</param>
		/// <param name="recursive">If set to <c>true</c> recursive.</param>
		public abstract bool ContainsFile(string fileName, bool recursive);

		/// <summary>
		/// Containses the dir.
		/// </summary>
		/// <returns><c>true</c>, if dir was containsed, <c>false</c> otherwise.</returns>
		/// <param name="dirName">Dir name.</param>
		/// <param name="recursive">If set to <c>true</c> recursive.</param>
		public abstract bool ContainsDir(string dirName, bool recursive);

		// Returns null if fileName or dirName is null
		/// <summary>
		/// Gets the file.
		/// </summary>
		/// <returns>The file.</returns>
		/// <param name="fileName">File name.</param>
		public abstract File422 GetFile(string fileName);

		/// <summary>
		/// Gets the dir.
		/// </summary>
		/// <returns>The dir.</returns>
		/// <param name="dirName">Dir name.</param>
		public abstract Dir422 GetDir(string dirName);

		// When dir and file name doesnt exists create. If File Exists 0 out the file if directory exists return and do nothing
		/// <summary>
		/// Creates the file.
		/// </summary>
		/// <returns>The file.</returns>
		/// <param name="fileName">File name.</param>
		public abstract File422 CreateFile(string fileName);

		/// <summary>
		/// Creates the dir.
		/// </summary>
		/// <returns>The dir.</returns>
		/// <param name="dirName">Dir name.</param>
		public abstract Dir422 CreateDir(string dirName);
	}





	/// <summary>
	/// Represents a file in the filesystem
	/// </summary>
	public abstract class File422
	{
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public abstract string Name { get; }

		/// <summary>
		/// Gets the parent.
		/// </summary>
		/// <value>The parent.</value>
		public abstract Dir422 Parent { get; }

		// OpenReadOnly stream has property CanWrite=False
		/// <summary>
		/// Opens the read only.
		/// </summary>
		/// <returns>The read only.</returns>
		public abstract Stream OpenReadOnly();

		/// <summary>
		/// Opens the read write.
		/// </summary>
		/// <returns>The read write.</returns>
		public abstract Stream OpenReadWrite();

	}






	/// <summary>
	/// File sys422.
	/// </summary>
	public abstract class FileSys422
	{
		/// <summary>
		/// Gets the root.
		/// </summary>
		/// <returns>The root.</returns>
		public abstract Dir422 GetRoot();

		/// <summary>
		/// Contains the specified file.
		/// </summary>
		/// <param name="file">File.</param>
		public virtual bool Contains(File422 file)
		{
			return Contains(file.Parent);

		}

		/// <summary>
		/// Contains the specified dir.
		/// </summary>
		/// <param name="dir">Dir.</param>
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












	/// <summary>
	/// Standard File System Directory
	/// </summary>
	public class StdFSDir : Dir422
	{
		private string m_path;

		public StdFSDir(string path)
		{
			// If dir does not exist
			// throw arg expt.
			if (!Directory.Exists(path))
			{
				throw new ArgumentException();
			}

			// Path is valid proceed.
			m_path = path;
		}

		/// <summary>
		/// Use nameFromPath()
		/// </summary>
		/// <value>The name.</value>
		public override string Name { get; }


		/// <summary>
		/// Use Directory.GetDirectories Method
		/// then construct a Dir422 for each item
		/// in resultant array then add them to a list
		/// </summary>
		/// <returns>The dirs.</returns>
		public override IList<Dir422> GetDirs()
		{
			throw new NotImplementedException();
		}

	
		public override IList<File422> GetFiles()
		{
			List<File422> files = new List<File422> ( );
			foreach ( var file in Directory.GetFiles ( m_path ) )
			{
				files.Add ( new StdFSFile ( file ) );
			}

			return files;
		}

		/// <summary>
		/// Check if Dir422 is root?
		/// Use Directory.GetParent Method
		/// </summary>
		/// <value>The parent.</value>
		public override Dir422 Parent { get; }



		public override bool ContainsFile(string fileName, bool recursive)
		{
			throw new NotImplementedException();
		}

		public override bool ContainsDir(string dirName, bool recursive)
		{
			throw new NotImplementedException();
		}


		/// <summary>
		/// Not recursive.
		/// Use validatePath() if false, return null.
		/// Use Directory.GetDirectories
		/// Check if dirName is in resultant array.
		/// Return appropriate result.
		/// </summary>
		/// <returns>The dir.</returns>
		/// <param name="dirName">Dir name.</param>
		public override Dir422 GetDir(string dirName)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		// Analogous to Get Dir
		/// </summary>
		/// <returns>The file.</returns>
		/// <param name="fileName">File name.</param>
		public override File422 GetFile(string fileName)
		{
			throw new NotImplementedException ( );
		}


		/// <summary>
		/// Use validatePath() if false, return null.
		/// Use File.Create Method.
		/// Construct File422 obj from resultant FileStream.
		/// Return File422 obj.
		/// </summary>
		/// <returns>The file.</returns>
		/// <param name="fileName">File name.</param>
		public override File422 CreateFile(string fileName)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Use validatePath() if false, return null.
		/// Use Directory.CreateDirectory Method.
		/// Construct Dir422 obj from resultant DirectoryInfo.Fullname.
		/// Return Dir422 obj. 
		/// </summary>
		/// <returns>The dir.</returns>
		/// <param name="dirName">Dir name.</param>
		public override Dir422 CreateDir(string dirName)
		{
			throw new NotImplementedException();
		}
	}
















	public class StdFSFile : File422
	{
		private string m_path;

		public override string Name { get; }
		public override Dir422 Parent { get; }

		public override Stream OpenReadOnly()
		{
			return new FileStream(m_path, FileMode.Open, FileAccess.Read);
		}

		public override Stream OpenReadWrite()
		{
			throw new NotImplementedException();
		}
	}












	public class StandardFileSystem : FileSys422
	{
		private readonly Dir422 m_root;

		public override Dir422 GetRoot()
		{
			return m_root;
		}
	}



}
