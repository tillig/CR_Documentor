using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.Proxies
{
	public class NamedTypeParameterConstraintProxy
	{
		public TypeReferenceExpression TypeReference { get; private set; }

		public NamedTypeParameterConstraintProxy(string stringValue)
		{
			this.TypeReference = new TypeReferenceExpressionProxy(stringValue).CreateFakeTypeReferenceExpression();
		}

		public NamedTypeParameterConstraint CreateFakeTypeParameter()
		{
			var element = Isolate.Fake.Instance<NamedTypeParameterConstraint>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
