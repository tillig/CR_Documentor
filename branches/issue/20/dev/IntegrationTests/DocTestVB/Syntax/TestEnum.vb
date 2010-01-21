Namespace Syntax

	''' <summary>
	''' Enum used in testing syntax generation.
	''' </summary>
	<Flags(), _
	 System.Xml.Serialization.XmlRootAttribute("root", DataType:="string")> _
	Public Enum TestEnum As Long

		''' <summary>
		''' First value.
		''' </summary>
		Value1

		''' <summary>
		''' Second value.
		''' </summary>
		Value2

	End Enum

End Namespace
