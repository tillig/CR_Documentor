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
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="context" /> is <see langword="null" />.
		/// </exception>
		public static void WriteHtml(HttpListenerContext context, string html)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			HttpListenerResponse response = context.Response;
			string content = String.IsNullOrEmpty(html) ? "&nbsp;" : html;
			byte[] buffer = Encoding.UTF8.GetBytes(content);
			WriteSuccessResponse(response, buffer, Encoding.UTF8, "text/html");
		}

		/// <summary>
		/// Writes a successful response.
		/// </summary>
		/// <param name="response">The response object to write to.</param>
		/// <param name="content">The content to send.</param>
		/// <param name="encoding">The encoding the content is in.</param>
		/// <param name="contentType">The MIME type of the response.</param>
		private static void WriteSuccessResponse(HttpListenerResponse response, byte[] content, Encoding encoding, string contentType)
		{
			response.StatusCode = (int)HttpStatusCode.OK;
			response.StatusDescription = "OK";
			response.ContentLength64 = content.Length;
			if (encoding != null)
			{
				response.ContentEncoding = encoding;
			}
			response.ContentType = contentType;
			response.OutputStream.Write(content, 0, content.Length);
			response.OutputStream.Close();
			response.Close();
		}
	}
}
