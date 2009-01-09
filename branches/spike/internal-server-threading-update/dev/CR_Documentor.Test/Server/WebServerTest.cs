using CR_Documentor.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;

namespace CR_Documentor.Test.Server
{
	[TestClass]
	public class WebServerTest
	{
		private const UInt16 TestServerPort = 22334;

		[TestMethod]
		public void AsyncListenThreadStart_EmptyContent()
		{
			this.ServerRequestTestBody("", "&nbsp;");
		}

		[TestMethod]
		public void AsyncListenThreadStart_NullContent()
		{
			this.ServerRequestTestBody(null, "&nbsp;");
		}

		[TestMethod]
		public void AsyncListenThreadStart_ServesContent()
		{
			this.ServerRequestTestBody("expected content", "expected content");
		}

		[TestMethod]
		public void Dispose_CallsStopIfStarted()
		{
			WebServerMock server = new WebServerMock(TestServerPort);
			server.Start();
			server.Dispose();
			Assert.IsTrue(server.StopCalled, "The Stop method should be called during disposal.");
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

		private class WebServerMock : WebServer
		{
			public WebServerMock(UInt16 port) : base(port) { }
			public bool StopCalled { get; set; }
			public override void Stop()
			{
				this.StopCalled = true;
				base.Stop();
			}
		}
	}
}
