using System;
using DevExpress.DXCore.Threading;
using TypeMock;

namespace CR_Documentor.Test.Diagnostics
{
	public class SynchronizationManagerMock
	{
		public static void Initialize()
		{
			// Set up calls to SynchronizationManager so they will pass in a unit
			// test environment.
			DynamicReturnValue beginInvoke = new DynamicReturnValue(SynchronizationManagerBeginInvoke);
			using (RecordExpectations recorder = RecorderManager.StartRecording())
			{
				IAsyncResult dummyResult = SynchronizationManager.BeginInvoke(null, null);
				recorder.Return(beginInvoke);
				recorder.RepeatAlways();
			}
		}

		// The SynchronizationManager static class in DXCore can't run outside a
		// DXCore environment. This DynamicReturnValue method can be used to swap
		// calls to the SynchronizationManager for unit testing purposes.
		//
		// Note that in our version we are blocking on the wait handle so the call
		// finishes. This doesn't happen in real SynchronizationManager calls, but
		// since we're trying to test the results of the call and we don't want
		// to Thread.Sleep in every test while we wait, we'll just make the call
		// synchronously.
		private delegate object SynchronizationManagerMethodCall(object[] parameters);

		public static IAsyncResult SynchronizationManagerBeginInvoke(object[] parameters, object context)
		{
			Delegate exec = parameters[0] as Delegate;
			SynchronizationManagerMethodCall call = delegate(object[] callParams)
			{
				if (callParams.Length == 0)
				{
					callParams = null;
				}
				return exec.DynamicInvoke(callParams);
			};
			object[] paramArray = parameters[1] as object[];
			IAsyncResult result = call.BeginInvoke(paramArray, null, null);
			result.AsyncWaitHandle.WaitOne();
			return result;
		}
	}
}
