using System;

namespace DocTest.Tags.TopLevel
{
	/// <summary>
	/// This class tests the 'example' tag.
	/// </summary>
	/// <example>
	/// This is some example text.
	/// </example>
	public class ExampleClass
	{
		/// <summary>
		/// This is a method with an example.
		/// </summary>
		/// <example>
		/// This is some example text.
		/// </example>
		public void Method()
		{
		}
	}

	/// <summary>
	/// This enum tests the 'example' tag. Enum values don't generally have
	/// individually rendered documentation.
	/// </summary>
	/// <example>
	/// This is some example text.
	/// </example>
	public enum ExampleEnum
	{
		/// <summary>
		/// This is value 1 with an example.
		/// </summary>
		/// <example>
		/// This is some example text.
		/// </example>
		Value1,

		/// <summary>
		/// This is value 2 with an example.
		/// </summary>
		/// <example>
		/// This is some example text.
		/// </example>
		Value2
	}
}
