using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.Proxies
{
	public class DelegateProxy : MemberWithParametersProxy
	{
		public DelegateProxy(string name)
		{
			this.Name = name;
		}

		public DelegateDefinition CreateFakeDelegate()
		{
			var element = Isolate.Fake.Instance<DelegateDefinition>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
