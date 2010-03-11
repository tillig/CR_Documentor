using System;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Service class for working with type names and other type information when
	/// rendering syntax previews.
	/// </summary>
	public static class TypeInfo
	{
		/// <summary>
		/// Determines if a string type name equates to "void."
		/// </summary>
		/// <param name="typename">The type to check.</param>
		/// <returns><see langword="true" /> if the type is void, <see langword="false" /> otherwise.</returns>
		public static bool TypeIsVoid(string typename)
		{
			return (String.IsNullOrEmpty(typename) || String.Compare(typename, "void", true) == 0 || String.Compare(typename, "System.Void", true) == 0);
		}
	}
}
