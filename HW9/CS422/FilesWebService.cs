using System;

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

			//1. Percent-decode URI.
			Req.URI = Req.URI.Replace ("%20", " ");;

			//Remove Last / here!!!!

			// Set name of requested file||dir.
			string uriName = Utility.NameFromPath (Req.URI);

			//2. If it refers to file somewhere in the shared folder.
			if (Root.ContainsFile (uriName, true)) 
			{
				// Send file response.
				Console.WriteLine ("We in here baby");
			}

			//3. Else if it refers to a folder somewhere in the shared folder.
			//	 Or is the shared directory
			else if (Root.ContainsDir (uriName, true) || Req.URI == ServiceURI || uriName == "") 
			{
				// send an HTML listing for the folder.
				if(Req.URI == ServiceURI || uriName == "")
				{
					//List Root
					string dirHTMLListing = BuildDirHTML(Root);
					Req.WriteHTMLResponse (dirHTMLListing);
				}

				else
				{
					Dir422 Dir = Utility.TraverseToDir (Root, Req.URI);

					string dirHTMLListing = BuildDirHTML(Dir);

					Req.WriteHTMLResponse (dirHTMLListing);

				}
			}

			//4.Else it’s a bad URI.
			else
			{
				Req.WriteNotFoundResponse("File or directory not found");
			}
		}
			
		private void RespondWithList(Dir422 Dir, WebRequest Req)
		{
			/*var html = new System.Text.StringBuilder("<html>")
				foreach(FSFile file in dir.GetFiles())
				{
					//PRECENT ENCODING!!!!!!!!!!!!!
					html.AppendFormat(
						"<a href=\ "{0}\">{1}</a>"),
					);

					//GET HREF for File$22 object
					// Last part File422Obj.name
					//Recurse through parent directories until hitting root
					//For each one, append directory name to front of the string

				}

					html.AppendLine("</html>");
				req.WriteHTMLResponse(html.ToString());
				*/
		}

		private string BuildDirHTML (Dir422 Directory)
		{
			var html = new System.Text.StringBuilder("<html>");

			html.Append ("<h1>Folders</h1>");

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
					"<a href=\"{0}\">{1}</a><br>", Utility.AbsolutePath(file.Parent), file.Name
				);
			}

			html.Append ("<html>");

			return html.ToString();
		}
			
		public override string ServiceURI 
		{
			get { return "/files";}
		}
	}
}

