using System;
using CR_Documentor.Properties;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest
{
	[TestClass]
	[Isolated]
	public class GeneralTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullElement()
		{
			new PreviewGenerator(null, SupportedLanguageId.CSharp, true);
		}

		[TestMethod]
		public void Ctor_PropertiesSet()
		{
			var element = Isolate.Fake.Instance<AccessSpecifiedElement>();
			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			Assert.AreSame(generator.Element, element);
			Assert.AreEqual(SupportedLanguageId.CSharp, generator.Language);
		}

		[TestMethod]
		public void Generate_LanguageElementNotSupported()
		{
			var element = Isolate.Fake.Instance<AccessSpecifiedElement>();

			string expected =
@"<div class=""code cs"">
{0}
</div>";
			expected = String.Format(expected, Strings.PreviewGenerator_LanguageElementNotSupported);

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
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
			expected = String.Format(expected, Strings.PreviewGenerator_LanguageNotSupported);

			var generator = new PreviewGenerator(element, SupportedLanguageId.None, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
