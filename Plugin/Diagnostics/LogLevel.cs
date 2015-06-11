using System;

namespace CR_Documentor.Diagnostics
{
	/// <summary>
	/// Indicates the different levels at whic a log message might appear.
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// Informational message. FYI.
		/// </summary>
		Info,

		/// <summary>
		/// Warning message - not a real problem, but too many of these may mean a deeper problem.
		/// </summary>
		Warn,

		/// <summary>
		/// Real problems. An exception got hit or otherwise something really got messed up.
		/// </summary>
		Error
	}
}
