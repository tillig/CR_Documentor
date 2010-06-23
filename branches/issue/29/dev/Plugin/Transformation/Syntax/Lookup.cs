using System;
using System.Collections.Specialized;
using CR_Documentor.Collections;
using CR_Documentor.Transformation.Syntax;
using SP = DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Provides intelligent data mapping and lookup services for syntax rendering engines.
	/// </summary>
	public static class Lookup
	{
		#region Lookup Variables

		#region Statics

		/// <summary>
		/// Contains the lookup values for various operator method names.
		/// </summary>
		private static readonly StringDictionary _operatorNameLookup;

		/// <summary>
		/// Display name of the language based on the syntax constants.
		/// </summary>
		public static readonly DefaultValueStringDictionary LanguageName;

		/// <summary>
		/// The value under the HTML "select" element in the banner that corresponds
		/// to the DXCore document language.
		/// </summary>
		public static readonly DefaultValueStringDictionary LanguageValue;

		#endregion

		#endregion



		#region Lookup Implementation

		#region Constructors

		/// <summary>
		/// Initializes <see langword="static" /> values of the <see cref="CR_Documentor.Transformation.Syntax.Lookup" /> class.
		/// </summary>
		static Lookup()
		{
			// Define standard operator translations
			_operatorNameLookup = new StringDictionary();
			_operatorNameLookup.Add("--", "Decrement");
			_operatorNameLookup.Add("++", "Increment");
			_operatorNameLookup.Add("!", "Logical Not");
			_operatorNameLookup.Add("true", "True");
			_operatorNameLookup.Add("false", "False");
			_operatorNameLookup.Add("~", "Ones Complement");
			_operatorNameLookup.Add("*", "Multiplication");
			_operatorNameLookup.Add("/", "Division");
			_operatorNameLookup.Add("%", "Modulus");
			_operatorNameLookup.Add("^", "Exclusive Or");
			_operatorNameLookup.Add("|", "Bitwise Or");
			_operatorNameLookup.Add("&&", "Logical And");
			_operatorNameLookup.Add("||", "Logical Or");
			_operatorNameLookup.Add("=", "Assignment");
			_operatorNameLookup.Add("<<", "Left Shift");
			_operatorNameLookup.Add(">>", "Right Shift");
			_operatorNameLookup.Add("==", "Equality");
			_operatorNameLookup.Add(">", "Greater Than");
			_operatorNameLookup.Add("<", "Less Than");
			_operatorNameLookup.Add("!=", "Inequality");
			_operatorNameLookup.Add(">=", "Greater Than Or Equal");
			_operatorNameLookup.Add("<=", "Less Than Or Equal");
			_operatorNameLookup.Add(".", "Member Selection");
			_operatorNameLookup.Add(">>=", "Right Shift Assignment");
			_operatorNameLookup.Add("*=", "Multiplication Assignment");
			_operatorNameLookup.Add("->", "Pointer To Member Selection");
			_operatorNameLookup.Add("-=", "Subtraction Assignment");
			_operatorNameLookup.Add("^=", "Exclusive Or Assignment");
			_operatorNameLookup.Add("<<=", "Left Shift Assignment");
			_operatorNameLookup.Add("%=", "Modulus Assignment");
			_operatorNameLookup.Add("+=", "Addition Assignment");
			_operatorNameLookup.Add("&=", "Bitwise And Assignment");
			_operatorNameLookup.Add("|=", "Bitwise Or Assignment");
			_operatorNameLookup.Add(",", "Comma");
			_operatorNameLookup.Add("/=", "Division Assignment");

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

		#endregion

		#region Methods

		/// <summary>
		/// Resolves the member contract (new/static/abstract/etc.).
		/// </summary>
		/// <param name="element">
		/// The member to determine the contract for.
		/// </param>
		/// <returns>
		/// A language-specific member contract based on the element language.
		/// </returns>
		public static string Contract(SP.AccessSpecifiedElement element)
		{
			string retVal = "";
			string lang = element.Document.Language;
			if (element.IsNew)
			{
				retVal = Keyword.New[lang];
			}

			string memberKW = "";
			if (element.IsStatic)
			{
				memberKW = Keyword.StaticMember[lang];
			}
			else if (element.IsAbstract)
			{
				memberKW = Keyword.AbstractMember[lang];
			}
			else if (element.IsVirtual)
			{
				memberKW = Keyword.Virtual[lang];
			}
			else if (element.IsOverride)
			{
				memberKW = Keyword.Override[lang];
			}

			if (memberKW != "")
			{
				if (retVal != "")
				{
					retVal += " " + memberKW;
				}
				else
				{
					retVal = memberKW;
				}
			}
			return retVal;
		}

		/// <summary>
		/// Resolves the member contract (new/static/abstract/etc.).
		/// </summary>
		/// <param name="documentLanguage">The language to resolve the contract for.</param>
		/// <param name="access">The accessibility of the member.</param>
		/// <returns>
		/// A language-specific member contract based on the element language.
		/// </returns>
		public static string Contract(string documentLanguage, SP.AccessSpecifiers access)
		{
			string retVal = "";
			if (access.IsNew)
			{
				retVal = Keyword.New[documentLanguage];
			}

			string memberKW = "";
			if (access.IsStatic)
			{
				memberKW = Keyword.StaticMember[documentLanguage];
			}
			else if (access.IsAbstract)
			{
				memberKW = Keyword.AbstractMember[documentLanguage];
			}
			else if (access.IsVirtual)
			{
				memberKW = Keyword.Virtual[documentLanguage];
			}
			else if (access.IsOverride)
			{
				memberKW = Keyword.Override[documentLanguage];
			}
			else if (access.IsReadOnly)
			{
				memberKW = Keyword.ReadOnly[documentLanguage];
			}
			else if (access.IsWriteOnly)
			{
				memberKW = Keyword.WriteOnly[documentLanguage];
			}

			if (memberKW != "")
			{
				if (retVal != "")
				{
					retVal += " " + memberKW;
				}
				else
				{
					retVal = memberKW;
				}
			}
			return retVal;
		}

		/// <summary>
		/// Resolves the name of a given method.
		/// </summary>
		/// <param name="method">
		/// The method to determine the name of.
		/// </param>
		/// <returns>
		/// If this is an operator, returns the long name of the operator.  Otherwise,
		/// returns the name of the method directly.
		/// </returns>
		public static string MethodName(SP.Method method)
		{
			// If there wasn't logic to determine the lookup based on
			// parameter count, we could make this a hash table...
			if (method.IsClassOperator)
			{
				// Implicit/Explicit Cast
				if (method.IsImplicitCast || method.IsExplicitCast)
				{
					return String.Format("{0} to {1} Conversion",
						((SP.Param)method.Parameters[0]).ParamType,
						method.MemberType);
				}

				// Standard operator lookup
				if (_operatorNameLookup.ContainsKey(method.Name))
				{
					return _operatorNameLookup[method.Name];
				}

				// Special operators (could be unary or binary)
				switch (method.Name)
				{
					case "-":
						if (method.ParameterCount == 1)
						{
							return "Unary Negation";
						}
						else
						{
							return "Subtraction";
						}
					case "+":
						if (method.ParameterCount == 1)
						{
							return "Unary Plus";
						}
						else
						{
							return "Addition";
						}
					case "&":
						if (method.ParameterCount == 1)
						{
							return "Address Of";
						}
						else
						{
							return "Bitwise And";
						}
					default:
						return method.Name;
				}
			}
			else if (method.Document.Language == Language.Basic && method.IsConstructor)
			{
				return method.Parent.Name;
			}
			else
			{
				return method.Name;
			}
		}

		/// <summary>
		/// Resolves the language-specific keyword that defines the element.
		/// </summary>
		/// <param name="element">
		/// The member to determine the keyword for.
		/// </param>
		/// <returns>
		/// A language-specific keyword for the given element.
		/// </returns>
		public static string ElementType(SP.AccessSpecifiedElement element)
		{
			string lang = element.Document.Language;
			if (element is SP.Struct)
			{
				switch (lang)
				{
					case Language.Basic:
						return "Structure";
					default:
						return "struct";
				}
			}
			else if (element is SP.Interface)
			{
				switch (lang)
				{
					case Language.Basic:
						return "Interface";
					default:
						return "interface";
				}
			}
			else if (element is SP.Class)
			{
				switch (lang)
				{
					case Language.Basic:
						return "Class";
					default:
						return "class";
				}
			}
			else if (element is SP.Enumeration)
			{
				switch (lang)
				{
					case Language.Basic:
						return "Enum";
					default:
						return "enum";
				}
			}
			else if (element is SP.DelegateDefinition)
			{
				switch (lang)
				{
					case Language.Basic:
						string basicVal = "Delegate ";
						if (TypeInfo.TypeIsVoid(((SP.DelegateDefinition)element).MemberType))
						{
							basicVal += "Sub";
						}
						else
						{
							basicVal += "Function";
						}
						return basicVal;
					default:
						return "delegate";
				}
			}
			else if (element is SP.Event)
			{
				switch (lang)
				{
					case Language.Basic:
						return "Event";
					default:
						return "event";
				}
			}
			else if (element is SP.Const)
			{
				switch (lang)
				{
					case Language.Basic:
						return "Const";
					default:
						return "const";
				}
			}
			else if (element is SP.BaseVariable && element.IsReadOnly)
			{
				switch (lang)
				{
					case Language.Basic:
						return "ReadOnly";
					default:
						return "readonly";
				}
			}
			else if (element is SP.Method && ((SP.Method)element).IsClassOperator)
			{
				switch (lang)
				{
					case Language.Basic:
						return "Operator";
					default:
						return "operator";
				}
			}
			return "";
		}

		/// <summary>
		/// Determines the descriptive type of the provided element.
		/// </summary>
		/// <param name="element">The element to look up the type of.</param>
		/// <returns>A simple string that can be used in the banner for the type, describing what it is in plain terms.</returns>
		public static string ElementTypeDescription(SP.AccessSpecifiedElement element)
		{
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
		/// Resolves the visibility of the specified element.
		/// </summary>
		/// <param name="element">
		/// The member to determine the visibility of.
		/// </param>
		/// <returns>
		/// A language-specific string that contains the visibility of the provided member.
		/// </returns>
		public static string Visibility(SP.AccessSpecifiedElement element)
		{
			return Visibility(element.Document.Language, element.Visibility);
		}

		/// <summary>
		/// Resolves the language-specific visibility keyword.
		/// </summary>
		/// <param name="documentLanguage">The document language to return the value in.</param>
		/// <param name="visibility">The visibility to look up.</param>
		/// <returns>
		/// A language-specific string that contains the visibility information.
		/// </returns>
		public static string Visibility(string documentLanguage, SP.MemberVisibility visibility)
		{
			if (documentLanguage == Language.Basic)
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

		#endregion

		#endregion

	}
}
