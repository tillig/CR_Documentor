using System;
using SP = DevExpress.CodeRush.StructuralParser;
using System.Collections.Generic;

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
		public static readonly Dictionary<string, string> LanguageName = new Dictionary<string, string>()
		{
			{Language.Basic, "Visual Basic"},
			{Language.CSharp, "C#"}
		};

		/// <summary>
		/// The value under the HTML "select" element in the banner that corresponds
		/// to the DXCore document language.
		/// </summary>
		public static readonly Dictionary<string, string> LanguageValue = new Dictionary<string, string>()
		{
			{Language.Basic, "VisualBasic vb"},
			{Language.CSharp, "CSharp cs"}
		};
	}
}
