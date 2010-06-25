using System;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest
{
	[TestClass]
	[Isolated]
	public class EnumerationTest
	{
		[TestMethod]
		public void Simple_Basic()
		{
			var element = Isolate.Fake.Instance<Enumeration>();
			Isolate.WhenCalled(() => element.Visibility).WillReturn(MemberVisibility.Public);
			Isolate.WhenCalled(() => element.Name).WillReturn("TestEnum");
			Isolate.WhenCalled(() => element.UnderlyingType).WillReturn("");

			string expected =
@"<div class=""code"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Enum</span> <span class=""identifier"">TestEnum</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Simple_CSharp()
		{
			var element = Isolate.Fake.Instance<Enumeration>();
			Isolate.WhenCalled(() => element.Visibility).WillReturn(MemberVisibility.Public);
			Isolate.WhenCalled(() => element.Name).WillReturn("TestEnum");
			Isolate.WhenCalled(() => element.UnderlyingType).WillReturn("");

			string expected =
@"<div class=""code"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">enum</span> <span class=""identifier"">TestEnum</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void UnderlyingType_Basic()
		{
			var element = Isolate.Fake.Instance<Enumeration>();
			Isolate.WhenCalled(() => element.Visibility).WillReturn(MemberVisibility.Public);
			Isolate.WhenCalled(() => element.Name).WillReturn("TestEnum");
			Isolate.WhenCalled(() => element.UnderlyingType).WillReturn("Int64");

			string expected =
@"<div class=""code"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Enum</span> <span class=""identifier"">TestEnum</span> <span class=""keyword"">As</span> <span class=""keyword"">Int64</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void UnderlyingType_CSharp()
		{
			var element = Isolate.Fake.Instance<Enumeration>();
			Isolate.WhenCalled(() => element.Visibility).WillReturn(MemberVisibility.Public);
			Isolate.WhenCalled(() => element.Name).WillReturn("TestEnum");
			Isolate.WhenCalled(() => element.UnderlyingType).WillReturn("Int64");

			string expected =
@"<div class=""code"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">enum</span> <span class=""identifier"">TestEnum</span> : <span class=""keyword"">Int64</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
