using System;

namespace DocTestCS.Syntax
{
	/// <summary>
	/// Explicit implementation of the TestInterface.
	/// </summary>
	public class TestInterfaceImplementationExplicit : TestInterface
	{
		/// <summary>
		/// This is a member of the interface that is a Sub.
		/// </summary>
		void TestInterface.SubInterfaceMember()
		{
		}

		/// <summary>
		/// This is a member of the interface that is a Function.
		/// </summary>
		/// <param name="param1">The first parameter.</param>
		/// <returns>An arbitrary integer return value.</returns>
		int TestInterface.FunctionInterfaceMember(string param1)
		{
			return 0;
		}
	}
}
