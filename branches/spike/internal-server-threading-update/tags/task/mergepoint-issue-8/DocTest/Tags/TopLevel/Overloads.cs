using System;

namespace DocTest.Tags.TopLevel
{
	/// <summary>
	/// This class tests the 'overloads' tag.
	/// </summary>
	public class Overloads
	{
		/// <overloads>
		///	<summary>
		/// This is a method with an 'overloads' summary.
		///	</summary>
		///	<remarks>
		///	This is a method with an 'overloads' remarks.
		///	</remarks>
		///	<example>
		///	This is a method with an 'overloads' example.
		///	</example>
		/// </overloads>
		public void Method()
		{
		}

		/// <summary>
		/// This summary should be dealt with via the 'overloads' tag.
		/// </summary>
		/// <param name="obj">The sender for the event.</param>
		public void Method(object obj)
		{
		}
	}
}
