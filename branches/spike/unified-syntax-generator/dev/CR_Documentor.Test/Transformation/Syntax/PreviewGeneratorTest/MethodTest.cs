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
	public class MethodTest
	{
		[TestMethod]
		public void ImplementsMultiple_Basic()
		{
			// Implements is a special VB keyword that appears when a method
			// explicitly implements an interface function. There is no analog
			// in C# since C# explicit interface implementation just modifies
			// the method signature.
			MethodProxy info = new MethodProxy("TestMethod")
			{
				MemberType = "String"
			};
			var param = new ParamProxy("param1")
			{
				ParamType = "String"
			};
			info.Parameters.Add(param.CreateFakeParam());
			info.Implements.Add("TestInterface.TestMethod1");
			info.Implements.Add("TestInterface.TestMethod2");
			info.Implements.Add("TestInterface.TestMethod3");
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Function</span> <span class=""identifier"">TestMethod</span><div class=""parameters"">
( _<div class=""parameter"">
<span class=""identifier"">param1</span> <span class=""keyword"">As</span> <a href=""#"">String</a> _
</div>)
</div> <span class=""keyword"">As</span> <a href=""#"">String</a> <span class=""keyword"">Implements</span> <a href=""#"">TestInterface.TestMethod1</a>, <a href=""#"">TestInterface.TestMethod2</a>, <a href=""#"">TestInterface.TestMethod3</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ImplementsSingle_Basic()
		{
			// Implements is a special VB keyword that appears when a method
			// explicitly implements an interface function. There is no analog
			// in C# since C# explicit interface implementation just modifies
			// the method signature.
			MethodProxy info = new MethodProxy("TestMethod")
			{
				MemberType = "String"
			};
			var param = new ParamProxy("param1")
			{
				ParamType = "String"
			};
			info.Parameters.Add(param.CreateFakeParam());
			info.Implements.Add("TestInterface.TestMethod");
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Function</span> <span class=""identifier"">TestMethod</span><div class=""parameters"">
( _<div class=""parameter"">
<span class=""identifier"">param1</span> <span class=""keyword"">As</span> <a href=""#"">String</a> _
</div>)
</div> <span class=""keyword"">As</span> <a href=""#"">String</a> <span class=""keyword"">Implements</span> <a href=""#"">TestInterface.TestMethod</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SingleParameter_Basic()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				MemberType = "String"
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
<span class=""keyword"">Public</span> <span class=""keyword"">Function</span> <span class=""identifier"">TestMethod</span><div class=""parameters"">
( _<div class=""parameter"">
<span class=""identifier"">param1</span> <span class=""keyword"">As</span> <a href=""#"">String</a> _
</div>)
</div> <span class=""keyword"">As</span> <a href=""#"">String</a>
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
				MemberType = "string"
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
<span class=""keyword"">public</span> <a href=""#"">string</a> <span class=""identifier"">TestMethod</span><div class=""parameters"">
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
		public void SimpleReturnValue_Basic()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				MemberType = "String"
			};
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Function</span> <span class=""identifier"">TestMethod</span> <span class=""keyword"">As</span> <a href=""#"">String</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SimpleReturnValue_CSharp()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				MemberType = "string"
			};
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <a href=""#"">string</a> <span class=""identifier"">TestMethod</span><div class=""parameters"">
()
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SimpleVoid_Basic()
		{
			MethodProxy info = new MethodProxy("TestMethod");
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Sub</span> <span class=""identifier"">TestMethod</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void SimpleVoid_CSharp()
		{
			MethodProxy info = new MethodProxy("TestMethod");
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">void</span> <span class=""identifier"">TestMethod</span><div class=""parameters"">
()
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
