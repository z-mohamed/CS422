using System;
using System.IO;
using System.Text;

namespace CS422
{
	public class FilesWebService : WebService
	{
		private readonly FileSys422 m_FS;

		public FilesWebService  (FileSys422 FS)
		{
			m_FS = FS;
		}


		public override void Handler(WebRequest Req)
		{
			Dir422 Root = m_FS.GetRoot ();

			//Remove Last / here!!!!
			if (Req.URI.LastIndexOf ('/') == Req.URI.Length - 1)
				Req.URI = Req.URI.Substring (0, Req.URI.Length - 1);
				//int x = 0;


			//1. Percent-decode URI. 
			// THIS NOT GOOD ENOUGH!!!

			Req.URI = Utility.PercentDecode (Req.URI);
			//string test = Req.URI.Replace ("%20", " ");;	

			// Set name of requested file||dir.
			string uriName = Utility.NameFromPath (Req.URI);

			//2. If it refers to file somewhere in the shared folder.
			if (Root.ContainsFile (uriName, true)) 
			{	
				string path = Req.URI.Substring (0, Req.URI.LastIndexOf ("/"));

				Dir422 Dir = Utility.TraverseToDir (Root, path);
				StdFSFile File = (StdFSFile)Dir.GetFile (uriName);
				FileStream MyFileStream = (FileStream)File.OpenReadOnly ();

				if (Req.headers.ContainsKey ("Range")) 
				{
					// process partial response here
					Console.WriteLine("Process Partial Request");
					int x = 0;
				}
				else
				{
					string contentType = Utility.ContentType (uriName);
					string response = Utility.BuildFileResponseString (
						MyFileStream.Length.ToString(), contentType );

					//if (contentType != "video/mp4") 
					//{
						byte[] sendResponseString = Encoding.ASCII.GetBytes (response);
						Req.bodyRequest.Write (sendResponseString, 0, sendResponseString.Length);
					//}

				//byte[] sendResponseString = Encoding.ASCII.GetBytes(response);

					int read = 0;
					byte[] send = new byte[7500];

					while (read < MyFileStream.Length) 
					{
						read = read + MyFileStream.Read (send, 0, send.Length);
						Req.bodyRequest.Write (send, 0, send.Length);
					}
				}   
			}

			//3. Else if it refers to a folder somewhere in the shared folder.
			//	 Or is the shared directory
			else if (Root.ContainsDir (uriName, true) || Req.URI == ServiceURI || uriName == "") 
			{
				if(Req.URI == ServiceURI || uriName == "")
				{
					string dirHTMLListing = BuildDirHTML(Root);

					byte[] sendResponseString = Encoding.ASCII.GetBytes (dirHTMLListing);
					Req.bodyRequest.Write (sendResponseString, 0, sendResponseString.Length);

					//Req.WriteHTMLResponse (dirHTMLListing);
				}
				else
				{
					Dir422 Dir = Utility.TraverseToDir (Root, Req.URI);
					string dirHTMLListing = BuildDirHTML(Dir);

					byte[] sendResponseString = Encoding.ASCII.GetBytes (dirHTMLListing);
					Req.bodyRequest.Write (sendResponseString, 0, sendResponseString.Length);

					//Req.WriteHTMLResponse (dirHTMLListing);
				}
			}

			//4.Else it’s a bad URI.
			else
			{
				Req.WriteNotFoundResponse("File or directory not found");
			}
		}

		public override string ServiceURI 
		{
			get { return "/files";}
		}


		private string BuildDirHTML (Dir422 Directory)
		{
			var html = new System.Text.StringBuilder("<html>");
			html.Append ("<h1>Folders</h1>");

			// PERCENT ENCODE HERE 
			//get rid of ( # reserved html key)!!!!!!
			foreach(Dir422 Dir in Directory.GetDirs())
			{
				html.AppendFormat(
					"<a href=\"{0}\">{1}</a><br>", Utility.AbsolutePath(Dir), Dir.Name
				);
			}

			html.Append ("<h1>Files</h1>");

			foreach(File422 file in Directory.GetFiles())
			{
				html.AppendFormat(
					"<a href=\"{0}\">{1}</a><br>", Utility.AbsolutePath(file.Parent) + "/" + file.Name, file.Name
				);
			}

			html.Append ("<html>");
			return html.ToString();
		}




			
	}
}

