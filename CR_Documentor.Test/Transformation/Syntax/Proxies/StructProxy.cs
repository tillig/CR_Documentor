using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.Proxies
{
	public class StructProxy : ClassProxy
	{
		public StructProxy(string name)
			: base(name)
		{
		}

		public Struct CreateFakeStruct()
		{
			var element = Isolate.Fake.Instance<Struct>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
