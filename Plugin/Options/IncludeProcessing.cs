using System;

namespace CR_Documentor.Options
{
	/// <summary>
	/// Indicates how included documentation should be processed.
	/// </summary>
	public enum IncludeProcessing
	{
		/// <summary>
		/// Don't process includes.
		/// </summary>
		None,

		/// <summary>
		/// Process only absolute file paths for includes.
		/// </summary>
		Absolute,

		/// <summary>
		/// Process absolute and relative file paths for includes.
		/// </summary>
		Relative
	}
}
