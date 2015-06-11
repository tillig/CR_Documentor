using System;
using System.Collections.Specialized;

namespace CR_Documentor.Test.Transformation.Syntax.Proxies
{
	public abstract class MemberProxy : AccessSpecifiedElementProxy
	{
		public StringCollection Implements { get; set; }
		public int ImplementsCount
		{
			get
			{
				if (this.Implements == null)
				{
					return 0;
				}
				return this.Implements.Count;
			}
		}
		public string MemberType { get; set; }
		public MemberProxy()
		{
			this.Implements = new StringCollection();
		}
	}
}
