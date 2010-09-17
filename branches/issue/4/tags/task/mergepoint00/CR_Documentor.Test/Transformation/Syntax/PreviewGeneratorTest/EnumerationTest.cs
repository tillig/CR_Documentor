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
		public void Flags_Basic()
		{
			var element = Isolate.Fake.Instance<Enumeration>();
			Isolate.WhenCalled(() => element.Visibility).WillReturn(MemberVisibility.Public);
			Isolate.WhenCalled(() => element.Name).WillReturn("TestEnum");
			Isolate.WhenCalled(() => element.UnderlyingType).WillReturn("");
			Isolate.WhenCalled(() => element.AttributeCount).WillReturn(1);

			NodeList list = new NodeList();
			var flags = Isolate.Fake.Instance<DevExpress.CodeRush.StructuralParser.Attribute>();
			Isolate.WhenCalled(() => flags.Name).WillReturn("FlagsAttribute");
			Isolate.WhenCalled(() => flags.ArgumentCount).WillReturn(0);
			list.Add(flags);
			Isolate.WhenCalled(() => element.Attributes).WillReturn(list);

			string expected =
@"<div class=""code vb"">
<div class=""attributes"">
<div class=""attribute"">
&lt;<a href=""#"">FlagsAttribute</a>&gt; _
</div>
</div><div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Enum</span> <span class=""identifier"">TestEnum</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Flags_CSharp()
		{
			var element = Isolate.Fake.Instance<Enumeration>();
			Isolate.WhenCalled(() => element.Visibility).WillReturn(MemberVisibility.Public);
			Isolate.WhenCalled(() => element.Name).WillReturn("TestEnum");
			Isolate.WhenCalled(() => element.UnderlyingType).WillReturn("");
			Isolate.WhenCalled(() => element.AttributeCount).WillReturn(1);

			NodeList list = new NodeList();
			var flags = Isolate.Fake.Instance<DevExpress.CodeRush.StructuralParser.Attribute>();
			Isolate.WhenCalled(() => flags.Name).WillReturn("FlagsAttribute");
			Isolate.WhenCalled(() => flags.ArgumentCount).WillReturn(0);
			list.Add(flags);
			Isolate.WhenCalled(() => element.Attributes).WillReturn(list);

			string expected =
@"<div class=""code cs"">
<div class=""attributes"">
<div class=""attribute"">
[<a href=""#"">FlagsAttribute</a>]
</div>
</div><div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">enum</span> <span class=""identifier"">TestEnum</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Simple_Basic()
		{
			var element = Isolate.Fake.Instance<Enumeration>();
			Isolate.WhenCalled(() => element.Visibility).WillReturn(MemberVisibility.Public);
			Isolate.WhenCalled(() => element.Name).WillReturn("TestEnum");
			Isolate.WhenCalled(() => element.UnderlyingType).WillReturn("");
			Isolate.WhenCalled(() => element.AttributeCount).WillReturn(0);

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Enum</span> <span class=""identifier"">TestEnum</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
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
			Isolate.WhenCalled(() => element.AttributeCount).WillReturn(0);

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">enum</span> <span class=""identifier"">TestEnum</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
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
			Isolate.WhenCalled(() => element.AttributeCount).WillReturn(0);

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Enum</span> <span class=""identifier"">TestEnum</span>&nbsp;<span class=""keyword"">As</span> <span class=""keyword"">Int64</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
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
			Isolate.WhenCalled(() => element.AttributeCount).WillReturn(0);

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">enum</span> <span class=""identifier"">TestEnum</span> : <span class=""keyword"">Int64</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
