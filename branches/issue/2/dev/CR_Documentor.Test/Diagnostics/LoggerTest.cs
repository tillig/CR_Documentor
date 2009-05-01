using System;
using CR_Documentor.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock;
using DevExpress.DXCore.Threading;
using System.Collections.Generic;
using System.Collections;

namespace CR_Documentor.Test.Diagnostics
{
	[TestClass]
	[VerifyMocks]
	public class LoggerTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullLogOwner()
		{
			TestLogger logger = new TestLogger(null);
		}

		[TestMethod]
		public void Ctor_SetsLogOwner()
		{
			TestLogger logger = new TestLogger(typeof(LoggerTest));
			Assert.AreEqual(typeof(LoggerTest), logger.LogOwner, "The log owner was not set.");
		}

		[TestMethod]
		public void Enter_EntersLog()
		{
			string changed = null;
			WriteLogMessageHandler del = delegate(string s)
			{
				changed = s;
			};
			// TODO: Figure out why the SynchronizationManager is blocking and not executing.
			// As it stands, the log test below doesn't pass because the SyncManager isn't actually executing anything.
			IAsyncResult result = SynchronizationManager.BeginInvoke(del, new object[] { "changed" });
			//IAsyncResult result = del.BeginInvoke("changed", null, null);
			result.AsyncWaitHandle.WaitOne();
			Assert.AreEqual("changed", changed);

			TestLogger logger = new TestLogger(typeof(LoggerTest));
			string message = "Entry message.";
			logger.Enter(message);
			Assert.AreEqual(1, TestInternalLog.LogMessages.Count, "The wrong number of messages was added.");
			DictionaryEntry entry = TestInternalLog.LogMessages[0];
			Assert.AreEqual("Enter", entry.Key, "The wrong internal log method was called.");
			Assert.IsTrue(entry.Value.ToString().EndsWith(message), "The log message did not appear.");
		}

		private class TestLogger : Logger
		{
			public TestLogger(Type logOwner)
				: base(logOwner, typeof(TestInternalLog))
			{
				TestInternalLog.LogMessages.Clear();
			}
		}

		private sealed class TestInternalLog
		{
			public static List<DictionaryEntry> LogMessages = new List<DictionaryEntry>();
			public static void Enter(string message)
			{
				DictionaryEntry entry = new DictionaryEntry("Enter", message);
				LogMessages.Add(entry);
			}
		}
	}
}
