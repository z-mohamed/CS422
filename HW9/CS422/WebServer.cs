using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using System.Collections.Concurrent;

using System.Collections.Generic;


namespace CS422
{
	public class WebServer
	{
		public static ConcurrentDictionary<string, WebService> servicesList =  new ConcurrentDictionary<string,WebService>();

		public WebServer ()
		{
		}

		public static bool Start(int port, int numThreads)
		{
			// Set number of threads in the thread pool to numThreads. If this value is less
			// than or equal to zero, use 64 as a default instead.

			if (numThreads <= 0)
				numThreads = 64;

			// Not Sure if i doing this part right
			// Why are there two parameters???
			ThreadPool.SetMaxThreads (numThreads,numThreads);


			try
			{
				// Set the TcpListerner on provided port
				TcpListener server = new TcpListener(IPAddress.Any, port);

				// Start listening for client requests
				server.Start();

				//Accept new TCP socket connection
				//Get a thread from the thread pool and pass it the TCP socket
				//Repeat
				while(true)
				{
					try
					{ 
						//Accept TCP client
						TcpClient client = server.AcceptTcpClient();

						//Send Client to a thread pool Thread to be processed
						ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadWork), client);
					}

					// Failed to accept an incoming connection
					catch( Exception)
					{ 

					}
				}
			}

