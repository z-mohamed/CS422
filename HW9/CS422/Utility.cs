using System;
using System.Linq;

namespace CS422
{
	public class Utility
	{
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
			if(name.Contains("/") || name.Contains ("\\") || name == "" || name == null)
			{
				return true;
				
			}
			return false;
		}

		public static string PercentDecode(string uri)
		{
			string decodedURI = uri.Replace ("%20", " ");
			return decodedURI;
		}

		public static string AbsolutePath(Dir422 dir)
		{
			string absolutePath = "";

			do 
			{
				absolutePath = absolutePath.Insert (0, dir.Name);
				absolutePath = absolutePath.Insert (0, "/");
				dir = dir.Parent;

			} while(dir != null);
				
			return absolutePath;
		}

		public static Dir422 TraverseToDir(Dir422 Root, string path)
		{
			Dir422 directory = Root;

			string[] pathPieces = path.Split('/');

			for(int i = 2; i<pathPieces.Count(); i++)
			{
				directory = directory.GetDir (pathPieces[i]);	
			}

			return directory;

		}
	}
}

