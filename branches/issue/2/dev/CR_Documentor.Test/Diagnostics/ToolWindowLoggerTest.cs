using System;
using CR_Documentor.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test.Diagnostics
{
	[TestClass]
	public class ToolWindowLoggerTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullLogOwner()
		{
			ToolWindowLogger logger = new ToolWindowLogger(null);
		}

		[TestMethod]
		public void Ctor_SetsLogOwner()
		{
			ToolWindowLogger logger = new ToolWindowLogger(typeof(ToolWindowLoggerTest));
			Assert.AreEqual(typeof(ToolWindowLoggerTest), logger.LogOwner, "The log owner was not set.");
		}

		[TestMethod]
		public void Ctor_SetsPluginLogType()
		{
			ToolWindowLogger logger = new ToolWindowLogger(typeof(ToolWindowLoggerTest));
			Assert.AreEqual(typeof(DevExpress.CodeRush.Diagnostics.ToolWindows.Log), logger.PluginLogType, "The plugin log type was not set.");
		}
	}
}
