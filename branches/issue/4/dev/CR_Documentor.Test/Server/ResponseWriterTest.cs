using System;
using System.IO;
using System.Net;
using System.Text;
using CR_Documentor.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock;

namespace CR_Documentor.Test.Server
{
	[TestClass]
	[VerifyMocks]
	public class ResponseWriterTest
	{
		[TestMethod]
		public void WriteHtml_CommonResponseValues()
		{
			string content = "Test Content";
			byte[] buffer = Encoding.UTF8.GetBytes(content);
			int bufferLength = buffer.Length;

			ResponseContext responseContext = this.CreateMockContext();
			HttpListenerResponse response = responseContext.Response;
			Stream outputStream = responseContext.OutputStream;
			using (RecordExpectations recorder = RecorderManager.StartRecording())
			{
				response.StatusCode = (int)HttpStatusCode.OK;
				recorder.CheckArguments();
				response.StatusDescription = "OK";
				recorder.CheckArguments();
				response.ContentEncoding = Encoding.UTF8;
				recorder.CheckArguments();
				response.ContentType = "text/html";
				recorder.CheckArguments();

				outputStream.Write(buffer, 0, bufferLength);
				outputStream.Close();
			}

			// Running this will verify that the above expectations are fulfilled
			ResponseWriter.WriteHtml(responseContext.Context, content);
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
			using (RecordExpectations recorder = RecorderManager.StartRecording())
			{
				response.ContentLength64 = bufferLength;
				recorder.CheckArguments();

				response.ContentType = "";

				outputStream.Write(buffer, 0, bufferLength);
				recorder.CheckArguments();
			}

			// Running this will verify that the above expectations are fulfilled
			ResponseWriter.WriteHtml(responseContext.Context, "");
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
			using (RecordExpectations recorder = RecorderManager.StartRecording())
			{
				response.ContentLength64 = bufferLength;
				recorder.CheckArguments();

				response.ContentType = "";

				outputStream.Write(buffer, 0, bufferLength);
				recorder.CheckArguments();
			}

			// Running this will verify that the above expectations are fulfilled
			ResponseWriter.WriteHtml(responseContext.Context, null);
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
			using (RecordExpectations recorder = RecorderManager.StartRecording())
			{
				response.ContentLength64 = bufferLength;
				recorder.CheckArguments();

				response.ContentType = "";

				outputStream.Write(buffer, 0, bufferLength);
				recorder.CheckArguments();
			}

			// Running this will verify that the above expectations are fulfilled
			ResponseWriter.WriteHtml(responseContext.Context, expectedContent);
		}

		private ResponseContext CreateMockContext()
		{
			HttpListenerContext context = RecorderManager.CreateMockedObject<HttpListenerContext>();
			HttpListenerResponse response = RecorderManager.CreateMockedObject<HttpListenerResponse>();
			Stream outputStream = RecorderManager.CreateMockedObject<Stream>();

			using (RecordExpectations recorder = RecorderManager.StartRecording())
			{
				HttpListenerResponse dummyResponse = context.Response;
				recorder.Return(response);
				recorder.RepeatAlways();
				Stream dummyStream = response.OutputStream;
				recorder.Return(outputStream);
				recorder.RepeatAlways();

				// If we don't set up an expectation around Close, tests will
				// get exceptions if/when it runs.
				response.Close();
			}
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
