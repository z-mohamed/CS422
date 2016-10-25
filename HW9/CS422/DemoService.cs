using System;

namespace CS422
{
	public class DemoService : WebService
	{
		private const string c_template =
			"<html>This is the response to the request:<br>" +
			"Method: {0}<br>Request-Target/URI: {1}<br>" +
			"Request body size, in bytes: {2}<br><br>" +
			"Student ID: {3}</html>";


		public DemoService ()
		{
		}


		public override void Handler(WebRequest req)
		{
			req.HTTP_method = req.HTTP_method.Substring (0, 3);
			int byteCount = System.Text.ASCIIEncoding.Unicode.GetByteCount(c_template);
			string respond = String.Format (c_template, req.HTTP_method, req.URI, byteCount.ToString(), "11395829");
			req.WriteHTMLResponse (respond);

		}

		public override string ServiceURI 
		{
			get
			{
				return "/";
			}
		}
	}
}