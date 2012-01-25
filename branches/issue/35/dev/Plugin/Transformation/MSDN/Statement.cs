using System;
using CR_Documentor.Collections;
using CR_Documentor.Transformation.Syntax;

namespace CR_Documentor.Transformation.MSDN
{
	/// <summary>
	/// Provides statement punctuation/formatting lookup facilities for the MSDN rendering engine.
	/// </summary>
	public static class Statement
	{
		/// <summary>
		/// Language-specific closer for a list of generic parameters.
		/// </summary>
		public static readonly DefaultValueStringDictionary TypeParamListCloser;

		/// <summary>
		/// Language-specific opener for a list of generic parameters.
		/// </summary>
		public static readonly DefaultValueStringDictionary TypeParamListOpener;

		/// <summary>
		/// Initializes <see langword="static" /> values of the <see cref="CR_Documentor.Transformation.MSDN.Statement" /> class.
		/// </summary>
		static Statement()
		{
			// Typeparam list closer
			TypeParamListCloser = new DefaultValueStringDictionary();
			TypeParamListCloser.Add(Language.Basic, ")");
			TypeParamListCloser.DefaultValue = ">";

			// Typeparam list opener
			TypeParamListOpener = new DefaultValueStringDictionary();
			TypeParamListOpener.Add(Language.Basic, "(Of ");
			TypeParamListOpener.DefaultValue = "<";
		}
	}
}
