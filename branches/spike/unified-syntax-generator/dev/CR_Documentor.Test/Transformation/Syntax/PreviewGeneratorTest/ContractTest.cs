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
	public class ContractTest
	{
		[TestMethod]
		public void Abstract_Class_Basic()
		{
			ClassProxy info = new ClassProxy("TestClass")
			{
				IsAbstract = true
			};
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">MustInherit</span> <span class=""keyword"">Class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Abstract_Class_CSharp()
		{
			ClassProxy info = new ClassProxy("TestClass")
			{
				IsAbstract = true
			};
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">abstract</span> <span class=""keyword"">class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Constant_Field_Basic()
		{
			BaseVariableProxy info = new BaseVariableProxy("Field")
			{
				MemberType = "String",
				IsConst = true
			};
			var element = info.CreateFakeField();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Const</span> <span class=""identifier"">Field</span> <span class=""keyword"">As</span> <a href=""#"">String</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Constant_Field_CSharp()
		{
			BaseVariableProxy info = new BaseVariableProxy("Field")
			{
				MemberType = "string",
				IsConst = true
			};
			var element = info.CreateFakeField();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">const</span> <a href=""#"">string</a> <span class=""identifier"">Field</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void New_Class_Basic()
		{
			ClassProxy info = new ClassProxy("TestClass")
			{
				IsNew = true
			};
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Shadows</span> <span class=""keyword"">Class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void New_Class_CSharp()
		{
			ClassProxy info = new ClassProxy("TestClass")
			{
				IsNew = true
			};
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">new</span> <span class=""keyword"">public</span> <span class=""keyword"">class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Sealed_Class_Basic()
		{
			ClassProxy info = new ClassProxy("TestClass")
			{
				IsSealed = true
			};
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">NotInheritable</span> <span class=""keyword"">Class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Sealed_Class_CSharp()
		{
			ClassProxy info = new ClassProxy("TestClass")
			{
				IsSealed = true
			};
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">sealed</span> <span class=""keyword"">class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Static_Class_Basic()
		{
			ClassProxy info = new ClassProxy("TestClass")
			{
				IsStatic = true
			};
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Shared</span> <span class=""keyword"">Class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Static_Class_CSharp()
		{
			ClassProxy info = new ClassProxy("TestClass")
			{
				IsStatic = true
			};
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">static</span> <span class=""keyword"">class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void StaticReadonly_Field_Basic()
		{
			BaseVariableProxy info = new BaseVariableProxy("Field")
			{
				MemberType = "String",
				IsStatic = true,
				IsReadOnly = true
			};
			var element = info.CreateFakeField();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Shared</span> <span class=""keyword"">ReadOnly</span> <span class=""identifier"">Field</span> <span class=""keyword"">As</span> <a href=""#"">String</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void StaticReadonly_Field_CSharp()
		{
			BaseVariableProxy info = new BaseVariableProxy("Field")
			{
				MemberType = "string",
				IsStatic = true,
				IsReadOnly = true
			};
			var element = info.CreateFakeField();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">static</span> <span class=""keyword"">readonly</span> <a href=""#"">string</a> <span class=""identifier"">Field</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Virtual_Basic()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				MemberType = "String",
				IsVirtual = true
			};
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Overridable</span> <span class=""keyword"">Function</span> <span class=""identifier"">TestMethod</span> <span class=""keyword"">As</span> <a href=""#"">String</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Virtual_CSharp()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				MemberType = "string",
				IsVirtual = true
			};
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">virtual</span> <a href=""#"">string</a> <span class=""identifier"">TestMethod</span><div class=""parameters"">
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
