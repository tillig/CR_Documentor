Namespace Syntax

	''' <summary>
	''' Class used in testing syntax generation.
	''' </summary>
	<Serializable()> _
	Public Class TestClass

#Region "Fields"

		''' <summary>
		''' Constant.
		''' </summary>
		Public Const ConstantString As String = "Constant"

		''' <summary>
		''' Static read-only.
		''' </summary>
		Protected Shared ReadOnly StaticString As String = "Static"
#End Region

#Region "Events and Delegates"

		''' <summary>
		''' Delegate that takes parameters.
		''' </summary>
		Public Delegate Function DelegateWithParameters(ByVal param1 As System.String) As Int32

		''' <summary>
		''' Delegate that doesn't take parameters.
		''' </summary>
		Public Delegate Function DelegateWithoutParameters() As Int32

		''' <summary>
		''' Delegate that has no return value.
		''' </summary>
		Public Delegate Sub DelegateWithoutReturnValue()

		''' <summary>
		''' Event using a parameterized eventhandler signature.
		''' </summary>
		Public Event EventParameterized(ByVal param1 As String)

		''' <summary>
		''' Event using a non-generic eventhandler signature.
		''' </summary>
		Public Event EventRegular As EventHandler

		''' <summary>
		''' Event using a generic eventhandler signature.
		''' </summary>
		Public Event EventGeneric As EventHandler(Of EventArgs)

#End Region

#Region "Constructors and Destructors"

		''' <summary>
		''' Default constructor.
		''' </summary>
		Public Sub New()

		End Sub

		''' <summary>
		''' Overloaded constructor with one parameter.
		''' </summary>
		''' <param name="param1">First parameter.</param>
		Public Sub New(ByVal param1 As String)

		End Sub

		''' <summary>
		''' Overloaded constructor with two parameters.
		''' </summary>
		''' <param name="param1">First parameter.</param>
		''' <param name="param2">Second parameter.</param>
		Public Sub New(ByVal param1 As String, ByVal param2 As String)

		End Sub

		''' <summary>
		''' Destructor.
		''' </summary>
		Protected Overrides Sub Finalize()

		End Sub

#End Region

#Region "Properties"

		''' <summary>
		''' Indexed property.
		''' </summary>
		''' <param name="index">Index parameter.</param>
		Public Property Item(ByVal index As Int32) As String
			Get
				Return ""
			End Get
			Set(ByVal Value As String)

			End Set
		End Property

		''' <summary>
		''' Read-only property.
		''' </summary>
		Public ReadOnly Property PropertyReadOnly() As String
			Get
				Return ""
			End Get
		End Property

		''' <summary>
		''' Write-only property.
		''' </summary>
		Public WriteOnly Property PropertyWriteOnly() As String
			Set(ByVal value As String)

			End Set
		End Property

		''' <summary>
		''' Read/write property.
		''' </summary>
		Public Property PropertyReadWrite() As String
			Get
				Return ""
			End Get
			Set(ByVal value As String)

			End Set
		End Property

#End Region

#Region "Methods"

		''' <summary>
		''' Method marked as both "Protected" and "Shared."
		''' </summary>
		Protected Shared Sub MethodProtectedStatic()

		End Sub

		''' <summary>
		''' Method marked with the "Overridable" keyword.
		''' </summary>
		Public Overridable Sub MethodVirtual()

		End Sub

		''' <summary>
		''' Method with no parameters.
		''' </summary>
		Public Sub MethodWithNoParams()

		End Sub

		''' <summary>
		''' Method with optional parameters. Uses VB "Optional" keyword.
		''' </summary>
		''' <param name="required">Required parameter.</param>
		''' <param name="notRequired">Optional parameter.</param>
		Public Sub MethodWithOptionalParam(ByVal required As System.String, Optional ByVal notRequired As System.String = "")

		End Sub

		''' <summary>
		''' Method with an out parameter. (VB uses ByRef).
		''' </summary>
		''' <param name="required">Required parameter.</param>
		Public Sub MethodWithOutParam(ByRef required As System.String)

		End Sub

		''' <summary>
		''' Method with "params" array.
		''' </summary>
		''' <param name="required">Required parameter.</param>
		''' <param name="notRequired">Parameter array.</param>
		Public Sub MethodWithParamArray(ByVal required As System.String, ByVal ParamArray notRequired As Object())

		End Sub

		''' <summary>
		''' Method with a ref parameter. (VB uses ByRef).
		''' </summary>
		''' <param name="required">Required parameter.</param>
		Public Sub MethodWithRefParam(ByRef required As System.String)

		End Sub

		''' <summary>
		''' Method that returns a string.
		''' </summary>
		''' <returns>Always returns emtpy string.</returns>
		Public Function MethodWithReturnValue() As String
			Return ""
		End Function

#End Region

#Region "Operators"

		''' <summary>
		''' Overloaded multiplication operator.
		''' </summary>
		''' <param name="a">First parameter.</param>
		''' <param name="b">Second parameter.</param>
		''' <returns>Always returns <see langword="null" />.</returns>
		Public Shared Operator *(ByVal a As TestClass, ByVal b As TestClass) As TestClass
			Return Nothing
		End Operator

		''' <summary>
		''' Overloaded unary negation operator.
		''' </summary>
		''' <param name="a">First parameter.</param>
		''' <returns>Always returns <see langword="null" />.</returns>
		Public Shared Operator -(ByVal a As TestClass) As TestClass
			Return Nothing
		End Operator

		''' <summary>
		''' Overloaded widening conversion to double.
		''' </summary>
		''' <param name="a">First parameter.</param>
		''' <returns>Always returns <see langword="null" />.</returns>
		Public Shared Widening Operator CType(ByVal a As TestClass) As Double
			Return Nothing
		End Operator

		''' <summary>
		''' Overloaded narrowing conversion to long.
		''' </summary>
		''' <param name="a">First parameter.</param>
		''' <returns>Always returns <see langword="null" />.</returns>
		Public Shared Narrowing Operator CType(ByVal a As TestClass) As Long
			Return Nothing
		End Operator

#End Region

	End Class

End Namespace
