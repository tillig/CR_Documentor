using System;
using CR_Documentor.Collections;
using SP = DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Provides intelligent data mapping and lookup services for syntax rendering engines.
	/// </summary>
	public static class Lookup
	{
		/// <summary>
		/// Display name of the language based on the syntax constants.
		/// </summary>
		public static readonly DefaultValueStringDictionary LanguageName;

		/// <summary>
		/// The value under the HTML "select" element in the banner that corresponds
		/// to the DXCore document language.
		/// </summary>
		public static readonly DefaultValueStringDictionary LanguageValue;

		/// <summary>
		/// Initializes <see langword="static" /> values of the <see cref="CR_Documentor.Transformation.Syntax.Lookup" /> class.
		/// </summary>
		static Lookup()
		{
			// Language display names
			LanguageName = new DefaultValueStringDictionary();
			LanguageName.Add(Language.Basic, "Visual Basic");
			LanguageName.Add(Language.CSharp, "C#");
			LanguageName.DefaultValue = "--";

			// Language select values
			LanguageValue = new DefaultValueStringDictionary();
			LanguageValue.Add(Language.Basic, "VisualBasic vb");
			LanguageValue.Add(Language.CSharp, "CSharp cs");
			LanguageValue.DefaultValue = "x";
		}
	}
}
