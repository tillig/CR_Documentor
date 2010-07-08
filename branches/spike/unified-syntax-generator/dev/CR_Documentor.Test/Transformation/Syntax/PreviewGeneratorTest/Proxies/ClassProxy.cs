using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest.Proxies
{
	public class ClassProxy : AccessSpecifiedElementProxy
	{
		public int AttributeCount
		{
			get
			{
				return this.Attributes.Count;
			}
		}
		public NodeList Attributes { get; set; }

		public ClassProxy(string name)
		{
			this.Attributes = new NodeList();
			this.Name = name;
		}

		public Class CreateFakeClass()
		{
			var element = Isolate.Fake.Instance<Class>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
