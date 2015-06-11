using System;
using CR_Documentor.Test.Transformation.Syntax.Proxies;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax.SimpleSignatureGeneratorTest
{
	[TestClass]
	[Isolated]
	public class ElementTest
	{
		[TestMethod]
		public void Class_Generic1()
		{
			ClassProxy info = new ClassProxy("TestClass");
			var parameters = new TypeParameterCollection();
			parameters.Add(new TypeParameterProxy("T").CreateFakeTypeParameter());
			info.SetTypeParameters(parameters);
			var element = info.CreateFakeClass();
			this.AssertVbAndCSharp("TestClass(Of T)", "TestClass<T>", element);
		}

		[TestMethod]
		public void Class_Generic3()
		{
			ClassProxy info = new ClassProxy("TestClass");
			var parameters = new TypeParameterCollection();
			parameters.Add(new TypeParameterProxy("T").CreateFakeTypeParameter());
			parameters.Add(new TypeParameterProxy("U").CreateFakeTypeParameter());
			parameters.Add(new TypeParameterProxy("V").CreateFakeTypeParameter());
			info.SetTypeParameters(parameters);
			var element = info.CreateFakeClass();
			this.AssertVbAndCSharp("TestClass(Of T, U, V)", "TestClass<T, U, V>", element);
		}

		[TestMethod]
		public void Class_NameOnlyWhenNotGeneric()
		{
			ClassProxy info = new ClassProxy("TestClass")
			{
				Visibility = MemberVisibility.ProtectedInternal
			};
			var element = info.CreateFakeClass();
			this.AssertVbAndCSharp("TestClass", element);
		}

		[TestMethod]
		public void Constructor_MethodStyle()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				IsConstructor = true
			};
			var element = info.CreateFakeMethod();
			this.AssertVbAndCSharp("TestMethod", "TestMethod()", element);
		}

		[TestMethod]
		public void Destructor_MethodStyle()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				IsDestructor = true
			};
			var element = info.CreateFakeMethod();
			this.AssertVbAndCSharp("TestMethod", "TestMethod()", element);
		}

		[TestMethod]
		public void Enumeration_NameOnly()
		{
			var element = Isolate.Fake.Instance<Enumeration>();
			Isolate.WhenCalled(() => element.Visibility).WillReturn(MemberVisibility.Public);
			Isolate.WhenCalled(() => element.Name).WillReturn("TestEnum");
			Isolate.WhenCalled(() => element.UnderlyingType).WillReturn("");
			Isolate.WhenCalled(() => element.AttributeCount).WillReturn(0);
			this.AssertVbAndCSharp("TestEnum", element);
		}

		[TestMethod]
		public void Event_NameOnly()
		{
			EventProxy info = new EventProxy("TestEvent")
			{
				MemberType = null,
			};
			info.Parameters.Add(new ParamProxy("a") { MemberType = "string" }.CreateFakeParam());
			ClassProxy parent = new ClassProxy("TestClass");
			info.Parent = parent.CreateFakeClass();
			var element = info.CreateFakeEvent();
			this.AssertVbAndCSharp("TestEvent", element);
		}

		[TestMethod]
		public void ExplicitCast_MethodStyle()
		{
			MethodProxy info = new MethodProxy("op_Explicit")
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
			this.AssertVbAndCSharp("op_Explicit(TestClass)", element);
		}

		[TestMethod]
		public void Field_NameOnly()
		{
			BaseVariableProxy info = new BaseVariableProxy("Field")
			{
				MemberType = "String"
			};
			var element = info.CreateFakeField();
			this.AssertVbAndCSharp("Field", element);
		}

		[TestMethod]
		public void ImplicitCast_MethodStyle()
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
			this.AssertVbAndCSharp("op_Implicit(TestClass)", element);
		}

		[TestMethod]
		public void Interface_Generic1()
		{
			InterfaceProxy info = new InterfaceProxy("TestInterface");
			var parameters = new TypeParameterCollection();
			parameters.Add(new TypeParameterProxy("T").CreateFakeTypeParameter());
			info.SetTypeParameters(parameters);
			var element = info.CreateFakeInterface();
			this.AssertVbAndCSharp("TestInterface(Of T)", "TestInterface<T>", element);
		}

		[TestMethod]
		public void Interface_Generic3()
		{
			InterfaceProxy info = new InterfaceProxy("TestInterface");
			var parameters = new TypeParameterCollection();
			parameters.Add(new TypeParameterProxy("T").CreateFakeTypeParameter());
			parameters.Add(new TypeParameterProxy("U").CreateFakeTypeParameter());
			parameters.Add(new TypeParameterProxy("V").CreateFakeTypeParameter());
			info.SetTypeParameters(parameters);
			var element = info.CreateFakeInterface();
			this.AssertVbAndCSharp("TestInterface(Of T, U, V)", "TestInterface<T, U, V>", element);
		}

		[TestMethod]
		public void Interface_NameOnlyWhenNotGeneric()
		{
			InterfaceProxy info = new InterfaceProxy("TestInterface");
			var element = info.CreateFakeInterface();
			this.AssertVbAndCSharp("TestInterface", element);
		}

		[TestMethod]
		public void Method_ExplicitInterfaceImplementation()
		{
			MethodProxy info = new MethodProxy("TestInterface.TestMethod");
			info.Implements.Add("TestInterface.TestMethod");
			var element = info.CreateFakeMethod();
			this.AssertVbAndCSharp("TestInterface.TestMethod", "TestInterface.TestMethod()", element);
		}

		[TestMethod]
		public void Method_Generic3()
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
			this.AssertVbAndCSharp("TestMethod(Of H, I, J)", "TestMethod<H, I, J>()", element);
		}

		[TestMethod]
		public void Method_InterfaceMember()
		{
			MethodProxy info = new MethodProxy("TestMethod");
			info.Parent = new InterfaceProxy("TestInterface").CreateFakeInterface();
			var element = info.CreateFakeMethod();
			this.AssertVbAndCSharp("TestMethod", "TestMethod()", element);
		}

		[TestMethod]
		public void Method_MultipleParameters()
		{
			MethodProxy info = new MethodProxy("TestMethod")
			{
				MemberType = "String"
			};
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
				ParamType = "Boolean"
			};
			info.Parameters.Add(param3.CreateFakeParam());
			var element = info.CreateFakeMethod();
			this.AssertVbAndCSharp("TestMethod(String, Int32, Boolean)", element);
		}

		[TestMethod]
		public void Method_SingleParameter()
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
			this.AssertVbAndCSharp("TestMethod(String)", element);
		}

		[TestMethod]
		public void Property_HasParameters()
		{
			PropertyProxy info = new PropertyProxy("Item")
			{
				MemberType = "String",
				HasGetter = true,
				HasSetter = true
			};
			ParamProxy param = new ParamProxy("index")
			{
				ParamType = "Integer"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var element = info.CreateFakeProperty();
			this.AssertVbAndCSharp("Item(Integer)", "Item[Integer]", element);
		}

		[TestMethod]
		public void Property_NoParameters()
		{
			PropertyProxy info = new PropertyProxy("TestProperty")
			{
				MemberType = "string",
				HasGetter = true,
				HasSetter = true
			};
			var element = info.CreateFakeProperty();
			this.AssertVbAndCSharp("TestProperty", element);
		}

		[TestMethod]
		public void Struct_NameOnly()
		{
			StructProxy info = new StructProxy("TestStruct")
			{
				Visibility = MemberVisibility.ProtectedInternal
			};
			var element = info.CreateFakeStruct();
			this.AssertVbAndCSharp("TestStruct", element);
		}

		[TestMethod]
		public void UnaryNegation_MethodStyle()
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
			this.AssertVbAndCSharp("op_UnaryNegation(TestClass)", element);
		}

		private void AssertVbAndCSharp(string expected, AccessSpecifiedElement element)
		{
			this.AssertVbAndCSharp(expected, expected, element);
		}

		private void AssertVbAndCSharp(string expectedVB, string expectedCSharp, AccessSpecifiedElement element)
		{
			var vbGenerator = new SimpleSignatureGenerator(element, SupportedLanguageId.Basic);
			var csGenerator = new SimpleSignatureGenerator(element, SupportedLanguageId.CSharp);
			string vb = vbGenerator.Generate();
			string cs = csGenerator.Generate();
			Assert.AreEqual(expectedVB, vb, "VB signature generation failed.");
			Assert.AreEqual(expectedCSharp, cs, "C# signature generation failed.");
		}
	}
}
