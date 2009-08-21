using System;

namespace DocTestCS.Tags.TopLevel
{
	/// <summary>
	/// This class tests the 'permission' tag.
	/// </summary>
	public class Permission
	{
		/// <summary>
		/// This is a method with a permission.
		/// </summary>
		/// <permission cref="System.Security.Permissions.EnvironmentPermission">
		/// This is a reference to the System.Security.Permissions.EnvironmentPermission permission.
		/// </permission>
		public void MethodSingle()
		{
		}

		/// <summary>
		/// This is a method with a permission.
		/// </summary>
		/// <permission cref="System.Security.Permissions.EnvironmentPermission">
		/// This is a reference to the System.Security.Permissions.EnvironmentPermission permission.
		/// </permission>
		/// <permission cref="System.Security.Permissions.FileDialogPermission">
		/// This is a reference to the System.Security.Permissions.FileDialogPermission permission.
		/// </permission>
		public void MethodMultiple()
		{
		}
	}
}
