using System;
using System.Globalization;
using System.Reflection;
using DevExpress.DXCore.Threading;

namespace CR_Documentor.Diagnostics
{
	/// <summary>
	/// Base class for log object implementations.
	/// </summary>
	public abstract class Logger : ILog
	{
		/// <summary>
		/// Delegate for writing a log message to the internal DXCore log.
		/// </summary>
		/// <param name="message">The message to write to the log.</param>
		private delegate void MessageHandler(string message);

		/// <summary>
		/// Delegate for writing an exception to the internal DXCore log.
		/// </summary>
		/// <param name="error">The <see cref="System.Exception"/> to write to the log.</param>
		private delegate void ExceptionHandler(Exception error);

		/// <summary>
		/// Delegate for calling a no-parameter method on the internal DXCore log.
		/// </summary>
		private delegate void VoidHandler();

		/// <summary>
		/// Delegate for indenting the log.
		/// </summary>
		private MessageHandler _enter;

		/// <summary>
		/// Delegate for outdenting the log.
		/// </summary>
		private VoidHandler _exit;

		/// <summary>
		/// Delegate for writing error messages to the log.
		/// </summary>
		private MessageHandler _sendError;

		/// <summary>
		/// Delegate for writing exceptions to the log.
		/// </summary>
		private ExceptionHandler _sendException;

		/// <summary>
		/// Delegate for writing informational messages to the log.
		/// </summary>
		private MessageHandler _sendMsg;

		/// <summary>
		/// Delegate for writing warning messages to the log.
		/// </summary>
		private MessageHandler _sendWarning;

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
		/// Builds a complete, formatted log message.
		/// </summary>
		/// <param name="message">The content of the message.</param>
		/// <returns>
		/// A message containing the proper prefixes/suffixes for log entry.
		/// </returns>
		private string BuildMessage(string message)
		{
			return String.Format(CultureInfo.InvariantCulture, MessageFormat, this.LogOwner.Name, message);
		}

		/// <summary>
		/// Writes a message to the log and indents the log so subsequent messages appear as "children."
		/// </summary>
		/// <param name="message">The message to write to the log.</param>
		public virtual void Enter(string message)
		{
			SynchronizationManager.BeginInvoke(this._enter, new object[] { this.BuildMessage(message) });
		}

		/// <summary>
		/// Exits a logical log context and outdents the log.
		/// </summary>
		public virtual void Exit()
		{
			SynchronizationManager.BeginInvoke(this._exit, new object[] { });
		}

		/// <summary>
		/// Gets a public <see langword="static" /> method from the internal log object.
		/// </summary>
		/// <param name="methodName">The name of the method to get.</param>
		/// <param name="parameterTypes">The type(s) of parameters it accepts.</param>
		/// <returns>
		/// A reference to the public <see langword="static" /> method on the internal
		/// log object.
		/// </returns>
		private MethodInfo GetPluginLogTypeMethod(string methodName, Type[] parameterTypes)
		{
			return this.PluginLogType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static, null, parameterTypes, null);
		}

		/// <summary>
		/// Initializes delegates to write things to the internal DXCore log.
		/// </summary>
		private void InitializeLoggingDelegates()
		{
			Type[] stringParameter = new Type[] { typeof(string) };
			MethodInfo enterMethod = this.GetPluginLogTypeMethod("Enter", stringParameter);
			this._enter = delegate(string message)
			{
				enterMethod.Invoke(null, new object[] { message });
			};
			MethodInfo sendMsgMethod = this.GetPluginLogTypeMethod("SendMsg", stringParameter);
			this._sendMsg = delegate(string message)
			{
				sendMsgMethod.Invoke(null, new object[] { message });
			};
			MethodInfo sendWarningMethod = this.GetPluginLogTypeMethod("SendWarning", stringParameter);
			this._sendWarning = delegate(string message)
			{
				sendWarningMethod.Invoke(null, new object[] { message });
			};
			MethodInfo sendErrorMethod = this.GetPluginLogTypeMethod("SendError", stringParameter);
			this._sendError = delegate(string message)
			{
				sendErrorMethod.Invoke(null, new object[] { message });
			};

			MethodInfo exitMethod = this.GetPluginLogTypeMethod("Exit", System.Type.EmptyTypes);
			this._exit = delegate()
			{
				exitMethod.Invoke(null, null);
			};
			MethodInfo sendExceptionMethod = this.GetPluginLogTypeMethod("SendException", new Type[] { typeof(Exception) });
			this._sendException = delegate(Exception err)
			{
				sendExceptionMethod.Invoke(null, new object[] { err });
			};
		}

		/// <summary>
		/// Writes a message to the log.
		/// </summary>
		/// <param name="level">The level the message should appear at.</param>
		/// <param name="message">The message to write.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="message" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// Thrown if <paramref name="message" /> is <see cref="System.String.Empty" />.
		/// </exception>
		public virtual void Write(LogLevel level, string message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			if (message.Length == 0)
			{
				throw new ArgumentException("Message may not be empty.", "message");
			}
			MessageHandler levelHandler = null;
			switch (level)
			{
				case LogLevel.Error:
					levelHandler = this._sendError;
					break;
				case LogLevel.Info:
					levelHandler = this._sendMsg;
					break;
				case LogLevel.Warn:
					levelHandler = this._sendWarning;
					break;
			}
			SynchronizationManager.BeginInvoke(levelHandler, new object[] { this.BuildMessage(message) });
		}

		/// <summary>
		/// Writes a message to the log.
		/// </summary>
		/// <param name="level">The level the message should appear at.</param>
		/// <param name="message">The message to write.</param>
		/// <param name="error">Additional error information to include with the message.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="message" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		/// Thrown if <paramref name="message" /> is <see cref="System.String.Empty" />.
		/// </exception>
		public virtual void Write(LogLevel level, string message, Exception error)
		{
			this.Write(level, message);
			if (error != null)
			{
				SynchronizationManager.BeginInvoke(this._sendException, new object[] { error });
			}
		}
	}
}
