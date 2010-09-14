using System;
using CR_Documentor.Transformation.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test.Transformation.Syntax
{
	[TestClass]
	public class LanguageTest
	{
		[TestMethod]
		public void ConvertToSupportedLanguageId_Basic()
		{
			Assert.AreEqual(SupportedLanguageId.Basic, Language.ConvertToSupportedLanguageId(Language.Basic));
		}

		[TestMethod]
		public void ConvertToSupportedLanguageId_Cpp()
		{
			Assert.AreEqual(SupportedLanguageId.None, Language.ConvertToSupportedLanguageId("C/C++"));
		}

		[TestMethod]
		public void ConvertToSupportedLanguageId_CSharp()
		{
			Assert.AreEqual(SupportedLanguageId.CSharp, Language.ConvertToSupportedLanguageId(Language.CSharp));
		}

		[TestMethod]
		public void ConvertToSupportedLanguageId_Empty()
		{
			Assert.AreEqual(SupportedLanguageId.None, Language.ConvertToSupportedLanguageId(""));
		}
		
		[TestMethod]
		public void ConvertToSupportedLanguageId_Null()
		{
			Assert.AreEqual(SupportedLanguageId.None, Language.ConvertToSupportedLanguageId(null));
		}
	}
}
