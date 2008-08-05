using System;

namespace DocTest.Tags.TopLevel
{
	/// <summary>
	/// This class tests the 'remarks' tag.
	/// </summary>
	/// <remarks>
	/// This is some remarks text not inside a paragraph.
	/// </remarks>
	public class RemarksClass
	{
		/// <summary>
		/// This is a method with a remarks.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This is some remarks text - paragraph 1.
		/// </para>
		/// <para>
		/// This is some remarks text - paragraph 2.
		/// </para>
		/// </remarks>
		public void Method()
		{
		}
	}
}
