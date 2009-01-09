using System;
using System.Net;
using System.Text;

namespace CR_Documentor.Server
{
	/// <summary>
	/// Utility class for issuing responses to web requests.
	/// </summary>
	public static class ResponseWriter
	{
		/// <summary>
		/// Writes an HTML response.
		/// </summary>
		/// <param name="context">The incoming request context.</param>
		/// <param name="html">The HTML to respond with.</param>
		public static void WriteHtml(HttpListenerContext context, string html)
		{
			HttpListenerResponse response = context.Response;
			string content = String.IsNullOrEmpty(html) ? "&nbsp;" : html;
			byte[] buffer = Encoding.UTF8.GetBytes(content);
			WriteSuccessResponse(response, buffer, Encoding.UTF8);
		}

		/// <summary>
		/// Writes a successful response.
		/// </summary>
		/// <param name="response">The response object to write to.</param>
		/// <param name="content">The content to send.</param>
		/// <param name="encoding">The encoding the content is in.</param>
		private static void WriteSuccessResponse(HttpListenerResponse response, byte[] content, Encoding encoding)
		{
			response.StatusCode = (int)HttpStatusCode.OK;
			response.StatusDescription = "OK";
			response.ContentLength64 = content.Length;
			response.ContentEncoding = encoding;
			response.OutputStream.Write(content, 0, content.Length);
			response.OutputStream.Close();
			response.Close();
		}
	}
}
