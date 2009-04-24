using System;
using System.Xml;
using CR_Documentor.Transformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test.Transformation
{
	[TestClass]
	public class CommentMatchEventArgsTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullElement()
		{
			CommentMatchEventArgs args = new CommentMatchEventArgs(null);
		}

		[TestMethod]
		public void Ctor_PopulatesElementProperty()
		{
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("foo"));
			CommentMatchEventArgs args = new CommentMatchEventArgs(doc.DocumentElement);
			Assert.AreEqual(doc.DocumentElement, args.Element, "The Element property should be populated by the constructor.");
		}
	}
}
