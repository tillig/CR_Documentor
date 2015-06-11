using System;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Enumerates the list of supported languages that can be used to generate
	/// a syntax preview.
	/// </summary>
	public enum SupportedLanguageId
	{
		/// <summary>
		/// No language, or a language that isn't otherwise supported.
		/// </summary>
		None,
		
		/// <summary>
		/// C#
		/// </summary>
		CSharp,
		
		/// <summary>
		/// Visual Basic
		/// </summary>
		Basic
	}
}
