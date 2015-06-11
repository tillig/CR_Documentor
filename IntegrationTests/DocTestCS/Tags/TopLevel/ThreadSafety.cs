using System;

namespace DocTestCS.Tags.TopLevel.ThreadSafety
{
	/// <summary>
	/// This class tests the 'threadsafety' tag (static=false, instance=true).
	/// </summary>
	/// <threadsafety static="false" instance="true" />
	public class ThreadSafetyInstance
	{
	}

	/// <summary>
	/// This class tests the 'threadsafety' tag (static=true, instance=true).
	/// </summary>
	/// <threadsafety static="true" instance="true" />
	public class ThreadSafetyInstanceStatic
	{
	}

	/// <summary>
	/// This class tests the 'threadsafety' tag (static=false, instance=false).
	/// </summary>
	/// <threadsafety static="false" instance="false" />
	public class ThreadSafetyNone
	{
	}

	/// <summary>
	/// This class tests the 'threadsafety' tag (static=true, instance=false).
	/// </summary>
	/// <threadsafety static="true" instance="false" />
	public class ThreadSafetyStatic
	{
	}

	/// <summary>
	/// This struct tests the 'threadsafety' tag (static=false, instance=false).
	/// </summary>
	/// <threadsafety static="false" instance="false" />
	public struct ThreadSafetyNoneStruct
	{
	}
}
