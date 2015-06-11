Namespace Syntax
	''' <summary>
	''' This class is a test for the VB "Implements" keyword. It implements
	''' the TestInterface.
	''' </summary>
	Public Class TestInterfaceImplementation
		Implements TestInterface

		''' <summary>
		''' This is a member of the interface that is a Sub.
		''' </summary>
		Public Sub SubInterfaceMember() Implements TestInterface.SubInterfaceMember

		End Sub

		''' <summary>
		''' This is a member of the interface that is a Function.
		''' </summary>
		''' <param name="param1">The first parameter.</param>
		''' <returns>An arbitrary integer return value.</returns>
		Public Function FunctionInterfaceMember(ByVal param1 As String) As Integer Implements TestInterface.FunctionInterfaceMember
			Return 0
		End Function

	End Class
End Namespace

