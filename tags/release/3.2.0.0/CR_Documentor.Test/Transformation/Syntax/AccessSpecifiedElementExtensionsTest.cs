using System;
using CR_Documentor.Transformation.Syntax;
using DevExpress.CodeRush.StructuralParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Transformation.Syntax
{
	[TestClass]
	[Isolated]
	public class AccessSpecifiedElementExtensionsTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ElementTypeDescription_NullElement()
		{
			AccessSpecifiedElement element = null;
			element.ElementTypeDescription();
		}

		[TestMethod]
		public void ElementTypeDescription_Class()
		{
			var element = Isolate.Fake.Instance<Class>();
			Assert.AreEqual("Class", element.ElementTypeDescription());
		}

		[TestMethod]
		public void ElementTypeDescription_DelegateDefinition()
		{
			var element = Isolate.Fake.Instance<DelegateDefinition>();
			Assert.AreEqual("Delegate", element.ElementTypeDescription());
		}

		[TestMethod]
		public void ElementTypeDescription_EnumElement()
		{
			var element = Isolate.Fake.Instance<EnumElement>();
			Assert.AreEqual("Field", element.ElementTypeDescription());
		}

		[TestMethod]
		public void ElementTypeDescription_Enumeration()
		{
			var element = Isolate.Fake.Instance<Enumeration>();
			Assert.AreEqual("Enumeration", element.ElementTypeDescription());
		}

		[TestMethod]
		public void ElementTypeDescription_Event()
		{
			var element = Isolate.Fake.Instance<Event>();
			Assert.AreEqual("Event", element.ElementTypeDescription());
		}

		[TestMethod]
		public void ElementTypeDescription_Interface()
		{
			var element = Isolate.Fake.Instance<Interface>();
			Assert.AreEqual("Interface", element.ElementTypeDescription());
		}

		[TestMethod]
		public void ElementTypeDescription_MethodClassOperator()
		{
			var element = Isolate.Fake.Instance<Method>();
			Isolate.WhenCalled(() => element.IsClassOperator).WillReturn(true);
			Assert.AreEqual("Operator", element.ElementTypeDescription());
		}

		[TestMethod]
		public void ElementTypeDescription_MethodConstructor()
		{
			var element = Isolate.Fake.Instance<Method>();
			Isolate.WhenCalled(() => element.IsConstructor).WillReturn(true);
			Assert.AreEqual("Constructor", element.ElementTypeDescription());
		}

		[TestMethod]
		public void ElementTypeDescription_Method()
		{
			var element = Isolate.Fake.Instance<Method>();
			Assert.AreEqual("Method", element.ElementTypeDescription());
		}

		[TestMethod]
		public void ElementTypeDescription_Other()
		{
			// SnippetCodeMember isn't specifically documented or handled
			// so should always default to "Member."
			var element = Isolate.Fake.Instance<SnippetCodeMember>();
			Assert.AreEqual("Member", element.ElementTypeDescription());
		}

		[TestMethod]
		public void ElementTypeDescription_Param()
		{
			var element = Isolate.Fake.Instance<Param>();
			Assert.AreEqual("Field", element.ElementTypeDescription());
		}

		[TestMethod]
		public void ElementTypeDescription_Property()
		{
			var element = Isolate.Fake.Instance<Property>();
			Assert.AreEqual("Property", element.ElementTypeDescription());
		}

		[TestMethod]
		public void ElementTypeDescription_Struct()
		{
			var element = Isolate.Fake.Instance<Struct>();
			Assert.AreEqual("Structure", element.ElementTypeDescription());
		}

		[TestMethod]
		public void ElementTypeDescription_Variable()
		{
			var element = Isolate.Fake.Instance<Variable>();
			Assert.AreEqual("Field", element.ElementTypeDescription());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void HasGenericParameters_NullElement()
		{
			AccessSpecifiedElement element = null;
			element.HasGenericParameters();
		}

		[TestMethod]
		public void HasGenericParameters_GenericWithEmptyParameterCollection()
		{
			var element = Isolate.Fake.Instance<AccessSpecifiedElement>();
			Isolate.WhenCalled(() => element.IsGeneric).WillReturn(true);
			var param = new TypeParameterCollection();
			Isolate.WhenCalled(() => element.GenericModifier.TypeParameters).WillReturn(param);
			Assert.IsFalse(element.HasGenericParameters());
		}

		[TestMethod]
		public void HasGenericParameters_GenericWithNullParameterCollection()
		{
			var element = Isolate.Fake.Instance<AccessSpecifiedElement>();
			Isolate.WhenCalled(() => element.IsGeneric).WillReturn(true);
			Isolate.WhenCalled(() => element.GenericModifier.TypeParameters).WillReturn(null);
			Assert.IsFalse(element.HasGenericParameters());
		}

		[TestMethod]
		public void HasGenericParameters_GenericWithParameterCollection()
		{
			var element = Isolate.Fake.Instance<AccessSpecifiedElement>();
			Isolate.WhenCalled(() => element.IsGeneric).WillReturn(true);
			var paramColl = new TypeParameterCollection();
			var param = Isolate.Fake.Instance<TypeParameter>();
			paramColl.Add(param);
			Isolate.WhenCalled(() => element.GenericModifier.TypeParameters).WillReturn(paramColl);
			Assert.IsTrue(element.HasGenericParameters());
		}

		[TestMethod]
		public void HasGenericParameters_NotGeneric()
		{
			var element = Isolate.Fake.Instance<AccessSpecifiedElement>();
			Isolate.WhenCalled(() => element.IsGeneric).WillReturn(false);
			Assert.IsFalse(element.HasGenericParameters());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsSupportedForPreview_NullElement()
		{
			AccessSpecifiedElement element = null;
			element.IsSupportedForPreview();
		}

		[TestMethod]
		public void IsSupportedForPreview_Class()
		{
			var element = Isolate.Fake.Instance<Class>();
			Assert.IsTrue(element.IsSupportedForPreview());
		}

		[TestMethod]
		public void IsSupportedForPreview_DelegateDefinition()
		{
			var element = Isolate.Fake.Instance<DelegateDefinition>();
			Assert.IsTrue(element.IsSupportedForPreview());
		}

		[TestMethod]
		public void IsSupportedForPreview_EnumElement()
		{
			// Enumeration elements don't get their own preview -
			// jut the enum itself.
			var element = Isolate.Fake.Instance<EnumElement>();
			Assert.IsFalse(element.IsSupportedForPreview());
		}

		[TestMethod]
		public void IsSupportedForPreview_Enumeration()
		{
			var element = Isolate.Fake.Instance<Enumeration>();
			Assert.IsTrue(element.IsSupportedForPreview());
		}

		[TestMethod]
		public void IsSupportedForPreview_Event()
		{
			var element = Isolate.Fake.Instance<Event>();
			Assert.IsTrue(element.IsSupportedForPreview());
		}

		[TestMethod]
		public void IsSupportedForPreview_Interface()
		{
			var element = Isolate.Fake.Instance<Interface>();
			Assert.IsTrue(element.IsSupportedForPreview());
		}

		[TestMethod]
		public void IsSupportedForPreview_MethodClassOperator()
		{
			var element = Isolate.Fake.Instance<Method>();
			Isolate.WhenCalled(() => element.IsClassOperator).WillReturn(true);
			Assert.IsTrue(element.IsSupportedForPreview());
		}

		[TestMethod]
		public void IsSupportedForPreview_MethodConstructor()
		{
			var element = Isolate.Fake.Instance<Method>();
			Isolate.WhenCalled(() => element.IsConstructor).WillReturn(true);
			Assert.IsTrue(element.IsSupportedForPreview());
		}

		[TestMethod]
		public void IsSupportedForPreview_Method()
		{
			var element = Isolate.Fake.Instance<Method>();
			Assert.IsTrue(element.IsSupportedForPreview());
		}

		[TestMethod]
		public void IsSupportedForPreview_Other()
		{
			// SnippetCodeMember isn't specifically documented or handled
			// so should always be false.
			var element = Isolate.Fake.Instance<SnippetCodeMember>();
			Assert.IsFalse(element.IsSupportedForPreview());
		}

		[TestMethod]
		public void IsSupportedForPreview_Param()
		{
			var element = Isolate.Fake.Instance<Param>();
			Assert.IsTrue(element.IsSupportedForPreview());
		}

		[TestMethod]
		public void IsSupportedForPreview_Property()
		{
			var element = Isolate.Fake.Instance<Property>();
			Assert.IsTrue(element.IsSupportedForPreview());
		}

		[TestMethod]
		public void IsSupportedForPreview_Struct()
		{
			var element = Isolate.Fake.Instance<Struct>();
			Assert.IsTrue(element.IsSupportedForPreview());
		}

		[TestMethod]
		public void IsSupportedForPreview_Variable()
		{
			var element = Isolate.Fake.Instance<Variable>();
			Assert.IsTrue(element.IsSupportedForPreview());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void MemberType_NullElement()
		{
			AccessSpecifiedElement element = null;
			element.MemberType();
		}

		[TestMethod]
		public void MemberType_Class()
		{
			var element = Isolate.Fake.Instance<Class>();
			Assert.IsNull(element.MemberType());
		}

		[TestMethod]
		public void MemberType_EnumElement()
		{
			var element = Isolate.Fake.Instance<EnumElement>();
			Isolate.WhenCalled(() => element.MemberType).WillReturn("expected");
			Assert.AreEqual("expected", element.MemberType());
		}
	}
}
