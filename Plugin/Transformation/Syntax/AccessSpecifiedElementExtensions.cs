using System;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Transformation.Syntax
{
	/// <summary>
	/// Extension methods for <see cref="DevExpress.CodeRush.StructuralParser.AccessSpecifiedElement"/>.
	/// </summary>
	public static class AccessSpecifiedElementExtensions
	{
		/// <summary>
		/// Determines the descriptive type of the provided element.
		/// </summary>
		/// <param name="element">The element to look up the type of.</param>
		/// <returns>A simple string that can be used in the banner for the type, describing what it is in plain terms.</returns>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="element" /> is <see langword="null" />.
		/// </exception>
		public static string ElementTypeDescription(this AccessSpecifiedElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (element is BaseVariable || element is EnumElement)
			{
				return "Field";
			}
			else if (element is DelegateDefinition)
			{
				return "Delegate";
			}
			else if (element is Event)
			{
				return "Event";
			}
			else if (element is Method)
			{
				if (((Method)element).IsClassOperator)
				{
					return "Operator";
				}
				else if (((Method)element).IsConstructor)
				{
					return "Constructor";
				}
				else
				{
					return "Method";
				}
			}
			else if (element is Property)
			{
				return "Property";
			}
			else if (element is Class)
			{
				if (element is Interface)
				{
					return "Interface";
				}
				else if (element is Struct)
				{
					return "Structure";
				}
				else
				{
					return "Class";
				}
			}
			else if (element is Enumeration)
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
		public static bool HasGenericParameters(this AccessSpecifiedElement element)
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
		/// Determines if a language element is supported for preview.
		/// </summary>
		/// <param name="element">
		/// The element to check for compatibility.
		/// </param>
		/// <returns>
		/// <see langword="true" /> if a syntax preview can be generated
		/// for the element, otherwise <see langword="false" />.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="element" /> is <see langword="null" />.
		/// </exception>
		public static bool IsSupportedForPreview(this AccessSpecifiedElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
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
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="element" /> is <see langword="null" />.
		/// </exception>
		public static string MemberType(this AccessSpecifiedElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			Member member = element as Member;
			if (member == null)
			{
				return null;
			}
			return member.MemberType;
		}
	}
}
