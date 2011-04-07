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
	public class DelegateTest
	{
		[TestMethod]
		public void SingleParameter_Basic()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate")
			{
				MemberType = "String"
			};
			var param = new ParamProxy("param1")
			{
				ParamType = "String"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Delegate</span> <span class=""keyword"">Function</span> <span class=""identifier"">TestDelegate</span><div class=""parameters"">
( _<div class=""parameter"">
<span class=""identifier"">param1</span> <span class=""keyword"">As</span> <a href=""#"">String</a> _
</div>)
</div>&nbsp;<span class=""keyword"">As</span> <a href=""#"">String</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SingleParameter_CSharp()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate")
			{
				MemberType = "string"
			};
			var param = new ParamProxy("param1")
			{
				ParamType = "string"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">delegate</span> <a href=""#"">string</a> <span class=""identifier"">TestDelegate</span><div class=""parameters"">
(<div class=""parameter"">
<a href=""#"">string</a> <span class=""identifier"">param1</span>
</div>)
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SimpleReturnValue_Basic()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate")
			{
				MemberType = "String"
			};
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Delegate</span> <span class=""keyword"">Function</span> <span class=""identifier"">TestDelegate</span>&nbsp;<span class=""keyword"">As</span> <a href=""#"">String</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SimpleReturnValue_CSharp()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate")
			{
				MemberType = "string"
			};
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">delegate</span> <a href=""#"">string</a> <span class=""identifier"">TestDelegate</span><div class=""parameters"">
()
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SimpleVoid_Basic()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate");
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Delegate</span> <span class=""keyword"">Sub</span> <span class=""identifier"">TestDelegate</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SimpleVoid_CSharp()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate");
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">delegate</span> <span class=""keyword"">void</span> <span class=""identifier"">TestDelegate</span><div class=""parameters"">
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
