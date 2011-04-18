using System;
using CR_Documentor.Properties;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.SimpleSignatureGeneratorTest
{
	[TestClass]
	[Isolated]
	public class GeneralTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullElement()
		{
			new SimpleSignatureGenerator(null, SupportedLanguageId.CSharp);
		}

		[TestMethod]
		public void Ctor_PropertiesSet()
		{
			var element = Isolate.Fake.Instance<AccessSpecifiedElement>();
			var generator = new SimpleSignatureGenerator(element, SupportedLanguageId.CSharp);
			Assert.AreSame(generator.Element, element);
			Assert.AreEqual(SupportedLanguageId.CSharp, generator.Language);
		}

		[TestMethod]
		public void Generate_LanguageElementNotSupported()
		{
			var element = Isolate.Fake.Instance<AccessSpecifiedElement>();
			var generator = new SimpleSignatureGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual("", actual);
		}

		[TestMethod]
		public void Generate_LanguageNotSupported()
		{
			var element = Isolate.Fake.Instance<AccessSpecifiedElement>();
			var generator = new SimpleSignatureGenerator(element, SupportedLanguageId.None);
			string actual = generator.Generate();
			Assert.AreEqual("", actual);
		}
	}
}
