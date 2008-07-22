using CR_Documentor.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.IO;

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
		public void IsListening_Default()
		{
			WebServer server = new WebServer(TestServerPort);
			Assert.IsFalse(server.IsListening, "The server should not be listening right after construction.");
		}

		[TestMethod]
		public void Prefixes_Default()
		{
			WebServer server = new WebServer(TestServerPort);
			Assert.IsNotNull(server.Prefixes, "The prefix collection should not be null.");
			Assert.IsTrue(server.Prefixes.Contains(String.Format("http://*:{0}/", TestServerPort)), "The prefix with the specified port should be in the collection.");
		}

		[TestMethod]
		public void Start_Stop_IsListening()
		{
			WebServer server = new WebServer(TestServerPort);
			server.Start();
			Assert.IsTrue(server.IsListening, "The server should be listening after it starts.");
			server.Stop();
			Assert.IsFalse(server.IsListening, "The server should not be listening after it stops.");
		}

		private void ServerRequestTestBody(string initialContent, string expectedContent)
		{
			using (WebServer server = new WebServer(TestServerPort))
			{
				server.Content = initialContent;
				server.Start();
				for (int i = 0; i < 3; i++)
				{
					WebRequest request = WebRequest.Create(String.Format("http://localhost:{0}/", TestServerPort));
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
