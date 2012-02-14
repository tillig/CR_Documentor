using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.Proxies
{
	public class AttributeProxy
	{
		public int ArgumentCount
		{
			get
			{
				return this.Arguments.Count;
			}
		}

		public ExpressionCollection Arguments { get; set; }
		public string Name { get; set; }

		public AttributeProxy(string name)
		{
			this.Name = name;
			this.Arguments = new ExpressionCollection();
		}

		public DevExpress.CodeRush.StructuralParser.Attribute CreateFakeAttribute()
		{
			var element = Isolate.Fake.Instance<DevExpress.CodeRush.StructuralParser.Attribute>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
