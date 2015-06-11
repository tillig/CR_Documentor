using System;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test.Transformation.Syntax
{
	[TestClass]
	public class MemberVisibilityExtensionsTest
	{
		[TestMethod]
		public void ForLanguage_Protected_VB()
		{
			Assert.AreEqual("Protected", MemberVisibility.Protected.ForLanguage(SupportedLanguageId.Basic));
		}

		[TestMethod]
		public void ForLanguage_Protected_CS()
		{
			Assert.AreEqual("protected", MemberVisibility.Protected.ForLanguage(SupportedLanguageId.CSharp));
		}

		[TestMethod]
		public void ForLanguage_ProtectedInternal_VB()
		{
			Assert.AreEqual("Protected Friend", MemberVisibility.ProtectedInternal.ForLanguage(SupportedLanguageId.Basic));
		}

		[TestMethod]
		public void ForLanguage_ProtectedInternal_CS()
		{
			Assert.AreEqual("protected internal", MemberVisibility.ProtectedInternal.ForLanguage(SupportedLanguageId.CSharp));
		}

		[TestMethod]
		public void ForLanguage_Unknown_VB()
		{
			Assert.AreEqual("/* unknown */", MemberVisibility.Illegal.ForLanguage(SupportedLanguageId.Basic));
		}

		[TestMethod]
		public void ForLanguage_Unknown_CS()
		{
			Assert.AreEqual("/* unknown */", MemberVisibility.Illegal.ForLanguage(SupportedLanguageId.CSharp));
		}
	}
}
