using System;
using CR_Documentor.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test.Diagnostics
{
	[TestClass]
	public class MenuLoggerTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullLogOwner()
		{
			MenuLogger logger = new MenuLogger(null);
		}

		[TestMethod]
		public void Ctor_SetsLogOwner()
		{
			MenuLogger logger = new MenuLogger(typeof(MenuLoggerTest));
			Assert.AreEqual(typeof(MenuLoggerTest), logger.LogOwner, "The log owner was not set.");
		}
	}
}
