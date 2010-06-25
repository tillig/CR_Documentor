using System;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest
{
	[TestClass]
	[Isolated]
	public class ClassTest
	{
		[TestMethod]
		public void Simple_Basic()
		{
			ClassInfo info = new ClassInfo("TestClass")
			{
				Visibility = MemberVisibility.ProtectedInternal
			};
			var element = this.CreateFakeClass(info);

			string expected =
@"<div class=""code"">
<div class=""member"">
<span class=""keyword"">Protected Friend</span> <span class=""keyword"">Class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Simple_CSharp()
		{
			ClassInfo info = new ClassInfo("TestClass")
			{
				Visibility = MemberVisibility.ProtectedInternal
			};
			var element = this.CreateFakeClass(info);

			string expected =
@"<div class=""code"">
<div class=""member"">
<span class=""keyword"">protected internal</span> <span class=""keyword"">class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		private Class CreateFakeClass(ClassInfo info)
		{
			var element = Isolate.Fake.Instance<Class>();
			Isolate.Swap.CallsOn(element).WithCallsTo(info);
			return element;
		}

		private class ClassInfo
		{
			public bool IsNew { get; set; }
			public MemberVisibility Visibility { get; set; }
			public bool IsStatic { get; set; }
			public bool IsAbstract { get; set; }
			public bool IsSealed { get; set; }
			public string Name { get; private set; }
			public bool IsGeneric { get; set; }
			public int AttributeCount {
				get
				{
					return this.Attributes.Count;
				}
			}
			public NodeList Attributes { get; set; }

			public ClassInfo(string name)
			{
				this.Visibility = MemberVisibility.Public;
				this.Attributes = new NodeList();
				this.Name = name;
			}
		}
	}
}
