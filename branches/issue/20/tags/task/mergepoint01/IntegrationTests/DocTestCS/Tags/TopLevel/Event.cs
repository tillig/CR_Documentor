using System;

namespace DocTestCS.Tags.TopLevel
{
	/// <summary>
	/// This class tests the 'event' tag.
	/// </summary>
	public class Event
	{
		/// <summary>
		/// This method has an event associated with it.
		/// </summary>
		/// <event cref="System.Data.DataSet.MergeFailed">
		/// This is a reference to the System.Data.DataSet.MergeFailed event.
		/// </event>
		public void Method()
		{
		}

		/// <summary>
		/// This method has an event associated with it.
		/// </summary>
		/// <event cref="System.Data.DataSet.MergeFailed">
		/// This is a reference to the System.Data.DataSet.MergeFailed event.
		/// </event>
		/// <event cref="System.ComponentModel.MarshalByValueComponent.Disposed">
		/// This is a reference to the System.ComponentModel.MarshalByValueComponent.Disposed event.
		/// </event>
		public void MethodMultiple()
		{
		}
	}
}
