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
		/// The base format for the URI that the web server will listen for. Takes two parameters - the port to listen on and the GUID identifier.
		/// </summary>
		public const string BaseUriFormat = "http://localhost:{0}/CR_Documentor/{1:D}/";

		/// <summary>
		/// Flag indicating the <see cref="CR_Documentor.Server.WebServer.Dispose()"/> method has been called.
		/// </summary>
		private bool _disposed = false;

		/// <summary>
		/// The listener that will serve incoming requests.
		/// </summary>
		private HttpListener _listener = null;

		/// <summary>
		/// The prefix the server is listening to requests on.
		/// </summary>
		private Uri _prefix = null;

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
			this.UniqueId = Guid.NewGuid();
			this._prefix = new Uri(String.Format(CultureInfo.InvariantCulture, BaseUriFormat, port, this.UniqueId));
			this._listener.Prefixes.Add(this._prefix.AbsoluteUri);
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
		/// Gets the Uniform Resource Indicator (URI) prefix handled by this
		/// server.
		/// </summary>
		/// <value>
		/// A <see cref="System.Uri"/> indicating the base location for requests
		/// that this server is configured to handle.
		/// </value>
		public virtual Uri Prefix
		{
			get
			{
				return this._prefix;
			}
		}

		/// <summary>
		/// Gets the unique identifier for this server.
		/// </summary>
		/// <value>
		/// A <see cref="System.Guid"/> that uniquely identifies this server instance.
		/// Will show up in the <see cref="CR_Documentor.Server.WebServer.Prefix"/>
		/// that the server listens on.
		/// </value>
		public virtual Guid UniqueId { get; private set; }

		/// <summary>
		/// Loops and listens for a request. Responds with the <see cref="CR_Documentor.Server.WebServer.Content"/>.
		/// </summary>
		private void AsyncListenThreadStart()
		{
			// DXCore might die on finalization if logging happens
			// during the shutdown and the timing is *just wrong*
			// so we try/catch around log statements here, swallow the exceptions,
			// and move on. It seems to be because of thread synchronization
			// issues.
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

					try
					{
						Log.Enter(ImageType.Method, "Preview server handling request.");
					}
					catch
					{
					}
					try
					{
						// Respond to the request by passing the content back.
						HttpListenerResponse response = context.Response;
						string content = String.IsNullOrEmpty(this.Content) ? "&nbsp;" : this.Content;
						byte[] buffer = Encoding.UTF8.GetBytes(content);
						response.ContentLength64 = buffer.Length;
						try
						{
							Log.Send(String.Format("Sending {0} bytes of content.", response.ContentLength64));
						}
						catch
						{
						}
						response.ContentEncoding = Encoding.UTF8;
						response.OutputStream.Write(buffer, 0, buffer.Length);
						response.OutputStream.Close();
					}
					finally
					{
						try
						{
							Log.Exit();
						}
						catch
						{
						}
					}
				}
				catch (HttpListenerException err)
				{
					try
					{
						Log.SendException("Server exiting request handling loop.", err);
					}
					catch
					{
					}
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
			foreach (object p in this._listener.Prefixes)
			{
				Log.Send(String.Format("Server listening for requests on {0}", p));
			}
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
			try
			{
				Log.Send("Server stopped.");
			}
			catch
			{
				// If this happens in the finalizer, DXCore might already
				// have finalized the log so we need to swallow any exceptions
				// here and move on.
			}
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
