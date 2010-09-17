using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest.Proxies
{
	public class MethodProxy : MemberWithParametersProxy
	{
		public bool IsClassOperator { get; set; }
		public bool IsConstructor { get; set; }
		public bool IsDestructor { get; set; }
		public bool IsImplicitCast { get; set; }
		public bool IsExplicitCast { get; set; }

		public MethodProxy(string name)
		{
			this.Name = name;
		}

		public Method CreateFakeMethod()
		{
			var element = Isolate.Fake.Instance<Method>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
