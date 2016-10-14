using System;
using CS422;

namespace Main
{
	class MainClass
	{
		//WebServer server = new WebServer();
		public static void Main (string[] args)
		{
			DemoService test = new DemoService ();
			WebServer.AddService (test);
			WebServer.Start (2017, 22); 

			//Console.WriteLine ("Hello World!");
		}
	}
}