			//Failed to Start Server
			catch (SocketException) 
			{
				return false;
			}
		}
	

		public static void resetBuffer(byte[] buffer)
		{
			for(int x = 0; x <5; x++)
			{
				buffer[x] = 0;
			}
		}


		public static string ParseURL(byte[] buffer)
		{
			string requestedURL;
			int length = 0;

			while(buffer [length] != 32 )
			{
				length++;
			}
			requestedURL = "/" + System.Text.Encoding.ASCII.GetString(buffer, 0,length);

			return requestedURL;
		}


		public static bool checkCRLF(byte[] buffer, int index)
		{
			int next = index + 1;

			if (buffer [index] == 13 && buffer [next] == 10) 
			{
				return true;
			}
			else 
			{
				return false;

			}
		}


        //Accept TCP client and Call BulildRequest
		static void ThreadWork(Object clnt)
		{
			TcpClient client = clnt as TcpClient;

			WebRequest req = BuildRequest (client);

			WebService handler = null;;
			 
			bool processRequest;


			if(req == null)
			{
				//close client connection
				client.GetStream().Close();
				client.Close ();
			}
			else
			{
				//if the request-target/URI starts with the string specified by the WebService object’s
				//ServiceURI parameter, then it can process that request.

				processRequest = false;

				foreach (var x in servicesList) 
				{
					if (req.URI.StartsWith(x.Key))
					{
						processRequest = true;
						handler = x.Value; 
					}


				}
				if(processRequest)
				{
					//find appropriate handler  in the list of 
					//handlers/services that the webserver stores

					//WebService handler;
					//servicesList.TryGetValue (req.URI, out handler);

					//call the handle 
					handler.Handler (req);
				}
				//write a 404 – Not Found response
				else
				{
					string pageHTML = "<html> ERROR! </html>";
					req.WriteNotFoundResponse (pageHTML);
				}

				//client.
				client.GetStream().Close();
				client.Close ();


			}

		}

		private static WebRequest BuildRequest (TcpClient client)
		{

			NetworkStream stream = client.GetStream();

			WebRequest x = checkResposeValidity(stream);

			return x;


		}


		public static void AddService(WebService service)
		{
			//added to the list of services that the web server supports
			servicesList.TryAdd(service.ServiceURI, service);


		}

		public static void Stop()
		{
			// Not implemented

		}


		private static WebRequest checkResposeValidity(NetworkStream stream)
		{
			WebRequest build = new WebRequest(stream);
			const string GET = "GET /"; 
			const string HTTPV = "HTTP/1.1";
			string requestedURL = "/";
			int position = 0;
			bool loop = true;
			bool concat = true;

			MemoryStream frst = new MemoryStream ();

			int read;

			// Buffer for reading data 
			byte[] bytes = new byte[1024];

			while (true) 
			{
				// Get first five bytes
				read = stream.Read(bytes, 0, 5);

				//if first five not equal "GET /" return false
				if (GET != System.Text.Encoding.ASCII.GetString (bytes, 0, 5)) {

					return null;
				} 
				else 
				{
					//Set HTTP method (GET) in WebRequest Object
					build.HTTP_method = System.Text.Encoding.ASCII.GetString (bytes, 0, 5); 
				}

				//clear stream
				resetBuffer(bytes);

				//read a kilobyte or less from stream
				read = stream.Read(bytes, 0, bytes.Length);

				//parse requestedURL
				requestedURL = ParseURL(bytes);

				//Set URI in WebRequest Object
				build.URI = requestedURL;

				// check HTTP version
				if (HTTPV != System.Text.Encoding.ASCII.GetString (bytes, requestedURL.Length, 8)) {
					return null;
				} 
				else 
				{
					//Set HTTP Version(GET) in WebRequest Object
					// Might have to shorten this
					build.HTTP_version = System.Text.Encoding.ASCII.GetString (bytes, requestedURL.Length, 8);
				}

				//get the index of CR on Request Line if it exists
				position = requestedURL.Length + 8;

				if(!checkCRLF(bytes, position))
				{
					return null;
				}

				position = position + 2;

				//if there are no headers return false;
				if(checkCRLF(bytes, position))
				{
					return null;
				}

				bool newline = true;
				bool colonfound = false;
				int  tempCount = 0;

				string headerKey= "";
				string headerValue = "";

				int colonFoundP = 0;

				int i = 0;

				//check all headers, read again if you reach end of stream without getting double line break
				while(loop)
				{
					//if a colon is the first character of a new line return false
					if(newline && bytes[position] == 58)
					{
						return null;
					}

					//check that a colon exists in header
					if(newline)
					{
						//loop until end of line
						while(!checkCRLF(bytes, position))
						{
							//if we find colon set found flag to true
							if(bytes[position] == 58 && colonfound == false)
							{
								colonfound = true;
								colonFoundP = position;
							}
							if(colonfound && headerKey == "")
							{
								//Save Header Key
								headerKey = System.Text.Encoding.ASCII.GetString (bytes, position-tempCount, tempCount);
								if (i != 0)
									headerKey = headerKey.Substring (1);
								i++;

							}
							tempCount++;
							position ++;
						}
						//if colon wasn't found header is invalid return false
						if(!colonfound)
						{
							return null;
						}
						else
						{
							// Save Header Value
							headerValue = System.Text.Encoding.ASCII.GetString (bytes, colonFoundP + 1, position - colonFoundP+1-2);

							// This check just satisfies the compiler
							if(headerKey != "" && headerValue != "")
							{
								//add key value pair to WebRequest Object
								build.headers.Add (headerKey, headerValue);

								//clear key value pair
								headerKey= "";
								headerValue = "";
								tempCount = 0;
							}
							//reset colon found flag
							colonfound = false;
						}
					}

					//check for end of a header
					if(checkCRLF(bytes, position))
					{
						newline = true;
						//position++;
					}
					else
					{
						newline = false;

					}

					//check for end of header section/ begining of body 
					if(newline && checkCRLF(bytes, (position +2)))
					{
						//take me to beginning of body
						position = position + 4;
						//Store the bytes of body that have already beeen read so we can concat later 
						//MemoryStream frst = new MemoryStream(bytes,(position+4), bytes.Length - (position + 4));

						//if body is empty Dont Concat!
						if ( bytes[position] == 0)
						{
							concat = false;
						}
						// else prepare to concat by writing body bytes to frst stream
						else
						{
							//only write bytes until u hit NULL
							while (bytes [position] != 0) 
							{
								frst.Write (bytes, position, 1);
								position++;

							}
						}
						if(concat == true)
						{
							//Concat MemoryStream and NetworkStream
							if(build.headers.ContainsKey("Content-Length"))
							{
								string valueString;
								build.headers.TryGetValue ("Content-Length",out valueString);
								long valueLong = Convert.ToInt64(valueString);
								build.bodyRequest = new ConcatStream (frst, stream, valueLong);
							}
							else
							{
								build.bodyRequest = new ConcatStream (frst, stream);
							}
						}
						// One Stream to rule them all
						else
						{
							build.bodyRequest = stream;
						}
						loop = false;
					}
						
					if(loop)
					{
						position ++;
					}

					//end of byte array but not end of header section, so we read more from stream
					if(position == 1023 && loop)
					{
						read = stream.Read(bytes, 0, bytes.Length);
						position = 0;
					}
				}
				return build;
			}
		}
	}
}

