using System;

namespace DocTestCS.Tags.TopLevel
{
	/// <summary>
	/// This class tests the 'exception' tag.
	/// </summary>
	public class Exception
	{
		/// <summary>
		/// This is a method with an exception.
		/// </summary>
		/// <exception cref="System.Exception">
		/// This is a reference to the System.Exception exception.
		/// </exception>
		public void MethodException()
		{
		}

		/// <summary>
		/// This is a method with an exception.
		/// </summary>
		/// <exception cref="System.ArgumentNullException">
		/// This is a reference to the System.ArgumentNullException exception.
		/// </exception>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// This is a reference to the System.ArgumentOutOfRangeException exception.
		/// </exception>
		public void MethodMultipleExceptions()
		{
		}
	}
}
