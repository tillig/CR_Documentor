using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace CR_Documentor.Server
{
	/// <summary>
	/// Simple web server to provide content to the preview window.
	/// </summary>
	public class WebServer : IDisposable
	{
		/// <summary>
		/// Enumeration of possible run states for the web server.
		/// </summary>
		public enum State
		{
			/// <summary>
			/// The server is stopped.
			/// </summary>
			Stopped,

			/// <summary>
			/// The server is in the process of starting or stopping.
			/// </summary>
			Intermediate,

			/// <summary>
			/// The server is started.
			/// </summary>
			Started
		}

		private WebListener _listener = null;
		private Queue<HttpListenerContext> _requestQueue = new Queue<HttpListenerContext>();
		private long _runState = (long)State.Stopped;

		/// <summary>
		/// Gets or sets the content to serve to the browser.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> with the HTML to serve to the
		/// browser on incoming requests.
		/// </value>
		public virtual string Content { get; set; }

		/// <summary>
		/// Gets the port that the server was initialized with.
		/// </summary>
		/// <value>
		/// A <see cref="System.UInt16"/> with the port this server is listening on.
		/// </value>
		public virtual UInt16 Port { get; private set; }

		/// <summary>
		/// Gets the current run state of the server.
		/// </summary>
		/// <value>
		/// A <see cref="CR_Documentor.Server.WebServer.State"/> indicating the current
		/// run state of the server.
		/// </value>
		public State RunState
		{
			get
			{
				return (State)Interlocked.Read(ref _runState);
			}
		}

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
		/// Initializes a new instance of the <see cref="WebServer"/> class.
		/// </summary>
		/// <param name="port">
		/// The port that should be listened to for incoming requests.
		/// </param>
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
