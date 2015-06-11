using System;
using System.Drawing;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test
{
	[TestClass]
	public class AssemblyExtensionsTest
	{
		private const string ResourceIconPath = "CR_Documentor.Test.AssemblyExtensionsTest.ReadEmbeddedResourceIcon.ico";
		private const string ResourceStringPath = "CR_Documentor.Test.AssemblyExtensionsTest.ReadEmbeddedResourceString.txt";

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ReadEmbeddedResourceIcon_EmptyPath()
		{
			AssemblyExtensions.ReadEmbeddedResourceIcon(typeof(AssemblyExtensionsTest).Assembly, "");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ReadEmbeddedResourceIcon_NullAssembly()
		{
			AssemblyExtensions.ReadEmbeddedResourceIcon(null, ResourceIconPath);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ReadEmbeddedResourceIcon_NullPath()
		{
			AssemblyExtensions.ReadEmbeddedResourceIcon(typeof(AssemblyExtensionsTest).Assembly, null);
		}

		[TestMethod]
		public void ReadEmbeddedResourceIcon_ResourceFound()
		{
			Icon icon = AssemblyExtensions.ReadEmbeddedResourceIcon(typeof(AssemblyExtensionsTest).Assembly, ResourceIconPath);
			Assert.IsNotNull(icon, "The icon was not read.");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ReadEmbeddedResourceIcon_ResourceNotFound()
		{
			AssemblyExtensions.ReadEmbeddedResourceIcon(typeof(AssemblyExtensionsTest).Assembly, "NoSuchPath");
		}

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
			AssemblyExtensions.ReadEmbeddedResourceString(null, ResourceStringPath);
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
			string content = AssemblyExtensions.ReadEmbeddedResourceString(typeof(AssemblyExtensionsTest).Assembly, ResourceStringPath);
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
