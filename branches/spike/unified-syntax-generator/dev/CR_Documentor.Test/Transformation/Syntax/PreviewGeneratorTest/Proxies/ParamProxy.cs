using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest.Proxies
{
	public class ParamProxy : AccessSpecifiedElementProxy
	{
		public bool IsOptional { get; set; }
		public bool IsOutParam { get; set; }
		public bool IsReferenceParam { get; set; }
		public bool IsParamArray { get; set; }
		public string ParamType { get; set; }
		public string DefaultValue { get; set; }

		public ParamProxy(string name)
		{
			this.Name = name;
		}

		public Param CreateFakeParam()
		{
			var element = Isolate.Fake.Instance<Param>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
