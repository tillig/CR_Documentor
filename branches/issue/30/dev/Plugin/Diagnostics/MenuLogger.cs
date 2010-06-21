using System;
using DevExpress.CodeRush.Diagnostics.Menus;

namespace CR_Documentor.Diagnostics
{
	/// <summary>
	/// Logging implementation that writes log messages for menus.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Do not directly instantiate a <see cref="CR_Documentor.Diagnostics.MenuLogger"/>;
	/// instead, use the <see cref="CR_Documentor.Diagnostics.LogManager.GetLogger"/>
	/// method to retrieve the proper logging implementation for your class.
	/// </para>
	/// </remarks>
	public class MenuLogger : Logger
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MenuLogger"/> class.
		/// </summary>
		/// <param name="logOwner">The <see cref="System.Type"/> that will be writing log messages.</param>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="logOwner" /> is <see langword="null" />.
		/// </exception>
		public MenuLogger(Type logOwner) : base(logOwner, typeof(Log)) { }
	}
}
