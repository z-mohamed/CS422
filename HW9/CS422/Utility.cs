using System;
using System.IO;
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
			string decodedURI = uri;
			int position = 0;
			int length = uri.Length;	

			while( position < length) 
			{
				if (decodedURI [position] == '%') 
				{
					int hexASCII = 0;

					if (Int32.TryParse(decodedURI.Substring(position+1,2), out hexASCII))
					{
						int unicode = hexASCII + 12;
						char character = (char) unicode;
						string text = character.ToString();

						decodedURI = decodedURI.Insert (position, text);
						decodedURI = decodedURI.Remove (position+1, 3);

						length = length - 2;
						position = position + 2;
					}
				}

				position++;
			}

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



		public static string ContentType(string fileName)
		{
			int lastOccurrance = fileName.LastIndexOf ('.');
			string contentType = fileName.Substring (lastOccurrance + 1);

			if (contentType == "JPEG" || contentType == "jpeg") 
			{
				return "image/jpeg";
			}
			else if (contentType == "PNG" || contentType == "png") 
			{
				return "image/png";
			}
			else if (contentType == "PDF" || contentType == "pdf") 
			{
				return "application/pdf";
			}
			else if (contentType == "MP4" || contentType == "mp4") 
			{
				return "video/mp4";
			}
			else if (contentType == "TXT" || contentType == "txt") 
			{
				return "text/plain";
			}
			else if (contentType == "HTML" || contentType == "html") 
			{
				return "text/html";
			}
			else if (contentType == "XML" || contentType == "xml") 
			{
				return "application/xml";
			}

			return "";

		}

		public static string BuildFileResponseString(string contentLength, string contentType)
		{
			string responseString =
				"HTTP/1.1 200 OK\r\n" +
				"Accept-Ranges: bytes\r\n" +
				"Content-Type: " + contentType + "\r\n" +
				"Content-Length: " + contentLength + "\r\n" +
				"Connection:Keep-Alive" +
				"\r\n\r\n";

			return responseString;
		}
	}
}

