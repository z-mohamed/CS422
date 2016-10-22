using System;
using System.Linq;

namespace CS422
{
	public class Utility
	{

		//bool validatePath(string path){}

		public static string NameFromPath(string path)
		{
			string name;

			string[] path_pieces = path.Split('/');

			int last_piece = path_pieces.Length - 1;

			name = path_pieces [last_piece];

			return name;
		}

		public static bool PathCharPresent(string name)
		{
			if(name.Contains("/") || name.Contains ("\\"))
			{
				return true;
				
			}
			return false;
		}

		public static MemFSDir MemDirFileExists(string path, MemoryFileSystem root)
		{
			MemFSDir cwd = (MemFSDir)root.GetRoot ();

			string[] path_pieces = path.Split('/');

			for (int i = 1; i < path_pieces.Count() - 1; i++) 
			{
				if (cwd.directories.ContainsKey (path_pieces [i]))
					cwd.directories.TryGetValue (path_pieces [i], out cwd);
				else
					return null;
			}

			return cwd;
		}
			
	}
}

