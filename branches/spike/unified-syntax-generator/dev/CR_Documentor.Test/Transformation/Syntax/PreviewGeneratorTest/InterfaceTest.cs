using System;
using CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest.Proxies;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest
{
	[TestClass]
	public class InterfaceTest
	{
		[TestMethod]
		public void Simple_Basic()
		{
			InterfaceProxy info = new InterfaceProxy("TestInterface")
			{
				Visibility = MemberVisibility.ProtectedInternal
			};
			var element = info.CreateFakeInterface();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Protected Friend</span> <span class=""keyword"">Interface</span> <span class=""identifier"">TestInterface</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Simple_CSharp()
		{
			InterfaceProxy info = new InterfaceProxy("TestInterface")
			{
				Visibility = MemberVisibility.ProtectedInternal
			};
			var element = info.CreateFakeInterface();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">protected internal</span> <span class=""keyword"">interface</span> <span class=""identifier"">TestInterface</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
