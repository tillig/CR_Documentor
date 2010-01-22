using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test
{
	[TestClass]
	public class AssemblyExtensionsTest
	{
		private const string ResourcePath = "CR_Documentor.Test.AssemblyExtensionsTest.ReadEmbeddedResourceString.txt";

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ReadEmbeddedResourceString_EmptyPath()
		{
			AssemblyExtensions.ReadEmbeddedResourceString(typeof(AssemblyExtensionsTest).Assembly, "");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ReadEmbeddedResourceString_NullAssembly()
		{
			AssemblyExtensions.ReadEmbeddedResourceString(null, ResourcePath);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ReadEmbeddedResourceString_NullPath()
		{
			AssemblyExtensions.ReadEmbeddedResourceString(typeof(AssemblyExtensionsTest).Assembly, null);
		}

		[TestMethod]
		public void ReadEmbeddedResourceString_ResourceFound()
		{
			string content = AssemblyExtensions.ReadEmbeddedResourceString(typeof(AssemblyExtensionsTest).Assembly, ResourcePath);
			Assert.AreEqual("content", content, "The content read was not correct.");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ReadEmbeddedResourceString_ResourceNotFound()
		{
			AssemblyExtensions.ReadEmbeddedResourceString(typeof(AssemblyExtensionsTest).Assembly, "NoSuchPath");
		}
	}
}
