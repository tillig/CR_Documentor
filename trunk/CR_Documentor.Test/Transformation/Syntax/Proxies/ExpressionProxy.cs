using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.Proxies
{
	public class ExpressionProxy
	{
		public string StringValue { get; private set; }

		public ExpressionProxy(string stringValue)
		{
			this.StringValue = stringValue;
		}

		public override string ToString()
		{
			return this.StringValue;
		}

		public Expression CreateFakeExpression()
		{
			var element = Isolate.Fake.Instance<Expression>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
