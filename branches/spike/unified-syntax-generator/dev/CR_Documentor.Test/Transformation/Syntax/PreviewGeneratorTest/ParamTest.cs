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
	public class ParamTest
	{
		[TestMethod]
		public void Parameter_0_Basic()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate");
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Delegate</span> <span class=""keyword"">Sub</span> <span class=""identifier"">TestDelegate</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Parameter_0_CSharp()
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

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Parameter_1_Basic()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate");
			var param = new ParamProxy("param1")
			{
				ParamType = "String"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Delegate</span> <span class=""keyword"">Sub</span> <span class=""identifier"">TestDelegate</span><div class=""parameters"">
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
		public void Parameter_1_CSharp()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate");
			var param = new ParamProxy("param1")
			{
				ParamType = "string"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">delegate</span> <span class=""keyword"">void</span> <span class=""identifier"">TestDelegate</span><div class=""parameters"">
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

		[TestMethod]
		public void Parameter_3_Basic()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate");
			var param1 = new ParamProxy("param1")
			{
				ParamType = "String"
			};
			info.Parameters.Add(param1.CreateFakeParam());
			var param2 = new ParamProxy("param2")
			{
				ParamType = "Int32"
			};
			info.Parameters.Add(param2.CreateFakeParam());
			var param3 = new ParamProxy("param3")
			{
				ParamType = "Int64"
			};
			info.Parameters.Add(param3.CreateFakeParam());
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Delegate</span> <span class=""keyword"">Sub</span> <span class=""identifier"">TestDelegate</span><div class=""parameters"">
( _<div class=""parameter"">
<span class=""identifier"">param1</span> <span class=""keyword"">As</span> <a href=""#"">String</a>, _
</div><div class=""parameter"">
<span class=""identifier"">param2</span> <span class=""keyword"">As</span> <a href=""#"">Int32</a>, _
</div><div class=""parameter"">
<span class=""identifier"">param3</span> <span class=""keyword"">As</span> <a href=""#"">Int64</a> _
</div>)
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Parameter_3_CSharp()
		{
			DelegateProxy info = new DelegateProxy("TestDelegate");
			var param1 = new ParamProxy("param1")
			{
				ParamType = "string"
			};
			info.Parameters.Add(param1.CreateFakeParam());
			var param2 = new ParamProxy("param2")
			{
				ParamType = "int"
			};
			info.Parameters.Add(param2.CreateFakeParam());
			var param3 = new ParamProxy("param3")
			{
				ParamType = "long"
			};
			info.Parameters.Add(param3.CreateFakeParam());
			var element = info.CreateFakeDelegate();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">delegate</span> <span class=""keyword"">void</span> <span class=""identifier"">TestDelegate</span><div class=""parameters"">
(<div class=""parameter"">
<a href=""#"">string</a> <span class=""identifier"">param1</span>,
</div><div class=""parameter"">
<a href=""#"">int</a> <span class=""identifier"">param2</span>,
</div><div class=""parameter"">
<a href=""#"">long</a> <span class=""identifier"">param3</span>
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
