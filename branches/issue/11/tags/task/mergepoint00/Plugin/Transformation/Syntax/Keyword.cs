using System;
using CR_Documentor.Transformation.Syntax;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Provides keyword lookup facilities for the syntax rendering engines.
	/// </summary>
	public static class Keyword
	{
		/// <summary>
		/// Language-specific keyword translations for "abstract" when used with a class.
		/// </summary>
		public static readonly DefaultValueStringDictionary AbstractClass;

		/// <summary>
		/// Language-specific keyword translations for "abstract" when used with a member.
		/// </summary>
		public static readonly DefaultValueStringDictionary AbstractMember;

		/// <summary>
		/// Language-specific keyword translations for "constructor."
		/// </summary>
		public static readonly DefaultValueStringDictionary Constructor;

		/// <summary>
		/// Language-specific keyword translations for "destructor."
		/// </summary>
		public static readonly DefaultValueStringDictionary Destructor;

		/// <summary>
		/// Language-specific keyword translations for explicit conversion operators.
		/// </summary>
		public static readonly DefaultValueStringDictionary ExplicitConversion;

		/// <summary>
		/// Language-specific keyword translations for implicit conversion operators.
		/// </summary>
		public static readonly DefaultValueStringDictionary ImplicitConversion;

		/// <summary>
		/// Language-specific keyword translations for "new" (with respect to overriding a method that isn't marked virtual).
		/// </summary>
		public static readonly DefaultValueStringDictionary New;

		/// <summary>
		/// Language-specific keyword translations for "out" (like an "out" parameter).
		/// </summary>
		public static readonly DefaultValueStringDictionary Out;

		/// <summary>
		/// Language-specific keyword translations for "override."
		/// </summary>
		public static readonly DefaultValueStringDictionary Override;

		/// <summary>
		/// Language-specific keyword translations for "params" (like a "params" array parameter).
		/// </summary>
		public static readonly DefaultValueStringDictionary Params;

		/// <summary>
		/// Language-specific keyword translations for "property."
		/// </summary>
		public static readonly DefaultValueStringDictionary Property;

		/// <summary>
		/// Language-specific keyword translations for "ref" (like a "ref" parameter).
		/// </summary>
		public static readonly DefaultValueStringDictionary Ref;

		/// <summary>
		/// Language-specific keyword translations for readonly (like get-only property).
		/// </summary>
		public static readonly DefaultValueStringDictionary ReadOnly;

		/// <summary>
		/// Language-specific keyword translations for "sealed."
		/// </summary>
		public static readonly DefaultValueStringDictionary Sealed;

		/// <summary>
		/// Language-specific keyword translations for "static" when referring to a class.
		/// </summary>
		public static readonly DefaultValueStringDictionary StaticClass;

		/// <summary>
		/// Language-specific keyword translations for "static" when referring to a class member.
		/// </summary>
		public static readonly DefaultValueStringDictionary StaticMember;

		/// <summary>
		/// Language-specific keyword translations for "virtual."
		/// </summary>
		public static readonly DefaultValueStringDictionary Virtual;

		/// <summary>
		/// Language-specific keyword translations for writeonly (like set-only property).
		/// </summary>
		public static readonly DefaultValueStringDictionary WriteOnly;

		/// <summary>
		/// Initializes <see langword="static" /> values of the <see cref="CR_Documentor.Transformation.Syntax.Keyword" /> class.
		/// </summary>
		static Keyword()
		{
			// Abstract (class) keyword
			AbstractClass = new DefaultValueStringDictionary();
			AbstractClass.Add(Language.Basic, "MustInherit");
			AbstractClass.DefaultValue = "abstract";

			// Abstract (member) keyword
			AbstractMember = new DefaultValueStringDictionary();
			AbstractMember.Add(Language.Basic, "MustOverride");
			AbstractMember.DefaultValue = "abstract";

			// Constructor keyword
			Constructor = new DefaultValueStringDictionary();
			Constructor.Add(Language.Basic, "Sub New");

			// Destructor keyword
			Destructor = new DefaultValueStringDictionary();
			Destructor.DefaultValue = "Finalize";

			// Explicit conversion operator keyword
			ExplicitConversion = new DefaultValueStringDictionary();
			ExplicitConversion.Add(Language.Basic, "Narrowing");
			ExplicitConversion.DefaultValue = "explicit";

			// Implicit conversion operator keyword
			ImplicitConversion = new DefaultValueStringDictionary();
			ImplicitConversion.Add(Language.Basic, "Widening");
			ImplicitConversion.DefaultValue = "implicit";

			// New keyword
			New = new DefaultValueStringDictionary();
			New.Add(Language.Basic, "Shadows");
			New.Add(Language.CSharp, "new");

			// Out keyword
			Out = new DefaultValueStringDictionary();
			Out.Add(Language.Basic, "ByRef");
			Out.Add(Language.CSharp, "out");
			Out.Add(Language.C, "*");

			// Override keyword
			Override = new DefaultValueStringDictionary();
			Override.Add(Language.Basic, "Overrides");
			Override.Add(Language.CSharp, "override");

			// Params keyword
			Params = new DefaultValueStringDictionary();
			Params.Add(Language.Basic, "ParamArray");
			Params.Add(Language.CSharp, "params");

			// Property keyword
			Property = new DefaultValueStringDictionary();
			Property.Add(Language.Basic, "Property");
			Property.Add(Language.C, "property");

			// Ref keyword
			Ref = new DefaultValueStringDictionary();
			Ref.Add(Language.Basic, "ByRef");
			Ref.Add(Language.CSharp, "ref");
			Ref.Add(Language.C, "*");

			// ReadOnly keyword
			ReadOnly = new DefaultValueStringDictionary();
			ReadOnly.Add(Language.Basic, "ReadOnly");

			// Sealed keyword
			Sealed = new DefaultValueStringDictionary();
			Sealed.Add(Language.Basic, "NotInheritable");
			Sealed.DefaultValue = "sealed";

			// Static class keyword
			StaticClass = new DefaultValueStringDictionary();
			StaticClass.Add(Language.Basic, "NotInheritable");
			StaticClass.Add(Language.C, "abstract sealed");
			StaticClass.DefaultValue = "static";

			// Static member keyword
			StaticMember = new DefaultValueStringDictionary();
			StaticMember.Add(Language.Basic, "Shared");
			StaticMember.DefaultValue = "static";

			// Virtual keyword
			Virtual = new DefaultValueStringDictionary();
			Virtual.Add(Language.Basic, "Overridable");
			Virtual.DefaultValue = "virtual";

			// WriteOnly keyword
			WriteOnly = new DefaultValueStringDictionary();
			WriteOnly.Add(Language.Basic, "WriteOnly");
		}
	}
}
