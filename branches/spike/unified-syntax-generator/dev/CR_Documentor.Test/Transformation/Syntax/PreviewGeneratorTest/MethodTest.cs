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
		public void Generic_Constraint_3_Basic()
		{
			MethodProxy info = new MethodProxy("TestMethod");
			var parameters = new TypeParameterCollection();
			parameters.Add(new TypeParameterProxy("H")
			{
				Constraints = new TypeParameterConstraintCollection()
					{
						new NamedTypeParameterConstraintProxy("Attribute").CreateFakeTypeParameter(),
						new NamedTypeParameterConstraintProxy("ISerializable").CreateFakeTypeParameter()
					}
			}.CreateFakeTypeParameter());
			parameters.Add(new TypeParameterProxy("I")
			{
				Constraints = new TypeParameterConstraintCollection()
					{
						new TypeParameterConstraintProxy("Structure").CreateFakeTypeParameter()
					}
			}.CreateFakeTypeParameter());
			parameters.Add(new TypeParameterProxy("J")
			{
				Constraints = new TypeParameterConstraintCollection()
					{
						new TypeParameterConstraintProxy("Class").CreateFakeTypeParameter(),
						new NamedTypeParameterConstraintProxy("IList").CreateFakeTypeParameter(),
						new NamedTypeParameterConstraintProxy("ICollection").CreateFakeTypeParameter(),
						new TypeParameterConstraintProxy("New").CreateFakeTypeParameter()
					}
			}.CreateFakeTypeParameter());
			info.SetTypeParameters(parameters);
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Sub</span> <span class=""identifier"">TestMethod</span><div class=""typeparameters"">
(<span class=""keyword"">Of</span> <span class=""typeparameter"">H</span><div class=""constraints"">
&nbsp;<span class=""keyword"">As</span> {<div class=""constraint"">
<a href=""#"">Attribute</a>, <a href=""#"">ISerializable</a>
</div>}
</div>, <span class=""typeparameter"">I</span><div class=""constraints"">
&nbsp;<span class=""keyword"">As</span> <div class=""constraint"">
<span class=""keyword"">Structure</span>
</div>
</div>, <span class=""typeparameter"">J</span><div class=""constraints"">
&nbsp;<span class=""keyword"">As</span> {<div class=""constraint"">
<span class=""keyword"">Class</span>, <a href=""#"">IList</a>, <a href=""#"">ICollection</a>, <span class=""keyword"">New</span>
</div>}
</div>)
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Generic_Constraint_3_CSharp()
		{
			MethodProxy info = new MethodProxy("TestMethod");
			var parameters = new TypeParameterCollection();
			parameters.Add(new TypeParameterProxy("H")
			{
				Constraints = new TypeParameterConstraintCollection()
					{
						new NamedTypeParameterConstraintProxy("Attribute").CreateFakeTypeParameter(),
						new NamedTypeParameterConstraintProxy("ISerializable").CreateFakeTypeParameter()
					}
			}.CreateFakeTypeParameter());
			parameters.Add(new TypeParameterProxy("I")
			{
				Constraints = new TypeParameterConstraintCollection()
					{
						new TypeParameterConstraintProxy("struct").CreateFakeTypeParameter()
					}
			}.CreateFakeTypeParameter());
			parameters.Add(new TypeParameterProxy("J")
			{
				Constraints = new TypeParameterConstraintCollection()
					{
						new TypeParameterConstraintProxy("class").CreateFakeTypeParameter(),
						new NamedTypeParameterConstraintProxy("IList").CreateFakeTypeParameter(),
						new NamedTypeParameterConstraintProxy("ICollection").CreateFakeTypeParameter(),
						new TypeParameterConstraintProxy("new()").CreateFakeTypeParameter()
					}
			}.CreateFakeTypeParameter());
			info.SetTypeParameters(parameters);
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">void</span> <span class=""identifier"">TestMethod</span><div class=""typeparameters"">
&lt;<span class=""typeparameter"">H</span>, <span class=""typeparameter"">I</span>, <span class=""typeparameter"">J</span>&gt;
</div><div class=""parameters"">
()
</div><div class=""constraints"">
<div class=""constraint"">
<span class=""keyword"">where</span> <span class=""typeparameter"">H</span> : <a href=""#"">Attribute</a>, <a href=""#"">ISerializable</a>
</div><div class=""constraint"">
<span class=""keyword"">where</span> <span class=""typeparameter"">I</span> : <span class=""keyword"">struct</span>
</div><div class=""constraint"">
<span class=""keyword"">where</span> <span class=""typeparameter"">J</span> : <span class=""keyword"">class</span>, <a href=""#"">IList</a>, <a href=""#"">ICollection</a>, <span class=""keyword"">new()</span>
</div>
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

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
</div>&nbsp;<span class=""keyword"">As</span> <a href=""#"">String</a> <span class=""keyword"">Implements</span> <a href=""#"">TestInterface.TestMethod1</a>, <a href=""#"">TestInterface.TestMethod2</a>, <a href=""#"">TestInterface.TestMethod3</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
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
</div>&nbsp;<span class=""keyword"">As</span> <a href=""#"">String</a> <span class=""keyword"">Implements</span> <a href=""#"">TestInterface.TestMethod</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
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

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
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
<span class=""keyword"">Public</span> <span class=""keyword"">Function</span> <span class=""identifier"">TestMethod</span>&nbsp;<span class=""keyword"">As</span> <a href=""#"">String</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
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

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
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

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
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

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void StaticReturnValue_Basic()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				MemberType = "String",
				IsStatic = true
			};
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Shared</span> <span class=""keyword"">Function</span> <span class=""identifier"">TestMethod</span>&nbsp;<span class=""keyword"">As</span> <a href=""#"">String</a>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic, true);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void StaticReturnValue_CSharp()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				MemberType = "string",
				IsStatic = true
			};
			var element = info.CreateFakeMethod();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">static</span> <a href=""#"">string</a> <span class=""identifier"">TestMethod</span><div class=""parameters"">
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
