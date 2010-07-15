using System;
using CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest.Proxies;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest
{
	[TestClass]
	[Isolated]
	public class PropertyTest
	{
		[TestMethod]
		public void GetSet_Basic()
		{
			PropertyProxy info = new PropertyProxy("TestProperty")
			{
				MemberType = "String",
				HasGetter = true,
				HasSetter = true
			};
			var element = info.CreateFakeProperty();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Property</span> <span class=""identifier"">TestProperty</span> <span class=""keyword"">As</span> <a href=""#"">String</a><div class=""getset"">
<span class=""keyword"">Get</span>
</div><div class=""getset"">
<span class=""keyword"">Set</span>
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetSet_CSharp()
		{
			PropertyProxy info = new PropertyProxy("TestProperty")
			{
				MemberType = "string",
				HasGetter = true,
				HasSetter = true
			};
			var element = info.CreateFakeProperty();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <a href=""#"">string</a> <span class=""identifier"">TestProperty</span> {<div class=""getset"">
<span class=""keyword"">get</span>;
</div><div class=""getset"">
<span class=""keyword"">set</span>;
</div>}
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
