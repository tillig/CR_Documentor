using System;

namespace DocTestCS.Tags.TopLevel
{
	/// <preliminary>
	/// This is class-level specified preliminary text.
	/// </preliminary>
	/// <summary>
	/// This class tests the 'preliminary' tag. The class has top-level specified
	/// preliminary text.
	/// </summary>
	public class PreliminaryClass
	{
		/// <preliminary>
		/// This is method-level specified preliminary text.
		/// </preliminary>
		/// <summary>
		/// This is a method with a preliminary that has specified text.
		/// </summary>
		public void MethodSpecifiedText()
		{
		}

		/// <preliminary />
		/// <summary>
		/// This is a method with a preliminary using the default description.
		/// </summary>
		public void MethodDefault()
		{
		}

		/// <summary>
		/// This method is not marked with preliminary, but is inside a class
		/// that is marked.
		/// </summary>
		public void MethodNoPreliminary()
		{
		}
	}
}
