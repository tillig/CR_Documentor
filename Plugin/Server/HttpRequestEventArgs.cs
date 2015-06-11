using System;
using System.Net;

namespace CR_Documentor.Server
{
	/// <summary>
	/// Event arguments for responding to incoming HTTP requests.
	/// </summary>
	public class HttpRequestEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the incoming request context information.
		/// </summary>
		/// <value>
		/// A <see cref="System.Net.HttpListenerContext"/> with the incoming
		/// request information and the stream on which to respond to the
		/// request.
		/// </value>
		public HttpListenerContext RequestContext { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpRequestEventArgs"/> class.
		/// </summary>
		/// <param name="requestContext">The incoming HTTP request context.</param>
		public HttpRequestEventArgs(HttpListenerContext requestContext)
		{
			this.RequestContext = requestContext;
		}
	}
}
