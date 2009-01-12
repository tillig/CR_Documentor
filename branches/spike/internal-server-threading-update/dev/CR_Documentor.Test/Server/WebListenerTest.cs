using System;
using System.Net;
using CR_Documentor.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock;

namespace CR_Documentor.Test.Server
{
	[TestClass]
	[VerifyMocks]
	public class WebListenerTest
	{
		private const UInt16 TestListenerPort = 22334;
		private static readonly Guid TestListenerUniqueId = Guid.NewGuid();

		[TestMethod]
		[ExpectedException(typeof(System.NotSupportedException))]
		public void Ctor_HttpListenerNotSupported()
		{
			using (RecordExpectations recorder = RecorderManager.StartRecording())
			{
				bool dummy = HttpListener.IsSupported;
				recorder.Return(false);
			}
			WebListener listener = new WebListener(TestListenerPort, TestListenerUniqueId);
		}

		[TestMethod]
		public void Dispose_CallsStopIfListening()
		{
			using (WebListener listener = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				using (RecordExpectations recorder = RecorderManager.StartRecording())
				{
					listener.Stop();
					recorder.CallOriginal();
				}
				listener.Start();
				listener.Dispose();
			}
		}

		[TestMethod]
		public void Dispose_DoesNotCallStopIfNotListening()
		{
			using (WebListener listener = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				using (RecordExpectations recorder = RecorderManager.StartRecording())
				{
					listener.Stop();
					recorder.FailWhenCalled();
				}
				listener.Dispose();
			}
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetContext_FailsToGetContextIfNotListening()
		{
			using (WebListener listener = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				listener.GetContext();
			}
		}

		[TestMethod]
		public void GetContext_RetrievesContextIfListening()
		{
			HttpListenerContext expectedContext = RecorderManager.CreateMockedObject<HttpListenerContext>();
			using (RecordExpectations recorder = RecorderManager.StartRecording())
			{
				HttpListener dummyInternalListener = new HttpListener();
				dummyInternalListener.Prefixes.Add(null);
				bool dummyListening = dummyInternalListener.IsListening;
				recorder.Return(true);
				dummyInternalListener.GetContext();
				recorder.Return(expectedContext);
			}
			using (WebListener listener = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				HttpListenerContext actualContext = listener.GetContext();
				Assert.AreSame(expectedContext, actualContext, "The context was not retrieved even though the listener was running.");
			}
		}

		[TestMethod]
		public void IsListening_InitialValue()
		{
			using (WebListener listener = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				Assert.IsFalse(listener.IsListening, "The listener should not be listening until it's been started.");
			}
		}

		[TestMethod]
		public void IsListening_Started()
		{
			using (WebListener listener = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				listener.Start();
				Assert.IsTrue(listener.IsListening, "The listener should be listening after it's been started.");
			}
		}

		[TestMethod]
		public void IsListening_Stopped()
		{
			using (WebListener listener = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				listener.Start();
				listener.Stop();
				Assert.IsFalse(listener.IsListening, "The listener should not be listening after it's been stopped.");
			}
		}

		[TestMethod]
		public void Prefix_Format()
		{
			using (WebListener listener = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				string prefix = String.Format(WebListener.BaseUriFormat, TestListenerPort, TestListenerUniqueId);
				Assert.AreEqual(prefix, listener.Prefix.AbsoluteUri, "The prefix should be properly formatted.");
			}
		}

		[TestMethod]
		public void Prefix_GuidIsThirdSegment()
		{
			using (WebListener listener = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				string[] segments = listener.Prefix.Segments;
				// Get the path segment and trim the trailing slash
				string guidSegment = segments[2];
				guidSegment = guidSegment.Substring(0, guidSegment.Length - 1);
				Guid guidId = new Guid(guidSegment);
				Assert.AreEqual(TestListenerUniqueId, guidId, "The GUID identifier segment should be the last segment and should match the server's unique ID.");
			}
		}
	}
}
