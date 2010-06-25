using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest
{
	public class NamedTypeParameterConstraintProxy
	{
		public string StringValue { get; private set; }

		public NamedTypeParameterConstraintProxy(string stringValue)
		{
			this.StringValue = stringValue;
		}

		public override string ToString()
		{
			return this.StringValue;
		}

		public NamedTypeParameterConstraint CreateFakeTypeParameter()
		{
			var element = Isolate.Fake.Instance<NamedTypeParameterConstraint>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
