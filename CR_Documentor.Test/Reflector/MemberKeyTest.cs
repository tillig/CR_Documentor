using System;
using CR_Documentor.Reflector;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test.Reflector
{
	[TestClass]
	public class MemberKeyTest
	{
		[TestMethod]
		public void GetFullName_NoDots()
		{
			string name = MemberKey.GetFullName("Foo");
			Assert.AreEqual("Foo", name, "No prefix and no dots should equate to a type and the name should be that type.");
		}

		[TestMethod]
		public void GetFullName_NoPrefixNoParameters()
		{
			string name = MemberKey.GetFullName("Foo.Bar.Baz");
			Assert.AreEqual("Foo.Bar.Baz", name, "If no prefix is specified and there are no parameters, the passed in key is assumed to be a type and the name should be the type name.");
		}

		[TestMethod]
		public void GetFullName_NoPrefixWithParameters()
		{
			string name = MemberKey.GetFullName("Foo.Bar.Baz(string)");
			Assert.AreEqual("Foo.Bar.Baz", name, "If no prefix is specified and there are parameters, the passed in key is assumed to be a member and the name should be the member name.");
		}

		[TestMethod]
		public void GetFullName_StartsWithColon()
		{
			string name = MemberKey.GetFullName(":Foo.Bar.Baz");
			Assert.AreEqual("Foo.Bar.Baz", name, "Starting the name with a colon should be ignored and the result should be the type name.");
		}

		[TestMethod]
		public void GetFullName_UnknownPrefix()
		{
			string name = MemberKey.GetFullName("X:Foo.Bar.Baz");
			Assert.AreEqual("", name, "If the prefix is unknown, the full name is empty.");
		}

		[TestMethod]
		public void GetFullName_EPrefixSpecified()
		{
			string name = MemberKey.GetFullName("E:Foo.Bar.Baz");
			Assert.AreEqual("Foo.Bar.Baz", name, "If the prefix indicates an event, the name should be the event.");
		}

		[TestMethod]
		public void GetFullName_FPrefixSpecified()
		{
			string name = MemberKey.GetFullName("F:Foo.Bar.Baz");
			Assert.AreEqual("Foo.Bar.Baz", name, "If the prefix indicates a field, the name should be the field.");
		}

		[TestMethod]
		public void GetFullName_MPrefixSpecified()
		{
			string name = MemberKey.GetFullName("M:Foo.Bar.Baz");
			Assert.AreEqual("Foo.Bar.Baz()", name, "If the prefix indicates a member, the name should be the member.");
		}

		[TestMethod]
		public void GetFullName_MPrefixSpecifiedWithParameters()
		{
			string name = MemberKey.GetFullName("M:Foo.Bar.Baz(string)");
			Assert.AreEqual("Foo.Bar.Baz(string)", name, "If the prefix indicates a member, the name should be the member.");
		}

		[TestMethod]
		public void GetFullName_NPrefixSpecified()
		{
			string name = MemberKey.GetFullName("N:Foo.Bar.Baz");
			Assert.AreEqual("Foo.Bar.Baz", name, "If the prefix indicates a namespace, the name should be the namespace.");
		}

		[TestMethod]
		public void GetFullName_PPrefixSpecified()
		{
			string name = MemberKey.GetFullName("P:Foo.Bar.Baz");
			Assert.AreEqual("Foo.Bar.Baz", name, "If the prefix indicates a property, the name should be the property.");
		}

		[TestMethod]
		public void GetFullName_TPrefixSpecified()
		{
			string name = MemberKey.GetFullName("T:Foo.Bar.Baz");
			Assert.AreEqual("Foo.Bar.Baz", name, "If the prefix indicates a type, the name should be the type name.");
		}

		[TestMethod]
		public void GetName_NoDots()
		{
			string name = MemberKey.GetName("Foo");
			Assert.AreEqual("Foo", name, "No prefix and no dots should equate to a type and the name should be that type.");
		}

		[TestMethod]
		public void GetName_NoPrefixNoParameters()
		{
			string name = MemberKey.GetName("Foo.Bar.Baz");
			Assert.AreEqual("Baz", name, "If no prefix is specified and there are no parameters, the passed in key is assumed to be a type and the name should be the type name.");
		}

		[TestMethod]
		public void GetName_NoPrefixWithParameters()
		{
			string name = MemberKey.GetName("Foo.Bar.Baz(string)");
			Assert.AreEqual("Baz", name, "If no prefix is specified and there are parameters, the passed in key is assumed to be a member and the name should be the member name.");
		}

		[TestMethod]
		public void GetName_StartsWithColon()
		{
			string name = MemberKey.GetName(":Foo.Bar.Baz");
			Assert.AreEqual("Baz", name, "Starting the name with a colon should be ignored and the result should be the type name.");
		}

		[TestMethod]
		public void GetName_UnknownPrefix()
		{
			string name = MemberKey.GetName("X:Foo.Bar.Baz");
			Assert.AreEqual("Baz", name, "If the prefix is unknown, the name is the type name.");
		}

		[TestMethod]
		public void GetName_EPrefixSpecified()
		{
			string name = MemberKey.GetName("E:Foo.Bar.Baz");
			Assert.AreEqual("Baz", name, "If the prefix indicates an event, the name should be the event.");
		}

		[TestMethod]
		public void GetName_FPrefixSpecified()
		{
			string name = MemberKey.GetName("F:Foo.Bar.Baz");
			Assert.AreEqual("Baz", name, "If the prefix indicates a field, the name should be the field.");
		}

		[TestMethod]
		public void GetName_MPrefixSpecified()
		{
			string name = MemberKey.GetName("M:Foo.Bar.Baz");
			Assert.AreEqual("Baz", name, "If the prefix indicates a member, the name should be the member.");
		}

		[TestMethod]
		public void GetName_MPrefixSpecifiedWithParameters()
		{
			string name = MemberKey.GetName("M:Foo.Bar.Baz(string)");
			Assert.AreEqual("Baz", name, "If the prefix indicates a member, the name should be the member.");
		}

		[TestMethod]
		public void GetName_NPrefixSpecified()
		{
			string name = MemberKey.GetName("N:Foo.Bar.Baz");
			Assert.AreEqual("Foo.Bar.Baz", name, "If the prefix indicates a namespace, the name should be the namespace.");
		}

		[TestMethod]
		public void GetName_PPrefixSpecified()
		{
			string name = MemberKey.GetName("P:Foo.Bar.Baz");
			Assert.AreEqual("Baz", name, "If the prefix indicates a property, the name should be the property.");
		}

		[TestMethod]
		public void GetName_TPrefixSpecified()
		{
			string name = MemberKey.GetName("T:Foo.Bar.Baz");
			Assert.AreEqual("Baz", name, "If the prefix indicates a type, the name should be the type name.");
		}
	}
}
