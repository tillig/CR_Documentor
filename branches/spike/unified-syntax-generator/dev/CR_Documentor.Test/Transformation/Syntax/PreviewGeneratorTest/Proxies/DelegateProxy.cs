using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest.Proxies
{
	public class DelegateProxy : AccessSpecifiedElementProxy
	{
		public string MemberType { get; set; }
		public LanguageElementCollection Parameters { get; set; }

		public DelegateProxy(string name)
		{
			this.Visibility = MemberVisibility.Public;
			this.Parameters = new LanguageElementCollection();
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
