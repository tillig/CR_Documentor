using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.Proxies
{
	public class EventProxy : MemberWithParametersProxy
	{
		public EventProxy(string name)
		{
			this.Name = name;
		}

		public Event CreateFakeEvent()
		{
			var element = Isolate.Fake.Instance<Event>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
