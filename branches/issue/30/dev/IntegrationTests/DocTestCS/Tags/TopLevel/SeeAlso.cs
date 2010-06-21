using System;

namespace DocTestCS.Tags.TopLevel
{
	/// <summary>
	/// This class tests the 'seealso' tag.
	/// </summary>
	/// <seealso cref="System.String" />
	public class SeeAlsoClass
	{
		/// <summary>
		/// This is a method with a seealso that has only a cref.
		/// </summary>
		/// <seealso cref="System.String" />
		public void MethodCref()
		{
		}

		/// <summary>
		/// This is a method with a seealso that has a cref to a generic type.
		/// </summary>
		/// <seealso cref="System.Collections.Generic.Dictionary{TKey, TValue}" />
		public void MethodCrefGeneric()
		{
		}

		/// <summary>
		/// This is a method with a seealso that has a cref and label text.
		/// </summary>
		/// <seealso cref="System.String">See the String class</seealso>
		public void MethodCrefLabel()
		{
		}

		/// <summary>
		/// This is a method with a seealso that has only an href.
		/// </summary>
		/// <seealso href="http://www.microsoft.com" />
		public void MethodHref()
		{
		}

		/// <summary>
		/// This is a method with a seealso that has an href and label text.
		/// </summary>
		/// <seealso href="http://www.microsoft.com">See the Microsoft web site</seealso>
		public void MethodHrefLabel()
		{
		}

		/// <summary>
		/// This is a method with all types of seealso.
		/// </summary>
		/// <seealso cref="System.String" />
		/// <seealso cref="System.Collections.Generic.Dictionary{TKey, TValue}" />
		/// <seealso cref="System.Int32">See the Int32 class</seealso>
		/// <seealso href="http://www.corillian.com" />
		/// <seealso href="http://www.microsoft.com">See the Microsoft web site</seealso>
		public void MethodMixed()
		{
		}
	}
}
