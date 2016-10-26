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
			if(name.Contains("/") || name.Contains ("\\") || name == "" || name == null)
			{
				return true;
				
			}
			return false;
		}
			
	}
}

