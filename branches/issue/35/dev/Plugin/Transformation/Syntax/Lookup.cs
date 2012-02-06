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

		/// <summary>
		/// Determines the descriptive type of the provided element.
		/// </summary>
		/// <param name="element">The element to look up the type of.</param>
		/// <returns>A simple string that can be used in the banner for the type, describing what it is in plain terms.</returns>
		public static string ElementTypeDescription(SP.AccessSpecifiedElement element)
		{
			// TODO: Refactor ElementTypeDescription into an extension method on AccessSpecifiedElement.
			if (element is SP.BaseVariable || element is SP.EnumElement)
			{
				return "Field";
			}
			else if (element is SP.DelegateDefinition)
			{
				return "Delegate";
			}
			else if (element is SP.Event)
			{
				return "Event";
			}
			else if (element is SP.Method)
			{
				if (((SP.Method)element).IsClassOperator)
				{
					return "Operator";
				}
				else if (((SP.Method)element).IsConstructor)
				{
					return "Constructor";
				}
				else
				{
					return "Method";
				}
			}
			else if (element is SP.Property)
			{
				return "Property";
			}
			else if (element is SP.Class)
			{
				if (element is SP.Interface)
				{
					return "Interface";
				}
				else if (element is SP.Struct)
				{
					return "Structure";
				}
				else
				{
					return "Class";
				}
			}
			else if (element is SP.Enumeration)
			{
				return "Enumeration";
			}
			else
			{
				// If all else fails...
				return "Member";
			}
		}

		/// <summary>
		/// Resolves the language-specific visibility keyword.
		/// </summary>
		/// <param name="language">The document language to return the value in.</param>
		/// <param name="visibility">The visibility to look up.</param>
		/// <returns>
		/// A language-specific string that contains the visibility information.
		/// </returns>
		public static string Visibility(SupportedLanguageId language, SP.MemberVisibility visibility)
		{
			// TODO: Refactor Visibility into an extension method on MemberVisibility.
			if (language == SupportedLanguageId.Basic)
			{
				switch (visibility)
				{
					case SP.MemberVisibility.Public:
						return "Public";
					case SP.MemberVisibility.Private:
						return "Private";
					case SP.MemberVisibility.Protected:
						return "Protected";
					case SP.MemberVisibility.ProtectedInternal:
						return "Protected Friend";
					case SP.MemberVisibility.Internal:
						return "Friend";
					default:
						return "/* unknown */";
				}
			}
			else
			{
				switch (visibility)
				{
					case SP.MemberVisibility.Public:
						return "public";
					case SP.MemberVisibility.Private:
						return "private";
					case SP.MemberVisibility.Protected:
						return "protected";
					case SP.MemberVisibility.ProtectedInternal:
						return "protected internal";
					case SP.MemberVisibility.Internal:
						return "internal";
					default:
						return "/* unknown */";
				}
			}
		}
	}
}
