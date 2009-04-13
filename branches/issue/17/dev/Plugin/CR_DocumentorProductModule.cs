using System;
using System.Reflection;
using DevExpress.CodeRush.Common;

namespace CR_Documentor
{
	/// <summary>
	/// Product module for CR_Documentor.  Allows participation in the "About" box for DXCore.
	/// </summary>
	public sealed class CR_DocumentorProductModule : ProductModule
	{

		/// <summary>
		/// Contains the assembly copyright information.
		/// </summary>
		private static string __copyright;

		/// <summary>
		/// Contains the assembly description.
		/// </summary>
		private static string __description;

		/// <summary>
		/// Contains the minimum supported version of DXCore that is required.
		/// </summary>
		private static DevExpress.CodeRush.Common.Version __minEngineVersion = new DevExpress.CodeRush.Common.Version(9, 1, 2, 0);

		/// <summary>
		/// Contains the current version of the assembly.
		/// </summary>
		private static DevExpress.CodeRush.Common.Version __version;

		/// <summary>
		/// Gets the first copyright line.
		/// </summary>
		/// <value>
		/// The copyright text from the assembly.
		/// </value>
		public override string Copyright1
		{
			get
			{
				return __copyright;
			}
		}

		/// <summary>
		/// Gets the description of CR_Documentor.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> with the description of CR_Documentor.
		/// </value>
		public override string Description
		{
			get
			{
				return __description;
			}
		}

		/// <summary>
		/// Gets the product ID of CR_Documentor.
		/// </summary>
		/// <value>
		/// A <see cref="System.Guid"/> with the product ID for CR_Documentor.
		/// </value>
		public override Guid ID
		{
			get
			{
				return new Guid("B9F61C10-8741-4aca-8833-7D9FF65F33FC");
			}
		}

		/// <summary>
		/// Gets the minimum required engine version.
		/// </summary>
		/// <value>
		/// A <see cref="DevExpress.CodeRush.Common.Version"/> with the minimum supported
		/// DXCore version.
		/// </value>
		public override DevExpress.CodeRush.Common.Version MinimumEngineVersion
		{
			get
			{
				return __minEngineVersion;
			}
		}

		/// <summary>
		/// Gets the type of product this is.
		/// </summary>
		/// <value>
		/// Always returns <see cref="DevExpress.CodeRush.Common.ModuleTypes.Free"/>.
		/// </value>
		public override ModuleTypes ModuleType
		{
			get
			{
				return ModuleTypes.Free;
			}
		}

		/// <summary>
		/// Gets the product name.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> with the name of the product (CR_Documentor).
		/// </value>
		public override string Name
		{
			get
			{
				return "CR_Documentor";
			}
		}

		/// <summary>
		/// Returns the product title.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> that will be used when displaying the full name
		/// of the product and version in the "About" window.
		/// </value>
		public override string Title
		{
			get
			{
				return "CR_Documentor XML Comment Viewer";
			}
		}

		/// <summary>
		/// Gets the URL that users can visit to get the latest updates.
		/// </summary>
		/// <value>
		/// A <see cref="System.String"/> with a link to the product site.
		/// </value>
		public override string UpdateURL
		{
			get
			{
				return "http://cr-documentor.googlecode.com";
			}
		}

		/// <summary>
		/// Gets the current assembly version.
		/// </summary>
		/// <value>
		/// A <see cref="DevExpress.CodeRush.Common.Version"/> with product version information.
		/// </value>
		public override DevExpress.CodeRush.Common.Version Version
		{
			get
			{
				return __version;
			}
		}

		/// <summary>
		/// Initializes <see langword="static" /> properties of the <see cref="CR_Documentor.CR_DocumentorProductModule" /> class.
		/// </summary>
		/// <seealso cref="CR_Documentor.CR_DocumentorProductModule" />
		static CR_DocumentorProductModule()
		{
			Assembly current = typeof(CR_DocumentorProductModule).Assembly;
			System.Version sysVersion = current.GetName().Version;
			__version = new DevExpress.CodeRush.Common.Version(sysVersion.Major, sysVersion.Minor, sysVersion.Build, sysVersion.Revision, ReleaseType.Release);
			AssemblyCopyrightAttribute[] copyrightAttributes = (AssemblyCopyrightAttribute[])current.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
			__copyright = copyrightAttributes[0].Copyright;
			AssemblyDescriptionAttribute[] descriptionAttributes = (AssemblyDescriptionAttribute[])current.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
			__description = descriptionAttributes[0].Description;
		}

		/// <summary>
		/// Builds the product module definition.
		/// </summary>
		protected override void BuildDefenition()
		{
		}

		/// <summary>
		/// Gets the image to display in the "About" box.
		/// </summary>
		/// <returns>A <see cref="System.Drawing.Image"/> with the product logo.</returns>
		public override System.Drawing.Image GetImage()
		{
			Assembly current = typeof(CR_DocumentorProductModule).Assembly;
			return new System.Drawing.Bitmap(current.GetManifestResourceStream("CR_Documentor.Resources.CR_DocumentorProductModule.bmp"));
		}
	}
}
