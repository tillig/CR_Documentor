using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test
{
	[TestClass]
	public class ICustomAttributeProviderExtensionsTest
	{
		[TestMethod]
		public void GetCustomAttribute_AttributeNotFound()
		{
			System.Runtime.CompilerServices.DependencyAttribute attrib = ICustomAttributeProviderExtensions.GetCustomAttribute<System.Runtime.CompilerServices.DependencyAttribute>(typeof(ICustomAttributeProviderExtensionsTest).Assembly);
			Assert.IsNull(attrib, "The attribute should be null if not found.");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetCustomAttribute_NullProvider()
		{
			ICustomAttributeProviderExtensions.GetCustomAttribute<AssemblyTitleAttribute>(null);
		}

		[TestMethod]
		public void GetCustomAttribute_RetrievesAttribute()
		{
			AssemblyTitleAttribute attrib = ICustomAttributeProviderExtensions.GetCustomAttribute<AssemblyTitleAttribute>(typeof(ICustomAttributeProviderExtensionsTest).Assembly);
			Assert.IsNotNull(attrib, "The attribute should not be null if found.");
			Assert.AreEqual("CR_Documentor Tests", attrib.Title, "The retrieved attribute was not the correct one.");
		}
	}
}
