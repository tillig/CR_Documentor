using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DocTestCS.Syntax
{
	/// <summary>
	/// Class used in testing syntax generation for generics. Class definition
	/// has a type constraint on the <typeparamref name="T"/> parameter.
	/// </summary>
	/// <typeparam name="T">Arbitrary type parameter.</typeparam>
	public class TestGenerics<T> where T : Attribute, ICloneable
	{
		/// <summary>
		/// Method that has a generic in the parameter list that has two generic types associated.
		/// </summary>
		/// <param name="param1">First parameter. Should be a <see cref="Dictionary{K, V}"/>.</param>
		public void MethodWithGenericParameterDictionary(Dictionary<string, bool> param1)
		{
		}

		/// <summary>
		/// Method that returns a generic type and has a generic in the parameter list.
		/// </summary>
		/// <param name="param1">First parameter. Should be a <see cref="IEnumerable{T}"/>.</param>
		/// <returns>Says it returns a <see cref="List{T}"/>, but always returns <see langword="null" />.</returns>
		public List<string> MethodWithGenericReturnAndParameter(IEnumerable<string> param1)
		{
			return null;
		}

		/// <summary>
		/// Method that has a type parameter.
		/// </summary>
		/// <typeparam name="P">The method's type parameter.</typeparam>
		public void MethodWithTypeParameter<P>()
		{
		}

		/// <summary>
		/// Method with multiple type parameters, each with constraints.
		/// </summary>
		/// <typeparam name="H">The method's type parameter.</typeparam>
		/// <typeparam name="I">The method's type parameter.</typeparam>
		/// <typeparam name="J">The method's type parameter.</typeparam>
		public void MultipleTypeParametersWithConstraints<H, I, J>()
			where H : Attribute, ISerializable
			where I : struct
			where J : class, IList<H>, ICollection<H>, new()
		{
		}
	}
}
