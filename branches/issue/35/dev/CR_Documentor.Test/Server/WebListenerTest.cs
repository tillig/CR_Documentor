using System;
using System.Net;
using CR_Documentor.Server;
using DevExpress.DXCore.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Server
{
	[TestClass]
	[Isolated]
	public class WebListenerTest
	{
		private const UInt16 TestListenerPort = 22334;
		private static readonly Guid TestListenerUniqueId = Guid.NewGuid();

		[TestInitialize]
		public void TestInitialize()
		{
			Isolate.WhenCalled(() => SynchronizationManager.BeginInvoke(null, null)).ReturnRecursiveFake();
		}

		[TestMethod]
		[ExpectedException(typeof(System.NotSupportedException))]
		public void Ctor_HttpListenerNotSupported()
		{
			Isolate.WhenCalled(() => HttpListener.IsSupported).WillReturn(false);
			WebListener listener = new WebListener(TestListenerPort, TestListenerUniqueId);
		}

		[TestMethod]
		public void Dispose_CallsStopIfListening()
		{
			using (WebListener listener = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				Isolate.WhenCalled(() => listener.Stop()).CallOriginal();
				listener.Start();
				listener.Dispose();
				Isolate.Verify.WasCalledWithExactArguments(() => listener.Stop());
			}
		}

		[TestMethod]
		public void Dispose_DoesNotCallStopIfNotListening()
		{
			using (WebListener listener = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				Isolate.WhenCalled(() => listener.Stop()).CallOriginal();
				listener.Dispose();
				Isolate.Verify.WasNotCalled(() => listener.Stop());
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
			var expectedContext = Isolate.Fake.Instance<HttpListenerContext>();
			var dummyInternalListener = Isolate.Fake.Instance<HttpListener>();
			Isolate.WhenCalled(() => dummyInternalListener.IsListening).WillReturn(true);
			Isolate.WhenCalled(() => dummyInternalListener.GetContext()).WillReturn(expectedContext);
			Isolate.Swap.NextInstance<HttpListener>().With(dummyInternalListener);
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
