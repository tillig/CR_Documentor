Namespace Syntax
	''' <summary>
	''' Class used in testing syntax generation for generics. Class definition
	''' has a type constraint on the <typeparamref name="T"/> parameter.
	''' </summary>
	''' <typeparam name="T">Arbitrary type parameter.</typeparam>
	Public Class TestGenerics(Of T As {Attribute, ICloneable})

		''' <summary>
		''' Method that has a generic in the parameter list that has two generic types associated.
		''' </summary>
		''' <param name="param1">First parameter. Should be a <see cref="Dictionary(Of K, V)"/>.</param>
		Public Sub MethodWithGenericParameterDictionary(ByVal param1 As Dictionary(Of String, Boolean))
		End Sub

		''' <summary>
		''' Method that returns a generic type and has a generic in the parameter list.
		''' </summary>
		''' <param name="param1">First parameter. Should be a <see cref="IEnumerable(Of T)"/>.</param>
		''' <returns>Says it returns a <see cref="List(Of T)"/>, but always returns <see langword="null" />.</returns>
		Public Function MethodWithGenericReturnAndParameter(ByVal param1 As IEnumerable(Of String)) As List(Of String)
			Return Nothing
		End Function

		''' <summary>
		''' Method that has a type parameter.
		''' </summary>
		''' <typeparam name="P">The method's type parameter.</typeparam>
		Public Sub MethodWithTypeParameter(Of P)()
		End Sub

	End Class
End Namespace
