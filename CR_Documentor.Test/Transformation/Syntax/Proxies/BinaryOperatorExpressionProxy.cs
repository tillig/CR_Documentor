using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.Proxies
{
	public class BinaryOperatorExpressionProxy
	{
		public Expression LeftSide { get; private set; }
		public Expression RightSide { get; private set; }

		public BinaryOperatorExpressionProxy(string leftSide, string rightSide)
		{
			this.LeftSide = new ExpressionProxy(leftSide).CreateFakeExpression();
			this.RightSide = new ExpressionProxy(rightSide).CreateFakeExpression();
		}

		public BinaryOperatorExpression CreateFakeExpression()
		{
			var element = Isolate.Fake.Instance<BinaryOperatorExpression>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
