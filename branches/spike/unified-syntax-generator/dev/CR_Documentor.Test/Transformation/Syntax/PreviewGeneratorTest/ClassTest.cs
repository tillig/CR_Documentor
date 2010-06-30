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
	public class ClassTest
	{
		[TestMethod]
		public void Abstract_Basic()
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
		public void Abstract_CSharp()
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
		public void Generic_1_Basic()
		{
			ClassProxy info = new ClassProxy("TestClass");
			var parameters = new TypeParameterCollection();
			parameters.Add(new TypeParameterProxy("T").CreateFakeTypeParameter());
			info.SetTypeParameters(parameters);
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Class</span> <span class=""identifier"">TestClass</span><div class=""typeparameters"">
(<span class=""keyword"">Of</span> <span class=""typeparameter"">T</span>)
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Generic_1_CSharp()
		{
			ClassProxy info = new ClassProxy("TestClass");
			var parameters = new TypeParameterCollection();
			parameters.Add(new TypeParameterProxy("T").CreateFakeTypeParameter());
			info.SetTypeParameters(parameters);
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">class</span> <span class=""identifier"">TestClass</span><div class=""typeparameters"">
&lt;<span class=""typeparameter"">T</span>&gt;
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Generic_3_Basic()
		{
			ClassProxy info = new ClassProxy("TestClass");
			var parameters = new TypeParameterCollection();
			parameters.Add(new TypeParameterProxy("T").CreateFakeTypeParameter());
			parameters.Add(new TypeParameterProxy("U").CreateFakeTypeParameter());
			parameters.Add(new TypeParameterProxy("V").CreateFakeTypeParameter());
			info.SetTypeParameters(parameters);
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Class</span> <span class=""identifier"">TestClass</span><div class=""typeparameters"">
(<span class=""keyword"">Of</span> <span class=""typeparameter"">T</span>, <span class=""typeparameter"">U</span>, <span class=""typeparameter"">V</span>)
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Generic_3_CSharp()
		{
			ClassProxy info = new ClassProxy("TestClass");
			var parameters = new TypeParameterCollection();
			parameters.Add(new TypeParameterProxy("T").CreateFakeTypeParameter());
			parameters.Add(new TypeParameterProxy("U").CreateFakeTypeParameter());
			parameters.Add(new TypeParameterProxy("V").CreateFakeTypeParameter());
			info.SetTypeParameters(parameters);
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">class</span> <span class=""identifier"">TestClass</span><div class=""typeparameters"">
&lt;<span class=""typeparameter"">T</span>, <span class=""typeparameter"">U</span>, <span class=""typeparameter"">V</span>&gt;
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Generic_Constraint_1_Basic()
		{
			ClassProxy info = new ClassProxy("TestClass");
			var parameters = new TypeParameterCollection();
			parameters.Add(new TypeParameterProxy("T")
			{
				Constraints = new TypeParameterConstraintCollection()
					{
						new TypeParameterConstraintProxy("New").CreateFakeTypeParameter()
					}
			}.CreateFakeTypeParameter());
			info.SetTypeParameters(parameters);
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Class</span> <span class=""identifier"">TestClass</span><div class=""typeparameters"">
(<span class=""keyword"">Of</span> <span class=""typeparameter"">T</span><div class=""constraints"">
 <span class=""keyword"">As</span> <div class=""constraint"">
<span class=""keyword"">New</span>
</div>
</div>)
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Generic_Constraint_1_CSharp()
		{
			ClassProxy info = new ClassProxy("TestClass");
			var parameters = new TypeParameterCollection();
			parameters.Add(new TypeParameterProxy("T")
				{
					Constraints = new TypeParameterConstraintCollection()
					{
						new TypeParameterConstraintProxy("new()").CreateFakeTypeParameter()
					}
				}.CreateFakeTypeParameter());
			info.SetTypeParameters(parameters);
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">class</span> <span class=""identifier"">TestClass</span><div class=""typeparameters"">
&lt;<span class=""typeparameter"">T</span>&gt;
</div><div class=""constraints"">
<div class=""constraint"">
<span class=""keyword"">where</span> <span class=""typeparameter"">T</span> : <span class=""keyword"">new()</span>
</div>
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Generic_Constraint_3_Basic()
		{
			ClassProxy info = new ClassProxy("TestClass");
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
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Class</span> <span class=""identifier"">TestClass</span><div class=""typeparameters"">
(<span class=""keyword"">Of</span> <span class=""typeparameter"">H</span><div class=""constraints"">
 <span class=""keyword"">As</span> {<div class=""constraint"">
<a href=""#"">Attribute</a>, <a href=""#"">ISerializable</a>
</div>}
</div>, <span class=""typeparameter"">I</span><div class=""constraints"">
 <span class=""keyword"">As</span> <div class=""constraint"">
<span class=""keyword"">Structure</span>
</div>
</div>, <span class=""typeparameter"">J</span><div class=""constraints"">
 <span class=""keyword"">As</span> {<div class=""constraint"">
<span class=""keyword"">Class</span>, <a href=""#"">IList</a>, <a href=""#"">ICollection</a>, <span class=""keyword"">New</span>
</div>}
</div>)
</div>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Generic_Constraint_3_CSharp()
		{
			ClassProxy info = new ClassProxy("TestClass");
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
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">public</span> <span class=""keyword"">class</span> <span class=""identifier"">TestClass</span><div class=""typeparameters"">
&lt;<span class=""typeparameter"">H</span>, <span class=""typeparameter"">I</span>, <span class=""typeparameter"">J</span>&gt;
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

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void New_Basic()
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
		public void New_CSharp()
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
		public void Sealed_Basic()
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
		public void Sealed_CSharp()
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
		public void Simple_Basic()
		{
			ClassProxy info = new ClassProxy("TestClass")
			{
				Visibility = MemberVisibility.ProtectedInternal
			};
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Protected Friend</span> <span class=""keyword"">Class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Simple_CSharp()
		{
			ClassProxy info = new ClassProxy("TestClass")
			{
				Visibility = MemberVisibility.ProtectedInternal
			};
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code cs"">
<div class=""member"">
<span class=""keyword"">protected internal</span> <span class=""keyword"">class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.CSharp);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Static_Basic()
		{
			ClassProxy info = new ClassProxy("TestClass")
			{
				IsStatic = true // There's no such thing as a static class in VB; only members are static.
			};
			var element = info.CreateFakeClass();

			string expected =
@"<div class=""code vb"">
<div class=""member"">
<span class=""keyword"">Public</span> <span class=""keyword"">Class</span> <span class=""identifier"">TestClass</span>
</div>
</div>";

			var generator = new PreviewGenerator(element, SupportedLanguageId.Basic);
			string actual = generator.Generate();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Static_CSharp()
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
	}
}
