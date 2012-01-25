using System;
using CR_Documentor.Test.Transformation.Syntax.Proxies;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest
{
	[TestClass]
	[Isolated]
	public class DestructorTest
	{
		[TestMethod]
		public void Default_Basic()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				IsDestructor = true
			};
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Protected</span> <span class=""keyword"">Overrides</span> <span class=""keyword"">Sub</span> <span class=""identifier"">Finalize</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Default_CSharp()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				IsDestructor = true
			};
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">protected</span> <span class=""keyword"">override</span> <span class=""keyword"">void</span> <span class=""identifier"">Finalize</span><div class=""parameters"">
()
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
