using System;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Extension methods for <see cref="DevExpress.CodeRush.StructuralParser.AccessSpecifiedElement"/>.
	/// </summary>
	public static class AccessSpecifiedElementExtensions
	{
		// TODO: When we target .NET 3.5, convert these to real extension methods.

		/// <summary>
		/// Gets a flag indicating if there are generic type parameters
		/// to render to the preview.
		/// </summary>
		/// <param name="element">
		/// The element to check for generic type parameters.
		/// </param>
		/// <returns>
		/// <see langword="true" /> if <paramref name="element" />
		/// has generic type parameters to render; <see langword="false" /> if not.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="element" /> is <see langword="null" />.
		/// </exception>
		public static bool HasGenericParameters(AccessSpecifiedElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (!element.IsGeneric || element.GenericModifier.TypeParameters == null || element.GenericModifier.TypeParameters.Count == 0)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Determines if a langauge element is supported for preview.
		/// </summary>
		/// <param name="element">
		/// The element to check for compatibility.
		/// </param>
		/// <returns>
		/// <see langword="true" /> if a syntax preview can be generated
		/// for the element, otherwise <see langword="false" />.
		/// </returns>
		public static bool IsSupportedForPreview(AccessSpecifiedElement element)
		{
			return
				element is Enumeration ||
				element is Class ||
				element is DelegateDefinition ||
				element is Method ||
				element is Property ||
				element is Event ||
				element is BaseVariable;
		}

		/// <summary>
		/// Gets the member type of the current element, assuming the current element
		/// is a <see cref="DevExpress.CodeRush.StructuralParser.Member"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> containing the <see cref="DevExpress.CodeRush.StructuralParser.Member.MemberType"/>
		/// of the <paramref name="element" />.
		/// If <paramref name="element" />
		/// is not a <see cref="DevExpress.CodeRush.StructuralParser.Member"/>,
		/// this method returns <see langword="null" />.
		/// </returns>
		public static string MemberType(AccessSpecifiedElement element)
		{
			Member member = element as Member;
			if (member == null)
			{
				return null;
			}
			return member.MemberType;
		}
	}
}
