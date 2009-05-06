using System;

namespace CR_Documentor.Diagnostics
{
	/// <summary>
	/// Factory for retrieving log objects for writing messages to the
	/// DXCore event log.
	/// </summary>
	public static class LogManager
	{
		/// <summary>
		/// Gets a logging object for writing to the DXCore event log.
		/// </summary>
		/// <param name="logOwner">The <see cref="System.Type"/> that will be writing log messages.</param>
		/// <returns>
		/// An <see cref="CR_Documentor.Diagnostics.ILog"/> that can be used for
		/// writing messages to the event log.
		/// </returns>
		/// <remarks>
		/// Items in the <see cref="n:CR_Documentor.ContextMenu"/> namespace will
		/// log to the Menus log in DXCore; everything else logs to the ToolWindows log.
		/// </remarks>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="logOwner" /> is <see langword="null" />.
		/// </exception>
		public static ILog GetLogger(Type logOwner)
		{
			if (logOwner == null)
			{
				throw new ArgumentNullException("logOwner");
			}
			if (logOwner.Namespace.StartsWith("CR_Documentor.ContextMenu"))
			{
				return new MenuLogger(logOwner);
			}
			return new ToolWindowLogger(logOwner);
		}
	}
}
