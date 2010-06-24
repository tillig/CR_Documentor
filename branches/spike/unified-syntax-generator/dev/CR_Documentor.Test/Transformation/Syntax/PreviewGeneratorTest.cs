using System;
using CR_Documentor.Properties;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax
{
	[TestClass]
	[Isolated]
	public class PreviewGeneratorTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullElement()
		{
			new PreviewGenerator(null, SupportedLanguageId.CSharp);
		}

		[TestMethod]
		public void Ctor_PropertiesSet()
		{
			var element = Isolate.Fake.Instance<AccessSpecifiedElement>();
			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			Assert.AreSame(generator.Element, element);
			Assert.AreEqual(SupportedLanguageId.CSharp, generator.Language);
		}

		[TestMethod]
		public void Generate_Enumeration_Basic_Simple()
		{
			var element = Isolate.Fake.Instance<Enumeration>();
			Isolate.WhenCalled(() => element.Visibility).WillReturn(MemberVisibility.Public);
			Isolate.WhenCalled(() => element.Name).WillReturn("TestEnum");

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
		public void Generate_Enumeration_CSharp_Simple()
		{
			var element = Isolate.Fake.Instance<Enumeration>();
			Isolate.WhenCalled(() => element.Visibility).WillReturn(MemberVisibility.Public);
			Isolate.WhenCalled(() => element.Name).WillReturn("TestEnum");

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
		public void Generate_LanguageNotSupported()
		{
			var element = Isolate.Fake.Instance<AccessSpecifiedElement>();

			string expected =
@"<div class=""code"">
{0}
</div>";
			expected = String.Format(expected, Resources.PreviewGenerator_LanguageNotSupported);

			var generator = new PreviewGenerator(element, SupportedLanguageId.None);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
