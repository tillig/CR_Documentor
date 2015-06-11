using System;
using CR_Documentor.Reflector;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test.Reflector
{
	[TestClass]
	public class TypeInformationTest
	{
		[TestMethod]
		public void GetResolutionScope_TypeNull()
		{
			Assert.AreEqual(String.Empty, TypeInformation.GetResolutionScope(null), "A null type should yield an empty resolution scope.");
		}

		[TestMethod]
		public void GetResolutionScope_DeclaringTypeNullNamespaceNull()
		{
			Assert.AreEqual(String.Empty, TypeInformation.GetResolutionScope(typeof(TypeInformationTestNoNamespaceType)), "A type with no declaring type and no namespace should yield an empty resolution scope.");
		}

		[TestMethod]
		public void GetResolutionScope_DeclaringTypeNullNamespaceValid()
		{
			Assert.AreEqual("CR_Documentor.Test.Reflector", TypeInformation.GetResolutionScope(typeof(TypeInformationTest)), "A type with no declaring type but a valid namespace should return the namespace as the resolution scope.");
		}

		[TestMethod]
		public void GetResolutionScope_DeclaringTypeValidNoNamespace()
		{
			Assert.AreEqual("TypeInformationTestNoNamespaceType", TypeInformation.GetResolutionScope(typeof(TypeInformationTestNoNamespaceType.NestedType)), "A type with a declaring type should return the declaring type as the resolution scope.");
		}

		[TestMethod]
		public void GetResolutionScope_DeclaringTypeValidWithNamespace()
		{
			Assert.AreEqual("CR_Documentor.Test.Reflector.TypeInformationTest", TypeInformation.GetResolutionScope(typeof(NestedType)), "A type with a declaring type should return the declaring type as the resolution scope.");
		}

		private class NestedType { }
	}
}

internal class TypeInformationTestNoNamespaceType { internal class NestedType { } }
