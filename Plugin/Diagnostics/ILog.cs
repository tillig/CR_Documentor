using System;

namespace CR_Documentor.Diagnostics
{
	/// <summary>
	/// Represents a logging service.
	/// </summary>
	public interface ILog
	{
		/// <summary>
		/// Writes a message to the log.
		/// </summary>
		/// <param name="level">The level the message should appear at.</param>
		/// <param name="message">The message to write.</param>
		void Write(LogLevel level, string message);

		/// <summary>
		/// Writes a message to the log.
		/// </summary>
		/// <param name="level">The level the message should appear at.</param>
		/// <param name="message">The message to write.</param>
		/// <param name="error">Additional error information to include with the message.</param>
		void Write(LogLevel level, string message, Exception error);
	}
}
