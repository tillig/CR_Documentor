using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CR_Documentor.Diagnostics;
using Debug = System.Diagnostics.Debug;

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
	/// After instantiating an instance of the web server, call the
	/// <see cref="CR_Documentor.Server.WebServer.Start"/> method to begin listening
	/// for requests. When you're done listening for requests, call the
	/// <see cref="CR_Documentor.Server.WebServer.Stop"/> method to shut the
	/// process down.
	/// </para>
	/// </remarks>
	public class WebServer : IDisposable
	{
		/// <summary>
		/// Log entry handler.
		/// </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(WebServer));

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
			/// The server is in the process of stopping.
			/// </summary>
			Stopping,

			/// <summary>
			/// The server is in the process of starting.
			/// </summary>
			Starting,

			/// <summary>
			/// The server is started.
			/// </summary>
			Started
		}

		/// <summary>
		/// The thread that contains the primary connection manager loop.
		/// </summary>
		private Thread _connectionManagerThread = null;

		/// <summary>
		/// Flag indicating whether the object has been disposed or not.
		/// </summary>
		private bool _disposed = false;

		/// <summary>
		/// The listener used for receiving and responding to HTTP requests.
		/// </summary>
		private WebListener _listener = null;

		/// <summary>
		/// Private storage for the current run state of the server.
		/// </summary>
		private long _runState = (long)State.Stopped;

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
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="WebServer"/> is reclaimed by garbage collection.
		/// </summary>
		~WebServer()
		{
			this.Dispose(false);
		}

		/// <summary>
		/// Entry point for the connection manager to start listening for requests
		/// and add incoming ones to the queue.
		/// </summary>
		private void ConnectionManagerThreadStart()
		{
			Interlocked.Exchange(ref this._runState, (long)State.Starting);
			try
			{
				Log.Write(LogLevel.Info, "Starting web server connection manager.");
				if (!this._listener.IsListening)
				{
					this._listener.Start();
				}
				if (this._listener.IsListening)
				{
					Interlocked.Exchange(ref this._runState, (long)State.Started);
				}

				try
				{
					while (RunState == State.Started)
					{
						HttpListenerContext context = this._listener.GetContext();
						this.RaiseIncomingRequest(context);
					}
				}
				catch (HttpListenerException)
				{
					// This will occur when the listener gets shut down.
					// Just swallow it and move on.
				}
			}
			finally
			{
				Log.Write(LogLevel.Info, "Web server connection manager stopped.");
				Interlocked.Exchange(ref this._runState, (long)State.Stopped);
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
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
		/// <see langword="true"/> to release both managed and unmanaged resources;
		/// <see langword="false"/> to release only unmanaged resources.
		/// </param>
		private void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				return;
			}
			if (disposing)
			{
				if (this.RunState != State.Stopped)
				{
					// This shuts down the web listener and lets threads clean
					// themselves up.
					this.Stop();
				}
				if (this._connectionManagerThread != null)
				{
					this._connectionManagerThread.Abort();
					this._connectionManagerThread = null;
				}
			}
			this._disposed = true;
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
		/// <remarks>
		/// Once the event has been raised and everything is said and done,
		/// the response to the client will be closed and sent.
		/// </remarks>
		/// <seealso cref="CR_Documentor.Server.WebServer.IncomingRequest"/>
		/// <seealso cref="System.Net.HttpListenerResponse.Close()"/>
		private void RaiseIncomingRequest(HttpListenerContext context)
		{
			HttpRequestEventArgs e = new HttpRequestEventArgs(context);
			try
			{
				if (this.IncomingRequest != null)
				{
					this.IncomingRequest.BeginInvoke(this, e, null, null);
				}
			}
			catch (Exception ex)
			{
				Log.Write(LogLevel.Error, "Exception in raising the incoming request event.", ex);
			}
		}

		/// <summary>
		/// Starts the web server and allows incoming requests to be handled.
		/// </summary>
		/// <exception cref="System.Threading.ThreadStateException">
		/// Thrown if the request handling process is already running or if the
		/// process could not be properly initialized.
		/// </exception>
		/// <exception cref="System.TimeoutException">
		/// Thrown if the request handling process could not be started within 10 seconds.
		/// </exception>
		/// <remarks>
		/// <para>
		/// This method initializes and starts a thread that acts as a master
		/// "connection manager." The connection manager thread is responsible for
		/// two things - starting up a request queue handler (a separate thread
		/// that raises the <see cref="CR_Documentor.Server.WebServer.IncomingRequest"/>
		/// event for incoming requests) and adding incoming requests to a queue
		/// to be handled.
		/// </para>
		/// </remarks>
		public virtual void Start()
		{
			if (this._connectionManagerThread == null || this._connectionManagerThread.ThreadState == ThreadState.Stopped)
			{
				this._connectionManagerThread = new Thread(new ThreadStart(this.ConnectionManagerThreadStart));
				this._connectionManagerThread.Name = String.Format(CultureInfo.InvariantCulture, "ConnectionManager_{0}", this.UniqueId);
			}
			else if (this._connectionManagerThread.ThreadState == ThreadState.Running)
			{
				throw new ThreadStateException("The request handling process is already running.");
			}

			// By the time we get here, we should have a pristine connection manager
			// thread that's never been started before.
			if (this._connectionManagerThread.ThreadState != ThreadState.Unstarted)
			{
				throw new ThreadStateException("The request handling process was not properly initialized so it could not be started.");
			}
			this._connectionManagerThread.Start();

			long waitTime = DateTime.Now.Ticks + TimeSpan.TicksPerSecond * 10;
			while (this.RunState != State.Started)
			{
				Thread.Sleep(100);
				if (DateTime.Now.Ticks > waitTime)
				{
					throw new TimeoutException("Unable to start the request handling process.");
				}
			}
		}

		/// <summary>
		/// Stops the web server from handling incoming requests.
		/// </summary>
		/// <exception cref="System.TimeoutException">
		/// Thrown if the request handling process could not be stopped within 10 seconds.
		/// </exception>
		public virtual void Stop()
		{
			// Setting the runstate to something other than "started" and
			// stopping the listener should abort the AddIncomingRequestToQueue
			// method and allow the ConnectionManagerThreadStart sequence to
			// end, which sets the RunState to Stopped.
			Interlocked.Exchange(ref this._runState, (long)State.Stopping);
			if (this._listener.IsListening)
			{
				this._listener.Stop();
			}
			long waitTime = DateTime.Now.Ticks + TimeSpan.TicksPerSecond * 10;
			while (this.RunState != State.Stopped)
			{
				Thread.Sleep(100);
				if (DateTime.Now.Ticks > waitTime)
				{
					throw new TimeoutException("Unable to stop the web server process.");
				}
			}

			this._connectionManagerThread = null;
		}
	}
}
