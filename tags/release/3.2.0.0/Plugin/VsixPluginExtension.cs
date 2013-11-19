using System.ComponentModel.Composition;
using DevExpress.CodeRush.Common;

namespace CR_Documentor
{
	/// <summary>
	/// VSIX plugin extension class for CR_Documentor.
	/// </summary>
	[Export(typeof(IVsixPluginExtension))]
	public class VsixPluginExtension : IVsixPluginExtension
	{
	}
}
