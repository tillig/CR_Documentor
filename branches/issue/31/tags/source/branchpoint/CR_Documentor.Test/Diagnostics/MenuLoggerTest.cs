using System;
using CR_Documentor.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock;

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

		[TestMethod]
		public void Ctor_SetsPluginLogType()
		{
			MenuLogger logger = new MenuLogger(typeof(MenuLoggerTest));
			Assert.AreEqual(typeof(DevExpress.CodeRush.Diagnostics.Menus.Log), logger.PluginLogType, "The plugin log type was not set.");
		}

		[TestMethod]
		[VerifyMocks]
		public void Enter_EntersLog()
		{
			// We'll only test one of the methods to ensure the plugin log type
			// really got wired up properly; tests on the base Logger class
			// should take care of the rest.
			SynchronizationManagerMock.Initialize();
			using (RecordExpectations recorder = RecorderManager.StartRecording())
			{
				DevExpress.CodeRush.Diagnostics.Menus.Log.Enter("dummy");
			}

			MenuLogger logger = new MenuLogger(typeof(MenuLoggerTest));
			logger.Enter("message");
		}
	}
}
