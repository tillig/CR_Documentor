using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest.Proxies
{
	public abstract class MemberProxy : AccessSpecifiedElementProxy
	{
		public string MemberType { get; set; }
	}
}
