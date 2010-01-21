using System;

namespace DocTestCS.Tags.TopLevel
{
	/// <summary>
	/// This class tests the 'value' tag.
	/// </summary>
	public class Value
	{
		/// <summary>
		/// This is a property with a value.
		/// </summary>
		/// <value>
		/// Always returns empty string.
		/// </value>
		public string Property
		{
			get
			{
				return "";
			}
			set
			{
			}
		}
	}
}
