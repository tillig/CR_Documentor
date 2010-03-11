using System;

namespace CR_Documentor.Options
{
	/// <summary>
	/// The possible ways to handle unrecognized tags.
	/// </summary>
	public enum UnrecognizedTagHandlingMethod
	{
		/// <summary>
		/// Hide the tag and its contents.
		/// </summary>
		HideTagAndContents,

		/// <summary>
		/// Strip the tag, but show the contents.
		/// </summary>
		StripTagShowContents,

		/// <summary>
		/// Highlight the tag and its contents.
		/// </summary>
		HighlightTagAndContents,

		/// <summary>
		/// Render the contents anyway.
		/// </summary>
		RenderContents
	}
}
