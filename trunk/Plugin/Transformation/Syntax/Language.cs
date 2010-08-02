using System;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Contains language identifiers used in syntax preview generation.
	/// </summary>
	public static class Language
	{
		/// <summary>
		/// Language identifier: C#
		/// </summary>
		public const string CSharp = "CSharp";

		/// <summary>
		/// Language identifier: Visual Basic
		/// </summary>
		public const string Basic = "Basic";


		/// <summary>
		/// Converts a string language ID into a supported language identifier.
		/// </summary>
		/// <param name="language">
		/// The <see cref="System.String"/> with the document language.
		/// </param>
		/// <returns>
		/// An appropriately corresponding <see cref="CR_Documentor.Transformation.Syntax.SupportedLanguageId"/>.
		/// </returns>
		public static SupportedLanguageId ConvertToSupportedLanguageId(string language)
		{
			switch (language)
			{
				case Language.CSharp:
					return SupportedLanguageId.CSharp;
				case Language.Basic:
					return SupportedLanguageId.Basic;
				default:
					return SupportedLanguageId.None;
			}
		}
	}
}
