using System;
using CS422;

namespace Main
{
	class MainClass
	{
		//WebServer server = new WebServer();
		public static void Main (string[] args)
		{
			const string goodPath = 
				"/home/zak/Repos/CS422/HW9/CS422/files";
			
			DemoService test = new DemoService ();

			StandardFileSystem myFileSystem = StandardFileSystem.Create(goodPath);

			FilesWebService hostedFiles = new FilesWebService (myFileSystem);

			WebServer.AddService (hostedFiles);

			WebServer.Start (2017, 22); 
		}
	}
}

