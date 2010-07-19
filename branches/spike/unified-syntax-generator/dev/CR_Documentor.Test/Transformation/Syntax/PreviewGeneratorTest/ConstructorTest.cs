using System;
using CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest.Proxies;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.PreviewGeneratorTest
{
	[TestClass]
	[Isolated]
	public class ConstructorTest
	{
		[TestMethod]
		public void Default_Basic()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				IsConstructor = true
			};
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Sub</span> <span class=""identifier"">New</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Default_CSharp()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				IsConstructor = true
			};
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""identifier"">TestMethod</span><div class=""parameters"">
()
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SingleParameter_Basic()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				IsConstructor = true
			};
			var param = new ParamProxy("param1")
			{
				ParamType = "String"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Sub</span> <span class=""identifier"">New</span><div class=""parameters"">
( _<div class=""parameter"">
<span class=""identifier"">param1</span> <span class=""keyword"">As</span> <a href=""#"">String</a> _
</div>)
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SingleParameter_CSharp()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				IsConstructor = true
			};
			var param = new ParamProxy("param1")
			{
				ParamType = "string"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""identifier"">TestMethod</span><div class=""parameters"">
(<div class=""parameter"">
<a href=""#"">string</a> <span class=""identifier"">param1</span>
</div>)
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
