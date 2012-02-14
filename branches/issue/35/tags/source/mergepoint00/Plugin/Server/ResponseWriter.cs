using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
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
		/// Gets an embedded resource from the specified assembly and writes it to the response.
		/// </summary>
		/// <param name="context">The incoming request context.</param>
		/// <param name="assembly">The assembly in which the resource resides.</param>
		/// <param name="resourcePath">The path to tne embedded resource in the current assembly.</param>
		/// <param name="contentType">The MIME type to write to the response.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="context" />, <paramref name="assembly" />, <paramref name="resourcePath" />,
		/// or <paramref name="contentType" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// Thrown if <paramref name="resourcePath" /> or <paramref name="contentType" />
		/// is empty; or if <paramref name="resourcePath" /> is not found as an
		/// embedded resource in the current assembly.
		/// </exception>
		public static void WriteResource(HttpListenerContext context, Assembly assembly, string resourcePath, string contentType)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			if (resourcePath == null)
			{
				throw new ArgumentNullException("resourcePath");
			}
			if (resourcePath.Length == 0)
			{
				throw new ArgumentException("resourcePath may not be empty.", "resourcePath");
			}
			if (contentType == null)
			{
				throw new ArgumentNullException("contentType");
			}
			if (contentType.Length == 0)
			{
				throw new ArgumentException("contentType may not be empty.", "contentType");
			}

			using (Stream resourceStream = assembly.GetManifestResourceStream(resourcePath))
			{
				if (resourceStream == null)
				{
					throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Resource {0} not found in assembly {1}.", resourcePath, assembly.FullName), "resourcePath");
				}

				byte[] content = new byte[resourceStream.Length];
				resourceStream.Read(content, 0, (int)resourceStream.Length);
				WriteSuccessResponse(context.Response, content, null, contentType);
			}
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
