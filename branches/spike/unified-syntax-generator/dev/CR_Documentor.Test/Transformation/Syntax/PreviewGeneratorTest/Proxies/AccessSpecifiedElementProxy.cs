using System;
using DevExpress.CodeRush.StructuralParser;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest.Proxies
{
	public abstract class AccessSpecifiedElementProxy
	{
		public GenericModifier GenericModifier { get; private set; }
		public bool IsAbstract { get; set; }
		public bool IsGeneric
		{
			get
			{
				return this.GenericModifier != null;
			}
		}
		public bool IsNew { get; set; }
		public bool IsOverride { get; set; }
		public bool IsReadOnly { get; set; }
		public bool IsSealed { get; set; }
		public bool IsStatic { get; set; }
		public bool IsVirtual { get; set; }
		public bool IsWriteOnly { get; set; }
		public string Name { get; set; }
		public MemberVisibility Visibility { get; set; }

		protected AccessSpecifiedElementProxy()
		{
			this.Visibility = MemberVisibility.Public;
			this.GenericModifier = null;
		}

		public void SetTypeParameters(TypeParameterCollection parameters)
		{
			if (parameters == null)
			{
				this.GenericModifier = null;
			}
			else
			{
				this.GenericModifier = new GenericModifier(parameters);
			}
		}
	}
}
