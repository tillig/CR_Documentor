using System;

namespace DocTest.Tags.TopLevel
{
	/// <summary>
	/// This class tests the 'param' tag.
	/// </summary>
	public class Param
	{
		/// <summary>
		/// This is a method with one parameter.
		/// </summary>
		/// <param name="param1">The parameter for this method.</param>
		public void MethodOneParameter(object param1)
		{
		}

		/// <summary>
		/// This is a method with three parameters.
		/// </summary>
		/// <param name="param1">The first parameter for this method.</param>
		/// <param name="param2">The second parameter for this method.</param>
		/// <param name="param3">The third parameter for this method.</param>
		public void MethodMultipleParameters(object param1, object param2, object param3)
		{
		}

		/// <summary>
		/// This is a property with a param.
		/// </summary>
		/// <param name="index">
		/// The parameter for this property.
		/// </param>
		public string this[int index]
		{
			get
			{
				return "";
			}
			set
			{
				// No action
			}
		}
	}
}
