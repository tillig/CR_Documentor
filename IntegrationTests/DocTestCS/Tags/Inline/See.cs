using System;

namespace DocTestCS.Tags.Inline
{
	/// <summary>
	/// This class tests the 'see' tag: <see cref="System.String" />.
	/// </summary>
	public class SeeClass
	{
		/// <summary>
		/// This is a method that tests the see langword functionality.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item><term>null:  <see langword="null"/></term></item>
		/// <item><term>sealed:  <see langword="sealed"/></term></item>
		/// <item><term>static:  <see langword="static"/></term></item>
		/// <item><term>abstract:  <see langword="abstract"/></term></item>
		/// <item><term>virtual:  <see langword="virtual"/></term></item>
		/// <item><term>true:  <see langword="true"/></term></item>
		/// <item><term>false:  <see langword="false"/></term></item>
		/// </list>
		/// </remarks>
		public void Langword()
		{
		}

		/// <summary>
		/// This is a method with a see that has a cref: <see cref="System.String" />.
		/// </summary>
		public void MethodCref()
		{
		}

		/// <summary>
		/// This is a method with a see that has a cref to a generic type: <see cref="System.Collections.Generic.Dictionary{TKey, TValue}" />.
		/// </summary>
		public void MethodCrefGeneric()
		{
		}

		/// <summary>
		/// This is a method with a see that has a cref and label text: <see cref="System.String">See the String class</see>.
		/// </summary>
		public void MethodCrefLabel()
		{
		}

		/// <summary>
		/// This is a method with a see that has only an href: <see href="http://www.microsoft.com" />.
		/// </summary>
		public void MethodHref()
		{
		}

		/// <summary>
		/// This is a method with a see that has an href and label text: <see href="http://www.microsoft.com">See the Microsoft web site</see>.
		/// </summary>
		public void MethodHrefLabel()
		{
		}

		/// <summary>
		/// This is a method with a see that uses langword 'abstract': <see langword="abstract" />.
		/// </summary>
		public void MethodLangwordAbstract()
		{
		}

		/// <summary>
		/// This is a method with a see that uses langword 'null': <see langword="null" />.
		/// </summary>
		public void MethodLangwordNull()
		{
		}

		/// <summary>
		/// This is a method with a see that uses langword 'sealed': <see langword="sealed" />.
		/// </summary>
		public void MethodLangwordSealed()
		{
		}

		/// <summary>
		/// This is a method with a see that uses langword 'static': <see langword="static" />.
		/// </summary>
		public void MethodLangwordStatic()
		{
		}

		/// <summary>
		/// This is a method with a see that uses langword 'virtual': <see langword="virtual" />.
		/// </summary>
		public void MethodLangwordVirtual()
		{
		}

		/// <summary>
		/// This is a method with all types of see:
		/// <see cref="System.String" />;
		/// <see cref="System.Int32">See the Int32 class</see>;
		/// <see href="http://www.corillian.com" />;
		/// <see href="http://www.microsoft.com">See the Microsoft web site</see>;
		/// <see langword="abstract" />;
		/// <see langword="null" />;
		/// <see langword="sealed" />;
		/// <see langword="static" />;
		/// <see langword="virtual" />
		/// </summary>
		public void MethodMixed()
		{
		}

		/// <summary>
		/// This method has three string references, only the first of which should
		/// be linked: <see cref="System.String" />; <see cref="System.String" />; <see cref="System.String" />.
		/// </summary>
		public void MethodMultipleReferences()
		{
		}
	}
}
