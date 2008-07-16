using System;
using System.Xml.Serialization;

namespace DocTest.Syntax
{
	/// <summary>
	/// Enum used in testing syntax generation.
	/// </summary>
	[Flags]
	[XmlRoot("root", DataType="string")]
	public enum TestEnum : long
	{
		/// <summary>
		/// First value.
		/// </summary>
		Value1,

		/// <summary>
		/// Second value.
		/// </summary>
		Value2
	}
}
