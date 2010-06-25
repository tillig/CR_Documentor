using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;
namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest
{
	public class TypeReferenceExpressionProxy
	{
		private string _stringValue;

		public TypeReferenceExpressionProxy(string stringValue)
		{
			this._stringValue = stringValue;
		}

		public override string ToString()
		{
			return this._stringValue;
		}

		public TypeReferenceExpression CreateFakeTypeReferenceExpression()
		{
			var element = Isolate.Fake.Instance<TypeReferenceExpression>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
