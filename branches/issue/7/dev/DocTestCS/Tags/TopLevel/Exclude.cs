using System;

namespace DocTest.Tags.TopLevel
{
	/// <summary>
	/// This class tests the 'exclude' tag.
	/// </summary>
	/// <exclude />
	public class ExcludeClass
	{
		/// <summary>
		/// This is a method with an exclude.
		/// </summary>
		/// <exclude />
		public void Method()
		{
		}
	}
	/// <summary>
	/// This interface tests the 'exclude' tag.
	/// </summary>
	/// <exclude />
	public interface ExcludeInterface
	{
		/// <summary>
		/// This is a method inside an excluded interface.
		/// </summary>
		void Method();
	}
}
