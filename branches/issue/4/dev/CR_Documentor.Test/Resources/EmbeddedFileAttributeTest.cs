using System;
using CR_Documentor.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test.Resources
{
	[TestClass]
	public class EmbeddedFileAttributeTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullResourcePath()
		{
			new EmbeddedFileAttribute(null, "filename", "mimetype");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Ctor_EmptyResourcePath()
		{
			new EmbeddedFileAttribute("", "filename", "mimetype");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullFileName()
		{
			new EmbeddedFileAttribute("resourcepath", null, "mimetype");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Ctor_EmptyFileName()
		{
			new EmbeddedFileAttribute("resourcepath", "", "mimetype");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullMimeType()
		{
			new EmbeddedFileAttribute("resourcepath", "filename", null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Ctor_EmptyMimeType()
		{
			new EmbeddedFileAttribute("resourcepath", "filename", "");
		}

		[TestMethod]
		public void Ctor_StoresValues()
		{
			var attrib = new EmbeddedFileAttribute("resourcepath", "filename", "mimetype");
			Assert.AreEqual("resourcepath", attrib.ResourcePath);
			Assert.AreEqual("filename", attrib.FileName);
			Assert.AreEqual("mimetype", attrib.MimeType);
		}
	}
}
