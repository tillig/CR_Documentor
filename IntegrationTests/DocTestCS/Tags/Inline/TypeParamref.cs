using System;
using System.Collections.Generic;

namespace DocTestCS.Tags.Inline
{
	/// <summary>
	/// This class tests the 'typeparamref' tag.
	/// </summary>
	public class TypeParamref
	{
		/// <summary>
		/// Method with a type parameter and a regular parameter.
		/// </summary>
		/// <param name="length">The length of the array to make.</param>
		/// <typeparam name="T">The element <see cref="System.Type"/>.</typeparam>
		/// <returns>An array of <typeparamref name="T"/> of the length provided.</returns>
		public T[] MixedParams<T>(int length)
		{
			return new T[length];
		}

		/// <summary>
		/// Method with a single type parameter.
		/// </summary>
		/// <typeparam name="T">The element <see cref="System.Type"/>.</typeparam>
		/// <returns>A zero-length array of <typeparamref name="T"/>.</returns>
		public T[] OneTypeParam<T>()
		{
			return new T[0];
		}

		/// <summary>
		/// Method with two type parameters
		/// </summary>
		/// <typeparam name="K">The key <see cref="System.Type"/>.</typeparam>
		/// <typeparam name="V">The value <see cref="System.Type"/>.</typeparam>
		/// <returns>A dictionary using the <typeparamref name="K"/>/<typeparamref name="V"/> types.</returns>
		public Dictionary<K, V> TwoTypeParams<K, V>()
		{
			return new Dictionary<K, V>();
		}
	}
}
