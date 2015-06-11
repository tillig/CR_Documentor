using System;
using System.Xml;
using CR_Documentor.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test.Xml
{
	[TestClass]
	public class EvaluatorTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Test_EmptyXPath()
		{
			string xpath = "";
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("root"));
			Evaluator.Test(doc.DocumentElement, xpath);
		}

		[TestMethod]
		public void Test_FoundMultipleTimes()
		{
			string xpath = "entry";
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("root"));
			doc.DocumentElement.AppendChild(doc.CreateElement("entry"));
			doc.DocumentElement.AppendChild(doc.CreateElement("entry"));
			doc.DocumentElement.AppendChild(doc.CreateElement("entry"));
			bool actual = Evaluator.Test(doc.DocumentElement, xpath);
			Assert.IsTrue(actual, "The XPath should have been found.");
		}

		[TestMethod]
		public void Test_FoundOnce()
		{
			string xpath = "entry";
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("root"));
			doc.DocumentElement.AppendChild(doc.CreateElement("entry"));
			bool actual = Evaluator.Test(doc.DocumentElement, xpath);
			Assert.IsTrue(actual, "The XPath should have been found.");
		}

		[TestMethod]
		public void Test_NotFound()
		{
			string xpath = "entry";
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("root"));
			bool actual = Evaluator.Test(doc.DocumentElement, xpath);
			Assert.IsFalse(actual, "The XPath should not have been found.");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Test_NullParent()
		{
			string xpath = "entry";
			XmlDocument doc = new XmlDocument();
			Evaluator.Test(doc.DocumentElement, xpath);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Test_NullXPath()
		{
			string xpath = null;
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("root"));
			Evaluator.Test(doc.DocumentElement, xpath);
		}
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ValueOf_EmptyXPath()
		{
			string xpath = "";
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("root"));
			doc.DocumentElement.InnerText = "expected";
			Evaluator.ValueOf(doc.DocumentElement, xpath);
		}

		[TestMethod]
		public void ValueOf_FoundMultipleTimes()
		{
			string xpath = "entry";
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("root"));
			for (int i = 0; i < 3; i++)
			{
				XmlElement el = doc.CreateElement("entry");
				el.InnerText = "text";
				doc.DocumentElement.AppendChild(el);
			}
			string actual = Evaluator.ValueOf(doc.DocumentElement, xpath);
			Assert.AreEqual("", actual, "If more than one item exists, empty string should be returned.");
		}

		[TestMethod]
		public void ValueOf_FoundOnce()
		{
			string xpath = "entry";
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("root"));
			XmlElement el = doc.CreateElement("entry");
			el.InnerText = "text";
			doc.DocumentElement.AppendChild(el);
			string actual = Evaluator.ValueOf(doc.DocumentElement, xpath);
			Assert.AreEqual("text", actual, "If the item is found once, the innner text should be returned.");
		}

		[TestMethod]
		public void ValueOf_NotFound()
		{
			string xpath = "entry";
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("root"));
			XmlElement el = doc.CreateElement("othernode");
			el.InnerText = "text";
			doc.DocumentElement.AppendChild(el);
			string actual = Evaluator.ValueOf(doc.DocumentElement, xpath);
			Assert.AreEqual("", actual, "If the xpath wasn't found, empty string should be returned.");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ValueOf_NullParent()
		{
			string xpath = "entry";
			XmlDocument doc = new XmlDocument();
			Evaluator.ValueOf(doc.DocumentElement, xpath);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ValueOf_NullXPath()
		{
			string xpath = null;
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateElement("root"));
			doc.DocumentElement.InnerText = "expected";
			Evaluator.ValueOf(doc.DocumentElement, xpath);
		}
	}
}
