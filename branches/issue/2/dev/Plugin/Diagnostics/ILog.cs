using System;

namespace CR_Documentor.Diagnostics
{
	/// <summary>
	/// Represents a logging service.
	/// </summary>
	public interface ILog
	{
		/// <summary>
		/// Writes a message to the log and indents the log so subsequent messages appear as "children."
		/// </summary>
		/// <param name="message">
		/// The message to write to the log.
		/// </param>
		void Enter(string message);

		/// <summary>
		/// Exits a logical log context and outdents the log.
		/// </summary>
		void Exit();
	}
}
