using System;

namespace DocTestCS.Syntax
{
	/// <summary>
	/// Implicit implementation of the TestInterface.
	/// </summary>
	public class TestInterfaceImplementationImplicit : TestInterface
	{
		/// <summary>
		/// This is a member of the interface that is a Sub.
		/// </summary>
		public void SubInterfaceMember()
		{
		}

		/// <summary>
		/// This is a member of the interface that is a Function.
		/// </summary>
		/// <param name="param1">The first parameter.</param>
		/// <returns>An arbitrary integer return value.</returns>
		public int FunctionInterfaceMember(string param1)
		{
			return 0;
		}
	}
}
