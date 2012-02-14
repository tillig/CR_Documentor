using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.Proxies
{
	public class PropertyProxy : MemberWithParametersProxy
	{
		public bool HasGetter { get; set; }
		public bool HasSetter { get; set; }

		public PropertyProxy(string name)
		{
			this.Name = name;
		}

		public Property CreateFakeProperty()
		{
			var element = Isolate.Fake.Instance<Property>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
