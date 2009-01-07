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
		public void Content_Default()
		{
			WebServer server = new WebServer(TestServerPort);
			Assert.IsNull(server.Content, "The default content should be null.");
		}

		[TestMethod]
		public void Content_SetEmpty()
		{
			WebServer server = new WebServer(TestServerPort);
			string content = "";
			server.Content = content;
			Assert.AreEqual(content, server.Content, "The content should be settable to empty string.");
		}

		[TestMethod]
		public void Content_SetNull()
		{
			WebServer server = new WebServer(TestServerPort);
			string content = null;
			server.Content = content;
			Assert.AreEqual(content, server.Content, "The content should be settable to null.");
		}

		[TestMethod]
		public void Content_SetHtml()
		{
			WebServer server = new WebServer(TestServerPort);
			string content = "<html><head><title>Test</title></head><body><p>Test</p></body></html>";
			server.Content = content;
			Assert.AreEqual(content, server.Content, "The content should be settable to HTML.");
		}

		[TestMethod]
		public void Content_SetNonHtml()
		{
			WebServer server = new WebServer(TestServerPort);
			string content = "This is not HTML.";
			server.Content = content;
			Assert.AreEqual(content, server.Content, "The content should be settable to non-HTML.");
		}

		[TestMethod]
		public void Dispose_CallsStop()
		{
			WebServerMock server = new WebServerMock(TestServerPort);
			server.Dispose();
			Assert.IsTrue(server.StopCalled, "The Stop method should be called during disposal.");
		}

		[TestMethod]
		public void Port_Initialized()
		{
			WebServer server = new WebServer(TestServerPort);
			Assert.AreEqual(TestServerPort, server.Port, "The port value should be initialized by construction.");
		}

		[TestMethod]
		public void Prefix_CR_DocumentorIsSecondSegment()
		{
			WebServer server = new WebServer(TestServerPort);
			string[] segments = server.Url.Segments;
			// Get the path segment and trim the trailing slash
			string crdSegment = segments[1];
			crdSegment = crdSegment.Substring(0, crdSegment.Length - 1);
			Assert.AreEqual("CR_Documentor", crdSegment, "'CR_Documentor' should be at the base of the prefix, right after the root.");
		}

		private void ServerRequestTestBody(string initialContent, string expectedContent)
		{
			using (WebServer server = new WebServer(TestServerPort))
			{
				server.Content = initialContent;
				server.Start();
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
				server.Stop();
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
