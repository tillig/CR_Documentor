using System;
using DevExpress.CodeRush.StructuralParser;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest
{
	public class ClassProxy
	{
		public bool IsNew { get; set; }
		public MemberVisibility Visibility { get; set; }
		public bool IsStatic { get; set; }
		public bool IsAbstract { get; set; }
		public bool IsSealed { get; set; }
		public string Name { get; private set; }
		public bool IsGeneric
		{
			get
			{
				return this.GenericModifier != null;
			}
		}
		public int AttributeCount
		{
			get
			{
				return this.Attributes.Count;
			}
		}
		public NodeList Attributes { get; set; }
		public GenericModifier GenericModifier { get; private set; }

		public ClassProxy(string name)
		{
			this.Visibility = MemberVisibility.Public;
			this.Attributes = new NodeList();
			this.GenericModifier = null;
			this.Name = name;
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

		public Class CreateFakeClass()
		{
			var element = Isolate.Fake.Instance<Class>();
			Isolate.Swap.CallsOn(element).WithCallsTo(this);
			return element;
		}
	}
}
