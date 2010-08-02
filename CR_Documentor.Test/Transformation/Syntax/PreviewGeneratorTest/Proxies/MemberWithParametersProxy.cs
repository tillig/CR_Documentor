using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest.Proxies
{
	public abstract class MemberWithParametersProxy : MemberProxy
	{
		public LanguageElementCollection Parameters { get; set; }
		public int ParameterCount
		{
			get
			{
				return this.Parameters.Count;
			}
		}
		public MemberWithParametersProxy()
		{
			this.Parameters = new LanguageElementCollection();
		}
	}
}
