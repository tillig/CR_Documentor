using System;

namespace CR_Documentor.Server
{
	/// <summary>
	/// Simple web server to provide content to the preview window.
	/// </summary>
	public class WebServer : IDisposable
	{
		private WebListener _listener = null;

		/// <summary>
		/// Gets the port that the server was initialized with.
		/// </summary>
		/// <value>
		/// A <see cref="System.UInt16"/> with the port this server is listening on.
		/// </value>
		public virtual UInt16 Port { get; private set; }

		/// <summary>
		/// Gets the unique identifier for this server.
		/// </summary>
		/// <value>
		/// A <see cref="System.Guid"/> that uniquely identifies this server instance.
		/// </value>
		public virtual Guid UniqueId { get; private set; }

		/// <summary>
		/// Gets the URL that the server is listening on.
		/// </summary>
		/// <value>
		/// A <see cref="System.Uri"/> with the base URL that requests will
		/// get handled on for this server.
		/// </value>
		public virtual Uri Url
		{
			get
			{
				return new Uri(this._listener.Prefix.AbsoluteUri);
			}
		}

		/// <summary>
		/// Gets or sets the content to serve to the browser.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> with the HTML to serve to the
		/// browser on incoming requests.
		/// </value>
		public virtual string Content { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="WebServer"/> class.
		/// </summary>
		/// <exception cref="System.NotSupportedException">
		/// The <see cref="System.Net.HttpListener"/> is not supported on this
		/// operating system.
		/// </exception>
		public WebServer(UInt16 port)
		{
			this.Port = port;
			this.UniqueId = Guid.NewGuid();
			this._listener = new WebListener(this.Port, this.UniqueId);
		}

		public virtual void Start()
		{
			throw new NotImplementedException();
		}

		public virtual void Stop()
		{
			throw new NotImplementedException();
		}

		public virtual void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
