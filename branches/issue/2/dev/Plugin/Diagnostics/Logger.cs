using System;
using System.Reflection;
using DevExpress.DXCore.Threading;

namespace CR_Documentor.Diagnostics
{
	/// <summary>
	/// Delegate for writing a log message to the internal DXCore log.
	/// </summary>
	/// <param name="message">The message to write to the log.</param>
	public delegate void WriteLogMessageHandler(string message);

	/// <summary>
	/// Delegate for writing an exception to the internal DXCore log.
	/// </summary>
	/// <param name="error">The <see cref="System.Exception"/> to write to the log.</param>
	public delegate void WriteLogExceptionHandler(Exception error);

	/// <summary>
	/// Base class for log object implementations.
	/// </summary>
	public abstract class Logger : ILog
	{
		/// <summary>
		/// Delegate for indenting the log.
		/// </summary>
		private WriteLogMessageHandler _enter;

		/// <summary>
		/// Format for log messages - {0} is the log owner type name; {1} is the message.
		/// </summary>
		public const string MessageFormat = "CR_Documentor [{0}]: {1}";

		/// <summary>
		/// Gets the <see cref="System.Type"/> for which this log object writes messages.
		/// </summary>
		/// <value>
		/// The <see cref="System.Type"/> that owns this log object.
		/// </value>
		public Type LogOwner { get; private set; }

		/// <summary>
		/// Gets the type that writes log messages internally.
		/// </summary>
		/// <value>
		/// A <see cref="System.Type"/> from DXCore that will actually write the
		/// log messages to the DXCore log.
		/// </value>
		public Type PluginLogType { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Logger"/> class.
		/// </summary>
		/// <param name="logOwner">The <see cref="System.Type"/> that will be writing log messages.</param>
		/// <param name="pluginLogType">The <see langword="static" /> type that DXCore will use to log messages.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="logOwner" /> or <paramref name="pluginLogType" />
		/// is <see langword="null" />.
		/// </exception>
		protected Logger(Type logOwner, Type pluginLogType)
		{
			if (logOwner == null)
			{
				throw new ArgumentNullException("logOwner");
			}
			if (pluginLogType == null)
			{
				throw new ArgumentNullException("pluginLogType");
			}
			this.LogOwner = logOwner;
			this.PluginLogType = pluginLogType;
			this.InitializeLoggingDelegates();
		}

		/// <summary>
		/// Writes a message to the log and indents the log so subsequent messages appear as "children."
		/// </summary>
		/// <param name="message">The message to write to the log.</param>
		public virtual void Enter(string message)
		{
			SynchronizationManager.BeginInvoke(this._enter, new object[] { message });
		}

		/// <summary>
		/// Exits a logical log context and outdents the log.
		/// </summary>
		public virtual void Exit()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Initializes delegates to write things to the internal DXCore log.
		/// </summary>
		private void InitializeLoggingDelegates()
		{
			MethodInfo enterMethod = this.PluginLogType.GetMethod("Enter", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null);
			this._enter = delegate(string message)
			{
				enterMethod.Invoke(null, new object[] { message });
			};
		}

		/// <summary>
		/// Writes a message to the log.
		/// </summary>
		/// <param name="level">The level the message should appear at.</param>
		/// <param name="message">The message to write.</param>
		public virtual void Write(LogLevel level, string message)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Writes a message to the log.
		/// </summary>
		/// <param name="level">The level the message should appear at.</param>
		/// <param name="message">The message to write.</param>
		/// <param name="error">Additional error information to include with the message.</param>
		public virtual void Write(LogLevel level, string message, Exception error)
		{
			throw new NotImplementedException();
		}
	}
}
