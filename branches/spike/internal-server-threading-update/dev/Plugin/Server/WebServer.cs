using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using DevExpress.CodeRush.Diagnostics.ToolWindows;

namespace CR_Documentor.Server
{
	/// <summary>
	/// Simple web server to provide content to the preview window.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This server is used to internally serve requests from the hosted browser
	/// in the preview window. By handling the <see cref="CR_Documentor.Server.WebServer.IncomingRequest"/>
	/// event, the HTML content can be written to the response stream for a given
	/// incoming request.
	/// </para>
	/// <para>
	/// The overall structure of the server is based on some source code posted
	/// on Planet Source Code here: <see href="http://www.planet-source-code.com/vb/scripts/ShowCode.asp?txtCodeId=6314&amp;lngWId=10"/>
	/// </para>
	/// </remarks>
	public class WebServer : IDisposable
	{
		/// <summary>
		/// Raised when an incoming request is received from the web server.
		/// </summary>
		/// <remarks>
		/// Handle this event and write to the <see cref="System.Net.HttpListenerResponse"/>
		/// on the <see cref="CR_Documentor.Server.HttpRequestEventArgs.RequestContext"/>
		/// to respond to incoming requests.
		/// </remarks>
		public event EventHandler<HttpRequestEventArgs> IncomingRequest = null;

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

		/// <summary>
		/// The listener used for receiving and responding to HTTP requests.
		/// </summary>
		private WebListener _listener = null;

		/// <summary>
		/// The queue of incoming requests that needs to be handled.
		/// </summary>
		private Queue<HttpListenerContext> _requestQueue = new Queue<HttpListenerContext>();

		/// <summary>
		/// Private storage for the current run state of the server.
		/// </summary>
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
		/// Gets the Windows forms control that owns the server.
		/// </summary>
		/// <value>
		/// A <see cref="System.Windows.Forms.Control"/> that "owns" the server.
		/// This control will be used when invoking events so threading occurs
		/// correctly.
		/// </value>
		public virtual Control OwnerControl { get; set; }

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

		/// <summary>
		/// Raises the event that indicates an incoming request needs to be responded to.
		/// </summary>
		/// <param name="context">
		/// The incoming request context that will be passed along to the event handlers.
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if the <paramref name="context" /> is <see langword="null" />.
		/// </exception>
		/// <seealso cref="CR_Documentor.Server.WebServer.IncomingRequest"/>
		private void RaiseIncomingRequest(HttpListenerContext context)
		{
			HttpRequestEventArgs e = new HttpRequestEventArgs(context);
			Control owner = this.OwnerControl;
			try
			{
				if (this.IncomingRequest != null)
				{
					if (owner != null)
					{
						owner.BeginInvoke(this.IncomingRequest, this, e);
					}
					else
					{
						this.IncomingRequest.BeginInvoke(this, e, null, null);
					}
				}
			}
			catch (Exception ex)
			{
				Log.SendException("Exception in raising the incoming request event.", ex);
			}
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
