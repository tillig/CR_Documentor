using System;
using System.Globalization;
using System.Reflection;
using DevExpress.DXCore.Threading;
using DevExpress.CodeRush.Diagnostics;

namespace CR_Documentor.Diagnostics
{
	/// <summary>
	/// Base class for log object implementations.
	/// </summary>
	public abstract class Logger<TLog> : ILog
		where TLog : LogBase<TLog>, new()
	{
		/// <summary>
		/// Format for log messages - {0} is the log owner type name; {1} is the message.
		/// </summary>
		public const string MessageFormat = "CR_Documentor [{0}]: {1}";

		// TODO: Refactor delegates into lambdas.

		/// <summary>
		/// Delegate for writing a log message to the internal DXCore log.
		/// </summary>
		/// <param name="message">The message to write to the log.</param>
		public delegate void MessageHandler(string message);

		/// <summary>
		/// Delegate for writing an exception to the internal DXCore log.
		/// </summary>
		/// <param name="error">The <see cref="System.Exception"/> to write to the log.</param>
		public delegate void ExceptionHandler(Exception error);

		/// <summary>
		/// Delegate for writing error messages to the log.
		/// </summary>
		public MessageHandler SendErrorHandler { get; private set; }

		/// <summary>
		/// Delegate for writing exceptions to the log.
		/// </summary>
		public ExceptionHandler SendExceptionHandler { get; private set; }

		/// <summary>
		/// Delegate for writing informational messages to the log.
		/// </summary>
		public MessageHandler SendMsgHandler { get; private set; }

		/// <summary>
		/// Delegate for writing warning messages to the log.
		/// </summary>
		public MessageHandler SendWarningHandler { get; private set; }

		/// <summary>
		/// Gets the <see cref="System.Type"/> for which this log object writes messages.
		/// </summary>
		/// <value>
		/// The <see cref="System.Type"/> that owns this log object.
		/// </value>
		public Type LogOwner { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Logger"/> class.
		/// </summary>
		/// <param name="logOwner">The <see cref="System.Type"/> that will be writing log messages.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="logOwner" /> is <see langword="null" />.
		/// </exception>
		protected Logger(Type logOwner)
		{
			if (logOwner == null)
			{
				throw new ArgumentNullException("logOwner");
			}
			this.LogOwner = logOwner;
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
			return typeof(LogBase<TLog>).GetMethod(methodName, BindingFlags.Public | BindingFlags.Static, null, parameterTypes, null);
		}

		/// <summary>
		/// Initializes delegates to write things to the internal DXCore log.
		/// </summary>
		private void InitializeLoggingDelegates()
		{
			Type[] stringParameter = new Type[] { typeof(string) };
			MethodInfo sendMsgMethod = this.GetPluginLogTypeMethod("SendMsg", stringParameter);
			this.SendMsgHandler = delegate(string message)
			{
				sendMsgMethod.Invoke(null, new object[] { message });
			};
			MethodInfo sendWarningMethod = this.GetPluginLogTypeMethod("SendWarning", stringParameter);
			this.SendWarningHandler = delegate(string message)
			{
				sendWarningMethod.Invoke(null, new object[] { message });
			};
			MethodInfo sendErrorMethod = this.GetPluginLogTypeMethod("SendError", stringParameter);
			this.SendErrorHandler = delegate(string message)
			{
				sendErrorMethod.Invoke(null, new object[] { message });
			};
			MethodInfo sendExceptionMethod = this.GetPluginLogTypeMethod("SendException", new Type[] { typeof(Exception) });
			this.SendExceptionHandler = delegate(Exception err)
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
					levelHandler = this.SendErrorHandler;
					break;
				case LogLevel.Info:
					levelHandler = this.SendMsgHandler;
					break;
				case LogLevel.Warn:
					levelHandler = this.SendWarningHandler;
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
				SynchronizationManager.BeginInvoke(this.SendExceptionHandler, new object[] { error });
			}
		}
	}
}
