using System;
using System.Collections.Generic;
using System.Text;

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
		public static ILog GetLogger(Type logOwner)
		{
			throw new NotImplementedException();
		}
	}
}
