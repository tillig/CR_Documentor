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
	public class DelegateTest
	{
		[TestMethod]
		public void SimpleReturnValue_Basic()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate")
			{
				MemberType = "String"
			};
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Delegate</span> <span class=""keyword"">Function</span> <span class=""identifier"">TestDelegate</span> <span class=""keyword"">As</span> <a href=""#"">String</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SimpleReturnValue_CSharp()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate")
			{
				MemberType = "string"
			};
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">delegate</span> <a href=""#"">string</a> <span class=""identifier"">TestDelegate</span><div class=""parameters"">
()
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SimpleVoid_Basic()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate");
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Delegate</span> <span class=""keyword"">Sub</span> <span class=""identifier"">TestDelegate</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SimpleVoid_CSharp()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate");
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">delegate</span> <span class=""keyword"">void</span> <span class=""identifier"">TestDelegate</span><div class=""parameters"">
()
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
