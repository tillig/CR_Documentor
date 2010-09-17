using System;
using CR_Documentor.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test.Collections
{
	[TestClass]
	public class DefaultValueStringDictionaryTest
	{
		[TestMethod]
		public void DefaultValue_CanBeSet()
		{
			DefaultValueStringDictionary dict = new DefaultValueStringDictionary();
			dict.DefaultValue = "Foo";
			Assert.AreEqual("Foo", dict.DefaultValue, "The default value was not correctly set.");
		}

		[TestMethod]
		public void DefaultValue_EmptyString()
		{
			DefaultValueStringDictionary dict = new DefaultValueStringDictionary();
			Assert.AreEqual("", dict.DefaultValue, "The default value should be initialized to empty string.");
		}

		[TestMethod]
		public void Indexer_ActualValueReturnedWhenKeyFound()
		{
			DefaultValueStringDictionary dict = new DefaultValueStringDictionary();
			dict.DefaultValue = "Default Value";
			dict.Add("a", "1");
			dict.Add("b", "2");
			dict.Add("c", "3");
			Assert.AreEqual("3", dict["c"], "The actual value should be returned if the key is found in the dictionary.");
		}

		[TestMethod]
		public void Indexer_DefaultValueReturnedWhenKeyNotFound()
		{
			DefaultValueStringDictionary dict = new DefaultValueStringDictionary();
			string expected = "Expected";
			dict.DefaultValue = expected;
			dict.Add("a", "1");
			dict.Add("b", "2");
			dict.Add("c", "3");
			Assert.AreEqual(expected, dict["d"], "The default value should be returned if the key is not found in the dictionary.");
		}
	}
}
