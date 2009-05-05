using System;
using System.Collections;
using System.Collections.Generic;
using CR_Documentor.Diagnostics;
using DevExpress.DXCore.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock;

namespace CR_Documentor.Test.Diagnostics
{
	[TestClass]
	[VerifyMocks]
	public class LoggerTest
	{
		[TestInitialize]
		public void TestInitialize()
		{
			// Set up calls to SynchronizationManager so they will pass in a unit
			// test environment.
			DynamicReturnValue beginInvoke = new DynamicReturnValue(SynchronizationManagerBeginInvoke);
			using (RecordExpectations recorder = RecorderManager.StartRecording())
			{
				IAsyncResult dummyResult = SynchronizationManager.BeginInvoke(null, null);
				recorder.Return(beginInvoke);
				recorder.RepeatAlways();
			}
		}

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

		// The SynchronizationManager static class in DXCore can't run outside a
		// DXCore environment. This DynamicReturnValue method can be used to swap
		// calls to the SynchronizationManager for unit testing purposes.
		//
		// Note that in our version we are blocking on the wait handle so the call
		// finishes. This doesn't happen in real SynchronizationManager calls, but
		// since we're trying to test the results of the call and we don't want
		// to Thread.Sleep in every test while we wait, we'll just make the call
		// synchronously.

		private static IAsyncResult SynchronizationManagerBeginInvoke(object[] parameters, object context)
		{
			Delegate exec = parameters[0] as Delegate;
			SynchronizationManagerMethodCall call = delegate(object[] callParams) { return exec.DynamicInvoke(callParams); };
			object[] paramArray = parameters[1] as object[];
			IAsyncResult result = call.BeginInvoke(paramArray, null, null);
			result.AsyncWaitHandle.WaitOne();
			return result;
		}

		private delegate object SynchronizationManagerMethodCall(object[] parameters);
	}
}
