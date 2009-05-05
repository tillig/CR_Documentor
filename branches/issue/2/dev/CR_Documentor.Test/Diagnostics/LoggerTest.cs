using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using CR_Documentor.Diagnostics;
using DevExpress.CodeRush.Diagnostics.ToolWindows;
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
			SynchronizationManagerMock.Initialize();
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
			string message = "Entry message.";
			string expected = this.BuildExpectedMessage(message);

			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Enter(message);

			Assert.AreEqual(1, TestInternalLog.LogMessages.Count, "The wrong number of messages was added.");
			DictionaryEntry entry = TestInternalLog.LogMessages[0];
			Assert.AreEqual("Enter", entry.Key, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Value.ToString(), "The log message was incorrect.");
		}

		[TestMethod]
		public void Exit_ExitsLog()
		{
			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Exit();
			Assert.AreEqual(1, TestInternalLog.LogMessages.Count, "The wrong number of messages was added.");
			DictionaryEntry entry = TestInternalLog.LogMessages[0];
			Assert.AreEqual("Exit", entry.Key, "The wrong internal log method was called.");
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

			Assert.AreEqual(1, TestInternalLog.LogMessages.Count, "The wrong number of messages was added.");
			DictionaryEntry entry = TestInternalLog.LogMessages[0];
			Assert.AreEqual("SendError", entry.Key, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Value.ToString(), "The log message was incorrect.");
		}

		[TestMethod]
		public void Write_ErrorMessageWrittenWithException()
		{
			string message = "Entry message.";
			string expected = this.BuildExpectedMessage(message);
			Exception err = new Exception("exception message");

			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Error, message, err);

			Assert.AreEqual(2, TestInternalLog.LogMessages.Count, "The wrong number of messages was added.");
			DictionaryEntry entry = TestInternalLog.LogMessages[0];
			Assert.AreEqual("SendError", entry.Key, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Value.ToString(), "The log message was incorrect.");
			entry = TestInternalLog.LogMessages[1];
			Assert.AreEqual("SendException", entry.Key, "The wrong internal log method was called.");
			Assert.AreEqual(err.Message, entry.Value.ToString(), "The exception message was incorrect.");
		}

		[TestMethod]
		public void Write_InfoMessageWritten()
		{
			string message = "Entry message.";
			string expected = this.BuildExpectedMessage(message);

			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Info, message);

			Assert.AreEqual(1, TestInternalLog.LogMessages.Count, "The wrong number of messages was added.");
			DictionaryEntry entry = TestInternalLog.LogMessages[0];
			Assert.AreEqual("SendMsg", entry.Key, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Value.ToString(), "The log message was incorrect.");
		}

		[TestMethod]
		public void Write_InfoMessageWrittenWithException()
		{
			string message = "Entry message.";
			string expected = this.BuildExpectedMessage(message);
			Exception err = new Exception("exception message");

			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Info, message, err);

			Assert.AreEqual(2, TestInternalLog.LogMessages.Count, "The wrong number of messages was added.");
			DictionaryEntry entry = TestInternalLog.LogMessages[0];
			Assert.AreEqual("SendMsg", entry.Key, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Value.ToString(), "The log message was incorrect.");
			entry = TestInternalLog.LogMessages[1];
			Assert.AreEqual("SendException", entry.Key, "The wrong internal log method was called.");
			Assert.AreEqual(err.Message, entry.Value.ToString(), "The exception message was incorrect.");
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

			Assert.AreEqual(1, TestInternalLog.LogMessages.Count, "The wrong number of messages was added.");
			DictionaryEntry entry = TestInternalLog.LogMessages[0];
			Assert.AreEqual("SendMsg", entry.Key, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Value.ToString(), "The log message was incorrect.");
		}

		[TestMethod]
		public void Write_WarningMessageWritten()
		{
			string message = "Entry message.";
			string expected = this.BuildExpectedMessage(message);

			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Warn, message);

			Assert.AreEqual(1, TestInternalLog.LogMessages.Count, "The wrong number of messages was added.");
			DictionaryEntry entry = TestInternalLog.LogMessages[0];
			Assert.AreEqual("SendWarning", entry.Key, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Value.ToString(), "The log message was incorrect.");
		}

		[TestMethod]
		public void Write_WarningMessageWrittenWithException()
		{
			string message = "Entry message.";
			string expected = this.BuildExpectedMessage(message);
			Exception err = new Exception("exception message");

			TestLogger logger = new TestLogger(typeof(LoggerTest));
			logger.Write(LogLevel.Warn, message, err);

			Assert.AreEqual(2, TestInternalLog.LogMessages.Count, "The wrong number of messages was added.");
			DictionaryEntry entry = TestInternalLog.LogMessages[0];
			Assert.AreEqual("SendWarning", entry.Key, "The wrong internal log method was called.");
			Assert.AreEqual(expected, entry.Value.ToString(), "The log message was incorrect.");
			entry = TestInternalLog.LogMessages[1];
			Assert.AreEqual("SendException", entry.Key, "The wrong internal log method was called.");
			Assert.AreEqual(err.Message, entry.Value.ToString(), "The exception message was incorrect.");
		}

		[TestMethod]
		public void TestInternalLog_VerifyInterface()
		{
			// This test is needed to ensure the test internal log class we've
			// created is obeying the interface set forth in DXCore. If this
			// fails then the interface to the real log classes has changed.
			MethodInfo[] testMethods = typeof(TestInternalLog).GetMethods(BindingFlags.Public | BindingFlags.Static);
			foreach (MethodInfo expected in testMethods)
			{
				ParameterInfo[] expectedParams = expected.GetParameters();
				Type[] parameterTypes = new Type[expectedParams.Length];
				for (int i = 0; i < expectedParams.Length; i++)
				{
					parameterTypes[i] = expectedParams[i].ParameterType;
				}
				MethodInfo actual = typeof(Log).GetMethod(expected.Name, BindingFlags.Public | BindingFlags.Static, null, parameterTypes, null);
				Assert.IsNotNull(actual, "The method " + expected.Name + " from the test log class was not found in the real log implementation.");
			}
		}

		private string BuildExpectedMessage(string message)
		{
			string expected = String.Format(CultureInfo.InvariantCulture, Logger.MessageFormat, typeof(LoggerTest).Name, message);
			return expected;
		}

		private class TestLogger : Logger
		{
			public TestLogger(Type logOwner)
				: base(logOwner, typeof(TestInternalLog))
			{
				TestInternalLog.LogMessages.Clear();
			}
		}

		// Log class that mimicks internal DXCore classes like
		// DevExpress.CodeRush.Diagnostics.ToolWindows.Log
		private sealed class TestInternalLog
		{
			public static List<DictionaryEntry> LogMessages = new List<DictionaryEntry>();

			public static void Enter(string message)
			{
				DictionaryEntry entry = new DictionaryEntry("Enter", message);
				LogMessages.Add(entry);
			}

			public static void Exit()
			{
				DictionaryEntry entry = new DictionaryEntry("Exit", "");
				LogMessages.Add(entry);
			}

			public static void SendMsg(string message)
			{
				DictionaryEntry entry = new DictionaryEntry("SendMsg", message);
				LogMessages.Add(entry);
			}

			public static void SendWarning(string message)
			{
				DictionaryEntry entry = new DictionaryEntry("SendWarning", message);
				LogMessages.Add(entry);
			}

			public static void SendError(string message)
			{
				DictionaryEntry entry = new DictionaryEntry("SendError", message);
				LogMessages.Add(entry);
			}

			public static void SendException(Exception value)
			{
				DictionaryEntry entry = new DictionaryEntry("SendException", value.Message);
				LogMessages.Add(entry);
			}
		}
	}
}
