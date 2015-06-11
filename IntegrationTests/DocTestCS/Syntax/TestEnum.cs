using System;
using System.Xml.Serialization;

namespace DocTestCS.Syntax
{
	/// <summary>
	/// Enum used in testing syntax generation.
	/// </summary>
	[Flags]
	[XmlRoot("root", DataType = "string")]
	public enum TestEnum : long
	{
		/// <summary>
		/// First value.
		/// </summary>
		Value1,

		/// <summary>
		/// Second value.
		/// </summary>
		[XmlEnum]
		Value2,

		// Value3 // Uncomment to see a node with no documentation - should render the member but no summary when class doc is viewed.
	}
}
