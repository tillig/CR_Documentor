using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using CR_Documentor.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock.ArrangeActAssert;

namespace CR_Documentor.Test.Server
{
	[TestClass]
	[Isolated]
	public class ResponseWriterTest
	{
		private const string TextFilePath = "CR_Documentor.Test.Server.ResponseWriterTest.WriteResourceTextFile.txt";

		[TestMethod]
		public void WriteHtml_CommonResponseValues()
		{
			string content = "Test Content";
			byte[] buffer = Encoding.UTF8.GetBytes(content);
			int bufferLength = buffer.Length;

			ResponseContext responseContext = this.CreateMockContext();
			HttpListenerResponse response = responseContext.Response;
			Stream outputStream = responseContext.OutputStream;

			ResponseWriter.WriteHtml(responseContext.Context, content);

			Isolate.Verify.WasCalledWithExactArguments(() => response.ContentType = "text/html");
			Isolate.Verify.WasCalledWithExactArguments(() => response.ContentEncoding = Encoding.UTF8);
			Isolate.Verify.WasCalledWithExactArguments(() => response.StatusDescription = "OK");
			Isolate.Verify.WasCalledWithExactArguments(() => response.StatusCode = (int)HttpStatusCode.OK);
		}

		[TestMethod]
		public void WriteHtml_EmptyContent()
		{
			string expectedContent = "&nbsp;";
			byte[] buffer = Encoding.UTF8.GetBytes(expectedContent);
			int bufferLength = buffer.Length;

			ResponseContext responseContext = this.CreateMockContext();
			HttpListenerResponse response = responseContext.Response;
			Stream outputStream = responseContext.OutputStream;

			ResponseWriter.WriteHtml(responseContext.Context, "");

			Isolate.Verify.WasCalledWithExactArguments(() => response.ContentLength64 = bufferLength);
			Isolate.Verify.WasCalledWithExactArguments(() => outputStream.Write(buffer, 0, bufferLength));
		}

		[TestMethod]
		public void WriteHtml_NullContent()
		{
			string expectedContent = "&nbsp;";
			byte[] buffer = Encoding.UTF8.GetBytes(expectedContent);
			int bufferLength = buffer.Length;

			ResponseContext responseContext = this.CreateMockContext();
			HttpListenerResponse response = responseContext.Response;
			Stream outputStream = responseContext.OutputStream;

			ResponseWriter.WriteHtml(responseContext.Context, null);

			Isolate.Verify.WasCalledWithExactArguments(() => response.ContentLength64 = bufferLength);
			Isolate.Verify.WasCalledWithExactArguments(() => outputStream.Write(buffer, 0, bufferLength));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void WriteHtml_NullContext()
		{
			ResponseWriter.WriteHtml(null, "content");
		}

		[TestMethod]
		public void WriteHtml_ValidContent()
		{
			string expectedContent = "Lots of content here.";
			byte[] buffer = Encoding.UTF8.GetBytes(expectedContent);
			int bufferLength = buffer.Length;

			ResponseContext responseContext = this.CreateMockContext();
			HttpListenerResponse response = responseContext.Response;
			Stream outputStream = responseContext.OutputStream;

			ResponseWriter.WriteHtml(responseContext.Context, expectedContent);

			Isolate.Verify.WasCalledWithExactArguments(() => response.ContentLength64 = bufferLength);
			Isolate.Verify.WasCalledWithExactArguments(() => outputStream.Write(buffer, 0, bufferLength));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void WriteResource_EmptyContentType()
		{
			ResponseContext responseContext = this.CreateMockContext();
			ResponseWriter.WriteResource(responseContext.Context, Assembly.GetExecutingAssembly(), TextFilePath, "");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void WriteResource_EmptyResourcePath()
		{
			ResponseContext responseContext = this.CreateMockContext();
			ResponseWriter.WriteResource(responseContext.Context, Assembly.GetExecutingAssembly(), "", "text/plain");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void WriteResource_NullAssembly()
		{
			ResponseContext responseContext = this.CreateMockContext();
			ResponseWriter.WriteResource(responseContext.Context, null, TextFilePath, "text/plain");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void WriteResource_NullContentType()
		{
			ResponseContext responseContext = this.CreateMockContext();
			ResponseWriter.WriteResource(responseContext.Context, Assembly.GetExecutingAssembly(), TextFilePath, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void WriteResource_NullContext()
		{
			ResponseWriter.WriteResource(null, Assembly.GetExecutingAssembly(), TextFilePath, "text/plain");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void WriteResource_NullResourcePath()
		{
			ResponseContext responseContext = this.CreateMockContext();
			ResponseWriter.WriteResource(responseContext.Context, Assembly.GetExecutingAssembly(), null, "text/plain");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void WriteResource_ResourcePathNotInCurrentAssembly()
		{
			ResponseContext responseContext = this.CreateMockContext();
			ResponseWriter.WriteResource(responseContext.Context, Assembly.GetExecutingAssembly(), "notincurrentassembly", "text/plain");
		}

		[TestMethod]
		public void WriteResource_CommonResponseValues()
		{
			ResponseContext responseContext = this.CreateMockContext();
			HttpListenerResponse response = responseContext.Response;
			Stream outputStream = responseContext.OutputStream;

			ResponseWriter.WriteResource(responseContext.Context, Assembly.GetExecutingAssembly(), TextFilePath, "text/plain");

			Isolate.Verify.WasCalledWithExactArguments(() => response.ContentType = "text/plain");
			Isolate.Verify.WasNotCalled(() => response.ContentEncoding = Encoding.UTF8);
			Isolate.Verify.WasCalledWithExactArguments(() => response.StatusDescription = "OK");
			Isolate.Verify.WasCalledWithExactArguments(() => response.StatusCode = (int)HttpStatusCode.OK);
		}

		[TestMethod]
		public void WriteResource_ResponseContentFromResource()
		{
			int bufferLength = 18; // This is the file length of the embedded resource.

			ResponseContext responseContext = this.CreateMockContext();
			HttpListenerResponse response = responseContext.Response;
			var outputStream = responseContext.OutputStream;

			ResponseWriter.WriteResource(responseContext.Context, Assembly.GetExecutingAssembly(), TextFilePath, "text/plain");

			Isolate.Verify.WasCalledWithExactArguments(() => response.ContentLength64 = bufferLength);
			Isolate.Verify.WasCalledWithArguments(() => outputStream.Write(null, 0, 0)).Matching(
				args =>
				{
					return
					((byte[])args[0]).Length == bufferLength &&
					(int)args[1] == 0 &&
					(int)args[2] == bufferLength;
				}); ;
		}

		private ResponseContext CreateMockContext()
		{
			var context = Isolate.Fake.Instance<HttpListenerContext>();
			var response = Isolate.Fake.Instance<HttpListenerResponse>();
			var outputStream = Isolate.Fake.Instance<Stream>();
			Isolate.WhenCalled(() => context.Response).WillReturn(response);
			Isolate.WhenCalled(() => response.OutputStream).WillReturn(outputStream);
			return new ResponseContext()
			{
				Context = context,
				Response = response,
				OutputStream = outputStream
			};
		}

		private class ResponseContext
		{
			public HttpListenerContext Context { get; set; }
			public HttpListenerResponse Response { get; set; }
			public Stream OutputStream { get; set; }
		}
	}
}
