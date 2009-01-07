using System;
using CR_Documentor.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CR_Documentor.Test.Server
{
	[TestClass]
	public class WebListenerTest
	{
		private const UInt16 TestListenerPort = 22334;
		private static readonly Guid TestListenerUniqueId = Guid.NewGuid();

		[TestMethod]
		public void Dispose_CallsStopIfListening()
		{
			using (WebListenerMock server = new WebListenerMock(TestListenerPort, TestListenerUniqueId))
			{
				server.Start();
				server.Dispose();
				Assert.IsTrue(server.StopCalled, "The Stop method should be called during disposal if the listener had started.");
			}
		}

		[TestMethod]
		public void Dispose_DoesNotCallStopIfNotListening()
		{
			using (WebListenerMock server = new WebListenerMock(TestListenerPort, TestListenerUniqueId))
			{
				server.Dispose();
				Assert.IsFalse(server.StopCalled, "The Stop method should not be called during disposal if the listener wasn't listening.");
			}
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetContext_DoesNotCallStopIfNotListening()
		{
			using (WebListenerMock server = new WebListenerMock(TestListenerPort, TestListenerUniqueId))
			{
				server.GetContext();
			}
		}

		[TestMethod]
		public void IsListening_InitialValue()
		{
			using (WebListener server = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				Assert.IsFalse(server.IsListening, "The listener should not be listening until it's been started.");
			}
		}

		[TestMethod]
		public void IsListening_Started()
		{
			using (WebListener server = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				server.Start();
				Assert.IsTrue(server.IsListening, "The listener should be listening after it's been started.");
			}
		}

		[TestMethod]
		public void IsListening_Stopped()
		{
			using (WebListener server = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				server.Start();
				server.Stop();
				Assert.IsFalse(server.IsListening, "The listener should not be listening after it's been stopped.");
			}
		}

		[TestMethod]
		public void Prefix_Format()
		{
			using (WebListener server = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				string prefix = String.Format(WebListener.BaseUriFormat, TestListenerPort, TestListenerUniqueId);
				Assert.AreEqual(prefix, server.Prefix.AbsoluteUri, "The prefix should be properly formatted.");
			}
		}

		[TestMethod]
		public void Prefix_GuidIsThirdSegment()
		{
			using (WebListener server = new WebListener(TestListenerPort, TestListenerUniqueId))
			{
				string[] segments = server.Prefix.Segments;
				// Get the path segment and trim the trailing slash
				string guidSegment = segments[2];
				guidSegment = guidSegment.Substring(0, guidSegment.Length - 1);
				Guid guidId = new Guid(guidSegment);
				Assert.AreEqual(TestListenerUniqueId, guidId, "The GUID identifier segment should be the last segment and should match the server's unique ID.");
			}
		}

		private class WebListenerMock : WebListener
		{
			public WebListenerMock(UInt16 port, Guid uniqueId) : base(port, uniqueId) { }
			public bool StopCalled { get; set; }
			public override void Stop()
			{
				this.StopCalled = true;
				base.Stop();
			}
		}
	}
}
