using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Extension methods for <see cref="DevExpress.CodeRush.StructuralParser.MemberVisibility"/>.
	/// </summary>
	public static class MemberVisibilityExtensions
	{
		/// <summary>
		/// Resolves the language-specific visibility keyword.
		/// </summary>
		/// <param name="visibility">The visibility to look up.</param>
		/// <param name="language">The document language to return the value in.</param>
		/// <returns>
		/// A language-specific string that contains the visibility information.
		/// </returns>
		public static string ForLanguage(this MemberVisibility visibility, SupportedLanguageId language)
		{
			if (language == SupportedLanguageId.Basic)
			{
				switch (visibility)
				{
					case MemberVisibility.Public:
						return "Public";
					case MemberVisibility.Private:
						return "Private";
					case MemberVisibility.Protected:
						return "Protected";
					case MemberVisibility.ProtectedFriend:
					case MemberVisibility.ProtectedInternal:
						return "Protected Friend";
					case MemberVisibility.Internal:
						return "Friend";
					default:
						return "/* unknown */";
				}
			}
			else
			{
				switch (visibility)
				{
					case MemberVisibility.Public:
						return "public";
					case MemberVisibility.Private:
						return "private";
					case MemberVisibility.Protected:
						return "protected";
					case MemberVisibility.ProtectedFriend:
					case MemberVisibility.ProtectedInternal:
						return "protected internal";
					case MemberVisibility.Internal:
						return "internal";
					default:
						return "/* unknown */";
				}
			}
		}
	}
}
