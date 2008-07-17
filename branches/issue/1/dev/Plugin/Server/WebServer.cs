using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;
using DevExpress.CodeRush.Diagnostics.ToolWindows;

namespace CR_Documentor.Server
{
	/// <summary>
	/// Simple web server to provide content to the preview window.
	/// </summary>
	public class WebServer : IDisposable
	{
		/// <summary>
		/// Flag indicating the <see cref="CR_Documentor.Server.WebServer.Dispose"/> method has been called.
		/// </summary>
		private bool _disposed = false;

		/// <summary>
		/// The listener that will serve incoming requests.
		/// </summary>
		private HttpListener _listener = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="WebServer"/> class.
		/// </summary>
		/// <exception cref="System.NotSupportedException">
		/// The <see cref="System.Net.HttpListener"/> is not supported on this
		/// operating system.
		/// </exception>
		public WebServer(UInt16 port)
		{
			if (!HttpListener.IsSupported)
			{
				throw new NotSupportedException("The HttpListener class is not supported on this operating system.");
			}
			this._listener = new HttpListener();
			this._listener.Prefixes.Add(String.Format(CultureInfo.InvariantCulture, "http://*:{0}/", port));
		}

		/// <summary>
		/// Gets or sets the HTML content to return for any request.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> with the complete HTML page to return
		/// for any request.
		/// </value>
		public virtual string Content { get; set; }

		/// <summary>
		/// Gets a value indicating if the server is listening for requests.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the server is listening, <see langword="false" />
		/// if not.
		/// </value>
		public virtual bool IsListening { get; private set; }

		/// <summary>
		/// Gets the Uniform Resource Indicator (URI) prefixes handled by this
		/// server.
		/// </summary>
		/// <value>
		/// An <see cref="System.Net.HttpListenerPrefixCollection"/> that contains
		/// the URI prefixes that this server is configured to handle.
		/// </value>
		public virtual HttpListenerPrefixCollection Prefixes
		{
			get
			{
				return this._listener.Prefixes;
			}
		}

		/// <summary>
		/// Loops and listens for a request. Responds with the <see cref="CR_Documentor.Server.WebServer.Content"/>.
		/// </summary>
		private void AsyncListenThreadStart()
		{
			while (this.IsListening)
			{
				try
				{
					// GetContext blocks until it gets a request, but will throw
					// an HttpListenerException if you call Stop on the listener
					// while it's waiting. The "while(this.IsListening)" loop
					// is a double-check to make sure if we don't get the exception
					// we'll still exit.
					HttpListenerContext context = this._listener.GetContext();

					// When we start serving other things - images, etc. - we'll need to get the request info.
					// HttpListenerRequest request = context.Request;

					// Respond to the request by passing the content back.
					HttpListenerResponse response = context.Response;
					byte[] buffer = Encoding.UTF8.GetBytes(this.Content);
					response.ContentLength64 = buffer.Length;
					response.OutputStream.Write(buffer, 0, buffer.Length);
					response.OutputStream.Close();
				}
				catch (HttpListenerException err)
				{
					Log.SendException("Server exiting request handling loop.", err);
					break;
				}
			}
		}

		/// <summary>
		/// Starts the web server listening for requests.
		/// </summary>
		public virtual void Start()
		{
			if (this.IsListening)
			{
				return;
			}
			this._listener.Start();
			new ThreadStart(this.AsyncListenThreadStart).BeginInvoke(null, null);
			this.IsListening = true;
			Log.Send("Server listening for requests.");
		}

		/// <summary>
		/// Stops the web server listening for requests.
		/// </summary>
		public virtual void Stop()
		{
			if (!this.IsListening)
			{
				return;
			}
			this.IsListening = false;
			this._listener.Stop();
			Log.Send("Server stopped.");
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing,
		/// or resetting unmanaged resources.
		/// </summary>
		public virtual void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true" /> to release both managed and unmanaged resources;
		/// <see langword="false" /> to release only unmanaged resources.
		/// </param>
		private void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing)
			{
				this.Stop();
				this._listener = null;
			}
			this._disposed = true;
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="WebServer"/> is reclaimed by garbage collection.
		/// </summary>
		~WebServer()
		{
			this.Dispose(false);
		}
	}
}
