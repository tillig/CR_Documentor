using System;
using CR_Documentor.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CR_Documentor.Test.Diagnostics
{
	[TestClass]
	public class ActivityContextTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Ctor_EmptyActivityName()
		{
			TestLogger logger = new TestLogger();
			ActivityContext ctx = new ActivityContext(logger, "");
		}

		[TestMethod]
		public void Ctor_LogEntered()
		{
			TestLogger logger = new TestLogger();
			string activity = "Activity name";
			ActivityContext ctx = new ActivityContext(logger, activity);
			Assert.AreEqual(1, logger.EnterCallCount, "The log was not entered.");
			Assert.AreEqual(1, logger.Messages.Count, "The wrong number of messages were entered in the log.");
			Assert.AreEqual(activity, logger.Messages[0], "The wrong message content was entered in the log.");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullActivityName()
		{
			TestLogger logger = new TestLogger();
			ActivityContext ctx = new ActivityContext(logger, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Ctor_NullLogger()
		{
			ActivityContext ctx = new ActivityContext(null, "activity");
		}

		[TestMethod]
		public void Dispose_LogExited()
		{
			TestLogger logger = new TestLogger();
			string activity = "Activity name";
			using (ActivityContext ctx = new ActivityContext(logger, activity))
			{
				Assert.AreEqual(0, logger.ExitCallCount, "The log should not have exited yet.");
			}
			Assert.AreEqual(1, logger.ExitCallCount, "The log should exit on dispose.");
		}

		private class TestLogger : ILog
		{
			public int EnterCallCount { get; set; }
			public int ExitCallCount { get; set; }
			public List<String> Messages { get; set; }

			public TestLogger()
			{
				this.Messages = new List<string>();
			}

			public void Enter(string message)
			{
				this.Messages.Add(message);
				this.EnterCallCount++;
			}

			public void Exit()
			{
				this.ExitCallCount++;
			}
		}
	}
}
