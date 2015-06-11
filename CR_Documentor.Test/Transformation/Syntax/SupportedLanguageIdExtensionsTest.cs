using System;
using CR_Documentor.Transformation.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test.Transformation.Syntax
{
	[TestClass]
	public class SupportedLanguageIdExtensionsTest
	{
		[TestMethod]
		public void Basic()
		{
			var lang = "Basic";
			Assert.AreEqual(SupportedLanguageId.Basic, lang.ToLanguageId());
		}

		[TestMethod]
		public void CSharp()
		{
			var lang = "CSharp";
			Assert.AreEqual(SupportedLanguageId.CSharp, lang.ToLanguageId());
		}

		[TestMethod]
		public void Empty()
		{
			var lang = "";
			Assert.AreEqual(SupportedLanguageId.None, lang.ToLanguageId());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Null()
		{
			string lang = null;
			lang.ToLanguageId();
		}

		[TestMethod]
		public void Unrecognized()
		{
			var lang = "FSharp";
			Assert.AreEqual(SupportedLanguageId.None, lang.ToLanguageId());
		}
	}
}
