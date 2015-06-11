using System;
using System.Net;
using CR_Documentor.Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeMock;

namespace CR_Documentor.Test.Server
{
	[TestClass]
	[VerifyMocks]
	public class HttpRequestEventArgsTest
	{
		[TestMethod]
		public void Ctor_NonNullRequestContext()
		{
			HttpListenerContext context = RecorderManager.CreateMockedObject<HttpListenerContext>();
			HttpRequestEventArgs args = new HttpRequestEventArgs(context);
			Assert.AreSame(context, args.RequestContext, "The context should be populated by the constructor parameter.");
		}

		[TestMethod]
		public void Ctor_NullRequestContext()
		{
			HttpRequestEventArgs args = new HttpRequestEventArgs(null);
			Assert.IsNull(args.RequestContext, "The context should be null if the constructor is run with a null context parameter.");
		}
	}
}
