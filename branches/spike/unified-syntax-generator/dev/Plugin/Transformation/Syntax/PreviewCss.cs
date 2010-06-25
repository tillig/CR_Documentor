using System;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Contains constants for the CSS classes applied to syntax previews.
	/// </summary>
	public static class PreviewCss
	{
		/// <summary>
		/// Surrounds an individual attribute on a member.
		/// </summary>
		public const string Attribute = "attribute";

		/// <summary>
		/// Surrounds the complete set of attributes on a member.
		/// </summary>
		public const string Attributes = "attributes";

		/// <summary>
		/// Surrounds the entirety of rendered code.
		/// </summary>
		public const string Code = "code";

		/// <summary>
		/// Surrounds a comment in a preview.
		/// </summary>
		public const string Comment = "comment";

		/// <summary>
		/// Surrounds an individual type parameter constraint on a member.
		/// </summary>
		public const string Constraint = "constraint";

		/// <summary>
		/// Surrounds the complete set of type parameter constraints on a member.
		/// </summary>
		public const string Constraints = "constraints";

		/// <summary>
		/// Surrounds the member identifier.
		/// </summary>
		public const string Identifier = "identifier";

		/// <summary>
		/// Surrounds keywords in the preview.
		/// </summary>
		public const string Keyword = "keyword";

		/// <summary>
		/// Marks a code block in Visual Basic.
		/// </summary>
		public const string Language_Basic = "vb";

		/// <summary>
		/// Marks a code block in C#.
		/// </summary>
		public const string Language_CSharp = "cs";

		/// <summary>
		/// Surrounds literal values in a preview.
		/// </summary>
		public const string Literal = "literal";

		/// <summary>
		/// Surrounds the member in the preview (after attributes).
		/// </summary>
		public const string Member = "member";

		/// <summary>
		/// Surrounds an individual parameter in a list.
		/// </summary>
		public const string Parameter = "parameter";

		/// <summary>
		/// Surrounds the entire parameter list.
		/// </summary>
		public const string Parameters = "parameters";

		/// <summary>
		/// Surrounds an individual type parameter in a list.
		/// </summary>
		public const string TypeParameter = "typeparameter";

		/// <summary>
		/// Surrounds the entire type parameter list.
		/// </summary>
		public const string TypeParameters = "typeparameters";
	}
}
