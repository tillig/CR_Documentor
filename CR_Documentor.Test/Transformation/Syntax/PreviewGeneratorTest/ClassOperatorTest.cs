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
	public class ClassOperatorTest
	{
		[TestMethod]
		public void ExplicitCast_Basic()
		{
			MethodProxy info = new MethodProxy("op_Implicit")
			{
				MemberType = "Integer",
				IsStatic = true,
				IsClassOperator = true,
				IsExplicitCast = true
			};
			var param = new ParamProxy("param1")
			{
				ParamType = "TestClass"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Shared</span> <span class=""keyword"">Narrowing</span> <span class=""keyword"">Operator</span> <span class=""identifier"">CType</span><div class=""parameters"">
( _<div class=""parameter"">
<span class=""identifier"">param1</span> <span class=""keyword"">As</span> <a href=""#"">TestClass</a> _
</div>)
</div>&nbsp;<span class=""keyword"">As</span> <a href=""#"">Integer</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ExplicitCast_CSharp()
		{
			MethodProxy info = new MethodProxy("op_Implicit")
			{
				MemberType = "int",
				IsStatic = true,
				IsClassOperator = true,
				IsExplicitCast = true
			};
			var param = new ParamProxy("param1")
			{
				ParamType = "TestClass"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">static</span> <span class=""keyword"">explicit</span> <span class=""keyword"">operator</span> <a href=""#"">int</a><div class=""parameters"">
(<div class=""parameter"">
<a href=""#"">TestClass</a> <span class=""identifier"">param1</span>
</div>)
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ImplicitCast_Basic()
		{
			MethodProxy info = new MethodProxy("op_Implicit")
			{
				MemberType = "Integer",
				IsStatic = true,
				IsClassOperator = true,
				IsImplicitCast = true
			};
			var param = new ParamProxy("param1")
			{
				ParamType = "TestClass"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Shared</span> <span class=""keyword"">Widening</span> <span class=""keyword"">Operator</span> <span class=""identifier"">CType</span><div class=""parameters"">
( _<div class=""parameter"">
<span class=""identifier"">param1</span> <span class=""keyword"">As</span> <a href=""#"">TestClass</a> _
</div>)
</div>&nbsp;<span class=""keyword"">As</span> <a href=""#"">Integer</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ImplicitCast_CSharp()
		{
			MethodProxy info = new MethodProxy("op_Implicit")
			{
				MemberType = "int",
				IsStatic = true,
				IsClassOperator = true,
				IsImplicitCast = true
			};
			var param = new ParamProxy("param1")
			{
				ParamType = "TestClass"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">static</span> <span class=""keyword"">implicit</span> <span class=""keyword"">operator</span> <a href=""#"">int</a><div class=""parameters"">
(<div class=""parameter"">
<a href=""#"">TestClass</a> <span class=""identifier"">param1</span>
</div>)
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void UnaryNegation_Basic()
		{
			MethodProxy info = new MethodProxy("op_UnaryNegation")
			{
				MemberType = "TestClass",
				IsStatic = true,
				IsClassOperator = true
			};
			var param = new ParamProxy("param1")
			{
				ParamType = "TestClass"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Shared</span> <span class=""keyword"">Operator</span> <span class=""identifier"">op_UnaryNegation</span><div class=""parameters"">
( _<div class=""parameter"">
<span class=""identifier"">param1</span> <span class=""keyword"">As</span> <a href=""#"">TestClass</a> _
</div>)
</div>&nbsp;<span class=""keyword"">As</span> <a href=""#"">TestClass</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void UnaryNegation_CSharp()
		{
			MethodProxy info = new MethodProxy("op_UnaryNegation")
			{
				MemberType = "TestClass",
				IsStatic = true,
				IsClassOperator = true
			};
			var param = new ParamProxy("param1")
			{
				ParamType = "TestClass"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">static</span> <a href=""#"">TestClass</a> <span class=""keyword"">operator</span> <span class=""identifier"">op_UnaryNegation</span><div class=""parameters"">
(<div class=""parameter"">
<a href=""#"">TestClass</a> <span class=""identifier"">param1</span>
</div>)
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}
	}
}
