using System;
using DevExpress.CodeRush.Diagnostics.ToolWindows;

namespace CR_Documentor.Diagnostics
{
	/// <summary>
	/// Logging implementation that writes log messages for tool windows.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Do not directly instantiate a <see cref="CR_Documentor.Diagnostics.ToolWindowLogger"/>;
	/// instead, use the <see cref="CR_Documentor.Diagnostics.LogManager.GetLogger"/>
	/// method to retrieve the proper logging implementation for your class.
	/// </para>
	/// </remarks>
	public class ToolWindowLogger : Logger<Log>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ToolWindowLogger"/> class.
		/// </summary>
		/// <param name="logOwner">The <see cref="System.Type"/> that will be writing log messages.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="logOwner" /> is <see langword="null" />.
		/// </exception>
		public ToolWindowLogger(Type logOwner) : base(logOwner) { }
	}
}
