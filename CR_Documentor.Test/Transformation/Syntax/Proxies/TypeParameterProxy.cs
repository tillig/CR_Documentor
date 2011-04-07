using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.Proxies
{
	public class TypeParameterProxy
	{
		public string Name { get; private set; }
		public TypeParameterConstraintCollection Constraints { get; set; }

		public TypeParameterProxy(string name)
		{
			this.Name = name;
			this.Constraints = new TypeParameterConstraintCollection();
		}

		public TypeParameter CreateFakeTypeParameter()
		{
			var element = Isolate.Fake.Instance<TypeParameter>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
