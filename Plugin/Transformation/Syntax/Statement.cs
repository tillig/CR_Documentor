using System;
using CR_Documentor.Collections;
using CR_Documentor.Transformation.Syntax;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Provides statement punctuation/formatting lookup facilities for the MSDN rendering engine.
	/// </summary>
	public static class Statement
	{
		/// <summary>
		/// Language-specific attribute open characters.
		/// </summary>
		public static readonly DefaultValueStringDictionary AttributeClose;

		/// <summary>
		/// Language-specific attribute open characters.
		/// </summary>
		public static readonly DefaultValueStringDictionary AttributeOpen;

		/// <summary>
		/// Language-specific statement continuation characters.
		/// </summary>
		public static readonly DefaultValueStringDictionary Continue;

		/// <summary>
		/// Language-specific statement ending characters.
		/// </summary>
		public static readonly DefaultValueStringDictionary End;

		/// <summary>
		/// Language-specific parameter separation characters.
		/// </summary>
		public static readonly DefaultValueStringDictionary ParamSeparator;

		/// <summary>
		/// Language-specific closer for a list of generic parameters.
		/// </summary>
		public static readonly DefaultValueStringDictionary TypeParamListCloser;

		/// <summary>
		/// Language-specific opener for a list of generic parameters.
		/// </summary>
		public static readonly DefaultValueStringDictionary TypeParamListOpener;

		/// <summary>
		/// Initializes <see langword="static" /> values of the <see cref="CR_Documentor.Transformation.Syntax.Statement" /> class.
		/// </summary>
		static Statement()
		{
			// Statement attribute open chars
			AttributeClose = new DefaultValueStringDictionary();
			AttributeClose.Add(Language.Basic, "&gt;");
			AttributeClose.DefaultValue = "]";

			// Statement attribute open chars
			AttributeOpen = new DefaultValueStringDictionary();
			AttributeOpen.Add(Language.Basic, "&lt;");
			AttributeOpen.DefaultValue = "[";

			// Statement continuation chars
			Continue = new DefaultValueStringDictionary();
			Continue.Add(Language.Basic, " _ ");

			// Statement end chars
			End = new DefaultValueStringDictionary();
			End.Add(Language.Basic, "");
			End.DefaultValue = ";";

			// Statement param separator chars
			ParamSeparator = new DefaultValueStringDictionary();
			ParamSeparator.Add(Language.Basic, "As");

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
