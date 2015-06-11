using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.Proxies
{
	public class TypeParameterConstraintProxy
	{
		public string Name { get; private set; }

		public TypeParameterConstraintProxy(string name)
		{
			this.Name = name;
		}

		public TypeParameterConstraint CreateFakeTypeParameter()
		{
			var element = Isolate.Fake.Instance<TypeParameterConstraint>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
