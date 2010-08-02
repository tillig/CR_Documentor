using System;

namespace DocTestCS.Syntax
{
	/// <summary>
	/// Interface used in testing syntax generation.
	/// </summary>
	public interface TestInterface
	{
		/// <summary>
		/// This is a member of the interface that is a Sub.
		/// </summary>
		void SubInterfaceMember();

		/// <summary>
		/// This is a member of the interface that is a Function.
		/// </summary>
		/// <param name="param1">The first parameter.</param>
		/// <returns>An arbitrary integer return value.</returns>
		int FunctionInterfaceMember(string param1);
	}
}
