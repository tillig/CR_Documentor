using System;
using System.Globalization;
using System.Net;
using CR_Documentor.Diagnostics;

namespace CR_Documentor.Server
{
	/// <summary>
	/// Listener that handles web requests coming in to a <see cref="CR_Documentor.Server.WebServer"/>.
	/// </summary>
	public class WebListener : IDisposable
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(WebListener));

		/// <summary>
		/// The base format for the URI that the web server will listen for. Takes two parameters - the port to listen on and the GUID identifier.
		/// </summary>
		public const string BaseUriFormat = "http://localhost:{0}/CR_Documentor/{1:D}/";

		/// <summary>
		/// Flag indicating the <see cref="CR_Documentor.Server.WebListener.Dispose()"/> method has been called.
		/// </summary>
		private bool _disposed = false;

		/// <summary>
		/// The listener that will serve incoming requests.
		/// </summary>
		private HttpListener _listener = null;

		/// <summary>
		/// Gets a value that indicates whether <see cref="CR_Documentor.Server.WebListener"/>
		/// has been started.
		/// </summary>
		/// <value>
		/// <see langword="true" /> if the <see cref="CR_Documentor.Server.WebListener"/>
		/// was started; otherwise, <see langword="false" />.
		/// </value>
		public virtual bool IsListening
		{
			get
			{
				return this._listener.IsListening;
			}
		}

		/// <summary>
		/// Gets the Uniform Resource Indicator (URI) prefix handled by this
		/// server.
		/// </summary>
		/// <value>
		/// A <see cref="System.Uri"/> indicating the base location for requests
		/// that this server is configured to handle.
		/// </value>
		public virtual Uri Prefix { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="WebListener"/> class.
		/// </summary>
		/// <param name="port">The port that the listener should listen on.</param>
		/// <param name="uniqueId">The unique ID to qualify the listener's prefix.</param>
		/// <exception cref="System.NotSupportedException">
		/// The <see cref="System.Net.HttpListener"/> is not supported on this
		/// operating system.
		/// </exception>
		public WebListener(UInt16 port, Guid uniqueId)
		{
			if (!HttpListener.IsSupported)
			{
				throw new NotSupportedException("The HttpListener class is not supported on this operating system.");
			}
			this._listener = new HttpListener();
			this.Prefix = new Uri(String.Format(CultureInfo.InvariantCulture, BaseUriFormat, port, uniqueId));
			this._listener.Prefixes.Add(this.Prefix.AbsoluteUri);
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="WebListener"/> is reclaimed by garbage collection.
		/// </summary>
		~WebListener()
		{
			this.Dispose(false);
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
				if (this._listener != null)
				{
					if (this._listener.IsListening)
					{
						this.Stop();
					}
					this._listener = null;
				}
			}
			this._disposed = true;
		}

		/// <summary>
		/// Retrieves the next incoming request on the listener.
		/// </summary>
		/// <returns>
		/// A <see cref="System.Net.HttpListenerContext"/> with the incoming request.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">
		/// Thrown if this method is called and the listener has not been started.
		/// </exception>
		/// <seealso cref="System.Net.HttpListener.GetContext"/>
		public virtual HttpListenerContext GetContext()
		{
			if (!this._listener.IsListening)
			{
				throw new InvalidOperationException("The listener must be started and listening to return a context.");
			}
			return this._listener.GetContext();
		}

		/// <summary>
		/// Starts the web server listening for requests.
		/// </summary>
		public virtual void Start()
		{
			this._listener.Start();
			foreach (object p in this._listener.Prefixes)
			{
				Log.Write(LogLevel.Info, String.Format("Listener listening for requests on {0}", p));
			}
		}

		/// <summary>
		/// Stops the web server listening for requests.
		/// </summary>
		public virtual void Stop()
		{
			this._listener.Stop();
			try
			{
				Log.Write(LogLevel.Info, "Listener stopped.");
			}
			catch
			{
				// If this happens in the finalizer, DXCore might already
				// have finalized the log so we need to swallow any exceptions
				// here and move on.
			}
		}
	}
}
