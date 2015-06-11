using System;

namespace DocTestCS.Tags.Inline
{
	/// <summary>
	/// This class tests the 'paramref' tag.
	/// </summary>
	public class Paramref
	{
		/// <summary>
		/// This summary references the <paramref name="obj" /> parameter.
		/// </summary>
		/// <param name="obj">The parameter for this method.</param>
		public void Method(object obj)
		{
		}
	}
}
