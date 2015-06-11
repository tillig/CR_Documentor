using System;
using System.Reflection;

namespace CR_Documentor
{
	/// <summary>
	/// Extension methods for <see cref="System.Reflection.ICustomAttributeProvider"/>.
	/// </summary>
	public static class ICustomAttributeProviderExtensions
	{
		/// <summary>
		/// Retrieves a single custom attribute from a type implementing <see cref="System.Reflection.ICustomAttributeProvider"/>.
		/// Does not search the inheritance hierarchy.
		/// </summary>
		/// <typeparam name="T">The type of <see cref="System.Attribute"/> to retrieve.</typeparam>
		/// <param name="provider">The object from which the attribute should be retrieved.</param>
		/// <returns>
		/// The attribute of type <typeparamref name="T"/> if found, or <see langword="null" /> if not.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// Thrown if <paramref name="provider" /> is <see langword="null" />.
		/// </exception>
		public static T GetCustomAttribute<T>(this ICustomAttributeProvider provider) where T : Attribute
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			T[] attribs = (T[])provider.GetCustomAttributes(typeof(T), false);
			if (attribs.Length > 0)
			{
				return attribs[0];
			}
			return null;
		}
	}
}
