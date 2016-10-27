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
			// THIS NOT GOOD ENOUGH!!!
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
				if(Req.URI == ServiceURI || uriName == "")
				{
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
					"<a href=\"{0}\">{1}</a><br>", Utility.AbsolutePath(file.Parent), file.Name
				);
			}

			html.Append ("<html>");
			return html.ToString();
		}




			
	}
}

