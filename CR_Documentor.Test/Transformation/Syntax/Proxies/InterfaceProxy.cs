using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.Proxies
{
	public class InterfaceProxy : ClassProxy
	{
		public InterfaceProxy(string name)
			: base(name)
		{
		}

		public Interface CreateFakeInterface()
		{
			var element = Isolate.Fake.Instance<Interface>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
