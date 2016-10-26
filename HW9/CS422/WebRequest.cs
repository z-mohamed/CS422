using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;

namespace CS422
{


	public class WebRequest
	{
		//member variables
		public string HTTP_method{ get;set;}
		public string URI{ get;set;}
		public string HTTP_version{ get;set;}
	
		public Dictionary<string, string> headers = new Dictionary<string,string>();
		public Stream bodyRequest;

		private NetworkStream writeResponseStream;



		public WebRequest (NetworkStream client )
		{
			writeResponseStream = client;
		}


		public void WriteNotFoundResponse(string pageHTML)
		{
			int byteCount = System.Text.ASCIIEncoding.Unicode.GetByteCount(pageHTML);
			//writes a response with a 404 status code and the specified HTML 
			// string as the body of the response
			string template = 
				this.HTTP_version + " 404 Not Found\r\n" +
				"Content-Type: text/html\r\n" +
				"Content-Length: " + byteCount.ToString () + "\r\n" +
				"\r\n\r\n" +
				pageHTML;
				
			byte[] send = Encoding.ASCII.GetBytes(template);
			writeResponseStream.Write(send, 0, send.Length);
			//create and write the response line, response headers, and 
			//double break before writing the HTML string to the network stream.

			writeResponseStream.Dispose ();

		}


		public void WriteHTMLResponse(string htmlString)
		{

			int byteCount = System.Text.ASCIIEncoding.Unicode.GetByteCount(htmlString);

			 string DefaultTemplate =
				"HTTP/1.1 200 OK\r\n" +
				"Content-Type: text/html\r\n" +
				"Content-Length: " + byteCount.ToString () + "\r\n" +
				"\r\n\r\n" +
				htmlString;
			


			//byte[] send = Encoding.ASCII.GetBytes(responds_line);

			//Set up reponse template
			//string respond = String.Format(DefaultTemplate, 11395829, DateTime.Now,this.URI);
			//convert response template to bytes
			byte[] send = Encoding.ASCII.GetBytes(DefaultTemplate);

			//send bytes to client
			//stream.Write(send, 0, send.Length);

			//writes a response with a 200 status code and the specified HTML 
			// string as the body of the response


			writeResponseStream.Write(send, 0, send.Length);
			//create and write the response line, response headers, and 
			//double break before writing the HTML string to the network stream.

			writeResponseStream.Dispose ();
		}






	}
}

