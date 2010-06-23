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
		public void Generate_LanguageNotSupported()
		{
			var element = Isolate.Fake.Instance<AccessSpecifiedElement>();
			var generator = new PreviewGenerator(element, SupportedLanguageId.None);
			string preview = generator.Generate();
			Assert.AreEqual("<div class=\"code\">" + Environment.NewLine + Resources.PreviewGenerator_LanguageNotSupported + Environment.NewLine + "</div>", preview);
		}
	}
}
