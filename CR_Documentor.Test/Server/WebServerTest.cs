using System;
using System.IO;
using System.Net;
using CR_Documentor.Server;
using DevExpress.DXCore.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Server
{
	[TestClass]
	[Isolated]
	[Ignore] // The web listener and server tests cause the VS test process to hang.
	public class WebServerTest
	{
		private const UInt16 TestServerPort = 22334;

		[TestInitialize]
		public void TestInitialize()
		{
			Isolate.WhenCalled(() => SynchronizationManager.BeginInvoke(null, null)).ReturnRecursiveFake();
		}

		[TestMethod]
		public void Dispose_CallsStopIfStarted()
		{
			WebServer server = new WebServer(TestServerPort);
			Isolate.WhenCalled(() => server.Stop()).IgnoreCall();
			server.Start();
			server.Dispose();
			Isolate.Verify.WasCalledWithExactArguments(() => server.Stop());
		}

		[TestMethod]
		public void Dispose_DoesNotCallStopIfNotStarted()
		{
			WebServer server = new WebServer(TestServerPort);
			Isolate.WhenCalled(() => server.Stop()).IgnoreCall();
			server.Dispose();
			Isolate.Verify.WasNotCalled(() => server.Stop());
		}

		[TestMethod]
		public void Port_Initialized()
		{
			using (WebServer server = new WebServer(TestServerPort))
			{
				Assert.AreEqual(TestServerPort, server.Port, "The port value should be initialized by construction.");
			}
		}

		[TestMethod]
		public void Prefix_CR_DocumentorIsSecondSegment()
		{
			using (WebServer server = new WebServer(TestServerPort))
			{
				string[] segments = server.Url.Segments;
				// Get the path segment and trim the trailing slash
				string crdSegment = segments[1];
				crdSegment = crdSegment.Substring(0, crdSegment.Length - 1);
				Assert.AreEqual("CR_Documentor", crdSegment, "'CR_Documentor' should be at the base of the prefix, right after the root.");
			}
		}

		[TestMethod]
		public void RunState_InitialValue()
		{
			using (WebServer server = new WebServer(TestServerPort))
			{
				Assert.AreEqual(WebServer.State.Stopped, server.RunState, "The initial run state of the server should be Stopped.");
			}
		}

		[TestMethod]
		public void RunState_Started()
		{
			using (WebServer server = new WebServer(TestServerPort))
			{
				server.Start();
				Assert.AreEqual(WebServer.State.Started, server.RunState, "The run state of the server should be Started once the service has started.");
			}
		}

		[TestMethod]
		public void RunState_Stopped()
		{
			using (WebServer server = new WebServer(TestServerPort))
			{
				server.Start();
				server.Stop();
				Assert.AreEqual(WebServer.State.Stopped, server.RunState, "The run state of the server should be Stopped once the service has stopped.");
			}
		}

		[TestMethod]
		public void UniqueId_Initialized()
		{
			using (WebServer server = new WebServer(TestServerPort))
			{
				Assert.AreNotEqual(Guid.Empty, server.UniqueId, "The unique ID value should be initialized by construction.");
			}
		}

		#region Integration (Full Round-Trip) Tests

		[TestMethod]
		public void Integration_EmptyContent()
		{
			this.ServerRequestTestBody("", "&nbsp;");
		}

		[TestMethod]
		public void Integration_NullContent()
		{
			this.ServerRequestTestBody(null, "&nbsp;");
		}

		[TestMethod]
		public void Integration_ServesContent()
		{
			this.ServerRequestTestBody("expected content", "expected content");
		}

		private void ServerRequestTestBody(string initialContent, string expectedContent)
		{
			using (WebServer server = new WebServer(TestServerPort))
			{
				server.Start();
				WebRequestController controller = new WebRequestController(initialContent);
				server.IncomingRequest += controller.RequestEventHandler;
				for (int i = 0; i < 3; i++)
				{
					WebRequest request = WebRequest.Create(String.Format(WebListener.BaseUriFormat, TestServerPort, server.UniqueId));
					WebResponse response = request.GetResponse();
					Stream responseStream = response.GetResponseStream();
					StreamReader reader = new StreamReader(responseStream);
					string responseContent = reader.ReadToEnd();
					Assert.AreEqual(expectedContent, responseContent, "The expected content was not returned by the server.");
					response.Close();
				}
				server.IncomingRequest -= controller.RequestEventHandler;
				server.Stop();
			}
		}

		private class WebRequestController
		{
			public string ContentToServe { get; private set; }
			public WebRequestController(string contentToServe)
			{
				this.ContentToServe = contentToServe;
			}
			public void RequestEventHandler(object sender, HttpRequestEventArgs e)
			{
				ResponseWriter.WriteHtml(e.RequestContext, this.ContentToServe);
			}
		}

		#endregion
	}
}
