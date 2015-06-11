using System;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Extension methods for working with <see cref="CR_Documentor.Transformation.Syntax.SupportedLanguageId"/>.
	/// </summary>
	public static class SupportedLanguageIdExtensions
	{
		/// <summary>
		/// Converts a string language ID to its equivalent supported language ID.
		/// </summary>
		/// <param name="language">
		/// The <see cref="System.String"/> with the language to convert.
		/// </param>
		/// <returns>
		/// The <see cref="CR_Documentor.Transformation.Syntax.SupportedLanguageId"/>
		/// corresponding to <paramref name="language" />, or
		/// <see cref="CR_Documentor.Transformation.Syntax.SupportedLanguageId.None"/>
		/// if the language is unrecognized.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="language" /> is <see langword="null" />.
		/// </exception>
		public static SupportedLanguageId ToLanguageId(this string language)
		{
			if (language == null)
			{
				throw new ArgumentNullException("language");
			}
			SupportedLanguageId result;
			if (Enum.TryParse<SupportedLanguageId>(language, true, out result))
			{
				return result;
			}
			return SupportedLanguageId.None;
		}
	}
}
