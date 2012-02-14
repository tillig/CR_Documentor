using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using CR_Documentor.Diagnostics;
using DevExpress.CodeRush.Diagnostics;
using DevExpress.CodeRush.Diagnostics.ToolWindows;
using DevExpress.DXCore.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Diagnostics
{
	[TestClass]
	[Isolated]
	public class LoggerTest
	{
		private List<Tuple<string, string>> _messages = null;

		[TestInitialize]
		public void TestInitialize()
		{
			this._messages = new List<Tuple<string, string>>();
			Isolate.WhenCalled(() => SynchronizationManager.BeginInvoke(null, null)).DoInstead(this.SynchronizationManagerMock);
			Isolate.WhenCalled(() => LogBase<TestInternalLog>.SendError(null)).DoInstead(ctx => this.RecordMessage("SendError", ctx));
			Isolate.WhenCalled(() => LogBase<TestInternalLog>.SendMsg(null)).DoInstead(ctx => this.RecordMessage("SendMsg", ctx));
			Isolate.WhenCalled(() => LogBase<TestInternalLog>.SendWarning(null)).DoInstead(ctx => this.RecordMessage("SendWarning", ctx));
			Isolate.WhenCalled(() => LogBase<TestInternalLog>.SendException(null)).DoInstead(ctx => this.RecordMessage("SendException", ctx));
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
		public void Ctor_HandlersInitialized()
		{
			TestLogger logger = new TestLogger(typeof(LoggerTest));
			Assert.IsNotNull(logger.SendErrorHandler, "The error handler wasn't initialized.");
			Assert.IsNotNull(logger.SendExceptionHandler, "The exception handler wasn't initialized.");
			Assert.IsNotNull(logger.SendMsgHandler, "The message handler wasn't initialized.");
			Assert.IsNotNull(logger.SendWarningHandler, "The warning handler wasn't initialized.");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Write_EmptyMessage()
		{
			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Info, "");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Write_EmptyMessageValidException()
		{
			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Info, "", new Exception("exception message"));
		}

		[TestMethod]
		public void Write_ErrorMessageWritten()
		{
			string message = "Entry message.";
			string expected = this.BuildExpectedMessage(message);

			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Error, message);

			Assert.AreEqual(1, this._messages.Count, "The wrong number of messages was added.");
			var entry = this._messages[0];
			Assert.AreEqual("SendError", entry.Item1, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Item2, "The log message was incorrect.");
		}

		[TestMethod]
		public void Write_ErrorMessageWrittenWithException()
		{
			string message = "Entry message.";
			string expected = this.BuildExpectedMessage(message);
			Exception err = new Exception("exception message");

			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Error, message, err);

			Assert.AreEqual(2, this._messages.Count, "The wrong number of messages was added.");
			var entry = this._messages[0];
			Assert.AreEqual("SendError", entry.Item1, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Item2, "The log message was incorrect.");
			entry = this._messages[1];
			Assert.AreEqual("SendException", entry.Item1, "The wrong internal log method was called.");
			Assert.AreEqual(err.ToString(), entry.Item2, "The exception message was incorrect.");
		}

		[TestMethod]
		public void Write_InfoMessageWritten()
		{
			string message = "Entry message.";
			string expected = this.BuildExpectedMessage(message);

			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Info, message);

			Assert.AreEqual(1, this._messages.Count, "The wrong number of messages was added.");
			var entry = this._messages[0];
			Assert.AreEqual("SendMsg", entry.Item1, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Item2, "The log message was incorrect.");
		}

		[TestMethod]
		public void Write_InfoMessageWrittenWithException()
		{
			string message = "Entry message.";
			string expected = this.BuildExpectedMessage(message);
			Exception err = new Exception("exception message");

			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Info, message, err);

			Assert.AreEqual(2, this._messages.Count, "The wrong number of messages was added.");
			var entry = this._messages[0];
			Assert.AreEqual("SendMsg", entry.Item1, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Item2, "The log message was incorrect.");
			entry = this._messages[1];
			Assert.AreEqual("SendException", entry.Item1, "The wrong internal log method was called.");
			Assert.AreEqual(err.ToString(), entry.Item2, "The exception message was incorrect.");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Write_NullMessage()
		{
			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Info, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Write_NullMessageValidException()
		{
			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Info, null, new Exception("exception message"));
		}

		[TestMethod]
		public void Write_ValidMessageNullException()
		{
			string message = "Entry message.";
			string expected = this.BuildExpectedMessage(message);
			Exception err = null;

			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Info, message, err);

			Assert.AreEqual(1, this._messages.Count, "The wrong number of messages was added.");
			var entry = this._messages[0];
			Assert.AreEqual("SendMsg", entry.Item1, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Item2, "The log message was incorrect.");
		}

		[TestMethod]
		public void Write_WarningMessageWritten()
		{
			string message = "Entry message.";
			string expected = this.BuildExpectedMessage(message);

			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Warn, message);

			Assert.AreEqual(1, this._messages.Count, "The wrong number of messages was added.");
			var entry = this._messages[0];
			Assert.AreEqual("SendWarning", entry.Item1, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Item2, "The log message was incorrect.");
		}

		[TestMethod]
		public void Write_WarningMessageWrittenWithException()
		{
			string message = "Entry message.";
			string expected = this.BuildExpectedMessage(message);
			Exception err = new Exception("exception message");

			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Warn, message, err);

			Assert.AreEqual(2, this._messages.Count, "The wrong number of messages was added.");
			var entry = this._messages[0];
			Assert.AreEqual("SendWarning", entry.Item1, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Item2, "The log message was incorrect.");
			entry = this._messages[1];
			Assert.AreEqual("SendException", entry.Item1, "The wrong internal log method was called.");
			Assert.AreEqual(err.ToString(), entry.Item2, "The exception message was incorrect.");
		}

		private string BuildExpectedMessage(string message)
		{
			return String.Format(CultureInfo.InvariantCulture, TestLogger.MessageFormat, typeof(LoggerTest).Name, message);
		}

		private void RecordMessage(string method, MethodCallContext context)
		{
			var message = context.Parameters[0].ToString();
			this._messages.Add(new Tuple<string, string>(method, message));
		}

		private IAsyncResult SynchronizationManagerMock(MethodCallContext context)
		{
			Func<object[], object> call = (object[] callParams) =>
			{
				if (callParams.Length == 0)
				{
					callParams = null;
				}
				return ((Delegate)context.Parameters[0]).DynamicInvoke(callParams);
			};
			object[] paramArray = context.Parameters[1] as object[];
			IAsyncResult result = call.BeginInvoke(paramArray, null, null);
			result.AsyncWaitHandle.WaitOne();
			return result;
		}

		private class TestLogger : Logger<TestInternalLog>
		{
			public TestLogger(Type logOwner)
				: base(logOwner)
			{
			}
		}

		private class TestInternalLog : LogBase<TestInternalLog>
		{
			protected override string Category
			{
				get { return "TestCategory"; }
			}
		}
	}
}
