using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest.Proxies
{
	public abstract class LanguageElementProxy
	{
		private LanguageElement _parent;

		public LanguageElement GetParentClassInterfaceStructOrModule()
		{
			return _parent;
		}

		public void __SetParentClassInterfaceStructOrModule(LanguageElement parent)
		{
			this._parent = parent;
		}
	}
}
