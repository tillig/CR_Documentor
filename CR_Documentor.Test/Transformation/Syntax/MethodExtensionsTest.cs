using System;
using CR_Documentor.Test.Transformation.Syntax.Proxies;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax
{
	[TestClass]
	[Isolated]
	public class MethodExtensionsTest
	{
		[TestMethod]
		public void Addition()
		{
			var param1 = new ParamProxy("a")
			{
				ParamType = "Int32"
			};
			var param2 = new ParamProxy("b")
			{
				ParamType = "Int32"
			};
			var info = new MethodProxy("+")
			{
				IsClassOperator = true,
				MemberType = "Int32"
			};
			info.Parameters.Add(param1.CreateFakeParam());
			info.Parameters.Add(param2.CreateFakeParam());
			var method = info.CreateFakeMethod();
			var name = method.DisplayName();
			Assert.AreEqual("Addition", name);
		}

		[TestMethod]
		public void AddressOf()
		{
			var param = new ParamProxy("a")
			{
				ParamType = "Int32"
			};
			var info = new MethodProxy("&")
			{
				IsClassOperator = true,
				MemberType = "Int32"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var method = info.CreateFakeMethod();
			var name = method.DisplayName();
			Assert.AreEqual("Address Of", name);
		}

		[TestMethod]
		public void BitwiseAnd()
		{
			var param1 = new ParamProxy("a")
			{
				ParamType = "Int32"
			};
			var param2 = new ParamProxy("b")
			{
				ParamType = "Int32"
			};
			var info = new MethodProxy("&")
			{
				IsClassOperator = true,
				MemberType = "Int32"
			};
			info.Parameters.Add(param1.CreateFakeParam());
			info.Parameters.Add(param2.CreateFakeParam());
			var method = info.CreateFakeMethod();
			var name = method.DisplayName();
			Assert.AreEqual("Bitwise And", name);
		}

		[TestMethod]
		public void ExplicitCast()
		{
			var param = new ParamProxy("a")
			{
				ParamType = "Source"
			};
			var info = new MethodProxy("TestMethod")
			{
				IsClassOperator = true,
				IsExplicitCast = true,
				MemberType = "Destination"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var method = info.CreateFakeMethod();
			var name = method.DisplayName();
			Assert.AreEqual("Source to Destination Conversion", name);
		}

		[TestMethod]
		public void ImplicitCast()
		{
			var param = new ParamProxy("a")
			{
				ParamType = "Source"
			};
			var info = new MethodProxy("TestMethod")
			{
				IsClassOperator = true,
				IsImplicitCast = true,
				MemberType = "Destination"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var method = info.CreateFakeMethod();
			var name = method.DisplayName();
			Assert.AreEqual("Source to Destination Conversion", name);
		}

		[TestMethod]
		public void StandardClassOperator()
		{
			var info = new MethodProxy("=")
			{
				IsClassOperator = true,
			};
			var method = info.CreateFakeMethod();
			var name = method.DisplayName();
			Assert.AreEqual("Assignment", name);
		}

		[TestMethod]
		public void StandardMethod()
		{
			var info = new MethodProxy("TestMethod");
			var method = info.CreateFakeMethod();
			var name = method.DisplayName();
			Assert.AreEqual("TestMethod", name);
		}

		[TestMethod]
		public void Subtraction()
		{
			var param1 = new ParamProxy("a")
			{
				ParamType = "Int32"
			};
			var param2 = new ParamProxy("b")
			{
				ParamType = "Int32"
			};
			var info = new MethodProxy("-")
			{
				IsClassOperator = true,
				MemberType = "Int32"
			};
			info.Parameters.Add(param1.CreateFakeParam());
			info.Parameters.Add(param2.CreateFakeParam());
			var method = info.CreateFakeMethod();
			var name = method.DisplayName();
			Assert.AreEqual("Subtraction", name);
		}

		[TestMethod]
		public void UnaryNegation()
		{
			var param = new ParamProxy("a")
			{
				ParamType = "Int32"
			};
			var info = new MethodProxy("-")
			{
				IsClassOperator = true,
				MemberType = "Int32"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var method = info.CreateFakeMethod();
			var name = method.DisplayName();
			Assert.AreEqual("Unary Negation", name);
		}

		[TestMethod]
		public void UnaryPlus()
		{
			var param = new ParamProxy("a")
			{
				ParamType = "Int32"
			};
			var info = new MethodProxy("+")
			{
				IsClassOperator = true,
				MemberType = "Int32"
			};
			info.Parameters.Add(param.CreateFakeParam());
			var method = info.CreateFakeMethod();
			var name = method.DisplayName();
			Assert.AreEqual("Unary Plus", name);
		}

		[TestMethod]
		[Ignore]
		public void VisualBasicConstructor()
		{
			// Create a class with a constructor that is in a VB document.
			var info = new MethodProxy("Ignored");
			info.IsConstructor = true;
			var method = info.CreateFakeMethod();

			var classInfo = new ClassProxy("ParentClass");
			var parent = classInfo.CreateFakeClass();

			// Test ignored because TypeMock pitches a NullReferenceException here.
			Isolate.WhenCalled(() => method.Parent).WillReturn(parent);

			var document = Isolate.Fake.Instance<IDocument>();
			Isolate.WhenCalled(() => document.Language).WillReturn(Language.Basic);
			Isolate.WhenCalled(() => method.Document).WillReturn(document);

			// The name of the VB constructor should be the name of the parent class.
			var name = method.DisplayName();
			Assert.AreEqual("ParentClass", name);
		}
	}
}
