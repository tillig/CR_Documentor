Namespace Syntax

	''' <summary>
	''' Interface used in testing syntax generation.
	''' </summary>
	Public Interface TestInterface

		''' <summary>
		''' This is a member of the interface that is a Sub.
		''' </summary>
		Sub SubInterfaceMember()

		''' <summary>
		''' This is a member of the interface that is a Function.
		''' </summary>
		''' <param name="param1">The first parameter.</param>
		''' <returns>An arbitrary integer return value.</returns>
		Function FunctionInterfaceMember(ByVal param1 As String) As Integer

	End Interface

End Namespace
