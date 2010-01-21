using System;
using System.Collections.Generic;
using System.Text;

namespace CR_Documentor.Properties
{
	/// <summary>
	/// Constant values that relate to the product/plugin registration with DXCore.
	/// </summary>
	public class ProductConstants
	{
		/// <summary>
		/// Name of the plugin assembly for the product module to register.
		/// </summary>
		public const string PlugInAssemblyName = "CR_Documentor.dll";

		/// <summary>
		/// The name of the plugin assembly and the primary plugin contained therein.
		/// </summary>
		public const string PlugInName = "CR_Documentor";

		/// <summary>
		/// The ID of the product module that registers with the "About" box.
		/// </summary>
		public static readonly Guid ProductModuleId = new Guid("B9F61C10-8741-4aca-8833-7D9FF65F33FC");
	}
}
