using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest.Proxies
{
	public class BaseVariableProxy : AccessSpecifiedElementProxy
	{
		public BaseVariableProxy(string name)
		{
			this.Name = name;
		}

		public BaseVariable CreateFakeField()
		{
			var element = Isolate.Fake.Instance<BaseVariable>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
