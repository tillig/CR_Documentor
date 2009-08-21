using System;
using System.Collections.Generic;

namespace DocTest.Tags.TopLevel
{
	/// <summary>
	/// This class tests the 'typeparam' tag.
	/// </summary>
	public class TypeParam
	{
		/// <summary>
		/// Method with a type parameter and a regular parameter.
		/// </summary>
		/// <param name="length">The length of the array to make.</param>
		/// <typeparam name="T">The element <see cref="System.Type"/>.</typeparam>
		/// <returns>An array of the specified <see cref="System.Type"/> of the length provided.</returns>
		public T[] MixedParams<T>(int length)
		{
			return new T[length];
		}

		/// <summary>
		/// Method with a single type parameter.
		/// </summary>
		/// <typeparam name="T">The element <see cref="System.Type"/>.</typeparam>
		/// <returns>A zero-length array of the type provided.</returns>
		public T[] OneTypeParam<T>()
		{
			return new T[0];
		}

		/// <summary>
		/// Method with two type parameters
		/// </summary>
		/// <typeparam name="K">The key <see cref="System.Type"/>.</typeparam>
		/// <typeparam name="V">The value <see cref="System.Type"/>.</typeparam>
		/// <returns>A dictionary using the key/value types.</returns>
		public Dictionary<K, V> TwoTypeParams<K, V>()
		{
			return new Dictionary<K, V>();
		}
	}
}
