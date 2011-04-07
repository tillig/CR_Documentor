using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.Proxies
{
	public abstract class LanguageElementProxy
	{
		// TODO: There should be a more diligent mechanism for managing the parent class, interface, struct, or module than this.
		public LanguageElement Parent { get; set; }

		public LanguageElement GetParentClassInterfaceStructOrModule()
		{
			return this.Parent;
		}
	}
}
